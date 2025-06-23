using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using CourseWork.Departament;

namespace CourseWork
{
    public partial class NewDepartament : Window
    {
        private readonly List<string> positions = new List<string>
        {
    "Лаборант",
    "Старший лаборант",
    "Профессор",
    "Доцент",
    "Старший преподаватель",
    "Научный сотрудник",
    "Ассистент-профессор" ,
    "Соискатель ученой степени",
    "Аспирант",
    "Докторант",
    "Без степени"
        };

        private readonly List<string> academicDegrees = new List<string>
        {
    "Доктор технических наук",
    "Кандидат технических наук",
    "Доктор наук",
    "Кандидат наук",
    "Без степени"
        };

        private readonly List<string> subjects = new List<string>
        {
    "ИТ",
    "Лаборатория информатизации",
    "Технология программирования",
    "Программирование",
    "Базы данных",
    "Алгоритмы и структуры данных",
    "Искусственный интеллект",
    "Операционные системы",
    "Компьютерные сети",
    "Сопровождение программного обеспечения",
    "Веб-разработка",
    "Кибербезопасность","Схемотехника","Микропроцессорные системы"
        };

        private readonly List<string> publicWorks = new List<string>
{"Заведующий кафедры", "Заместитель заведующего кафедрой",
          "Заместитель заведующего кафедрой по научной деятельности",
    "Член ученого совета",
    "Председатель методической комиссии",
    "Куратор студенческой группы",
    "Ответственный за профориентацию",
    "Член приемной комиссии",
    "Ответственный за практику",
    "Координатор международных программ",
    "Отсуствует" };
        private readonly string connect = @"Data Source=DB.db";

        public NewDepartament()
        {
            InitializeComponent();

            TextBox_Position.ItemsSource = positions;
            TextBox_Academic.ItemsSource = academicDegrees;
            TextBox_Dicipline.ItemsSource = subjects;
            TextBox_PublicWork.ItemsSource = publicWorks;

            TextBox_Position.SelectedIndex = 0;
            TextBox_Academic.SelectedIndex = 0;
            TextBox_Dicipline.SelectedIndex = 0;
            TextBox_PublicWork.SelectedIndex = 0;

            // Добавляем обработчик для перемещения окна
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
            if (!int.TryParse(TextBox_Hours.Text, out int hours))
            {
                MessageBox.Show("Пожалуйста, введите корректное количество часов");
                return;
            }

            Department department = new Department(
                firstName: TextBox_First_Name.Text.Trim(),
                lastName: TextBox_Second_Name.Text.Trim(),
                middleName: TextBox_Midle.Text.Trim(),
                position: TextBox_Position.Text.Trim(),
                academicDegree: TextBox_Academic.Text.Trim(),
                subjects: TextBox_Dicipline.Text.Trim(),
                workloadHours: hours,
                publicWork: TextBox_PublicWork.Text.Trim(),
                mainJob: CheckBox_AdditionalWork.IsChecked ?? false
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

        private bool ValidateDepartmentData(Department department)
        {
            if (string.IsNullOrEmpty(department.FirstName))
            {
                MessageBox.Show("Поле имени не может быть пустым.");
                return false;
            }

            if (department.FirstName.Length > 16 || !Regex.IsMatch(department.FirstName, @"^[а-яА-ЯЁё]+$"))
            {
                MessageBox.Show("Имя должно быть на русском, без цифр и спецсимволов (макс. 16 символов).");
                return false;
            }

            if (string.IsNullOrEmpty(department.LastName))
            {
                MessageBox.Show("Поле фамилии не может быть пустым.");
                return false;
            }

            if (department.LastName.Length > 16 || !Regex.IsMatch(department.LastName, @"^[а-яА-ЯЁё]+$"))
            {
                MessageBox.Show("Фамилия должна быть на русском, без цифр и спецсимволов (макс. 16 символов).");
                return false;
            }

            if (!string.IsNullOrEmpty(department.MiddleName) &&
                (department.MiddleName.Length > 16 || !Regex.IsMatch(department.MiddleName, @"^[а-яА-ЯЁё]+$")))
            {
                MessageBox.Show("Отчество должно быть на русском, без цифр и спецсимволов (макс. 16 символов).");
                return false;
            }

            if (string.IsNullOrEmpty(department.Position))
            {
                MessageBox.Show("Поле должности не может быть пустым.");
                return false;
            }

            if (department.WorkloadHours <= 0)
            {
                MessageBox.Show("Некорректное значение часов нагрузки.");
                return false;
            }

            return true;
        }

        private void ToolBar_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}