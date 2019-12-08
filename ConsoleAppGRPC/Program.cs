using DemoGrpc.Domain.Entities;
using DemoGrpc.Protobufs;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using static DemoGrpc.Protobufs.CountryService;

namespace ConsoleAppGRPC
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // DI
            var services = new ServiceCollection();
            
            services.AddGrpcClient<CountryServiceClient>(o =>
            {
                o.Address = new Uri("https://localhost:5001");
            });

            var provider = services.BuildServiceProvider();

            var client = provider.GetRequiredService<CountryServiceClient>();
            
            
            // Execution
            var headers = new Metadata();
            //headers.Add("Authorization", $"Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6ImFQY3R3X29kdlJPb0VOZzNWb09sSWgydGlFcyIsImtpZCI6ImFQY3R3X29kdlJPb0VOZzNWb09sSWgydGlFcyJ9.eyJhdWQiOiIyNTdiNmMzNi0xMTY4LTRhYWMtYmU5My02ZjJjZDgxY2VjNDMiLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC8xMzY1NDRkOS0wMzhlLTQ2NDYtYWZmZi0xMGFjY2IzNzA2NzkvIiwiaWF0IjoxNTY5OTcxODkyLCJuYmYiOjE1Njk5NzE4OTIsImV4cCI6MTU2OTk3Mjc5MiwiYWlvIjoiQVZRQXEvOE5BQUFBV2NwVXIwOERnUUdjUXJoWER1VGF0b3pQaURJdkRPRjF5KzAwSzQ3cWtsU3MzbzdRdnNOUW9GK3NRUkFhMHp2OTBuN1pUalB5MzlWWmhuYVhaTzRNNjA5RUk3eXh2L01yQjZ1MGNHQjBxaEE9IiwiYW1yIjpbInB3ZCJdLCJlbWFpbCI6ImFudGhvbnkuZ2lyZXR0aUBnbWFpbC5jb20iLCJmYW1pbHlfbmFtZSI6IkdJUkVUVEkiLCJnaXZlbl9uYW1lIjoiQW50aG9ueSIsImdyb3VwcyI6WyIyYzM3MmQ5OC0xM2I2LTQwY2QtYjViMi1mOTFmZTJlYzUxNjIiLCJmYzE5YTg2Mi02NDUyLTRlOTktOTlhMi04MjBhZmEzOWNiZWUiLCI4MTE1ZTNiZS1hYzdhLTQ4ODYtYTFlNi01YjZhYWY4MTBhOGYiLCI2Yzc4Y2Q2MC0xNmViLTQ2OTYtYWUyOS04NGZlNzEzMzA1ZDQiXSwiaWRwIjoibGl2ZS5jb20iLCJpcGFkZHIiOiIxNjcuMjIwLjE1Mi43NyIsIm5hbWUiOiJBbnRob255IEdJUkVUVEkiLCJub25jZSI6IjNiZTkxMDdlLTc2ZWUtNGNmMi04ZDFhLTlhNTJlYWM4MzI2OSIsIm9pZCI6ImY5MTc1YmM4LWI3ZWMtNGQ5Zi05YjUzLTIwZjY4MzY2ZjJjOCIsInJvbGVzIjpbIlN1cnZleUNyZWF0b3IiXSwic3ViIjoiNDM3VmVqWnBzMVNxUnFaRXVHeWYtSEhxQkZUMmdOdTRDS3p3OVllMGJHcyIsInRpZCI6IjEzNjU0NGQ5LTAzOGUtNDY0Ni1hZmZmLTEwYWNjYjM3MDY3OSIsInVuaXF1ZV9uYW1lIjoibGl2ZS5jb20jYW50aG9ueS5naXJldHRpQGdtYWlsLmNvbSIsInV0aSI6InBOWmwyeWFGLTBxNjhxV29tOVpRQUEiLCJ2ZXIiOiIxLjAifQ.FaokZ9aXG3zsc8lcYB6ivH4Qi3l-l9fBYNcLsk1IR1mPoVqEpmjdNgAs-kExilIWolqjGMqbN8oCCDZBLmGgpDazsp_DSCbjmz4rPRMVLgZYOKOSFgbmG8C8MniiZ0X9JObEB3rT9XIutlej6xGMHrgenJq2Y3irEnlTwJPEGjB_3rNFtxA7rtwBAUtJz2We_FLaLTICiDzDhwIePfNV64zGjqE-9sIwj73RGsDqQmryeFStw1bV2hzqjwV7s8ZHn20hkavV1JNp_2dX8N1Dz1RvNYrulNIMmSa5gl_6k0fkfhHtVbeya2vAuSs2oUYgc0ZxBph2ivsUND55vhG5Mg");

            try
            {
                var countries = (await client.GetAllAsync(new EmptyRequest(), headers)).Countries.Select(x => new Country
                {
                    CountryId = x.Id,
                    Description = x.Description,
                    CountryName = x.Name
                }).ToList();
            }
            catch (RpcException e)
            {

            }
        }
    }
}