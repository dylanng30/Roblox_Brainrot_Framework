using Dylanng.Core;
using RobloxFW.MorphSystem.Data;
using RobloxFW.MorphSystem.Events;
using UnityEngine;

namespace RobloxFW.MorphSystem.Controllers
{
    public class PlayerModelController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private Transform modelRoot;

        [Header("Data")]
        [SerializeField] private MorphTierData morphTierData;
        
        private GameObject[] _instantiatedModels;
        private GameObject _currentActiveModel;

        private void Awake()
        {
            PreInstantiateModels();
        }

        private void OnEnable()
        {
            EventBus.Subscribe<MorphTierChangedEvent>(OnMorphTierChanged);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<MorphTierChangedEvent>(OnMorphTierChanged);
        }

        private void PreInstantiateModels()
        {
            if (morphTierData == null || morphTierData.Tiers == null)
            {
                Debug.LogError("[PlayerModelController] MorphTierData bị thiếu!");
                return;
            }

            int tierCount = morphTierData.Tiers.Count;
            _instantiatedModels = new GameObject[tierCount];

            for (int i = 0; i < tierCount; i++)
            {
                MorphTier tier = morphTierData.Tiers[i];
                if (tier.ModelPrefab != null)
                {
                    GameObject instance = Instantiate(tier.ModelPrefab, modelRoot);
                    instance.transform.localPosition = Vector3.zero;
                    instance.transform.localRotation = Quaternion.identity;
                    instance.transform.localScale = Vector3.one * tier.ScaleMultiplier;
                    
                    instance.SetActive(false); 
                    _instantiatedModels[i] = instance;
                }
                else
                {
                    Debug.LogWarning($"[PlayerModelController] ModelPrefab bị null ở mốc {i}");
                }
            }
        }

        private void OnMorphTierChanged(MorphTierChangedEvent evt)
        {
            if (evt.TierIndex < 0 || evt.TierIndex >= _instantiatedModels.Length)
                return;

            GameObject newModel = _instantiatedModels[evt.TierIndex];
            if (newModel == null) return;
            
            if (_currentActiveModel != null)
            {
                _currentActiveModel.SetActive(false);
            }
            
            newModel.SetActive(true);
            _currentActiveModel = newModel;
            
            UpdateAnimatorReference(newModel);
        }

        private void UpdateAnimatorReference(GameObject newModel)
        {
            Animator newModelAnimator = newModel.GetComponentInChildren<Animator>();
            if (newModelAnimator != null && playerAnimator != null)
            {
                playerAnimator.avatar = newModelAnimator.avatar;
            }
            else
            {
                Debug.LogWarning("[PlayerModelController] Không tìm thấy Animator trên Model mới hoặc PlayerAnimator chưa được gán.");
            }
        }
    }
}
