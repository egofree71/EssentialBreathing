using Godot;
using System;

/// <summary>
/// In-memory settings for one breathing session.
/// </summary>
/// <remarks>
/// Settings are not persisted yet. They are centralized here so saving/loading can
/// later be added without spreading duration and theme state through the UI code.
/// </remarks>
public sealed class BreathingSettings
{
    public const double DurationStep = 0.5;
    public const double MinimumDuration = 1.0;
    public const double MaximumDuration = 20.0;

    public double InhaleDuration { get; set; } = 4.0;
    public double ExhaleDuration { get; set; } = 4.0;

    public int CurrentThemeIndex { get; private set; }

    public Color BackgroundColor { get; private set; }
    public Color TextColor { get; private set; }
    public Color GaugeColor { get; private set; }
    public Color GaugeBorderColor { get; private set; }
    public Color BallColor { get; private set; }

    // Static theme list used by the settings screen. Each theme currently defines
    // all app colors directly; there is no separate Godot Theme resource yet.
    public static readonly BreathingTheme[] Themes =
    {
        new(
            "Océan",
            new Color(0.00f, 0.07f, 0.18f),
            new Color(0.86f, 0.96f, 1.00f),
            new Color(0.00f, 0.24f, 0.48f),
            new Color(0.00f, 0.55f, 0.95f),
            new Color(0.00f, 0.78f, 1.00f)),

        new(
            "Jungle",
            new Color(0.02f, 0.13f, 0.07f),
            new Color(0.90f, 1.00f, 0.88f),
            new Color(0.04f, 0.34f, 0.13f),
            new Color(0.17f, 0.66f, 0.24f),
            new Color(0.42f, 0.95f, 0.25f)),

        new(
            "Volcanique",
            new Color(0.16f, 0.04f, 0.02f),
            new Color(1.00f, 0.90f, 0.70f),
            new Color(0.42f, 0.05f, 0.02f),
            new Color(0.85f, 0.20f, 0.04f),
            new Color(1.00f, 0.42f, 0.02f)),

        new(
            "Ciel",
            new Color(0.78f, 0.92f, 1.00f),
            new Color(0.04f, 0.16f, 0.32f),
            new Color(0.96f, 0.99f, 1.00f),
            new Color(0.42f, 0.76f, 1.00f),
            new Color(0.18f, 0.62f, 1.00f))
    };

    public BreathingSettings()
    {
        ApplyTheme(0);
    }

    public string CurrentThemeName => Themes[CurrentThemeIndex].Name;

    public void MoveToNextTheme()
    {
        ApplyTheme(CurrentThemeIndex + 1);
    }

    public void MoveToPreviousTheme()
    {
        ApplyTheme(CurrentThemeIndex - 1);
    }

    public void ApplyTheme(int themeIndex)
    {
        CurrentThemeIndex = WrapIndex(themeIndex, Themes.Length);
        BreathingTheme theme = Themes[CurrentThemeIndex];

        BackgroundColor = theme.BackgroundColor;
        TextColor = theme.TextColor;
        GaugeColor = theme.GaugeColor;
        GaugeBorderColor = theme.GaugeBorderColor;
        BallColor = theme.BallColor;
    }

    public static double ClampDuration(double value)
    {
        return Math.Clamp(value, MinimumDuration, MaximumDuration);
    }

    private static int WrapIndex(int value, int length)
    {
        if (length <= 0)
        {
            return 0;
        }

        // C# '%' keeps the sign of the left operand, so negative indexes need the
        // extra correction to wrap backward through the theme list.
        int result = value % length;
        return result < 0 ? result + length : result;
    }
}

/// <summary>
/// Immutable color palette used by the breathing UI.
/// </summary>
public sealed class BreathingTheme
{
    public BreathingTheme(
        string name,
        Color backgroundColor,
        Color textColor,
        Color gaugeColor,
        Color gaugeBorderColor,
        Color ballColor)
    {
        Name = name;
        BackgroundColor = backgroundColor;
        TextColor = textColor;
        GaugeColor = gaugeColor;
        GaugeBorderColor = gaugeBorderColor;
        BallColor = ballColor;
    }

    public string Name { get; }
    public Color BackgroundColor { get; }
    public Color TextColor { get; }
    public Color GaugeColor { get; }
    public Color GaugeBorderColor { get; }
    public Color BallColor { get; }
}
