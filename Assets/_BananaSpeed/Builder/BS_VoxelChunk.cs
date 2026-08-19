using System.Collections.Generic;
using UnityEngine;

namespace _BananaSpeed.Builder
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class BS_VoxelChunk : MonoBehaviour
    {
        [Header("Chunk Dimensions")]
        public int chunkWidth = 16;
        public int chunkHeight = 16;
        public int chunkLength = 16;

        [Header("Material Settings")]
        public int atlasSize = 2;

        [Header("Block Palette")]
        public List<VoxelBlockProfile> blockPalette = new List<VoxelBlockProfile>();

        [HideInInspector]
        public byte[] blocks;

        private List<Vector3> vertices = new List<Vector3>();
        private List<int> triangles = new List<int>();
        private List<Vector2> uvs = new List<Vector2>();

        private int vertexIndex = 0;
        private MeshFilter meshFilter;
        private MeshCollider meshCollider;

        public void InitializeData()
        {
            blocks = new byte[chunkWidth * chunkHeight * chunkLength];
        }

        public byte GetBlock(int x, int y, int z)
        {
            if (x < 0 || x >= chunkWidth || y < 0 || y >= chunkHeight || z < 0 || z >= chunkLength) return 0;
            return blocks[x + chunkWidth * (y + chunkHeight * z)];
        }

        public void SetBlock(int x, int y, int z, byte blockID)
        {
            if (x < 0 || x >= chunkWidth || y < 0 || y >= chunkHeight || z < 0 || z >= chunkLength) return;
            blocks[x + chunkWidth * (y + chunkHeight * z)] = blockID;
        }

        public VoxelBlockProfile GetProfile(byte blockID)
        {
            foreach (var profile in blockPalette)
            {
                if (profile != null && profile.blockID == blockID)
                {
                    return profile;
                }
            }
            return null;
        }

        public void GenerateMesh()
        {
            if (blocks == null || blocks.Length == 0) return;

            vertices.Clear();
            triangles.Clear();
            uvs.Clear();
            vertexIndex = 0;

            for (int y = 0; y < chunkHeight; y++)
            {
                for (int x = 0; x < chunkWidth; x++)
                {
                    for (int z = 0; z < chunkLength; z++)
                    {
                        if (GetBlock(x, y, z) != 0)
                        {
                            UpdateBlockFaces(x, y, z);
                        }
                    }
                }
            }

            CreateMesh();
        }

        public void ExplodeAt(Vector3 worldPosition, float radius)
        {
            if (blocks == null || blocks.Length == 0) return;

            bool hasChanged = false;
            float radiusSquared = radius * radius;

            for (int y = 0; y < chunkHeight; y++)
            {
                for (int x = 0; x < chunkWidth; x++)
                {
                    for (int z = 0; z < chunkLength; z++)
                    {
                        if (GetBlock(x, y, z) != 0)
                        {
                            Vector3 blockWorldPos = transform.position + new Vector3(x, y, z) + new Vector3(0.5f, 0.5f, 0.5f);

                            if ((blockWorldPos - worldPosition).sqrMagnitude <= radiusSquared)
                            {
                                SetBlock(x, y, z, 0);
                                hasChanged = true;
                            }
                        }
                    }
                }
            }

            if (hasChanged)
            {
                GenerateMesh();
            }
        }

        private void UpdateBlockFaces(int x, int y, int z)
        {
            byte blockID = GetBlock(x, y, z);
            VoxelBlockProfile profile = GetProfile(blockID);

            for (int p = 0; p < 6; p++)
            {
                if (CheckFace(x, y, z, p))
                {
                    Vector3 pos = new Vector3(x, y, z);

                    vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 0]]);
                    vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 1]]);
                    vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 2]]);
                    vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 3]]);

                    triangles.Add(vertexIndex);
                    triangles.Add(vertexIndex + 1);
                    triangles.Add(vertexIndex + 2);
                    triangles.Add(vertexIndex + 2);
                    triangles.Add(vertexIndex + 1);
                    triangles.Add(vertexIndex + 3);

                    int tileID = 0;
                    if (profile != null)
                    {
                        tileID = profile.GetTileIDForFace(p);
                    }
                    else
                    {
                        tileID = blockID - 1;
                    }

                    Vector2[] faceUVs = VoxelData.GetUVsForTile(tileID, atlasSize);

                    uvs.Add(faceUVs[0]);
                    uvs.Add(faceUVs[1]);
                    uvs.Add(faceUVs[2]);
                    uvs.Add(faceUVs[3]);

                    vertexIndex += 4;
                }
            }
        }

        private bool CheckFace(int x, int y, int z, int p)
        {
            Vector3Int check = new Vector3Int(x, y, z) + VoxelData.faceChecks[p];

            if (check.x < 0 || check.x >= chunkWidth ||
                check.y < 0 || check.y >= chunkHeight ||
                check.z < 0 || check.z >= chunkLength)
            {
                return true;
            }

            return GetBlock(check.x, check.y, check.z) == 0;
        }

        private void CreateMesh()
        {
            if (meshFilter == null) meshFilter = GetComponent<MeshFilter>();
            if (meshCollider == null) meshCollider = GetComponent<MeshCollider>();

            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.RecalculateNormals();

            meshFilter.sharedMesh = mesh;
            meshCollider.sharedMesh = mesh;
        }
    }
}
