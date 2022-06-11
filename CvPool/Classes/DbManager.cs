using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace CvPool.Classes
{
    public class DbManager // DESKTOP-64K6Q5N\\SQLEXPRESS
    {
        private static readonly SqlConnection connection = new("Data Source = .; Initial Catalog = CvPoolDB; Integrated Security = True");

        public static void GetData(Control control, string tableName, string[] columns, string[] joinTables, string[] joinColumns, string joinValue, string[] whereColumns, object[] whereValues, bool isDistinct, bool sendDataMessage, JoinType? joinType = JoinType.Inner, string displayMember = null, string valueMember = null)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            string allColumns = string.Empty, allJoins = string.Empty, allWheres = string.Empty;

            if (columns != null)
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    if (i == columns.Length - 1)
                        allColumns += columns[i];
                    else
                        allColumns += columns[i] + ", ";
                }
            }

            if (joinTables != null)
            {
                for (int i = 0; i < joinTables.Length; i++)
                {
                    allJoins += $"{joinType.ToString().ToUpper()} JOIN {joinTables[i]} ON ({joinColumns[i]} = {joinValue} ";
                }
            }

            if (whereColumns != null)
            {
                for (int i = 0; i < whereColumns.Length; i++)
                {
                    if (whereColumns[i].Contains("Gender") || whereColumns[i].Contains("MaritalStatus") || whereColumns[i].Contains("DriverLicense"))
                    {
                        if (i == whereColumns.Length - 1)
                            allWheres += $"{whereColumns[i]} = {whereValues[i]}";
                        else
                            allWheres += $"{whereColumns[i]} = {whereValues[i]} OR ";
                    }
                    else
                    {
                        if (i == whereColumns.Length - 1)
                            allWheres += $"{whereColumns[i]} LIKE {whereValues[i]}";
                        else
                            allWheres += $"{whereColumns[i]} LIKE {whereValues[i]} OR ";
                    }
                }
            }

            string query = $"SELECT {(isDistinct ? "DISTINCT" : string.Empty)} {(columns == null ? "*" : allColumns)} FROM {tableName} {(joinColumns == null ? string.Empty : allJoins)} {(whereColumns == null ? string.Empty : "WHERE " + allWheres)}";

            switch (control)
            {
                case DataGridView:
                {
                    DataTable dgvTable = new();
                    SqlDataAdapter adapter = new(query, connection);
                    adapter.Fill(dgvTable);
                    (control as DataGridView).DataSource = dgvTable;

                    if (sendDataMessage)
                    {
                        switch (dgvTable.Rows.Count)
                        {
                            case 0: MessageBox.Show("Veri kaydı bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information); break;
                            default: MessageBox.Show($"{dgvTable.Rows.Count} adet verdi kaydı bulundu.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information); break;
                        }
                    }
                    break;
                }
                case ComboBox:
                {
                    DataTable cbTable = new(tableName);
                    SqlCommand command = new(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    cbTable.Load(reader);

                    (control as ComboBox).DataSource = cbTable;
                    if (!string.IsNullOrEmpty(displayMember))
                        (control as ComboBox).DisplayMember = displayMember;
                    if (!string.IsNullOrEmpty(valueMember))
                        (control as ComboBox).ValueMember = valueMember;
                    (control as ComboBox).SelectedIndex = -1;

                    break;
                }
                case ListView:
                {
                    SqlCommand command = new(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ListViewItem item = new(reader[0].ToString());
                        for (int i = 1; i < columns.Length; i++)
                        {
                            item.SubItems.Add(reader[i].ToString());
                        }
                        (control as ListView).Items.Add(item);
                    }

                    break;
                }
            }

            connection.Close();
        }

        public static string GetData(string tableName, string[] columns, string[] whereColumns, object[] whereValues)
        {
            string allWheres = string.Empty, allColumns = string.Empty;

            if (columns != null)
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    if (i == columns.Length - 1)
                        allColumns += columns[i];
                    else
                        allColumns += columns[i] + ", ";
                }
            }

            if (whereColumns != null)
            {
                for (int i = 0; i < whereColumns.Length; i++)
                {
                    if (i == whereColumns.Length - 1)
                        allWheres += $"{whereColumns[i]} = {whereValues[i]}";
                    else
                        allWheres += $"{whereColumns[i]} = {whereValues[i]} AND ";
                }
            }

            string query = $"SELECT {(columns == null ? "*" : allColumns)} FROM {tableName} {(whereColumns == null ? string.Empty : "WHERE " + allWheres)}";
            string value = string.Empty;

            connection.Open();
            SqlCommand command = new(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
                value = reader[0].ToString();
            connection.Close();

            return value;
        }

        public static void GetSelectedRowData(GroupBox groupBox, object[] controls, string tableName, string whereColumn, int whereValue, params string[] columns)
        {
            Utilities.GroupBoxProcess(groupBox);

            connection.Open();

            string tableColumns = string.Empty;
            List<string> values = new();

            for (int i = 0; i < columns.Length; i++)
            {
                if (i == columns.Length - 1)
                    tableColumns += columns[i];
                else
                    tableColumns += columns[i] + ", ";

                values.Add(columns[i]);
            }
            
            SqlCommand command = new($"SELECT {tableColumns} FROM {tableName} WHERE {whereColumn} = {whereValue}", connection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                for (int i = 0; i < controls.Length; i++)
                {
                    switch (controls[i])
                    {
                        case Label: (controls[i] as Label).Text = reader[values[i]].ToString(); break;
                        case TextBox: (controls[i] as TextBox).Text = reader[values[i]].ToString(); break;
                        case RichTextBox: (controls[i] as RichTextBox).Text = reader[values[i]].ToString(); break;
                        case ComboBox: (controls[i] as ComboBox).Text = reader[values[i]].ToString(); break;
                        case GroupBox: (controls[i] as GroupBox).Text += " " + reader[values[i]]; break;
                        case DateTimePicker: (controls[i] as DateTimePicker).Value = (DateTime)reader[values[i]]; break;
                        case NumericUpDown: (controls[i] as NumericUpDown).Value = (decimal)reader[values[i]]; break;
                        case PictureBox: (controls[i] as PictureBox).BackgroundImage = Image.FromFile(CvPool.imagesFolder + "\\" + reader[values[i]]); break;
                    }
                }
            }

            connection.Close();
        }

        public static int GetDataCount(string tableName, string columnName, string[] whereColumns, object[] whereValues, ArithmeticOperator[] arithmeticOperators, bool isDistinct = false, LogicOperator logicOperator = LogicOperator.And)
        {
            string allWheres = string.Empty;
            List<string> operatorTypes = new();

            if (arithmeticOperators != null)
            {
                for (int i = 0; i < arithmeticOperators.Length; i++)
                {
                    switch (arithmeticOperators[i])
                    {
                        case ArithmeticOperator.Equal: operatorTypes.Add("="); break;
                        case ArithmeticOperator.NotEqual: operatorTypes.Add("!="); break;
                        case ArithmeticOperator.LessThan: operatorTypes.Add("<"); ; break;
                        case ArithmeticOperator.LessThanOrEqual: operatorTypes.Add("<="); ; break;
                        case ArithmeticOperator.BiggerThan: operatorTypes.Add(">"); ; break;
                        case ArithmeticOperator.BiggerThanOrEqual: operatorTypes.Add(">="); ; break;
                    }
                }
            }

            if (whereColumns != null)
            {
                for (int i = 0; i < whereColumns.Length; i++)
                {
                    if (i == whereColumns.Length - 1)
                        allWheres += $"{whereColumns[i]} {operatorTypes[i]} {whereValues[i]}";
                    else
                        allWheres += $"{whereColumns[i]} {operatorTypes[i]} {whereValues[i]} {logicOperator.ToString().ToUpper()} ";
                }
            }

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            SqlCommand command = new($"SELECT {(isDistinct ? "DISTINCT" : string.Empty)} {(columnName == string.Empty ? "COUNT(*)" : columnName)} FROM {tableName} {(allWheres == string.Empty ? string.Empty : "WHERE " + allWheres)}", connection);
            SqlDataReader reader = command.ExecuteReader();
            int dataCount = 0;
            if (reader.Read())
                dataCount = (int)reader[0];

            connection.Close();

            return dataCount;
        }

        public static void AddData(string tableName, string columns, string values, string message = null)
        {
            connection.Open();

            SqlCommand query = new($"INSERT INTO {tableName} ({columns}) VALUES ({values})", connection);
            query.ExecuteNonQuery();

            if (message != null)
                MessageBox.Show($"{message}", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            connection.Close();
        }

        public static void UpdateData(string tableName, string[] columns, object[] values, string[] whereColumns, object[] whereValues, string message = null, GroupBox groupBoxToShow = null)
        {
            string columnsAndValues = string.Empty, allWheres = string.Empty;

            for (int i = 0; i < columns.Length; i++)
            {
                if (i == columns.Length - 1)
                    columnsAndValues += columns[i] + " = " + values[i];
                else
                    columnsAndValues += columns[i] + " = " + values[i] + ", ";
            }

            for (int i = 0; i < whereColumns.Length; i++)
            {
                if (i == whereColumns.Length - 1)
                    allWheres += $"{whereColumns[i]} = {whereValues[i]}";
                else
                    allWheres += $"{whereColumns[i]} = {whereValues[i]} AND ";
            }
            
            connection.Open();
            SqlCommand command = new($"UPDATE {tableName} SET {columnsAndValues} WHERE {allWheres}", connection);
            command.ExecuteNonQuery();

            if (message != null)
                MessageBox.Show($"{message}", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            connection.Close();

            if (groupBoxToShow != null)
                Utilities.GroupBoxProcess(groupBoxToShow);
        }

        public static void DeleteData(string tableName, string column, int value, string requiredId = null, int valueForId = 0, string message = null)
        {
            connection.Open();

            SqlCommand command = requiredId == null
                                 ? (new($"DELETE FROM {tableName} WHERE {column} = {value}", connection))
                                 : (new($"DELETE FROM {tableName} WHERE {column} = {value} AND {requiredId} = {valueForId}", connection));

            command.ExecuteNonQuery();

            if (message != null)
                MessageBox.Show($"{message}.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            connection.Close();
        }
    }
}