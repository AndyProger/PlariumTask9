using System;
using System.Collections.Generic;
using UserSpace;

namespace VariantB
{
    class SortingAlgorithms
    {
        /// <summary>
        /// Сортировка пользователей по фамилиям в алфавитном порядке
        /// </summary>
        public static List<User> DefSorting(List<User> collection)
        {
            collection.Sort();

            return collection;
        }

        /// <summary>
        /// Сортировка пользователей по фамилиям в алфавитном порядке (Гномья сортировка)
        /// </summary>
        public static List<User> GnomeSort(List<User> collection)
        {
            var sortedList = new List<User>(collection);

            var index = 1;
            var nextIndex = index + 1;

            while (index < collection.Count)
            {
                if (sortedList[index - 1].CompareTo(sortedList[index]) == -1)
                {
                    index = nextIndex;
                    nextIndex++;
                }
                else
                {
                    (sortedList[index - 1], sortedList[index]) = (sortedList[index], sortedList[index - 1]);
                    index--;
                    if (index == 0)
                    {
                        index = nextIndex;
                        nextIndex++;
                    }
                }
            }

            return sortedList;
        }

        /// <summary>
        /// Болотная сортировка 
        /// !WARNING! Не рекомендуется использовать для большого числа сортируемых элементов
        /// </summary>
        public static List<User> BogoSort(List<User> collection)
        {
            var sortedArray = new List<User>(collection);

            while (!IsSorted(sortedArray))
            {
                collection = RandomPermutation(sortedArray);
            }

            return sortedArray;
        }

        /// <summary>
        /// Проверка на то, отссортирован ли список
        /// </summary>
        private static bool IsSorted(List<User> collection)
        {
            for (var i = 0; i < collection.Count - 1; i++)
            {
                if (collection[i].CompareTo(collection[i + 1]) == 1)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Случайная перестановка элементов списка
        /// </summary>
        private static List<User> RandomPermutation(List<User> list)
        {
            Random random = new Random();
            var n = list.Count;

            while (n > 1)
            {
                n--;
                var i = random.Next(n + 1);
                var temp = list[i];
                list[i] = list[n];
                list[n] = temp;
            }

            return list;
        }

        /// <summary>
        /// Сортировка Шелла
        /// </summary>
        public static List<User> ShellSort(List<User> list)
        {
            var sortedList = new List<User>(list);
            var d = sortedList.Count / 2;

            while (d >= 1)
            {
                for (var i = d; i < sortedList.Count; i++)
                {
                    var j = i;
                    while ((j >= d) && (sortedList[j - d].CompareTo(sortedList[j]) == 1))
                    {
                        (sortedList[j], sortedList[j - d]) = (sortedList[j - d], sortedList[j]);
                        j = j - d;
                    }
                }

                d = d / 2;
            }

            return sortedList;
        }
    }
}
