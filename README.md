# 🧃 Nuka Cola Dispenser - Projet Console C#

Bienvenue dans **Nuka Cola Dispenser**, un simulateur de distributeur de boissons dans l'univers de **Fallout**, développé en **C#** sous forme de **projet console**.

Ce projet a été conçu comme un **exercice d'apprentissage personnel** pour explorer :
- Les **menus interactifs**
- L'utilisation de **sons**
- Les **dictionnaires**
- La **gestion d'états** (admin / client)
- Le **rendu de monnaie**
- Le **stockage dynamique des boissons**

---

## 🚀 Fonctionnalités

### 🎮 Mode Utilisateur
- Choix de boissons via un menu console coloré
- Affichage des stocks et des prix (en "caps")
- Paiement simulé :
  - L'utilisateur insère une valeur au clavier
  - Si le montant est insuffisant, il peut compléter
  - Rendu automatique de la **monnaie** si surplus
- Sons d’ambiance (paiement, erreurs, sélection, etc.)

### 🔐 Mode Administrateur
- Accès via un **mot de passe simple** : `Vault Tech` (visible dans le code)
- Ajout, modification ou suppression de boissons
- Gestion du stock existant
- Modification des propriétés d'une boisson :
  - Nom
  - Prix
  - Description
  - Stock
  - Couleur console

---

## 🛠️ Technologies utilisées

- **Langage :** C#
- **Type de projet :** Application console (.NET)
- **Packages :**
  - [`NAudio`](https://github.com/naudio/NAudio) pour la lecture de fichiers audio (`.wav`/`.mp3`)
- **Structures de données :**
  - `Dictionary<int, Drinks>`
  - `Dictionary<int, Select>`
  - `Dictionary<int, Actions>`

---

## 📂 Structure du projet

Nuka_Machine/
│
├── Program.cs → Lancement du programme
├── Machine.cs → Logique du distributeur (menu client et admin)
├── Drinks.cs → Définition d'une boisson
├── /sounds/ → Contient tous les effets sonores utilisés
└── README.md


---

## 🎧 Sons

Les sons sont lus depuis le dossier `/sounds` grâce à `NAudio`. Assure-toi qu’il contient les fichiers suivants (ou remplace-les) :

- `admin_mode.wav`
- `Erreur.wav`
- `access_granted.mp3`
- `task_done.wav`
- `leaves.wav`
- `Payment.wav`
- `not_enough_cash.wav`
- `those_aint_caps.wav`
- `Change.wav`
- `out_of_stock.mp3`
- `received_drink.wav`
- `received_RadX.wav`
- `pudding.wav` *(Easter egg pour une boisson nommée Dean !)*

---

## ▶️ Exécution

1. Cloner le dépôt :

   ```bash
   git clone https://github.com/ton-utilisateur/nuka-cola-dispenser.git
   cd nuka-cola-dispenser
2. Ouvrir dans Visual Studio (ou tout IDE C#)

3. Restaurer les packages si nécessaire (NAudio via NuGet)

4. Appuyer sur F5 pour exécuter

✨ Extraits de console
======== Welcome to your post-apocalyptic drink machine ========
ID    | Name                     | Price (caps) | Stock   
-------------------------------------------------------------
1     | Nuka Cola               | 20           | 10      
2     | Nuka Cherry             | 40           | 10      
3     | Nuka Quantum            | 50           | 10      
4     | Nuka Void               | 55           | 10      
5     | Rad X                   | 300          | 1       
6     | Admin Mode              | -            | -   

🎯 Objectif pédagogique
Approfondir la programmation orientée objet en C#

Manipuler les entrées utilisateurs et erreurs

Implémenter une logique conditionnelle multi-états (admin/client)

Ajouter une touche de fun et d'immersion avec de l'audio

Créer un projet structuré et complet pour un portfolio ou un cours

📜 Licence
Ce projet est un exercice personnel. Tu peux le modifier, le forker ou le réutiliser librement.

✏️ Auteure
Projet développé par LadyLJ13, dans le cadre d’un apprentissage en C#.
Toute ressemblance avec un distributeur de Nuka Cola est totalement assumée 😄.
