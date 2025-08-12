namespace BillNum.Tests;

using Xunit;
using BillNumParser;

public class UnitTest1
{
    [Fact]
    public void ValidInput_Caps_Space()
    {
        string id = "HCR 0046";
        var bill = new BillNumber(id);

        Assert.True(bill.IsValid);
        Assert.Equal("HCR00046", bill.BillNumberLong());
        Assert.Equal("HCR 46", bill.BillNumberShort());
    }

    [Fact]
    public void ValidInput_NoCaps_NoSpace()
    {
        string id = "sr00768";
        var bill = new BillNumber(id);

        Assert.True(bill.IsValid);
        Assert.Equal("SR00768", bill.BillNumberLong());
        Assert.Equal("SR 768", bill.BillNumberShort());
    }

    [Fact]
    public void ValidInput_MaximumSuffix()
    {
        string id = "sr99999";
        var bill = new BillNumber(id);

        Assert.True(bill.IsValid);
        Assert.Equal("SR99999", bill.BillNumberLong());
        Assert.Equal("SR 99999", bill.BillNumberShort());
    }

    [Fact]
    public void ValidInput_MixedCase()
    {
        string id = "sJr00003";
        var bill = new BillNumber(id);

        Assert.True(bill.IsValid);
        Assert.Equal("SJR00003", bill.BillNumberLong());
        Assert.Equal("SJR 3", bill.BillNumberShort());
    }

    [Fact]
    public void InvalidInput_ZeroSuffix()
    {
        string id = "scr00000";
        var bill = new BillNumber(id);

        Assert.False(bill.IsValid);
        var longEx = Assert.Throws<InvalidBillNumberException>(() => bill.BillNumberLong());
        var shortEx = Assert.Throws<InvalidBillNumberException>(() => bill.BillNumberShort());

        Assert.Equal("Bill number is invalid", longEx.Message);
        Assert.Equal("Bill number is invalid", shortEx.Message);
    }

    [Fact]
    public void InvalidInput_NoSuffix()
    {
        string id = "scr";
        var bill = new BillNumber(id);

        Assert.False(bill.IsValid);
        var longEx = Assert.Throws<InvalidBillNumberException>(() => bill.BillNumberLong());
        var shortEx = Assert.Throws<InvalidBillNumberException>(() => bill.BillNumberShort());

        Assert.Equal("Bill number is invalid", longEx.Message);
        Assert.Equal("Bill number is invalid", shortEx.Message);
    }

    [Fact]
    public void InvalidInput_OverFizeZeros()
    {
        string id = "sr000030";
        var bill = new BillNumber(id);

        Assert.False(bill.IsValid);
        // Why does this still work?
        var longEx = Assert.Throws<InvalidBillNumberException>(() => bill.BillNumberLong());
        var shortEx = Assert.Throws<InvalidBillNumberException>(() => bill.BillNumberShort());

        Assert.Equal("Bill number is invalid", longEx.Message);
        Assert.Equal("Bill number is invalid", shortEx.Message);
    }

    [Fact]
    public void InvalidInput_OverOneWhitespace()
    {
        string id = "sr  00003";
        var bill = new BillNumber(id);

        Assert.False(bill.IsValid);
        var longEx = Assert.Throws<InvalidBillNumberException>(() => bill.BillNumberLong());
        var shortEx = Assert.Throws<InvalidBillNumberException>(() => bill.BillNumberShort());

        Assert.Equal("Bill number is invalid", longEx.Message);
    }

    [Fact]
    public void InvalidInput_WhitespaceWrongPlaceSuffix()
    {
        string id = "sr0 0003";
        var bill = new BillNumber(id);

        Assert.False(bill.IsValid);
        var longEx = Assert.Throws<InvalidBillNumberException>(() => bill.BillNumberLong());
        var shortEx = Assert.Throws<InvalidBillNumberException>(() => bill.BillNumberShort());

        Assert.Equal("Bill number is invalid", longEx.Message);
    }

    [Fact]
    public void InvalidInput_WhitespaceWrongPlaceHead()
    {
        string id = "s r00003";
        var bill = new BillNumber(id);

        Assert.False(bill.IsValid);
        var longEx = Assert.Throws<InvalidBillNumberException>(() => bill.BillNumberLong());
        var shortEx = Assert.Throws<InvalidBillNumberException>(() => bill.BillNumberShort());

        Assert.Equal("Bill number is invalid", longEx.Message);
    }

    [Fact]
    public void InvalidInput_NoInput()
    {
        string id = "";
        var bill = new BillNumber(id);

        Assert.False(bill.IsValid);
        var longEx = Assert.Throws<InvalidBillNumberException>(() => bill.BillNumberLong());
        var shortEx = Assert.Throws<InvalidBillNumberException>(() => bill.BillNumberShort());

        Assert.Equal("Bill number is invalid", longEx.Message);
    }

    [Fact]
    public void InvalidInput_NullInput()
    {
        string id = null;
        var bill = new BillNumber(id);

        Assert.False(bill.IsValid);
        var longEx = Assert.Throws<InvalidBillNumberException>(() => bill.BillNumberLong());
        var shortEx = Assert.Throws<InvalidBillNumberException>(() => bill.BillNumberShort());

        Assert.Equal("Bill number is invalid", longEx.Message);
    }

    [Fact]
    public void InvalidInput_FlippedChamberType()
    {
        string id = "rs 00003";
        var bill = new BillNumber(id);

        Assert.False(bill.IsValid);
        var longEx = Assert.Throws<InvalidBillNumberException>(() => bill.BillNumberLong());
        var shortEx = Assert.Throws<InvalidBillNumberException>(() => bill.BillNumberShort());

        Assert.Equal("Bill number is invalid", longEx.Message);
    }

    [Fact]
    public void InvalidInput_InvalidCharacters()
    {
        string id = "sr} 00003";
        var bill = new BillNumber(id);

        Assert.False(bill.IsValid);
        var longEx = Assert.Throws<InvalidBillNumberException>(() => bill.BillNumberLong());
        var shortEx = Assert.Throws<InvalidBillNumberException>(() => bill.BillNumberShort());

        Assert.Equal("Bill number is invalid", longEx.Message);
    }
}
