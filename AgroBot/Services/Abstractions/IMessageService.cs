﻿using AgroBot.Models;

namespace AgroBot.Services.Abstractions
{
    public interface IMessageService : IService<Message>
    {
        public Task<IEnumerable<Message>> GetAllByChatIdAsync(string chatId);
        public Task<Message> GetLastByChatIdAsync(string chatId);
    }
}
