# Package Android / .NET 9 / barres système

Ce package contient uniquement les fichiers nécessaires pour reprendre les corrections faites dans la discussion, sans réécraser tout le projet.

## Fichiers inclus

- `SimpleBreathing.csproj`
- `scripts/Main.cs`
- `docs/current_implementation.md`

## Pourquoi `SimpleBreathing.csproj` est inclus cette fois

Godot 4.6.2 utilise les templates d'export Android .NET avec `net9.0`.
Si le projet reste uniquement en `net8.0`, l'export Android peut afficher :

```text
C# project targets 'net8.0' but the export template only supports 'net9.0'.
```

Le fichier `.csproj` inclus garde `net8.0` par défaut pour l'éditeur / PC, mais passe automatiquement en `net9.0` seulement pour l'export Android :

```xml
<TargetFramework>net8.0</TargetFramework>
<TargetFramework Condition=" '$(GodotTargetPlatform)' == 'android' ">net9.0</TargetFramework>
```

## Réglages Godot à vérifier

Dans le preset Android :

```text
Project > Export > Android > Options > Screen

Immersive Mode : Off
Edge to Edge   : On
```

Dans le projet :

```text
Project > Project Settings > Application > Boot Splash

Show Image = Off
Minimum Display Time = 0
BG Color = couleur proche du fond de l'app
```

## Pré-requis local

Pour exporter vers Android avec C#, il faut aussi que le SDK .NET 9 soit installé sur la machine.
Tu peux vérifier avec :

```bash
dotnet --list-sdks
```

Il doit y avoir au moins une ligne en `9.x.x`.

## Installation du package

Extraire le contenu de ce zip à la racine du dépôt local `SimpleBreathing`, en acceptant le remplacement des fichiers existants.

Ce package ne contient pas :

- `project.godot`
- `export_presets.cfg`
- `SimpleBreathing.sln`
- les dossiers `bin`, `obj` ou `.godot`
