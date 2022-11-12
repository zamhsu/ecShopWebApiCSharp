using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using Common.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.Net.Http.Headers;
using NSubstitute;
using Service.Dtos.Members;
using Service.Interfaces.Members;
using WebApi.Controllers;
using WebApi.Infrastructures.Core;
using WebApi.Infrastructures.Helpers;
using WebApi.Infrastructures.Models.Paramaters;
using WebApiTests.TestUtilities;
using Xunit;

namespace WebApiTests.Controllers;

public class AdminMemberAccountControllerTests : IClassFixture<CommonClassFixture>
{
    private readonly IAdminMemberService _adminMemberService;
    private readonly IAdminMemberAccountService _adminMemberAccountService;
    private readonly IMapper _mapper;
    private readonly JwtHelper _jwtHelper;
    private readonly IFixture _fixture;
    private readonly AdminMemberAccountController _sut;

    public AdminMemberAccountControllerTests(CommonClassFixture commonClassFixture)
    {
        _adminMemberService = Substitute.For<IAdminMemberService>();
        _adminMemberAccountService = Substitute.For<IAdminMemberAccountService>();
        _mapper = commonClassFixture.Mapper;
        _jwtHelper = new JwtHelper(AppSettingProvider.GetTestAppSettings());
        _fixture = commonClassFixture.Fixture;
        _sut = new AdminMemberAccountController(_adminMemberService, _adminMemberAccountService, _mapper, _jwtHelper);
    }

    [Fact]
    public async Task Login_輸入符合規格的資料_應回傳Token()
    {
        // arrange
        var adminMemberInfoDto = _fixture.Build<AdminMemberInfoDto>()
            .With(q => q.ExpirationDate, DateTimeOffset.Now)
            .Create();
        var loginParam = _fixture.Build<AdminMemberLoginParameter>().Create();
        var param = _fixture.Build<BaseRequest<AdminMemberLoginParameter>>()
            .With(q => q.Data, loginParam)
            .Create();
        
        _adminMemberAccountService.LoginAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(adminMemberInfoDto);
        
        // act
        var actual = await _sut.Login(param);

        // assert
        Assert.True(actual.Value.IsSuccess);
        Assert.NotNull(actual.Value.Data);
    }
    
    [Fact]
    public async Task Login_輸入不存在的資料_應回傳登入失敗()
    {
        // arrange
        AdminMemberInfoDto adminMemberInfoDto = null;
        var loginParam = _fixture.Build<AdminMemberLoginParameter>().Create();
        var param = _fixture.Build<BaseRequest<AdminMemberLoginParameter>>()
            .With(q => q.Data, loginParam)
            .Create();
        
        _adminMemberAccountService.LoginAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(adminMemberInfoDto);
        
        // act
        var actual = await _sut.Login(param);

        // assert
        Assert.False(actual.Value.IsSuccess);
        Assert.NotNull(actual.Value.Message);
        Assert.Contains("登入失敗", actual.Value.Message);
    }

    [Fact]
    public async Task LoginCheck_輸入合法的JWT_應回傳新的JWT()
    {
        // assert
        var adminMemberInfoDto = _fixture.Build<AdminMemberInfoDto>()
            .With(q => q.ExpirationDate, DateTimeOffset.Now)
            .Create();
        
        _adminMemberAccountService.LoginByOnlyGuidAsync(Arg.Any<string>()).Returns(adminMemberInfoDto);
        
        // act
        _sut.ControllerContext = GetControllerContextMock();
        var actual = await _sut.LoginCheck();

        // arrange
        Assert.True(actual.Value.IsSuccess);
        Assert.NotNull(actual.Value.Data);
    }
    
    [Fact]
    public async Task LoginCheck_輸入不合法的JWT_應回傳檢查失敗()
    {
        // assert
        AdminMemberInfoDto adminMemberInfoDto = null;
        
        _adminMemberAccountService.LoginByOnlyGuidAsync(Arg.Any<string>()).Returns(adminMemberInfoDto);
        
        // act
        _sut.ControllerContext = GetControllerContextMock();
        var actual = await _sut.LoginCheck();

        // arrange
        Assert.False(actual.Value.IsSuccess);
        Assert.NotNull(actual.Value.Message);
        Assert.Contains("檢查失敗", actual.Value.Message);
    }

    /// <summary>
    /// 取得自訂的ControllerContext
    /// </summary>
    /// <returns></returns>
    private ControllerContext GetControllerContextMock()
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, "userName"),
            new Claim(CustomClaimType.Guid, Guid.NewGuid().ToString()),
            new Claim("role", MemberRoleEnum.AdminMember.ToString("D"))
        };
        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };

         var controllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        return controllerContext;
    }
}