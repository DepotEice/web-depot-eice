# Épreuve intégrée - Depot EICE

## Description

Le but de ce projet est de créer un _PoC_ (Proof of Concept) d'une application de gestion d'établissement scolaire.
Ce prototype permet de gérer non seulement les étudiants, mais aussi les professeurs, les cours, les groupes et les sessions, etc.

## Installation

Il n'y a aucune installation à prévoir pour le moment. L'application se test directement sur le web, à l'adresse suivante: [https://www.tfe-depot-eice.be/](https://www.tfe-depot-eice.be/)

## Utilisation

Le site en lui-même est assez intuitif. Il suffit de se connecter avec un compte valide pour pouvoir accéder à toutes les fonctionnalités du site.

Pour la démonstration, vous pouvez utiliser les comptes suivants:

| Utilisateur                        | Mot de passe    | Rôle       |
| ---------------------------------- | --------------- | ---------- |
| <tfe.soultan.hatsijev+1@gmail.com> | VOIR secret.txt | Direction  |
| <tfe.soultan.hatsijev+2@gmail.com> | VOIR secret.txt | Direction  |
| <tfe.soultan.hatsijev+3@gmail.com> | VOIR secret.txt | Professeur |
| <tfe.soultan.hatsijev+4@gmail.com> | VOIR secret.txt | Professeur |
| <tfe.soultan.hatsijev+5@gmail.com> | VOIR secret.txt | Etudiant   |
| <tfe.soultan.hatsijev+6@gmail.com> | VOIR secret.txt | Etudiant   |
| <tfe.soultan.hatsijev+7@gmail.com> | VOIR secret.txt | Invité     |

Libre à vous de créer vos propres comptes pour tester l'application. Après la création du compte, vous recevrez un email de l'adresse <soultan.hatsijev@hainaut-promsoc.be>. Il faudra cliquer sur le lien pour activer le compte. (Vérifiez vos spams si vous ne recevez pas l'email, il est probable qu'il faille attendre quelques minutes avant de le recevoir)

## Remarques

L'application comporte encore quelques bugs. Ces bugs touchent principalement les fonctionnalités de dates et de messagerie. Les bugs connus sont listés ci-dessous:

- Un message d'erreur apparait lorsqu'on crée un module signalant qu'il est impossible d'assigner des élèves ou de gérer l'horaire. Il faut d'abord créer le module et l'enregistrer. Une fois fait, il sera possible de s'occuper du reste.
- Lors de l'envoi du premier message, il faut rafraichir la page pour voir le message envoyé. Après le premier message, tout fonctionne correctement.
- Lors de l'assignation d'un horaire pour un module ou des heures d'ouvertures. Les dates de fermetures exceptionelles peuvent s'afficher de la mauvaise manière. Ce bug demande une correction urgente mais surtout une refonte de la gestion des dates dans l'application.
- Lors de la modification d'un utilisateur dans le menu de gestion des utilisateurs, il est impossible de sauvegarder les changements apportés à l'utilisateur si l'adresse email de l'école et le genre de l'utilisateur ne sont pas définis. Il faut donc définir ces deux champs avant de pouvoir sauvegarder les changements.
- Lors de la modification de la photo de profile, il est possible de déplacer l'image pour qu'elle s'aligne horizontalement comme l'utilisateur l'entend. Cependant, étant donné que c'est une fonctionnalité implmentée 100% de 0, le résultat n'est pas celui escompté. L'image modifié revient à son état normal une fois enregistré. Le traitement de l'image en arrière plan ne se fait pas comme attendu. Il faut donc revoir cette fonctionnalité pour qu'elle soit plus intuitive et fonctionnelle.

## Fonctionnalités

### Connexion / Inscription / Déconnexion

![Connexion et inscription](image.png)

![Déconnexion](image-1.png)

#### Inscription

![Inscription](image-3.png)

#### Connexion

![Connexion](image-2.png)

Utiliser les identifiants fournis dans le tableau ci-dessus pour vous connecter ou utilisez les vôtres si vous en avez créé.

### Blog

![Blog navigation](image-6.png)

![Blog](image-4.png)

### Prendez rendez-vous

![Rendez-vous navigation](image-7.png)

![Rendez-vous](image-5.png)

### Modules disponibles

![Modules disponibles navigation](image-8.png)

