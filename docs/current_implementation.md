# SimpleBreathing — Current Implementation

## Goal

SimpleBreathing is a very simple Android breathing app built with **Godot 4.6.2** and **C#**.

The app displays a vertical gauge with a ball that moves upward during inhalation and downward during exhalation.

The current design goal is to keep the mobile interface calm and minimal:

- the main screen is focused on the breathing animation;
- settings are separated from the breathing screen;
- controls disappear during the breathing session;
- the breathing movement should feel smooth and natural rather than mechanical.

## Project structure

```text
SimpleBreathing/
├── project.godot
├── SimpleBreathing.csproj
├── SimpleBreathing.sln
├── README.md
├── assets/
│   └── icons/
│       └── floppy-disk.svg
├── scenes/
│   └── Main.tscn
├── scripts/
│   ├── Main.cs
│   ├── BreathingGauge.cs
│   ├── BreathingSettings.cs
│   └── FloppyIcon.cs
└── docs/
    └── current_implementation.md
```

## Project configuration

### `SimpleBreathing.csproj`

The project uses:

```xml
<Project Sdk="Godot.NET.Sdk/4.6.2">
```

Target framework:

```text
net8.0
```

### `project.godot`

The main scene is:

```text
res://scenes/Main.tscn
```

Current viewport configuration:

```text
width  : 480
height : 854
orientation : portrait
stretch mode : canvas_items
stretch aspect : expand
```

This matches a phone-oriented vertical layout.

## Main scene

### `scenes/Main.tscn`

Root scene of type:

```text
Control
```

It uses:

```text
res://scripts/Main.cs
```

The scene itself is intentionally minimal. Most UI elements are created in code by `Main.cs`, which makes it easier to iterate quickly on mobile layout and behavior.

## Scripts

## `scripts/Main.cs`

Main application controller.

Responsibilities:

- build the runtime UI;
- create the main breathing screen;
- create the settings screen;
- manage screen switching;
- manage the breathing session state;
- manage the inhalation/exhalation cycle;
- apply breathing durations;
- apply color themes to the main screen;
- keep the settings screen readable with a neutral black-and-white style.

## Main screen

The main screen is intentionally minimal.

Visible at startup:

```text
[⚙]

[gauge + ball]

[▶]
```

Details:

- the `Simple Breathing` title is no longer displayed;
- the settings button is in the top-left corner;
- the start button is centered near the bottom;
- the gauge is positioned slightly higher than the exact vertical center;
- the bottom button area is kept stable so the gauge does not resize when buttons appear or disappear.

### Start

The start button uses:

```text
▶
```

When pressed:

- the breathing session starts;
- the settings button disappears;
- the start button disappears;
- only the gauge and moving ball remain visible.

### Running session

During a running session:

```text
[gauge + moving ball]
```

The screen is visually clean. No buttons are visible.

A transparent full-screen touch area catches taps/clicks.

When the user touches/clicks the screen:

- the session is paused;
- the stop button appears to the left;
- the resume button appears centered under the gauge.

### Paused session

Visible controls:

```text
[■]     [▶]
```

The `▶` resume button stays in the same centered position as the original start button.

The `■` stop button appears to its left.

### Stop

The stop button uses:

```text
■
```

When pressed:

- the session stops;
- the breathing cycle resets;
- the ball returns to the bottom;
- the app returns to the initial main screen.

## Settings screen

The settings screen is separate from the main screen.

It uses a fixed neutral style:

- black background;
- white text;
- white button borders;
- very dark button fill;
- white SVG save icon.

The settings screen no longer previews the selected theme colors directly. This is intentional: some themes made buttons hard to read when the settings screen used theme colors.

Current layout:

```text
[←] Réglages

Respiration

Inspiration    [−]  4.0s  [+]
Expiration     [−]  4.0s  [+]

Thèmes

[‹]  Océan  [›]

[save icon]
```

### Settings screen buttons

The duration buttons use:

```text
−
+
```

The theme selection buttons use:

```text
‹
›
```

These button symbols and the main labels have been enlarged for better readability on mobile.

### Save button

The save button uses an SVG floppy disk icon:

```text
assets/icons/floppy-disk.svg
```

It is displayed through:

```text
scripts/FloppyIcon.cs
```

Important: settings are currently applied immediately in memory. The save button currently acts as an explicit validation button that returns to the main screen.

Settings are **not persisted to disk yet**.

## Breathing cycle

The cycle has two phases:

```csharp
Inhale
Exhale
```

During inhalation:

```text
visual progress: 0 -> 1
ball movement  : bottom -> top
```

During exhalation:

```text
visual progress: 1 -> 0
ball movement  : top -> bottom
```

