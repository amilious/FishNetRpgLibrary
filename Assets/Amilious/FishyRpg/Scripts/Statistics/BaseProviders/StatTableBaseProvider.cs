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

using UnityEngine;

namespace Amilious.FishyRpg.Statistics.BaseProviders {
    
    /// <summary>
    /// This class is used to provide a table of stat base values based on level.
    /// </summary>
    [CreateAssetMenu(fileName = "NewTableBaseProvider",
        menuName = FishNetRpg.STATS_MENU_ROOT + "Table Base Value Provider", order = 45)]
    public class StatTableBaseProvider : StatBaseValueProvider {

        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField] private int cap;
        [SerializeField] private int minimum;
        [SerializeField] private int[] baseValues;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override int GetMinimum(int level) => minimum;

        /// <inheritdoc />
        public override int GetCap(int level) => cap;

        /// <inheritdoc />
        public override int BaseValue(int level) {
            if(level<=0) return 0;
            return level >= baseValues.Length ? baseValues[^1] : baseValues[level];
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}