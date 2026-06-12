# 🏢 RoomBooking API

API de réservation de salles développée en .NET, permettant de gérer des salles et leurs réservations avec gestion des conflits de planning.

---

## 🚀 Objectif du projet

Ce projet a été réalisé dans le but de :

- Concevoir une API REST propre et maintenable
- Implémenter une logique métier réaliste (gestion de réservations)
- Appliquer de bonnes pratiques (architecture, validation, sécurité)
- Servir de projet de démonstration pour un portfolio développeur backend

---

## 🧠 Fonctionnalités

- ✅ Création, modification et suppression de salles
- ✅ Réservation de salles
- ✅ Vérification des disponibilités
- ✅ Gestion des conflits de réservation
- 🔜 Authentification des utilisateurs (JWT)
- 🔜 Validation des données
- 🔜 Gestion des erreurs centralisée

---

## 🏗️ Architecture

Le projet suit une architecture modulaire (en cours d'amélioration) :

- **API** : gestion des routes HTTP
- **Application** : logique métier
- **Domain** : entités et règles métier
- **Infrastructure** : accès aux données

---

## 🛠️ Technologies utilisées

- .NET (ASP.NET Core Web API)
- Entity Framework Core
- Docker / Docker Compose
- SQL (ou base de données configurée)

---

## 📦 Installation

### 1. Cloner le projet

```bash
git clone https://github.com/Pyroblastouille/RoomBooking.git
cd RoomBooking
```

### 2. Lancer avec Docker

```bash
docker-compose up --build
```

### 3. Accéder à l'API

- API : http://localhost:5000
- Swagger : http://localhost:5000/swagger

---

## 📌 Exemple d'utilisation

### Créer une réservation

```http
POST /reservations
```

```json
{
  "roomId": 1,
  "startDate": "2026-06-12T10:00:00",
  "endDate": "2026-06-12T12:00:00"
}
```

---

## ⚠️ Règles métier importantes

- Une salle ne peut pas être réservée sur deux créneaux qui se chevauchent
- Les dates de réservation doivent être cohérentes (début < fin)

---

## 🔐 Sécurité (à venir)

- Authentification JWT
- Autorisation par rôle
- Protection des endpoints sensibles

---

## 🧪 Tests (à venir)

- Tests unitaires sur la logique métier
- Tests d’intégration API

---

## 📈 Améliorations prévues

- Ajout de l’authentification
- Implémentation de FluentValidation
- Gestion centralisée des erreurs
- Ajout de logs (Serilog)
- Refactor en Clean Architecture complète

---

## 👨‍💻 Auteur

Projet développé par [Pyroblastouille](https://github.com/Pyroblastouille)

---

## 📄 Licence

Ce projet est open-source et disponible sous licence MIT.
---
