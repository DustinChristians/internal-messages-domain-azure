using System.Threading.Tasks;
using Internal.Messages.Core.Models.Domain;
using Internal.Messages.Core.Models.Helpers;

namespace Internal.Messages.Core.Abstractions.Repositories
{
    public interface ISettingsRepository : IBaseRepository<Setting>
    {
        Task<T> GetSettingValue<T>(string key, T defaultValue);
        Task<AsyncTryGetResult<T>> TryGetSettingValue<T>(string key);
        Task<bool> TryUpdateSettingValue<T>(string key, T value);
    }
}
