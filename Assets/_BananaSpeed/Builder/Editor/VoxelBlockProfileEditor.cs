using UnityEngine;
using UnityEditor;

namespace _BananaSpeed.Builder.Editor
{
    [CustomEditor(typeof(VoxelBlockProfile))]
    public class VoxelBlockProfileEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            VoxelBlockProfile profile = (VoxelBlockProfile)target;

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Preview Settings", EditorStyles.boldLabel);
            profile.previewMaterial = (Material)EditorGUILayout.ObjectField(new GUIContent("Preview Material"), profile.previewMaterial, typeof(Material), false);
            profile.atlasSize = EditorGUILayout.IntSlider("Atlas Size", profile.atlasSize, 1, 16);

            GUILayout.Space(10);
            profile.blockID = (byte)EditorGUILayout.IntField(new GUIContent("Block ID"), profile.blockID);
            
            GUILayout.Space(15);
            EditorGUILayout.LabelField("Face Textures Configuration", EditorStyles.boldLabel);

            Texture previewTex = null;
            if (profile.previewMaterial != null)
            {
                if (profile.previewMaterial.HasProperty("_MainTexture")) previewTex = profile.previewMaterial.GetTexture("_MainTexture");
                else if (profile.previewMaterial.HasProperty("_BaseMap")) previewTex = profile.previewMaterial.GetTexture("_BaseMap");
                else if (profile.previewMaterial.HasProperty("_MainTex")) previewTex = profile.previewMaterial.GetTexture("_MainTex");
            }

            if (previewTex == null)
            {
                EditorGUILayout.HelpBox("Missing Preview Material", MessageType.Warning);
            }

            profile.topTileID = DrawFaceField("Top", profile.topTileID, previewTex, profile.atlasSize);
            profile.bottomTileID = DrawFaceField("Bottom", profile.bottomTileID, previewTex, profile.atlasSize);
            profile.frontTileID = DrawFaceField("Front", profile.frontTileID, previewTex, profile.atlasSize);
            profile.backTileID = DrawFaceField("Back", profile.backTileID, previewTex, profile.atlasSize);
            profile.leftTileID = DrawFaceField("Left", profile.leftTileID, previewTex, profile.atlasSize);
            profile.rightTileID = DrawFaceField("Right", profile.rightTileID, previewTex, profile.atlasSize);

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(profile);
            }
        }

        private int DrawFaceField(string label, int tileID, Texture previewTex, int atlasSize)
        {
            GUILayout.BeginHorizontal();
            
            tileID = EditorGUILayout.IntField(label, tileID, GUILayout.Width(250));

            if (previewTex != null)
            {
                float tileSize = 1f / atlasSize;
                int tx = tileID % atlasSize;
                int ty = tileID / atlasSize;

                Rect texCoords = new Rect(tx * tileSize, ty * tileSize, tileSize, tileSize);
                
                Rect previewRect = GUILayoutUtility.GetRect(40, 40, GUILayout.Width(40), GUILayout.Height(40));
                
                EditorGUI.DrawRect(new Rect(previewRect.x - 2, previewRect.y - 2, previewRect.width + 4, previewRect.height + 4), Color.gray);
                GUI.DrawTextureWithTexCoords(previewRect, previewTex, texCoords);
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            
            return tileID;
        }
    }
}
