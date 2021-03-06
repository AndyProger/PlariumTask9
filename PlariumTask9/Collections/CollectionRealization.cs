using System.Linq;
using UserSpace;
using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace CollectionOfUsers
{
    partial class UsersCollection
    {
        // Класс для синхронизации потоков при записи / чтения в один файл
        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();

        /// <summary>
        /// Найти пользователя, длина писем которого наименьшая.
        /// </summary>
        public static User FindUserWithTheShortesLetter()
        {
            return Dictionary.Keys.AsParallel().SelectMany(u => u.Letters,
                                             (u, l) => new { User = u, Letter = l })
                                             .OrderBy(u => u.Letter.Text.Length)
                                             .Select(u => u.User).FirstOrDefault();
        }

        /// <summary>
        /// Информация о пользователях, а также количестве полученных и отправленных ими письмах
        /// </summary>
        public static string GetUsersInfo()
        {
            var usersInfo = Dictionary.Keys.AsParallel().Select(x => x + $"Sent letters: {x.LettersSent}\n" +
                    $"Received letters: {x.LettersReceived}\n\n");

            return string.Join(" ", usersInfo);
        }

        /// <summary>
        /// Информация о пользователях, которые получили хотя бы одно сообщение с заданной темой
        /// </summary>
        public static string GetUsersWithSuchTopic(string topic)
        {
            var users = Dictionary.Keys.AsParallel().SelectMany(u => u.Letters,
                                                (u, l) => new { User = u, Letter = l })
                                                .Where(u => u.Letter.Topic == topic)
                                                .Select(u => u.User);

            return string.Join(" ", users);
        }

        /// <summary>
        /// Информация о пользователях, которые не получали сообщения с заданной темой
        /// </summary>
        public static string GetUsersWithoutSuchTopic(string topic)
        {
            var users = Dictionary.Keys.AsParallel().Except(Dictionary.Keys.AsParallel()
                                                .SelectMany(u => u.Letters,
                                                (u, l) => new { User = u, Letter = l })
                                                .Where(u => u.Letter.Topic == topic)
                                                .Select(u => u.User).Distinct());

            return string.Join(" ", users);
        }

        /// <summary>
        /// Запись в один файл из разный потоков
        /// </summary>
        public static void MultithreadWritingToFiles()
        {
            new Thread(() => WriteToFileThreadSafe(GetUsersInfo())).Start();
            new Thread(() => WriteToFileThreadSafe(GetUsersWithSuchTopic("Work"))).Start();
            new Thread(() => WriteToFileThreadSafe(GetUsersWithoutSuchTopic("Study"))).Start();
            new Thread(() => WriteToFileThreadSafe(FindUserWithTheShortesLetter().ToString())).Start();
        }

        /// <summary>
        /// Метод для безопасной записи в файл из разных потоков
        /// </summary>
        /// <param name="str"> Строка для записи в файл </param>
        private static void WriteToFileThreadSafe(string text)
        {
            _readWriteLock.EnterWriteLock();
            try
            {
                using (StreamWriter sw = File.AppendText("MultiThreadWritingResult.txt"))
                {
                    sw.WriteLine(text + new string('*',40));
                }
            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Доп. задание для практики
        /// </summary>
        /// <returns> Пользователя, который получил больше всего писем </returns>
        public static User GetUserWithTheMostEmailsReceived()
        {
            return Dictionary.Keys.AsParallel().OrderByDescending(l => l.LettersReceived).First();
        }

        /// <summary>
        /// Доп. задание для практики
        /// </summary>
        /// <returns> Сортировка пользователей от старших к младшим </returns>
        public static List<User> SortUsersByBirthday() => Dictionary.Keys.AsParallel().OrderBy(x => x.Birthdate).ToList();
    }
}
