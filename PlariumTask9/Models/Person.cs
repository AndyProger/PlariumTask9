using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace VariantB.Models
{
    abstract class Person : ICloneable
    {
        [JsonProperty]
        public string Name { get; protected set; }
        [JsonProperty]
        public string Surname { get; protected set; }
        [JsonProperty]
        public DateTime Birthdate { get; protected set; }

        /// <summary>
        /// К-ор без параметров для json
        /// </summary>
        public Person() { }

        /// <summary>
        /// К-ор создает человека с фамилией и именем
        /// </summary>
        /// <param name="name"> Имя человека </param>
        /// <param name="surname"> Фамилия человека </param>
        protected Person(string name, string surname)
        {
            name = Regex.Replace(name.Trim(), "\\s+", " ");
            surname = Regex.Replace(surname.Trim(), "\\s+", " ");

            if (IsNameValid(name) && IsNameValid(surname))
            {
                Name = name;
                Surname = surname;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Объект в текстовом представлении 
        /// </summary>
        /// <returns>Имя, Фамилия и ДР в string</returns>
        public override string ToString()
        {
            return $"Name: {Name} \nSurname: {Surname} \nBirthdate: {Birthdate} \n";
        }

        /// <summary>
        /// Классы наследники сами определяют логику валидации ФИО
        /// </summary>
        /// <param name="name"> Имя для валидации </param>
        /// <returns>
        /// true - если фио проходит валидацию, иначе false
        /// </returns>
        protected abstract bool IsNameValid(string name);

        /// <summary>
        /// Альтернатива копирующему конструктору
        /// </summary>
        /// <returns>Неглубокую копию объекта</returns>
        public virtual object Clone() => MemberwiseClone();
    }
}
