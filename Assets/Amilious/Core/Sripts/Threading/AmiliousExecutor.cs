using System;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Amilious.Core.Threading {
    
    /// <summary>
    /// This <see cref="MonoBehaviour"/> is used to execute code on the main game thread when using multiple threads.
    /// </summary>
    [AddComponentMenu("Amilious/Threading/Executor")]
    public class AmiliousExecutor : MonoBehaviour {
        
        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This constant is the sleep time to wait when using the <see cref="Invoke"/> method.
        /// </summary>
        public const int INVOKE_SLEEP_TIME = 5;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////

        [SerializeField,Tooltip("The maximum size of the queue.  If the queue exceeds this amount actions will be " +
                                "continuously invoked until the queued items are less than or equal to this number.")]
        private int maxQueueSize = 500;

        [Header("Update")]
        [SerializeField,Tooltip("The max time for invoking for each update. If less than or equal to zero there will " +
                                 "be no max time.")]
        private float maxUpdateSeconds = .05f;
        [SerializeField,Tooltip("The max invokes for each update.  If less than or equal to zero there will be " +
                                "no max invokes.")]
        private int maxInvokesPerUpdate = 5;
        [SerializeField,Tooltip("The number of updates to skip before invoking queued actions.")]
        private int skippedUpdates = 2;

        [Header("Fixed Update")]
        [SerializeField,Tooltip("The max time for invoking for each fixed update. If less than or equal to zero " +
                                 "there will be no max time.")]
        private float maxFixedUpdateSeconds = .05f;
        [SerializeField,Tooltip("The max invokes for each fixed update.  If less than or equal to zero there will be " +
                                "no max invokes.")]
        private int maxInvokesPerFixedUpdate = 5;
        [SerializeField,Tooltip("The number of fixed updates to skip before invoking queued actions.")]
        private int skippedFixedUpdates = 2;

        [Header("Late Update")]
        [SerializeField,Tooltip("The max time for invoking for each late update. If less than or equal to zero " +
                                "there will be no max time.")]
        private float maxLateUpdateSeconds = .05f;
        [SerializeField,Tooltip("The max invokes for each late update.  If less than or equal to zero there will be " +
                                "no max invokes.")]
        private int maxInvokesPerLateUpdate = 5;
        [SerializeField,Tooltip("The number of late updates to skip before invoking queued actions.")]
        private int skippedLateUpdates = 2;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
        #region Non-Serialized Fields //////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This field is used to store the active instance of the executor.
        /// </summary>
        private static AmiliousExecutor _instance;

        /// <summary>
        /// This dictionary is used to store queued actions.
        /// </summary>
        private static readonly Dictionary<UpdateType, ConcurrentQueue<Action>> ActionQueue = new();

        /// <summary>
        /// This dictionary is used to keep track of the skipped updates.
        /// </summary>
        private static readonly Dictionary<UpdateType, int> SkipCounter = new() {
            { UpdateType.Update, 0 }, { UpdateType.FixedUpdate, 0 }, { UpdateType.LateUpdate, 0 }
        };

        /// <summary>
        /// This dictionary is used to hold the max update time to preform updates.
        /// </summary>
        private static readonly Dictionary<UpdateType, float> MaxUpdateSeconds = new() {
            { UpdateType.Update, 0 }, { UpdateType.FixedUpdate, 0 }, { UpdateType.LateUpdate, 0 }
        };

        /// <summary>
        /// This dictionary contains the max invokes to preform per update.
        /// </summary>
        private static readonly Dictionary<UpdateType, int> MaxInvokes = new() {
            { UpdateType.Update, 0 }, { UpdateType.FixedUpdate, 0 }, { UpdateType.LateUpdate, 0 }
        };

        /// <summary>
        /// This dictionary contains the number of updates to skip between the execution of actions.
        /// </summary>
        private static readonly Dictionary<UpdateType, int> UpdatesToSkip = new() {
            { UpdateType.Update, 0 }, { UpdateType.FixedUpdate, 0 }, { UpdateType.LateUpdate, 0 }
        };

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////   

        public static Thread MainThread { get; private set; }
        
        public static int MaxQueueSize { get; private set; }

        public static bool IsMainThread => Thread.CurrentThread == MainThread;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Unity Methods //////////////////////////////////////////////////////////////////////////////////////////

        private void Awake() => OnEnable();

        private void OnDestroy() { if(_instance == this) _instance = null; }

        private void OnDisable() { if(_instance == this) _instance = null; }

        private void OnEnable() {
            if(_instance && _instance != this) {
                DestroyImmediate(this);
                return;
            }
            if(_instance == this) return; //the executor is already set up.
            //make sure that the queue is set up
            ActionQueue.TryAdd(UpdateType.Update, new ConcurrentQueue<Action>());
            ActionQueue.TryAdd(UpdateType.FixedUpdate, new ConcurrentQueue<Action>());
            ActionQueue.TryAdd(UpdateType.LateUpdate, new ConcurrentQueue<Action>());
            //set the max update seconds
            MaxUpdateSeconds[UpdateType.Update] = maxUpdateSeconds;
            MaxUpdateSeconds[UpdateType.FixedUpdate] = maxFixedUpdateSeconds;
            MaxUpdateSeconds[UpdateType.LateUpdate] = maxLateUpdateSeconds;
            //set the max invokes
            MaxInvokes[UpdateType.Update] = maxInvokesPerUpdate;
            MaxInvokes[UpdateType.FixedUpdate] = maxInvokesPerFixedUpdate;
            MaxInvokes[UpdateType.LateUpdate] = maxInvokesPerLateUpdate;
            //set the skipped updates
            UpdatesToSkip[UpdateType.Update] = skippedUpdates;
            UpdatesToSkip[UpdateType.FixedUpdate] = skippedFixedUpdates;
            UpdatesToSkip[UpdateType.LateUpdate] = skippedLateUpdates;
            //set the max queue size
            MaxQueueSize = maxQueueSize;
            //set the instance
            MainThread = Thread.CurrentThread;
            _instance = this;
        }

        private void Update() => Process(UpdateType.Update);

        private void FixedUpdate() => Process(UpdateType.FixedUpdate);

        private void LateUpdate() => Process(UpdateType.LateUpdate);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to queue an action to be invoked on the main game thread.
        /// </summary>
        /// <param name="action">The action to be queued.</param>
        /// <param name="updateType">The update loop that the action should be executed on.</param>
        /// <seealso cref="Invoke"/>
        public static void InvokeAsync(Action action, UpdateType updateType = UpdateType.Update) {
            if(!_instance){Debug.LogError(AmiliousCore.NO_EXECUTOR); return; }
            if(IsMainThread) action();
            else ActionQueue[updateType].Enqueue(action);
        }

        /// <summary>
        /// This method is used to queue an action to be invoked on the main game thread and blocks
        /// the current thread until the action has been executed.  This method will not return until
        /// after the action has been executed.
        /// </summary>
        /// <param name="action">The action to be executed.</param> 
        /// <param name="updateType">The update loop that the action should be executed on.</param>
        /// <seealso cref="InvokeAsync"/>
        public static void Invoke(Action action, UpdateType updateType = UpdateType.Update) {
            if(!_instance){Debug.LogError(AmiliousCore.NO_EXECUTOR); return; }
            var hasRun = false;
            InvokeAsync(() => { action(); hasRun = true; },updateType);
            while(!hasRun) Thread.Sleep(INVOKE_SLEEP_TIME);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to execute queue tasks.
        /// </summary>
        /// <param name="updateType">The current update method type.</param>
        private static void Process(UpdateType updateType) {
            //return if the queue for the update type is empty
            if(ActionQueue[updateType].IsEmpty) return;
            //get the max update time
            //before I used a timer and I am not sure which way is the best
            //TODO:test different timing methods
            var maxTime = MaxUpdateSeconds[updateType]>0?
                Time.realtimeSinceStartupAsDouble + MaxUpdateSeconds[updateType]:
                double.MaxValue;
            //make sure the the queue is under the max queue size;
            while(ActionQueue[updateType].Count>MaxQueueSize) 
                if(ActionQueue[updateType].TryDequeue(out var action)){ action();}
            //check if this is a skipped update
            SkipCounter[updateType]++;
            if(SkipCounter[updateType] <= UpdatesToSkip[updateType]) return;
            UpdatesToSkip[updateType] = 0;
            //get max invokes
            var maxInvokes = MaxInvokes[updateType] > 0 ? MaxInvokes[updateType] : int.MaxValue;
            //invoke actions
            var invokes = 0;
            while(!ActionQueue[updateType].IsEmpty && invokes < maxInvokes &&
                  Time.realtimeSinceStartupAsDouble <= maxTime) {
                invokes++;
                if(ActionQueue[updateType].TryDequeue(out var action)){ action();}
            }
        }
        
    }
}