![Modules disponibles](image-9.png)

## Profile

Quelques fonctionnalités pour le profile se sont ajoutées à l'application. Notamment la possibilité de modifier son mot de passe, de modifier sa photo de profile, modifier les adresses et sélectionner une adresse principale, voir ses rendez-vous. Mais surtout, supprimer le compte.

### Informations générales

![Informations générales du profile](image-28.png)

![Modification de la photo de profile](image-29.png)

### Adresses du compte

![Adresses du compte](image-30.png)

### Modifier le mot de passe

![Modifier le mot de passe](image-31.png)

### Les rendez-vous du compte

![Rendez-vous du compte](image-32.png)

### Supprimer le compte

![Supprimer le compte](image-33.png)

## Depot

### Calendrier du dépot

![Calendrier du depot](image-10.png)

Le choix de la date dans le calendrier demande une amélioration mais surtout une customisation pour qu'il soit plus facile à utiliser. En effet pour l'instant, si l'affiche est défini sur le mois ou la semaine et une date est sélectionnée, le calendrier va sélectionné le premier jour qui apparaitra dans l'agenda.

![Sélection du 13 Septembre](image-23.png)

![Jour sélectionné par l'application](image-24.png)

Sur les screens précédents, on peut voir qu'on a sélectionné le 13 Septembre, mais l'application a sélectionné le 11 Septembre qui est le premier jour qui s'affiche dans l'agenda.

### Modules du dépot

![Modules du dépot](image-11.png)

Pour créer un module. Il faut appuyer sur le bouton "Créer un module" et remplir le formulaire. Lors du premier chargement de la page, il sera impossible d'assigner des élèves ou de gérer l'horaire. Il faut d'abord créer le module et l'enregistrer. Une fois fait, il sera possible de s'occuper du reste.

(Un message d'erreur apparait en haut à droite, il faut ignorer ce message pour le moment. Il sera corrigé dans une prochaine version)

![Création du module](image-22.png)

### Utilisateurs disponibles

![Utilisateurs disponibles](image-12.png)

Il est possible de modifier et visualiser certaines information de l'utilisateur en cliquant sur le bouton dans la dernière colonnes du tableau des utilisateurs.

![Modify user](image-25.png)

![Info de l'utilisateur](image-26.png)

### Horaires du dépot

![Horaires du dépot](image-13.png)

### Rendez-vous

![Rendez-vous](image-14.png)

Pour avoir plus d'information sur les rendez-vous. Il faut cliquer sur un des rendez-vous du calendrier. Un popup apparaitra avec les informations du rendez-vous. Il est possible de modifier le rendez-vous en sélectionnant une nouvelle date de début et de fin pour ensuite confirmer. Mais il est également possible d'annuler le rendez-vous en cliquant sur le bouton 'Annuler'.

![Information du rendez-vous](image-27.png)

Pour fermer le rendez-vous sans appliquer de modification, il suffit de cliquer sur le X en haut à droite du popup.

### Articles du blog du dépot

Il est possible de modifier, supprimer et filtrer les articles. Pour modifier un article, il faut cliquer sur le bouton `Editer` sur un des articles. Pour supprimer un article, il faut cliquer sur l'icone de la poubelle. Pour filtrer les articles, c'est intuitif, il suffit de suivre les directions affichées à l'écran.

Pour récupérer un article supprimé, il faut sélectioner l'option `Inclure les articles supprimés` et lancer la recherche en cliquant sur `Recherche`.

Pour restaurer un article supprimé, il faut appuyer sur l'icone de la flèche.

![Articles du blog du dépot](image-15.png)

## Messagerie

J'ai remarqué un bug avec la messagerie. Lors de l'envoi du premier message, il faut rafraichir la page pour voir le message envoyé. Après le premier message, tout fonctionne correctement.

Malheureusement ce bug a été remarqué assez tard et je n'ai pas eu le temps de le corriger.

![Messagerie](image-16.png)

Pour envoyer le premier message lorsque la liste est vide. Il faut appuyer sur le bouton plus, choisir un utilisateur dans la barre de recherche, taper le message et cliquer sur le bouton envoyer.

![Plus](image-17.png)

![Sélection de l'utilisateur](image-18.png)

![Envoi du message](image-20.png)

Après avoir recharché la page, le message est bien présent.

![Message envoyé](image-21.png)
