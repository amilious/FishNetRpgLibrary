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

using UnityEngine;
using Amilious.Core;

namespace Amilious.FishNetRpg.Modifiers {
    
    /// <summary>
    /// This is the base class for scriptable object modifiers.
    /// </summary>
    public abstract class Modifier : AmiliousScriptableObject, IModifier {
        
        [SerializeField, Tooltip("The type of the modifier.")] 
        private ModifierType modifierType;
        [SerializeField, Tooltip("The modifier amount.")] 
        private float amount;
        [SerializeField, Tooltip("If greater than -1, the time that the modifier will last.")] 
        private float duration = -1;
        
        /// <inheritdoc />
        public abstract Systems System { get; }

        /// <inheritdoc />
        public virtual float Amount => amount;

        /// <inheritdoc />
        public virtual ModifierType ModifierType => modifierType;

        /// <inheritdoc />
        public virtual float Duration => duration;
    }
}