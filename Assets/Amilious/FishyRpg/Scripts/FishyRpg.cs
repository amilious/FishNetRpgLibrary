/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
   _____            .__ .__   .__                             ____       ___________                __                  
  /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /  _ \      \__    ___/____  ___  ____/  |_  ____   ______ 
 /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     >  _ </\      |    | _/ __ \ \  \/  /\   __\/  _ \ /  ___/ 
/    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \     /  <_\ \/      |    | \  ___/  >    <  |  | (  <_> )\___ \  
\____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    \_____\ \      |____|  \___  >/__/\_ \ |__|  \____//____  > 
        \/       \/                                  \/            \/                  \/       \/                  \/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious,com         Unity Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious, Textos since 2022                      //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/          

namespace Amilious.FishyRpg {
    
    /// <summary>
    /// This class is used to hold global information for the FishNetRpg library.
    /// </summary>
    public static class FishyRpg {
        
        #region Menus //////////////////////////////////////////////////////////////////////////////////////////////////
    
        /// <summary>
        /// This value is the root path for this assembly in the asset menu.
        /// </summary>
        public const string ASSET_MENU_ROOT = "Amilious/";

        public const string PROJECT_NAME = "FishNetRpg";

        /// <summary>
        /// This value is the root path for the component menu.
        /// </summary>
        public const string COMPONENT_MENU_ROOT = "Amilious/"+PROJECT_NAME+"/";

        /// <summary>
        /// This value is the root path of the requirements asset menu.
        /// </summary>
        public const string REQUIREMENT_MENU_ROOT = ASSET_MENU_ROOT + "Requirements ("+PROJECT_NAME+")/";

        /// <summary>
        /// This value is the root path of the stats asset menu.
        /// </summary>
        public const string STATS_MENU_ROOT = ASSET_MENU_ROOT + "Stats ("+PROJECT_NAME+")/";

        /// <summary>
        /// This value is the root path of the resource asset menu.
        /// </summary>
        public const string MODIFIERS_MENU_ROOT = ASSET_MENU_ROOT + "Modifiers ("+PROJECT_NAME+")/";
        
        /// <summary>
        /// This value is the root path of the resource asset menu.
        /// </summary>
        public const string QUEST_MENU_ROOT = ASSET_MENU_ROOT + "Quests ("+PROJECT_NAME+")/";

        /// <summary>
        /// This value is the root path of the resource asset menu.
        /// </summary>
        public const string RESOURCES_MENU_ROOT = ASSET_MENU_ROOT + "Resources ("+PROJECT_NAME+")/";

        /// <summary>
        /// This value is the root path of the level and xp asset menu.
        /// </summary>
        public const string XP_MENU_ROOT = ASSET_MENU_ROOT + "Experience ("+PROJECT_NAME+")/";
        
        /// <summary>
        /// This value is the root path of the inventory asset menu.
        /// </summary>
        public const string INVENTORY_MENU_ROOT = ASSET_MENU_ROOT + "Inventory ("+PROJECT_NAME+")/";

        public const string ITEM_MENU_ROOT = ASSET_MENU_ROOT + "Items ("+PROJECT_NAME+")/";

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

        #region Default Values /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This is the default description.
        /// </summary>
        public const string DEFAULT_DESCRIPTION = "Do description!";

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}
