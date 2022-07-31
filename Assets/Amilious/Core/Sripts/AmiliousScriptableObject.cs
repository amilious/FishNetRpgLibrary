////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                                    //
//    _____            .__ .__   .__                             _________  __              .___.__                   //
//   /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /   _____/_/  |_  __ __   __| _/|__|  ____   ______   //
//  /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     \_____  \ \   __\|  |  \ / __ | |  | /  _ \ /  ___/   //
// /    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \      /        \ |  |  |  |  // /_/ | |  |(  <_> )\___ \    //
// \____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    /_______  / |__|  |____/ \____ | |__| \____//____  >   //
//         \/       \/                                  \/             \/                    \/                 \/    //
//                                                                                                                    //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious,comUnity          Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;
using Amilious.Core.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Amilious.Core {
    
    /// <summary>
    /// This is a <see cref="ScriptableObject"/> that stores it's own guid.
    /// </summary>
    public abstract class AmiliousScriptableObject : ScriptableObject, ISerializationCallbackReceiver {

        /// <summary>
        /// This serialized field is used to store the assets guid.
        /// </summary>
        [SerializeField, HideInInspector] private long id = 0;

        [SerializeField, HideInInspector] private string resourcePath;

        [SerializeField, HideInInspector] private bool isInResourceFolder;

        /// <summary>
        /// This property contains the assets guid from the asset database.
        /// </summary>
        public long Id => id;

        public string ResourcePath => resourcePath;

        public bool IsInResourceFolder => isInResourceFolder;

        public virtual bool NeedsToBeLoadableById => false;
        
        /// <inheritdoc />
        void  ISerializationCallbackReceiver.OnBeforeSerialize() {
            #if UNITY_EDITOR
            if(id==0) { GenerateId(); }
            var pathParts = UnityEditor.AssetDatabase.GetAssetPath(this).Split("Resources/");
            var isResource = pathParts.Length>1;    
            var path = pathParts.Last().Replace(".asset",string.Empty);
            if(resourcePath == null || resourcePath != path) {
                resourcePath = path;
                isInResourceFolder = isResource;
            }
            #endif
            BeforeSerialize();
        }

        #if UNITY_EDITOR
        
        /// <summary>
        /// This method is used to generate the 
        /// </summary>
        /// <returns></returns>
        [ContextMenu("Regenerate Id")]
        private long GenerateId() {
            var oldId = id;
            id = GetNewId();
            var path = UnityEditor.AssetDatabase.GetAssetPath(this)??name;
            if(string.IsNullOrWhiteSpace(path)) path = GetType().SplitCamelCase();
            if(oldId==0) Debug.LogFormat("{0}\n<color=#8888ff>Generated id for:</color>\t\t<color=#ff88ff><b>{1}</b></color>\n<color=#8888ff>Id:</color>\t\t\t<color=#88ff88>{2}</color>",AmiliousCore.MakeTitle("Generating Amilious Scriptable Object Id"), path,id);
            else Debug.LogFormat("{0}\n<color=#8888ff>Regenerated id for:</color>\t<color=#ff88ff><b>{1}</b></color>\n<color=#8888ff>New Id:</color>\t\t\t<color=#88ff88>{2}</color>\n<color=#8888ff>Old Id:</color>\t\t\t<color=#ff8888>{3}</color>", AmiliousCore.MakeTitle("Generating Amilious Scriptable Object Id"), path,id,oldId);
            return id;
        }

        private static void Initialize() {
            if(_cachedIds != null) return;
            _cachedIds = new List<long>();
            foreach(var path in UnityEditor.AssetDatabase.GetAllAssetPaths()) {
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<AmiliousScriptableObject>(path);
                if(asset==null) continue;
                _cachedIds.Add(asset.Id);
            }
        }
        
        private static List<long> _cachedIds;
        
        private static long GetNewId() {
            Initialize();
            var id = DateTime.UtcNow.ToFileTime();
            while(_cachedIds.Contains(id)) id++;
            _cachedIds.Add(id);
            return id;
        }

        [UnityEditor.MenuItem("Amilious/Amilious Scriptable Objects/Fix Duplicate Ids")]
        private static void FixDuplicateIds() {
            _cachedIds ??= new List<long>();
            _cachedIds.Clear();
            var fixedIds = 0;
            foreach(var path in UnityEditor.AssetDatabase.GetAllAssetPaths()) {
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<AmiliousScriptableObject>(path);
                if(asset==null) continue;
                if(_cachedIds.Contains(asset.id)) { asset.GenerateId();
                    fixedIds++;
                }
                _cachedIds.Add(asset.Id);
            }
            Debug.LogFormat("{0}\n<color=#88ff88>Unique Objects:</color>\t\t<color=#ff88ff><b>{1}</b></color>\t<color=#8888ff>Fixed Ids:</color>\t<color=#ff8888>{2}</color>",AmiliousCore.MakeTitle("Fixed Amilious Scriptable Object Ids"), _cachedIds.Count,fixedIds);
        }
        
        #endif

        /// <inheritdoc />
        void ISerializationCallbackReceiver.OnAfterDeserialize() { AfterDeserialize(); }
        
        protected virtual void AfterDeserialize(){}
        protected virtual void BeforeSerialize(){}
        
    }
}