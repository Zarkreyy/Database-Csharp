/*
 * Projet : Projet base de données en C#
 * Description : Classe représentant les résultats d'une requête SQL.
 * Date de création : 30/03/2023
 * Auteur : Rémy / Zarkrey
 * Version : 1.0
 */

using System;
using System.Collections.Generic;
using System.Linq;
using MySqlConnector;


namespace DatabaseCsharp.sql
{
    /// <summary>
    /// Classe représentant les résultats d'une requête SQL.
    /// </summary>
    public class SqlResult
    {
        private readonly List<Dictionary<string, object>> _values;

        /// <summary>
        /// Constructeur de la classe SqlResult sans paramètre.
        /// </summary>
        public SqlResult() : this(null)
        {
        }

        /// <summary>
        /// Constructeur de la classe SqlResult avec un MySqlDataReader.
        /// </summary>
        /// <param name="sqlDataReader">Le MySqlDataReader contenant les résultats de la requête SQL</param>
        public SqlResult(MySqlDataReader sqlDataReader)
        {
            _values = new List<Dictionary<string, object>>();
            if (sqlDataReader == null) return;
            while (sqlDataReader.Read())
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                for (int i = 0; i < sqlDataReader.FieldCount; i++)
                {
                    string rowLabel = sqlDataReader.GetName(i);
                    object rowItem = sqlDataReader.GetValue(i);
                    row.Add(rowLabel, rowItem);
                }

                Values.Add(row);
            }

            sqlDataReader.Close();
        }

        private List<Dictionary<string, object>> Values => _values;

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne
        /// </summary>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public object GetObject(int columnIndex)
        {
            return GetObject(0, columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne
        /// </summary>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public object GetObject(string columnLabel)
        {
            return GetObject(0, columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public object GetObject(int rowIndex, int columnIndex)
        {
            if (GetRowCount() > rowIndex)
            {
                Dictionary<string, object> row = Values[rowIndex];
                int i = 0;
                foreach (object item in row.Values)
                {
                    if (i == columnIndex) return item;
                    i++;
                }
            }

            return null;
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public object GetObject(int rowIndex, string columnLabel)
        {
            if (GetRowCount() > rowIndex)
            {
                Dictionary<string, object> row = Values[rowIndex];
                if (row.ContainsKey(columnLabel)) return row[columnLabel];
            }

            return null;
        }

        /// <summary>
        /// Récupère le nombre de colonnes de la table
        /// </summary>
        /// <returns>Le nombre de colonnes</returns>
        public int GetColumnCount()
        {
            if (GetRowCount() > 0)
            {
                Dictionary<string, object> row = Values[0];
                return row.Count;
            }

            return 0;
        }

        /// <summary>
        /// Récupère le nombre de lignes de la table
        /// </summary>
        /// <returns>Le nombre de lignes</returns>
        public int GetRowCount()
        {
            return Values.Count;
        }

        /// <summary>
        /// Récupère le nom de colonne à partir de son index
        /// </summary>
        /// <param name="columnIndex">L'index de colonne</param>
        /// <returns>Le nom de colonne</returns>
        public string GetColumnLabel(int columnIndex)
        {
            if (GetRowCount() > 0)
            {
                Dictionary<string, object>.KeyCollection labels = Values[0].Keys;
                if (labels.Count > columnIndex) return labels.ElementAt(columnIndex);
            }

            return "null";
        }

        /// <summary>
        /// Vérifie si la table est vide
        /// </summary>
        /// <returns>True si la table est vide, False sinon</returns>
        public bool IsEmpty()
        {
            return Values.Count == 0;
        }

        /// <summary>
        /// Affiche la table de données dans la console
        /// </summary>
        public void Broadcast()
        {
            List<string> lines = new List<string>();
            if (GetRowCount() > 0)
            {
                string header = "[";
                for (int columnIndex = 0; columnIndex < GetColumnCount() - 1; columnIndex++)
                {
                    header += GetColumnLabel(columnIndex) + ", ";
                }

                header += GetColumnLabel(GetColumnCount() - 1) + "]";
                lines.Add(header);
            }

            for (int rowIndex = 0; rowIndex < GetRowCount(); rowIndex++)
            {
                string row = "[";
                for (int columnIndex = 0; columnIndex < GetColumnCount() - 1; columnIndex++)
                {
                    row += GetObject(rowIndex, columnIndex) + ", ";
                }

                row += GetObject(rowIndex, GetColumnCount() - 1) + "]";
                lines.Add(row);
            }

            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }
        }
    }
}