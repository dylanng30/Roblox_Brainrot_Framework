using UnityEngine;

namespace _BananaSpeed.Builder
{
    [CreateAssetMenu(fileName = "NewBlockProfile", menuName = "BananaSpeed/Voxel Block Profile")]
    public class VoxelBlockProfile : ScriptableObject
    {
        [Header("Preview Settings")]
        public Material previewMaterial;
        public int atlasSize = 2;

        [Space(10)]
        public byte blockID = 1;
        
        [Header("Face Texture IDs")]
        public int topTileID = 0;
        public int bottomTileID = 0;
        public int frontTileID = 0;
        public int backTileID = 0;
        public int leftTileID = 0;
        public int rightTileID = 0;

        public int GetTileIDForFace(int faceIndex)
        {
            switch (faceIndex)
            {
                case 0: return backTileID;
                case 1: return frontTileID;
                case 2: return topTileID;
                case 3: return bottomTileID;
                case 4: return leftTileID;
                case 5: return rightTileID;
                default: return 0;
            }
        }
    }
}
