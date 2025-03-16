/**
 * Global Spinner Management for EpicMarket
 * This script handles showing and hiding a central spinner during AJAX requests
 */

// Create the spinner element if it doesn't exist
function createSpinner() {
  if (!document.getElementById("global-spinner")) {
    const spinner = document.createElement("div");
    spinner.id = "global-spinner";
    spinner.innerHTML = `
            <div class="spinner-overlay"></div>
            <div class="spinner-container">
                <div class="spinner-border text-primary" role="status">
                    <span class="visually-hidden">Loading...</span>
                </div>
                <div class="spinner-text mt-2">Loading...</div>
            </div>
        `;
    document.body.appendChild(spinner);
  }
}

// Show the spinner
function showSpinner() {
  createSpinner();
  const spinner = document.getElementById("global-spinner");
  spinner.style.display = "block";
}

// Hide the spinner
function hideSpinner() {
  const spinner = document.getElementById("global-spinner");
  if (spinner) {
    spinner.style.display = "none";
  }
}

// Initialize AJAX interceptors to automatically show/hide spinner
document.addEventListener("DOMContentLoaded", function () {
  createSpinner();

  // Track active AJAX requests
  let activeRequests = 0;

  // Intercept all AJAX requests
  $(document).ajaxSend(function () {
    activeRequests++;
    showSpinner();
  });

  // Hide spinner when AJAX requests complete
  $(document).ajaxComplete(function () {
    activeRequests--;
    if (activeRequests <= 0) {
      activeRequests = 0;
      hideSpinner();
    }
  });

  // Handle AJAX errors
  $(document).ajaxError(function () {
    activeRequests--;
    if (activeRequests <= 0) {
      activeRequests = 0;
      hideSpinner();
    }
  });

  // Also handle fetch API requests
  const originalFetch = window.fetch;
  window.fetch = function () {
    showSpinner();
    return originalFetch
      .apply(this, arguments)
      .then((response) => {
        hideSpinner();
        return response;
      })
      .catch((error) => {
        hideSpinner();
        throw error;
      });
  };
});

// Manual control functions that can be called from anywhere in the application
window.SpinnerManager = {
  show: showSpinner,
  hide: hideSpinner,
};
