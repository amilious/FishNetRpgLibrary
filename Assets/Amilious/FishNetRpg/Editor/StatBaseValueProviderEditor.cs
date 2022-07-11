using Amilious.FishNetRpg.Statistics.BaseProviders;
using Amilious.Inspector.Editor.Editors;
using UnityEditor;
using UnityEngine;

namespace Amilious.FishNetRpg.Editor {
    
    [CustomEditor(typeof(StatBaseValueProvider), true)]
    public class StatBaseValueProviderEditor : AmiliousBaseEditor{
        protected override void AfterDefault() {
            if(target is StatFunctionBaseProvider functionProvider) {
                if(GUILayout.Button("Open In Editor")) AssetDatabase.OpenAsset(target);
            }
            //TODO: add an example chart
        }
    }
    
}