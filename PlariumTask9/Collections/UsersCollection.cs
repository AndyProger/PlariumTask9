using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using LetterSpace;
using UserSpace;
using System.IO;
using Newtonsoft.Json;

namespace CollectionOfUsers
{
    /// <summary>
    /// Делегат, для методов сортировки списка пользователей
    /// </summary>
    /// <param name="colection"> Список пользователей </param>
    /// <returns> Отсортированный список пользователей </returns>
    delegate List<User> SortingAlgorithmDelegate(List<User> colection);

    partial class UsersCollection 
    {
        /// <summary>
        /// ключ - пользователь
        /// значение - список писем
        /// </summary>
        private static ConcurrentDictionary<User, List<Letter>> _dictionary;
        [JsonProperty]
        public static Dictionary<User, List<Letter>> Dictionary 
        {
            get => new Dictionary<User, List<Letter>>(_dictionary);
        }

        /// <summary>
        /// к-ор без параметров для json
        /// </summary>
        public UsersCollection() { }

        /// <summary>
        /// Статический к-ор для инициализации словаря 
        /// </summary>
        static UsersCollection() => _dictionary = new ConcurrentDictionary<User, List<Letter>>();

        /// <summary>
        /// Добавить пользователя в коллекцию
        /// </summary>
        /// <param name="user"> Пользователь </param>
        public static void AddUser(User user) => _dictionary.TryAdd(user, user.Letters);

        public static void AddUsers(List<User> users)
        {
            foreach(var user in users)
            {
                AddUser(user);
            }
        }

        /// <summary>
        /// индексатор
        /// </summary>
        /// <param name="user"> Пользователь </param>
        /// <param name="letterIndex"> Индекс письма в списке указанного пользователя </param>
        /// <returns> Копию письма пользователя по указанному индексу </returns>
        public Letter this[User user, int letterIndex]
        {
            get => new Letter(_dictionary [user][letterIndex]);
        }

        /// <summary>
        /// индексатор
        /// </summary>
        /// <param name="user"> Пользователь </param>
        /// <returns> копию списка писем указанного пользователя </returns>
        public List<Letter> this[User user]
        {
            get => new List<Letter>(_dictionary[user]);
        }

        public IEnumerator GetEnumerator() => _dictionary.Keys.GetEnumerator();

        /// <summary>
        /// Сортировка пользователей по алгоритму заданным поздним связыванием
        /// </summary>
        /// <param name="sorting"> Метод соответствующий сигнатуре делегата </param>
        /// <returns> Копию отсортированного списка пользователей </returns>
        public static List<User> SpecificUsersSort(SortingAlgorithmDelegate sorting)
        {
            return sorting(new List<User>(_dictionary.Keys));
        }

        public static List<User> GetListOfUser() => new List<User>(Dictionary.Keys);

        /// <summary>
        /// Запись результат запроса к коллекции в файл
        /// </summary>
        /// <param name="result"> Результат </param>
        /// <param name="comment"> Комментарий к результату (необязательный) </param>
        public static void WriteToFileResult(string result, string comment = null)
        {
            using (StreamWriter sw = new StreamWriter("Results.txt", true))
            {
                sw.WriteLine(new string('*',20) + comment + new string('*', 20) + "\n");
                sw.WriteLine(result);
            }
        }

        /// <summary>
        /// Считать результат из файла в строку
        /// </summary>
        /// <returns> Строка с результатом выполненных запросов </returns>
        public static string[] GetResultsFromFile()
        {
            if (File.Exists("Results.txt"))
            {
                string[] text = File.ReadAllLines("Results.txt");

                return text;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Сохранить коллекцию в JSON
        /// </summary>
        public static void SaveUsersToJSON()
        {
            using (StreamWriter file = File.CreateText("SavedUsers.json"))
            {
                JsonSerializer serializer = new JsonSerializer()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.All
                };

                serializer.Serialize(file, Dictionary.Keys);
            }
        }
    }
}
