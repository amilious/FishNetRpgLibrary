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

using System;
using UnityEngine;
using System.Threading;
using Amilious.Core.Extensions;

namespace Amilious.Core.Threading {
    
    /// <summary>
    /// This class is used to execute a process on a background thread.
    /// </summary>
    /// <seealso cref="ReusableFuture{T}"/>
    /// <seealso cref="ReusableFuture{T,T2}"/>
    /// <seealso cref="ReusableFuture{T,T2,T3}"/>
    /// <seealso cref="ReusableFuture{T,T2,T3,T4}"/>
    public class ReusableFuture : IReusableFuture {
        
        #region Instance Varriables 
        
        private volatile FutureState _state;
        private Action _successCallback;
        private bool _successMain;
        private Action<Exception> _errorCallback = Debug.LogError;
        private bool _errorMain = true;
        private Action _cancelCallback;
        private bool _cancelMain;
        private Func<CancellationToken, bool> _processMethod;
        private bool _executeAfterCancel;
        private CancellationTokenSource _tokenSource;
        private Exception _error;
        private readonly Action _triggerError;
        private readonly Action _triggerSuccess;
        private readonly WaitCallback _invokeProcess;
        
        #endregion
        
        #region Properties
        
        /// <summary>
        /// Gets the state of the ReusableFuture.
        /// </summary>
        public FutureState State { get { return _state; } }
        
        #endregion
        
        #region Constructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ReusableFuture"/> class.
        /// </summary>
        public ReusableFuture() {
            _state = FutureState.Pending;
            //create actions for the methods to prevent gc.
            _triggerError = TriggerError;
            _triggerSuccess = TriggerSuccess;
            _invokeProcess = InvokeProcess;
        }

        /// <summary>
        /// This constructor is used to create a new reusable future using the passed
        /// callbacks.
        /// </summary>
        /// <param name="onProcess">The method that is called to execute the task.</param>
        /// <param name="onSuccess">The method to call on success.</param>
        /// <param name="onError">The method to call on error.</param>
        /// <param name="onCancel">The method to call on cancel.</param>
        /// <param name="successMT">If true the success method will be called from the main thread.</param>
        /// <param name="errorMT">If true the error method will be called from the main thread.</param>
        /// <param name="cancelMT">If true the cancel method will be called from the main thread.</param>
        public ReusableFuture(Func<CancellationToken, bool> onProcess, Action onSuccess = null, 
            Action<Exception> onError = null, Action onCancel = null, bool successMT = true,
            bool errorMT = true, bool cancelMT = true) {
            _state = FutureState.Pending;
            if(onProcess != null) OnProcess(onProcess);
            if(onSuccess != null) OnSuccess(onSuccess, successMT);
            if(onError != null) OnError(onError, errorMT);
            if(onCancel != null) OnCancel(onCancel, cancelMT);
        }

        /// <summary>
        /// This constructor is used to create a new reusable future using the passed callbacks.
        /// </summary>
        /// <param name="onProcess">The method that is called to execute the task.</param>
        /// <param name="onSuccess">The method to call on success.</param>
        /// <param name="onCancel">The method to call on cancel.</param>
        /// <param name="successMT">If true the success method will be called from the main thread.</param>
        /// <param name="cancelMT">If true the cancel method will be called from the main thread.</param>
        public ReusableFuture(Func<CancellationToken, bool> onProcess, Action onSuccess, Action onCancel,
            bool successMT = true, bool cancelMT = true) {
            _state = FutureState.Pending;
            if(onProcess != null) OnProcess(onProcess);
            if(onSuccess != null) OnSuccess(onSuccess, successMT);
            if(onCancel != null) OnCancel(onCancel, cancelMT);
        }

        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Adds a new callback to invoke if the ReusableFuture value is retrieved successfully.
        /// </summary>
        /// <param name="callback">The callback to invoke.</param>
        /// <param name="useMainThread">If true the callback will be invoked on the main thread,
        /// otherwise the callback will be executed on the worker thread.</param>
        /// <returns>The future so additional calls can be chained together.</returns>
        public ReusableFuture OnSuccess(Action callback, bool useMainThread = true) {
            _successCallback = callback;
            _successMain = useMainThread;
            return this;
        }

        /// <summary>
        /// Adds a new callback to invoke if the ReusableFuture has an error.
        /// </summary>
        /// <param name="callback">The callback to invoke.</param>
        /// <param name="useMainThread">If true the callback will be invoked on the main thread,
        /// otherwise the callback will be executed on the worker thread.</param>
        /// <returns>The future so additional calls can be chained together.</returns>
        public ReusableFuture OnError(Action<Exception> callback, bool useMainThread = true) {
            _errorCallback = callback;
            _errorMain = useMainThread;
            return this;
        }

        /// <summary>
        /// Adds a new callback to invoke if the ReusableFuture is canceled.
        /// </summary>
        /// <param name="callback">The callback to invoke.</param>
        /// <param name="useMainThread">If true the callback will be invoked on the main thread,
        /// otherwise the callback will be executed on the worker thread.</param>
        /// <returns>The future so additional calls can be chained together.</returns>
        public ReusableFuture OnCancel(Action callback, bool useMainThread = true) {
            _cancelCallback = callback;
            _cancelMain = useMainThread;
            return this;
        }

        /// <summary>
        /// Adds a new callback to invoke when processing the ReusableFuture.
        /// </summary>
        /// <param name="processMethod">The method that will be invoked to process the ReusableFuture.</param>
        /// <returns>The future so additional calls can be chained together.</returns>
        public ReusableFuture OnProcess(Func<CancellationToken, bool> processMethod) {
            _processMethod = processMethod;
            return this;
        }

