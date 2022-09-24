using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoFixture;
using Common.Interfaces;
using NSubstitute;
using Repository.Entities.Members;
using Service.Implements.Members;
using Xunit;
using System.Threading.Tasks;
using MockQueryable.NSubstitute;
using NSubstitute.ReturnsExtensions;
using Service.Dtos.Members;
using ServiceTests.TestUtilities;

namespace ServiceTests.Implements.Members;

public class AdminMemberServiceTests : IClassFixture<CommonClassFixture>
{
    private readonly CommonClassFixture _commonFixture;
    private readonly IAppLogger<AdminMember> _logger;
    private readonly AdminMemberService _sut;
    
    public AdminMemberServiceTests(CommonClassFixture commonFixture)
    {
        _commonFixture = commonFixture;
        _logger = Substitute.For<IAppLogger<AdminMember>>();
        _sut = new AdminMemberService(_commonFixture.UnitOfWork, _commonFixture.Mapper, _logger);
    }

    [Fact]
    public async Task GetByGuidAsync_輸入有資料的Guid_應回傳一筆資料()
    {
        // arrange
        var guid = Guid.NewGuid().ToString();
        var adminMember = _commonFixture.Fixture.Build<AdminMember>()
            .With(q => q.Guid, guid)
            .With(q => q.AdminMemberStatus, new AdminMemberStatus())
            .Create();
        
        _commonFixture.UnitOfWork.Repository<AdminMember>()
            .GetAsync(Arg.Any<Expression<Func<AdminMember, bool>>>())
            .Returns(adminMember);

        // act
        var actual = await _sut.GetByGuidAsync(guid);

        // assert
        Assert.NotNull(actual);
        Assert.Contains(guid, actual.Guid);
    }
    
    [Fact]
    public async Task GetByGuidAsync_輸入沒有資料的Guid_應回傳null()
    {
        // arrange
        var guid = Guid.NewGuid().ToString();

        _commonFixture.UnitOfWork.Repository<AdminMember>()
            .GetAsync(Arg.Any<Expression<Func<AdminMember, bool>>>())
            .ReturnsNull();

        // act
        var actual = await _sut.GetByGuidAsync(guid);

        // assert
        Assert.Null(actual);
    }
    
    [Fact]
    public async Task GetDetailByGuidAsync_輸入有資料的Guid_應回傳一筆資料()
    {
        // arrange
        var guid = Guid.NewGuid().ToString();
        var adminMember = _commonFixture.Fixture.Build<AdminMember>()
            .With(q => q.Guid, guid)
            .With(q => q.AdminMemberStatus, new AdminMemberStatus())
            .CreateMany(1)
            .AsQueryable()
            .BuildMock();

        _commonFixture.UnitOfWork.Repository<AdminMember>()
            .GetAll()
            .Returns(adminMember);

        // act
        var actual = await _sut.GetDetailByGuidAsync(guid);

        // assert
        Assert.NotNull(actual);
        Assert.Contains(guid, actual.Guid);
    }
    
    [Fact]
    public async Task GetDetailByGuidAsync_輸入沒有資料的Guid_應回傳null()
    {
        // arrange
        var guid = Guid.NewGuid().ToString();
        var adminMembers = new List<AdminMember>()
            .AsQueryable()
            .BuildMock();

        _commonFixture.UnitOfWork.Repository<AdminMember>()
            .GetAll()
            .Returns(adminMembers);

        // act
        var actual = await _sut.GetDetailByGuidAsync(guid);

        // assert
        Assert.Null(actual);
    }
    
    [Fact]
    public async Task GetByAccountAsync_輸入有資料的Guid_應回傳一筆資料()
    {
        // arrange
        var guid = Guid.NewGuid().ToString();
        var adminMember = _commonFixture.Fixture.Build<AdminMember>()
            .With(q => q.Guid, guid)
            .With(q => q.AdminMemberStatus, new AdminMemberStatus())
            .Create();

        _commonFixture.UnitOfWork.Repository<AdminMember>()
            .GetAsync(Arg.Any<Expression<Func<AdminMember, bool>>>())
            .Returns(adminMember);

        // act
        var actual = await _sut.GetByAccountAsync(guid);

        // assert
        Assert.NotNull(actual);
        Assert.Contains(guid, actual.Guid);
    }
    
    [Fact]
    public async Task GetByAccountAsync_輸入沒有資料的Guid_應回傳null()
    {
        // arrange
        var guid = Guid.NewGuid().ToString();

        _commonFixture.UnitOfWork.Repository<AdminMember>()
            .GetAsync(Arg.Any<Expression<Func<AdminMember, bool>>>())
            .ReturnsNull();

        // act
        var actual = await _sut.GetByAccountAsync(guid);

        // assert
        Assert.Null(actual);
    }
    
