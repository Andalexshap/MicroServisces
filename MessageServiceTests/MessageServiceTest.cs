using AutoMapper;
using MessageApi.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApiLibrary;
using WebApiLibrary.DataStore.Entities;
using WebApiLibrary.DataStore.Models;

namespace MessageServiceTests
{
    public class MessageServiceTest
    {
        private static AppDbContext AppDbContextCreator()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: "Final")
               .Options;
            return new AppDbContext(options.ToString());
        }
        [Test]
        public void GetNewMessages_ReturnsMessagesAndMarksAsRead()
        {

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Final")
                .Options;

            using (var context = new AppDbContext(options.ToString()!))
            {
                var recipientEmail = "test@example.com";
                var userEntity = new UserEntity { Email = recipientEmail };

                var messageEntities = new List<MessageEntity>
            {
                new MessageEntity { Recipient = userEntity, IsRead = false, Text = "Hello" },
                new MessageEntity { Recipient = userEntity, IsRead = false, Text = "Hi" }
            };

                context.Messages.AddRange(messageEntities);
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options.ToString()))
            {
                var mapperMock = new Mock<Mapper>();
                var messageService = new MessageService(AppDbContextCreator, mapperMock.Object);

                var result = messageService.GetNewMessages("test@example.com");


                Assert.IsTrue(result.IsSuccess);
                Assert.IsNotNull(result.Messages);
                Assert.AreEqual(2, result.Messages.Count);


                foreach (var messageEntity in context.Messages)
                {
                    Assert.IsTrue(messageEntity.IsRead);
                }
            }
        }

        [Test]
        public void SendMessage_ShouldReturnSuccessResponse()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            var context = new AppDbContext(options.ToString());
            var mapperMock = new Mock<IMapper>();
            var messageService = new MessageService(() => context, mapperMock.Object);

            var recipientEmail = "test@example.com";
            var userEntity = new UserEntity { Email = recipientEmail };

            var messageModel = new MessageModel { RecipientEmail = userEntity.Email, IsRead = false, Text = "Hello" };

            var result = messageService.SendMessage(messageModel);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(1, result.Messages.Count);

            Assert.AreEqual(1, context.Messages.Count());


        }
    }
}