/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
   _____            .__ .__   .__                             ____       ___________                __                  
  /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /  _ \      \__    ___/____  ___  ____/  |_  ____   ______ 
 /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     >  _ </\      |    | _/ __ \ \  \/  /\   __\/  _ \ /  ___/ 
/    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \     /  <_\ \/      |    | \  ___/  >    <  |  | (  <_> )\___ \  
\____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    \_____\ \      |____|  \___  >/__/\_ \ |__|  \____//____  > 
        \/       \/                                  \/            \/                  \/       \/                  \/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious,com         Unity Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            CopyrightÂ© Amilious, Textos since 2022                      //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using System;
using UnityEngine;
using Amilious.Core.Attributes;
using System.Collections.Generic;
using Amilious.FishyRpg.Modifiers;

namespace Amilious.FishyRpg.Entities {
    
    /// <summary>
    /// This class is used as the base class for game entities.
    /// </summary>
    public abstract class Entity : AmiliousNetworkBehavior {

        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField, AmiliousTab("Entity"), Tooltip("The type of the entity.")] 
        private EntityType entityType;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This dictionary is used to cache the managers.
        /// </summary>
        private readonly Dictionary<Type, ISystemManager> _managers = new();

        /// <summary>
        /// This value is used to store if the entity is alive or dead.
        /// </summary>
        private bool _alive = true;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Delegates //////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This delegate is used for the <see cref="Entity.OnEntityDied"/> event.
        /// </summary>
        public delegate void OnEntityDiedDelegate(Entity killed, Entity killer, string deathMessage);

        /// <summary>
        /// This delegate is used for the <see cref="Entity.OnEntityRevived"/> event.
        /// </summary>
        public delegate void OnEntityRevivedDelegate(Entity revived, Entity reviver, string reviveMessage);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This event is triggered when an entity dies.
        /// </summary>
        public static OnEntityDiedDelegate OnEntityDied;
        
        /// <summary>
        /// This event is triggered when an entity is revived.
        /// </summary>
        public static OnEntityRevivedDelegate OnEntityRevived;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is true if the entity has been initialized, otherwise false.
        /// </summary>
        public bool Initialized { get; private set; }

        /// <summary>
        /// This method is used to check if an entity is the given player or in the player's party.
        /// </summary>
        /// <param name="player">The player that you want to check.</param>
        /// <param name="includeFollowers">If true party followers will be counted as party members.</param>
        /// <returns>True if this entity is the given player or one of its party members.</returns>
        public bool IsPlayerOrParty(Player player, bool includeFollowers = true) {
            if(player == null) return false;
            if(player.ObjectId == ObjectId) return true;
            return player.Party != null && player.Party.Contains(this, includeFollowers);
        }

        /// <summary>
        /// This property contains the entity's entity type.
        /// </summary>
        public virtual EntityType EntityType => entityType;

        /// <summary>
        /// This property is used to check if the entity 
        /// </summary>
        public abstract bool IsLivableEntity { get; }

        /// <summary>
        /// This property is true if the player is a liveable entity and is alive.
        /// </summary>
        public bool IsAlive => !IsLivableEntity && _alive;

        /// <summary>
        /// This property is false if the player is a liveable entity and is alive.
        /// </summary>
        public bool IsDead => !IsLivableEntity && !_alive;
        
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
            throw new MissingMemberException(string.Format(FishyRpg.MISSING_SYSTEM_MANAGER, name, type.Name));
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
        public virtual void ApplyModifier(UnityEngine.Object source, IModifier modifier) {
            if(!TryGetManager(modifier, out var manager)) return;
            manager.ApplyModifier(source, modifier);
        }

        /// <summary>
        /// This method is used to add a modifier to the entity.  This method should only be called from the server.
        /// </summary>
        /// <param name="sourceId">The source of the modifier.</param>
        /// <param name="modifier">The modifier.</param>
        public virtual void ApplyModifier(int sourceId, IModifier modifier) {
            if(!TryGetManager(modifier, out var manager)) return;
            manager.ApplyModifier(sourceId, modifier);
        }
        
        /// <summary>
        /// This method is used to remove a modifier from the entity.  This method should only be called from the server.
        /// </summary>
        /// <param name="source">The source of the modifier.</param>
        /// <param name="modifier">The modifier that you want to remove from the entity.</param>
        public virtual void RemoveModifier(UnityEngine.Object source, IModifier modifier) {
            if(!TryGetManager(modifier, out var manager)) return;
            manager.RemoveModifier(source, modifier);
        }

        /// <summary>
        /// This method is used to remove a modifier from the entity.  This method should only be called from the server.
        /// </summary>
        /// <param name="sourceId">The source of the modifier.</param>
        /// <param name="modifier">The modifier that you want to remove from the entity.</param>
        public virtual void RemoveModifier(int sourceId, IModifier modifier) {
            if(!TryGetManager(modifier, out var manager)) return;
            manager.RemoveModifier(sourceId, modifier);
        }
        
        /// <summary>
        /// This method is used to add modifiers to the entity.  This method should only be called from the server.
        /// </summary>
        /// <param name="source">The source of the modifiers.</param>
        /// <param name="modifiers">The modifiers that you want to add to the entity.</param>
        public void ApplyModifiers(UnityEngine.Object source, IEnumerable<IModifier> modifiers) {
            foreach(var modifier in modifiers) ApplyModifier(source,modifier);
        }

        /// <summary>
        /// This method is used to add modifiers to the entity.  This method should only be called from the server.
        /// </summary>
        /// <param name="sourceId">The source of the modifiers.</param>
        /// <param name="modifiers">The modifiers that you want to add to the entity.</param>
        public void ApplyModifiers(int sourceId, IEnumerable<IModifier> modifiers) {
            foreach(var modifier in modifiers) ApplyModifier(sourceId,modifier);
        }

        /// <summary>
        /// This method is used to remove modifiers from the entity.  This method should only be called from the server.
        /// </summary>
        /// <param name="source">The source of the modifiers.</param>
        /// <param name="modifiers">The modifiers that you want to remove from the entity.</param>
        public void RemoveModifiers(UnityEngine.Object source, IEnumerable<IModifier> modifiers) {
            foreach(var modifier in modifiers) RemoveModifier(source,modifier);
        }

        /// <summary>
        /// This method is used to remove modifiers from the entity.  This method should only be called from the server.
        /// </summary>
        /// <param name="sourceId">The source of the modifiers.</param>
        /// <param name="modifiers">The modifiers that you want to remove from the entity.</param>
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
        
        #region Life Methods ///////////////////////////////////////////////////////////////////////////////////////////

        public bool Kill(Entity entity = null, string deathMessage = "") {
            if(!IsLivableEntity||!_alive) return false;
            _alive = false;
            OnEntityDied?.Invoke(this, entity,deathMessage);
            OnDie();
            return true;
        }
        
        public bool Revive(Entity entity = null, string reviveMessage = "") {
            if(!IsLivableEntity||_alive) return false;
            _alive = true;
            OnEntityRevived?.Invoke(this,entity, reviveMessage);
            OnRevive();
            return true;
        }

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

        /// <summary>
        /// This method is called when the entity dies.
        /// </summary>
        protected virtual void OnDie() { }

        /// <summary>
        /// This method is called when the entity is revived
        /// </summary>
        protected virtual void OnRevive() { }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}
