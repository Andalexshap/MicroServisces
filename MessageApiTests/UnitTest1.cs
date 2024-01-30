using MessageApi.Services;
using MessageApi.Controller;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using MessageApi.Mapper;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApiLibrary;
using System.Security.Cryptography;
using System.Text;
using WebApiLibrary.DataStore.Entities;
using WebApiLibrary.DataStore.Models;

namespace MessageApiTests
{
    [TestFixture]
    public class YourClassTests
    {
        [Test]
        public void CheckPasswordTest()
        {
            string pass = "123";
            byte[] pw = Encoding.UTF8.GetBytes(pass);
            byte[] salt = Encoding.UTF8.GetBytes("asd");
            UserEntity userEntity = new UserEntity()
            {
                Id = new Guid(),
                Email = "test@mail.ru",
                Password = pw,
                Salt = salt,
                Name = "Test",
                Surname = "Test"
            };

            var expected = pw.Concat(salt).ToArray();
            SHA512 shaM = new SHA512Managed();
            var expPass = shaM.ComputeHash(expected);

            var data = userEntity.Password.Concat(salt).ToArray();
            var password = shaM.ComputeHash(data);
            CollectionAssert.AreEqual(expPass, password);
        }

        [Test]
        public void AuthentificateUserNullTest()
        {
            UserEntity user = null;

            Assert.IsNull(user);
        }

        [Test]
        public void AuthentificateCorrectPasswordTest()
        {
            string pass = "123";
            byte[] pw = Encoding.UTF8.GetBytes(pass);
            byte[] salt = Encoding.UTF8.GetBytes("asd");
            UserEntity userEntity = new UserEntity()
            {
                Id = new Guid(),
                Email = "test@mail.ru",
                Password = pw,
                Salt = salt,
                Name = "Test",
                Surname = "Test"
            };

            var expected = pw.Concat(salt).ToArray();
            SHA512 shaM = new SHA512Managed();
            var expPass = shaM.ComputeHash(expected);

            var data = userEntity.Password.Concat(salt).ToArray();
            var password = shaM.ComputeHash(data);
            CollectionAssert.AreEqual(expPass, password);
        }

        [Test]
        public void AuthentificateIncorrectPasswordTest()
        {
            string pass = "123";
            byte[] pw = Encoding.UTF8.GetBytes(pass);
            byte[] salt = Encoding.UTF8.GetBytes("asd");
            UserEntity userEntity = new UserEntity()
            {
                Id = new Guid(),
                Email = "test@mail.ru",
                Password = pw,
                Salt = salt,
                Name = "Test",
                Surname = "Test"
            };

            SHA512 shaM = new SHA512Managed();
            var data = userEntity.Password.Concat(salt).ToArray();
            var password = shaM.ComputeHash(data);
            CollectionAssert.AreNotEqual(Encoding.UTF8.GetBytes("232"), password);
        }
    }
}