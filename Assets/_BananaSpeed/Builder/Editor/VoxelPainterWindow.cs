using UnityEngine;
using UnityEditor;

namespace _BananaSpeed.Builder.Editor
{
    public class VoxelPainterWindow : EditorWindow
    {
        private BS_VoxelChunk targetChunk;
        private VoxelBlockProfile currentBlockProfile;
        private bool isPaintingEnabled = false;

        [MenuItem("Tools/BananaSpeed/Voxel Painter")]
        public static void ShowWindow()
        {
            GetWindow<VoxelPainterWindow>("Voxel Painter");
        }

        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnGUI()
        {
            GUILayout.Label("Voxel Painter Settings", EditorStyles.boldLabel);

            targetChunk = (BS_VoxelChunk)EditorGUILayout.ObjectField("Target Chunk", targetChunk, typeof(BS_VoxelChunk), true);

            if (GUILayout.Button("Create New Empty Chunk"))
            {
                CreateEmptyChunk();
            }

            GUILayout.Space(10);

            EditorGUI.BeginChangeCheck();
            isPaintingEnabled = GUILayout.Toggle(isPaintingEnabled, "Enable Painting Mode", "Button", GUILayout.Height(30));
            if (EditorGUI.EndChangeCheck())
            {
                GUIUtility.hotControl = 0;
                SceneView.RepaintAll();
            }

            if (isPaintingEnabled)
            {
                EditorGUILayout.HelpBox("Left Click: Add Block\nShift + Left Click: Remove Block", MessageType.Info);
            }

            GUILayout.Space(10);
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Selected Profile:", GUILayout.Width(120));
            currentBlockProfile = (VoxelBlockProfile)EditorGUILayout.ObjectField(currentBlockProfile, typeof(VoxelBlockProfile), false, GUILayout.Width(150));
            
            DrawTexturePreview();
            GUILayout.EndHorizontal();
        }

        private void DrawTexturePreview()
        {
            if (targetChunk == null) return;
            
            MeshRenderer renderer = targetChunk.GetComponent<MeshRenderer>();
            if (renderer == null || renderer.sharedMaterial == null) return;
            
            Material mat = renderer.sharedMaterial;
            Texture previewTex = null;
            if (mat.HasProperty("_MainTexture")) previewTex = mat.GetTexture("_MainTexture");
            else if (mat.HasProperty("_BaseMap")) previewTex = mat.GetTexture("_BaseMap");
            else if (mat.HasProperty("_MainTex")) previewTex = mat.GetTexture("_MainTex");

            if (previewTex != null && currentBlockProfile != null)
            {
                int atlasSize = targetChunk.atlasSize;
                int tileID = currentBlockProfile.topTileID;
                float tileSize = 1f / atlasSize;
                int tx = tileID % atlasSize;
                int ty = tileID / atlasSize;

                Rect texCoords = new Rect(tx * tileSize, ty * tileSize, tileSize, tileSize);
                
                Rect previewRect = GUILayoutUtility.GetRect(40, 40, GUILayout.Width(40), GUILayout.Height(40));
                GUI.DrawTextureWithTexCoords(previewRect, previewTex, texCoords);
            }
        }

        private void CreateEmptyChunk()
        {
            GameObject obj = new GameObject("VoxelChunk_Painted");
            Undo.RegisterCreatedObjectUndo(obj, "Create Painted Chunk");
            targetChunk = obj.AddComponent<BS_VoxelChunk>();
            targetChunk.chunkWidth = 16;
            targetChunk.chunkHeight = 16;
            targetChunk.chunkLength = 16;
            targetChunk.atlasSize = 2;
            targetChunk.InitializeData();
            
            // Xóa đoạn tự lát nền vì ta chưa có Profile mặc định an toàn

            targetChunk.GenerateMesh();
            Selection.activeGameObject = obj;
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (!isPaintingEnabled || targetChunk == null) return;
            if (targetChunk.blocks == null || targetChunk.blocks.Length == 0) return;

            Event e = Event.current;
            int controlID = GUIUtility.GetControlID(FocusType.Passive);

            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            bool isHit = false;
            Vector3 hitPoint = Vector3.zero;
            Vector3 hitNormal = Vector3.zero;

            MeshCollider collider = targetChunk.GetComponent<MeshCollider>();
            if (collider != null)
            {
                RaycastHit hit;
                if (collider.Raycast(ray, out hit, 1000f))
                {
                    isHit = true;
                    hitPoint = hit.point;
                    hitNormal = hit.normal;
                }
            }

            if (isHit)
            {
                HandleUtility.AddDefaultControl(controlID);

                bool isRemoving = e.shift;
            
                Vector3 offsetPoint = hitPoint + hitNormal * (isRemoving ? -0.1f : 0.1f);
                
                Vector3 localPoint = targetChunk.transform.InverseTransformPoint(offsetPoint);
                int gridX = Mathf.FloorToInt(localPoint.x);
                int gridY = Mathf.FloorToInt(localPoint.y);
                int gridZ = Mathf.FloorToInt(localPoint.z);

                if (gridX >= 0 && gridX < targetChunk.chunkWidth &&
                    gridY >= 0 && gridY < targetChunk.chunkHeight &&
                    gridZ >= 0 && gridZ < targetChunk.chunkLength)
                {
                    Vector3 wireframeCenter = targetChunk.transform.TransformPoint(new Vector3(gridX + 0.5f, gridY + 0.5f, gridZ + 0.5f));
                    Handles.color = isRemoving ? Color.red : Color.green;
                    Handles.DrawWireCube(wireframeCenter, Vector3.one * 1.01f);

                    if (e.type == EventType.MouseDown && e.button == 0)
                    {
                        Undo.RecordObject(targetChunk, isRemoving ? "Remove Voxel" : "Add Voxel");
                        
                        byte newValue = 0;
                        if (!isRemoving && currentBlockProfile != null)
                        {
                            newValue = currentBlockProfile.blockID;
                            
                            if (!targetChunk.blockPalette.Contains(currentBlockProfile))
                            {
                                targetChunk.blockPalette.Add(currentBlockProfile);
                            }
                        }

                        targetChunk.SetBlock(gridX, gridY, gridZ, newValue);
                        targetChunk.GenerateMesh();
                        
                        e.Use(); 
                    }
                }
            }
            
            sceneView.Repaint();
        }
    }
}
