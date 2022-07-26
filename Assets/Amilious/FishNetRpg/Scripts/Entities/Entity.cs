using System;
using FishNet.Object;
using System.Collections.Generic;
using Amilious.FishNetRpg.Modifiers;

namespace Amilious.FishNetRpg.Entities {
    
    /// <summary>
    /// This class is used as the base class for game entities.
    /// </summary>
    public class Entity : NetworkBehaviour {

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This dictionary is used to cache the managers.
        /// </summary>
        private readonly Dictionary<Type, ISystemManager> _managers = new();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is true if the entity has been initialized, otherwise false.
        /// </summary>
        public bool Initialized { get; private set; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Manager Methods ////////////////////////////////////////////////////////////////////////////////////////
                 
        /// <summary>
        /// This method is used to check if the entity has a manager of the given type.
        /// </summary>
        /// <typeparam name="T">The type of the manager.</typeparam>
        /// <returns>True if the entity contains a manager of the given type.</returns>
        public bool HasManager<T>() where T : ISystemManager {
            Initialize();
            return _managers.ContainsKey(typeof(T));
        }

        /// <summary>
        /// This method is used to check if the entity has a manager for the given system.
        /// </summary>
        /// <param name="system">The system type that you want to check for.</param>
        /// <returns>True if the entity contains a manager for the given system.</returns>
        public bool HasManager(Systems system) {
            Initialize();
            return _managers.ContainsKey(system.GetSystemType());
        }

        /// <summary>
        /// This method is used to try get the entity's manager of the given type.
        /// </summary>
        /// <typeparam name="T">The type of manager that you want to get.</typeparam>
        /// <returns>The entity's manager of the given type.</returns>
        /// <exception cref="MissingMemberException">Thrown if the entity does not have
        /// a manager of the given type.</exception>
        public T GetManager<T>() where T : ISystemManager {
            Initialize();
            var type = typeof(T);
            if(_managers.TryGetValue(type,out var manager)) return (T) manager;
            throw new MissingMemberException(string.Format(FishNetRpg.MISSING_SYSTEM_MANAGER, name, type.Name));
        }

        /// <summary>
        /// This method is used to try get the entity's manager of the given type.
        /// </summary>
        /// <param name="manager">The manager of the given type.</param>
        /// <typeparam name="T">The type of the manager.</typeparam>
        /// <returns>True if the entity contains a manager of the given type, otherwise false.</returns>
        public bool TryGetManager<T>(out T manager) where T : ISystemManager {
            Initialize();
            var type = typeof(T);
            manager = default;
            if(!_managers.TryGetValue(type, out var value)) return false;
            manager = (T) value;
            return true;
        }

        /// <summary>
        /// This method is used to try get the entity's manager of the given system.
        /// </summary>
        /// <param name="system">The system of the manager.</param>
        /// <param name="manager">The manager of the given system.</param>
        /// <returns>True if the entity contains a manager of the given system, otherwise false.</returns>
        public bool TryGetManager(Systems system, out ISystemManager manager) {
            Initialize();
            return TryGetManager(system.GetSystemType(), out manager);
        }

        /// <summary>
        /// This method is used to try get the entity's manager of the given type.
        /// </summary>
        /// <param name="type">The type of the manager.</param>
        /// <param name="manager">The manager of the given type.</param>
        /// <returns>True if the entity contains a manager of the given type, otherwise false.</returns>
        public bool TryGetManager(Type type, out ISystemManager manager) {
            Initialize();
            return _managers.TryGetValue(type, out manager);
        }

        /// <summary>
        /// This method is used to try get the entity's manager of the given modifier.
        /// </summary>
        /// <param name="modifier">The modifier of the manager.</param>
        /// <param name="manager">The manager of the given modifier.</param>
        /// <returns>True if the entity contains a manager of the given modifier, otherwise false.</returns>
        public bool TryGetManager(IModifier modifier, out ISystemManager manager) {
            manager = null;
            return modifier != null && TryGetManager(modifier.System.GetSystemType(), out manager);
        }

        /// <summary>
        /// This method can be called to register a manager that is added after the entity has been initialized.
        /// </summary>
        /// <param name="manager">The manager that you want to register to this entity.</param>
        public void RegisterManager(ISystemManager manager) {
            if(manager == null) return;
            var type = manager.GetType();
            if(_managers.ContainsKey(type)) return;
            _managers.Add(type, manager);
        }

        /// <summary>
        /// This method can be used to unregister a manager that is removed after the entity has been initialized.
        /// </summary>
        /// <param name="manager">The manager that you want to unregister from this entity.</param>
        public void UnregisterManager(ISystemManager manager) {
            if(manager == null) return;
            _managers.Remove(manager.GetType());
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Modifier Methods ///////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to add a modifier to the entity.  This method should only be called from the server.
        /// </summary>
        /// <param name="source">The source of the modifier.</param>
        /// <param name="modifier">The modifier.</param>
        [Server]
        public virtual void ApplyModifier(UnityEngine.Object source, IModifier modifier) {
            if(!TryGetManager(modifier, out var manager)) return;
            manager.ApplyModifier(source, modifier);
        }

        /// <summary>
        /// This method is used to add a modifier to the entity.  This method should only be called from the server.
        /// </summary>
        /// <param name="sourceId">The source of the modifier.</param>
        /// <param name="modifier">The modifier.</param>
        [Server]
        public virtual void ApplyModifier(int sourceId, IModifier modifier) {
            if(!TryGetManager(modifier, out var manager)) return;
            manager.ApplyModifier(sourceId, modifier);
        }
        
        /// <summary>
        /// This method is used to remove a modifier from the entity.  This method should only be called from the server.
        /// </summary>
        /// <param name="source">The source of the modifier.</param>
        /// <param name="modifier">The modifier that you want to remove from the entity.</param>
        [Server]
        public virtual void RemoveModifier(UnityEngine.Object source, IModifier modifier) {
            if(!TryGetManager(modifier, out var manager)) return;
            manager.RemoveModifier(source, modifier);
        }

        /// <summary>
        /// This method is used to remove a modifier from the entity.  This method should only be called from the server.
        /// </summary>
        /// <param name="sourceId">The source of the modifier.</param>
        /// <param name="modifier">The modifier that you want to remove from the entity.</param>
        [Server]
        public virtual void RemoveModifier(int sourceId, IModifier modifier) {
            if(!TryGetManager(modifier, out var manager)) return;
            manager.RemoveModifier(sourceId, modifier);
        }
        
        /// <summary>
        /// This method is used to add modifiers to the entity.  This method should only be called from the server.
        /// </summary>
        /// <param name="source">The source of the modifiers.</param>
        /// <param name="modifiers">The modifiers that you want to add to the entity.</param>
        [Server]
        public void ApplyModifiers(UnityEngine.Object source, IEnumerable<IModifier> modifiers) {
            foreach(var modifier in modifiers) ApplyModifier(source,modifier);
        }

        /// <summary>
        /// This method is used to add modifiers to the entity.  This method should only be called from the server.
        /// </summary>
        /// <param name="sourceId">The source of the modifiers.</param>
        /// <param name="modifiers">The modifiers that you want to add to the entity.</param>
        [Server]
        public void ApplyModifiers(int sourceId, IEnumerable<IModifier> modifiers) {
            foreach(var modifier in modifiers) ApplyModifier(sourceId,modifier);
        }

        /// <summary>
        /// This method is used to remove modifiers from the entity.  This method should only be called from the server.
        /// </summary>
        /// <param name="source">The source of the modifiers.</param>
        /// <param name="modifiers">The modifiers that you want to remove from the entity.</param>
        [Server]
        public void RemoveModifiers(UnityEngine.Object source, IEnumerable<IModifier> modifiers) {
            foreach(var modifier in modifiers) RemoveModifier(source,modifier);
        }

        /// <summary>
        /// This method is used to remove modifiers from the entity.  This method should only be called from the server.
        /// </summary>
        /// <param name="sourceId">The source of the modifiers.</param>
        /// <param name="modifiers">The modifiers that you want to remove from the entity.</param>
        [Server]
        public void RemoveModifiers(int sourceId, IEnumerable<IModifier> modifiers) {
            foreach(var modifier in modifiers) RemoveModifier(sourceId,modifier);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Unity Methods //////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is the first method called on the object.
        /// </summary>
        private void Awake() => Initialize();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to initialize the entity.
        /// </summary>
        protected virtual void Initialize() {
            if(Initialized) return;
            Initialized = true;
            //initialize manager
            foreach(var manager in GetComponents<ISystemManager>()) {
                _managers.Add(manager.SystemType,manager);
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}
