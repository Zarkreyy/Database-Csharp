using System;
using DatabaseCsharp.sql;

namespace DatabaseCsharp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Exécute une requête qui retourne des données
            Database.ExecuteReader("SELECT * FROM gos_mc_users").Broadcast();

            // Exécute une requête qui ne retourne pas de données
            Database.ExecuteUpdate("DELETE FROM gos_mc_users WHERE idMcUser = 1");

            // Ferme la connexion à la base de données
            Database.Close();

            Console.Write("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
    }
}