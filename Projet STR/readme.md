# Projet IFT 611-729


L'objectif principal du projet consiste � cr�er une intelligence
artificielle de jeu-vid�o capable de r�agir aux actions du joueur
sous la forme d�un ou plusieurs ennemis.

Ce projet est bas� sur Unity 2020 et permet de faire s'affronter
un joueur et une IA sur une vari�t� de sc�nes disponibles.

Les m�thodes d'IA utilis�es sont NEAT et A*, r�utilisant du code 
existant.

## Lancer une partie

Pour lancer une partie, il faut aller dans le dossier 
[Scene](Assets/Scenes).
Ensuite dans le sous-dossier [NeatScene](Assets/Scenes/NeatScenes) 
se trouve un vari�t� de plateformes, avec comme ennemi, une IA
utilisant l'algorithme NEAT. <br>
Dans le sous-dossier [AStarScenes](Assets/Scenes/AStarScene) 
contient une scene WallArena_astar, pr�sentant un exemple de 
fonctionnement de notre IA utilisant A*.

## Entrainement d'une IA

Pour entrainer une IA bas�e sur NEAT, il faut utiliser la sc�ne 
[NeatTraining](Assets/Scenes/NeatScenes/NeatTraining.unity). 
Pour choisir le type de plateforme sur laquelle entrainer 
l'IA, il faut placer le prefab de cette scene dans le champ
`platformPrefab` du `LearningManager` pr�sent sur le GameObject
`plane`.

## Mesure de contraintes temps r�el

Pour enregistrer les contraintes temps r�els mesur�es dans 
`TestManager`, il faut appuyer sur la touche M pendant l'ex�cution
d'une partie. Cela enregistrera les temps dans [log.csv](log.csv).

Ensuite, le jupyter notebook [visualisation.ipynb](Visualisation/visualisation.ipynb)
permet de faire l'affichage des donn�es temps r�elles,
comme pr�sent�s dans le rapport

## Code existant utilis�

Pour faire ce projet, nous avons utilis� des bases exitantes 
de [arongranberg.com](https://arongranberg.com/astar/features)
et du github [b2developer](https://github.com/b2developer/MonopolyNEAT).

## Rapport

Nos rapport (L00, L01 et L02) sont disponibles dans le dossier [Rapport](Rapport/)

