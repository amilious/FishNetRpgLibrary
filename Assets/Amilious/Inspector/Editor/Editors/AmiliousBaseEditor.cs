using UnityEditor;
using UnityEngine;

namespace Amilious.Inspector.Editor.Editors {
    
    public abstract class AmiliousBaseEditor : UnityEditor.Editor {

        protected virtual string Youtube { get; } = null;
        protected virtual string Discord { get; } = null;
        protected virtual string AssetStore { get; } = null;
        protected virtual string Website { get; } = null;
        
        private static Texture _discordLogo;
        private static Texture _youtubeLogo;
        private static Texture _assetStoreLogo;
        private static Texture _websiteIcon;
        private GUIStyle _style;
        private GUIContent _assetStore;
        private GUIContent _website;
        private GUIContent _discord;
        private GUIContent _youtube;
        private bool _initialized;
        
        private void OnEnable() {
            _style = new GUIStyle { fixedHeight = 12, alignment = TextAnchor.MiddleLeft,
                fontSize = 10, fontStyle = FontStyle.Bold,
                //_style.fixedWidth = 85;
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0),
                normal = { textColor = Color.white }
            };
            _discordLogo ??= Resources.Load<Texture>("Icons/discord");
             _youtubeLogo ??= Resources.Load<Texture>("Icons/youtube");
            _assetStoreLogo ??=  EditorGUIUtility.IconContent("AssetStore Icon").image;
            _websiteIcon ??=  EditorGUIUtility.IconContent("BuildSettings.Web").image;
            _discord ??= new GUIContent("Discord", _discordLogo, "Click to go to the Amilious Console on the discord server.");
            _website ??= new GUIContent("Website", _websiteIcon, "Click to go to the Amilious Console on the amilious website.");
            _assetStore ??= new GUIContent("Asset Store", _assetStoreLogo, "Click to go to the Amilious Console on the unity asset store.");
            _youtube ??= new GUIContent("Youtube", _youtubeLogo, "Click to go to the Amilious Console on youtube.");
        }

        public override void OnInspectorGUI() {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            //buttons
            var options = new [] {GUILayout.ExpandWidth(false), GUILayout.MaxHeight(22) };
            if(Website!=null) if(GUILayout.Toggle(false, _website,_style, options)){ Application.OpenURL(Website);}
            if(AssetStore!=null)if(GUILayout.Toggle(false, _assetStore, _style, options)){ Application.OpenURL(AssetStore);}
            if(Discord!=null)if(GUILayout.Toggle(false,_discord,_style, options)){ Application.OpenURL(Discord);}
            if(Youtube!=null)if(GUILayout.Toggle(false,_youtube,_style, options)){ Application.OpenURL(Youtube);}
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            BeforeDefault();
            DrawPropertiesExcluding(serializedObject,"m_Script");
            AfterDefault();
            if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();
        }
        
        protected virtual void BeforeDefault(){}
        protected virtual void AfterDefault() {}

    }
}