# SimpleBreathing — implémentation actuelle

## Version

Package 02 — Thèmes de couleurs.

Objectif : garder la base fonctionnelle du Package 01, puis ajouter un premier réglage visuel simple permettant de changer rapidement l'ambiance de l'application.

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
- L'interface est encore créée en code pour garder le prototype très facile à modifier.

## Scripts

### `scripts/Main.cs`

Script principal de l'application.

Responsabilités actuelles :

- crée l'interface utilisateur au lancement ;
- gère le cycle inspiration / expiration ;
- met à jour le texte affiché ;
- met à jour la position visuelle de la boule dans la jauge ;
- permet de modifier temporairement les durées avec des boutons ;
- permet de mettre en pause et de réinitialiser le cycle ;
- permet de changer de thème de couleurs avec les boutons `‹` et `›`.

Le cycle actuel est volontairement simple :

1. inspiration : la boule monte progressivement de bas en haut ;
2. expiration : la boule descend progressivement de haut en bas ;
3. le cycle recommence.

Les durées par défaut sont :

- inspiration : 4 secondes ;
- expiration : 4 secondes.

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

La jauge reçoit ses couleurs depuis `Main.cs`, qui les lit dans `BreathingSettings`.

### `scripts/BreathingSettings.cs`

Contient les réglages de base de l'application :

- durée d'inspiration ;
- durée d'expiration ;
- limites minimales et maximales des durées ;
- liste de thèmes de couleurs ;
- thème courant.

Les thèmes actuellement disponibles sont :

- `Océan nocturne` ;
- `Forêt douce` ;
- `Crépuscule` ;
- `Clair minimal`.

Pour l'instant, ces réglages ne sont pas encore sauvegardés. Ils existent seulement en mémoire pendant l'exécution.

## Tests à faire pour ce package

### Test 1 — remplacement des fichiers

1. Copier les fichiers du package à la racine du dépôt.
2. Accepter le remplacement des fichiers existants.
3. Ouvrir le projet avec Godot Engine .NET 4.6.2.

Résultat attendu : le projet s'ouvre sans erreur majeure.

### Test 2 — compilation C#

1. Cliquer sur **Build** dans Godot.

Résultat attendu : le projet compile sans erreur.

### Test 3 — lancement

1. Lancer le projet avec **F5**.

Résultat attendu :

- l'application démarre comme avant ;
- la boule monte et descend ;
- les boutons de durée fonctionnent ;
- les boutons `Pause` et `Reset` fonctionnent.

### Test 4 — changement de couleurs

1. Cliquer sur `›` dans la ligne `Couleurs`.
2. Cliquer plusieurs fois pour parcourir les thèmes.
3. Cliquer sur `‹` pour revenir en arrière.

Résultat attendu :

- le nom du thème change ;
- le fond change ;
- les couleurs de la jauge et de la boule changent ;
- l'animation continue normalement.

### Test 5 — thème clair

1. Aller jusqu'au thème `Clair minimal`.
2. Vérifier que les textes restent lisibles.
3. Revenir à un thème sombre.

Résultat attendu : l'interface reste utilisable aussi bien en thème sombre qu'en thème clair.

## Prochaines étapes prévues

1. Sauvegarder localement les durées et le thème choisi.
2. Ajouter un vrai écran de réglages si l'écran principal devient trop chargé.
3. Ajouter éventuellement une petite pause entre inspiration et expiration.
4. Préparer l'export Android.
5. Tester sur téléphone.
