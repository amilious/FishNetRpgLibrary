using UnityEngine;

namespace Amilious.Core {
    
    /// <summary>
    /// This is a <see cref="ScriptableObject"/> that stores it's own guid.
    /// </summary>
    public abstract class AmiliousScriptableObject : ScriptableObject, ISerializationCallbackReceiver {

        /// <summary>
        /// This serialized field is used to store the assets guid.
        /// </summary>
        [SerializeField, HideInInspector] private string guid;

        /// <summary>
        /// This serialized field is used to store the assets local file id.
        /// </summary>
        [SerializeField, HideInInspector] private long fileId;

        /// <summary>
        /// This property contains the assets guid from the asset database.
        /// </summary>
        public string AssetGuid => guid;

        /// <summary>
        /// This property contains the assets local file id from the asset database.
        /// </summary>
        public long AssetFileId => fileId;
        
        /// <inheritdoc />
        public void OnBeforeSerialize() {
            #if UNITY_EDITOR
            UnityEditor.AssetDatabase.TryGetGUIDAndLocalFileIdentifier(GetInstanceID(), out guid, out fileId);
            #endif
        }

        /// <inheritdoc />
        public void OnAfterDeserialize() { }
        
    }
}