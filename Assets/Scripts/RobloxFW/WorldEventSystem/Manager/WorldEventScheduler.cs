using System;
using System.Collections.Generic;
using UnityEngine;
using RobloxFW.WorldEventSystem.Data;
using RobloxFW.WorldEventSystem.Data.Remote;
using RobloxFW.WorldEventSystem.Data.Local.WorldEventData;
using RobloxFW.WorldEventSystem.Data.Runtime;
using RobloxFW.WorldEventSystem.Interfaces;
using RobloxFW.WorldEventSystem.Strategies.Selection;
using Dylanng.Core.Base;
using Dylanng.Core.Systems.TickSystem;

namespace RobloxFW.WorldEventSystem.Manager
{
    public class WorldEventScheduler : ManagerBase, IUpdatable
    {
        [Header("--- REFERENCES ---")]
        [SerializeField] private WorldEventManager worldEventManager;
        [SerializeField] private LocalWorldEventDatabaseSO localDatabase;

        [Space(5)]
        [Header("--- SETTINGS ---")]
        [SerializeField] private int upcomingQueueSize = 3;
        
        [Space(5)]
        [Header("--- MOCK DATA ---")] 
        [SerializeField] private TextAsset remoteWorldEventSystemData_TextAsset;
        
        private RemoteWorldEventSystemData _remoteWorldEventSystemData;
        
        private Dictionary<ScheduleModeEnum, IEventSelectionStrategy> _selectionStrategies;
        private IEventSelectionStrategy _activeStrategy;
        
        private bool _isWaitingForCooldown;
        private float _currentCooldownTimer;
        private bool _isEventActive;
        private float _currentEventTimer;
        private WorldEventEnum _activeEventId;

        public bool IsWaitingForCooldown => _isWaitingForCooldown;
        public float CurrentCooldownTimer => _currentCooldownTimer;
        public bool IsEventActive => _isEventActive;
        public float CurrentEventTimer => _currentEventTimer;
        public WorldEventEnum ActiveEventId => _activeEventId;
        
        private List<RemoteWorldEventData> _upcomingEvents = new List<RemoteWorldEventData>();
        public IReadOnlyList<RemoteWorldEventData> UpcomingEvents => _upcomingEvents;
        
        // TEMP
        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }
        
        private void Update()
        {
            OnUpdate(Time.deltaTime);
        }
        //

        public override void Initialize()
        {
            _selectionStrategies = new Dictionary<ScheduleModeEnum, IEventSelectionStrategy>
            {
                { ScheduleModeEnum.Random, new RandomSelectionStrategy() },
                { ScheduleModeEnum.Sequential, new SequentialSelectionStrategy() }
            };
            
            _remoteWorldEventSystemData = JsonUtility.FromJson<RemoteWorldEventSystemData>(remoteWorldEventSystemData_TextAsset.text);

            if (_remoteWorldEventSystemData != null)
            {
                _selectionStrategies.TryGetValue(_remoteWorldEventSystemData.ScheduleMode, out _activeStrategy);
            }
        }
        
        private void Start()
        {
            if (_remoteWorldEventSystemData == null)
            {
                Debug.LogWarning("WorldEventScheduler: No Remote Config assigned.");
                return;
            }
            if (localDatabase == null)
            {
                Debug.LogWarning("WorldEventScheduler: No Local Database assigned.");
                return;
            }

            RefillQueue();

            if (_remoteWorldEventSystemData.WaitBeforeFirstEvent)
            {
                StartCooldown();
            }
            else
            {
                TriggerNextEvent();
            }
        }

        private void RefillQueue()
        {
            if (_activeStrategy == null || _remoteWorldEventSystemData == null) return;
            
            while (_upcomingEvents.Count < upcomingQueueSize)
            {
                RemoteWorldEventData nextRemoteEvent = _activeStrategy.GetNextEvent(_remoteWorldEventSystemData.EventPool);
                if (nextRemoteEvent != null && nextRemoteEvent.EventId != WorldEventEnum.None)
                {
                    _upcomingEvents.Add(nextRemoteEvent);
                }
                else
                {
                    break;
                }
            }
        }

        private void OnEnable()
        {
            if (worldEventManager != null)
            {
                worldEventManager.OnWorldEventEnded += HandleWorldEventEnded;
            }
        }

        private void OnDisable()
        {
            if (worldEventManager != null)
            {
                worldEventManager.OnWorldEventEnded -= HandleWorldEventEnded;
            }
        }

        private void HandleWorldEventEnded(WorldEventEnum eventId)
        {
            _isEventActive = false;
            StartCooldown();
        }

        private void StartCooldown()
        {
            _isWaitingForCooldown = true;
            _currentCooldownTimer = _remoteWorldEventSystemData.EventCooldown;
        }

        private void TriggerNextEvent()
        {
            if (worldEventManager == null) return;

            if (_upcomingEvents.Count > 0)
            {
                RemoteWorldEventData nextRemoteEvent = _upcomingEvents[0];
                _upcomingEvents.RemoveAt(0);

                WorldEventSO localData = localDatabase.GetLocalData(nextRemoteEvent.EventId);
                Texture2D[] envTextures = localData != null ? localData.EnviromentTextures : null;

                WorldEventContext context = new WorldEventContext(
                    nextRemoteEvent.EventId,
                    nextRemoteEvent.Duration,
                    nextRemoteEvent.Multiplier,
                    envTextures
                );

                worldEventManager.StartEvent(context);
                
                _isEventActive = true;
                _currentEventTimer = nextRemoteEvent.Duration;
                _activeEventId = nextRemoteEvent.EventId;
                
                RefillQueue();
            }
            else
            {
                Debug.LogWarning("Scheduler tried to trigger an invalid or null event.");
                StartCooldown(); 
            }
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (_isWaitingForCooldown)
            {
                _currentCooldownTimer -= deltaTime;
                
                if (_currentCooldownTimer <= 0)
                {
                    _isWaitingForCooldown = false;
                    TriggerNextEvent();
                }
            }
            else if (_isEventActive)
            {
                _currentEventTimer -= deltaTime;
                
                if (_currentEventTimer <= 0)
                {
                    _isEventActive = false;
                    if (worldEventManager != null)
                    {
                        worldEventManager.EndEvent(_activeEventId);
                    }
                }
            }
        }
        
        public float GetTimeUntilEventStarts(int queueIndex)
        {
            if (queueIndex < 0 || queueIndex >= _upcomingEvents.Count) return 0f;

            float time = 0f;

            if (_isWaitingForCooldown)
            {
                time += _currentCooldownTimer;
            }
            else if (_isEventActive)
            {
                time += _currentEventTimer + _remoteWorldEventSystemData.EventCooldown;
            }

            for (int i = 0; i < queueIndex; i++)
            {
                time += _upcomingEvents[i].Duration + _remoteWorldEventSystemData.EventCooldown;
            }

            return time;
        }
        
    }
}
