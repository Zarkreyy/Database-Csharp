using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

namespace DatabaseCsharp.sql
{
    public class SqlResult
    {
        private readonly List<Dictionary<string, object>> values;

        public SqlResult()
        {
            values = new List<Dictionary<string, object>>();
        }

        public SqlResult(MySqlDataReader reader)
        {
            values = new List<Dictionary<string, object>>();

            if (reader == null)
            {
                return;
            }

            while (reader.Read())
            {
                Dictionary<string, object> row = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string rowLabel = reader.GetName(i);
                    object rowItem = reader.GetValue(i);
                    row.Add(rowLabel, rowItem);
                }

                values.Add(row);
            }

            reader.Close();
        }

        public object GetObject(int columnIndex)
        {
            return GetObject(0, columnIndex);
        }

        public object GetObject(string columnLabel)
        {
            return GetObject(0, columnLabel);
        }

        public object GetObject(int rowIndex, int columnIndex)
        {
            if (GetRowCount() > rowIndex)
            {
                Dictionary<string, object> row = values[rowIndex];
                int i = 0;

                foreach (object item in row.Values)
                {
                    if (i == columnIndex)
                    {
                        return item;
                    }

                    i++;
                }
            }

            return null;
        }

        public object GetObject(int rowIndex, string columnLabel)
        {
            if (GetRowCount() > rowIndex)
            {
                Dictionary<string, object> row = values[rowIndex];

                if (row.ContainsKey(columnLabel))
                {
                    return row[columnLabel];
                }
            }

            return null;
        }

        public int GetColumnCount()
        {
            if (GetRowCount() > 0)
            {
                Dictionary<string, object> row = values[0];
                return row.Count;
            }

            return 0;
        }

        public int GetRowCount()
        {
            return values.Count;
        }

        public string GetColumnLabel(int columnIndex)
        {
            if (GetRowCount() > 0)
            {
                Dictionary<string, object>.KeyCollection labels = values[0].Keys;
                if (labels.Count > columnIndex) return labels.ElementAt(columnIndex);
            }

            return "null";
        }

        public bool IsEmpty()
        {
            return values.Count == 0;
        }

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