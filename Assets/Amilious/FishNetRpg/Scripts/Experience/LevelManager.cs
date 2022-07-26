using System;
using Amilious.FishNetRpg.Entities;
using Amilious.FishNetRpg.Modifiers;
using FishNet.Object;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Amilious.FishNetRpg.Experience {
    
    [RequireComponent(typeof(Entity),typeof(ModifierManager))]
    [AddComponentMenu(FishNetRpg.COMPONENT_MANAGERS+"Level Manager")]
    public class LevelManager : NetworkBehaviour, ISystemManager {
        
        private Entity _entity;
        
        /// <inheritdoc />
        public Systems System => Systems.ResourceSystem;

        /// <inheritdoc />
        public Type SystemType => GetType();
        
        /// <inheritdoc />
        public Entity Entity {
            get {
                //get a reference to the entity if it does not exist.
                _entity ??= GetComponent<Entity>();
                return _entity; //return the entities reference.
            }
        }

        public bool ApplyModifier(Object source, IModifier modifier) {
            if(modifier.System != System) return false;
            throw new NotImplementedException();
        }

        public bool ApplyModifier(int sourceId, IModifier modifier) {
            if(modifier.System != System) return false;
            throw new NotImplementedException();
        }

        public bool RemoveModifier(Object source, IModifier modifier) {
            if(modifier.System != System) return false;
            throw new NotImplementedException();
        }

        public bool RemoveModifier(int sourceId, IModifier modifier) {
            if(modifier.System != System) return false;
            throw new NotImplementedException();
        }

        public void RemoveModifiersFromSource(Object source) {
            throw new NotImplementedException();
        }

        public void RemoveModifiersFromSource(int sourceId) {
            throw new NotImplementedException();
        }
    }
}