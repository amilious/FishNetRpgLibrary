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

using System;

namespace Amilious.Core.Threading {
    
    /// <summary>
    /// Defines the interface of an object that can be used to track a future value.
    /// </summary>
    /// <typeparam name="T">The type of object being retrieved.</typeparam>
    public interface IFuture<T> {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Gets the state of the future.
        /// </summary>
        FutureState State { get; }

        /// <summary>
        /// Gets the value if the State is Success.
        /// </summary>
        T Value { get; }

        /// <summary>
        /// Gets the failure exception if the State is Error.
        /// </summary>
        Exception Error { get; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Callbacks //////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Adds a new callback to invoke if the future value is retrieved successfully.
        /// </summary>
        /// <param name="callback">The callback to invoke.</param>
        /// <returns>The future so additional calls can be chained together.</returns>
        IFuture<T> OnSuccess(FutureCallback<T> callback);

        /// <summary>
        /// Adds a new callback to invoke if the future has an error.
        /// </summary>
        /// <param name="callback">The callback to invoke.</param>
        /// <returns>The future so additional calls can be chained together.</returns>
        IFuture<T> OnError(FutureCallback<T> callback);

        /// <summary>
        /// Adds a new callback to invoke if the future value is retrieved successfully or has an error.
        /// </summary>
        /// <param name="callback">The callback to invoke.</param>
        /// <returns>The future so additional calls can be chained together.</returns>
        IFuture<T> OnComplete(FutureCallback<T> callback);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}