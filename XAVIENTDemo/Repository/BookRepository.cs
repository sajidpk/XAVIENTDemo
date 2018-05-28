using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace XAVIENTDemo.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private static readonly string tableName = "BookTable";

        public BookRepository(IAmazonDynamoDB dynamoDBClient)
        {
            _dynamoDbClient = dynamoDBClient;
        }


        public void CreateBookTable()
        {

            try

            {

                Console.WriteLine("Creating Table");

                var request = new CreateTableRequest
                {
                     AttributeDefinitions = new List<AttributeDefinition>
                        {
                            new AttributeDefinition
                            {
                                AttributeName = "Id",
                                AttributeType = "N"
                            },
                            new AttributeDefinition
                            {
                                AttributeName = "ReplyDateTime",
                                AttributeType = "N"
                            }
                        },
                       KeySchema = new List<KeySchemaElement>
                        {
                            new KeySchemaElement
                            {
                                AttributeName = "Id",
                                KeyType = "HASH" // Partition Key
                            },
                            new KeySchemaElement
                            {
                                AttributeName = "ReplyDateTime",
                                KeyType = "Range" // Sort Key
                            }
                  },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 2,
                        WriteCapacityUnits = 2
                    },
                    TableName = tableName
                };

                var response = _dynamoDbClient.CreateTableAsync(request);

                WaitUntilTableReady(tableName);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }



        }

        private void WaitUntilTableReady(string tableName)
        {
            string status = null;

            do
            {
                Thread.Sleep(5000);
                try
                {
                    var res = _dynamoDbClient.DescribeTableAsync(new DescribeTableRequest
                    {
                        TableName = tableName
                    });

                    status = res.Result.Table.TableStatus;
                }
                catch (ResourceNotFoundException)
                {

                }

            } while (status != "ACTIVE");
            {
                Console.WriteLine("Table Created Successfully");
            }
        }
    }

}
