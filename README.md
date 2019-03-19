# Notifire - Outil de notification d'entreprise

Ceci est un logiciel open source associant la technologie CachetHQ et BurntToast développé en C#

[![placeholder - copie](https://user-images.githubusercontent.com/39912632/51183365-46689000-18d1-11e9-87cb-83a809c152c3.png)](https://github.com/jerkhouri/Notifire)
           [![placeholder - copie](https://user-images.githubusercontent.com/39912632/51183369-49638080-18d1-11e9-8cb1-614c7fac6050.png)](https://cachethq.io/)
           [![placeholder - copie](https://user-images.githubusercontent.com/39912632/51183373-4bc5da80-18d1-11e9-9430-833b52adebd0.png)](https://github.com/Windos/BurntToast)           
[![placeholder - copie](https://user-images.githubusercontent.com/39912632/51183506-b2e38f00-18d1-11e9-9217-9419c3faff0f.png)](https://docs.microsoft.com/fr-fr/dotnet/csharp/programming-guide/)           
[![placeholder - copie](https://user-images.githubusercontent.com/39912632/51183386-508a8e80-18d1-11e9-9a7d-31199ece72e0.png)](https://www.aramisauto.com/)

Cette application à pour but de générer des notifications d'incident via le module BurntToast (PowerShell) sur  les postes Windows de l'organisation AramisAuto.

## Etape 1 : Le fonctionnement de CachetHQ pour l'outil.
* L'admin créer un incident via un tableau de bord CachetHQ héberger par un serveur linux sous docker [doc here](https://docs.cachethq.io/docs/get-started-with-docker)
![placeholder - copie](https://user-images.githubusercontent.com/39912632/51183388-52545200-18d1-11e9-88ce-38688a01144d.png)
* La section composant doit contenir uniquement le nom des application impactée par un incident afin d'afficher la bonne images par application.

## Etape 2 : Le fonctionnement de l'outil
* Lorsqu'un incident est créé, il génère un requête dite "HTTP" accessible via l'url suivant: 
![placeholder - copie](https://user-images.githubusercontent.com/39912632/54598057-00968800-4a38-11e9-86d0-826364afb537.png)
* L'outil se charge de lire dynamiquement le contenu de la requête html et récupère les éléments suivant:
  + id : c'est le numéros individuel de l'incident, afin de mettre en cache la notification.
  + component_id : c'est le nom de l'application impactée
  + name : le titre de l'incident
  + message : le contenu du message
  + created_at : la date de création de l'incident
  + human_status : afin de gérer l’évolution de l'incident
  
* Il vérifie l’existence préalable de l'incident via le cache précédemment cité (id).
* Il ajoute à la future notification la bonne image correspondante au nom de l'application impactée (compenent_id).
* Il créer la notification sur le poste Windows.


## Etape 3 : Les plus de l'outil. 
* L'outil se lance en tache de fond au lancement de Windows et apparaît dans la barre des icônes, le clic droit permet d’accéder aux fonctionnalités suivantes:
  + Créer une demande d'immobilisation (ouvre une page web)
  + Créer un ticket à l'équipe support (ouvre une page web dans un formulaire)
  + Ouvrir la page des incidents (ouvre le tableau de bord de CachetHQ accessible à tous)
 * Fonctionnant à l'aide d'un formulaire Windows on peut très facilement imaginer de nombreuses fonctionnalités supplémentaires.
 
 ## Etape 4 : Utilisation de l'outil
 * L'outil s'installe via un msi géneré grace aux module installer de visual studio.
 * Il faut installer le module [BurnToast](https://github.com/Windos/BurntToast) sur la machine cliente.
 * Lors du premier lancement, l'outil vas demander à l'utilisateur de choisir entre une serie d'application sur lesquelles il vas être notifier, ce choix pourras être modifier par l'utilisateur dans le menu >Infos et >Paramètre.
 
## Contributeurs
+ CachetHQ
+ BurnToast (Windows)
+ AramisAuto
