using MySql.Data.MySqlClient;

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

        private static readonly DatabaseConnection Connection;

        /// <summary>
        /// Établit une connexion à la base de données.
        /// </summary>
        static Database()
        {
            Connection = new DatabaseConnection(new DatabaseCredentials(Host, User, Password, DatabaseName, Port));
        }

        /// <summary>
        /// Permet d'exécuter une requête SQL de type SELECT et de renvoyer les résultats sous forme d'objet SqlResult.
        /// </summary>
        /// <param name="query">La requête SQL à exécuter.</param>
        /// <returns>Un objet SqlResult contenant les résultats de la requête.</returns>
        public static SqlResult ExecuteReader(string query)
        {
            MySqlConnection sqlConnection = Connection.GetConnection();
            MySqlCommand sqlCommand = new MySqlCommand(query, sqlConnection);
            MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            SqlResult sqlResult = new SqlResult(sqlDataReader);
            sqlDataReader.Close();
            return sqlResult;
        }

        /// <summary>
        /// Permet d'exécuter une requête SQL de type UPDATE, INSERT ou DELETE.
        /// </summary>
        /// <param name="query">La requête SQL à exécuter.</param>
        public static void ExecuteUpdate(string query)
        {
            using (MySqlConnection conn = Connection.GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Permet de fermer la connexion à la base de données.
        /// </summary>
        public static void Close()
        {
            Connection?.Close();
        }
    }
}