using System;
using SQLite;
using System.Collections.Generic;

namespace AIChatBotAppRAG.Models
{
    public class Conversation
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        
        [Ignore]
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
