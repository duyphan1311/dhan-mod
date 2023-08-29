using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod.Messenger
{
    internal class TestMessage
    {
        private static List<Message> messages = new List<Message>()
        {
            new Message(true, "Hello", 2023, 07, 31, 10, 16, 3),
            new Message(false, "Hello", 2023, 07, 31, 10, 17, 3),
            new Message(false, "There are two ways to initialize the DateTime variable: DateTime DT = new DateTime();// this will initialze variable with a date(01/01/0001) and time(00:00:00).", 2023, 08, 01, 15, 29, 3),
            new Message(true, "I don't know that", 2023, 08, 01, 15, 42, 3),
            new Message(true, "Can you say again?", 2023, 08, 01, 16, 1, 3),
            //new Message(false, "Sure", 2023, 08, 02, 20, 16, 3),
            //new Message(false, "Địt con mẹ mày nói thế mà không hiểu à", 2023, 08, 02, 20, 16, 40),
            //new Message(true, "Mày chủi con cặc à", 2023, 08, 07, 08, 16, 3),
            //new Message(true, "Bố lại địt cả lò nhà mày", 2023, 08, 08, 09, 56, 3),
            //new Message(false, "CMM", 2023, 08, 08, 10, 2, 3),
            //new Message(true, "Thứ súc vật", 2023, 8, 8, 10, 5, 3),
            //new Message(true, "Đéo phải người", 2023, 8, 8, 10, 6, 3),
            //new Message(false, "À thế à", 2023, 8, 8, 10, 16, 3),
            new Message(true, "À thế làm sao mà à", 2023, 8, 8, 20, 16, 3),
            new Message(true, "Hello", 2023, 07, 31, 10, 16, 3),
            new Message(false, "Hello", 2023, 07, 31, 10, 17, 3),
            new Message(false, "There are two ways to initialize the DateTime variable: DateTime DT = new DateTime();// this will initialze variable with a date(01/01/0001) and time(00:00:00).", 2023, 08, 01, 15, 29, 3),
            new Message(true, "I don't know that", 2023, 08, 01, 15, 42, 3),
            new Message(true, "Can you say again?", 2023, 08, 01, 16, 1, 3),
            new Message(false, "Sure", 2023, 08, 02, 20, 16, 3),
            new Message(false, "Địt con mẹ mày nói thế mà không hiểu à", 2023, 08, 02, 20, 16, 40),
            new Message(true, "Mày chủi con cặc à", 2023, 08, 07, 08, 16, 3),
            new Message(true, "Bố lại địt cả lò nhà mày", 2023, 08, 08, 09, 56, 3),
            new Message(false, "CMM", 2023, 08, 08, 10, 2, 3),
            new Message(true, "Thứ súc vật", 2023, 8, 8, 10, 5, 3),
            new Message(true, "Đéo phải người", 2023, 8, 8, 10, 6, 3),
            new Message(false, "À thế à", 2023, 8, 8, 10, 16, 3),
            new Message(true, "À thế làm sao mà à", 2023, 8, 8, 20, 16, 3),
            new Message(false, "There are two ways to initialize the DateTime variable: DateTime DT = new DateTime();// this will initialze variable with a date(01/01/0001) and time(00:00:00).", 2023, 08, 01, 15, 29, 3),
            //new Message(true, "I don't know that", 2023, 08, 01, 15, 42, 3),
        };
        private static List<Message> messages4 = new List<Message>()
        {
            new Message(true, "Hello", 2023, 07, 1, 10, 16, 3),
            new Message(true, "Hello", 2023, 07, 2, 10, 16, 3),
        };

        public static List<Conversation> ConversationList = new List<Conversation>()
        {
            new Conversation()
            {
                id = 1,
                name = "Test 1",
                messages = messages
            },
            new Conversation()
            {
                id = 2,
                name = "Test 2",
                messages = messages
            },
            new Conversation()
            {
                id = 3,
                name = "Test 3",
                messages = messages
            },
            new Conversation()
            {
                id = 4,
                name = "Test 4",
                messages = messages4
            }
        };
    }
}
