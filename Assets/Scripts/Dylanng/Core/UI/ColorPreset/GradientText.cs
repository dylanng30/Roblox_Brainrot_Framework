using TMPro;
using UnityEngine;

public enum GradientTextMode
{
    [InspectorName("Vertical Gradient")]
    VerticalGradient,
    [InspectorName("Hor Gradient")]
    HorGradient,
    [InspectorName("Rainbow")]
    Rainbow
}

[ExecuteAlways]
[DisallowMultipleComponent]
[RequireComponent(typeof(TMP_Text))]
public class GradientText : MonoBehaviour
{
    [SerializeField] private TMP_Text targetText;
    [SerializeField] private bool applyOnEnable = true;
    [SerializeField] private GradientTextMode mode = GradientTextMode.HorGradient;
    [SerializeField] private Gradient gradient = CreateDefaultGradient();

    private void Reset()
    {
        targetText = GetComponent<TMP_Text>();
        gradient = CreateDefaultGradient();
    }

    private void Awake()
    {
        ResolveTargetText();
    }

    private void OnEnable()
    {
        ResolveTargetText();
        if (applyOnEnable)
        {
            Apply();
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        ResolveTargetText();
        if (gradient == null)
        {
            gradient = CreateDefaultGradient();
        }

        Apply();
    }
#endif

    public void SetGradient(ColorPresetData.GradientPreset preset)
    {
        if (preset == null)
        {
            return;
        }

        mode = preset.mode;
        gradient = CopyGradient(preset.gradient ?? CreateDefaultGradient());
        Apply();
    }

    public void SetGradient(Gradient newGradient)
    {
        gradient = CopyGradient(newGradient ?? CreateDefaultGradient());
        Apply();
    }

    public void SetMode(GradientTextMode newMode)
    {
        mode = newMode;
        Apply();
    }

    public void SetText(string value)
    {
        ResolveTargetText();
        if (targetText == null)
        {
            return;
        }

        targetText.text = value;
        Apply();
    }

    public void Apply()
    {
        ResolveTargetText();
        ApplyRaw(targetText, mode, gradient);
    }

    public static void ApplyTo(TMP_Text text, ColorPresetData.GradientPreset preset)
    {
        if (text == null || preset == null)
        {
            return;
        }

        GradientText gradientText = text.GetComponent<GradientText>();
        if (gradientText != null)
        {
            gradientText.SetGradient(preset);
            return;
        }

        ApplyRaw(text, preset.mode, preset.gradient);
    }

    private static void ApplyRaw(TMP_Text text, GradientTextMode gradientMode, Gradient sourceGradient)
    {
        if (text == null)
        {
            return;
        }

        Gradient resolvedGradient = ResolveGradient(gradientMode, sourceGradient);
        text.enableVertexGradient = false;
        text.colorGradientPreset = null;
        text.ForceMeshUpdate();

        TMP_TextInfo textInfo = text.textInfo;
        if (textInfo == null)
        {
            return;
        }

        int charCount = textInfo.characterCount;
        if (charCount == 0 || textInfo.characterInfo == null || textInfo.meshInfo == null)
        {
            return;
        }

        float min = float.MaxValue;
        float max = float.MinValue;

        for (int i = 0; i < charCount; i++)
        {
            TMP_CharacterInfo character = textInfo.characterInfo[i];
            if (!character.isVisible)
            {
                continue;
            }

            int meshIndex = character.materialReferenceIndex;
            int vertexIndex = character.vertexIndex;
            if (!TryGetMeshData(textInfo, meshIndex, vertexIndex, out Vector3[] vertices, out _))
            {
                continue;
            }

            for (int j = 0; j < 4; j++)
            {
                float value = GetAxisValue(vertices[vertexIndex + j], gradientMode);
                min = Mathf.Min(min, value);
                max = Mathf.Max(max, value);
            }
        }

        float size = max - min;
        if (size <= 0.001f)
        {
            return;
        }

        for (int i = 0; i < charCount; i++)
        {
            TMP_CharacterInfo character = textInfo.characterInfo[i];
            if (!character.isVisible)
            {
                continue;
            }

            int meshIndex = character.materialReferenceIndex;
            int vertexIndex = character.vertexIndex;
            if (!TryGetMeshData(textInfo, meshIndex, vertexIndex, out Vector3[] vertices, out Color32[] colors))
            {
                continue;
            }

            for (int j = 0; j < 4; j++)
            {
                float value = GetAxisValue(vertices[vertexIndex + j], gradientMode);
                float t = Mathf.InverseLerp(min, max, value);
                colors[vertexIndex + j] = resolvedGradient.Evaluate(t);
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
            if (meshInfo.mesh == null || meshInfo.colors32 == null)
            {
                continue;
            }

            meshInfo.mesh.colors32 = meshInfo.colors32;
            text.UpdateGeometry(meshInfo.mesh, i);
        }
    }

    private static bool TryGetMeshData(
        TMP_TextInfo textInfo,
        int meshIndex,
        int vertexIndex,
        out Vector3[] vertices,
        out Color32[] colors)
    {
        vertices = null;
        colors = null;

        if (meshIndex < 0 || meshIndex >= textInfo.meshInfo.Length)
        {
            return false;
        }

        TMP_MeshInfo meshInfo = textInfo.meshInfo[meshIndex];
        vertices = meshInfo.vertices;
        colors = meshInfo.colors32;

        return vertices != null &&
               colors != null &&
               vertexIndex >= 0 &&
               vertexIndex + 3 < vertices.Length &&
               vertexIndex + 3 < colors.Length;
    }

    private static float GetAxisValue(Vector3 vertex, GradientTextMode gradientMode)
    {
        return gradientMode == GradientTextMode.VerticalGradient ? vertex.y : vertex.x;
    }

    private static Gradient ResolveGradient(GradientTextMode gradientMode, Gradient sourceGradient)
    {
        return gradientMode == GradientTextMode.Rainbow
            ? CreateRainbowGradient()
            : sourceGradient ?? CreateDefaultGradient();
    }

    public static Gradient CreateDefaultGradient()
    {
        return CreateGradient(Color.white, Color.white);
    }

    public static Gradient CreateRainbowGradient()
    {
        Gradient rainbow = new Gradient();
        rainbow.SetKeys(
            new[]
            {
                new GradientColorKey(ColorPresetData.ParseColor("#0041FF", Color.blue), 0f),
                new GradientColorKey(ColorPresetData.ParseColor("#04FF00", Color.green), 0.33f),
                new GradientColorKey(ColorPresetData.ParseColor("#FF00B9", Color.magenta), 0.66f),
                new GradientColorKey(ColorPresetData.ParseColor("#FF4800", Color.red), 1f)
            },
            new[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            });
        return rainbow;
    }

    public static Gradient CreateGradient(Color startColor, Color endColor)
    {
        Gradient newGradient = new Gradient();
        newGradient.SetKeys(
            new[]
            {
                new GradientColorKey(startColor, 0f),
                new GradientColorKey(endColor, 1f)
            },
            new[]
            {
                new GradientAlphaKey(startColor.a, 0f),
                new GradientAlphaKey(endColor.a, 1f)
            });
        return newGradient;
    }

    private static Gradient CopyGradient(Gradient source)
    {
        Gradient copy = new Gradient();
        copy.SetKeys(source.colorKeys, source.alphaKeys);
        copy.mode = source.mode;
        return copy;
    }

    private void ResolveTargetText()
    {
        if (targetText == null)
        {
            targetText = GetComponent<TMP_Text>();
        }
    }
}