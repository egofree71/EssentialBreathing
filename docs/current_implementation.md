# SimpleBreathing — implémentation actuelle

## Objectif

Application Android très simple de respiration réalisée avec Godot 4.6.2 et C#.

L'application affiche une jauge verticale avec une boule qui monte pendant l'inspiration et descend pendant l'expiration. L'utilisateur peut modifier les durées d'inspiration et d'expiration, ainsi que le thème de couleurs.

## Structure du projet

```text
SimpleBreathing/
├── project.godot
├── SimpleBreathing.csproj
├── SimpleBreathing.sln
├── scenes/
│   └── Main.tscn
├── scripts/
│   ├── Main.cs
│   ├── BreathingGauge.cs
│   └── BreathingSettings.cs
└── docs/
    └── current_implementation.md
```

## Scène principale

### `scenes/Main.tscn`

Scène racine de type `Control`.

Elle utilise le script :

```text
res://scripts/Main.cs
```

La scène est volontairement presque vide dans l'éditeur Godot. L'interface est construite par code dans `Main.cs`, afin de pouvoir itérer rapidement sur la structure de l'application.

## Scripts

### `scripts/Main.cs`

Script principal de l'application.

Responsabilités :

- construire l'interface ;
- gérer l'écran principal ;
- gérer l'écran des réglages ;
- gérer le cycle inspiration / expiration ;
- démarrer et mettre en pause l'animation ;
- appliquer les réglages de durée ;
- appliquer les thèmes de couleurs.

### Écran principal

Au lancement, l'application affiche uniquement :

- la jauge verticale ;
- la boule de respiration ;
- un bouton de démarrage en bas à gauche ;
- un bouton de réglages en bas à droite.

Le titre `Simple Breathing` n'est plus affiché.

Les réglages ne sont plus visibles sur l'écran principal.

Le bouton de démarrage affiche :

```text
▶
```

Quand l'animation est en cours, le bouton devient :

```text
⏸
```

### Écran des réglages

Le bouton de réglages affiche :

```text
⚙
```

Il ouvre un écran séparé avec :

- un bouton retour ;
- les réglages de durée d'inspiration ;
- les réglages de durée d'expiration ;
- le choix du thème de couleurs ;
- un bouton pour réinitialiser le cycle.

Quand on ouvre l'écran des réglages, l'animation est mise en pause. Cela évite que la respiration continue en arrière-plan pendant que l'utilisateur modifie les paramètres.

### Cycle de respiration

Le cycle alterne entre deux phases :

```csharp
Inhale
Exhale
```

Pendant l'inspiration, la progression visuelle va de `0` à `1` : la boule monte.

Pendant l'expiration, la progression visuelle va de `1` à `0` : la boule descend.

Au lancement, l'application est arrêtée et la boule est en bas.

## `scripts/BreathingGauge.cs`

Contrôle personnalisé qui dessine :

- la jauge verticale ;
- le contour de la jauge ;
- la boule ;
- deux petits repères visuels en haut et en bas.

La méthode importante est :

```csharp
SetProgress(float progress)
```

Avec :

- `0.0` : boule en bas ;
- `1.0` : boule en haut.

## `scripts/BreathingSettings.cs`

Contient les paramètres de respiration :

```csharp
InhaleDuration
ExhaleDuration
```

Valeurs actuelles :

```text
Inspiration : 4.0 secondes
Expiration : 4.0 secondes
```

Les durées sont modifiables par pas de :

```text
0.5 seconde
```

Limites :

```text
Minimum : 1.0 seconde
Maximum : 20.0 secondes
```

## Thèmes de couleurs

Les thèmes disponibles sont définis dans `BreathingSettings.cs` :

```text
Océan nocturne
Forêt douce
Crépuscule
Clair minimal
```

Chaque thème définit :

- couleur de fond ;
- couleur du texte ;
- couleur de la jauge ;
- couleur du bord de jauge ;
- couleur de la boule.

## État actuel

Fonctionnel :

- projet Godot C# minimal ;
- scène principale configurée ;
- jauge dessinée par code ;
- boule animée ;
- démarrage / pause depuis l'écran principal ;
- écran de réglages séparé ;
- réglage des durées ;
- changement de thème ;
- documentation de l'implémentation actuelle.

À faire plus tard :

- améliorer l'esthétique générale ;
- sauvegarder les réglages localement ;
- ajouter éventuellement un court temps de pause entre inspiration et expiration ;
- tester sur Android ;
- préparer l'export Android.
