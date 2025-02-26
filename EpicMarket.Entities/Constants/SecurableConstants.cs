using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EpicMarket.Entities.Constants
{
    public static class SecurableConstants
    {
        
        // More specific constants for ApplicationsTables
        public const string ApplicationTablesView = "ApplicationTablesView";
        public const string ApplicationTablesAdd = "ApplicationTablesAdd";
        public const string ApplicationTablesEdit = "ApplicationTablesEdit";
        public const string ApplicationTablesDelete = "ApplicationTablesDelete";
        
        // Constants for AccessTypes
        public const string AccessTypesView = "AccessTypesView";
        public const string AccessTypesAdd = "AccessTypesAdd";
        public const string AccessTypesEdit = "AccessTypesEdit";
        public const string AccessTypesDelete = "AccessTypesDelete";

        // Constants for ApplicationConfigurations
        public const string ApplicationConfigurationsView = "ApplicationConfigurationsView";
        public const string ApplicationConfigurationsAdd = "ApplicationConfigurationsAdd";
        public const string ApplicationConfigurationsEdit = "ApplicationConfigurationsEdit";
        public const string ApplicationConfigurationsDelete = "ApplicationConfigurationsDelete";

        // Constants for ApplicationSecurables
        public const string ApplicationSecurablesRead = "ApplicationSecurablesRead";
        public const string ApplicationSecurablesWrite = "ApplicationSecurablesWrite";

        // Constants for AccessControlLists
        public const string AccessControlListsView = "AccessControlListsView";
        public const string AccessControlListsAdd = "AccessControlListsAdd";
        public const string AccessControlListsEdit = "AccessControlListsEdit";
        public const string AccessControlListsDelete = "AccessControlListsDelete";

        // Constants for AppUsers
        public const string AppUsersView = "AppUsersView";
        public const string AppUsersAdd = "AppUsersAdd";
        public const string AppUsersEdit = "AppUsersEdit";
        public const string AppUsersDelete = "AppUsersDelete";
    }
}