        /// <summary>
        /// Begins running a given function on a background thread to resolve the ReusableFuture's value
        /// <remarks>This will cancel a previous process if it is still being executed.</remarks>
        /// </summary>
        public void Process() {
            if(_processMethod == null)
                throw new InvalidOperationException("Cannot process a future that has no process assigned.");
            if(_state == FutureState.Processing) {
                _executeAfterCancel = true;
                Cancel();
                return;
            }
            _state = FutureState.Processing;
            _tokenSource = CancellationTokenPool.GetToken();
            _executeAfterCancel = false;
            ThreadPool.QueueUserWorkItem(_invokeProcess);
        }

        /// <summary>
        /// This method is used to cancel
        /// <returns>True if the process was canceled, otherwise returns false if
        /// the process was not being executed.</returns>
        /// </summary>
        public bool Cancel() {
            if(_tokenSource == null) {
                //still call the cancel callback even if there was no running process.
                CancelImpl();
                return false;
            }
            _executeAfterCancel = false;
            _tokenSource?.Cancel();
            return true;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method is called on completion of the task.
        /// </summary>
        /// <param name="success">Contains true if the task was completed
        /// successfully.</param>
        private void AssignImpl(bool success) {
            if(!success)FailImpl(new InvalidOperationException("The result was false."));
            _state = FutureState.Success;
            if(_successCallback == null) return;
            if(_successMain) AmiliousExecutor.InvokeAsync(_triggerSuccess);
            else _successCallback.Invoke();
        }

        /// <summary>
        /// This method is called when the task throws an error.
        /// </summary>
        /// <param name="error">The thrown error.</param>
        private void FailImpl(Exception error) {
            _state = FutureState.Error;
            if(_errorCallback == null) return;
            if(_errorMain) {
                _error = error;
                AmiliousExecutor.InvokeAsync(_triggerError);
            }
            else _errorCallback.Invoke(error);
        }

        /// <summary>
        /// This method is called when the task is cancelled.
        /// </summary>
        private void CancelImpl() {
            _state = FutureState.Canceled;
            if(_cancelCallback == null) return;
            if(_cancelMain) AmiliousExecutor.InvokeAsync(_cancelCallback);
            else _cancelCallback.Invoke();
        }
        
        /// <summary>
        /// This method is called when there is an available worker thread.
        /// </summary>
        /// <param name="state">The state</param>
        private void InvokeProcess(object state) {
            try {
                // Directly call the Impl version to avoid the state validation of the public method
                AssignImpl(_processMethod(_tokenSource.Token));
                _tokenSource.Token.ThrowIfCancellationRequested();
            } catch(OperationCanceledException) {
                CancelImpl();
            }
            catch(Exception e) {
                //if cancellation was requested ignore errors.
                if(_tokenSource.IsCancellationRequested) CancelImpl();
                // Directly call the Impl version to avoid the state validation of the public method
                FailImpl(e);
            }finally {
                CancellationTokenPool.ReturnToken(_tokenSource);
                _tokenSource = null;
                if(_executeAfterCancel) Process();
            }
        }

        /// <summary>
        /// This method is used to invoke the success callback.
        /// </summary>
        private void TriggerSuccess() => _successCallback?.Invoke();
        
        /// <summary>
        /// This method is used to invoke the error callback.
        /// </summary>
        private void TriggerError() => _errorCallback?.Invoke(_error);
        
        #endregion
        
    }
    
    /// <summary>
    /// This class is used to execute a process on a background thread.
    /// </summary>
    /// <typeparam name="T">The type of value that will be generated by the process.</typeparam>
    /// <seealso cref="ReusableFuture"/>
    /// <seealso cref="ReusableFuture{T,T2}"/>
    /// <seealso cref="ReusableFuture{T,T2,T3}"/>
    /// <seealso cref="ReusableFuture{T,T2,T3,T4}"/>
    public class ReusableFuture<T> : IReusableFuture {
        
        #region Instance Variables
        
        private volatile FutureState _state;
        private Action<T> _successCallback;
        private bool _successMain;
        private Action<Exception> _errorCallback = Debug.LogError;
        private bool _errorMain = true;
        private Action _cancelCallback;
        private bool _cancelMain;
        private Func<CancellationToken,T> _processMethod;
        private bool _executeAfterCancel;
        private CancellationTokenSource _tokenSource;
        private Exception _error;
        private readonly Action _triggerError;
        private readonly WaitCallback _invokeProcess;
        
        #endregion
        
        #region Properties
        
        /// <summary>
        /// Gets the state of the ReusableFuture.
        /// </summary>
        public FutureState State { get { return _state; } }
        
        #endregion
        
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReusableFuture{T}"/> class.
        /// </summary>
        public ReusableFuture() {
            _state = FutureState.Pending;
            _triggerError = TriggerError;
            _invokeProcess = InvokeProcess;
        }
        
        /// <summary>
        /// This constructor is used to create a new reusable future using the passed
        /// callbacks.
        /// </summary>
        /// <param name="onProcess">The method that is called to execute the task.</param>
        /// <param name="onSuccess">The method to call on success.</param>
        /// <param name="onError">The method to call on error.</param>
        /// <param name="onCancel">The method to call on cancel.</param>
        /// <param name="successMT">If true the success method will be called from the main thread.</param>
        /// <param name="errorMT">If true the error method will be called from the main thread.</param>
        /// <param name="cancelMT">If true the cancel method will be called from the main thread.</param>
        public ReusableFuture(Func<CancellationToken,T> onProcess, Action<T> onSuccess = null, 
            Action<Exception> onError = null, Action onCancel = null, bool successMT = true,
            bool errorMT = true, bool cancelMT = true) {
            _state = FutureState.Pending;
            _triggerError = TriggerError;
            _invokeProcess = InvokeProcess;
            if(onProcess != null) OnProcess(onProcess);
            if(onSuccess != null) OnSuccess(onSuccess, successMT);
            if(onError != null) OnError(onError, errorMT);
            if(onCancel != null) OnCancel(onCancel, cancelMT);
        }

