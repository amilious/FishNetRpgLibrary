using System;
using System.IO;
using UnityEditor;

namespace Amilious.FunctionGraph.Editor {
    
    public class FunctionGraphModificationProcessor : AssetModificationProcessor {

        private static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions opt) {
            var type = AssetDatabase.GetMainAssetTypeAtPath(path);
            if(!typeof(IFunctionProvider).IsAssignableFrom(type)) 
                return AssetDeleteResult.DidNotDelete;
            var guid = AssetDatabase.GUIDFromAssetPath(path).ToString();
            FunctionGraphEditor.AssetBeingDeleted(guid);
            return AssetDeleteResult.DidNotDelete;
        }

        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath) {
            var type = AssetDatabase.GetMainAssetTypeAtPath(sourcePath);
            if(!typeof(IFunctionProvider).IsAssignableFrom(type)) return AssetMoveResult.DidNotMove;
            var guid = AssetDatabase.GUIDFromAssetPath(sourcePath).ToString();
            var newName = Path.GetFileName(destinationPath);
            FunctionGraphEditor.AssetBeingRenamed(guid, newName);
            return AssetMoveResult.DidNotMove;
        }
    }
}