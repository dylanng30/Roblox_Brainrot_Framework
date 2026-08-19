using UnityEngine;

namespace _BananaSpeed.Builder
{
    public class VoxelInteraction : MonoBehaviour
    {
        [Header("Explosion Settings")]
        public float explosionRadius = 3f;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ShootRayAndExplode();
            }
        }

        private void ShootRayAndExplode()
        {
            Ray ray;
            if (Camera.main != null)
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            }
            else
            {
                Debug.LogWarning("Camera.main not found!");
                return;
            }

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 explosionCenter = hit.point;

                Collider[] hitColliders = Physics.OverlapSphere(explosionCenter, explosionRadius);
                foreach (Collider col in hitColliders)
                {
                    BS_VoxelChunk chunk = col.GetComponent<BS_VoxelChunk>();
                    if (chunk != null)
                    {
                        chunk.ExplodeAt(explosionCenter, explosionRadius);
                    }
                }
            }
        }
    }
}
