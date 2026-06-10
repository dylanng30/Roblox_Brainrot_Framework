using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum B_Rare
{

}

public enum B_Style
{

}

[CreateAssetMenu(fileName = "ColorPresetData", menuName = "UI/ColorPresetData", order = 0)]
public class ColorPresetData : ScriptableObject
{
    [Space(8)]
    [Title("Level Colors (per 50 levels)")]
    [InfoBox("Each entry is a HTML color code for a 50-level range (0-49, 50-99, 100-149, ...). Example: #FFFFFF or #FFFFFFFF.")]
    [ListDrawerSettings(Expanded = true)]
    public List<string> levelColorCodes = new List<string>();

    [LabelText("Default Level Color")]
    public string defaultLevelColorCode = "#FFFFFF";

    [Title("Rarity Gradients")]
    [InfoBox("Use Unity Gradient directly. GradientText applies it across the full TMP mesh.")]
    [TableList]
    public List<RarityGradientEntry> rarityGradients = new List<RarityGradientEntry>();

    [Space(8)]
    [Title("Style Gradients")]
    [TableList]
    public List<StyleGradientEntry> styleGradients = new List<StyleGradientEntry>();

    [Space(8)]
    [Title("Fallbacks")]
    [InlineProperty]
    public GradientPreset defaultGradientPreset = GradientPreset.FromColors(
        GradientTextMode.VerticalGradient,
        Color.white,
        Color.white);

    [LabelText("Default Style Color")]
    public string defaultStyleColorCode = "#FFFFFF";

    [PropertySpace(6)]
    [ShowInInspector, ReadOnly]
    [LabelText("Rarity Count")]
    private string RarityStatus => $"{rarityGradients?.Count ?? 0} / {Enum.GetNames(typeof(B_Rare)).Length} rarities assigned";

    [Button("Auto Fill Rarities (add missing)", ButtonSizes.Small)]
    private void AutoFillRarities()
    {
        foreach (B_Rare rarity in Enum.GetValues(typeof(B_Rare)))
        {
            if (FindRarityEntry(rarity) == null)
            {
                rarityGradients.Add(new RarityGradientEntry
                {
                    rarity = rarity,
                    preset = GetDefaultGradientCodes().Clone()
                });
            }
        }
    }

    [Button("Auto Fill Styles (add missing)", ButtonSizes.Small)]
    private void AutoFillStyles()
    {
        foreach (B_Style style in Enum.GetValues(typeof(B_Style)))
        {
            if (FindStyleEntry(style) == null)
            {
                Color color = ParseColor(defaultStyleColorCode, Color.white);
                styleGradients.Add(new StyleGradientEntry
                {
                    style = style,
                    preset = GradientPreset.FromColors(GradientTextMode.VerticalGradient, color, color)
                });
            }
        }
    }

    [Button("Fill Missing With Defaults", ButtonSizes.Small)]
    private void FillMissingWithDefaults()
    {
        AutoFillRarities();
        AutoFillStyles();
    }

    [Button("Clear All (rarity+style)", ButtonSizes.Small)]
    private void ClearAll()
    {
        rarityGradients.Clear();
        styleGradients.Clear();
    }

    public string GetColorCodeForLevel(int level)
    {
        if (levelColorCodes == null || levelColorCodes.Count == 0)
        {
            return defaultLevelColorCode;
        }

        int index = Mathf.FloorToInt(level / 50f);
        if (index < 0) index = 0;

        if (index >= levelColorCodes.Count)
        {
            return levelColorCodes[levelColorCodes.Count - 1];
        }

        return levelColorCodes[index];
    }

    public Color GetColorForLevel(int level)
    {
        return ParseColor(GetColorCodeForLevel(level), ParseColor(defaultLevelColorCode, Color.white));
    }

    public GradientPreset GetGradientCodesForRarity(B_Rare rarity)
    {
        RarityGradientEntry entry = FindRarityEntry(rarity);
        return entry != null && entry.preset != null
            ? entry.preset
            : GetDefaultGradientCodes();
    }

    public GradientPreset GetGradientCodesForStyle(B_Style style)
    {
        StyleGradientEntry entry = FindStyleEntry(style);
        if (entry != null && entry.preset != null)
        {
            return entry.preset;
        }

        Color color = ParseColor(defaultStyleColorCode, Color.white);
        return GradientPreset.FromColors(GradientTextMode.VerticalGradient, color, color);
    }

