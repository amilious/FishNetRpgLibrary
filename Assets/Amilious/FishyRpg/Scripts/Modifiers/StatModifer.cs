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

using Amilious.FishyRpg.Statistics;
using UnityEngine;

namespace Amilious.FishyRpg.Modifiers {
    
    /// <summary>
    /// This class is used to modify values in the stat's system.
    /// </summary>
    [CreateAssetMenu(fileName = "NewStatModifier", menuName = FishyRpg.MODIFIERS_MENU_ROOT + "Stat",
        order = FishyRpg.MODIFIERS_START+200)]
    public class StatModifer : Modifier, IStatModifier {
        
        #region Inspector Variables ////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField, Tooltip("The name of the stat that the modifier will be applied to.")] 
        private Stat stat;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Properties //////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc/>
        public string StatName => stat.StatName;

        /// <inheritdoc/>
        public Stat Stat => stat;
        
        /// <inheritdoc/>
        public override Systems System => Systems.StatsSystem;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}