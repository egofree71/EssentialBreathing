using Godot;
using System;

public partial class Main : Control
{
    private enum BreathingPhase
    {
        Inhale,
        Exhale
    }

    private readonly BreathingSettings _settings = new();

    private BreathingGauge _gauge = null!;
    private Label _phaseLabel = null!;
    private Label _durationLabel = null!;
    private Label _themeLabel = null!;
    private Button _pauseButton = null!;
    private ColorRect _background = null!;

    private BreathingPhase _currentPhase = BreathingPhase.Inhale;
    private double _phaseElapsed;
    private bool _isPaused;

    public override void _Ready()
    {
        BuildInterface();
        ApplyColors();
        UpdateTexts();
        UpdateGauge();
    }

    public override void _Process(double delta)
    {
        if (_isPaused)
        {
            return;
        }

        _phaseElapsed += delta;
        double phaseDuration = GetCurrentPhaseDuration();

        if (_phaseElapsed >= phaseDuration)
        {
            _phaseElapsed -= phaseDuration;
            _currentPhase = _currentPhase == BreathingPhase.Inhale
                ? BreathingPhase.Exhale
                : BreathingPhase.Inhale;
        }

        UpdateTexts();
        UpdateGauge();
    }

    private void BuildInterface()
    {
        _background = new ColorRect
        {
            Name = "Background",
            MouseFilter = MouseFilterEnum.Ignore
        };
        AddChild(_background);
        FillParent(_background);

        var margin = new MarginContainer
        {
            Name = "MainMargin"
        };
        margin.AddThemeConstantOverride("margin_left", 24);
        margin.AddThemeConstantOverride("margin_top", 24);
        margin.AddThemeConstantOverride("margin_right", 24);
        margin.AddThemeConstantOverride("margin_bottom", 24);
        AddChild(margin);
        FillParent(margin);

        var mainColumn = new VBoxContainer
        {
            Name = "MainColumn",
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        mainColumn.AddThemeConstantOverride("separation", 14);
        margin.AddChild(mainColumn);

        var titleLabel = new Label
        {
            Name = "TitleLabel",
            Text = "Simple Breathing",
            HorizontalAlignment = Godot.HorizontalAlignment.Center
        };
        titleLabel.AddThemeFontSizeOverride("font_size", 30);
        mainColumn.AddChild(titleLabel);

        _phaseLabel = new Label
        {
            Name = "PhaseLabel",
            HorizontalAlignment = Godot.HorizontalAlignment.Center
        };
        _phaseLabel.AddThemeFontSizeOverride("font_size", 24);
        mainColumn.AddChild(_phaseLabel);

        _gauge = new BreathingGauge
        {
            Name = "BreathingGauge",
            CustomMinimumSize = new Vector2(260, 360),
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        mainColumn.AddChild(_gauge);

        _durationLabel = new Label
        {
            Name = "DurationLabel",
            HorizontalAlignment = Godot.HorizontalAlignment.Center
        };
        _durationLabel.AddThemeFontSizeOverride("font_size", 18);
        mainColumn.AddChild(_durationLabel);

        var controls = new VBoxContainer
        {
            Name = "Controls",
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        controls.AddThemeConstantOverride("separation", 8);
        mainColumn.AddChild(controls);

        var inhaleRow = CreateButtonRow(
            "Inspiration",
            () => AdjustInhaleDuration(-BreathingSettings.DurationStep),
            () => AdjustInhaleDuration(BreathingSettings.DurationStep));
        controls.AddChild(inhaleRow);

        var exhaleRow = CreateButtonRow(
            "Expiration",
            () => AdjustExhaleDuration(-BreathingSettings.DurationStep),
            () => AdjustExhaleDuration(BreathingSettings.DurationStep));
        controls.AddChild(exhaleRow);

        var themeRow = CreateThemeRow();
        controls.AddChild(themeRow);

        var actionRow = new HBoxContainer
        {
            Name = "ActionRow",
            Alignment = BoxContainer.AlignmentMode.Center
        };
        actionRow.AddThemeConstantOverride("separation", 10);
        controls.AddChild(actionRow);

        _pauseButton = CreateButton("Pause", TogglePause);
        actionRow.AddChild(_pauseButton);
        actionRow.AddChild(CreateButton("Reset", ResetCycle));
    }

    private HBoxContainer CreateButtonRow(string labelText, Action decrease, Action increase)
    {
        var row = new HBoxContainer
        {
            Name = labelText + "Row",
            Alignment = BoxContainer.AlignmentMode.Center
        };
        row.AddThemeConstantOverride("separation", 10);

        var label = new Label
        {
            Text = labelText,
            CustomMinimumSize = new Vector2(110, 0),
            VerticalAlignment = Godot.VerticalAlignment.Center
        };
        label.AddThemeFontSizeOverride("font_size", 16);

        row.AddChild(label);
        row.AddChild(CreateButton("−", decrease));
        row.AddChild(CreateButton("+", increase));

        return row;
    }

    private HBoxContainer CreateThemeRow()
    {
        var row = new HBoxContainer
        {
            Name = "ThemeRow",
            Alignment = BoxContainer.AlignmentMode.Center
        };
        row.AddThemeConstantOverride("separation", 10);

        var label = new Label
        {
            Text = "Couleurs",
            CustomMinimumSize = new Vector2(110, 0),
            VerticalAlignment = Godot.VerticalAlignment.Center
        };
        label.AddThemeFontSizeOverride("font_size", 16);
        row.AddChild(label);

        row.AddChild(CreateButton("‹", SelectPreviousTheme));

        _themeLabel = new Label
        {
            Name = "ThemeLabel",
            CustomMinimumSize = new Vector2(128, 0),
            HorizontalAlignment = Godot.HorizontalAlignment.Center,
            VerticalAlignment = Godot.VerticalAlignment.Center
        };
        _themeLabel.AddThemeFontSizeOverride("font_size", 15);
        row.AddChild(_themeLabel);

        row.AddChild(CreateButton("›", SelectNextTheme));

        return row;
    }

    private Button CreateButton(string text, Action onPressed)
    {
        var button = new Button
        {
            Text = text,
            CustomMinimumSize = new Vector2(82, 42)
        };
        button.Pressed += onPressed;
        return button;
    }

    private void AdjustInhaleDuration(double delta)
    {
        _settings.InhaleDuration = BreathingSettings.ClampDuration(_settings.InhaleDuration + delta);
        ResetCycle();
    }

    private void AdjustExhaleDuration(double delta)
    {
        _settings.ExhaleDuration = BreathingSettings.ClampDuration(_settings.ExhaleDuration + delta);
        ResetCycle();
    }

    private void SelectPreviousTheme()
    {
        _settings.MoveToPreviousTheme();
        ApplyColors();
        UpdateTexts();
    }

    private void SelectNextTheme()
    {
        _settings.MoveToNextTheme();
        ApplyColors();
        UpdateTexts();
    }

    private void TogglePause()
    {
        _isPaused = !_isPaused;
        _pauseButton.Text = _isPaused ? "Reprendre" : "Pause";
        UpdateTexts();
    }

    private void ResetCycle()
    {
        _currentPhase = BreathingPhase.Inhale;
        _phaseElapsed = 0.0;
        UpdateTexts();
        UpdateGauge();
    }

    private double GetCurrentPhaseDuration()
    {
        return _currentPhase == BreathingPhase.Inhale
            ? _settings.InhaleDuration
            : _settings.ExhaleDuration;
    }

    private void UpdateTexts()
    {
        string phaseName = _currentPhase == BreathingPhase.Inhale ? "Inspiration" : "Expiration";
        double remaining = Math.Max(0.0, GetCurrentPhaseDuration() - _phaseElapsed);
        string pauseSuffix = _isPaused ? " — pause" : string.Empty;

        _phaseLabel.Text = $"{phaseName}{pauseSuffix} · {remaining:0.0}s";
        _durationLabel.Text = $"Inspiration : {_settings.InhaleDuration:0.0}s   |   Expiration : {_settings.ExhaleDuration:0.0}s";
        _themeLabel.Text = _settings.CurrentThemeName;
    }

    private void UpdateGauge()
    {
        double phaseProgress = _phaseElapsed / GetCurrentPhaseDuration();
        phaseProgress = Math.Clamp(phaseProgress, 0.0, 1.0);

        float visualProgress = _currentPhase == BreathingPhase.Inhale
            ? (float)phaseProgress
            : 1.0f - (float)phaseProgress;

        _gauge.SetProgress(visualProgress);
    }

    private void ApplyColors()
    {
        _background.Color = _settings.BackgroundColor;
        _gauge.GaugeColor = _settings.GaugeColor;
        _gauge.GaugeBorderColor = _settings.GaugeBorderColor;
        _gauge.BallColor = _settings.BallColor;
        _gauge.QueueRedraw();

        ApplyTextColorRecursive(this, _settings.TextColor);
    }

    private static void ApplyTextColorRecursive(Node node, Color color)
    {
        if (node is Label label)
        {
            label.AddThemeColorOverride("font_color", color);
        }
        else if (node is Button button)
        {
            button.AddThemeColorOverride("font_color", color);
            button.AddThemeColorOverride("font_hover_color", color);
            button.AddThemeColorOverride("font_pressed_color", color);
        }

        foreach (Node child in node.GetChildren())
        {
            ApplyTextColorRecursive(child, color);
        }
    }

    private static void FillParent(Control control)
    {
        control.AnchorLeft = 0.0f;
        control.AnchorTop = 0.0f;
        control.AnchorRight = 1.0f;
        control.AnchorBottom = 1.0f;
        control.OffsetLeft = 0.0f;
        control.OffsetTop = 0.0f;
        control.OffsetRight = 0.0f;
        control.OffsetBottom = 0.0f;
    }
}
