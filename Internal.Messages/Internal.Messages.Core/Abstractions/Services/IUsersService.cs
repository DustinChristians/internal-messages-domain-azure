using Internal.Messages.Core.Abstractions.Repositories;

namespace Internal.Messages.Core.Abstractions.Services
{
    public interface IUsersService
    {
        public IUsersRepository UsersRepository { get; }
    }
}
