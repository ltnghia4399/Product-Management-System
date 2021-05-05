using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using Bunifu.Connection;

namespace Bunifu.DataProvider
{
    class DataProvider
    {

        private static DataProvider instance;

        public static DataProvider Instance
        {
            get
            { if (instance == null)
                {
                    instance = new DataProvider();
                }
                return instance;
            }

            private set { instance = value; }
        }

        private DataProvider() { }

        /// <summary>
        /// Select
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable data = new DataTable();

            using (MySqlConnection conn = DBGetConnection.GetDBConnection())
            {
                try
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    if (parameter != null)
                    {
                        string[] listPara = query.Split(' ');
                        int i = 0;
                        foreach (string item in listPara)
                        {
                            if (item.Contains('@'))
                            {
                                cmd.Parameters.AddWithValue(item, parameter[i]);
                                i++;
                            }
                        }
                    }

                    MySqlDataAdapter adap = new MySqlDataAdapter(cmd);

                    try
                    {
                        adap.Fill(data);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error : " + ex.Message, "Cant connect with database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

            return data;

        }

        /// <summary>
        /// Insert, Update, Delete
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>

        public int ExecuteNonQuery(string query, object[] parameter = null)
        {
            int data = 0;

            using (MySqlConnection conn = DBGetConnection.GetDBConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, conn);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }

                data = cmd.ExecuteNonQuery();

                conn.Close();
            }

            return data;
        }

        /// <summary>
        /// Select Count*
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public object ExecuteScalar(string query, object[] parameter = null)
        {
            object data = 0;

            using (MySqlConnection conn = DBGetConnection.GetDBConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, conn);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }

                data = cmd.ExecuteScalar();

                conn.Close();
            }

            return data;
        }

        /// <summary>
        /// Get String values from database
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public string GetFieldValues(string query)
        {
            string mString = null;

            using (MySqlConnection conn = DBGetConnection.GetDBConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, conn);

                MySqlDataReader reader;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mString = reader.GetValue(0).ToString();

                }
                reader.Close();

                conn.Close();
            }

            return mString;
        }


        /// <summary>
        /// Format Date 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public string ConvertDateTime(string date)
        {
            string[] elements = date.Split('/');
            string dt = string.Format("{0}/{1}/{2}", elements[0], elements[1], elements[2]);
            return dt;

        }

        /// <summary>
        /// Convert AM to PM
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        public string ConvertTimeTo24(string hour)
        {
            string h = "";
            switch (hour)
            {
                case "1":
                    h = "13";
                    break;
                case "2":
                    h = "14";
                    break;
                case "3":
                    h = "15";
                    break;
                case "4":
                    h = "16";
                    break;
                case "5":
                    h = "17";
                    break;
                case "6":
                    h = "18";
                    break;
                case "7":
                    h = "19";
                    break;
                case "8":
                    h = "20";
                    break;
                case "9":
                    h = "21";
                    break;
                case "10":
                    h = "22";
                    break;
                case "11":
                    h = "23";
                    break;
                case "12":
                    h = "0";
                    break;
            }
            return h;
        }


        /// <summary>
        /// Hàm tạo ID có dạng: Ngaythangnam_giophutgiay
        /// </summary>
        /// <param name="tiento"></param>
        /// <returns></returns>
        public string CreateKey(string tiento)
        {
            string key = tiento;
            try
            {

                string[] partsDay;
                //partsDay = DateTime.Now.ToShortDateString().Split('/');
                partsDay = DateTime.Now.ToString("yyyy-MM-dd").Split('-');
                //Ví dụ 07/08/2009
                string d = String.Format("{0}{1}{2}", partsDay[0], partsDay[1], partsDay[2]);
                key = key + d;
                string[] partsTime;
                partsTime = DateTime.Now.ToLongTimeString().Split(':');
                //Ví dụ 7:08:03 PM hoặc 7:08:03 AM
                if (partsTime[2].Substring(3, 2) == "PM")
                    partsTime[0] = ConvertTimeTo24(partsTime[0]);
                if (partsTime[2].Substring(3, 2) == "AM")
                    if (partsTime[0].Length == 1)
                        partsTime[0] = "0" + partsTime[0];
                //Xóa ký tự trắng và PM hoặc AM
                partsTime[2] = partsTime[2].Remove(2, 3);
                string t;
                t = String.Format("_{0}{1}{2}", partsTime[0], partsTime[1], partsTime[2]);
                key = key + t;

            }
            catch (Exception)
            {
                MessageBox.Show("Cant create invoice ID\nPlease change format day to YYYY-MM-DD", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return key;

        }
    }

}

