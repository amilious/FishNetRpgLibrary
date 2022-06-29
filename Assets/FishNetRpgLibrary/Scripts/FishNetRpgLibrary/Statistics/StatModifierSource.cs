
using System.Collections.Generic;
using UnityEngine;

namespace FishNetRpgLibrary.Statistics {
    
    public class StatModifierSource {
        
        public IStatModifier Modifier { get; }
        
        public int SourceId { get; }
        
        public float AppliedTime { get; }

        public StatModifierSource(IStatModifier modifier, Object source) {
            Modifier = modifier;
            SourceId = source.GetInstanceID();
            AppliedTime = Time.realtimeSinceStartup;
        }

        public StatModifierSource(IStatModifier modifier, int sourceId) {
            Modifier = modifier;
            SourceId = sourceId;
            AppliedTime = Time.realtimeSinceStartup;
        }

        public bool HasSource(Object source) => SourceId == source.GetInstanceID();

        public bool HasSource(int sourceId) => SourceId == sourceId;

    }
    
}