        /// <summary>
        /// This constructor is used to create a new reusable future using the passed callbacks.
        /// </summary>
        /// <param name="onProcess">The method that is called to execute the task.</param>
        /// <param name="onSuccess">The method to call on success.</param>
        /// <param name="onCancel">The method to call on cancel.</param>
        /// <param name="successMT">If true the success method will be called from the main thread.</param>
        /// <param name="cancelMT">If true the cancel method will be called from the main thread.</param>
        public ReusableFuture(Func<CancellationToken,T> onProcess, Action<T> onSuccess, Action onCancel,
            bool successMT = true, bool cancelMT = true) {
            _state = FutureState.Pending;
            _triggerError = TriggerError;
            _invokeProcess = InvokeProcess;
            if(onProcess != null) OnProcess(onProcess);
            if(onSuccess != null) OnSuccess(onSuccess, successMT);
            if(onCancel != null) OnCancel(onCancel, cancelMT);
        }
        
        #endregion
        
        #region Public Methods

        /// <summary>
        /// Adds a new callback to invoke if the ReusableFuture value is retrieved successfully.
        /// </summary>
        /// <param name="callback">The callback to invoke.</param>
        /// <param name="useMainThread">If true the callback will be invoked on the main thread,
        /// otherwise the callback will be executed on the worker thread.</param>
        /// <returns>The future so additional calls can be chained together.</returns>
        public ReusableFuture<T> OnSuccess(Action<T> callback, bool useMainThread = true) {
            _successCallback = callback;
            _successMain = useMainThread;
            return this;
        }

        /// <summary>
        /// Adds a new callback to invoke if the ReusableFuture has an error.
        /// </summary>
        /// <param name="callback">The callback to invoke.</param>
        /// <param name="useMainThread">If true the callback will be invoked on the main thread,
        /// otherwise the callback will be executed on the worker thread.</param>
        /// <returns>The future so additional calls can be chained together.</returns>
        public ReusableFuture<T> OnError(Action<Exception> callback, bool useMainThread = true) {
            _errorCallback = callback;
            _errorMain = useMainThread;
            return this;
        }

        /// <summary>
        /// Adds a new callback to invoke if the ReusableFuture is canceled.
        /// </summary>
        /// <param name="callback">The callback to invoke.</param>
        /// <param name="useMainThread">If true the callback will be invoked on the main thread,
        /// otherwise the callback will be executed on the worker thread.</param>
        /// <returns>The future so additional calls can be chained together.</returns>
        public ReusableFuture<T> OnCancel(Action callback, bool useMainThread = true) {
            _cancelCallback = callback;
            _cancelMain = useMainThread;
            return this;
        }

        /// <summary>
        /// Adds a new callback to invoke when processing the ReusableFuture.
        /// </summary>
        /// <param name="processMethod">The method that will be invoked to process the ReusableFuture.</param>
        /// <returns>The future so additional calls can be chained together.</returns>
        public ReusableFuture<T> OnProcess(Func<CancellationToken,T> processMethod) {
            _processMethod = processMethod;
            return this;
        }

        /// <summary>
        /// Begins running a given function on a background thread to resolve the ReusableFuture's value
        /// <remarks>This will cancel a previous process if it is still being executed.</remarks>
        /// </summary>
        public void Process() {
            if(_processMethod == null) {
                throw new InvalidOperationException("Cannot process a future that has no process assigned.");
            }
            if(_state == FutureState.Processing) {
                _executeAfterCancel = true;
                _tokenSource?.Cancel();
                return;
            }
            _state = FutureState.Processing;
            _tokenSource = new CancellationTokenSource();
            _executeAfterCancel = false;
            ThreadPool.QueueUserWorkItem(_invokeProcess);
        }

        /// <summary>
        /// This method is used to cancel
        /// <returns>True if the process was canceled, otherwise returns false if
        /// the process was not being executed.</returns>
        /// </summary>
        public bool Cancel() {
            if(_tokenSource == null) {
                //still call the cancel callback even if there was no running process.
                CancelImpl();
                return false;
            }
            _tokenSource?.Cancel();
            return true;
        }
        
        #endregion
        
        #region Private Methods

        /// <summary>
        /// This method is called on completion of the task.
        /// </summary>
        /// <param name="value">The tasks result.</param>
        private void AssignImpl(T value) {
            _state = FutureState.Success;
            if(_successCallback == null) return;
            if(_successMain) {
                AmiliousExecutor.InvokeAsync(()=>_successCallback?.Invoke(value));
            }
            else _successCallback.Invoke(value);
        }

        /// <summary>
        /// This method is called when the task throws an error.
        /// </summary>
        /// <param name="error">The thrown error.</param>
        private void FailImpl(Exception error) {
            _state = FutureState.Error;
            if(_errorCallback == null) return;
            if(_errorMain) {
                _error = error;
                AmiliousExecutor.InvokeAsync(_triggerError);
            }
            else _errorCallback.Invoke(error);
        }

        /// <summary>
        /// This method is called when the task is cancelled.
        /// </summary>
        private void CancelImpl() {
            _state = FutureState.Canceled;
            if(_cancelCallback == null) return;
            if(_cancelMain) AmiliousExecutor.InvokeAsync(_cancelCallback);
            else _cancelCallback.Invoke();
        }
        
        /// <summary>
        /// This method is called when there is an available worker thread.
        /// </summary>
        /// <param name="state">The state</param>
        private void InvokeProcess(object state) {
            try {
                // Directly call the Impl version to avoid the state validation of the public method
                AssignImpl(_processMethod(_tokenSource.Token));
                _tokenSource.Token.ThrowIfCancellationRequested();
            }
            catch(OperationCanceledException) {
                CancelImpl();
            }
            catch(Exception e) {
                //if cancellation was requested ignore errors.
                if(_tokenSource.IsCancellationRequested) CancelImpl();
                // Directly call the Impl version to avoid the state validation of the public method
                FailImpl(e);
            }finally {
                _tokenSource.ReturnToPool();
                _tokenSource = null;
                if(_executeAfterCancel) Process();
            }
        }

