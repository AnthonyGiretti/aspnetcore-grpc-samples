using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DemoAspNetCore3.Services
{
    public class MyOwnGRpcService : MyOwnService.MyOwnServiceBase
    {
        private readonly ILogger<MyOwnGRpcService> _logger;
        public MyOwnGRpcService(ILogger<MyOwnGRpcService> logger)
        {
            _logger = logger;
        }

        //[Authorize]
        public override Task<WhoIsReply> WhoIs(EmptyRequest request, ServerCallContext context)
        {
            //throw new RpcException(new Status(StatusCode.Internal, "Ooooops!"));
            var user = context.GetHttpContext().User;
            var httpContext = context.GetHttpContext();
            return Task.FromResult(new WhoIsReply
            {
                Message = "Anthony" 
            });
        }

        //[Authorize]
        public override Task<IntroduceYourselfReply> IntroduceYourself(IntroduceYourselfRequest request, ServerCallContext context)
        {
            var user = context.GetHttpContext().User;
            var httpContext = context.GetHttpContext();

            if (string.IsNullOrEmpty(request.Name))
            {
                context.ResponseTrailers.Add(new Metadata.Entry("Name","the value was empty"));
                context.Status = new Status(StatusCode.InvalidArgument, $"Validation failed");
                return Task.FromResult(new IntroduceYourselfReply());
            }
            
            return Task.FromResult(new IntroduceYourselfReply
            {
                Name = request.Name,
                Job = "Developer",
                Country = "Canada",
                Citizenship = { new string[] {"France", "Canada" } },
                Skills = { new Skill[] { new Skill { SkillName = ".NET", SkillLevel = SkillLevel.Good }, 
                                         new Skill { SkillName = "CSS", SkillLevel = SkillLevel.Bad } } 
                }
            });
        }
    }
}