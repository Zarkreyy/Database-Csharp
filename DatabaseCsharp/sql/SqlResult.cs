/*
 * Projet : Projet base de données en C#
 * Description : Classe représentant les résultats d'une requête SQL.
 * Date de création : 30/03/2023
 * Auteur : Rémy / Zarkrey
 * Version : 1.1.0
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
        public object GetObject(int rowIndex, string columnLabel)
        {
            return GetRowCount() > rowIndex && Values.ContainsKey(columnLabel) ? Values[columnLabel][rowIndex] : null;
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
                    row += GetObject(rowIndex, columnIndex) + ", ";
                }

                row += GetObject(rowIndex, GetColumnCount() - 1) + "]";
                lines.Add(row);
            }

            foreach (string line in lines) Console.WriteLine(line);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne
        /// </summary>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public string GetString(int columnIndex)
        {
            return (string)GetObject(columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne
        /// </summary>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public string GetString(string columnLabel)
        {
            return (string)GetObject(columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public string GetString(int rowIndex, int columnIndex)
        {
            return (string)GetObject(rowIndex, columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public string GetString(int rowIndex, string columnLabel)
        {
            return (string)GetObject(rowIndex, columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne
        /// </summary>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public char GetChar(int columnIndex)
        {
            return (char)GetObject(columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne
        /// </summary>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public char GetChar(string columnLabel)
        {
            return (char)GetObject(columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public char GetChar(int rowIndex, int columnIndex)
        {
            return (char)GetObject(rowIndex, columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public char GetChar(int rowIndex, string columnLabel)
        {
            return (char)GetObject(rowIndex, columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne
        /// </summary>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public bool GetBool(int columnIndex)
        {
            return (bool)GetObject(columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne
        /// </summary>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public bool GetBool(string columnLabel)
        {
            return (bool)GetObject(columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public bool GetBool(int rowIndex, int columnIndex)
        {
            return (bool)GetObject(rowIndex, columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public bool GetBool(int rowIndex, string columnLabel)
        {
            return (bool)GetObject(rowIndex, columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne
        /// </summary>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public long GetLong(int columnIndex)
        {
            return (long)GetObject(columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne
        /// </summary>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public long GetLong(string columnLabel)
        {
            return (long)GetObject(columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public long GetLong(int rowIndex, int columnIndex)
        {
            return (long)GetObject(rowIndex, columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public long GetLong(int rowIndex, string columnLabel)
        {
            return (long)GetObject(rowIndex, columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne
        /// </summary>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public int GetInt(int columnIndex)
        {
            return (int)GetObject(columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne
        /// </summary>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public int GetInt(string columnLabel)
        {
            return (int)GetObject(columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public int GetInt(int rowIndex, int columnIndex)
        {
            return (int)GetObject(rowIndex, columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public int GetInt(int rowIndex, string columnLabel)
        {
            return (int)GetObject(rowIndex, columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne
        /// </summary>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public short GetShort(int columnIndex)
        {
            return (short)GetObject(columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne
        /// </summary>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public short GetShort(string columnLabel)
        {
            return (short)GetObject(columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public short GetShort(int rowIndex, int columnIndex)
        {
            return (short)GetObject(rowIndex, columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public short GetShort(int rowIndex, string columnLabel)
        {
            return (short)GetObject(rowIndex, columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne
        /// </summary>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public sbyte GetSByte(int columnIndex)
        {
            return (sbyte)GetObject(columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne
        /// </summary>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public sbyte GetSByte(string columnLabel)
        {
            return (sbyte)GetObject(columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public sbyte GetSByte(int rowIndex, int columnIndex)
        {
            return (sbyte)GetObject(rowIndex, columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public sbyte GetSByte(int rowIndex, string columnLabel)
        {
            return (sbyte)GetObject(rowIndex, columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne
        /// </summary>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public ulong GetULong(int columnIndex)
        {
            return (ulong)GetObject(columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne
        /// </summary>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public ulong GetULong(string columnLabel)
        {
            return (ulong)GetObject(columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public ulong GetULong(int rowIndex, int columnIndex)
        {
            return (ulong)GetObject(rowIndex, columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public ulong GetULong(int rowIndex, string columnLabel)
        {
            return (ulong)GetObject(rowIndex, columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne
        /// </summary>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public uint GetUInt(int columnIndex)
        {
            return (uint)GetObject(columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne
        /// </summary>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public uint GetUInt(string columnLabel)
        {
            return (uint)GetObject(columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public uint GetUInt(int rowIndex, int columnIndex)
        {
            return (uint)GetObject(rowIndex, columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public uint GetUInt(int rowIndex, string columnLabel)
        {
            return (uint)GetObject(rowIndex, columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne
        /// </summary>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public ushort GetUShort(int columnIndex)
        {
            return (ushort)GetObject(columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne
        /// </summary>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public ushort GetUShort(string columnLabel)
        {
            return (ushort)GetObject(columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public ushort GetUShort(int rowIndex, int columnIndex)
        {
            return (ushort)GetObject(rowIndex, columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public ushort GetUShort(int rowIndex, string columnLabel)
        {
            return (ushort)GetObject(rowIndex, columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne
        /// </summary>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public byte GetByte(int columnIndex)
        {
            return (byte)GetObject(columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne
        /// </summary>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public byte GetByte(string columnLabel)
        {
            return (byte)GetObject(columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public byte GetByte(int rowIndex, int columnIndex)
        {
            return (byte)GetObject(rowIndex, columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public byte GetByte(int rowIndex, string columnLabel)
        {
            return (byte)GetObject(rowIndex, columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne
        /// </summary>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public double GetDouble(int columnIndex)
        {
            return (double)GetObject(columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne
        /// </summary>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public double GetDouble(string columnLabel)
        {
            return (double)GetObject(columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public double GetDouble(int rowIndex, int columnIndex)
        {
            return (double)GetObject(rowIndex, columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public double GetDouble(int rowIndex, string columnLabel)
        {
            return (double)GetObject(rowIndex, columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne
        /// </summary>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public float GetFloat(int columnIndex)
        {
            return (float)GetObject(columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne
        /// </summary>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public float GetFloat(string columnLabel)
        {
            return (float)GetObject(columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public float GetFloat(int rowIndex, int columnIndex)
        {
            return (float)GetObject(rowIndex, columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public float GetFloat(int rowIndex, string columnLabel)
        {
            return (float)GetObject(rowIndex, columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne
        /// </summary>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public DateTime GetDateTime(int columnIndex)
        {
            return (DateTime)GetObject(columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne
        /// </summary>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public DateTime GetDateTime(string columnLabel)
        {
            return (DateTime)GetObject(columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public DateTime GetDateTime(int rowIndex, int columnIndex)
        {
            return (DateTime)GetObject(rowIndex, columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public DateTime GetDateTime(int rowIndex, string columnLabel)
        {
            return (DateTime)GetObject(rowIndex, columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne
        /// </summary>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public TimeSpan GetTimeSpan(int columnIndex)
        {
            return (TimeSpan)GetObject(columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne
        /// </summary>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public TimeSpan GetTimeSpan(string columnLabel)
        {
            return (TimeSpan)GetObject(columnLabel);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son index de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnIndex">L'index de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public TimeSpan GetTimeSpan(int rowIndex, int columnIndex)
        {
            return (TimeSpan)GetObject(rowIndex, columnIndex);
        }

        /// <summary>
        /// Récupère la valeur d'un champ à partir de son nom de colonne et de son index de ligne
        /// </summary>
        /// <param name="rowIndex">L'index de ligne du champ à récupérer</param>
        /// <param name="columnLabel">Le nom de colonne du champ à récupérer</param>
        /// <returns>La valeur du champ</returns>
        public TimeSpan GetTimeSpan(int rowIndex, string columnLabel)
        {
            return (TimeSpan)GetObject(rowIndex, columnLabel);
        }
    }
}