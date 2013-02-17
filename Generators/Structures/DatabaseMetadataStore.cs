using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ObliteracyDotNet.Generators.Structures
{
    public class DatabaseMetadataStore
    {
        public static DataSet GetMetadata(string server, string instance)
        {
            DataSet metadata = new DataSet();
            SqlConnection connection = new SqlConnection(string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True", server, instance));
            connection.Open();
            foreach (DataRow item in connection.GetSchema().Rows)
            {
                DataTable t = connection.GetSchema(item[0].ToString());
                t.TableName = item[0].ToString();
                metadata.Tables.Add(t);
            }
            // Now create the "view" that contains all of the goods that are needed to create classes
            metadata.Tables.Add(CreateTableView(metadata));
            connection.Close();
            return metadata;
        }

        private static DataTable CreateTableView(DataSet set)
        {
            DataTable t = new DataTable();
            t.TableName = "TableColumnView";
            t.Columns.Add(new DataColumn("TableName", typeof(string)));
            t.Columns.Add(new DataColumn("ColumnName", typeof(string)));
            t.Columns.Add(new DataColumn("ColumnDataType", typeof(string)));
            foreach (DataRow row in set.Tables["Tables"].Rows)
            {
                set.Tables["Columns"].DefaultView.RowFilter = string.Format("TABLE_NAME = '{0}'", row[2].ToString());
                DataTable cols = set.Tables["Columns"].DefaultView.ToTable();
                foreach (DataRow cRow in cols.Rows)
                {
                    DataRow dr = t.NewRow();
                    dr[0] = row[2].ToString();
                    dr[1] = cRow[3].ToString();
                    set.Tables["DataTypes"].DefaultView.RowFilter = string.Format("TypeName = '{0}'", cRow[7].ToString());
                    dr[2] = set.Tables["DataTypes"].DefaultView.ToTable().Rows[0].ItemArray[5].ToString();
                    if (cRow[6].ToString() == "YES" && dr[2].ToString() != "System.String")
                        dr[2] += "?";
                    t.Rows.Add(dr);
                }
            }
            return t;
        }
    }
}
