# Structure
| Couche | Rôle 
|--------|------|
| `Controllers/` | Reçoit les requêtes HTTP, répond au client | 
| `Services/` | Contient toute la logique métier | 
| `Interfaces/` | Contrats que les services doivent respecter |
| `Models/` | Les entités de la base de données |
| `DTOs/` | Ce qu'on envoie/reçoit via l'API | 
| `Data/` | Le contexte EF Core (accès DB) | 

# Packages Installés
| Nom | Fonction |
| - | - |
| Swashbuckle.AspNetCore | Ajoute Swagger pour les endpoints |
| Npgsql.EntityFrameworkCore.PostgreSQL | Entity Framework pour parler avec PostgreSQL |
| Microsoft.EntityFrameworkCore.Design | commandes de migration |  

# Database Export
`dotnet ef migrations add <nom>` Crée une snapshot de la bdd et un build prêt à envoyer
`dotnet ef database update` Envoi le build dans connexion string

# user-secrets
Rajoute de la sécurité
`dotnet user-secrets init` : Initialise l'utilisation des user-secrets
`dotnet user-secrets set "key" "value"` : Rajoute ou change une paire de key-value
`dotnet user-secrets remove "key"` : Enlève la clé
`dotnet user-secrets list` : Liste les pairs key-value