using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AIChatBotAppRAG.Models;

namespace AIChatBotAppRAG.Data
{
    public class DatabaseHelper
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseHelper(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Conversation>().Wait();
            _database.CreateTableAsync<Message>().Wait();
        }

        // Conversation operations
        public Task<List<Conversation>> GetConversationsAsync()
        {
            return _database.Table<Conversation>()
                            .OrderByDescending(c => c.Timestamp)
                            .ToListAsync();
        }

        public Task<Conversation> GetConversationAsync(int id)
        {
            return _database.Table<Conversation>()
                            .Where(i => i.Id == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveConversationAsync(Conversation conversation)
        {
            if (conversation.Id != 0)
            {
                return _database.UpdateAsync(conversation);
            }
            else
            {
                return _database.InsertAsync(conversation);
            }
        }

        public Task<int> DeleteConversationAsync(Conversation conversation)
        {
            return _database.DeleteAsync(conversation);
        }

        // Message operations
        public Task<List<Message>> GetMessagesAsync(int conversationId)
        {
            return _database.Table<Message>()
                            .Where(m => m.ConversationId == conversationId)
                            .OrderBy(m => m.Timestamp)
                            .ToListAsync();
        }

        public Task<int> SaveMessageAsync(Message message)
        {
            if (message.Id != 0)
            {
                return _database.UpdateAsync(message);
            }
            else
            {
                return _database.InsertAsync(message);
            }
        }

        // Limit conversations to max 10
        public async Task LimitConversationsAsync(int maxCount = 10)
        {
            var conversations = await GetConversationsAsync();
            if (conversations.Count > maxCount)
            {
                var toDelete = conversations.GetRange(maxCount, conversations.Count - maxCount);
                foreach (var conv in toDelete)
                {
                    // Delete messages first
                    var messages = await GetMessagesAsync(conv.Id);
                    foreach (var msg in messages)
                    {
                        await _database.DeleteAsync(msg);
                    }
                    await DeleteConversationAsync(conv);
                }
            }
        }
    }
}