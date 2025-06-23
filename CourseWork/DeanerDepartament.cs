using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Course_work;
namespace CourseWork
{

    public class DeanerDepartament : Course_work.User
    {
        private readonly string connect = @"Data Source=DB.db";


        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }



        // Конструктор с параметрами для User + Department


        public DeanerDepartament(string firstName, string lastName, string middleName,
                 string login, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Login = login;
            Password = password;
            Role = "Деканат";

        }



        public override void Add()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connect))
            {
                connection.Open();


                string checkSql = "SELECT COUNT(*) FROM Staff WHERE Login = @Login";
                using (SQLiteCommand checkCommand = new SQLiteCommand(checkSql, connection))
                {
                    checkCommand.Parameters.AddWithValue("@Login", Login);
                    int existingCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (existingCount > 0)
                    {
                        throw new Exception("Пользователь с таким логином уже существует");
                    }
                }

                // If login doesn't exist, proceed with insertion
                string sql = @"INSERT INTO Staff 
                     (FirstName, LastName, MiddleName, Login, Password, 
                      Role) 
                     VALUES 
                     (@FirstName, @LastName, @MiddleName, @Login, @Password, 
                      @Role)";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@MiddleName", MiddleName);
                    command.Parameters.AddWithValue("@Login", Login);
                    command.Parameters.AddWithValue("@Password", Password);
                    command.Parameters.AddWithValue("@Role", Role);

                    command.ExecuteNonQuery();

                    if (ID == 0)
                    {
                        command.CommandText = "SELECT last_insert_rowid()";
                        ID = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
        }


        public override void Update(int ID)
        {
            if (ID == 0) throw new Exception("ID не указан");

            using (SQLiteConnection connection = new SQLiteConnection(connect))
            {
                connection.Open();

                // First check if login exists in other records
                string checkSql = "SELECT COUNT(*) FROM Staff WHERE Login = @Login AND ID != @ID";
                using (SQLiteCommand checkCommand = new SQLiteCommand(checkSql, connection))
                {
                    checkCommand.Parameters.AddWithValue("@Login", Login);
                    checkCommand.Parameters.AddWithValue("@ID", ID);
                    int existingCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (existingCount > 0)
                    {
                        throw new Exception("Пользователь с таким логином уже существует");
                    }
                }

                // If login is unique (or belongs to this record), proceed with update
                string sql = @"UPDATE Staff SET 
                     FirstName = @FirstName,
                     LastName = @LastName,
                     MiddleName = @MiddleName,
                     Login = @Login,
                     Password = @Password,
                     Role = @Role
                     WHERE ID = @ID";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ID", ID);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@MiddleName", MiddleName);
                    command.Parameters.AddWithValue("@Login", Login);
                    command.Parameters.AddWithValue("@Password", Password);
                    command.Parameters.AddWithValue("@Role", Role);

                    command.ExecuteNonQuery();
                }
            }
        }


        public override void Delete(int ID)
        {
            if (ID == 0) throw new Exception("ID не указан");

            using (SQLiteConnection connection = new SQLiteConnection(connect))
            {
                connection.Open();
                string sql = "DELETE FROM Staff WHERE ID = @ID";

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


        public static void DeleteById(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=DB.db"))
            {
                connection.Open();
                string sql = "DELETE FROM Staff WHERE ID = @ID";

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
