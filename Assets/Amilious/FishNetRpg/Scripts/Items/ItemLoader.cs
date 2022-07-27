using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace Amilious.FishNetRpg.Items {
    public static class ItemLoader {

        public static Item LoadFromId(long itemId) {
            //later this method can choose the loading method
            return LoadResourceFromId(itemId); //load by resources
            //TODO: load by addressables
        }

        #region Load From Resource Folders /////////////////////////////////////////////////////////////////////////////

        private static bool _initializedResources = false;
        private static readonly Dictionary<long, Item> LoadedResourceItems = new();
        private static readonly Dictionary<long, string> CachedResourcePaths = new();
        
        private static Item LoadResourceFromId(long itemId) {
            InitializeResources();
            //return the loaded item if it still exists
            if(LoadedResourceItems.TryGetValue(itemId, out var item)) return item;
            //if the id is invalid return null
            if(!CachedResourcePaths.TryGetValue(itemId, out var path)) return null;
            //load the resource
            item = Resources.Load<Item>(path);
            //cache the loaded resource
            LoadedResourceItems.TryAdd(item.Id, item);
            //return the loaded item
            return item;
        }
        
        private static void InitializeResources() {
            if(_initializedResources) return;
            _initializedResources = true;
            var items = Resources.LoadAll<Item>(string.Empty) ?? Array.Empty<Item>();
            foreach(var item in items) {
                //check if there is a duplicate id
                if(items.Count(x => x.Id == item.Id) > 1) {
                    Debug.LogErrorFormat("Multiple items have been found with the id \"{0}\".",item.Id);
                }
                //keep loaded item if there are multiple resources with the same path
                if(items.Count(x => x.ResourcePath == item.ResourcePath) > 1) {
                    LoadedResourceItems.TryAdd(item.Id,item);
                    continue;
                }
                //if there is only one resource with the path cache the path and unload.
                CachedResourcePaths.TryAdd(item.Id,item.ResourcePath);
                Resources.UnloadAsset(item);
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}