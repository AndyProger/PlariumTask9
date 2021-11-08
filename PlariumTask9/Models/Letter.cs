using System;
using System.Drawing;
using UserSpace;
using Newtonsoft.Json;

namespace LetterSpace
{
    // пример использования generics
    interface IAttach<T>
    {
        /// <summary>
        /// Прикрепить что-то (в случае с письмом, будет реализован метод прикрепления изображения к письму)
        /// </summary>
        public void Attach(T obj);
    }

    class Letter : IAttach<Image>
    {
        [JsonProperty]
        public User Sender { get; set; }
        [JsonProperty]
        public User Recipient { get; set; }
        [JsonProperty]
        public DateTime SendingDate { get; set; }
        [JsonProperty]
        public string Topic { get; private set; } = string.Empty;
        [JsonProperty]
        public string Text { get; private set; } = string.Empty;
        [JsonProperty]
        public Image AttachedImage { get; private set; }

        public Letter(string topic, string text)
        {
            Topic = topic;
            Text = text;
        }

        public Letter(Letter other)
        {
            Sender = other.Sender;
            Recipient = other.Recipient;
            Topic = other.Topic;
            Text = other.Text;
        }

        /// <summary>
        /// К-ор без параметров для json
        /// </summary>
        public Letter()
        {
            Topic = "Without Topic";
            Text = string.Empty;
        }
        
        public override string ToString()
        {
            return $"Sender:\n{Sender}\nRecipient:\n{Recipient}\nTopic: {Topic}\nText: {Text}\nDate: {SendingDate}";
        }

        /// <summary>
        /// Прикрепить изображение к письму
        /// </summary>
        /// <param name="img"> Изображение </param>
        public void Attach(Image img)
        {
            if(img is not null)
            {
                AttachedImage = img;
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
    }
}
