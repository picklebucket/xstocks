using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Data.Entities
{
    public class BaseEntity
    {
        public ObjectId Id { get; private set; }
    }
}
