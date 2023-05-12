/*
 * Projet : Projet base de données en C#
 * Description : Classe statique permettant la gestion des connexions à la base de données MySQL ainsi que l'exécution des requêtes SQL.
 * Date de création : 30/03/2023
 * Auteur : Rémy / Zarkrey
 * Version : 1.1.1
 */

using System.Collections.Generic;
using MySqlConnector;

namespace DatabaseCsharp.sql
{
    /// <summary>
    /// Classe statique permettant la gestion des connexions à la base de données MySQL ainsi que l'exécution des requêtes SQL.
    /// </summary>
    public static class Database
    {
        private const string Host = "localhost";
        private const string User = "root";
        private const string Password = "";
        private const string DatabaseName = "gos";
        private const int Port = 3306;

        /// <summary>
        /// Établit une connexion à la base de données.
        /// </summary>
        private static ConnexionHandler ConnexionHandler { get; } = new ConnexionHandler(new DatabaseCredentials(Host, User, Password, DatabaseName, Port));

        /// <summary>
        /// Permet d'exécuter une requête SQL de type SELECT et de renvoyer les résultats sous forme d'objet SqlResult.
        /// </summary>
        /// <param name="query">La requête SQL à exécuter.</param>
        /// <param name="parameters">Une liste d'objets qui contiennent les paramètres pour la requête SQL.</param>
        /// 
        /// <returns>Un objet SqlResult contenant les résultats de la requête.</returns>
        public static SqlResult ExecuteReader(string query, List<object> parameters = null)
        {
            MySqlConnection sqlConnection = ConnexionHandler.Connection;
            using (MySqlCommand sqlCommand = new MySqlCommand(query, sqlConnection))
            {
                BindParam(sqlCommand, parameters);
                return new SqlResult(sqlCommand.ExecuteReader());
            }
        }

        /// <summary>
        /// Permet d'exécuter une requête SQL de type UPDATE, INSERT ou DELETE et de renvoyer le nombre de lignes affectées.
        /// </summary>
        /// <param name="query">La requête SQL à exécuter.</param>
        /// <param name="parameters">Une liste d'objets qui contiennent les paramètres pour la requête SQL.</param>
        /// <returns>Le nombre de lignes affectées</returns>
        public static int ExecuteUpdate(string query, List<object> parameters = null)
        {
            MySqlConnection sqlConnection = ConnexionHandler.Connection;
            using (MySqlCommand sqlCommand = new MySqlCommand(query, sqlConnection))
            {
                BindParam(sqlCommand, parameters);
                return sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Permet de fermer la connexion à la base de données.
        /// </summary>
        public static void Close()
        {
            ConnexionHandler?.Close();
        }

        private static void BindParam(MySqlCommand sqlCommand, List<object> parameters)
        {
            if (parameters == null) return;
            int parameterCount = 1;
            foreach (object parameter in parameters)
            {
                sqlCommand.Parameters.AddWithValue($"@param{parameterCount}", parameter);
                parameterCount++;
            }
        }
    }
}