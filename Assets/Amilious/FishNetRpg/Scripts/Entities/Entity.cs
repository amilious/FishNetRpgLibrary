using System;
using FishNet.Object;
using System.Collections.Generic;

namespace Amilious.FishNetRpg.Entities {
    
    public class Entity : NetworkBehaviour {

        private readonly Dictionary<Type, ISystemManager> _managers = new();

        private void Awake() {
            foreach(var manager in GetComponents<ISystemManager>()) {
                _managers.Add(manager.SystemType,manager);
            }
        }

        public bool HasSystem<T>() where T : ISystemManager => _managers.ContainsKey(typeof(T));

        public T GetSystem<T>() where T : ISystemManager {
            var type = typeof(T);
            if(_managers.TryGetValue(type,out var manager)) return (T) manager;
            throw new MissingMemberException(string.Format(FishNetRpg.MISSING_SYSTEM_MANAGER, name, type.Name));
        }
        
    }
    
}
