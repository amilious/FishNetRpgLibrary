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

    }
}