        /// <summary>
        /// This method is used to invoke the error callback.
        /// </summary>
        private void TriggerError() {
            if(_error==null) return;
            _errorCallback?.Invoke(_error);
            _error = null;
        }

        #endregion
        
    }
    
    /// <summary>
    /// This class is used to execute a process on a background thread.
    /// </summary>
    /// <typeparam name="T">The type of value that will be generated by the process.</typeparam>
    /// <typeparam name="T2">The first type of value that will be used to generate the value.</typeparam>
    /// <seealso cref="ReusableFuture"/>
    /// <seealso cref="ReusableFuture{T}"/>
    /// <seealso cref="ReusableFuture{T,T2,T3}"/>
    /// <seealso cref="ReusableFuture{T,T2,T3,T4}"/>
    public class ReusableFuture<T,T2> : IReusableFuture {
        
        #region Instance Variables
        
        private volatile FutureState _state;
        private Action<T> _successCallback;
        private bool _successMain;
        private Action<Exception> _errorCallback = Debug.LogError;
        private bool _errorMain = true;
        private Action _cancelCallback;
        private bool _cancelMain;
        private Func<T2,CancellationToken,T> _processMethod;
        private bool _executeAfterCancel;
        private T2 _executeInput;
        private CancellationTokenSource _tokenSource;
        private Exception _error;
        private readonly Action _triggerError;
        private readonly WaitCallback _invokeProcess;
        
        #endregion
        
        #region Properties
        
        /// <summary>
        /// Gets the state of the ReusableFuture.
        /// </summary>
        public FutureState State { get { return _state; } }
        
        #endregion
        
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReusableFuture{T,T2}"/> class.
        /// </summary>
        public ReusableFuture() {
            _state = FutureState.Pending;
            _triggerError = TriggerError;
            _invokeProcess = InvokeProcess;
        }
        
        /// <summary>
        /// This constructor is used to create a new reusable future using the passed
        /// callbacks.
        /// </summary>
        /// <param name="onProcess">The method that is called to execute the task.</param>
        /// <param name="onSuccess">The method to call on success.</param>
        /// <param name="onError">The method to call on error.</param>
        /// <param name="onCancel">The method to call on cancel.</param>
        /// <param name="successMT">If true the success method will be called from the main thread.</param>
        /// <param name="errorMT">If true the error method will be called from the main thread.</param>
        /// <param name="cancelMT">If true the cancel method will be called from the main thread.</param>
        public ReusableFuture(Func<T2,CancellationToken,T> onProcess, Action<T> onSuccess, 
            Action<Exception> onError = null, Action onCancel = null, bool successMT = true,
            bool errorMT = true, bool cancelMT = true) {
            _state = FutureState.Pending;
            _triggerError = TriggerError;
            _invokeProcess = InvokeProcess;
            if(onProcess != null) OnProcess(onProcess);
            if(onSuccess != null) OnSuccess(onSuccess, successMT);
            if(onError != null) OnError(onError, errorMT);
            if(onCancel != null) OnCancel(onCancel, cancelMT);
        }

        /// <summary>
        /// This constructor is used to create a new reusable future using the passed callbacks.
        /// </summary>
        /// <param name="onProcess">The method that is called to execute the task.</param>
        /// <param name="onSuccess">The method to call on success.</param>
        /// <param name="onCancel">The method to call on cancel.</param>
        /// <param name="successMT">If true the success method will be called from the main thread.</param>
        /// <param name="cancelMT">If true the cancel method will be called from the main thread.</param>
        public ReusableFuture(Func<T2,CancellationToken,T> onProcess, Action<T> onSuccess, Action onCancel,
            bool successMT = true, bool cancelMT = true) {
            _state = FutureState.Pending;
            _triggerError = TriggerError;
            _invokeProcess = InvokeProcess;
            if(onProcess != null) OnProcess(onProcess);
            if(onSuccess != null) OnSuccess(onSuccess, successMT);
            if(onCancel != null) OnCancel(onCancel, cancelMT);
        }

        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Adds a new callback to invoke if the ReusableFuture value is retrieved successfully.
        /// </summary>
        /// <param name="callback">The callback to invoke.</param>
        /// <param name="useMainThread">If true the callback will be invoked on the main thread,
        /// otherwise the callback will be executed on the worker thread.</param>
        /// <returns>The future so additional calls can be chained together.</returns>
        public ReusableFuture<T,T2> OnSuccess(Action<T> callback, bool useMainThread = true) {
            _successCallback = callback;
            _successMain = useMainThread;
            return this;
        }

        /// <summary>
        /// Adds a new callback to invoke if the ReusableFuture has an error.
        /// </summary>
        /// <param name="callback">The callback to invoke.</param>
        /// <param name="useMainThread">If true the callback will be invoked on the main thread,
        /// otherwise the callback will be executed on the worker thread.</param>
        /// <returns>The future so additional calls can be chained together.</returns>
        public ReusableFuture<T,T2> OnError(Action<Exception> callback, bool useMainThread = true) {
            _errorCallback = callback;
            _errorMain = useMainThread;
            return this;
        }

        /// <summary>
        /// Adds a new callback to invoke if the ReusableFuture is canceled.
        /// </summary>
        /// <param name="callback">The callback to invoke.</param>
        /// <param name="useMainThread">If true the callback will be invoked on the main thread,
        /// otherwise the callback will be executed on the worker thread.</param>
        /// <returns>The future so additional calls can be chained together.</returns>
        public ReusableFuture<T,T2> OnCancel(Action callback, bool useMainThread = true) {
            _cancelCallback = callback;
            _cancelMain = useMainThread;
            return this;
        }

