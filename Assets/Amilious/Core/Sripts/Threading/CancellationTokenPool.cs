/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using System.Threading;
using System.Collections.Generic;

namespace Amilious.Core.Threading {
    
    /// <summary>
    /// This class is used to make a pool of <see cref="CancellationToken"/>s that have not been canceled.
    /// </summary>
    public static class CancellationTokenPool {

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private static readonly Queue<CancellationTokenSource> Tokens = new();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to get a cancellation token.
        /// </summary>
        /// <returns>A cancellation token from the pool.</returns>
        public static CancellationTokenSource GetToken() {
            return Tokens.TryDequeue(out var token) ? token : new CancellationTokenSource();
        }

        /// <summary>
        /// This method is used to return a cancellation token to the pool.
        /// </summary>
        /// <param name="tokenSource">The cancellation token source.</param>
        public static void ReturnToken(CancellationTokenSource tokenSource) {
            if(tokenSource.IsCancellationRequested) {
                tokenSource.Dispose();
                return;
            }
            Tokens.Enqueue(tokenSource);
        }
        
    }
}