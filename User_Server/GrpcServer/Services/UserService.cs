using Grpc.Core;
using GrpcServer.Protos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWT.Builder;
using JWT.Exceptions;
using Microsoft.AspNetCore.Authorization;
using GrpcServer.DbContexts;

namespace GrpcServer.Services
{
    public class UserService : User.UserBase
    {
        private readonly ILogger<UserService> _logger;
        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }
        [Authorize]
        public override async Task<UserModel> GetUserInfo(UserInput request, ServerCallContext context)
        {
            

            UserModel output = new UserModel();
            using (var Db = new Dbcontext())
            {
                var infouser = Db.Users.AsQueryable().Where(x => x.Id == request.UserId).ToList();
                foreach (var info in infouser)
                {
                    output.UserName = info.UserName;
                    output.Email = info.Email;
                }
            }
            return output;
        }
    }
}
