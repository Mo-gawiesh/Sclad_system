using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Reflection;
using Microsoft.VisualBasic.ApplicationServices;

namespace Sclad_system
{
    internal class Mainclass
    {
        //Copy the data from Restaurant mamgement class
        public static readonly string con_string = "Data Source=DESKTOP-JNSHVQ2\\SQLEXPRESS initial Catalog=Sclad;Persist Security Info-true; User ID=sa; password=123;";
        public static SqlConnection con = new SqlConnection(con_string);

        //Methord to check user validation

        public static bool IsValidUser(string user, string pass)
        {
            bool isValid = false;

            string qry = @"select * from users where username ='" + user + "' and upass = '" + pass + "' ";
            SqlCommand cmd = new SqlCommand(qry, con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                isValid = true;
                user = dt.Rows[0]["uName"].ToString();
            }


            return isValid;
        }

        public static void StopBuffering(Panel ctr, bool doubleBuffer)
        {
            try
            {
                typeof(Control).InvokeMember("DoubleBuffered",
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                    null, ctr, new object[] { doubleBuffer });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        //Create preoprty for username

        
        private static DataGridViewCellFormattingEventHandler gv_cellFormatting;
        public static string user;
        public static string USER
        {
            get { return user; }
            private set { user = value; }
        }

        public static Image imge;
        public static Image IMG
        {
            get { return imge; }
            private set { imge = value; }
        }

        // for user image

        //Method for curd operation

        public static int SQl(string qry, Hashtable ht)
        {
            int res = 0;

            try
            {
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.CommandType = CommandType.Text;

                foreach (DictionaryEntry item in ht)
                {
                    cmd.Parameters.AddWithValue(item.Key.ToString(), item.Value);
                }
                if (con.State == ConnectionState.Closed) { con.Open(); }
                res = cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open) { con.Close(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                con.Close();
            }
            return res;
        }

        // for loading data from database

        public static void LoadData(string qry, DataGridView gv, ListBox lb)
        {
            // Serial no in gridview

            gv.CellFormatting += new DataGridViewCellFormattingEventHandler(gv_cellFormatting);
            try
            {
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                da.Fill(dt);

                for (int i = 0; i < lb.Items.Count; i++)
                {
                    string colName1 = ((DataGridViewColumn)lb.Items[i]).Name;
                    gv.Columns[colName1].DataPropertyName = dt.Columns[i].ToString();
                }

                gv.DataSource = dt;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString());
                con.Close();
            }
        }

        private static void Gv_cellformatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            
              Guna.UI2.WinForms.Guna2DataGridView gv = (Guna.UI2.WinForms.Guna2DataGridView)sender;
              int count = 0;

            foreach (DataGridViewRow row in gv.Rows) 
            {

                count++;
                row.Cells[0].Value = count;
            }
        }

        public static void BlureBackground(Form model) 
        { 
           Form Background = new Form();
            using (model) 
            {
                Background.StartPosition = FormStartPosition.Manual;
                Background.FormBorderStyle = FormBorderStyle.None;
                Background.Opacity= 0.5d;
                Background.BackColor = Color.Black;
                Background.Size = FrmMain.Instance.Size;
                Background.Location=FrmMain.Instance.Location;
                Background.ShowInTaskbar= false;
                Background.Show();
                model.Owner= Background;
                model.ShowDialog(Background);
                Background.Dispose();
            }
        
        }
        
        // for cb fill

       
    }
}
