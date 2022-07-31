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
using System.Collections.Generic;
using UnityEngine;

namespace Amilious.FishyRpg.Modifiers {
    
    /// <summary>
    /// This manager is used to handle special duration modifiers in an efficient way.
    /// </summary>
    [AddComponentMenu(FishNetRpg.COMPONENT_MANAGERS+"Modifier Manager")]
    public class ModifierManager : MonoBehaviour {

        #region Private Instance Fields ////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field contains the cached time that the next modifier will expire.
        /// </summary>
        private float _nextActionTime = float.MaxValue;
        
        /// <summary>
        /// This field contains a cached value for if there are currently and unexpired duration modifiers.
        /// </summary>
        private bool _containsModifiers = false;
        
        /// <summary>
        /// This field is used to hold values that will be removed.  By using a readonly list we do not need
        /// to create a new list every time.
        /// </summary>
        private readonly List<IModifierSource> _removeKeys = new();
        
        /// <summary>
        /// This field contains a dictionary of the current modifiers and their callbacks.
        /// </summary>
        private readonly Dictionary<IModifierSource,Action<IModifierSource>> _durationModifiers = new();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Unity Methods //////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is called by unity on a fixed rate which will insure that our expire time will be
        /// as accurate as possible.
        /// </summary>
        private void FixedUpdate() {
            if(!_containsModifiers || Time.time < _nextActionTime) return;
            var nextActionTime = float.MaxValue;
            _removeKeys.Clear();            //clear the remove keys just to make sure that it is empty.
            foreach(var keyValue in _durationModifiers) {
                if(keyValue.Key.ExpireTime < Time.time) {   // if the modifier is expired
                    keyValue.Value?.Invoke(keyValue.Key);   // invoke the expireCallback
                    _removeKeys.Add(keyValue.Key);          // add the modifier into the list to remove
                } else {                    // the modifier is not expired
                    //get the next time that a modifier will expire.
                    if(keyValue.Key.ExpireTime < nextActionTime) nextActionTime = keyValue.Key.ExpireTime;
                }
            }
            //remove the modifiers whose callback was invoked.
            foreach(var key in _removeKeys) _durationModifiers.Remove(key);
            _removeKeys.Clear(); //clear the remove keys again so that all references to the modifer are removed.
            _nextActionTime = nextActionTime;               // set the next action time
            if(_durationModifiers.Count == 0)               // if there are no more modifiers
                _containsModifiers = false;                 // set _containsModifiers to false.
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
        #region Add Duration Modifier //////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to add a modifier to the duration timer if it has a duration.
        /// </summary>
        /// <param name="modifierSource">The modifier source.</param>
        /// <param name="expireCallback">A method that will be called when the modifier expires.</param>
        /// <returns>True if the given <see cref="IModifierSource"/> is a duration modifier.</returns>
        public bool AddDurationModifier(IModifierSource modifierSource, Action<IModifierSource> expireCallback) {
            if(!modifierSource.DurationModifier || _durationModifiers.ContainsKey(modifierSource)) return false;
            _durationModifiers[modifierSource] = expireCallback;
            if(modifierSource.ExpireTime < _nextActionTime) _nextActionTime = modifierSource.ExpireTime;
            _containsModifiers = true;
            return true;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}