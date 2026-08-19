using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace _BananaSpeed.Builder.Editor
{
    public class VoxelGridGenerator : EditorWindow
    {
        [System.Serializable]
        public class LayerSetup
        {
            public int startY;
            public int endY;
            public VoxelBlockProfile blockProfile;

            public LayerSetup(int startY, int endY, VoxelBlockProfile blockProfile)
            {
                this.startY = startY;
                this.endY = endY;
                this.blockProfile = blockProfile;
            }
        }

        private Material chunkMaterial;
        private int atlasSize = 2;
        private int widthX = 16;
        private int heightY = 16;
        private int lengthZ = 16;

        private List<LayerSetup> layers = new List<LayerSetup>();
        
        [MenuItem("Tools/BananaSpeed/Voxel Grid Generator")]
        public static void ShowWindow()
        {
            GetWindow<VoxelGridGenerator>("Voxel Grid Generator");
        }

        private void OnEnable()
        {
            if (layers.Count == 0)
            {
                layers.Add(new LayerSetup(0, 5, null));
            }
        }

        private void OnGUI()
        {
            GUILayout.Label("Voxel Chunk Settings", EditorStyles.boldLabel);

            chunkMaterial = (Material)EditorGUILayout.ObjectField("Chunk Material", chunkMaterial, typeof(Material), false);
            atlasSize = EditorGUILayout.IntSlider("Atlas Size", atlasSize, 1, 16);

            GUILayout.Space(10);
            widthX = EditorGUILayout.IntSlider("Chunk Width (X)", widthX, 1, 100);
            heightY = EditorGUILayout.IntSlider("Chunk Height (Y)", heightY, 1, 100);
            lengthZ = EditorGUILayout.IntSlider("Chunk Length (Z)", lengthZ, 1, 100);

            GUILayout.Space(20);
            GUILayout.Label("Layer Configuration", EditorStyles.boldLabel);

            for (int i = 0; i < layers.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"Layer {i + 1}:", GUILayout.Width(60));
                
                GUILayout.Label("Y from", GUILayout.Width(40));
                layers[i].startY = EditorGUILayout.IntField(layers[i].startY, GUILayout.Width(40));
                
                GUILayout.Label("to", GUILayout.Width(20));
                layers[i].endY = EditorGUILayout.IntField(layers[i].endY, GUILayout.Width(40));
                
                GUILayout.Label("-> Profile:", GUILayout.Width(60));
                layers[i].blockProfile = (VoxelBlockProfile)EditorGUILayout.ObjectField(layers[i].blockProfile, typeof(VoxelBlockProfile), false, GUILayout.Width(120));

                Texture previewTex = null;
                if (chunkMaterial != null)
                {
                    if (chunkMaterial.HasProperty("_MainTexture")) previewTex = chunkMaterial.GetTexture("_MainTexture");
                    else if (chunkMaterial.HasProperty("_BaseMap")) previewTex = chunkMaterial.GetTexture("_BaseMap");
                    else if (chunkMaterial.HasProperty("_MainTex")) previewTex = chunkMaterial.GetTexture("_MainTex");
                }

                if (previewTex != null && layers[i].blockProfile != null)
                {
                    int tileID = layers[i].blockProfile.topTileID;
                    float tileSize = 1f / atlasSize;
                    int tx = tileID % atlasSize;
                    int ty = tileID / atlasSize;

                    Rect texCoords = new Rect(tx * tileSize, ty * tileSize, tileSize, tileSize);
                    
                    Rect previewRect = GUILayoutUtility.GetRect(40, 40, GUILayout.Width(40), GUILayout.Height(40));
                    GUI.DrawTextureWithTexCoords(previewRect, previewTex, texCoords);
                }
                else
                {
                    GUILayout.Space(44);
                }

                if (GUILayout.Button("X", GUILayout.Width(25)))
                {
                    layers.RemoveAt(i);
                    i--;
                }
                GUILayout.EndHorizontal();
            }

            if (GUILayout.Button("+ Add Layer", GUILayout.Height(25)))
            {
                int nextY = layers.Count > 0 ? layers[layers.Count - 1].endY + 1 : 0;
                layers.Add(new LayerSetup(nextY, nextY, null));
            }

            GUILayout.Space(20);

            if (GUILayout.Button("Generate Voxel Chunk", GUILayout.Height(40)))
            {
                GenerateChunk();
            }
        }

        private void GenerateChunk()
        {
            string chunkName = $"VoxelChunk_{System.DateTime.Now:yyyyMMdd_HHmmss}";
            GameObject chunkObj = new GameObject(chunkName);
            Undo.RegisterCreatedObjectUndo(chunkObj, "Create Voxel Chunk");

            BS_VoxelChunk chunk = chunkObj.AddComponent<BS_VoxelChunk>();
            chunk.chunkWidth = widthX;
            chunk.chunkHeight = heightY;
            chunk.chunkLength = lengthZ;
            chunk.atlasSize = atlasSize;
            
            if (chunkMaterial != null)
            {
                chunkObj.GetComponent<MeshRenderer>().sharedMaterial = chunkMaterial;
            }

            List<VoxelBlockProfile> uniqueProfiles = new List<VoxelBlockProfile>();
            foreach (var layer in layers)
            {
                if (layer.blockProfile != null && !uniqueProfiles.Contains(layer.blockProfile))
                {
                    uniqueProfiles.Add(layer.blockProfile);
                }
            }
            chunk.blockPalette = uniqueProfiles;

            chunk.InitializeData();

            for (int y = 0; y < heightY; y++)
            {
                byte currentBlockID = 0;
                foreach (var layer in layers)
                {
                    if (y >= layer.startY && y <= layer.endY && layer.blockProfile != null)
                    {
                        currentBlockID = layer.blockProfile.blockID;
                    }
                }

                if (currentBlockID > 0)
                {
                    for (int x = 0; x < widthX; x++)
                    {
                        for (int z = 0; z < lengthZ; z++)
                        {
                            chunk.SetBlock(x, y, z, currentBlockID);
                        }
                    }
                }
            }

            chunk.GenerateMesh();

            Selection.activeGameObject = chunkObj;
            Debug.Log($"Successfully generated Voxel Chunk with dimensions {widthX}x{heightY}x{lengthZ}.");
        }
    }
}
