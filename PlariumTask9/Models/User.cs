using System;
using VariantB;
using LetterSpace;
using VariantB.Models;
using CollectionOfUsers;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace UserSpace
{

    class User : Person, ICloneable, IComparable<User>
    {
        [JsonProperty]
        public string ID { get; private set; } = Guid.NewGuid().ToString();
        [JsonProperty]
        public int LettersSent { get; private set; }
        [JsonProperty]
        public int LettersReceived { get => _letters.Count; }

        /// <summary>
        /// Событие, которое вызывается при отправке письма
        /// </summary>
        public event SendLetterHandler LetterSent;

        /// <summary>
        /// В списке хранятся письма, которые получил пользователь
        /// </summary>
        private List<Letter> _letters = new List<Letter>();
        [JsonProperty]
        public List<Letter> Letters
        {
            get => _letters;

            private set => _letters = value;
        }

        public User(string name, string surname, DateTime birthdate, bool addToCollection = true) : base(name, surname)
        {
            if(birthdate > new DateTime(1900,1,1) && birthdate < DateTime.Now)
            {
                Birthdate = birthdate;

                if(addToCollection)
                    UsersCollection.AddUser(this);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public override object Clone() => MemberwiseClone();

        public User(User other)
        {
            other.Name = Name;
            other.Surname = Surname;
            other.Birthdate = Birthdate;
            other.Letters = new List<Letter>(_letters);
        }

        /// <summary>
        /// К-ор без параметров для json
        /// </summary>
        public User() { }

        /// <summary>
        /// Отправить письмо получателю
        /// </summary>
        /// <param name="reciper"> Пользователь-получатель </param>
        /// <param name="letter"> Письмо, которое отправляется </param>
        public void SendLetter(User reciper, Letter letter)
        {
            if (reciper == this)
                throw new ArgumentException();

            Letter copyLetter = new Letter(letter);

            copyLetter.Sender = this;
            copyLetter.Recipient = reciper;
            copyLetter.SendingDate = DateTime.Now;

            reciper._letters.Add(copyLetter);
            LettersSent++;

            // вызываем переданный метод на возникшее событие (отправка письма)
            var eventArgs = new SendLetterEventArgs();

            if(LetterSent is not null)
            {
                eventArgs.Letter = copyLetter;
                LetterSent(this, eventArgs);
            }
        }

        /// <summary>
        /// Отправить письмо всем адресатам.
        /// </summary>
        /// <param name="letter"> Письмо на отправку </param>
        public void SendToAll(Letter letter)
        {
            List<User> users = new List<User>(UsersCollection.Dictionary.Keys);
            users.Remove(this);

            foreach(User user in users)
            {
                SendLetter(user, letter);
            }
        }

        public override string ToString() => $"ID: {ID} \n" + base.ToString();

        public Letter this[int index] { get => _letters[index]; }

        protected override bool IsNameValid(string name)
        {
            return !string.IsNullOrEmpty(name) && Regex.IsMatch(name, @"^[a-zA-Z]+$");
        }

        public int CompareTo(User other) => Surname.CompareTo(other.Surname);
    }
}
