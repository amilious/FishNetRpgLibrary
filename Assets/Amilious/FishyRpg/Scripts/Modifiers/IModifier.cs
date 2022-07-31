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

namespace Amilious.FishyRpg.Modifiers {
    
    /// <summary>
    /// This interface is the base of all modifiers.
    /// </summary>
    public interface IModifier {
        
        /// <summary>
        /// This property contains the system that the modifier is for.
        /// </summary>
        public Systems System { get; }

        /// <summary>
        /// This property contains the modifier's value.
        /// </summary>
        public float Amount { get; }
        
        /// <summary>
        /// This property contains the modifier's operation.
        /// </summary>
        public ModifierType ModifierType { get; }
        
        /// <summary>
        /// This property is used to add a duration for the modifier.  If this value is less than zero the modifier
        /// will not be automatically removed after a period of time.
        /// </summary>
        public float Duration { get; }
        
    }
    
}