    [Fact]
    public async Task GetAllAsync_有5筆資料_應回傳5筆資料()
    {
        // arrange
        var count = 5;
        var adminMembers = _commonFixture.Fixture.Build<AdminMember>()
            .With(q => q.AdminMemberStatus, new AdminMemberStatus())
            .CreateMany(count)
            .AsQueryable()
            .BuildMock();

        _commonFixture.UnitOfWork.Repository<AdminMember>()
            .GetAll()
            .Returns(adminMembers);

        // act
        var actual = await _sut.GetAllAsync();

        // assert
        Assert.NotEmpty(actual);
        Assert.Equal(count, actual.Count);
    }
    
    [Fact]
    public async Task GetAllAsync_沒有資料_應回傳空集合()
    {
        // arrange
        var adminMembers = new List<AdminMember>()
            .AsQueryable()
            .BuildMock();
        
        _commonFixture.UnitOfWork.Repository<AdminMember>()
            .GetAll()
            .Returns(adminMembers);

        // act
        var actual = await _sut.GetAllAsync();

        // assert
        Assert.Empty(actual);
    }
    
    [Fact]
    public async Task GetDetailAllAsync_有5筆資料_應回傳5筆資料()
    {
        // arrange
        var count = 5;
        var adminMembers = _commonFixture.Fixture.Build<AdminMember>()
            .With(q => q.AdminMemberStatus, new AdminMemberStatus())
            .CreateMany(count)
            .AsQueryable()
            .BuildMock();

        _commonFixture.UnitOfWork.Repository<AdminMember>()
            .GetAll()
            .Returns(adminMembers);

        // act
        var actual = await _sut.GetDetailAllAsync();

        // assert
        Assert.NotEmpty(actual);
        Assert.Equal(count, actual.Count);
    }
    
    [Fact]
    public async Task GetDetailAllAsync_沒有資料_應回傳空集合()
    {
        // arrange
        var adminMembers = new List<AdminMember>()
            .AsQueryable()
            .BuildMock();
        
        _commonFixture.UnitOfWork.Repository<AdminMember>()
            .GetAll()
            .Returns(adminMembers);

        // act
        var actual = await _sut.GetDetailAllAsync();

        // assert
        Assert.Empty(actual);
    }
    
    [Theory]
    [InlineData(5, 1)]
    public void GetPagedDetailAll_輸入有資料的頁數_應回傳5筆資料(int pageSize, int page)
    {
        // arrange
        var count = 5;
        var adminMembers = _commonFixture.Fixture.Build<AdminMember>()
            .With(q => q.AdminMemberStatus, new AdminMemberStatus())
            .CreateMany(count)
            .AsQueryable()
            .BuildMock();

        _commonFixture.UnitOfWork.Repository<AdminMember>()
            .GetAll()
            .Returns(adminMembers);

        // act
        var actual = _sut.GetPagedDetailAll(pageSize, page);

        // assert
        Assert.NotEmpty(actual.PagedData);
        Assert.Equal(count, actual.PagedData.Count);
    }
    
    [Theory]
    [InlineData(100, 100)]
    [InlineData(50, 10)]
    public void GetPagedDetailAll_輸入沒有資料的頁數_應回傳空集合(int pageSize, int page)
    {
        // arrange
        var count = 5;
        var adminMembers = _commonFixture.Fixture.Build<AdminMember>()
            .With(q => q.AdminMemberStatus, new AdminMemberStatus())
            .CreateMany(count)
            .AsQueryable()
            .BuildMock();

        _commonFixture.UnitOfWork.Repository<AdminMember>()
            .GetAll()
            .Returns(adminMembers);

        // act
        var actual = _sut.GetPagedDetailAll(pageSize, page);

        // assert
        Assert.Empty(actual.PagedData);
    }
    
    [Fact]
    public async Task CreateAsync_輸入正確的資料_新增成功_應回傳true()
    {
        // arrange
        var createDto = _commonFixture.Fixture.Build<AdminMemberCreateDto>().Create();

        await _commonFixture.UnitOfWork.Repository<AdminMember>()
            .CreateAsync(Arg.Any<AdminMember>());
        _commonFixture.UnitOfWork.SaveChangesAsync().Returns(1);

        // act
        var actual = await _sut.CreateAsync(createDto);

        // assert
        Assert.True(actual);
    }
    
    [Fact]
    public async Task CreateAsync_輸入正確的資料_新增失敗_應回傳true()
    {
        // arrange
        var createDto = _commonFixture.Fixture.Build<AdminMemberCreateDto>().Create();

        await _commonFixture.UnitOfWork.Repository<AdminMember>()
            .CreateAsync(Arg.Any<AdminMember>());
        _commonFixture.UnitOfWork.SaveChangesAsync().Returns(0);

        // act
        var actual = await _sut.CreateAsync(createDto);

        // assert
        Assert.False(actual);
    }
    
