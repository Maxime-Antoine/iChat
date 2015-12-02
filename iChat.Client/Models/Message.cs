using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iChat.Client.Models
{
    public class Message
    {
        public string Author { get; set; }

        public string Content { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
