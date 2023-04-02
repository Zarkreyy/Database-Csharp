/*
 * Projet : Projet base de données en C#
 * Description : Classe permettant de gérer la connexion à une base de données MySQL.
 * Date de création : 30/03/2023
 * Auteur : Rémy / Zarkrey
 * Version : 1.1.0
 */

using System;
using MySqlConnector;

namespace DatabaseCsharp.sql
{
    /// <summary>
    /// Classe permettant de gérer la connexion à une base de données MySQL.
    /// </summary>
    public class DatabaseConnection
    {
        private readonly DatabaseCredentials _credentials;
        private MySqlConnection _connection;

        /// <summary>
        /// Initialise une nouvelle instance de la classe DatabaseConnection avec les informations de connexion fournies.
        /// </summary>
        /// <param name="credentials">Les informations de connexion à la base de données.</param>
        public DatabaseConnection(DatabaseCredentials credentials)
        {
            _credentials = credentials;
            Connect();
        }

        private DatabaseCredentials Credentials => _credentials;

        private MySqlConnection Connection
        {
            get => _connection;
            set => _connection = value;
        }

        /// <summary>
        /// Récupère la connexion à la base de données. Si la connexion n'est pas encore établie ou si elle est fermée, elle est ouverte.
        /// </summary>
        /// <returns>La connexion à la base de données.</returns>
        public MySqlConnection GetSqlConnection()
        {
            if (Connection == null) Connect();
            else if (Connection.State != System.Data.ConnectionState.Open) Connection.Open();
            return Connection;
        }

        private void Connect()
        {
            Connection = new MySqlConnection(Credentials.ToString());
            Connection.Open();
            Console.WriteLine("Connected to MySQL : " + Credentials.Host + ":" + Credentials.Port);
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