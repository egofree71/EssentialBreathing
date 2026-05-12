using Godot;
using System;

public partial class BreathingGauge : Control
{
    private float _progress;

    public Color GaugeColor { get; set; } = new(0.15f, 0.22f, 0.30f);
    public Color GaugeBorderColor { get; set; } = new(0.36f, 0.48f, 0.62f);
    public Color BallColor { get; set; } = new(0.43f, 0.78f, 0.98f);

    /// <summary>
    /// 0 = boule en bas, 1 = boule en haut.
    /// </summary>
    public void SetProgress(float progress)
    {
        _progress = Mathf.Clamp(progress, 0.0f, 1.0f);
        QueueRedraw();
    }

    public override void _Draw()
    {
        float width = Size.X;
        float height = Size.Y;

        if (width <= 1.0f || height <= 1.0f)
        {
            return;
        }

        float gaugeHeight = Math.Max(120.0f, height - 30.0f);
        float gaugeWidth = Mathf.Clamp(width * 0.22f, 54.0f, 96.0f);
        float left = (width - gaugeWidth) * 0.5f;
        float top = (height - gaugeHeight) * 0.5f;

        var gaugeRect = new Rect2(left, top, gaugeWidth, gaugeHeight);

        DrawRect(gaugeRect, GaugeColor, filled: true);
        DrawRect(gaugeRect, GaugeBorderColor, filled: false, width: 3.0f);

        float ballRadius = Mathf.Clamp(gaugeWidth * 0.36f, 18.0f, 34.0f);
        float usableTop = gaugeRect.Position.Y + ballRadius + 8.0f;
        float usableBottom = gaugeRect.Position.Y + gaugeRect.Size.Y - ballRadius - 8.0f;
        float ballY = Mathf.Lerp(usableBottom, usableTop, _progress);
        var ballCenter = new Vector2(width * 0.5f, ballY);

        DrawCircle(ballCenter, ballRadius, BallColor);

        // Petits repères visuels haut/bas pour rendre la jauge plus lisible.
        float markerLeft = gaugeRect.Position.X - 14.0f;
        float markerRight = gaugeRect.Position.X - 4.0f;
        DrawLine(new Vector2(markerLeft, usableTop), new Vector2(markerRight, usableTop), GaugeBorderColor, 2.0f);
        DrawLine(new Vector2(markerLeft, usableBottom), new Vector2(markerRight, usableBottom), GaugeBorderColor, 2.0f);
    }
}
