# SimpleBreathing — Current Implementation

## Goal

SimpleBreathing is a very simple Android breathing app built with **Godot 4.6.2** and **C#**.

The app displays a vertical gauge with a ball that moves upward during inhalation and downward during exhalation.

The current design goal is to keep the mobile interface calm and minimal:

- the main screen is focused on the breathing animation;
- settings are separated from the breathing screen;
- controls disappear during the breathing session;
- the breathing movement should feel smooth and natural rather than mechanical;
- the gauge should feel visually centered and stable;
- settings changes should not affect the main screen until explicitly saved.

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
│   ├── SettingsStorage.cs
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

Root scene type:

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
- manage settings draft values;
- apply settings only when the user saves;
- persist saved settings through `user://settings.cfg`;
- manage the breathing session state;
- manage the inhalation/exhalation cycle;
- manage total session duration;
- update the pause progress display;
- show the completion fade overlay when a session ends naturally;
- apply color themes to the main screen;
- keep the settings screen readable with a neutral black-and-white style.

The controller methods are now documented with XML comments. Short inline comments are also used for non-trivial layout and state-management details.

### Important runtime state

`Main.cs` keeps two categories of settings:

```text
_settings
```

Active settings used by the main breathing screen and the running session.

```text
_draftInhaleDuration
_draftExhaleDuration
_draftSessionDurationMinutes
_draftThemeIndex
```

Draft settings edited on the settings screen.

The draft values are copied to `_settings` only when the user presses the save icon.

When the save icon is pressed, the active settings are also written to:

```text
user://settings.cfg
```

This avoids a confusing behavior where pressing the back arrow after editing settings still changed the main screen.

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
- the gauge is vertically centered in the screen;
- the top pause-progress area and bottom control area reserve matching heights, so the gauge remains centered and stable;
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
- the resume button appears centered under the gauge;
- the pause progress display appears above the gauge.

### Paused session

Visible controls:

```text
[■]     [▶]
```

The `▶` resume button stays in the same centered position as the original start button.

The `■` stop button appears to its left.

Above the gauge, the app displays:

```text
elapsed time / total session duration
[session progress bar]
```

The progress bar:

- is about three quarters of the base viewport width;
- has a semi-transparent white background;
- fills with white according to the elapsed session duration;
- represents progress through the whole breathing session, not only the current inhale/exhale cycle.

Example:

```text
0:42 / 5:00
```

### Stop

The stop button uses:

```text
■
```

When pressed:

- the session stops;
- the total session timer resets;
- the breathing cycle resets;
- the ball returns to the bottom;
- the app returns to the initial main screen.

### Automatic end

When the configured session duration is reached:

- the session stops automatically;
- a full-screen overlay fades the current view to black;
- the app resets the session while the screen is black;
- the text `Session terminée` fades in;
- the message stays visible for about 2 seconds;
- the overlay fades out;
- the app returns to the initial main screen.

The fade transition avoids a sudden visual jump from the final breathing frame back to the start state.

### Completion overlay

The completion message is handled by a full-screen overlay created in `Main.cs`.

Overlay elements:

```text
CompletionOverlay
CompletionFade
CompletionLabel
```

The label text is:

```text
Session terminée
```

Animation sequence:

```text
fade to black       : about 0.45 s
text fade in        : about 0.35 s
message hold        : 2.0 s
text + black fade out: about 0.45 s
```

The app resets the session while the screen is black, so the user does not see an abrupt jump.


## Settings screen

The settings screen is separate from the main screen.

It uses a fixed neutral style:

- black background;
- white text;
- white button borders;
- very dark button fill;
- white SVG save icon.

The settings screen does not preview the selected theme colors directly. This is intentional: some themes made buttons hard to read when the settings screen used theme colors.

Current layout:

```text
[←] Réglages

Respiration

Inspiration        [−]  4.0s  [+]
Expiration         [−]  4.0s  [+]
Durée de séance     5 min
[slider]

Thèmes

[‹]  Océan  [›]

[save icon]
```

### Settings editing model

The settings screen uses a draft model:

- changing a value updates only the draft state;
- pressing `←` cancels the draft changes and returns to the main screen;
- pressing the save icon applies the draft changes to the active settings and returns to the main screen.

Affected values:

- inhalation duration;
- exhalation duration;
- total session duration;
- selected theme.

### Duration buttons

The duration buttons use:

```text
−
+
```

The values are edited in steps of:

```text
0.5 second
```

