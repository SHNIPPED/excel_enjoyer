using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace workingBD
{
    public  class test 
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public ObjectId id { get; set; }
        public BsonDocument values { get; set; }

        public test(ObjectId id, BsonDocument values) { 
            this.id = id;
            this.values = values;
        }
    }
}
