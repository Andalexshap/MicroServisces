using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApiLibrary;
using WebApiLibrary.Abstraction;
using WebApiLibrary.DataStore.Entities;
using WebApiLibrary.DataStore.Models;
using WebApiLibrary.DataStore.Response;

namespace MessageApi.Services
{
    public class MessageService : IMessageService
    {
        private readonly Func<AppDbContext> _context;
        private readonly IMapper _mapper;

        public MessageService(Func<AppDbContext> context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public MessageResponse GetNewMessages(string recipientEmail)
        {
            var response = new MessageResponse();
            using (var context = _context())
            {
                var messages = context.Messages
                    .Include(x => x.Recipient)
                    .Include(x => x.Sender)
                    .Where(x => x.Recipient.Email == recipientEmail && !x.IsRead).ToList();
                
                foreach (var message in messages)
                {
                    message.IsRead = true;
                }

                context.UpdateRange(messages);
                context.SaveChanges();

                response.Messages.AddRange(messages.Select(_mapper.Map<MessageModel>));
                response.IsSuccess = true;
            }
            return response;
        }

        public MessageResponse SendMessage(MessageModel model)
        {
            var response = new MessageResponse();
            using (var context = _context())
            {
                var messge = _mapper.Map<MessageEntity>(model);
                context.Messages.Add(messge);
                context.SaveChanges();

                response.Messages.Add(model);
                response.IsSuccess = true;
            }
            return response;
        }
    }
}
