using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XAVIENTDemo.Model;

namespace XAVIENTDemo.Repository
{
    public class PlayersRepository : IPlayersRepository
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private static readonly string tableName = "Player";
        private readonly DynamoDBContextConfig DynamoConfig = new DynamoDBContextConfig { ConsistentRead = true, SkipVersionCheck = true, IgnoreNullValues = true, TableNamePrefix = tableName };

        public PlayersRepository(IAmazonDynamoDB dynamoDBClient)
        {
            _dynamoDbClient = dynamoDBClient;
        }
        public async Task<Player> GetPlayerByIDAsync(Int32 playerId)
        {

            try
            {
                DynamoDBContext context = new DynamoDBContext(_dynamoDbClient);
                var result = await context.LoadAsync<Player>(playerId);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex);
                return null;
            }

        }

        public async Task<bool> SavePlayersAsync(Player player)
        {

            try
            {
                var config = new DynamoDBContextConfig { Conversion = DynamoDBEntryConversion.V2 };
                                         
                DynamoDBContext context = new DynamoDBContext(_dynamoDbClient, config);
                await context.SaveAsync<Player>(player);

                Console.WriteLine("player Added  Successfully");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("player insert failed");
                Console.WriteLine(ex);
                return false;
            }


        }

        public async Task<List<Player>> GetPlayersAsync()
        {

            var conditions = new List<ScanCondition>();

            try
            {

                DynamoDBContext context = new DynamoDBContext(_dynamoDbClient);

                // you can add scan conditions, or leave empty
                var result = await context.ScanAsync<Player>(conditions).GetRemainingAsync();


                return result.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex);
                return null;
            }
        }
               
        public async Task<bool> DeletePlayersAsync(Player player)
        {
            try
            {
                DynamoDBContext context = new DynamoDBContext(_dynamoDbClient);
                await context.DeleteAsync<Player>(player);

                return true;
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex);
                return false;
            }

        }

        //Create New Dyanmo DB Table
        public async Task<bool> CreateTableAsync(bool RecreateIfexists)
        {

            try

            {

                Console.WriteLine("Creating Table");


                var tableList = await _dynamoDbClient.ListTablesAsync();

                if (tableList.TableNames.Contains(tableName))
                {
                    if (!RecreateIfexists) return true;

                    await _dynamoDbClient.DeleteTableAsync(tableName);

                }




                var request = new CreateTableRequest
                {
                    AttributeDefinitions = new List<AttributeDefinition>
                        {
                            new AttributeDefinition
                            {
                                AttributeName = "Id",
                                AttributeType = "N"
                            }
                            //},
                            //new AttributeDefinition
                            //{
                            //    AttributeName = "Name",
                            //    AttributeType = "S"
                            //}
                        },
                    KeySchema = new List<KeySchemaElement>
                        {
                            new KeySchemaElement
                            {
                                AttributeName = "Id",
                                KeyType = "HASH" // Partition Key
                            }
                            //,
                            //new KeySchemaElement
                            //{
                            //    AttributeName = "Name",
                            //    KeyType = "Range" // Sort Key
                            //}
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

                return true;

            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message);
                return false;
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

        public Task<bool> UpdatePlayersAsync(Player player)
        {
            throw new NotImplementedException();
        }
    }

}
