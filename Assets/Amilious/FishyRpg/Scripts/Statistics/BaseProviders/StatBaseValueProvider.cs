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

using Amilious.Core;

namespace Amilious.FishyRpg.Statistics.BaseProviders {
    
    /// <summary>
    /// This is the base class for stat base value providers.
    /// </summary>
    public abstract class StatBaseValueProvider : AmiliousScriptableObject {

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the minimum value for the stat.
        /// </summary>
        /// <param name="level">The stat's level.</param>
        /// <returns>The minimum value for the stat.</returns>
        public abstract int GetMinimum(int level);

        /// <summary>
        /// This method is used to get the cap value for the stat.
        /// </summary>
        /// <param name="level">The stat's level.</param>
        /// <returns>The cap value used for the stat.</returns>
        public abstract int GetCap(int level);

        /// <summary>
        /// This method is used to get the base value of the stat for the given level.
        /// </summary>
        /// <param name="level">The stat's level.</param>
        /// <returns>The base value for the stat's level.</returns>
        public abstract int BaseValue(int level);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
}