### Session duration slider

The total session duration is edited with a slider.

Default value:

```text
5 minutes
```

Limits:

```text
Minimum : 1 minute
Maximum : 60 minutes
```

Step:

```text
1 minute
```

The slider intentionally edits only whole minutes, not seconds.

### Theme buttons

The theme selection buttons use:

```text
‹
›
```

The selected theme name is displayed between the buttons.

The theme change is applied to the main screen only after pressing the save icon.

### Save button

The save button uses an SVG floppy disk icon:

```text
assets/icons/floppy-disk.svg
```

It is displayed through:

```text
scripts/FloppyIcon.cs
```

Important: settings are applied and persisted only when the save button is pressed.

The save button:

- copies draft values to the active settings;
- writes the active settings to `user://settings.cfg`;
- returns to the main screen.

If the user presses `←`, draft changes are discarded and no file is written.

## Breathing session and cycle

There are two timing layers:

1. the total session timer;
2. the repeated inhale/exhale breathing cycle.

### Session timer

The session timer is stored in seconds internally:

```text
_sessionElapsed
```

The configured session duration comes from:

```csharp
_settings.SessionDurationSeconds
```

When:

```text
_sessionElapsed >= SessionDurationSeconds
```

The app calls `StopBreathingSession()`.

### Breathing cycle

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
- elapsed phase time is `0`;
- elapsed session time is `0`.

## Eased movement

The ball does not move linearly.

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

The gauge was reduced by roughly 10% compared with an earlier version, to feel lighter on a phone screen.

The main screen now positions the gauge so it is visually centered in the screen.

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

Contains breathing parameters and color themes. Values are kept in memory during runtime and persisted by `SettingsStorage` when the user saves settings.

### Inhalation and exhalation durations

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

### Session duration

Default value:

```text
5 minutes
```

Step:

```text
1 minute
```

Limits:

```text
Minimum : 1 minute
Maximum : 60 minutes
```

The active session duration is exposed as seconds through:

```csharp
SessionDurationSeconds
```

The clamp method is:

```csharp
ClampSessionDurationMinutes(int value)
```

### Themes

Current themes:

```text
Océan
Jungle
Volcan
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
- `Volcan` is based on dark red, lava orange, and yellow colors.
- `Ciel` uses light sky-like colors, including light blue and white.

## `scripts/SettingsStorage.cs`

Static helper responsible for loading and saving settings in Godot's user data folder.

Storage path:

```text
user://settings.cfg
```

This is the portable Godot path for app-specific user data. On Android it resolves to the application's private writable storage, so no external storage permission is needed.

The file is a Godot `ConfigFile` with a `breathing` section.

Saved keys:

```text
inhale_duration
exhale_duration
session_duration_minutes
theme_index
```

Loading behavior:

- called during `Main._Ready()` before building the UI;
- missing file means defaults are kept;
- invalid or missing values fall back to the current default values;
- loaded values are clamped through `BreathingSettings` before use.

Saving behavior:

- called only from `SaveSettings()` after the user presses the save icon;
- canceled draft changes are not saved.

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
- pause progress display above the gauge;
- progress bar based on total session duration;
- soft completion fade when session duration is reached;
- vertical rounded capsule-shaped gauge;
- gauge vertically centered on the main screen;
- gauge without visible border;
- gauge without side markers;
- gauge reduced by about 10%;
- ball using the full gauge width;
- eased breathing movement with slowdown near the top and bottom;
- editable inhalation/exhalation durations;
- editable total session duration with a whole-minute slider;
- theme switching;
- settings draft/cancel/save behavior;
- settings persistence in `user://settings.cfg`;
- themes renamed and recolored;
- neutral black-and-white settings screen;
- larger settings text and button symbols;
- SVG save icon;
- XML comments added to `Main.cs` methods;
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

### Settings persistence

Settings are stored in:

```text
user://settings.cfg
```

On Android, this is app-specific private storage. The app should keep the settings after closing and reopening it, as long as the app data is not cleared or the app is not uninstalled.

For testing:

1. change a setting;
2. press the save icon;
3. close the app completely;
4. reopen it;
5. verify that the saved values are restored.

## Later improvements

Possible next steps:

- test settings persistence on a real Android phone;
- test button sizes on a real Android phone;
- test the breathing rhythm on an actual phone screen;
- prepare Android export;
- optionally refine the completion fade timing after testing on a phone;
- optionally add haptic feedback or sound, if it remains calm and unobtrusive.
