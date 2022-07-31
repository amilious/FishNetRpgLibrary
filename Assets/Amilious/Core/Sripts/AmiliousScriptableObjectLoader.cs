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

using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace Amilious.Core {

    public static class AmiliousScriptableObjectLoader {

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to load an <see cref="AmiliousScriptableObject"/> by its id.
        /// </summary>
        /// <param name="id">The id of the <see cref="AmiliousScriptableObject"/> that you want to load.</param>
        /// <typeparam name="T">The type of the scriptable object.</typeparam>
        /// <returns>The loaded scriptable object if it exists, otherwise false.</returns>
        public static T LoadFromId<T>(long id) where T : AmiliousScriptableObject {
            var type = typeof(T);
            //later this method can choose the loading method
            return LoadResourceFromId<T>(id, type); //load by resources
            //TODO: load by addressables
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Load From Resource Folders Fields //////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This dictionary is used to keep track if a given type has been initialized.
        /// </summary>
        private static Dictionary<Type, bool> _initializedResources;

        /// <summary>
        /// This dictionary is used to keep track of the loaded AmiliousScriptableObjects.
        /// </summary>
        private static readonly Dictionary<Type, Dictionary<long, AmiliousScriptableObject>>
            LoadedResourceItems = new();

        /// <summary>
        /// This dictionary is used to keep track of the AmiliousScriptableObject paths.
        /// </summary>
        private static readonly Dictionary<Type, Dictionary<long, string>> CachedResourcePaths = new();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Load From Resource Folders Methods /////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get an asset from its id.
        /// </summary>
        /// <param name="assetId">The id of the asset that you want to get.</param>
        /// <param name="type">The type of asset that you want to get.</param>
        /// <typeparam name="T">The type of asset that you want to get.</typeparam>
        /// <returns>The asset with the given id if it exists, otherwise null.</returns>
        private static T LoadResourceFromId<T>(long assetId, Type type) where T : AmiliousScriptableObject {
            InitializeResources<T>(type);
            //return the loaded item if it still exists
            if(LoadedResourceItems[type].TryGetValue(assetId, out var item)) return item as T;
            //if the id is invalid return null
            if(!CachedResourcePaths[type].TryGetValue(assetId, out var path)) return null;
            //load the resource
            item = Resources.Load<AmiliousScriptableObject>(path);
            //cache the loaded resource
            LoadedResourceItems[type].TryAdd(item.Id, item);
            //return the loaded item
            return item as T;
        }

        /// <summary>
        /// This method is used to initialize the assets for the given type.
        /// </summary>
        /// <param name="type">The type of the asset that you want to initialize.</param>
        /// <typeparam name="T">The type of the asset that you want to initialize.</typeparam>
        private static void InitializeResources<T>(Type type) where T : AmiliousScriptableObject {
            if(_initializedResources.TryGetValue(type, out var result) && result) return;
            _initializedResources[type] = true;
            LoadedResourceItems.TryAdd(type, new Dictionary<long, AmiliousScriptableObject>());
            CachedResourcePaths.TryAdd(type, new Dictionary<long, string>());
            var assets = Resources.LoadAll<T>(string.Empty) ?? Array.Empty<T>();
            foreach(var asset in assets) {
                //check if there is a duplicate id
                if(assets.Count(x => x.Id == asset.Id) > 1) {
                    Debug.LogErrorFormat("Multiple items have been found with the id \"{0}\".", asset.Id);
                }
                //keep loaded item if there are multiple resources with the same path
                if(assets.Count(x => x.ResourcePath == asset.ResourcePath) > 1) {
                    LoadedResourceItems[type].TryAdd(asset.Id, asset);
                    continue;
                }
                //if there is only one resource with the path cache the path and unload.
                CachedResourcePaths[type].TryAdd(asset.Id, asset.ResourcePath);
                Resources.UnloadAsset(asset);
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}