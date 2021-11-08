using System.Collections.Generic;
using System.Linq;
using System.IO;
using LetterSpace;

namespace PlariumTask7
{
    class LettersDataBase : DataBase
    {
        /// <summary>
        /// Добавить письмо в БД
        /// </summary>
        /// <param name="letter"> Письмо </param>
        /// <param name="databaseName"> Название БД </param>
        public static void AddLetterToDatabase(Letter letter, string databaseName)
        {
            using (StreamWriter sw = new StreamWriter(databaseName + ".txt", true))
            {
                sw.WriteLine(letter.Topic + "|" + letter.Text);
            }
        }

        /// <summary>
        /// Добавить список писем
        /// </summary>
        /// <param name="letters"> Список писем</param>
        /// <param name="databaseName"> Название БД </param>
        public static void AddLettersToDatabase(List<Letter> letters, string databaseName)
        {
            using (StreamWriter sw = new StreamWriter(databaseName + ".txt", true))
            {
                foreach (var letter in letters)
                {
                    sw.WriteLine(letter.Topic + "|" + letter.Text);
                }
            }
        }

        /// <summary>
        /// Получить список писем из БД
        /// </summary>
        /// <param name="databaseName"> Название БД </param>
        /// <returns> Список писем </returns>
        public static List<Letter> GetLettersFromDatabase(string databaseName)
        {
            List<Letter> list = new List<Letter>();

            using (StreamReader fs = new StreamReader(databaseName + ".txt"))
            {
                string str;

                while ((str = fs.ReadLine()) != null)
                {
                    string[] splitedString = str.Split('|');

                    string topic = splitedString[0];
                    string text = splitedString[1];

                    list.Add(new Letter(topic, text));
                }
            }

            return list;
        }

        /// <summary>
        /// Удалить письмо из БД
        /// </summary>
        /// <param name="letter"> Письмо </param>
        /// <param name="databaseName"> Название БД </param>
        public static void DeleteLetterFromDatabase(Letter letter, string databaseName)
        {
            string stringToDelete = letter.Topic + "|" + letter.Text;
            string path = databaseName + ".txt";

            var tempFile = Path.GetTempFileName();
            var linesToKeep = File.ReadLines(path).Where(l => l != stringToDelete);

            File.WriteAllLines(tempFile, linesToKeep);

            File.Delete(path);
            File.Move(tempFile, path);
        }
    }
}
