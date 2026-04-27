using Grpc.Core;
using Library.Protos.User;
using UserService.Repositories;
using static Library.Protos.User.UserService;

namespace UserService.GRPC
{
    public class UserGrpcService : UserServiceBase
    {
        private readonly IUserRepository _repo;

        public UserGrpcService(IUserRepository repo)
        {
            _repo = repo;
        }

        public override async Task<UserResponse> GetById(UserIdRequest request, ServerCallContext context)
        {
            var user = await _repo.GetByIdAsync(request.UserId);
            if (user == null) throw new RpcException(new Status(StatusCode.NotFound, "User not found"));

            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}
