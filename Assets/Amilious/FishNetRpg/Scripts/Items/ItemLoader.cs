/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
   _____            .__ .__   .__                             ____       ___________                __                  
  /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /  _ \      \__    ___/____  ___  ____/  |_  ____   ______ 
 /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     >  _ </\      |    | _/ __ \ \  \/  /\   __\/  _ \ /  ___/ 
/    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \     /  <_\ \/      |    | \  ___/  >    <  |  | (  <_> )\___ \  
\____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    \_____\ \      |____|  \___  >/__/\_ \ |__|  \____//____  > 
        \/       \/                                  \/            \/                  \/       \/                  \/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious,com         Unity Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious, Textos since 2022                      //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/          

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