When the app starts:

- no session is running;
- the ball is visible at the bottom;
- the current phase is `Inhale`;
- elapsed phase time is `0`.

## Eased movement

The ball no longer moves linearly.

Instead, the phase progress is passed through an easing function:

```csharp
EaseInOut(double value)
```

The current easing uses a smootherstep formula:

```csharp
return t * t * t * (t * (t * 6.0 - 15.0) + 10.0);
```

This gives a more natural breathing feeling:

- the ball starts slowly;
- it accelerates through the middle;
- it slows down near the top or bottom;
- there is no hard pause at either end.

This replaced the earlier idea of adding explicit pause durations after inhalation and exhalation, which felt too mechanical.

## `scripts/BreathingGauge.cs`

Custom `Control` that draws the gauge and the ball procedurally.

No image asset is used for the gauge or ball.

### Gauge shape

The gauge is a vertical capsule:

- one central rectangle;
- one circle at the top;
- one circle at the bottom.

The gauge has:

- rounded ends;
- no visible border;
- no side markers;
- no tick marks.

The gauge was reduced by roughly 10% compared with the earlier version, to feel lighter on a phone screen.

### Ball

The ball is drawn with:

```csharp
DrawCircle(...)
```

It uses the full gauge width:

```text
ball radius = gauge width / 2
```

This means there is no side margin between the ball and the gauge.

The ball remains fully inside the capsule at the top and bottom.

Progress convention:

```text
0.0 : ball at the bottom
1.0 : ball at the top
```

The public method is:

```csharp
SetProgress(float progress)
```

The progress value is clamped between `0.0` and `1.0`.

## `scripts/BreathingSettings.cs`

Contains in-memory breathing parameters and color themes.

### Durations

Default values:

```text
Inhalation : 4.0 seconds
Exhalation : 4.0 seconds
```

Step:

```text
0.5 second
```

Limits:

```text
Minimum : 1.0 second
Maximum : 20.0 seconds
```

The clamp method is:

```csharp
ClampDuration(double value)
```

### Themes

Current themes:

```text
Océan
Jungle
Volcanique
Ciel
```

Each theme defines:

- background color;
- text color;
- gauge color;
- gauge border color;
- ball color.

Note: `GaugeBorderColor` still exists in the theme data, but the gauge no longer draws a visible border. It is kept for now to avoid unnecessary churn in the theme structure.

### Theme notes

- `Océan` is a saturated blue theme.
- `Jungle` is based on bright green jungle-like colors.
- `Volcanique` is based on dark red, lava orange, and yellow colors.
- `Ciel` uses light sky-like colors, including light blue and white.

## `scripts/FloppyIcon.cs`

Custom `Control` used inside the settings save button.

Responsibilities:

- load the SVG texture;
- draw it inside the button;
- tint it with `IconColor`;
- provide a simple fallback drawing if the SVG is not imported yet.

The icon path is:

```csharp
private const string IconPath = "res://assets/icons/floppy-disk.svg";
```

The SVG was sourced from SVG Repo and was marked as public domain / CC0 on the source site.

The SVG fill is stored as white so it can be tinted clearly at draw time.

## Assets

### `assets/icons/floppy-disk.svg`

Save icon used in the settings screen.

License note:

```text
Source: SVG Repo
License: Public Domain / CC0 1.0 Universal
```

This is kept here as a practical trace for future publishing or maintenance.

## Current validated state

Implemented and validated:

- minimal Godot C# project;
- mobile portrait project configuration;
- main scene configured;
- runtime UI built in code;
- main breathing screen separated from settings screen;
- title removed from the main screen;
- settings button moved to the top-left;
- centered start/resume button;
- stop button appearing left of the resume button when paused;
- hidden controls during a running session;
- tap/click anywhere to pause while running;
- vertical rounded capsule-shaped gauge;
- gauge without visible border;
- gauge without side markers;
- gauge reduced by about 10%;
- ball using the full gauge width;
- eased breathing movement with slowdown near the top and bottom;
- editable inhalation/exhalation durations;
- theme switching;
- themes renamed and recolored;
- neutral black-and-white settings screen;
- larger settings text and button symbols;
- SVG save icon;
- implementation documentation.

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

### SVG import

If the save icon does not appear after copying the package, close and reopen the Godot project so the SVG asset can be imported.

## Later improvements

Possible next steps:

- persist settings locally;
- improve the main screen theme colors further;
- test button sizes on a real Android phone;
- test the breathing rhythm on an actual phone screen;
- prepare Android export;
- decide whether the save button should actually save persistent settings or simply be renamed/treated as a confirmation button;
- optionally add haptic feedback or sound, if it remains calm and unobtrusive.
