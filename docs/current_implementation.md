# SimpleBreathing — implémentation actuelle

## Objectif

SimpleBreathing est une application Android très simple de respiration réalisée avec Godot 4.6.2 et C#.

L'application affiche une jauge verticale avec une boule qui monte pendant l'inspiration et descend pendant l'expiration.

L'objectif actuel est de garder une interface mobile très sobre : au lancement, l'utilisateur voit uniquement la jauge, la boule, un bouton pour démarrer ou mettre en pause, et un bouton pour ouvrir les réglages.

## Structure du projet

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

## Scène principale

### `scenes/Main.tscn`

Scène racine de type `Control`.

Elle utilise le script :

```text
res://scripts/Main.cs
```

La scène reste volontairement très simple dans l'éditeur Godot. L'interface est construite par code dans `Main.cs`, ce qui permet d'itérer rapidement sur la structure de l'application.

La scène principale est configurée dans `project.godot` :

```text
res://scenes/Main.tscn
```

## Scripts

## `scripts/Main.cs`

Script principal de l'application.

Responsabilités :

- construire l'interface par code ;
- gérer l'écran principal ;
- gérer l'écran des réglages ;
- gérer le cycle inspiration / expiration ;
- démarrer et mettre en pause l'animation ;
- réinitialiser le cycle ;
- appliquer les réglages de durée ;
- appliquer les thèmes de couleurs.

### Écran principal

Au lancement, l'application affiche uniquement :

- la jauge verticale ;
- la boule de respiration ;
- un bouton démarrer / pause en bas à gauche ;
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

Les icônes du bouton démarrer / pause ont été agrandies pour être plus lisibles sur mobile.

Le bouton de réglages affiche :

```text
⚙
```

### Écran des réglages

Le bouton `⚙` ouvre un écran séparé avec :

- un bouton retour ;
- les réglages de durée d'inspiration ;
- les réglages de durée d'expiration ;
- le choix du thème de couleurs ;
- un bouton pour réinitialiser le cycle.

Quand on ouvre l'écran des réglages, l'animation est mise en pause.

Cela évite que la respiration continue en arrière-plan pendant que l'utilisateur modifie les paramètres.

### Cycle de respiration

Le cycle alterne entre deux phases :

```csharp
Inhale
Exhale
```

Pendant l'inspiration, la progression visuelle va de `0` à `1` : la boule monte.

Pendant l'expiration, la progression visuelle va de `1` à `0` : la boule descend.

Au lancement, l'application est arrêtée et la boule est en bas.

La progression de la boule est envoyée à la jauge avec :

```csharp
_gauge.SetProgress(visualProgress);
```

## `scripts/BreathingGauge.cs`

Contrôle personnalisé qui dessine la jauge et la boule.

La jauge est dessinée par code, sans texture externe.

### Forme de la jauge

La jauge a maintenant une forme de capsule verticale :

- rectangle central ;
- demi-cercle arrondi en haut ;
- demi-cercle arrondi en bas.

Techniquement, elle est dessinée avec :

- un rectangle central rempli ;
- un cercle en haut ;
- un cercle en bas.

La bordure de couleur différente autour de la jauge a été supprimée.

Les deux petits repères visuels à gauche, en haut et en bas, ont également été supprimés.

### Boule

La boule est dessinée avec `DrawCircle`.

Elle prend davantage de place dans la jauge qu'au début du projet, avec des marges latérales plus petites.

La position verticale dépend de la progression :

```csharp
SetProgress(float progress)
```

Avec :

```text
0.0 : boule en bas
1.0 : boule en haut
```

La progression est limitée entre `0.0` et `1.0` avec `Mathf.Clamp`.

## `scripts/BreathingSettings.cs`

Contient les paramètres de respiration et les thèmes de couleurs.

### Durées

Paramètres actuels :

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

La méthode utilisée pour garder les durées dans les limites est :

```csharp
ClampDuration(double value)
```

### Thèmes de couleurs

Les thèmes disponibles sont :

```text
Océan nocturne
Forêt douce
Crépuscule
Clair minimal
```

Chaque thème définit actuellement :

- couleur de fond ;
- couleur du texte ;
- couleur de la jauge ;
- couleur de bord de jauge ;
- couleur de la boule.

Note : la couleur de bord de jauge existe encore dans la structure des thèmes, mais la jauge ne dessine plus de bordure visible pour le moment.

## Interface actuelle

### Écran principal

Éléments visibles :

```text
[jauge + boule]

[▶ ou ⏸]                              [⚙]
```

L'écran principal est volontairement dépouillé pour être calme et utilisable sur téléphone.

### Écran réglages

Éléments visibles :

```text
[←] Réglages

Respiration
Inspiration    [−]  4.0s  [+]
Expiration     [−]  4.0s  [+]

Couleurs
[‹]  Nom du thème  [›]

[Réinitialiser le cycle]
```

## État actuel validé

Fonctionnel :

- projet Godot C# minimal ;
- scène principale configurée ;
- interface construite par code ;
- écran principal séparé de l'écran des réglages ;
- jauge verticale arrondie en forme de capsule ;
- jauge sans bordure visible ;
- jauge sans repères latéraux ;
- boule plus grande dans la jauge ;
- animation inspiration / expiration ;
- bouton démarrer / pause ;
- bouton réglages ;
- écran de réglages séparé ;
- réglage des durées ;
- changement de thème ;
- documentation de l'implémentation actuelle.

## Points techniques à surveiller

### Compilation C# dans Godot

Il peut arriver que Godot ne recompilie pas correctement l'assembly C# après un remplacement de fichiers.

En cas de doute, depuis la racine du projet :

```bash
dotnet clean
dotnet build
```

Si le problème persiste, fermer Godot puis supprimer les dossiers générés :

```bat
rmdir /s /q bin
rmdir /s /q obj
rmdir /s /q .godot\mono
```

Ensuite rouvrir le projet dans Godot, faire un build, puis relancer.

## À faire plus tard

Pistes possibles :

- améliorer encore l'esthétique générale ;
- sauvegarder les réglages localement ;
- ajouter éventuellement un court temps de pause entre inspiration et expiration ;
- tester sur Android ;
- préparer l'export Android ;
- vérifier les tailles des boutons et icônes sur un vrai téléphone.
