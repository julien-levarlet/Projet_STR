# Projet IFT 611-729


L'objectif principal du projet consiste à créer une intelligence
artificielle de jeu-vidéo capable de réagir aux actions du joueur
sous la forme d’un ou plusieurs ennemis.

Ce projet est basé sur Unity 2020 et permet de faire s'affronter
un joueur et une IA sur une variété de scènes disponibles.

Les méthodes d'IA utilisées sont NEAT et A*, réutilisant du code 
existant.

## Lancer une partie

Pour lancer une partie, il faut aller dans le dossier 
[Scene](Assets/Scenes).
Ensuite dans le sous-dossier [NeatScene](Assets/Scenes/NeatScenes) 
se trouve une variété de plateformes, avec comme ennemi, une IA
utilisant l'algorithme NEAT. <br>
Dans le sous-dossier [AStarScenes](Assets/Scenes/AStarScene) 
contient une scene WallArena_astar, présentant un exemple de 
fonctionnement de notre IA utilisant A*.

## Entrainement d'une IA

Pour entrainer une IA basée sur NEAT, il faut utiliser la scène 
[NeatTraining](Assets/Scenes/NeatScenes/NeatTraining.unity). 
Pour choisir le type de plateforme sur laquelle entrainer 
l'IA, il faut placer le prefab de cette scene dans le champ
`platformPrefab` du `LearningManager` présent sur le GameObject
`plane`.

## Mesure de contraintes temps réel

Pour enregistrer les contraintes temps réels mesurées dans 
`TestManager`, il faut appuyer sur la touche M pendant l'exécution
d'une partie. Cela enregistrera les temps dans [log.csv](log.csv).

Ensuite, le jupyter notebook [visualisation.ipynb](Visualisation/visualisation.ipynb)
permet de faire l'affichage des données temps réelles,
comme présentés dans le rapport

## Code existant utilisé

Pour faire ce projet, nous avons utilisé des bases existantes 
de [arongranberg.com](https://arongranberg.com/astar/features)
et du github [b2developer](https://github.com/b2developer/MonopolyNEAT).

## Rapport

Nos rapport (L00, L01 et L02) sont disponibles dans le dossier [Rapport](Rapport/)

