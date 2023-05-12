/*
 * Projet : Projet base de données en C#
 * Description : Classe Représentant les informations de connexion à une base de données MySQL.
 * Date de création : 30/03/2023
 * Auteur : Rémy / Zarkrey
 * Version : 1.1.1
 */

using MySqlConnector;


namespace DatabaseCsharp.sql
{
    /// <summary>
    /// Représente les informations de connexion à une base de données MySQL.
    /// </summary>
    public class DatabaseCredentials
    {

        /// <summary>
        /// Obtient l'adresse du serveur MySQL.
        /// </summary>
        public string Host { get; }

        /// <summary>
        /// Obtient le nom d'utilisateur MySQL.
        /// </summary>
        public string User { get; }

        /// <summary>
        /// Obtient le mot de passe MySQL.
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// Obtient le nom de la base de données MySQL.
        /// </summary>
        public string DatabaseName { get; }

        /// <summary>
        /// Obtient le numéro de port pour la connexion à MySQL.
        /// </summary>
        public uint Port { get; }

        /// <summary>
        /// Initialise une nouvelle instance de la classe DatabaseCredentials avec les informations de connexion fournies.
        /// </summary>
        /// <param name="host">Adresse du serveur MySQL</param>
        /// <param name="user">Nom d'utilisateur MySQL</param>
        /// <param name="password">Mot de passe MySQL</param>
        /// <param name="databaseName">Nom de la base de données MySQL</param>
        /// <param name="port">Numéro de port pour la connexion à MySQL</param>
        public DatabaseCredentials(string host, string user, string password, string databaseName, uint port)
        {
            Host = host;
            User = user;
            Password = password;
            DatabaseName = databaseName;
            Port = port;
        }

        /// <summary>
        /// Retourne une chaîne de connexion formatée pour la base de données MySQL en utilisant les informations de connexion fournies.
        /// </summary>
        /// <returns>Une chaîne de connexion pour la base de données MySQL.</returns>
        public override string ToString()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
            {
                Server = Host,
                UserID = User,
                Password = Password,
                Database = DatabaseName,
                Port = Port,
                AllowZeroDateTime = true
            };
            return builder.ConnectionString;
        }
    }
}