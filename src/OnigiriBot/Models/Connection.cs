using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnigiriBot
{
    public class Connection
    {
        public DateTime LastUpdatedTime { get; set; }
        public string IPAddress { get; set; }
    }
}