        /// <summary>
        /// Adds a new callback to invoke when processing the ReusableFuture.
        /// </summary>
        /// <param name="processMethod">The method that will be invoked to process the ReusableFuture.</param>
        /// <returns>The future so additional calls can be chained together.</returns>
        public ReusableFuture<T,T2> OnProcess(Func<T2,CancellationToken,T> processMethod) {
            _processMethod = processMethod;
            return this;
        }

        /// <summary>
        /// Begins running a given function on a background thread to resolve the ReusableFuture's value
        /// <param name="inputValue">The first value that will be used to process the ReusableFuture's value</param>
        /// <remarks>This will cancel a previous process if it is still being executed.</remarks>
        /// </summary>
        public void Process(T2 inputValue) {
            if(_processMethod == null) {
                throw new InvalidOperationException("Cannot process a future that has no process assigned.");
            }
            _executeInput = inputValue;
            if(_state == FutureState.Processing) {
                _executeAfterCancel = true;
                _tokenSource?.Cancel();
                return;
            }
            _state = FutureState.Processing;
            _tokenSource = CancellationTokenPool.GetToken();
            _executeAfterCancel = false;
            ThreadPool.QueueUserWorkItem(_invokeProcess);
        }

        /// <summary>
        /// This method is used to cancel
        /// <returns>True if the process was canceled, otherwise returns false if
        /// the process was not being executed.</returns>
        /// </summary>
        public bool Cancel() {
            if(_tokenSource == null) {
                //still call the cancel callback even if there was no running process.
                CancelImpl();
                return false;
            }
            _executeAfterCancel = false;
            _tokenSource?.Cancel();
            return true;
        }

        #endregion
        
        #region Private Methods
        
        /// <summary>
        /// This method is called on completion of the task.
        /// </summary>
        /// <param name="value">The tasks result.</param>
        private void AssignImpl(T value) {
            _state = FutureState.Success;
            if(_successCallback == null) return;
            if(_successMain) AmiliousExecutor.InvokeAsync(() => _successCallback.Invoke(value));
            else _successCallback.Invoke(value);
        }

        /// <summary>
        /// This method is called when the task throws an error.
        /// </summary>
        /// <param name="error">The thrown error.</param>
        private void FailImpl(Exception error) {
            _state = FutureState.Error;
            if(_errorCallback == null) return;
            if(_errorMain) {
                _error = error;
                AmiliousExecutor.InvokeAsync(_triggerError);
            }
            else _errorCallback.Invoke(error);
        }

        /// <summary>
        /// This method is called when the task is cancelled.
        /// </summary>
        private void CancelImpl() {
            _state = FutureState.Canceled;
            if(_cancelCallback == null) return;
            if(_cancelMain) AmiliousExecutor.InvokeAsync(_cancelCallback);
            else _cancelCallback.Invoke();
        }
        
        /// <summary>
        /// This method is called when there is an available worker thread.
        /// </summary>
        /// <param name="state">The state</param>
        private void InvokeProcess(object state) {
            try {
                AssignImpl(_processMethod(_executeInput,_tokenSource.Token));
                _tokenSource.Token.ThrowIfCancellationRequested();
            }catch(OperationCanceledException) {CancelImpl();
            }catch(Exception e) {
                //if cancellation was requested ignore errors.
                if(_tokenSource.IsCancellationRequested) CancelImpl();
                FailImpl(e);
            }finally {
                _tokenSource.ReturnToPool();
                _tokenSource = null;
                if(_executeAfterCancel) Process(_executeInput);
            }
        }
        
        /// <summary>
        /// This method is used to invoke the error callback.
        /// </summary>
        private void TriggerError() {
            if(_error==null) return;
            _errorCallback?.Invoke(_error);
            _error = null;
        }
        
        #endregion
        
    }
    
    /// <summary>
    /// This class is used to execute a process on a background thread.
    /// </summary>
    /// <typeparam name="T">The type of value that will be generated by the process.</typeparam>
    /// <typeparam name="T2">The first type of value that will be used to generate the value.</typeparam>
    /// <typeparam name="T3">The second type of value that will be used to generate the value.</typeparam>
    /// <seealso cref="ReusableFuture"/>
    /// <seealso cref="ReusableFuture{T}"/>
    /// <seealso cref="ReusableFuture{T,T2}"/>
    /// <seealso cref="ReusableFuture{T,T2,T3,T4}"/>
    public class ReusableFuture<T,T2,T3> : IReusableFuture {
        
        #region Instance Variables
        
        private volatile FutureState _state;
        private Action<T> _successCallback;
        private bool _successMain;
        private Action<Exception> _errorCallback = Debug.LogError;
        private bool _errorMain = true;
        private Action _cancelCallback;
        private bool _cancelMain;
        private Func<T2,T3,CancellationToken,T> _processMethod;
        private bool _executeAfterCancel;
        private T2 _executeInput;
        private T3 _executeInput2;
        private CancellationTokenSource _tokenSource;
        private Exception _error;
        private readonly Action _triggerError;
        private readonly WaitCallback _invokeProcess;
        
        #endregion
        
        #region Properties
        
        /// <summary>
        /// Gets the state of the ReusableFuture.
        /// </summary>
        public FutureState State { get { return _state; } }

        #endregion
        
        #region Constructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ReusableFuture{T,T2,T3}"/> class.
        /// </summary>
        public ReusableFuture() {
            _state = FutureState.Pending;
            _triggerError = TriggerError;
            _invokeProcess = InvokeProcess;
        }
        
