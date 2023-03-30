# Projet base de données en C#

Ce projet consiste en une classe statique permettant la gestion des connexions à une base de données MySQL ainsi que l'exécution des requêtes SQL.

## Utilisation

Pour utiliser cette classe, il suffit d'installer la bibliothèque `MySql.Data` de MySQL à l'aide du gestionnaire de packages NuGet.

### Connexion à la base de données

La connexion à la base de données est initialisée lors du chargement de la classe `Database`. Vous devrez cependant modifier les constantes `Host`, `User`, `Password`, `DatabaseName` et `Port` en fonction des paramètres de connexion de votre base de données.

### Exemples de code

Pour exécuter une requête SQL de type SELECT, vous devez utiliser la méthode `ExecuteQuery` qui prend en paramètre la requête SQL à exécuter et éventuellement des paramètres et renvoie un objet `SqlResult` contenant les résultats de la requête.

Pour exécuter une requête SQL de type UPDATE, INSERT ou DELETE, vous devez utiliser la méthode `ExecuteUpdate` qui prend en paramètre la requête SQL à exécuter et éventuellement des paramètres et renvoie le nombre de lignes affectées.

```csharp
// Exécute une requête qui retourne des données
SqlResult sqlResult = Database.ExecuteReader(
    "SELECT * FROM gos_mc_users WHERE idMcUser = ? AND username = ?",
    new List<object> { 2, "Zarkrey" }
);

// Affiche toutes les données retournées par la requête
sqlResult.Broadcast();

// Affiche le nom de tous les utilisateurs
for (int rowIndex = 0; rowIndex < sqlResult.GetRowCount(); rowIndex++)
{
    Console.WriteLine("username: " + sqlResult.GetObject(rowIndex, "username"));
}

// Exécute une requête qui ne retourne pas de données
int rowsAffected = Database.ExecuteUpdate("DELETE FROM gos_mc_users WHERE idMcUser = ? AND username = ?",
    new List<object> { 2, "Zazouh" }
);
Console.WriteLine(rowsAffected + " lignes affectées");

// Ferme la connexion à la base de données
Database.Close();
```
