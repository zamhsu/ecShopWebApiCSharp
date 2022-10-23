using AutoFixture;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NSubstitute;
using Repository.Contexts;
using Repository.Entities.Members;
using Repository.Implements;
using RepositoryTests.TestUtilities;
using RepositoryTests.TestUtilities.Fixtures;

namespace RepositoryTests.Implements;

[Collection(nameof(DatabaseCollectionFixture))]
public class EfRepositoryTests : IClassFixture<AdminMemberTableFixture>, IDisposable
{
    private readonly DbContext _context;
    private readonly TestSettingProvider _settingProvider;
    private readonly string _connectionString;
    private readonly IFixture _fixture;
    private readonly EfRepository<AdminMember> _sut;

    public EfRepositoryTests()
    {
        _settingProvider = new TestSettingProvider();
        _connectionString = _settingProvider.ConnectionString;
        var contextOptions = new DbContextOptionsBuilder<EcShopContext>()
            .UseNpgsql(_settingProvider.ConnectionString)
            .Options;
        _context = new EcShopContext(contextOptions);
        _fixture = new Fixture();
        _sut = new EfRepository<AdminMember>(_context);

        CreateData();
    }

    public void Dispose()
    {
        TruncateData();
    }

    private void CreateData()
    {
        var adminMemberPath = Path.Combine("DatabaseScripts", "Data_AdminMember.sql");

        using var adminMemberStreamReader = new StreamReader(adminMemberPath);
        var adminMemberSql = adminMemberStreamReader.ReadToEnd();
        
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        connection.Execute(adminMemberSql);
    }

    private void TruncateData()
    {
        var sql = @"TRUNCATE public.""AdminMember"", public.""AdminMemberStatus"" RESTART IDENTITY;";

        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        connection.Execute(sql);
    }
    
    [Fact]
    public void TestDb()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetAsync_輸入有資料的Id_應回傳一筆資料()
    {
        // arrange
        var guid = "936DA01F-9ABD-4d9d-80C7-02AF85C822A8";
        
        // act
        var actual = await _sut.GetAsync(q => q.Guid == guid);

        // assert
        Assert.NotNull(actual);
        Assert.Contains(guid, actual.Guid);
    }
    
    [Fact]
    public async Task GetAsync_輸入沒有資料的Id_應回傳null()
    {
        // arrange
        var guid = Guid.Empty.ToString();
        
        // act
        var actual = await _sut.GetAsync(q => q.Guid == guid);

        // assert
        Assert.Null(actual);
    }
    
    [Fact]
    public void GetAll_資料庫有2筆資料_應回傳2筆資料()
    {
        // arrange
        var count = 2;

        // act
        var actual = _sut.GetAll();

        // assert
        Assert.NotNull(actual);
        Assert.Equal(count, actual.Count());
    }

    [Fact]
    public void GetAll_資料庫沒有資料_應回傳空集合()
    {
        // arrange
        TruncateData();
        
        // act
        var actual = _sut.GetAll();

        // assert
        Assert.Empty(actual);
    }
    
    [Fact]
    public void GetAllNoTracking_資料庫有2筆資料_應回傳2筆資料()
    {
        // arrange
        var count = 2;

        // act
        var actual = _sut.GetAllNoTracking();

        // assert
        Assert.NotNull(actual);
        Assert.Equal(count, actual.Count());
    }

    [Fact]
    public void GetAllNoTracking_資料庫沒有資料_應回傳空集合()
    {
        // arrange
        TruncateData();
        
        // act
        var actual = _sut.GetAllNoTracking();

        // assert
        Assert.Empty(actual);
    }
    
    [Fact]
    public void CreateAsync_輸入一筆null的資料_應拋出例外ArgumentNullException()
    {
        // arrange
        AdminMember adminMember = null;

        // act
        var exception = Assert.ThrowsAsync<ArgumentNullException>(
            () => _sut.CreateAsync(adminMember));

        // assert
        Assert.NotNull(exception.Result.Message);
        Assert.Contains("entity", exception.Result.Message);
    }
    
    [Fact]
    public void CreateAsync_輸入一筆null的集合資料_應拋出例外ArgumentNullException()
    {
        // arrange
        List<AdminMember> adminMembers = null;

        // act
        var exception = Assert.ThrowsAsync<ArgumentNullException>(
            () => _sut.CreateAsync(adminMembers));

        // assert
        Assert.NotNull(exception.Result.Message);
        Assert.Contains("source", exception.Result.Message);
    }
    
    [Fact]
    public void CreateAsync_輸入一筆空集合的資料_應拋出例外ArgumentNullException()
    {
        // arrange
        var adminMembers = new List<AdminMember>();

        // act
        var exception = Assert.ThrowsAsync<ArgumentNullException>(
            () => _sut.CreateAsync(adminMembers));

        // assert
        Assert.NotNull(exception.Result.Message);
        Assert.Contains("entities", exception.Result.Message);
    }
    
    [Fact]
    public void UpdateAsync_輸入一筆null的資料_應拋出例外ArgumentNullException()
    {
        // arrange
        AdminMember adminMember = null;

        // act
        var exception = Assert.Throws<ArgumentNullException>(
            () => _sut.Update(adminMember));

        // assert
        Assert.NotNull(exception.Message);
        Assert.Contains("entity", exception.Message);
    }
    
    [Fact]
    public void UpdateAsync_輸入一筆null的集合資料_應拋出例外ArgumentNullException()
    {
        // arrange
        List<AdminMember> adminMembers = null;

        // act
        var exception = Assert.Throws<ArgumentNullException>(
            () => _sut.Update(adminMembers));

        // assert
        Assert.NotNull(exception.Message);
        Assert.Contains("source", exception.Message);
    }
    
    [Fact]
    public void UpdateAsync_輸入一筆空集合的資料_應拋出例外ArgumentNullException()
    {
        // arrange
        var adminMembers = new List<AdminMember>();

        // act
        var exception = Assert.Throws<ArgumentNullException>(
            () => _sut.Update(adminMembers));

        // assert
        Assert.NotNull(exception.Message);
        Assert.Contains("entities", exception.Message);
    }
    
    [Fact]
    public void DeleteAsync_輸入一筆null的資料_應拋出例外ArgumentNullException()
    {
        // arrange
        AdminMember adminMember = null;

        // act
        var exception = Assert.Throws<ArgumentNullException>(
            () => _sut.Delete(adminMember));

        // assert
        Assert.NotNull(exception.Message);
        Assert.Contains("entity", exception.Message);
    }
    
    [Fact]
    public void DeleteAsync_輸入一筆null的集合資料_應拋出例外ArgumentNullException()
    {
        // arrange
        List<AdminMember> adminMembers = null;

        // act
        var exception = Assert.Throws<ArgumentNullException>(
            () => _sut.Delete(adminMembers));

        // assert
        Assert.NotNull(exception.Message);
        Assert.Contains("source", exception.Message);
    }
    
    [Fact]
    public void DeleteAsync_輸入一筆空集合的資料_應拋出例外ArgumentNullException()
    {
        // arrange
        var adminMembers = new List<AdminMember>();

        // act
        var exception = Assert.Throws<ArgumentNullException>(
            () => _sut.Delete(adminMembers));

        // assert
        Assert.NotNull(exception.Message);
        Assert.Contains("entities", exception.Message);
    }
}
