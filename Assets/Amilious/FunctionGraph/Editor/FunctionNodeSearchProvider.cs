using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using Amilious.FunctionGraph.Nodes.Hidden;

namespace Amilious.FunctionGraph.Editor {
    
    public class FunctionNodeSearchProvider : ScriptableObject, ISearchWindowProvider {

        private static readonly Dictionary<string, Type> OptionCache = new Dictionary<string, Type>();
        private static readonly List<string> SortedOptions = new List<string>();
        private static bool _loaded;
        private static readonly List<SearchTreeEntry> SearchList = new List<SearchTreeEntry>();
        
        private Action<Type,Vector2> Callback { get; set; }
        private Vector2 NodePosition { get; set; }

        public FunctionNodeSearchProvider() {
            if(_loaded) return;
            var types = TypeCache.GetTypesDerivedFrom<FunctionNode>()
                .Where(t => !t.IsAbstract&&!typeof(HiddenNode).IsAssignableFrom(t));
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
        
        public FunctionNodeSearchProvider AddCallback(Action<Type, Vector2> callback, Vector2 nodePosition) {
            Callback = callback;
            NodePosition = nodePosition;
            return this;
        }
        
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

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context) {
            Callback?.Invoke((Type)searchTreeEntry.userData,NodePosition);
            return true;
        }
        
    }
    
}