using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;

namespace Internal.Messages.TestUtilities
{
    public static class MapperUtilities
    {
        public static IMapper GetTestMapper()
        {
            var mappingConfig = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddMaps("Internal.Messages.Infrastructure");
                    cfg.AddMaps("Internal.Messages.Repository");
                    cfg.AddExpressionMapping();
                });

            var mapper = mappingConfig.CreateMapper();

            return mapper;
        }
    }
}
