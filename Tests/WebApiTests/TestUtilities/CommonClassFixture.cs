using AutoFixture;
using AutoMapper;
using WebApi.Infrastructures.Mappings;

namespace WebApiTests.TestUtilities;

public class CommonClassFixture
{
    public IMapper Mapper { get; private set; }
    public IFixture Fixture { get; private set; }

    public CommonClassFixture()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ControllersProfile>();
        });
        Mapper = new Mapper(config);
        Fixture = new Fixture();
    }
}