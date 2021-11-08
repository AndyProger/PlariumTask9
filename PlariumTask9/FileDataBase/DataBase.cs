using System;
using System.IO;

namespace PlariumTask7
{
    /// <summary>
    /// Базовый класс БД, отвечает за создание и удаление БД
    /// </summary>
    class DataBase
    {
        /// <summary>
        /// Создание БД
        /// </summary>
        /// <param name="dataBaseName"> название БД </param>
        public static void CreateEmptyDatabase(string dataBaseName)
        {
            string name = dataBaseName + ".txt";

            if (!File.Exists(name))
            {
                // создаем и сразу закрываем (для избежания конфликтов при обращении)
                File.Create(name).Close();
            }
            else
            {
                name = string.Empty;
                throw new ArgumentException("Such file already exists!");
            }
        }

        /// <summary>
        /// Удаление БД
        /// </summary>
        /// <param name="dataBaseName"> название БД, которая будет удаленна </param>
        public static void DeleteDatabase(string dataBaseName)
        {
            string path = dataBaseName + ".txt";

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }
    }
}
