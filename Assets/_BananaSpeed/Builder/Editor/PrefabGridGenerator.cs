using UnityEngine;
using UnityEditor;

namespace BananaSpeed.Tools
{
    public class PrefabGridGenerator : EditorWindow
    {
        private GameObject prefabToInstantiate;
        private int widthX = 3;
        private int heightY = 3;
        private int lengthZ = 3;
        private Vector3 spacing = Vector3.one;

        [MenuItem("Tools/BananaSpeed/3D Grid Generator")]
        public static void ShowWindow()
        {
            GetWindow<PrefabGridGenerator>("3D Grid Generator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Prefab Grid Settings", EditorStyles.boldLabel);

            prefabToInstantiate = (GameObject)EditorGUILayout.ObjectField("Prefab", prefabToInstantiate, typeof(GameObject), false);

            widthX = EditorGUILayout.IntSlider("Width (X)", widthX, 1, 100);
            heightY = EditorGUILayout.IntSlider("Height (Y)", heightY, 1, 100);
            lengthZ = EditorGUILayout.IntSlider("Length (Z)", lengthZ, 1, 100);

            spacing = EditorGUILayout.Vector3Field("Spacing", spacing);

            GUILayout.Space(20);

            if (GUILayout.Button("Generate Grid", GUILayout.Height(40)))
            {
                GenerateGrid();
            }
        }

        private void GenerateGrid()
        {
            if (prefabToInstantiate == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign a prefab to instantiate.", "OK");
                return;
            }

            string gridName = $"Grid_3D_{System.DateTime.Now:yyyyMMdd_HHmmss}";
            GameObject gridParent = new GameObject(gridName);
            Undo.RegisterCreatedObjectUndo(gridParent, "Create 3D Grid");

            float offsetX = (widthX - 1) * spacing.x / 2f;
            float offsetY = -spacing.y / 2f;
            float offsetZ = (lengthZ - 1) * spacing.z / 2f;

            for (int y = 0; y < heightY; y++)
            {
                GameObject layerParent = new GameObject($"Layer_{y}");
                Undo.RegisterCreatedObjectUndo(layerParent, "Create Layer Parent");
                layerParent.transform.SetParent(gridParent.transform);

                layerParent.transform.localPosition = Vector3.zero;

                for (int x = 0; x < widthX; x++)
                {
                    for (int z = 0; z < lengthZ; z++)
                    {
                        Vector3 position = new Vector3(
                            (x * spacing.x) - offsetX, 
                            (y * spacing.y) - offsetY, 
                            (z * spacing.z) - offsetZ
                        );
                        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefabToInstantiate);
                        
                        Undo.RegisterCreatedObjectUndo(instance, "Create Grid Item");

                        instance.transform.position = position;
                        instance.transform.SetParent(layerParent.transform);
                        
                        string itemName = $"{prefabToInstantiate.name}_X{x}_Z{z}_Layer{y}";
                        instance.name = itemName;
                    }
                }
            }

            Selection.activeGameObject = gridParent;
            Debug.Log($"Successfully generated 3D grid with {widthX * heightY * lengthZ} objects.");
        }
    }
}
