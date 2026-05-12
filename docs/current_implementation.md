# SimpleBreathing — Current Implementation

## Goal

SimpleBreathing is a very simple Android breathing app built with Godot 4.6.2 and C#.

The app displays a vertical gauge with a ball that moves upward during inhalation and downward during exhalation.

The current goal is to keep the mobile interface very minimal: when the app starts, the user only sees the gauge, the ball, a start/pause button, and a button to open the settings screen.

## Project structure

```text
SimpleBreathing/
├── project.godot
├── SimpleBreathing.csproj
├── SimpleBreathing.sln
├── README.md
├── scenes/
│   └── Main.tscn
├── scripts/
│   ├── Main.cs
│   ├── BreathingGauge.cs
│   └── BreathingSettings.cs
└── docs/
    └── current_implementation.md
```

## Main scene

### `scenes/Main.tscn`

Root scene of type `Control`.

It uses the following script:

```text
res://scripts/Main.cs
```

The scene is intentionally kept very simple in the Godot editor. The interface is built in code inside `Main.cs`, which makes it easier to iterate quickly on the application structure.

The main scene is configured in `project.godot`:

```text
res://scenes/Main.tscn
```

## Scripts

## `scripts/Main.cs`

Main application script.

Responsibilities:

- build the interface in code;
- manage the main screen;
- manage the settings screen;
- manage the inhalation/exhalation cycle;
- start and pause the animation;
- reset the cycle;
- apply duration settings;
- apply color themes.

### Main screen

When the app starts, it only displays:

- the vertical gauge;
- the breathing ball;
- a start/pause button at the bottom left;
- a settings button at the bottom right.

The `Simple Breathing` title is no longer displayed.

The settings are no longer visible on the main screen.

The start button displays:

```text
▶
```

When the animation is running, the button becomes:

```text
⏸
```

The start/pause icons have been enlarged to improve readability on mobile.

The settings button displays:

```text
⚙
```

### Settings screen

The `⚙` button opens a separate settings screen with:

- a back button;
- inhalation duration controls;
- exhalation duration controls;
- color theme selection;
- a button to reset the breathing cycle.

When the settings screen is opened, the animation is paused.

This prevents the breathing cycle from continuing in the background while the user changes settings.

### Breathing cycle

The cycle alternates between two phases:

```csharp
Inhale
Exhale
```

During inhalation, the visual progress goes from `0` to `1`: the ball moves upward.

During exhalation, the visual progress goes from `1` to `0`: the ball moves downward.

When the app starts, it is stopped and the ball is at the bottom.

The ball progress is sent to the gauge with:

```csharp
_gauge.SetProgress(visualProgress);
```

## `scripts/BreathingGauge.cs`

Custom control that draws the gauge and the ball.

The gauge is drawn in code, without external textures.

### Gauge shape

The gauge now has a vertical capsule shape:

- central rectangle;
- rounded upper half-circle;
- rounded lower half-circle.

Technically, it is drawn with:

- one filled central rectangle;
- one circle at the top;
- one circle at the bottom.

The different-colored border around the gauge has been removed.

The two small visual markers on the left, at the top and bottom, have also been removed.

### Ball

The ball is drawn with `DrawCircle`.

It takes up more space inside the gauge than it did at the beginning of the project, with smaller side margins.

Its vertical position depends on the progress value:

```csharp
SetProgress(float progress)
```

With:

```text
0.0 : ball at the bottom
1.0 : ball at the top
```

The progress value is clamped between `0.0` and `1.0` with `Mathf.Clamp`.

## `scripts/BreathingSettings.cs`

Contains breathing parameters and color themes.

### Durations

Current values:

```text
Inhalation : 4.0 seconds
Exhalation : 4.0 seconds
```

Durations can be changed by steps of:

```text
0.5 second
```

Limits:

```text
Minimum : 1.0 second
Maximum : 20.0 seconds
```

The method used to keep durations within limits is:

```csharp
ClampDuration(double value)
```

### Color themes

Available themes:

```text
Océan nocturne
Forêt douce
Crépuscule
Clair minimal
```

Each theme currently defines:

- background color;
- text color;
- gauge color;
- gauge border color;
- ball color.

Note: the gauge border color still exists in the theme structure, but the gauge currently does not draw a visible border.

## Current interface

### Main screen

Visible elements:

```text
[gauge + ball]

[▶ or ⏸]                              [⚙]
```

The main screen is intentionally minimal, calm, and suitable for phone use.

### Settings screen

Visible elements:

```text
[←] Settings

Breathing
Inhalation    [−]  4.0s  [+]
Exhalation    [−]  4.0s  [+]

Colors
[‹]  Theme name  [›]

[Reset cycle]
```

## Current validated state

Implemented and validated:

- minimal Godot C# project;
- configured main scene;
- interface built in code;
- main screen separated from the settings screen;
- vertical rounded capsule-shaped gauge;
- gauge without visible border;
- gauge without side markers;
- larger ball inside the gauge;
- inhalation/exhalation animation;
- start/pause button;
- settings button;
- separate settings screen;
- duration controls;
- theme switching;
- current implementation documentation.

## Technical points to watch

### C# compilation in Godot

Sometimes Godot may fail to recompile the C# assembly correctly after replacing files.

When in doubt, run this from the project root:

```bash
dotnet clean
dotnet build
```

If the issue persists, close Godot and delete the generated folders:

```bat
rmdir /s /q bin
rmdir /s /q obj
rmdir /s /q .godot\mono
```

Then reopen the project in Godot, build it, and run it again.

## Later improvements

Possible next steps:

- further improve the overall visual design;
- save settings locally;
- optionally add a short pause between inhalation and exhalation;
- test on Android;
- prepare Android export;
- check button and icon sizes on a real phone.
