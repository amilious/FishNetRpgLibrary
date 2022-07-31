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

namespace Amilious.FishyRpg.Modifiers {
    
    
    /// <summary>
    /// This is a wrapper class for <see cref="IModifier"/>s that contains the source of the
    /// modifier and the time that the modifier was applied.
    /// </summary>
    public class ModifierSource<T> : IModifierSource where T : IModifier {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the modifier.
        /// </summary>
        public T Modifier { get; }
        
        /// <inheritdoc />
        public int SourceId { get; }
        
        /// <inheritdoc />
        public float AppliedTime { get; }
        
        /// <inheritdoc />
        public float ExpireTime { get; }
        
        /// <inheritdoc />
        public bool DurationModifier { get; }

        /// <inheritdoc />
        public Systems System { get; }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to create a new instance using the source object.
        /// </summary>
        /// <param name="modifier">The modifier.</param>
        /// <param name="source">The source of the modifier.</param>
        public ModifierSource(T modifier, Object source) {
            Modifier = modifier;
            SourceId = source.GetInstanceID();
            AppliedTime = Time.realtimeSinceStartup;
            DurationModifier = modifier.Duration > -1;
            ExpireTime = DurationModifier? Time.time + modifier.Duration : -1;
            System = modifier.System;
        }

        /// <summary>
        /// This constructor is used to create a new instance using the source's id.
        /// </summary>
        /// <param name="modifier">The modifier.</param>
        /// <param name="sourceId">The source id.</param>
        public ModifierSource(T modifier, int sourceId) {
            Modifier = modifier;
            SourceId = sourceId;
            AppliedTime = Time.time;
            DurationModifier = modifier.Duration > -1;
            ExpireTime = DurationModifier? Time.time + modifier.Duration : -1;
            System = modifier.System;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public bool HasSource(Object source) => SourceId == source.GetInstanceID();

        /// <inheritdoc />
        public bool HasSource(int sourceId) => SourceId == sourceId;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}