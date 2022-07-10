using System;

namespace Amilious.FunctionGraph {
    
    /// <summary>
    /// This class is used as a calculation identifier.
    /// </summary>
    [Serializable]
    public class CalculationId {

        #region Private Static Fields //////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field is used to hold the next id.
        /// </summary>
        private static long _idPool = 1;
        
        /// <summary>
        /// This field is a lock used to prevent any async issues.
        /// </summary>
        private static object _poolLock =  new ();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        public long Id { get; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get a new calculation id with the next available id.
        /// </summary>
        public CalculationId() {
            Id = GetNextId();
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the next id.
        /// </summary>
        /// <returns>The next id.</returns>
        private static long GetNextId() {
            //lock the pool while getting the next id.
            lock(_poolLock) { var id = _idPool; _idPool++; }
            //return the id.
            return _idPool;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}