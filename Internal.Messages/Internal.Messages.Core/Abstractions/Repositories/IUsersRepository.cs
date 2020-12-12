using System.Collections.Generic;
using System.Threading.Tasks;
using Internal.Messages.Core.Models.Domain;
using Internal.Messages.Core.Models.ResourceParameters;

namespace Internal.Messages.Core.Abstractions.Repositories
{
    public interface IUsersRepository : IBaseRepository<User>
    {
        Task<IEnumerable<User>> GetUsersAsync(UsersResourceParameters parameters);
    }
}
