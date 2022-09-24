using System;
using System.Collections.Generic;
using AutoFixture;
using Repository.Entities.Orders;
using Service.Dtos.Orders;
using Service.Implements.Orders;
using ServiceTests.TestUtilities;
using Xunit;

namespace ServiceTests.Implements.Orders;

public class OrderAmountServiceTests : IClassFixture<CommonClassFixture>
{
    private readonly CommonClassFixture _commonFixture;
    private readonly OrderAmountService _sut;

    public OrderAmountServiceTests(CommonClassFixture commonFixture)
    {
        _commonFixture = commonFixture;
        _sut = new OrderAmountService();
    }

    [Fact]
    public void CalculateDiscountAmount_輸入金額1000_數量3_應回傳3000()
    {
        // arrange
        var expected = 3000;
        var item = _commonFixture.Fixture.Build<PlaceOrderDetailDto>()
            .With(q => q.Price, 1000)
            .With(q => q.Quantity, 3)
            .Create();

        // act
        var actual = _sut.CalculateItemTotal(item);

        // assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CalculateDiscountAmount_輸入優惠80_金額1000_應回傳負200()
    {
        // arrange
        var expected = -200;
        var totalAmount = 1000;
        var coupon = _commonFixture.Fixture.Build<Coupon>()
            .With(q => q.DiscountPercentage, 80)
            .With(q => q.CouponStatus, new CouponStatus())
            .With(q => q.OrderDetail, new List<OrderDetail>())
            .Create();

        // act
        var actual = _sut.CalculateDiscountAmount(coupon, totalAmount);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(1000, -50, 950)]
    [InlineData(1000, -100, 900)]
    [InlineData(2050, -50, 2000)]
    public void CalculateTotalAmount_輸入商品總額_負數優惠金額_應回傳正確的應付金額(int itemTotalAmount, int discountAmount, int expected)
    {
        // arrange
        
        // act
        var actual = _sut.CalculateTotalAmount(itemTotalAmount, discountAmount);

        // assert
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [InlineData(1000, 50)]
    [InlineData(1000, 100)]
    [InlineData(2050, 50)]
    public void CalculateTotalAmount_輸入商品總額_正數優惠金額_應拋出ArgumentException(int itemTotalAmount, int discountAmount)
    {
        // arrange
        var expected = "優惠金額應該是負數";
        
        // act
        var exception = Assert.Throws<ArgumentException>(
            () => _sut.CalculateTotalAmount(itemTotalAmount, discountAmount));

        // assert
        Assert.Contains(expected, exception.Message);
    }
}