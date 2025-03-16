/**
 * Securable Helper - Client-side helper for handling securable elements
 *
 * This script helps with handling securable elements that are dynamically generated
 * on the client-side. It works in conjunction with the server-side SecurableTagHelper.
 */

(function () {
  // Store user permissions in memory after fetching them once
  let userPermissions = null;
  let permissionsLoading = false;
  let permissionsCallbacks = [];
  let lastFetchTime = 0;
  const CACHE_DURATION = 60000; // 1 minute cache duration

  /**
   * Fetch user permissions from the server
   * @param {boolean} forceRefresh - Whether to force a refresh of permissions
   * @returns {Promise} Promise that resolves with user permissions
   */
  function fetchUserPermissions(forceRefresh = false) {
    const now = Date.now();

    // If we have permissions and they're not expired, and we're not forcing a refresh
    if (
      userPermissions &&
      !forceRefresh &&
      now - lastFetchTime < CACHE_DURATION
    ) {
      return Promise.resolve(userPermissions);
    }

    if (permissionsLoading) {
      return new Promise((resolve) => {
        permissionsCallbacks.push(resolve);
      });
    }

    permissionsLoading = true;

    return fetch("/api/Securables/GetUserPermissions")
      .then((response) => {
        if (!response.ok) {
          throw new Error(
            `Failed to fetch user permissions: ${response.status} ${response.statusText}`
          );
        }
        return response.json();
      })
      .then((permissions) => {
        userPermissions = permissions;
        permissionsLoading = false;
        lastFetchTime = Date.now();

        // Resolve any pending callbacks
        permissionsCallbacks.forEach((callback) => callback(userPermissions));
        permissionsCallbacks = [];

        return userPermissions;
      })
      .catch((error) => {
        console.error("Error fetching user permissions:", error);
        permissionsLoading = false;

        // Resolve callbacks with empty array on error
        permissionsCallbacks.forEach((callback) => callback([]));
        permissionsCallbacks = [];

        return [];
      });
  }

  /**
   * Check if the user has access to a specific securable
   * @param {string} securable - The securable to check
   * @returns {Promise<boolean>} Promise that resolves with true if the user has access, false otherwise
   */
  function hasAccess(securable) {
    return fetchUserPermissions().then((permissions) => {
      return permissions.includes(securable);
    });
  }

  /**
   * Process securable elements in the DOM
   * @param {HTMLElement|Document} container - The container element to process
   * @param {boolean} forceRefresh - Whether to force a refresh of permissions
   * @returns {Promise} Promise that resolves when all securable elements have been processed
   */
  function processSecurableElements(
    container = document,
    forceRefresh = false
  ) {
    const securableElements = container.querySelectorAll(
      "[data-securable], [securable]"
    );

    if (securableElements.length === 0) {
      return Promise.resolve();
    }

    return fetchUserPermissions(forceRefresh).then((permissions) => {
      securableElements.forEach((element) => {
        // Check both data-securable and securable attributes
        const securable =
          element.dataset.securable || element.getAttribute("securable");

        if (securable && !permissions.includes(securable)) {
          element.style.display = "none";
        } else if (
          securable &&
          permissions.includes(securable) &&
          element.style.display === "none"
        ) {
          // If the element was previously hidden but should now be shown
          element.style.display = "";
        }
      });
    });
  }

  /**
   * Refresh permissions and update all securable elements
   * @returns {Promise} Promise that resolves when all securable elements have been updated
   */
  function refreshPermissions() {
    // Clear cached permissions
    userPermissions = null;

    // Process all securable elements with a forced refresh
    return processSecurableElements(document, true);
  }

  // Expose public API
  window.SecurableHelper = {
    hasAccess,
    processSecurableElements,
    refreshPermissions,
  };

  // Process securable elements when the DOM is ready
  document.addEventListener("DOMContentLoaded", () => {
    processSecurableElements();
  });

  // Create a MutationObserver to process securable elements in dynamically added content
  const observer = new MutationObserver((mutations) => {
    mutations.forEach((mutation) => {
      if (mutation.type === "childList" && mutation.addedNodes.length > 0) {
        mutation.addedNodes.forEach((node) => {
          if (node.nodeType === Node.ELEMENT_NODE) {
            processSecurableElements(node);
          }
        });
      }
    });
  });

  // Start observing the document body for changes
  observer.observe(document.body, { childList: true, subtree: true });
})();
