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
//  Discord Server: https://discord.gg/SNqyDWu            CopyrightÂ© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Amilious.Core.Extensions;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace Amilious.FunctionGraph.Editor {
    
    /// <summary>
    /// This class is used to display the different types of function nodes.
    /// </summary>
    public class FunctionNodeSearchProvider : ScriptableObject, ISearchWindowProvider {

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field is used to cache the search options.
        /// </summary>
        private static readonly Dictionary<string, Type> OptionCache = new ();
        
        /// <summary>
        /// This field is used to hold the sorted options.
        /// </summary>
        private static readonly List<string> SortedOptions = new ();
        
        /// <summary>
        /// This field is used to hold the state of the search provider.
        /// </summary>
        private static bool _loaded;
        
        /// <summary>
        /// This field is used to hold the search list.
        /// </summary>
        private static readonly List<SearchTreeEntry> SearchList = new ();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the callback method.
        /// </summary>
        private Func<Type,Vector2,FunctionNodeView> Callback { get; set; }
        
        /// <summary>
        /// This property contains the position that the node should be created at.
        /// </summary>
        private Vector2 NodePosition { get; set; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                    
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to create a new search provider.
        /// </summary>
        public FunctionNodeSearchProvider() {
            if(_loaded) return;
            var types = TypeCache.GetTypesDerivedFrom<FunctionNode>()
                .Where(t => !t.IsAbstract&&!FunctionNode.GetAttribute(t).Hidden);
            foreach(var type in types) {
                var menu = type.SplitCamelCase();
                var test = type.BaseType;
                while(test != null && test != typeof(FunctionNode)) {
                    menu = test.SplitCamelCase() + "/" + menu;
                    test = test.BaseType;
                }
                OptionCache.Add(menu,type);
                SortedOptions.Add(menu);
            }
            
            SortedOptions.Sort((a, b) => {
                var splits1 = a.Split('/');
                var splits2 = b.Split('/');
                for(var i = 0; i < splits1.Length; i++) {
                    if(i >= splits2.Length) return 1;
                    var value = string.Compare(splits1[i], splits2[i], StringComparison.Ordinal);
                    if(value == 0) continue;
                    if(splits1.Length != splits2.Length && (i == splits1.Length - 1 || i == splits2.Length - 1))
                        return splits1.Length < splits2.Length ? 1 : -1;
                    return value;
                }
                return 0;
            });
            _loaded = true;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to update update the callback for the search provider.
        /// </summary>
        /// <param name="callback">The callback methods.</param>
        /// <param name="nodePosition">The position to send back with the callback.</param>
        /// <returns>The reference to this search provider.</returns>
        public FunctionNodeSearchProvider UpdateCallback(Func<Type, Vector2,FunctionNodeView> callback, 
            Vector2 nodePosition) {
            Callback = callback;
            NodePosition = nodePosition;
            return this;
        }
        
        /// <inheritdoc />
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context) {
            if(SearchList.Count>0) return SearchList;
            SearchList.Add(new SearchTreeGroupEntry(new GUIContent("Function Nodes"),0));
            var groups = new List<string>();
            foreach(var item in SortedOptions) {
                var entryTitle = item.Split('/');
                var groupName = "";
                for(var i = 0; i < entryTitle.Length - 1; i++) {
                    groupName += entryTitle[i];
                    if(!groups.Contains(groupName)) {
                        SearchList.Add(new SearchTreeGroupEntry(new GUIContent(entryTitle[i]),i+1));
                        groups.Add(groupName);
                    }
                    groupName += "/";
                }
                var tooltip = FunctionNode.GetAttribute(OptionCache[item])?.Description ?? "";
                var entry = new SearchTreeEntry(new GUIContent(entryTitle.Last(),tooltip)) {
                    level = entryTitle.Length,
                    userData = OptionCache[item]
                };
                SearchList.Add(entry);
            }
            return SearchList;
        }

        /// <inheritdoc />
        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context) {
            Callback?.Invoke((Type)searchTreeEntry.userData,NodePosition);
            return true;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}