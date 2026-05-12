using Godot;
using System;

public sealed class BreathingSettings
{
    public const double DurationStep = 0.5;
    public const double MinimumDuration = 1.0;
    public const double MaximumDuration = 20.0;

    public double InhaleDuration { get; set; } = 4.0;
    public double ExhaleDuration { get; set; } = 6.0;

    public Color BackgroundColor { get; set; } = new(0.06f, 0.09f, 0.12f);
    public Color TextColor { get; set; } = new(0.92f, 0.96f, 1.0f);
    public Color GaugeColor { get; set; } = new(0.15f, 0.22f, 0.30f);
    public Color GaugeBorderColor { get; set; } = new(0.36f, 0.48f, 0.62f);
    public Color BallColor { get; set; } = new(0.43f, 0.78f, 0.98f);

    public static double ClampDuration(double value)
    {
        return Math.Clamp(value, MinimumDuration, MaximumDuration);
    }
}