    [Fact]
    public void CreateAsync_輸入空的資料_應回傳拋出ArgumentNullException()
    {
        // arrange

        // act
        var exception = Assert.ThrowsAsync<ArgumentNullException>(
            () => _sut.CreateAsync(null));

        // assert
        Assert.NotNull(exception.Result.Message);
        _logger.Received(1).LogInformation(Arg.Any<string>());
    }

    [Fact]
    public async Task UpdateUserInfoAsync_輸入正確的資料_修改成功_應回傳true()
    {
        // arrange
        var userInfoDto = _commonFixture.Fixture.Build<AdminMemberUserInfoDto>().Create();
        var adminMembers = _commonFixture.Fixture.Build<AdminMember>()
            .With(q => q.AdminMemberStatus, new AdminMemberStatus())
            .Create();

        _commonFixture.UnitOfWork.Repository<AdminMember>()
            .GetAsync(Arg.Any<Expression<Func<AdminMember, bool>>>()).Returns(adminMembers);
        _commonFixture.UnitOfWork.SaveChangesAsync().Returns(1);

        // act
        var actual = await _sut.UpdateUserInfoAsync(userInfoDto);

        // assert
        Assert.True(actual);
    }
    
    [Fact]
    public async Task UpdateUserInfoAsync_輸入正確的資料_修改失敗_應回傳false()
    {
        // arrange
        var userInfoDto = _commonFixture.Fixture.Build<AdminMemberUserInfoDto>().Create();
        var adminMembers = _commonFixture.Fixture.Build<AdminMember>()
            .With(q => q.AdminMemberStatus, new AdminMemberStatus())
            .Create();

        _commonFixture.UnitOfWork.Repository<AdminMember>()
            .GetAsync(Arg.Any<Expression<Func<AdminMember, bool>>>()).Returns(adminMembers);
        _commonFixture.UnitOfWork.SaveChangesAsync().Returns(0);

        // act
        var actual = await _sut.UpdateUserInfoAsync(userInfoDto);

        // assert
        Assert.False(actual);
    }

    [Fact]
    public void UpdateUserInfoAsync_輸入空的資料_應回傳拋出ArgumentNullException()
    {
        // arrange
        var excepted = "userInfoDto";

        // act
        var exception = Assert.ThrowsAsync<ArgumentNullException>(
            () => _sut.UpdateUserInfoAsync(null));

        // assert
        Assert.Contains(excepted, exception.Result.Message);
        _logger.Received(1).LogInformation(Arg.Any<string>());
    }
    
    [Fact]
    public void UpdateUserInfoAsync_輸入不存在的資料_應回傳拋出ArgumentException()
    {
        // arrange
        var excepted = "userInfoDto";
        var userInfoDto = _commonFixture.Fixture.Build<AdminMemberUserInfoDto>().Create();

        _commonFixture.UnitOfWork.Repository<AdminMember>()
            .GetAsync(Arg.Any<Expression<Func<AdminMember, bool>>>()).ReturnsNull();
        _commonFixture.UnitOfWork.SaveChangesAsync().Returns(1);

        // act
        var exception = Assert.ThrowsAsync<ArgumentException>(
            () => _sut.UpdateUserInfoAsync(userInfoDto));

        // assert
        Assert.NotNull(exception.Result.Message);
        Assert.Contains(excepted, exception.Result.Message);
        _logger.Received(1).LogInformation(Arg.Any<string>());
    }
    
    [Fact]
    public async Task UpdateAsync_輸入正確的資料_修改成功_應回傳true()
    {
        // arrange
        var updateDto = _commonFixture.Fixture.Build<AdminMemberUpdateDto>().Create();
        var adminMembers = _commonFixture.Fixture.Build<AdminMember>()
            .With(q => q.AdminMemberStatus, new AdminMemberStatus())
            .Create();

        _commonFixture.UnitOfWork.Repository<AdminMember>()
            .GetAsync(Arg.Any<Expression<Func<AdminMember, bool>>>()).Returns(adminMembers);
        _commonFixture.UnitOfWork.SaveChangesAsync().Returns(1);

        // act
        var actual = await _sut.UpdateAsync(updateDto);

        // assert
        Assert.True(actual);
    }
    