        /// <summary>
        /// This constructor is used to create a new reusable future using the passed
        /// callbacks.
        /// </summary>
        /// <param name="onProcess">The method that is called to execute the task.</param>
        /// <param name="onSuccess">The method to call on success.</param>
        /// <param name="onError">The method to call on error.</param>
        /// <param name="onCancel">The method to call on cancel.</param>
        /// <param name="successMT">If true the success method will be called from the main thread.</param>
        /// <param name="errorMT">If true the error method will be called from the main thread.</param>
        /// <param name="cancelMT">If true the cancel method will be called from the main thread.</param>
        public ReusableFuture(Func<T2,T3,CancellationToken,T> onProcess, Action<T> onSuccess, 
            Action<Exception> onError = null, Action onCancel = null, bool successMT = true,
            bool errorMT = true, bool cancelMT = true) {
            _state = FutureState.Pending;
            _triggerError = TriggerError;
            _invokeProcess = InvokeProcess;
            if(onProcess != null) OnProcess(onProcess);
            if(onSuccess != null) OnSuccess(onSuccess, successMT);
            if(onError != null) OnError(onError, errorMT);
            if(onCancel != null) OnCancel(onCancel, cancelMT);
        }

        /// <summary>
        /// This constructor is used to create a new reusable future using the passed callbacks.
        /// </summary>
        /// <param name="onProcess">The method that is called to execute the task.</param>
        /// <param name="onSuccess">The method to call on success.</param>
        /// <param name="onCancel">The method to call on cancel.</param>
        /// <param name="successMT">If true the success method will be called from the main thread.</param>
        /// <param name="cancelMT">If true the cancel method will be called from the main thread.</param>
        public ReusableFuture(Func<T2,T3,CancellationToken,T> onProcess, Action<T> onSuccess, Action onCancel,
            bool successMT = true, bool cancelMT = true) {
            _state = FutureState.Pending;
            _triggerError = TriggerError;
            _invokeProcess = InvokeProcess;
            if(onProcess != null) OnProcess(onProcess);
            if(onSuccess != null) OnSuccess(onSuccess, successMT);
            if(onCancel != null) OnCancel(onCancel, cancelMT);
        }

        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Adds a new callback to invoke if the ReusableFuture value is retrieved successfully.
        /// </summary>
        /// <param name="callback">The callback to invoke.</param>
        /// <param name="useMainThread">If true the callback will be invoked on the main thread,
        /// otherwise the callback will be executed on the worker thread.</param>
        /// <returns>The future so additional calls can be chained together.</returns>
        public ReusableFuture<T,T2,T3> OnSuccess(Action<T> callback, bool useMainThread = true) {
            _successCallback = callback;
            _successMain = useMainThread;
            return this;
        }

        /// <summary>
        /// Adds a new callback to invoke if the ReusableFuture has an error.
        /// </summary>
        /// <param name="callback">The callback to invoke.</param>
        /// <param name="useMainThread">If true the callback will be invoked on the main thread,
        /// otherwise the callback will be executed on the worker thread.</param>
        /// <returns>The future so additional calls can be chained together.</returns>
        public ReusableFuture<T,T2,T3> OnError(Action<Exception> callback, bool useMainThread = true) {
            _errorCallback = callback;
            _errorMain = useMainThread;
            return this;
        }

        /// <summary>
        /// Adds a new callback to invoke if the ReusableFuture is canceled.
        /// </summary>
        /// <param name="callback">The callback to invoke.</param>
        /// <param name="useMainThread">If true the callback will be invoked on the main thread,
        /// otherwise the callback will be executed on the worker thread.</param>
        /// <returns>The future so additional calls can be chained together.</returns>
        public ReusableFuture<T,T2,T3> OnCancel(Action callback, bool useMainThread = true) {
            _cancelCallback = callback;
            _cancelMain = useMainThread;
            return this;
        }

        /// <summary>
        /// Adds a new callback to invoke when processing the ReusableFuture.
        /// </summary>
        /// <param name="processMethod">The method that will be invoked to process the ReusableFuture.</param>
        /// <returns>The future so additional calls can be chained together.</returns>
        public ReusableFuture<T,T2,T3> OnProcess(Func<T2,T3,CancellationToken,T> processMethod) {
            _processMethod = processMethod;
            return this;
        }

        /// <summary>
        /// Begins running a given function on a background thread to resolve the ReusableFuture's value
        /// <param name="inputValue">The first value that will be used to process the ReusableFuture's value</param>
        /// <param name="inputValue2">The second value that will be used to process the ReusableFuture's value</param>
        /// <remarks>This will cancel a previous process if it is still being executed.</remarks>
        /// </summary>
        public void Process(T2 inputValue, T3 inputValue2) {
            if(_processMethod == null) {
                throw new InvalidOperationException("Cannot process a future that has no process assigned.");
            }
            _executeInput = inputValue;
            _executeInput2 = inputValue2;
            if(_state == FutureState.Processing) {
                _executeAfterCancel = true;
                _tokenSource?.Cancel();
                return;
            }
            _state = FutureState.Processing;
            _tokenSource = CancellationTokenPool.GetToken();
            _executeAfterCancel = false;
            ThreadPool.QueueUserWorkItem(_invokeProcess);
        }

        /// <summary>
        /// This method is used to cancel
        /// <returns>True if the process was canceled, otherwise returns false if
        /// the process was not being executed.</returns>
        /// </summary>
        public bool Cancel() {
            if(_tokenSource == null) {
                //still call the cancel callback even if there was no running process.
                CancelImpl();
                return false;
            }
            _executeAfterCancel = false;
            _tokenSource?.Cancel();
            return true;
        }

        #endregion
        
        #region Private Methods
        
        /// <summary>
        /// This method is called on completion of the task.
        /// </summary>
        /// <param name="value">The tasks result.</param>
        private void AssignImpl(T value) {
            _state = FutureState.Success;
            if(_successCallback == null) return;
            if(_successMain) AmiliousExecutor.InvokeAsync(() => _successCallback.Invoke(value));
            else _successCallback.Invoke(value);
        }

