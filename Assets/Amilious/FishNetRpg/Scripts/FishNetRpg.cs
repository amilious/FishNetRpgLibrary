namespace Amilious.FishNetRpg {
    
    /// <summary>
    /// This class is used to hold global information for the FishNetRpg library.
    /// </summary>
    public static class FishNetRpg {
        
        #region Menus //////////////////////////////////////////////////////////////////////////////////////////////////
    
        /// <summary>
        /// This value is the root path for this assembly in the asset menu.
        /// </summary>
        public const string ASSET_MENU_ROOT = "Amilious/";

        /// <summary>
        /// This value is the root path for the component menu.
        /// </summary>
        public const string COMPONENT_MENU_ROOT = "Amilious/FishNetRpg/";

        /// <summary>
        /// This value is the root path of the requirements asset menu.
        /// </summary>
        public const string REQUIREMENT_MENU_ROOT = ASSET_MENU_ROOT + "Requirements (FishNetRpg)/";

        /// <summary>
        /// This value is the root path of the stats asset menu.
        /// </summary>
        public const string STATS_MENU_ROOT = ASSET_MENU_ROOT + "Stats (FishNetRpg)/";

        /// <summary>
        /// This value is the root path of the resource asset menu.
        /// </summary>
        public const string RESOURCES_MENU_ROOT = ASSET_MENU_ROOT + "Resources (FishNetRpg)/";

        /// <summary>
        /// This value is the root path of the level and xp asset menu.
        /// </summary>
        public const string XP_MENU_ROOT = ASSET_MENU_ROOT + "Experience (FishNetRpg)/";

        /// <summary>
        /// This value is the root component menu for managers.
        /// </summary>
        public const string COMPONENT_MANAGERS = COMPONENT_MENU_ROOT + "Managers/";

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
        #region Warning Messages ///////////////////////////////////////////////////////////////////////////////////////
                  
        /// <summary>
        /// This warning is used by the StatManager when someone is trying to access a stat that is not registered.
        /// <param name="{0}">The name of the Entity to whom the stat manager belongs.</param>
        /// <param name="{1}">The name of the stat that was missing.</param>
        /// </summary>
        public const string MISSING_STAT = "The StatManager for \"{0}\" does not contain a stat with the name \"{1}\"!";

        /// <summary>
        /// This warning is used by the StatManager when trying to initialize a stat that already exists.
        /// <param name="{0}">The name of the Entity to whom the stat manager belongs.</param>
        /// <param name="{1}">The name of the stat.</param>
        /// </summary>
        public const string EXISTING_STAT = "The StatManager for \"{0}\" already contains a stat with the name \"{1}\"!";

        /// <summary>
        /// This warning is used by the Entity when trying to get a system that is not included on the Entity.
        /// <param name="{0}">The name of the Entity with the missing system manager.</param>
        /// <param name="{1}">The missing system manager's name.</param>
        /// </summary>
        public const string MISSING_SYSTEM_MANAGER = "The entity \"{0}\" does not contains the \"{1}\" system manager.";

        #endregion


    }
    
}