    public GradientPreset GetDefaultGradientCodes()
    {
        if (defaultGradientPreset == null)
        {
            defaultGradientPreset = GradientPreset.FromColors(GradientTextMode.VerticalGradient, Color.white, Color.white);
        }

        defaultGradientPreset.EnsureGradient();
        return defaultGradientPreset;
    }

    public Color GetColorForStyle(B_Style style)
    {
        GradientPreset preset = GetGradientCodesForStyle(style);
        return preset != null ? preset.GetStartColor(ParseColor(defaultStyleColorCode, Color.white)) : Color.white;
    }

    private RarityGradientEntry FindRarityEntry(B_Rare rarity)
    {
        if (rarityGradients == null)
        {
            return null;
        }

        for (int i = 0; i < rarityGradients.Count; i++)
        {
            RarityGradientEntry entry = rarityGradients[i];
            if (entry != null && entry.rarity == rarity)
            {
                return entry;
            }
        }

        return null;
    }

    private StyleGradientEntry FindStyleEntry(B_Style style)
    {
        if (styleGradients == null)
        {
            return null;
        }

        for (int i = 0; i < styleGradients.Count; i++)
        {
            StyleGradientEntry entry = styleGradients[i];
            if (entry != null && entry.style == style)
            {
                return entry;
            }
        }

        return null;
    }

    private void OnValidate()
    {
        if (levelColorCodes == null) levelColorCodes = new List<string>();
        if (rarityGradients == null) rarityGradients = new List<RarityGradientEntry>();
        if (styleGradients == null) styleGradients = new List<StyleGradientEntry>();
        if (defaultGradientPreset == null)
        {
            defaultGradientPreset = GradientPreset.FromColors(GradientTextMode.VerticalGradient, Color.white, Color.white);
        }

        defaultGradientPreset.EnsureGradient();
        for (int i = 0; i < rarityGradients.Count; i++)
        {
            rarityGradients[i]?.preset?.EnsureGradient();
        }

        for (int i = 0; i < styleGradients.Count; i++)
        {
            styleGradients[i]?.preset?.EnsureGradient();
        }
    }

    public static Color ParseColor(string colorCode, Color fallback)
    {
        if (string.IsNullOrWhiteSpace(colorCode))
        {
            return fallback;
        }

        string normalized = colorCode.Trim();
        if (!normalized.StartsWith("#", StringComparison.Ordinal))
        {
            normalized = "#" + normalized;
        }

        return ColorUtility.TryParseHtmlString(normalized, out Color parsed)
            ? parsed
            : fallback;
    }

    [Serializable]
    public class RarityGradientEntry
    {
        public B_Rare rarity;
        [InlineProperty]
        public GradientPreset preset = GradientPreset.FromColors(
            GradientTextMode.VerticalGradient,
            Color.white,
            Color.white);
    }

    [Serializable]
    public class StyleGradientEntry
    {
        public B_Style style;
        [InlineProperty]
        public GradientPreset preset = GradientPreset.FromColors(
            GradientTextMode.VerticalGradient,
            Color.white,
            Color.white);
    }

    [Serializable]
    [InlineProperty]
    public class GradientPreset
    {
        [LabelText("Mode")]
        public GradientTextMode mode = GradientTextMode.VerticalGradient;

        [LabelText("Gradient")]
        public Gradient gradient = GradientText.CreateDefaultGradient();

        public static GradientPreset FromColors(GradientTextMode mode, Color startColor, Color endColor)
        {
            return new GradientPreset
            {
                mode = mode,
                gradient = GradientText.CreateGradient(startColor, endColor)
            };
        }

        public static GradientPreset Rainbow()
        {
            return new GradientPreset
            {
                mode = GradientTextMode.Rainbow,
                gradient = GradientText.CreateRainbowGradient()
            };
        }

        public GradientPreset Clone()
        {
            EnsureGradient();
            Gradient copy = new Gradient();
            copy.SetKeys(gradient.colorKeys, gradient.alphaKeys);
            copy.mode = gradient.mode;
            return new GradientPreset
            {
                mode = mode,
                gradient = copy
            };
        }

        public void EnsureGradient()
        {
            if (gradient == null)
            {
                gradient = mode == GradientTextMode.Rainbow
                    ? GradientText.CreateRainbowGradient()
                    : GradientText.CreateDefaultGradient();
            }
        }

        public Color GetStartColor(Color fallback)
        {
            EnsureGradient();
            GradientColorKey[] keys = gradient.colorKeys;
            return keys != null && keys.Length > 0 ? keys[0].color : fallback;
        }

        public override string ToString()
        {
            return mode.ToString();
        }
    }
}