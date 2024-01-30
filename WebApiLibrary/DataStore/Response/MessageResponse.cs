using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiLibrary.DataStore.Models;

namespace WebApiLibrary.DataStore.Response
{
    public class MessageResponse
    {
        public bool IsSuccess { get; set; }
        public List<MessageModel> Messages { get; set; }
        public List<ErrorModel> Errors { get; set; }
    }
}
