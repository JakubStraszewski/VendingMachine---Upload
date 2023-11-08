using Oracle.ManagedDataAccess.Client;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VendingMachineSYS
{
    internal class MenuItem
    {
        private int menuItemId;
        private string name;
        private float price;
        private string description;
        private int catId;
        private float calories;

        public MenuItem(int menuItemId, string name, float price, string description, int catId, float calories)
        {
            this.menuItemId = menuItemId;
            this.name = name;
            this.price = price;
            this.description = description;
            this.catId = catId;
            this.calories = calories;
        }

        public void DeduceID()
        {
            OracleConnection connection = new OracleConnection(Program.connectionStr);
            try
            {
                connection.Open();
            } catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to the database.\n\n" + ex.Message + "\n\n" + ex.StackTrace);
            }
            int numItems = 0;
            DataSet dataSet = new DataSet("Unnamed");
            OracleDataAdapter da = new OracleDataAdapter("SELECT * FROM MENU", connection);
            try
            {
                da.Fill(dataSet);
                numItems = dataSet.Tables[0].Rows.Count;  
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to deduce ID for an instance of \"MenuItem\".\n\n" + ex.Message + "\n\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (numItems > 0)
            {
                try
                {
                    OracleDataReader r = new OracleCommand("SELECT MAX(MENUID) FROM MENU", connection).ExecuteReader();
                    r.Read();
                    menuItemId = r.GetInt32(0)+1;
                    r.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to deduce ID for an instance of \"MenuItem\".\n\n" + ex.Message + "\n\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    menuItemId = -1;
                }
            }
            else
            {
                menuItemId = 0;
            }
            connection.Close();
        }
        public bool AddMenuItem()
        {
            string commandStr = "INSERT INTO MENU VALUES(" + menuItemId + ", '" + name + "', " + price + ", '" + description + "', " + catId + ", " + calories + ")";
            try
            {
                OracleConnection connection = new OracleConnection(Program.connectionStr);
                connection.Open();
                OracleCommand command = new OracleCommand(commandStr, connection);
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            } catch (Exception ex)
            {
                MessageBox.Show(commandStr + ": Failed to perform database operation.\n\n" + ex.Message + "\n\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }
        public bool UpdateMenuItem()
        {
            string commandStr = "UPDATE MENU SET MENUID=" + menuItemId + ", NAME='" + name + "', PRICE=" + price + ", DESCRIPTION='" + description + "', CATID=" + catId + ", CALORIES=" + calories + " WHERE MENUID=" + menuItemId;
            try
            {
                OracleConnection connection = new OracleConnection(Program.connectionStr);
                connection.Open();
                OracleCommand command = new OracleCommand(commandStr, connection);
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(commandStr + ": Failed to perform database operation.\n\n" + ex.Message + "\n\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }
        public bool DeleteMenuItem()
        {
            string commandStr = "DELETE FROM MENU WHERE MENUID=" + menuItemId;
            try
            {
                OracleConnection connection = new OracleConnection(Program.connectionStr);
                connection.Open();
                OracleCommand command = new OracleCommand(commandStr, connection);
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(commandStr + ": Failed to perform database operation.\n\n" + ex.Message + "\n\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }
    }
}
