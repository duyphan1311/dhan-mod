using LitJSON;
using System.Collections.Generic;

namespace Mod.Messenger
{
    internal class Conversation
    {
        public int id { get; set; }

        public string name { get; set; }

        public List<Message> messages { get; set; }

        [JsonSkip]
        public bool isNewMessage;

        public Conversation() { }

        public Conversation(Char @char)
        {
            id = @char.charID;
            name = @char.cName;
            messages = new List<Message>();
        }

        public Conversation(Char @char, List<Message> listMessage)
        {
            id = @char.charID;
            name = @char.cName;
            messages = listMessage;
        }

        public Conversation(int id, string name)
        {
            this.id = id;
            this.name = name;
            messages = new List<Message>();
        }

        public Conversation(int id, string name, List<Message> listMessage)
        {
            this.id = id;
            this.name = name;
            this.messages = listMessage;
        }
    }
}
