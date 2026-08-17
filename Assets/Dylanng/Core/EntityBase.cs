using UnityEngine;

namespace Dylanng
{
    public class EntityBase : MonoBehaviourBase
    {
        public Transform Parent => transform.parent;
        public Transform RootParent => transform.root;


        #region --- ACTIVE ---
        public bool IsActive() => gameObject.activeInHierarchy;

        public virtual void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
        #endregion

        public virtual void SetParent(Transform parent, bool isPosRotReseted = false)
        {
            transform.SetParent(parent);

            if (isPosRotReseted)
            {
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
        }

        #region --- POSITION ---
        public virtual void SetWorldPosition(Vector3 targetPosition)
        {
            transform.position = targetPosition;
        }

        public virtual void SetLocalPosition(Vector3 targetPosition)
        {
            transform.localPosition = targetPosition;
        }
        #endregion

        #region --- ROTATION ---
        public virtual void SetWorldRotation(Quaternion targetRotation)
        {
            transform.rotation = targetRotation;
        }

        public virtual void SetLocalRotation(Quaternion targetRotation)
        {
            transform.localRotation = targetRotation;
        }
        #endregion

        #region --- SCALE ---
        public virtual void SetWorldScale(Vector3 targetScale)
        {
            if (transform.parent != null)
            {
                Vector3 parentScale = transform.parent.lossyScale;
                transform.localScale = new Vector3(
                    Mathf.Approximately(parentScale.x, 0f) ? 0f : targetScale.x / parentScale.x,
                    Mathf.Approximately(parentScale.y, 0f) ? 0f : targetScale.y / parentScale.y,
                    Mathf.Approximately(parentScale.z, 0f) ? 0f : targetScale.z / parentScale.z
                );
            }
            else
            {
                transform.localScale = targetScale;
            }
        }

        public virtual void SetLocalScale(Vector3 targetScale)
        {
            transform.localScale = targetScale;
        }
        #endregion
    }
}