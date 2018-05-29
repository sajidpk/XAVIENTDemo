using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XAVIENTDemo.Model
{
   
        public class Player
        {
            [DynamoDBHashKey]
            public int Id { get; set; }

            [DynamoDBProperty]
            public string Name { get; set; }

            [DynamoDBProperty]
            public int Points { get; set; }

            [DynamoDBProperty]
            public int Gold { get; set; }

            [DynamoDBProperty]
            public int Level { get; set; }

            [DynamoDBProperty]
            public List<string> Items { get; set; }


          public Dictionary<string, AttributeValue> ToDynamoAttributes()

        {

            // Define item attributes
            var attributes = new Dictionary<string, AttributeValue>
            {
                ["Id"] = new AttributeValue { N = Id.ToString() },
                ["Name"] = new AttributeValue { S = Name },
                ["Points"] = new AttributeValue { N = Points.ToString() },
                ["Gold"] = new AttributeValue { N = Gold.ToString() },
                ["Level"] = new AttributeValue { N = Level.ToString() },
                ["Items"] = new AttributeValue
                {
                    SS = Items
                }
            };


            return attributes;
        }

        }


   
}
