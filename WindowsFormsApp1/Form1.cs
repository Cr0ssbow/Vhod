using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        string connStr = "server=10.90.12.110;port=33333;user=st_4_20_2;database=is_4_20_st2_KURS;password=44556988";
        MySqlConnection conn;
        static string sha256(string randomString)
        {
            //Тут происходит криптографическая магия. Смысл данного метода заключается в том, что строка залетает в метод
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
        public void GetUserInfo(string login_user)
        {
            //Объявлем переменную для запроса в БД
            string selected_id_stud = metroTextBox1.Text;
            // устанавливаем соединение с БД
            conn.Open();
            // запрос
            string sql = $"SELECT Login FROM Data WHERE Login='{login_user}'";
            // объект для выполнения SQL-запроса
            MySqlCommand command = new MySqlCommand(sql, conn);
            // объект для чтения ответа сервера
            MySqlDataReader reader = command.ExecuteReader();
            // читаем результат
            while (reader.Read())
            {
                // элементы массива [] - это значения столбцов из запроса SELECT
                Auth.auth_id = reader[0].ToString();
                Auth.auth_fio = reader[1].ToString();
                Auth.auth_role = Convert.ToInt32(reader[4].ToString());
            }
            reader.Close(); // закрываем reader
            // закрываем соединение с БД
            conn.Close();
        }
        public Form1()
        {
            InitializeComponent();
        }
        private void metroButton2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
        private void metroButton1_Click_1(object sender, EventArgs e)
        {
            {
                {
                    //Запрос в БД на предмет того, если ли строка с подходящим логином и паролем
                    string sql = "SELECT Login FROM Data WHERE Login = @un and  Password= @up";
                    //Открытие соединения
                    conn.Open();
                    //Объявляем таблицу
                    DataTable table = new DataTable();
                    //Объявляем адаптер
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    //Объявляем команду
                    MySqlCommand command = new MySqlCommand(sql, conn);
                    //Определяем параметры
                    command.Parameters.Add("@un", MySqlDbType.VarChar, 25);
                    command.Parameters.Add("@up", MySqlDbType.VarChar, 25);
                    //Присваиваем параметрам значение
                    command.Parameters["@un"].Value = metroTextBox1.Text;
                    command.Parameters["@up"].Value = sha256(metroTextBox2.Text);
                    //Заносим команду в адаптер
                    adapter.SelectCommand = command;
                    //Заполняем таблицу
                    adapter.Fill(table);
                    //Закрываем соединение
                    conn.Close();
                    //Если вернулась больше 0 строк, значит такой пользователь существует
                    if (table.Rows.Count > 0)
                    {
                        //Присваеваем глобальный признак авторизации
                        Auth.auth = true;
                        //Достаем данные пользователя в случае успеха
                        GetUserInfo(metroTextBox1.Text);
                        //Закрываем форму
                        this.Close();
                    }
                    else
                    {
                        //Отобразить сообщение о том, что авторизаия неуспешна
                        MessageBox.Show("Неверные данные или обратитесь к системному администратору");
                    }
                }
            }


        }
        private void Form2_Load(object sender, EventArgs e)
        {
            //Инициализируем соединение с подходящей строкой
            conn = new MySqlConnection(connStr);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            metroTextBox1.Text = label2.Text;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            metroTextBox2.Text = label3.Text;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            metroTextBox2.Text = label4.Text;
        }

        private void label5_Click(object sender, EventArgs e)
        {
            metroTextBox1.Text = label5.Text;
        }

        private void label6_Click(object sender, EventArgs e)
        {
            metroTextBox2.Text = label6.Text;
        }

        private void label7_Click(object sender, EventArgs e)
        {
            metroTextBox1.Text = label7.Text;
        }


    }
}
