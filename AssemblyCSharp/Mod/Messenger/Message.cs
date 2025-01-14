using System;

namespace Mod.Messenger
{
    internal class Message
    {
        public bool isRecieve { get; set; }

        public string message { get; set; }

        public DateTime date { get; set; }

        public Message() { }

        public Message(bool isRecieve, string message)
        {
            this.isRecieve = isRecieve;
            this.message = message;
            this.date = DateTime.Now;
        }

        public Message(bool isRecieve, string message, int y, int m, int d, int h, int minute, int s)
        {
            this.isRecieve = isRecieve;
            this.message = message;
            this.date = new DateTime(y, m, d, h, minute, s);
        }

        public string getTime()
        {
            return date.ToString("HH:mm");
        }

        public string getDate()
        {
            return date.ToString("dd/MM/yyyy");
        }
    }
}
