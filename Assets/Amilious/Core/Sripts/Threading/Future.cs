using System;
using System.Threading;
using System.Collections.Generic;

namespace Amilious.Core.Threading {

    /// <summary>
    /// An implementation of <see cref="IFuture{T}"/> that can be used internally by methods that return futures.
    /// </summary>
    /// <remarks>
    /// Methods should always return the <see cref="IFuture{T}"/> interface when calling code requests a future.
    /// This class is intended to be constructed internally in the method to provide a simple implementation of
    /// the interface. By returning the interface instead of the class it ensures the implementation can change
    /// later on if requirements change, without affecting the calling code.
    /// </remarks>
    /// <typeparam name="T">The type of object being retrieved.</typeparam>
    public sealed class Future<T> : IFuture<T> {
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private volatile FutureState _state;
        private T _value;
        private Exception _error;
        private readonly List<FutureCallback<T>> _successCallbacks = new ();
        private readonly List<FutureCallback<T>> _errorCallbacks = new ();
        private readonly Action _flushError;
        private readonly Action _flushSuccess;
        private readonly Action<FutureCallback<T>> _executeCallback;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties
        
        /// <inheritdoc />
        public FutureState State { get => _state; }

        /// <inheritdoc />
        public T Value {
            get {
                if(_state == FutureState.Success) return _value;
                throw new InvalidOperationException(AmiliousCore.INVALID_SUCCESS);
            }
        }

        /// <inheritdoc />
        public Exception Error {
            get {
                if(_state != FutureState.Error) return _error;
                throw new InvalidOperationException(AmiliousCore.INVALID_ERROR);
            }
        }
        
        #endregion

        #region Constructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Future{T}"/> class.
        /// </summary>
        public Future() {
            _state = FutureState.Pending;
            _flushError = FlushErrorCallbacks;
            _flushSuccess = FlushSuccessCallbacks;
        }

        #endregion
        
        #region Public Methods

        /// <inheritdoc />
        public IFuture<T> OnSuccess(FutureCallback<T> callback) {
            if (_state == FutureState.Success) {
                if (AmiliousExecutor.IsMainThread) callback(this);
                else AmiliousExecutor.InvokeAsync(() => callback(this));
            }else if (_state != FutureState.Error && !_successCallbacks.Contains(callback)) {
                _successCallbacks.Add(callback);
            }
            return this;
        }

        /// <inheritdoc />
        public IFuture<T> OnError(FutureCallback<T> callback) {
            if (_state == FutureState.Error) {
                if (AmiliousExecutor.IsMainThread) callback(this);
                else AmiliousExecutor.InvokeAsync(() => callback(this));
            }else if (_state != FutureState.Success && !_errorCallbacks.Contains(callback)) {
                _errorCallbacks.Add(callback);
            }
            return this;
        }
        
        

        /// <inheritdoc />
        public IFuture<T> OnComplete(FutureCallback<T> callback) {
            if (_state is FutureState.Success or FutureState.Error) {
                if (AmiliousExecutor.IsMainThread) callback(this);
                else AmiliousExecutor.InvokeAsync(() => callback(this));
            } else {
                if (!_successCallbacks.Contains(callback)) _successCallbacks.Add(callback);
                if (!_errorCallbacks.Contains(callback)) _errorCallbacks.Add(callback);
            }
            return this;
        }

        private void ExecuteCallback(FutureCallback<T> callback) => callback?.Invoke(this);

        /// <summary>
        /// Begins running a given function on a background thread to resolve the future's value, as long
        /// as it is still in the Pending state.
        /// </summary>
        /// <param name="func">The function that will retrieve the desired value.</param>
        public IFuture<T> Process(Func<T> func) {
            if (_state != FutureState.Pending) {
                throw new InvalidOperationException(AmiliousCore.INVALID_PENDING);
            }
            _state = FutureState.Processing;
            ThreadPool.QueueUserWorkItem(_ => {
                try {
                    // Directly call the Impl version to avoid the state validation of the public method
                    AssignImpl(func());
                }
                catch (Exception e) {
                    // Directly call the Impl version to avoid the state validation of the public method
                    FailImpl(e);
                }
            });
            return this;
        }
        
        /// <summary>
        /// Allows manually assigning a value to a future, as long as it is still in the pending state.
        /// </summary>
        /// <remarks>
        /// There are times where you may not need to do background processing for a value. For example,
        /// you may have a cache of values and can just hand one out. In those cases you still want to
        /// return a future for the method signature, but can just call this method to fill in the future.
        /// </remarks>
        /// <param name="value">The value to assign the future.</param>
        public void Assign(T value) {
            if (_state != FutureState.Pending) {
                throw new InvalidOperationException("Cannot assign a value to a future that isn't in the Pending state.");
            }
            AssignImpl(value);
        }

        /// <summary>
        /// Allows manually failing a future, as long as it is still in the pending state.
        /// </summary>
        /// <remarks>
        /// As with the Assign method, there are times where you may know a future value is a failure without
        /// doing any background work. In those cases you can simply fail the future manually and return it.
        /// </remarks>
        /// <param name="error">The exception to use to fail the future.</param>
        public void Fail(Exception error) {
            if (_state != FutureState.Pending) {
                throw new InvalidOperationException("Cannot fail future that isn't in the Pending state.");
            }
            FailImpl(error);
        }
        
        #endregion

        #region Private Methods
        
        /// <summary>
        /// This method is called when the future's task has been completed.
        /// </summary>
        /// <param name="value">The resulting value.</param>
        private void AssignImpl(T value) {
            _value = value;
            _error = null;
            _state = FutureState.Success;
            AmiliousExecutor.InvokeAsync(_flushSuccess);
        }

        /// <summary>
        /// This method is called when an error occurs when trying to execute a future.
        /// </summary>
        /// <param name="error">The error that occured.</param>
        private void FailImpl(Exception error) {
            _value = default(T);
            _error = error;
            _state = FutureState.Error;
            AmiliousExecutor.InvokeAsync(_flushError);
        }

        /// <summary>
        /// This method is used to flush the success callbacks.
        /// </summary>
        private void FlushSuccessCallbacks() {
            foreach (var callback in _successCallbacks) callback(this);
            _successCallbacks.Clear();
            _errorCallbacks.Clear();
        }

        /// <summary>
        /// This method is used to flush the error callbacks.
        /// </summary>
        private void FlushErrorCallbacks() {
            foreach (var callback in _errorCallbacks) callback(this);
            _successCallbacks.Clear();
            _errorCallbacks.Clear();
        }
        
        #endregion
        
    }
}