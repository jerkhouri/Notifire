# Notifire - Outil de notification d'entreprise

Ceci est un logiciel open source associant la technologie open source CachetHQ et BurntToast développé en C#

                                    




Cette application à pour but de générer des notifications d'incident via le module BurntToast (PowerShell) sur  les postes Windows de l'organisation AramisAuto.

## Etape 1 : Le fonctionnement de CachetHQ pour l'outil.
* L'admin créer un incident via un tableau de bord CachetHQ héberger par un serveur linux sous docker (doc here)
* La section composant doit contenir uniquement le nom des application impactée par un incident afin d'afficher la bonne images par application.

## Etape 2 : Le fonctionnement de l'outil
* Lorsqu'un incident est créé, il génère un requête dite "HTTP" accessible via l'url suivant:


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
