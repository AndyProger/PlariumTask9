using System;
using System.IO;
using System.Collections.Generic;
using UserSpace;
using System.Linq;

namespace PlariumTask7
{
    class UsersDataBase : DataBase
    {
        /// <summary>
        /// Добавить пользователя в БД
        /// </summary>
        /// <param name="user"> Пользователь </param>
        /// <param name="databaseName"> название БД </param>
        public static void AddUserToDatabase(User user, string databaseName)
        {
            using (StreamWriter sw = new StreamWriter(databaseName + ".txt", true))
            {
                sw.WriteLine(user.Name + " " + user.Surname + " " + user.Birthdate.ToString("MM dd yyyy"));
            }
        }

        /// <summary>
        /// Добавить список пользователей в БД
        /// </summary>
        /// <param name="users"> Список пользователей </param>
        /// <param name="databaseName"> название БД </param>
        public static void AddUsersToDatabase(List<User> users, string databaseName)
        {
            using (StreamWriter sw = new StreamWriter(databaseName + ".txt", true))
            {
                foreach(var user in users)
                {
                    sw.WriteLine(user.Name + " " + user.Surname + " " + user.Birthdate.ToString("MM dd yyyy"));
                }
            }
        }

        /// <summary>
        /// Получить список пользователей из БД
        /// </summary>
        /// <param name="databaseName"> название БД </param>
        /// <returns> список пользователей </returns>
        public static List<User> GetUsersFromDatabase(string databaseName)
        {
            List<User> list = new List<User>();

            using (StreamReader fs = new StreamReader(databaseName + ".txt"))
            {
                string str;

                while ((str = fs.ReadLine()) != null)
                {
                    string[] splitedString = str.Split(' ');

                    string name = splitedString[0];
                    string surname = splitedString[1];
                    int month = int.Parse(splitedString[2]);
                    int day = int.Parse(splitedString[3]);
                    int year = int.Parse(splitedString[4]);

                    list.Add(new User(name, surname, new DateTime(year, month, day)));
                }
            }

            return list;
        }

        /// <summary>
        /// Удалить пользователя из БД
        /// </summary>
        /// <param name="user"> Пользователь </param>
        /// <param name="databaseName"> название БД </param>
        public static void DeleteUserFromDatabase(User user, string databaseName)
        {
            string stringToDelete = user.Name + " " + user.Surname + " " + user.Birthdate.ToString("MM dd yyyy");
            string path = databaseName + ".txt";

            var tempFile = Path.GetTempFileName();
            var linesToKeep = File.ReadLines(path).Where(l => l != stringToDelete);

            File.WriteAllLines(tempFile, linesToKeep);

            File.Delete(path);
            File.Move(tempFile, path);
        }
    }
}
