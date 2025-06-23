using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using Course_work;
using System.Net;
using System.Runtime.Remoting.Contexts;
using CourseWork;
using CourseWork.Departament;

namespace Course_work
{
    public partial class MainWindow : Window
    {
                string connection = @" Data Source=DB.db";
        string Login;
        int ID;

        string connect = @" Data Source=DB.db";
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Authorization_Click(object sender, RoutedEventArgs e)
        {
            // Получаем и очищаем введенные данные
            string login = TextBox_Login.Text.Trim();
            string password = TextBox_Password.Text.Trim();

            // Валидация ввода
            if (string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Логин не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пароль не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверка минимальной длины пароля
            if (password.Length < 5)
            {
                MessageBox.Show("Пароль должен содержать минимум 5 символов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connect))
                {
                    connection.Open();

                    if (!VerifyUserCredentials(login, password, connection))
                    {
                        MessageBox.Show("Неверный логин или пароль.", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    string role = GetUserRole(login, connection);
                    OpenAppropriateMenu(login, role);

                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при авторизации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool VerifyUserCredentials(string login, string password, SQLiteConnection connection)
        {
            const string query = "SELECT COUNT(1) FROM Staff WHERE Login = @Login AND Password = @Password";

            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@Password", password);

                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        private string GetUserRole(string login, SQLiteConnection connection)
        {
            const string query = "SELECT Role FROM Staff WHERE Login = @Login";

            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Login", login);
                object result = command.ExecuteScalar();
                return result?.ToString() ?? string.Empty;
            }
        }

        private void OpenAppropriateMenu(string login, string role)
        {
            switch (role)
            {
                case "Учебный отдел":
                    EducationDepartementWindow adminMenu = new EducationDepartementWindow();
                    adminMenu.Show();
                    break;

                case "Деканат":
                    DeanerWindow test = new DeanerWindow();
                    test.Show();
                    break;

              

                default:
                    MessageBox.Show("Ваша учетная запись не имеет прав доступа.", "Ошибка доступа", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
            }
        }

        private void CloseButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void MinButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ToolBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void TextBlock_GoToReg_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}