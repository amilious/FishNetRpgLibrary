using System;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Amilious.Core.Editor {

    /// <summary>
    /// Defines the ODIN_INSPECTOR symbol.
    /// </summary>
    public static class DefineManager {

        private static readonly string[] DefineSymbols = { "AMILIOUS_CORE" };
        
        private static readonly StringBuilder StringBuilder = new StringBuilder();

        /// <summary>
        /// This method is used to make sure that the given define symbols are present.
        /// </summary>
        /// <param name="name">The name of the project the define symbols are for.</param>
        /// <param name="defineSymbols">The define symbols that you want to be present.</param>
        public static void AddDefinesIfNotPresent(string name, params string[] defineSymbols) {
            var currentTarget = EditorUserBuildSettings.selectedBuildTargetGroup;
            if(currentTarget == BuildTargetGroup.Unknown) return;
            var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentTarget).Trim();
            var defines = definesString.Split(';');
            var changed = false;
            foreach(var define in defineSymbols) {
                if(defines.Contains(define)) continue;
                if(!definesString.EndsWith(";", StringComparison.InvariantCulture)) definesString += ";";
                definesString += define;
                changed = true;
            }
            if(!changed) return;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(currentTarget, definesString);
            Debug.Log(AmiliousCore.MakeTitle($"Added {name} Define Symbols"));
        }

        /// <summary>
        /// This method is used to make sure that the give define symbols are not present.
        /// </summary>
        /// <param name="name">The name of the project the define symbols are for.</param>
        /// <param name="defineSymbols">The define symbols that you want to not be present.</param>
        public static void RemoveDefinesIfPresent(string name, params string[] defineSymbols) {
            var currentTarget = EditorUserBuildSettings.selectedBuildTargetGroup;
            if(currentTarget == BuildTargetGroup.Unknown) return;
            StringBuilder.Clear();
            var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentTarget).Trim();
            var defines = definesString.Split(';');
            var changed = false;
            foreach(var define in defines) {
                if(defineSymbols.Contains(define)) { changed = true; continue; }
                if(StringBuilder.Length > 0) StringBuilder.Append(';');
                StringBuilder.Append(define);
            }
            if(!changed) return;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(currentTarget, StringBuilder.ToString());
            Debug.Log(AmiliousCore.MakeTitle($"Removed {name} Define Symbols"));
        }

        /// <summary>
        /// This method is used to add the amilious core defines.
        /// </summary>
        [InitializeOnLoadMethod]
        private static void AddCoreDefineSymbols() {
            AddDefinesIfNotPresent("Amilious Core", DefineSymbols);
        }
        
    }
}