        /// <summary>
        /// This method is called when the task throws an error.
        /// </summary>
        /// <param name="error">The thrown error.</param>
        private void FailImpl(Exception error) {
            _state = FutureState.Error;
            if(_errorCallback == null) return;
            if(_errorMain) {
                _error = error;
                AmiliousExecutor.InvokeAsync(_triggerError);
            }
            else _errorCallback.Invoke(error);
        }

        /// <summary>
        /// This method is called when the task is cancelled.
        /// </summary>
        private void CancelImpl() {
            _state = FutureState.Canceled;
            if(_cancelCallback == null) return;
            if(_cancelMain) AmiliousExecutor.InvokeAsync(_cancelCallback);
            else _cancelCallback.Invoke();
        }
        
        /// <summary>
        /// This method is called when there is an available worker thread.
        /// </summary>
        /// <param name="state">The state</param>
        private void InvokeProcess(object state) {
            try {
                AssignImpl(_processMethod(_executeInput,_executeInput2,_tokenSource.Token));
                _tokenSource.Token.ThrowIfCancellationRequested();
            }catch(OperationCanceledException) {CancelImpl();
            }catch(Exception e) {
                //if cancellation was requested ignore errors.
                if(_tokenSource.IsCancellationRequested) CancelImpl();
                FailImpl(e);
            }finally {
                _tokenSource.ReturnToPool();
                _tokenSource = null;
                if(_executeAfterCancel) Process(_executeInput,_executeInput2);
            }
        }
        
        /// <summary>
        /// This method is used to invoke the error callback.
        /// </summary>
        private void TriggerError() {
            if(_error==null) return;
            _errorCallback?.Invoke(_error);
            _error = null;
        }
        
        #endregion
        
    }
    
    /// <summary>
    /// This class is used to execute a process on a background thread.
    /// </summary>
    /// <typeparam name="T">The type of value that will be generated by the process.</typeparam>
    /// <typeparam name="T2">The first type of value that will be used to generate the value.</typeparam>
    /// <typeparam name="T3">The second type of value that will be used to generate the value.</typeparam>
    /// <typeparam name="T4">The third type of value that will be used to generate the value.</typeparam>
    /// <seealso cref="ReusableFuture"/>
    /// <seealso cref="ReusableFuture{T}"/>
    /// <seealso cref="ReusableFuture{T,T2}"/>
    /// <seealso cref="ReusableFuture{T,T2,T3}"/>
    public class ReusableFuture<T,T2,T3,T4> : IReusableFuture {
        
        #region Instance Variables
        
        private volatile FutureState _state;
        private Action<T> _successCallback;
        private bool _successMain;
        private Action<Exception> _errorCallback = Debug.LogError;
        private bool _errorMain = true;
        private Action _cancelCallback;
        private bool _cancelMain;
        private Func<T2,T3,T4,CancellationToken,T> _processMethod;
        private bool _executeAfterCancel;
        private T2 _executeInput;
        private T3 _executeInput2;
        private T4 _executeInput3;
        private CancellationTokenSource _tokenSource;
        private Exception _error;
        private readonly Action _triggerError;
        private readonly WaitCallback _invokeProcess;
        
        #endregion
        
        #region Properties
        
        /// <summary>
        /// Gets the state of the ReusableFuture.
        /// </summary>
        public FutureState State { get { return _state; } }

        #endregion
        
        #region Constructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ReusableFuture{T,T2,T3,T4}"/> class.
        /// </summary>
        public ReusableFuture() {
            _state = FutureState.Pending;
            _triggerError = TriggerError;
            _invokeProcess = InvokeProcess;
        }
        
        /// <summary>
        /// This constructor is used to create a new reusable future using the passed
        /// callbacks.
        /// </summary>
        /// <param name="onProcess">The method that is called to execute the task.</param>
        /// <param name="onSuccess">The method to call on success.</param>
        /// <param name="onError">The method to call on error.</param>
        /// <param name="onCancel">The method to call on cancel.</param>
        /// <param name="successMT">If true the success method will be called from the main thread.</param>
        /// <param name="errorMT">If true the error method will be called from the main thread.</param>
        /// <param name="cancelMT">If true the cancel method will be called from the main thread.</param>
        public ReusableFuture(Func<T2,T3,T4,CancellationToken,T> onProcess, Action<T> onSuccess, 
            Action<Exception> onError = null, Action onCancel = null, bool successMT = true,
            bool errorMT = true, bool cancelMT = true) {
            _state = FutureState.Pending;
            _triggerError = TriggerError;
            _invokeProcess = InvokeProcess;
            if(onProcess != null) OnProcess(onProcess);
            if(onSuccess != null) OnSuccess(onSuccess, successMT);
            if(onError != null) OnError(onError, errorMT);
            if(onCancel != null) OnCancel(onCancel, cancelMT);
        }

        /// <summary>
        /// This constructor is used to create a new reusable future using the passed callbacks.
        /// </summary>
        /// <param name="onProcess">The method that is called to execute the task.</param>
        /// <param name="onSuccess">The method to call on success.</param>
        /// <param name="onCancel">The method to call on cancel.</param>
        /// <param name="successMT">If true the success method will be called from the main thread.</param>
        /// <param name="cancelMT">If true the cancel method will be called from the main thread.</param>
        public ReusableFuture(Func<T2,T3,T4,CancellationToken,T> onProcess, Action<T> onSuccess, Action onCancel,
            bool successMT = true, bool cancelMT = true) {
            _state = FutureState.Pending;
            _triggerError = TriggerError;
            _invokeProcess = InvokeProcess;
            if(onProcess != null) OnProcess(onProcess);
            if(onSuccess != null) OnSuccess(onSuccess, successMT);
            if(onCancel != null) OnCancel(onCancel, cancelMT);
        }

        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Adds a new callback to invoke if the ReusableFuture value is retrieved successfully.
        /// </summary>
        /// <param name="callback">The callback to invoke.</param>
        /// <param name="useMainThread">If true the callback will be invoked on the main thread,
        /// otherwise the callback will be executed on the worker thread.</param>
        /// <returns>The future so additional calls can be chained together.</returns>
        public ReusableFuture<T,T2,T3,T4> OnSuccess(Action<T> callback, bool useMainThread = true) {
            _successCallback = callback;
            _successMain = useMainThread;
            return this;
        }

