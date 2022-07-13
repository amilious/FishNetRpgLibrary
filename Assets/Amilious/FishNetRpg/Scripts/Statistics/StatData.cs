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