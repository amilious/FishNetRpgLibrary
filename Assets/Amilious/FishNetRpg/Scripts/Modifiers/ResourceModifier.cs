using UnityEngine;

namespace Amilious.FishNetRpg.Modifiers {
    
    /// <summary>
    /// This class is used to modify values in the resource's system.
    /// </summary>
    [CreateAssetMenu(fileName = "NewResourceModifier", menuName = FishNetRpg.MODIFIERS_MENU_ROOT + "Resource")]
    public class ResourceModifier : Modifier, IResourceModifier {
        
        #region Inspector Variables ////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField, Tooltip("The name of the stat that the modifier will be applied to.")] 
        private string resourceName;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Properties //////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc/>
        public string ResourceName => resourceName;
        
        /// <inheritdoc/>
        public override Systems System => Systems.ResourceSystem;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}