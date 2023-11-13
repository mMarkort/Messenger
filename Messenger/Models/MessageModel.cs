using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Models
{
    public class MessageModel
    {
        public string Message { get; set; }
        public int ChatID { get; set; }
        public DateTime dateTime { get; set; }
        public string MessageAutor { get; set; }
    }
}
