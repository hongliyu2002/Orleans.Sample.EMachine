using EMachine.Orleans.Shared;
using EMachine.Sales.Orleans.States;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace EMachine.Sales.Orleans.Tests;

[Collection(MoneyCollectionFixture.Name)]
public class MoneyTests
{
    private readonly ITestOutputHelper _output;

    public MoneyTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Two_Money_Instances_Equal_If_Contain_The_Same_Money_Amount()
    {
        var money1 = new Money(1, 2, 3, 4, 5, 6, 7);
        var money2 = new Money(1, 2, 3, 4, 5, 6, 7);
        money1.Should().Be(money2);
        money1.GetHashCode().Should().Be(money2.GetHashCode());
    }

    [Fact]
    public void Two_Money_Instances_Do_Not_Equal_If_Contain_Different_Money_Amounts()
    {
        var yuan100 = Money.OneHundredYuan;
        var yuan1x100 = new Money(100, 0, 0, 0, 0, 0, 0);
        yuan100.Should().NotBe(yuan1x100);
        yuan100.Amount.Should().Be(yuan1x100.Amount);
    }

    [Theory]
    [InlineData(-1, 0, 0, 0, 0, 0, 0)]
    [InlineData(0, -2, 0, 0, 0, 0, 0)]
    [InlineData(0, 0, -3, 0, 0, 0, 0)]
    [InlineData(0, 0, 0, -4, 0, 0, 0)]
    [InlineData(0, 0, 0, 0, -5, 0, 0)]
    [InlineData(0, 0, 0, 0, 0, -6, 0)]
    [InlineData(0, 0, 0, 0, 0, 0, -7)]
    public void Cannot_Create_Money_With_Negative_Value(int yuan1, int yuan2, int yuan5, int yuan10, int yuan20, int yuan50, int yuan100)
    {
        var action = () =>
                     {
                         var money = new Money(yuan1, yuan2, yuan5, yuan10, yuan20, yuan50, yuan100);
                         _output.WriteLine(money.ToString());
                     };
        action.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0, 0, 0, 0, 0, 0, 0, 0)]
    [InlineData(1, 0, 0, 0, 0, 0, 0, 1)]
    [InlineData(1, 2, 0, 0, 0, 0, 0, 5)]
    [InlineData(1, 2, 3, 0, 0, 0, 0, 20)]
    [InlineData(1, 2, 3, 4, 0, 0, 0, 60)]
    [InlineData(1, 2, 3, 4, 5, 0, 0, 160)]
    [InlineData(1, 2, 3, 4, 5, 6, 0, 460)]
    [InlineData(11, 0, 0, 0, 0, 0, 10, 1011)]
    [InlineData(110, 0, 0, 0, 100, 0, 0, 2110)]
    public void Amount_Is_Calculated_Correctly(int yuan1, int yuan2, int yuan5, int yuan10, int yuan20, int yuan50, int yuan100, decimal expectedAmount)
    {
        var money = new Money(yuan1, yuan2, yuan5, yuan10, yuan20, yuan50, yuan100);
        money.Amount.Should().Be(expectedAmount);
    }

    [Fact]
    public void Sum_Of_Two_Moneys_Produces_Correct_Result()
    {
        var money1 = new Money(1, 2, 3, 4, 5, 6, 7);
        var money2 = new Money(1, 2, 3, 4, 5, 6, 7);
        var moneyResult = money1 + money2;
        moneyResult.Yuan1.Should().Be(2);
        moneyResult.Yuan2.Should().Be(4);
        moneyResult.Yuan5.Should().Be(6);
        moneyResult.Yuan10.Should().Be(8);
        moneyResult.Yuan20.Should().Be(10);
        moneyResult.Yuan50.Should().Be(12);
        moneyResult.Yuan100.Should().Be(14);
    }

    [Fact]
    public void Subtraction_Of_Two_Moneys_Produces_Correct_Result()
    {
        var money1 = new Money(10, 10, 10, 10, 10, 10, 10);
        var money2 = new Money(1, 2, 3, 4, 5, 6, 7);
        var moneyResult = money1 - money2;
        moneyResult.Yuan1.Should().Be(9);
        moneyResult.Yuan2.Should().Be(8);
        moneyResult.Yuan5.Should().Be(7);
        moneyResult.Yuan10.Should().Be(6);
        moneyResult.Yuan20.Should().Be(5);
        moneyResult.Yuan50.Should().Be(4);
        moneyResult.Yuan100.Should().Be(3);
    }

    [Fact]
    public void Cannot_Subtract_More_Than_Exists()
    {
        var money1 = new Money(0, 1, 0, 0, 0, 0, 0);
        var money2 = new Money(1, 0, 0, 0, 0, 0, 0);
        var action = () =>
                     {
                         var money = money1 - money2;
                         _output.WriteLine(money.ToString());
                     };
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Can_Create_Money()
    {
        var moneyResult = Money.Create(1, 2, 3, 4, 5, 6, 7);
        moneyResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Cannot_Create_Negative_Money()
    {
        var moneyResult = Money.Create(-1, 2, -3, 4, -5, 6, -7);
        moneyResult.IsFailed.Should().BeTrue();
        moneyResult.Errors.Should().HaveCount(4);
        _output.WriteLine(moneyResult.ToString());
    }
}
