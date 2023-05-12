/*
 * Projet : Projet base de données en C#
 * Description : Classe permettant de gérer la connexion à une base de données MySQL.
 * Date de création : 30/03/2023
 * Auteur : Rémy / Zarkrey
 * Version : 1.1.1
 */

using System;
using MySqlConnector;

namespace DatabaseCsharp.sql
{
    /// <summary>
    /// Classe permettant de gérer la connexion à une base de données MySQL.
    /// </summary>
    public class ConnexionHandler
    {
        private DatabaseCredentials Credentials { get; }
        private MySqlConnection UnsafeConnexion { get; }

        /// <summary>
        /// Initialise une nouvelle instance de la classe DatabaseConnection avec les informations de connexion fournies.
        /// </summary>
        /// <param name="credentials">Les informations de connexion à la base de données.</param>
        public ConnexionHandler(DatabaseCredentials credentials)
        {
            Credentials = credentials;
            UnsafeConnexion = new MySqlConnection(Credentials.ToString());
        }

        /// <summary>
        /// Récupère la connexion à la base de données. Si la connexion n'est pas encore établie ou si elle est fermée, elle est ouverte.
        /// </summary>
        /// <returns>La connexion à la base de données.</returns>
        public MySqlConnection Connection
        {
            get
            {
                if (UnsafeConnexion.State != System.Data.ConnectionState.Open)
                {
                    UnsafeConnexion.Open();
                    Console.WriteLine("Connected to MySQL : " + Credentials.Host + ":" + Credentials.Port);
                }
                return UnsafeConnexion;
            }
        }

        /// <summary>
        /// Ferme la connexion à la base de données si elle est ouverte.
        /// </summary>
        public void Close()
        {
            if (Connection != null && Connection.State != System.Data.ConnectionState.Closed) Connection.Close();
        }
    }
}