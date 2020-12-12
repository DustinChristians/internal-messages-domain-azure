using Internal.Messages.Core.Abstractions.Repositories;
using Internal.Messages.Core.Abstractions.Services;

namespace Internal.Messages.Infrastructure.Services
{
    public class UsersService : IUsersService
    {
        public UsersService(IUsersRepository usersRepository)
        {
            this.UsersRepository = usersRepository;
        }

        public IUsersRepository UsersRepository { get; }

        // Add business logic here
    }
}
