using System;
using System.Threading.Tasks;
using AutoMapper;
using Internal.Messages.Core.Abstractions.Repositories;
using Internal.Messages.Core.Models.Domain;
using Internal.Messages.Core.Models.Helpers;
using Internal.Messages.Repository.Data;
using Internal.Messages.Repository.Entities;

namespace Internal.Messages.Repository.Repositories.Settings
{
    public class SettingsRepository : BaseRepository<Setting, SettingEntity>, ISettingsRepository
    {
        public SettingsRepository(InternalMessagesContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public async Task<T> GetSettingValue<T>(string key, T defaultValue)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return defaultValue;
            }

            var setting = await FirstOrDefaultAsync(x => x.Key == key);

            return setting == null ? defaultValue : (T)Convert.ChangeType(setting.Value, typeof(T));
        }

        public async Task<AsyncTryGetResult<T>> TryGetSettingValue<T>(string key)
        {
            var result = new AsyncTryGetResult<T>
            {
                Successful = false
            };

            if (string.IsNullOrWhiteSpace(key))
            {
                return result;
            }

            var setting = await FirstOrDefaultAsync(x => x.Key == key);

            if (setting == null)
            {
                return result;
            }

            result.Value = (T)Convert.ChangeType(setting.Value, typeof(T));
            result.Successful = true;

            return result;
        }

        public async Task<bool> TryUpdateSettingValue<T>(string key, T value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            var setting = await FirstOrDefaultAsync(x => x.Key == key);

            if (setting == null)
            {
                return false;
            }

            setting.Value = value.ToString();

            return true;
        }
    }
}
