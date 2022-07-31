/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
   _____            .__ .__   .__                             ____       ___________                __                  
  /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /  _ \      \__    ___/____  ___  ____/  |_  ____   ______ 
 /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     >  _ </\      |    | _/ __ \ \  \/  /\   __\/  _ \ /  ___/ 
/    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \     /  <_\ \/      |    | \  ___/  >    <  |  | (  <_> )\___ \  
\____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    \_____\ \      |____|  \___  >/__/\_ \ |__|  \____//____  > 
        \/       \/                                  \/            \/                  \/       \/                  \/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious,com         Unity Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious, Textos since 2022                      //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/          

using System;
using UnityEngine;
using FishNet.Object;
using Object = UnityEngine.Object;
using Amilious.FishNetRpg.Entities;
using Amilious.FishNetRpg.Modifiers;

namespace Amilious.FishNetRpg.Resource {
    
    [RequireComponent(typeof(Entity),typeof(ModifierManager))]
    [AddComponentMenu(FishNetRpg.COMPONENT_MANAGERS+"Resource Manager")]
    public class ResourceManager : NetworkBehaviour, ISystemManager {

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