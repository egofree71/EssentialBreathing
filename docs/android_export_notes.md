# Android export notes

This document summarizes the Android-specific setup used by SimpleBreathing.

It replaces the temporary root-level `README_A_LIRE.md` file, which can be removed from the repository once this file has been added under `docs/`.

## Purpose

SimpleBreathing is built with Godot 4.6.2 and C#. The app is primarily designed for Android, but the project should also remain convenient to run from the Godot editor on desktop.

The current setup keeps the project compatible with the editor while also allowing Android export with the C# Android export templates.

## .NET target framework setup

Godot 4.6.2 Android C# export expects `net9.0`.

The project keeps `net8.0` as the default target framework, and switches to `net9.0` only when exporting to Android:

```xml
<TargetFramework>net8.0</TargetFramework>
<TargetFramework Condition=" '$(GodotTargetPlatform)' == 'android' ">net9.0</TargetFramework>
```

This means:

```text
Desktop / Godot editor: net8.0
Android export:         net9.0
```

If Android export reports an error such as:

```text
C# project targets 'net8.0' but the export template only supports 'net9.0'.
```

then check that the conditional `net9.0` line is still present in `SimpleBreathing.csproj`.

## Local prerequisites

For Android export with C#, the local machine must have the .NET 9 SDK installed.

Check installed SDKs with:

```bash
dotnet --list-sdks
```

At least one `9.x.x` SDK should be listed.

The Android SDK, Android export templates, and Godot Android export settings must also be configured locally.

## Android system bars

The app should keep the Android navigation bar visible.

Recommended Android export preset settings:

```text
Project > Export > Android > Options > Screen

Immersive Mode: Off
Edge to Edge: On
```

Expected behavior:

- the Android navigation bar remains visible;
- the navigation bar area can show the app background;
- UI controls are not placed under the navigation bar;
- the app uses safe area handling in `Main.cs` to keep controls above system bars.

## Boot splash and Godot logo

The Godot boot splash image can be disabled or customized in the project settings:

```text
Project > Project Settings > Application > Boot Splash

Show Image = Off
Minimum Display Time = 0
BG Color = color close to the app background
```

Android may also briefly show the app launcher icon during startup. If the Godot icon is still visible at launch, configure custom Android launcher icons in the Android export preset.

## Files affected by the Android setup

The Android/.NET and system bar setup currently affects these files:

```text
SimpleBreathing.csproj
scripts/Main.cs
docs/current_implementation.md
docs/android_export_notes.md
README.md
```

The following files are intentionally not included in the documentation package because they are local or generated configuration files:

```text
project.godot
export_presets.cfg
SimpleBreathing.sln
bin/
obj/
.godot/
```

## Notes for future updates

When changing Android behavior, check these points again:

- the conditional `net9.0` Android target in `SimpleBreathing.csproj`;
- the Android export preset screen settings;
- safe area handling in `scripts/Main.cs`;
- `docs/current_implementation.md`;
- this file.
