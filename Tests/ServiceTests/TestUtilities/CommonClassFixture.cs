using AutoFixture;
using AutoMapper;
using NSubstitute;
using Repository.Interfaces;
using Service.Mappings;

namespace ServiceTests.TestUtilities;

public class CommonClassFixture
{
    public IUnitOfWork UnitOfWork { get; private set; }
    public IMapper Mapper { get; private set; }
    public IFixture Fixture { get; private set; }

    public CommonClassFixture()
    {
        UnitOfWork = Substitute.For<IUnitOfWork>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ServicesProfile>();
        });
        Mapper = new Mapper(config);
        Fixture = new Fixture();
    }
}