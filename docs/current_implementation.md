# SimpleBreathing — implémentation actuelle

## Version

Package 01 — Architecture de base.

Objectif : avoir un projet Godot 4.6.2 C# ouvrable, compilable et lançable, avec une première scène principale fonctionnelle.

## Structure actuelle

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

## Scènes

### `scenes/Main.tscn`

Scène principale du projet.

- Noeud racine : `Control`.
- Script attaché : `scripts/Main.cs`.
- La scène ne contient presque rien directement : l'interface est créée par le script pour ce premier prototype.

## Scripts

### `scripts/Main.cs`

Script principal de l'application.

Responsabilités actuelles :

- crée l'interface utilisateur au lancement ;
- gère le cycle inspiration / expiration ;
- met à jour le texte affiché ;
- met à jour la position visuelle de la boule dans la jauge ;
- permet de modifier temporairement les durées avec des boutons ;
- permet de mettre en pause et de réinitialiser le cycle.

Le cycle actuel est volontairement simple :

1. inspiration : la boule monte progressivement de bas en haut ;
2. expiration : la boule descend progressivement de haut en bas ;
3. le cycle recommence.

Les durées par défaut sont :

- inspiration : 4 secondes ;
- expiration : 6 secondes.

### `scripts/BreathingGauge.cs`

Contrôle visuel personnalisé qui dessine :

- une jauge verticale ;
- une boule ;
- deux petits repères haut/bas.

La méthode principale est :

```csharp
SetProgress(float progress)
```

Avec :

- `0` = boule en bas ;
- `1` = boule en haut.

### `scripts/BreathingSettings.cs`

Contient les réglages de base de l'application :

- durée d'inspiration ;
- durée d'expiration ;
- couleurs principales ;
- limites minimales et maximales des durées.

Pour l'instant, ces réglages ne sont pas encore sauvegardés. Ils existent seulement en mémoire pendant l'exécution.

## Tests à faire pour ce package

### Test 1 — ouverture du projet

1. Copier les fichiers du package à la racine du dépôt.
2. Ouvrir le projet avec Godot Engine .NET 4.6.2.
3. Vérifier que la scène principale est bien `res://scenes/Main.tscn`.

Résultat attendu : le projet s'ouvre sans erreur majeure.

### Test 2 — compilation C#

1. Cliquer sur **Build** dans Godot.

Résultat attendu : le projet compile sans erreur.

Si Godot indique une erreur liée à `Godot.NET.Sdk`, vérifier que la version .NET de Godot est utilisée et que le SDK .NET est installé.

### Test 3 — lancement

1. Lancer le projet avec **F5**.

Résultat attendu :

- fond sombre ;
- titre `Simple Breathing` ;
- texte `Inspiration` puis `Expiration` ;
- jauge verticale ;
- boule qui monte puis descend.

### Test 4 — boutons de durée

1. Cliquer sur `+` ou `−` pour l'inspiration.
2. Cliquer sur `+` ou `−` pour l'expiration.

Résultat attendu :

- les durées affichées changent par pas de 0.5 seconde ;
- le cycle repart au début de l'inspiration.

### Test 5 — pause et reset

1. Cliquer sur `Pause`.
2. Vérifier que la boule s'arrête.
3. Cliquer sur `Reprendre`.
4. Cliquer sur `Reset`.

Résultat attendu :

- pause : l'animation s'arrête ;
- reprendre : l'animation repart ;
- reset : le cycle repart au début de l'inspiration.

## Prochaines étapes prévues

1. Séparer un peu mieux l'interface en composants si nécessaire.
2. Ajouter un vrai écran de réglages.
3. Ajouter la sauvegarde locale des durées et des couleurs.
4. Préparer l'export Android.
5. Tester sur téléphone.
