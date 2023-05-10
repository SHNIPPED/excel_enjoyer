using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace workingBD
{
    public class workingDB
    {

        private static string Cleaner(string str)
        {
            string cl = new string(str.Where(x => char.IsDigit(x) || char.IsLetter(x)).ToArray());
            string[] clear = cl.Split(new string[] { "idObjectId" }, StringSplitOptions.None);
            return clear[1].ToString();
        }

        private static string cleanValues(string str)
        {
           Console.WriteLine(str);
            string clean = str.Replace("{", String.Empty);
            clean = clean.Replace("\"", String.Empty);
            clean = clean.Replace("}", String.Empty);
            clean = clean.Substring(2);
            return clean;
        }
        public static void loader(List<Values> values)
        {
            try
            {
                var client = new MongoClient("mongodb+srv://SHNIPPED:Asdfg123@cluster0.axk9z4z.mongodb.net/test");

                var mongo = client.GetDatabase("test");

                var collection = mongo.GetCollection<BsonDocument>("tables");

                var cursor = collection.Find(new BsonDocument()).ToList();

                for (int i = 0; i < cursor.Count; i++)
                {
                    string[] parser = cursor[i].ToString().Split(new string[] { "values" }, StringSplitOptions.None);
                    string id = Cleaner(parser[0].ToString());
                    string _values = cleanValues(parser[1].ToString());
                    values.Add(new Values(
                        id,
                        _values
                        ));
                }
            }
            catch { }
        }

        public static async void loaderTest(List<test> tests)
        {
          
                var client = new MongoClient("mongodb+srv://SHNIPPED:Asdfg123@cluster0.axk9z4z.mongodb.net/test");

                var mongo = client.GetDatabase("test");

                var collection = mongo.GetCollection<test>("tables");

                List<test>tests1 = await collection.Find(new BsonDocument()).ToListAsync();
                
                foreach(var items in tests1)
                {
                tests.Add(new test(
                    items.id,
                    items.values
                    ));
                }
     
        }


        public static async void Deleter(string id)
        {
            try
            {
                var client = new MongoClient("mongodb+srv://SHNIPPED:Asdfg123@cluster0.axk9z4z.mongodb.net/test");

                var mongo = client.GetDatabase("test");

                var collection = mongo.GetCollection<BsonDocument>("tables");

                var result = await collection.DeleteOneAsync(new BsonDocument("_id", ObjectId.Parse(id)));
            }
            catch { }
        }

        public async void Editer(string _id)
        {
            try
            {
                var client = new MongoClient("mongodb+srv://SHNIPPED:Asdfg123@cluster0.axk9z4z.mongodb.net/test");

                var mongo = client.GetDatabase("test");

                var collection = mongo.GetCollection<BsonDocument>("tables");

                 var cursor = collection.Find(new BsonDocument("_id", ObjectId.Parse(_id))).ToList();
            }
            catch
            {

            }
           
        }

        public static void Edit(string id,string result)
        {

            try
            {
                string _id = id;
                string _result = result;
                var client = new MongoClient("mongodb+srv://SHNIPPED:Asdfg123@cluster0.axk9z4z.mongodb.net/test");

                var mongo = client.GetDatabase("test");

                var collection = mongo.GetCollection<BsonDocument>("tables");

                var filter = new BsonDocument { { "_id", ObjectId.Parse(_id) } };

                BsonDocument newDocument = BsonDocument.Parse(@"{values : { " + _result + "}}");

                var results = collection.ReplaceOneAsync(filter, newDocument, new ReplaceOptions { IsUpsert = true });
            }
            catch
            {

            }

        }

        public  async static void Improt(string result)
        {
            try
            {
                BsonDocument tom = BsonDocument.Parse(@"{values : { " + result + "}}");

                var client = new MongoClient("mongodb+srv://SHNIPPED:Asdfg123@cluster0.axk9z4z.mongodb.net/test");

                var mongo = client.GetDatabase("test");

                var collection = mongo.GetCollection<BsonDocument>("tables");

                await collection.InsertOneAsync(tom);
            }
            catch
            {

            }

        }

    }
}
