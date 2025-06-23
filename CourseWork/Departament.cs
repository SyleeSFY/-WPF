using System;
using System.Data.SQLite;

namespace CourseWork.Departament
{
    public class Department : Course_work.User  
    {
        private readonly string connect = @"Data Source=DB.db";

       
        public string Position { get; set; }
        public string AcademicDegree { get; set; }
        public string Subjects { get; set; }
        public int WorkloadHours { get; set; }
        public string PublicWork { get; set; }
        public bool MainJob { get; set; }

        // Конструктор с параметрами для User + Department
        public Department(string firstName, string lastName, string middleName,
                         string position, string academicDegree, string subjects,
                         int workloadHours, string publicWork, bool mainJob)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Position = position;
            AcademicDegree = academicDegree;
            Subjects = subjects;
            WorkloadHours = workloadHours;
            PublicWork = publicWork;
            MainJob = mainJob;
        }

        // Реализация абстрактного метода Add() из User
        public override void Add()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connect))
            {
                connection.Open();

                string sql = @"INSERT INTO Departament 
                             (FirstName, LastName, MiddleName, Position, AcademicDegree, 
                              Subjects, WorkloadHours, PublicWork, MainJob) 
                             VALUES 
                             (@FirstName, @LastName, @MiddleName, @Position, @AcademicDegree, 
                              @Subjects, @WorkloadHours, @PublicWork, @MainJob)";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@MiddleName", MiddleName);
                    command.Parameters.AddWithValue("@Position", Position);
                    command.Parameters.AddWithValue("@AcademicDegree", AcademicDegree);
                    command.Parameters.AddWithValue("@Subjects", Subjects);
                    command.Parameters.AddWithValue("@WorkloadHours", WorkloadHours);
                    command.Parameters.AddWithValue("@PublicWork", PublicWork);
                    command.Parameters.AddWithValue("@MainJob", MainJob ? 1 : 0);

                    command.ExecuteNonQuery();

                    // Получаем ID последней добавленной записи
                    if (ID == 0)
                    {
                        command.CommandText = "SELECT last_insert_rowid()";
                        ID = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
        }

        // Реализация абстрактного метода Update() из User
        public override void Update(int ID)
        {
            if (ID == 0) throw new Exception("ID не указан");

            using (SQLiteConnection connection = new SQLiteConnection(connect))
            {
                connection.Open();

                string sql = @"UPDATE Departament SET 
                             FirstName = @FirstName,
                             LastName = @LastName,
                             MiddleName = @MiddleName,
                             Position = @Position,
                             AcademicDegree = @AcademicDegree,
                             Subjects = @Subjects,
                             WorkloadHours = @WorkloadHours,
                             PublicWork = @PublicWork,
                             MainJob = @MainJob
                             WHERE ID = @ID";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ID", ID);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@MiddleName", MiddleName);
                    command.Parameters.AddWithValue("@Position", Position);
                    command.Parameters.AddWithValue("@AcademicDegree", AcademicDegree);
                    command.Parameters.AddWithValue("@Subjects", Subjects);
                    command.Parameters.AddWithValue("@WorkloadHours", WorkloadHours);
                    command.Parameters.AddWithValue("@PublicWork", PublicWork);
                    command.Parameters.AddWithValue("@MainJob", MainJob ? 1 : 0);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Реализация абстрактного метода Delete() из User
        public override void Delete(int ID)
        {
            if (ID == 0) throw new Exception("ID не указан");

            using (SQLiteConnection connection = new SQLiteConnection(connect))
            {
                connection.Open();
                string sql = "DELETE FROM Departament WHERE ID = @ID";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ID", ID);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("Запись не найдена или уже удалена");
                    }
                }
            }
        }

        // Статический метод для удаления по ID (оставлен без изменений)
        public static void DeleteById(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(@"Data Source=DB.db"))
            {
                connection.Open();
                string sql = "DELETE FROM Departament WHERE ID = @ID";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("Запись не найдена или уже удалена");
                    }
                }
            }
        }
    }
}