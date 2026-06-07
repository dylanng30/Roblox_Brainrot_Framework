using System.Collections.Generic;
using Dylanng.Core;
using Dylanng.Core.Systems.TickSystem;
using RobloxFW.InteractionSystem.Data;
using RobloxFW.InteractionSystem.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RobloxFW.InteractionSystem.Components
{
    public class PlayerInteractor: MonoBehaviour, IUpdatable, IEntityInteractor
    {
        [Header("--- INTERACTION SETTINGS ---")]
        [SerializeField] private Transform interationPivot;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private float interactRadius = 2f;
        [SerializeField] private int maxInteractablesPerScan = 5;
        
        // Update settings
        private float _scanTimer;
        private const float ScanInterval = 0.1f;
        //
        
        // Components
        private Collider[] _colliders;
        private IInteractable _focusedInteractableObject;
        //
        
        private bool _onInteract;
        
        private void Awake()
        {
            _colliders = new Collider[maxInteractablesPerScan];
            
            ServiceLocator.Get<ITickSystem>().Register(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.Get<ITickSystem>().Unregister(this);
        }

        public void OnUpdate(float deltaTime)
        {
            _scanTimer += deltaTime;
            if (_scanTimer >= ScanInterval)
            {
                _scanTimer = 0f;
                IInteractable nearestInteractableObjectObj = FindNearestInteractableObject();
                UpdateFocus(nearestInteractableObjectObj);
            }
        }

        private IInteractable FindNearestInteractableObject()
        {
            int count = Physics.OverlapSphereNonAlloc(interationPivot.position, interactRadius, _colliders, interactableLayer, QueryTriggerInteraction.Collide);
            
            IInteractable nearestInteractableObjectObj = null;
            float bestDistanceSqr = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                Collider col = _colliders[i];
                
                if(col == null) continue;
                if (!col.TryGetComponent(out IInteractable interactableObject)) continue;
                if (!interactableObject.CanInteract()) continue;
                
                Vector3 closestPoint = col.ClosestPoint(interationPivot.position);
                float distanceSqr = (closestPoint - interationPivot.position).sqrMagnitude;

                if (distanceSqr < bestDistanceSqr)
                {
                    bestDistanceSqr = distanceSqr;
                    nearestInteractableObjectObj = interactableObject;
                }
            }
            
            return nearestInteractableObjectObj;
        }

        private void UpdateFocus(IInteractable nearestObj)
        {
            if ((Object)_focusedInteractableObject == (Object)nearestObj) return;
            
            if (_focusedInteractableObject != null)
            {
                _focusedInteractableObject.OnFocusLost();
            }
            
            _focusedInteractableObject = nearestObj;
            List<InteractionData> interactions = null;
            
            if (_focusedInteractableObject != null)
            {
                _focusedInteractableObject.OnFocusGained();
                interactions = _focusedInteractableObject.GetAvailableInteractions(this);
            }
            
            EventBus.Publish(new InteractionFocusChangedEvent 
            {
                FocusedObject = _focusedInteractableObject,
                AvailableInteractions = interactions
            });
        }

        #region --- HANDLE EVENT ---
        public void HandleDemo(IInteractable interactableObject)
        {
            Debug.Log("Demo");
        }
        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            if (interationPivot != null)
                Gizmos.DrawWireSphere(interationPivot.position, interactRadius);
        }

        
    }
}

