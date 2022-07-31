////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                                    //
//    _____            .__ .__   .__                             _________  __              .___.__                   //
//   /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /   _____/_/  |_  __ __   __| _/|__|  ____   ______   //
//  /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     \_____  \ \   __\|  |  \ / __ | |  | /  _ \ /  ___/   //
// /    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \      /        \ |  |  |  |  // /_/ | |  |(  <_> )\___ \    //
// \____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    /_______  / |__|  |____/ \____ | |__| \____//____  >   //
//         \/       \/                                  \/             \/                    \/                 \/    //
//                                                                                                                    //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious,comUnity          Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using Amilious.Core.Extensions;

namespace Amilious.Core {
    
    /// <summary>
    /// This class is used to contain global values and methods.
    /// </summary>
    public static class AmiliousCore {

        public const string NO_EXECUTOR = "No Amilious Executor exists in the scene.  Actions will not be invoked!";

        public const string MAIN_CONTEXT_MENU = "Amilious/";

        public const string THREADING_CONTEXT_MENU = MAIN_CONTEXT_MENU + "Threading/";

        public const string INVALID_SUCCESS = "The value property is not available unless state is Success.";

        public const string INVALID_ERROR = "The error property is not available unless state is Error.";

        public const string INVALID_PENDING = "Cannot process a future that isn't in the Pending state.";

        public static string MakeTitle(string title) {
            return title.PadText('#', 60, 10).SetColor("ffff88");
        }

        #region Menu Buttons ///////////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR
        
        [UnityEditor.MenuItem("Amilious/Amilious Scriptable Objects/Fix Duplicate Ids", false,0)]
        public static void FixDuplicateIds() => AmiliousScriptableObject.FixDuplicateIds();
        
        [UnityEditor.MenuItem("Amilious/Amilious Scriptable Objects/Regenerate Ids", false,1)]
        public static void RegenerateIds() => AmiliousScriptableObject.RegenerateIds();
        
        #endif
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
}