    [Fact]
    public async Task UpdateAsync_輸入正確的資料_修改失敗_應回傳false()
    {
        // arrange
        var updateDto = _commonFixture.Fixture.Build<AdminMemberUpdateDto>().Create();
        var adminMembers = _commonFixture.Fixture.Build<AdminMember>()
            .With(q => q.AdminMemberStatus, new AdminMemberStatus())
            .Create();

        _commonFixture.UnitOfWork.Repository<AdminMember>()
            .GetAsync(Arg.Any<Expression<Func<AdminMember, bool>>>()).Returns(adminMembers);
        _commonFixture.UnitOfWork.SaveChangesAsync().Returns(0);

        // act
        var actual = await _sut.UpdateAsync(updateDto);

        // assert
        Assert.False(actual);
    }

    [Fact]
    public void UpdateAsync_輸入空的資料_應回傳拋出ArgumentNullException()
    {
        // arrange
        var excepted = "updateDto";

        // act
        var exception = Assert.ThrowsAsync<ArgumentNullException>(
            () => _sut.UpdateAsync(null));

        // assert
        Assert.Contains(excepted, exception.Result.Message);
        _logger.Received(1).LogInformation(Arg.Any<string>());
    }
    
    [Fact]
    public void UpdateAsync_輸入不存在的資料_應回傳拋出ArgumentException()
    {
        // arrange
        var excepted = "updateDto";
        var updateDto = _commonFixture.Fixture.Build<AdminMemberUpdateDto>().Create();

        _commonFixture.UnitOfWork.Repository<AdminMember>()
            .GetAsync(Arg.Any<Expression<Func<AdminMember, bool>>>()).ReturnsNull();
        _commonFixture.UnitOfWork.SaveChangesAsync().Returns(1);

        // act
        var exception = Assert.ThrowsAsync<ArgumentException>(
            () => _sut.UpdateAsync(updateDto));

        // assert
        Assert.NotNull(exception.Result.Message);
        Assert.Contains(excepted, exception.Result.Message);
        _logger.Received(1).LogInformation(Arg.Any<string>());
    }
    
    [Fact]
    public async Task DeleteByGuidAsync_輸入正確的資料_刪除成功_應回傳true()
    {
        // arrange
        var guid = _commonFixture.Fixture.Build<string>().Create();
        var adminMembers = _commonFixture.Fixture.Build<AdminMember>()
            .With(q => q.AdminMemberStatus, new AdminMemberStatus())
            .Create();

        _commonFixture.UnitOfWork.Repository<AdminMember>()
            .GetAsync(Arg.Any<Expression<Func<AdminMember, bool>>>()).Returns(adminMembers);
        _commonFixture.UnitOfWork.SaveChangesAsync().Returns(1);

        // act
        var actual = await _sut.DeleteByGuidAsync(guid);

        // assert
        Assert.True(actual);
    }
    
    [Fact]
    public async Task DeleteByGuidAsync_輸入正確的資料_刪除失敗_應回傳false()
    {
        // arrange
        var guid = _commonFixture.Fixture.Build<string>().Create();
        var adminMembers = _commonFixture.Fixture.Build<AdminMember>()
            .With(q => q.AdminMemberStatus, new AdminMemberStatus())
            .Create();

        _commonFixture.UnitOfWork.Repository<AdminMember>()
            .GetAsync(Arg.Any<Expression<Func<AdminMember, bool>>>()).Returns(adminMembers);
        _commonFixture.UnitOfWork.SaveChangesAsync().Returns(0);

        // act
        var actual = await _sut.DeleteByGuidAsync(guid);

        // assert
        Assert.False(actual);
    }

    [Fact]
    public void DeleteByGuidAsync_輸入空的資料_應回傳拋出ArgumentNullException()
    {
        // arrange
        var excepted = "guid";

        // act
        var exception = Assert.ThrowsAsync<ArgumentNullException>(
            () => _sut.DeleteByGuidAsync(null));

        // assert
        Assert.Contains(excepted, exception.Result.Message);
        _logger.Received(1).LogInformation(Arg.Any<string>());
    }
    
    [Fact]
    public void DeleteByGuidAsync_輸入不存在的資料_應回傳拋出ArgumentException()
    {
        // arrange
        var excepted = "guid";
        var guid = _commonFixture.Fixture.Build<string>().Create();

        _commonFixture.UnitOfWork.Repository<AdminMember>()
            .GetAsync(Arg.Any<Expression<Func<AdminMember, bool>>>()).ReturnsNull();
        _commonFixture.UnitOfWork.SaveChangesAsync().Returns(1);

        // act
        var exception = Assert.ThrowsAsync<ArgumentException>(
            () => _sut.DeleteByGuidAsync(guid));

        // assert
        Assert.NotNull(exception.Result.Message);
        Assert.Contains(excepted, exception.Result.Message);
        _logger.Received(1).LogInformation(Arg.Any<string>());
    }
}