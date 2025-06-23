using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using CourseWork.Departament;

namespace CourseWork
{
    public partial class NewEducationDepartament : Window
    {
        private readonly List<string> positions = new List<string>
        {
            "Профессор",
            "Доцент",
            "Старший преподаватель",
            "Преподаватель",
            "Ассистент"
        };

        private readonly List<string> academicDegrees = new List<string>
        {
            "Доктор наук",
            "Кандидат наук",
            "Без степени"
        };

        private readonly List<string> subjects = new List<string>
        {
            "ИТ",
            "Технология программирования",
            "Программирование",
            "Базы данных"
        };

        private readonly List<string> publicWorks = new List<string>
        {
            "Член ученого совета",
            "Председатель методической комиссии",
            "Куратор студенческой группы",
            "Ответственный за профориентацию",
            "Член приемной комиссии"
        };

        private readonly string connect = @"Data Source=DB.db";

        public NewEducationDepartament()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += Window_MouseLeftButtonDown;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void CloseButton_MouseDown(object sender, MouseButtonEventArgs e) => this.Hide();
        private void MinButton_MouseDown(object sender, MouseButtonEventArgs e) => this.WindowState = WindowState.Minimized;

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {

            EducationalDepartament department = new EducationalDepartament(
                firstName: TextBox_First_Name.Text.Trim(),
                lastName: TextBox_Second_Name.Text.Trim(),
                middleName: TextBox_Midle.Text.Trim(),
                login: TextBox_Login.Text.Trim(),
                password: TextBox_Password.Text.Trim()
            );

            if (ValidateDepartmentData(department))
            {
                try
                {
                    department.Add();
                    MessageBox.Show("Сотрудник успешно добавлен!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении сотрудника: {ex.Message}");
                }
            }
        }

private bool ValidateDepartmentData(EducationalDepartament department)
        {
            // Валидация имени
            if (string.IsNullOrEmpty(department.FirstName))
            {
                MessageBox.Show("Поле имени не может быть пустым.");
                return false;
            }

            if (department.FirstName.Length > 50 || !Regex.IsMatch(department.FirstName, @"^[а-яА-ЯЁё-]+$"))
            {
                MessageBox.Show("Имя должно быть на русском, может содержать дефис, без цифр и спецсимволов (макс. 50 символов).");
                return false;
            }

            // Валидация фамилии
            if (string.IsNullOrEmpty(department.LastName))
            {
                MessageBox.Show("Поле фамилии не может быть пустым.");
                return false;
            }

            if (department.LastName.Length > 50 || !Regex.IsMatch(department.LastName, @"^[а-яА-ЯЁё-]+$"))
            {
                MessageBox.Show("Фамилия должна быть на русском, может содержать дефис, без цифр и спецсимволов (макс. 50 символов).");
                return false;
            }

            // Валидация отчества (необязательное поле)
            if (!string.IsNullOrEmpty(department.MiddleName) &&
                (department.MiddleName.Length > 50 || !Regex.IsMatch(department.MiddleName, @"^[а-яА-ЯЁё-]+$")))
            {
                MessageBox.Show("Отчество должно быть на русском, может содержать дефис, без цифр и спецсимволов (макс. 50 символов).");
                return false;
            }

            // Валидация логина
            if (string.IsNullOrEmpty(department.Login))
            {
                MessageBox.Show("Поле логина не может быть пустым.");
                return false;
            }

            if (department.Login.Length < 4 || department.Login.Length > 20 || !Regex.IsMatch(department.Login, @"^[a-zA-Z0-9_]+$"))
            {
                MessageBox.Show("Логин должен содержать 4-20 символов (латинские буквы, цифры и подчеркивание).");
                return false;
            }

            // Валидация пароля
            if (string.IsNullOrEmpty(department.Password))
            {
                MessageBox.Show("Поле пароля не может быть пустым.");
                return false;
            }

            if (department.Password.Length < 6 || department.Password.Length > 15)
            {
                MessageBox.Show("Пароль должен содержать от 6 до 15 символов.");
                return false;
            }

            return true;
        }
        private void ToolBar_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}