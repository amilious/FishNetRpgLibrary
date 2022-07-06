namespace Amilious.FishNetRpg.Modifiers {
    public interface IResourceModifier : IModifier {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the name of the resource that the modifier should be applied to.
        /// </summary>
        public string ResourceName { get; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}