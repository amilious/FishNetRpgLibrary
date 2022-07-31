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

namespace Amilious.FishNetRpg.Statistics {
    
    /// <summary>
    /// This class is used to hold changeable stat information.
    /// </summary>
    public class StatData {
        
        #region Public Fields //////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field is used to hold a stat's level.
        /// </summary>
        public int Level;
        
        /// <summary>
        /// This field is used to hold a stat's base value.
        /// </summary>
        public int BaseValue;
        
        /// <summary>
        /// This field is used to hold the stat's value.
        /// </summary>
        public int Value;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This default constructor is used for serialization.
        /// </summary>
        public StatData() { }

        /// <summary>
        /// This constructor is used to create a new instance of a stat data.
        /// </summary>
        /// <param name="level">The stat's level.</param>
        /// <param name="baseValue">The stat's base value.</param>
        /// <param name="value">The stat's value.</param>
        public StatData(int level, int baseValue, int value) {
            Level = level;
            BaseValue = baseValue;
            Value = value;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}