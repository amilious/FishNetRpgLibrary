using System.IO;
using UnityEditor;

namespace Amilious.FunctionGraph.Editor {
    
    /// <summary>
    /// This class is used to watch for the deletion or renaming of a function provider.
    /// </summary>
    public class FunctionGraphModificationProcessor : AssetModificationProcessor {

        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to listen for assets that are being deleted.
        /// </summary>
        /// <param name="path">The path of the asset.</param>
        /// <param name="opt">The passed options.</param>
        /// <returns>The current state of the deletion.</returns>
        private static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions opt) {
            if(FunctionGraphEditor.Instance==null) return AssetDeleteResult.DidNotDelete;
            var type = AssetDatabase.GetMainAssetTypeAtPath(path);
            if(!typeof(IFunctionProvider).IsAssignableFrom(type)) 
                return AssetDeleteResult.DidNotDelete;
            var guid = AssetDatabase.GUIDFromAssetPath(path).ToString();
            FunctionGraphEditor.AssetBeingDeleted(guid);
            return AssetDeleteResult.DidNotDelete;
        }

        /// <summary>
        /// This method is sued to listen for assets that are being renamed.
        /// </summary>
        /// <param name="sourcePath">The current path of the asset.</param>
        /// <param name="destinationPath">The new path of the asset</param>
        /// <returns>The current state of the rename process.</returns>
        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath) {
            if(FunctionGraphEditor.Instance == null) return AssetMoveResult.DidNotMove;
            var type = AssetDatabase.GetMainAssetTypeAtPath(sourcePath);
            if(!typeof(IFunctionProvider).IsAssignableFrom(type)) return AssetMoveResult.DidNotMove;
            var guid = AssetDatabase.GUIDFromAssetPath(sourcePath).ToString();
            var newName = Path.GetFileName(destinationPath);
            FunctionGraphEditor.AssetBeingRenamed(guid, newName);
            return AssetMoveResult.DidNotMove;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
    }
}