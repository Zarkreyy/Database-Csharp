/*
 * Projet : Projet base de données en C#
 * Description : Classe principale du programme permettant d'expérimenter la connexion à une base de données MySQL.
 * Date de création : 30/03/2023
 * Auteur : Rémy / Zarkrey
 * Version : 1.1.1
 */

using System;
using System.Collections.Generic;
using DatabaseCsharp.sql;

namespace DatabaseCsharp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Exécute une requête qui retourne des données
            SqlResult sqlResult = Database.ExecuteReader(
                "SELECT * FROM gos_mc_users WHERE idMcUser = ? AND username = ?",
                new List<object> { 1, "Zarkrey" }
            );

            // Affiche toutes les données retournées par la requête
            sqlResult.Broadcast();

            // Affiche le nom de tous les utilisateurs
            for (int rowIndex = 0; rowIndex < sqlResult.GetRowCount(); rowIndex++)
            {
                Console.WriteLine("username: " + sqlResult.Get<string>(rowIndex, "username"));
            }

            // Exécute une requête qui ne retourne pas de données
            int rowsAffected = Database.ExecuteUpdate("DELETE FROM gos_mc_users WHERE idMcUser = ? AND username = ?",
                new List<object> { 2, "Zazouh" }
            );
            Console.WriteLine(rowsAffected + " lignes affectées");

            // Ferme la connexion à la base de données
            Database.Close();

            Console.Write("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
    }
}