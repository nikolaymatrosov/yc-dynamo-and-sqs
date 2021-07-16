using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.SQS;
using Amazon.SQS.Model;




public class Handler
{


    public static void Main(string[] args)
    {

    }


    public async Task<Response> FunctionHandler(Request request)
    {
        string status = "";
        try
        {
            String AWS_ACCESS_KEY = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY");
            String AWS_SECRET_KEY = Environment.GetEnvironmentVariable("AWS_SECRET_KEY");
            String ENDPOINT = Environment.GetEnvironmentVariable("ENDPOINT");

            AmazonSQSConfig amazonSQSConfig = new AmazonSQSConfig
            {
                ServiceURL = "https://message-queue.api.cloud.yandex.net",
                AuthenticationRegion = "ru-central1"

            };

            AmazonSQSClient amazonSQS = new AmazonSQSClient(
                    AWS_ACCESS_KEY,
                    AWS_SECRET_KEY,
                    amazonSQSConfig
                );

            CreateQueueResponse queueResp  = await amazonSQS.CreateQueueAsync(new CreateQueueRequest {
                QueueName="csharp-example"
            });

            Console.WriteLine("QueueUrl: " + queueResp.QueueUrl);

            AmazonDynamoDBConfig config = new AmazonDynamoDBConfig
            {
                ServiceURL = ENDPOINT
            };

            AmazonDynamoDBClient client = new AmazonDynamoDBClient(
                    AWS_ACCESS_KEY,
                    AWS_SECRET_KEY,
                    config
                    );




            //Console.WriteLine($"bucket: {BUCKET}, key: {PREFIX}/{ originalKey}");

            var response = await client.DescribeTableAsync(new DescribeTableRequest {
                TableName = "doc-test"
            });
            Console.WriteLine("Table = {0}, Status = {1}",
              response.Table.TableName,
              response.Table.TableStatus);
            status = response.Table.TableStatus;


        }
        catch (ResourceNotFoundException)
        {
            Console.WriteLine("Failed to find table");
        }
        return new Response(
            200,
            status,
            new Dictionary<string,string>{},
            false
        );
    }

}