        /// <summary>
        /// Adds a new callback to invoke if the ReusableFuture has an error.
        /// </summary>
        /// <param name="callback">The callback to invoke.</param>
        /// <param name="useMainThread">If true the callback will be invoked on the main thread,
        /// otherwise the callback will be executed on the worker thread.</param>
        /// <returns>The future so additional calls can be chained together.</returns>
        public ReusableFuture<T,T2,T3,T4> OnError(Action<Exception> callback, bool useMainThread = true) {
            _errorCallback = callback;
            _errorMain = useMainThread;
            return this;
        }

        /// <summary>
        /// Adds a new callback to invoke if the ReusableFuture is canceled.
        /// </summary>
        /// <param name="callback">The callback to invoke.</param>
        /// <param name="useMainThread">If true the callback will be invoked on the main thread,
        /// otherwise the callback will be executed on the worker thread.</param>
        /// <returns>The future so additional calls can be chained together.</returns>
        public ReusableFuture<T,T2,T3,T4> OnCancel(Action callback, bool useMainThread = true) {
            _cancelCallback = callback;
            _cancelMain = useMainThread;
            return this;
        }

        /// <summary>
        /// Adds a new callback to invoke when processing the ReusableFuture.
        /// </summary>
        /// <param name="processMethod">The method that will be invoked to process the ReusableFuture.</param>
        /// <returns>The future so additional calls can be chained together.</returns>
        public ReusableFuture<T,T2,T3,T4> OnProcess(Func<T2,T3,T4,CancellationToken,T> processMethod) {
            _processMethod = processMethod;
            return this;
        }

        /// <summary>
        /// Begins running a given function on a background thread to resolve the ReusableFuture's value
        /// <param name="inputValue">The first value that will be used to process the ReusableFuture's value</param>
        /// <param name="inputValue2">The second value that will be used to process the ReusableFuture's value</param>
        /// <param name="inputValue3">The third value that will be used to process the ReusableFuture's value</param>
        /// <remarks>This will cancel a previous process if it is still being executed.</remarks>
        /// </summary>
        public void Process(T2 inputValue, T3 inputValue2, T4 inputValue3) {
            if(_processMethod == null) {
                throw new InvalidOperationException("Cannot process a future that has no process assigned.");
            }
            _executeInput = inputValue;
            _executeInput2 = inputValue2;
            _executeInput3 = inputValue3;
            if(_state == FutureState.Processing) {
                _executeAfterCancel = true;
                _tokenSource?.Cancel();
                return;
            }
            _state = FutureState.Processing;
            _tokenSource = CancellationTokenPool.GetToken();
            _executeAfterCancel = false;
            ThreadPool.QueueUserWorkItem(_invokeProcess);
        }

        /// <summary>
        /// This method is used to cancel
        /// <returns>True if the process was canceled, otherwise returns false if
        /// the process was not being executed.</returns>
        /// </summary>
        public bool Cancel() {
            if(_tokenSource == null) {
                //still call the cancel callback even if there was no running process.
                CancelImpl();
                return false;
            }
            _executeAfterCancel = false;
            _tokenSource?.Cancel();
            return true;
        }

        #endregion
        
        #region Private Methods
        
        /// <summary>
        /// This method is called on completion of the task.
        /// </summary>
        /// <param name="value">The tasks result.</param>
        private void AssignImpl(T value) {
            _state = FutureState.Success;
            if(_successCallback == null) return;
            if(_successMain) AmiliousExecutor.InvokeAsync(() => _successCallback.Invoke(value));
            else _successCallback.Invoke(value);
        }

        /// <summary>
        /// This method is called when the task throws an error.
        /// </summary>
        /// <param name="error">The thrown error.</param>
        private void FailImpl(Exception error) {
            _state = FutureState.Error;
            if(_errorCallback == null) return;
            if(_errorMain) {
                _error = error;
                AmiliousExecutor.InvokeAsync(_triggerError);
            }
            else _errorCallback.Invoke(error);
        }

        /// <summary>
        /// This method is called when the task is cancelled.
        /// </summary>
        private void CancelImpl() {
            _state = FutureState.Canceled;
            if(_cancelCallback == null) return;
            if(_cancelMain) AmiliousExecutor.InvokeAsync(_cancelCallback);
            else _cancelCallback.Invoke();
        }
        
        /// <summary>
        /// This method is called when there is an available worker thread.
        /// </summary>
        /// <param name="state">The state</param>
        private void InvokeProcess(object state) {
            try {
                AssignImpl(_processMethod(_executeInput,_executeInput2,_executeInput3,_tokenSource.Token));
                _tokenSource.Token.ThrowIfCancellationRequested();
            }catch(OperationCanceledException) {CancelImpl();
            }catch(Exception e) {
                //if cancellation was requested ignore errors.
                if(_tokenSource.IsCancellationRequested) CancelImpl();
                FailImpl(e);
            }finally {
                _tokenSource.ReturnToPool();
                _tokenSource = null;
                if(_executeAfterCancel) Process(_executeInput,_executeInput2,_executeInput3);
            }
        }
        
        /// <summary>
        /// This method is used to invoke the error callback.
        /// </summary>
        private void TriggerError() {
            if(_error==null) return;
            _errorCallback?.Invoke(_error);
            _error = null;
        }
        
        #endregion
        
    }
    
}