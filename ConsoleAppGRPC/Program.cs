using DemoAspNetCore3;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Threading.Tasks;
using static DemoAspNetCore3.MyOwnService;

namespace ConsoleAppGRPC
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new MyOwnServiceClient(channel);

            var headers = new Metadata();
            //headers.Add("Authorization", $"Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6ImFQY3R3X29kdlJPb0VOZzNWb09sSWgydGlFcyIsImtpZCI6ImFQY3R3X29kdlJPb0VOZzNWb09sSWgydGlFcyJ9.eyJhdWQiOiIyNTdiNmMzNi0xMTY4LTRhYWMtYmU5My02ZjJjZDgxY2VjNDMiLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC8xMzY1NDRkOS0wMzhlLTQ2NDYtYWZmZi0xMGFjY2IzNzA2NzkvIiwiaWF0IjoxNTY5Nzg0Nzc0LCJuYmYiOjE1Njk3ODQ3NzQsImV4cCI6MTU2OTc4NTY3NCwiYWlvIjoiQVZRQXEvOE1BQUFBZGhMOEpBZ3dmT3pkQ09jcUZ3MHYxeWxmZ2FMR3dwbi9Oc0FqeklqdnU0NWFZT0ZiZkZlczYydERDS2dBdHVkSlkvWFZLZjN3MjlxTlpCQ2hyb0YrZkRwZFhJR0s2V3NkK3lWVU10UzdPOHc9IiwiYW1yIjpbInB3ZCJdLCJlbWFpbCI6ImFudGhvbnkuZ2lyZXR0aUBnbWFpbC5jb20iLCJmYW1pbHlfbmFtZSI6IkdJUkVUVEkiLCJnaXZlbl9uYW1lIjoiQW50aG9ueSIsImdyb3VwcyI6WyIyYzM3MmQ5OC0xM2I2LTQwY2QtYjViMi1mOTFmZTJlYzUxNjIiLCJmYzE5YTg2Mi02NDUyLTRlOTktOTlhMi04MjBhZmEzOWNiZWUiLCI4MTE1ZTNiZS1hYzdhLTQ4ODYtYTFlNi01YjZhYWY4MTBhOGYiLCI2Yzc4Y2Q2MC0xNmViLTQ2OTYtYWUyOS04NGZlNzEzMzA1ZDQiXSwiaWRwIjoibGl2ZS5jb20iLCJpcGFkZHIiOiI3MC44Mi4xNzYuMjE4IiwibmFtZSI6IkFudGhvbnkgR0lSRVRUSSIsIm5vbmNlIjoiMjQwN2YyZTAtZWI0NC00ODFmLWJiZTctZGY3NmQxM2QzNTYyIiwib2lkIjoiZjkxNzViYzgtYjdlYy00ZDlmLTliNTMtMjBmNjgzNjZmMmM4Iiwicm9sZXMiOlsiU3VydmV5Q3JlYXRvciJdLCJzdWIiOiI0MzdWZWpacHMxU3FScVpFdUd5Zi1ISHFCRlQyZ051NENLenc5WWUwYkdzIiwidGlkIjoiMTM2NTQ0ZDktMDM4ZS00NjQ2LWFmZmYtMTBhY2NiMzcwNjc5IiwidW5pcXVlX25hbWUiOiJsaXZlLmNvbSNhbnRob255LmdpcmV0dGlAZ21haWwuY29tIiwidXRpIjoidHBneTlNRHZPRWk2eldTNkhKVTVBQSIsInZlciI6IjEuMCJ9.WkwlG9kf1Z2JMyKBXsteNcwQaDN-YyeOuJYNKmz5tMBeqlf7NUu2ri1YGYAVmF5Kh_gyPwg78lBoHvinVDUGgaGr6T2Dq9ZXXON3vbpq7-YJGGuTFoM5DYkgoK1iZTqGhc3JRuX-ybKsgjQy16Q6AFC-1qijwzkDcIOKVIc8UkoTnllocF4SHgNzCFwI9wqKPTwYz5NBPk6E_Q_3-p_kyMkQNwD3t-AmYTqRnoU1KkKSmCsKgXtG1lKxS0zmB-oIOFhkoqt3albUjrpEAZZvPSf1M7aDfU9LksHpj1_k7L7XwuNVvQZ9SPCm0zdQyF7xheCOZshk_UT0qHYCVllRIg");

            try
            {
                var reply = await client.WhoIsAsync(new EmptyRequest(), headers);
                var reply2 = await client.IntroduceYourselfAsync(new IntroduceYourselfRequest { Name = reply.Message }, headers);

                Console.WriteLine(reply2);
            }
            catch (RpcException e)
            {

            }
        }
    }
}