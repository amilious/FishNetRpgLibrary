/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                                    //
//    _____            .__ .__   .__                             _________  __              .___.__                   //
//   /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /   _____/_/  |_  __ __   __| _/|__|  ____   ______   //
//  /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     \_____  \ \   __\|  |  \ / __ | |  | /  _ \ /  ___/   //
// /    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \      /        \ |  |  |  |  // /_/ | |  |(  <_> )\___ \    //
// \____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    /_______  / |__|  |____/ \____ | |__| \____//____  >   //
//         \/       \/                                  \/             \/                    \/                 \/    //
//                                                                                                                    //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious,com         Unity Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

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