# F-Droid build files for Essential Breathing

This folder contains an Android export preset intended for F-Droid/source builds.
It deliberately contains no signing key and no password.

F-Droid will copy this file to the project root as `export_presets.cfg`, patch the
release template path to the Godot Android template it builds from source, and then
run Godot headless to export the APK.

Keep this file in sync when you change:

- Android package ID;
- version/name;
- version/code;
- Android architecture settings;
- Android export options.
