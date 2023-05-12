/*
 * Projet : Projet base de données en C#
 * Description : Classe représentant les résultats d'une requête SQL.
 * Date de création : 30/03/2023
 * Auteur : Rémy / Zarkrey
 * Version : 1.1.1
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.WellKnownTypes;
using MySqlConnector;


namespace DatabaseCsharp.sql
{
    /// <summary>
    /// Classe représentant les résultats d'une requête SQL.
    /// </summary>
    public class SqlResult
    {
        private Dictionary<string, List<object>> Values { get; }

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
            Values = new Dictionary<string, List<object>>();
            if (sqlDataReader == null) return;
            if (sqlDataReader.Read())
            {
                for (int columnIndex = 0; columnIndex < sqlDataReader.FieldCount; columnIndex++)
                {
                    string rowLabel = sqlDataReader.GetName(columnIndex);
                    object rowItem = sqlDataReader.GetValue(columnIndex);
                    Values.Add(rowLabel, new List<object>() { rowItem });
                }
            }

            while (sqlDataReader.Read())
            {
                for (int columnIndex = 0; columnIndex < sqlDataReader.FieldCount; columnIndex++)
                {
                    string rowLabel = sqlDataReader.GetName(columnIndex);
                    object rowItem = sqlDataReader.GetValue(columnIndex);
                    Values[rowLabel].Add(rowItem);
                }
            }

            sqlDataReader.Close();
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne
        /// </summary>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public object Get(int columnIndex)
        {
            return Get(0, columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne
        /// </summary>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public object Get(string columnLabel)
        {
            return Get(0, columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public object Get(int rowIndex, int columnIndex)
        {
            return GetRowCount() > rowIndex && GetColumnCount() > columnIndex
                ? Values.Values.ElementAt(columnIndex)[rowIndex]
                : null;
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public object Get(int rowIndex, string columnLabel)
        {
            return GetRowCount() > rowIndex && Values.ContainsKey(columnLabel)
                ? Values[columnLabel][rowIndex]
                : null;
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne, et tente de la convertir au type spécifié.
        /// </summary>
        /// <typeparam name="T">Le type dans lequel la valeur du champ doit être convertie.</typeparam>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ, convertie en type T.</returns>
        /// <exception cref="InvalidCastException">Lancée lorsque la valeur du champ ne peut pas être convertie au type T. L'exception contiendra des détails sur le type réel de l'objet et le type auquel la conversion a été tentée.</exception>
        public T Get<T>(int columnIndex)
        {
            return Cast<T>(Get(columnIndex));
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne, et tente de la convertir au type spécifié.
        /// </summary>
        /// <typeparam name="T">Le type dans lequel la valeur du champ doit être convertie.</typeparam>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ, convertie en type T.</returns>
        /// <exception cref="InvalidCastException">Lancée lorsque la valeur du champ ne peut pas être convertie au type T. L'exception contiendra des détails sur le type réel de l'objet et le type auquel la conversion a été tentée.</exception>
        public T Get<T>(string columnLabel)
        {
            return Cast<T>(Get(columnLabel));
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne et de son index de ligne, et tente de la convertir au type spécifié.
        /// </summary>
        /// <typeparam name="T">Le type dans lequel la valeur du champ doit être convertie.</typeparam>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ, convertie en type T.</returns>
        /// <exception cref="InvalidCastException">Lancée lorsque la valeur du champ ne peut pas être convertie au type T. L'exception contiendra des détails sur le type réel de l'objet et le type auquel la conversion a été tentée.</exception>
        public T Get<T>(int rowIndex, int columnIndex)
        {
            return Cast<T>(Get(rowIndex, columnIndex));
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne et de son index de ligne, et tente de la convertir au type spécifié.
        /// </summary>
        /// <typeparam name="T">Le type dans lequel la valeur du champ doit être convertie.</typeparam>
        /// <param name="rowIndex">L'index de la ligne du champ à récupérer.</param>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer.</param>
        /// <returns>La valeur du champ, convertie en type T.</returns>
        /// <exception cref="InvalidCastException">Lancée lorsque la valeur du champ ne peut pas être convertie au type T. L'exception contiendra des détails sur le type réel de l'objet et le type auquel la conversion a été tentée.</exception>
        public T Get<T>(int rowIndex, string columnLabel)
        {
            return Cast<T>(Get(rowIndex, columnLabel));
        }

        /// <summary>
        /// Convertit la valeur en type T et lance une exception détaillée si la conversion échoue.
        /// </summary>
        /// <typeparam name="T">Le type dans lequel la valeur doit être convertie.</typeparam>
        /// <param name="value">La valeur à convertir en type T.</param>
        /// <returns>La valeur convertie en type T.</returns>
        /// <exception cref="InvalidCastException">Lancée lorsque la valeur du champ ne peut pas être convertie au type T. L'exception contiendra des détails sur le type réel de l'objet et le type auquel la conversion a été tentée.</exception>
        private T Cast<T>(object value)
        {
            try
            {
                return (T)value;
            }
            catch (InvalidCastException)
            {
                throw new InvalidCastException("Unable to cast " + value.GetType().Name + " to " + typeof(T).Name);
            }
        }

        /// <summary>
        /// Récupère le nom de colonne à partir de son index
        /// </summary>
        /// <param name="columnIndex">L'index de colonne</param>
        /// <returns>Le nom de colonne</returns>
        public string GetColumnLabel(int columnIndex)
        {
            return GetColumnCount() > columnIndex ? Values.Keys.ElementAt(columnIndex) : null;
        }

        /// <summary>
        /// Récupère l'index de colonne à partir de son nom
        /// Retourne -1 si aucun colonne possède le nom
        /// </summary>
        /// <param name="columnLabel">Le nom de colonne</param>
        /// <returns>L'index de colonne ou -1</returns>
        public int GetColumnIndex(string columnLabel)
        {
            return new List<string>(Values.Keys).IndexOf(columnLabel);
        }

        /// <summary>
        /// Récupère le nombre de colonnes de la table
        /// </summary>
        /// <returns>Le nombre de colonnes</returns>
        public int GetColumnCount()
        {
            return Values.Count;
        }

        /// <summary>
        /// Récupère le nombre de lignes de la table
        /// </summary>
        /// <returns>Le nombre de lignes</returns>
        public int GetRowCount()
        {
            return IsEmpty() ? 0 : Values.ElementAt(0).Value.Count;
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
                    row += Get(rowIndex, columnIndex) + ", ";
                }

                row += Get(rowIndex, GetColumnCount() - 1) + "]";
                lines.Add(row);
            }

            foreach (string line in lines) Console.WriteLine(line);
        }
    }
}