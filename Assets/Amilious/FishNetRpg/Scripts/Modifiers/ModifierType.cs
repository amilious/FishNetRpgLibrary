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

namespace Amilious.FishNetRpg.Modifiers {
    
    /// <summary>
    /// This class is used to represent a modifier type.
    /// </summary>
    public enum ModifierType {
        
        /// <summary>
        /// This value indicates that a value should be added to the base value before the multipliers.
        /// </summary>
        Additive = 0,
        
        /// <summary>
        /// All modifiers of this type will have their values added together before multiplying the stats base
        /// value after applying the additive modifiers.
        /// </summary>
        AdditiveMultiplier = 5,
        
        /// <summary>
        /// Modifiers of this type will multiply the base value individually after applying the AdditiveMultiplier
        /// modifiers and the additive multipliers. 
        /// </summary>
        StackableMultiplier = 10,
        
        /// <summary>
        /// Modifiers of this type will be added to the base value after the multiplier modifiers have been applied.
        /// </summary>
        PostMultiplierAdditive = 15,
        
        /// <summary>
        /// Modifiers of this type will override the base value.
        /// </summary>
        Override = 20
        
    }
}