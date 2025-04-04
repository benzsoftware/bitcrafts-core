using System.Globalization;
using BitCrafts.Infrastructure.Abstraction.Extensions;

namespace BitCrafts.Infrastructure.Tests.Extensions;

[TestClass]
public class StringExtensionTests
{
    [TestMethod]
    public void ParseOrGetDefault_Int_ValidInput_ReturnsParsedValue()
    {
        // Arrange
        string input = "123";

        // Act
        int result = input.ParseIntOrGetDefault();

        // Assert
        Assert.AreEqual(123, result);
    }

    [TestMethod]
    public void ParseOrGetDefault_Int_InvalidInput_ReturnsDefaultValue()
    {
        // Arrange
        string input = "abc";

        // Act
        int result = input.ParseIntOrGetDefault(42);

        // Assert
        Assert.AreEqual(42, result);
    }

    [TestMethod]
    public void ParseOrGetDefault_Int_NullInput_ReturnsDefaultValue()
    {
        // Arrange
        string input = null;

        // Act
        int result = input.ParseIntOrGetDefault(42);

        // Assert
        Assert.AreEqual(42, result);
    }

    [TestMethod]
    public void ParseOrGetDefault_Int_WithCulture_ParsesCorrectly()
    {
        // Arrange
        string input = "1,234";
        var culture = new CultureInfo("en-US");

        // Act
        int result = input.ParseIntOrGetDefault(0, NumberStyles.AllowThousands, culture);

        // Assert
        Assert.AreEqual(1234, result);
    }

    [TestMethod]
    public void ParseOrGetDefault_Double_ValidInput_ReturnsParsedValue()
    {
        // Arrange
        string input = "123.45";
        var culture = new CultureInfo("en-US");

        // Act
        double result = input.ParseDoubleOrGetDefault(0.0, NumberStyles.Float, culture);

        // Assert
        Assert.AreEqual(123.45, result);
    }

    [TestMethod]
    public void ParseOrGetDefault_Double_InvalidInput_ReturnsDefaultValue()
    {
        // Arrange
        string input = "abc";

        // Act
        double result = input.ParseDoubleOrGetDefault(42.5);

        // Assert
        Assert.AreEqual(42.5, result);
    }

    [TestMethod]
    public void ParseOrGetDefault_Double_NullInput_ReturnsDefaultValue()
    {
        // Arrange
        string input = null;

        // Act
        double result = input.ParseDoubleOrGetDefault(42.5);

        // Assert
        Assert.AreEqual(42.5, result);
    }

    [TestMethod]
    public void ParseOrGetDefault_Decimal_ValidInput_ReturnsParsedValue()
    {
        // Arrange
        string input = "123.45";
        var culture = new CultureInfo("en-US");

        // Act
        decimal result = input.ParseDecimalOrGetDefault(0.0m, NumberStyles.Number, culture);

        // Assert
        Assert.AreEqual(123.45m, result);
    }

    [TestMethod]
    public void ParseOrGetDefault_Decimal_InvalidInput_ReturnsDefaultValue()
    {
        // Arrange
        string input = "abc";

        // Act
        decimal result = input.ParseDecimalOrGetDefault(42.5m);

        // Assert
        Assert.AreEqual(42.5m, result);
    }

    [TestMethod]
    public void ParseOrGetDefault_Decimal_NullInput_ReturnsDefaultValue()
    {
        // Arrange
        string input = null;

        // Act
        decimal result = input.ParseDecimalOrGetDefault(42.5m);

        // Assert
        Assert.AreEqual(42.5m, result);
    }

    [TestMethod]
    public void ToBoolOrDefault_ValidTrueInput_ReturnsTrue()
    {
        // Arrange
        string input = "true";

        // Act
        bool result = input.ToBoolOrDefault();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void ToBoolOrDefault_ValidFalseInput_ReturnsFalse()
    {
        // Arrange
        string input = "false";

        // Act
        bool result = input.ToBoolOrDefault(true);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void ToBoolOrDefault_InvalidInput_ReturnsDefaultValue()
    {
        // Arrange
        string input = "notbool";

        // Act
        bool result = input.ToBoolOrDefault(true);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void ToBoolOrDefault_NullInput_ReturnsDefaultValue()
    {
        // Arrange
        string input = null;

        // Act
        bool result = input.ToBoolOrDefault(true);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void TrimOrEmpty_NormalString_ReturnsTrimmedString()
    {
        // Arrange
        string input = "  hello  ";

        // Act
        string result = input.TrimOrEmpty();

        // Assert
        Assert.AreEqual("hello", result);
    }

    [TestMethod]
    public void TrimOrEmpty_NullString_ReturnsEmptyString()
    {
        // Arrange
        string input = null;

        // Act
        string result = input.TrimOrEmpty();

        // Assert
        Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    public void ExtractDigits_StringWithDigits_ReturnsOnlyDigits()
    {
        // Arrange
        string input = "abc123def456";

        // Act
        string result = input.ExtractDigits();

        // Assert
        Assert.AreEqual("123456", result);
    }

    [TestMethod]
    public void ExtractDigits_StringWithoutDigits_ReturnsEmptyString()
    {
        // Arrange
        string input = "abcdef";

        // Act
        string result = input.ExtractDigits();

        // Assert
        Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    public void ExtractDigits_NullString_ReturnsEmptyString()
    {
        // Arrange
        string input = null;

        // Act & Assert
        Assert.ThrowsException<NullReferenceException>(() => input.ExtractDigits());
    }

    [TestMethod]
    public void ToDateTimeOrDefault_ValidFormat_ReturnsParsedDateTime()
    {
        // Arrange
        string input = "2023-04-15";
        DateTime defaultValue = new DateTime(2000, 1, 1);

        // Act
        DateTime result = input.ToDateTimeOrDefault(defaultValue);

        // Assert
        Assert.AreEqual(new DateTime(2023, 4, 15), result);
    }

    [TestMethod]
    public void ToDateTimeOrDefault_InvalidFormat_ReturnsDefaultValue()
    {
        // Arrange
        string input = "15/04/2023";
        DateTime defaultValue = new DateTime(2000, 1, 1);

        // Act
        DateTime result = input.ToDateTimeOrDefault(defaultValue);

        // Assert
        Assert.AreEqual(defaultValue, result);
    }

    [TestMethod]
    public void ToDateTimeOrDefault_CustomFormat_ReturnsParsedDateTime()
    {
        // Arrange
        string input = "15/04/2023";
        DateTime defaultValue = new DateTime(2000, 1, 1);

        // Act
        DateTime result = input.ToDateTimeOrDefault(defaultValue, "dd/MM/yyyy");

        // Assert
        Assert.AreEqual(new DateTime(2023, 4, 15), result);
    }

    [TestMethod]
    public void ToDateTimeOrDefault_NullInput_ReturnsDefaultValue()
    {
        // Arrange
        string input = null;
        DateTime defaultValue = new DateTime(2000, 1, 1);

        // Act
        DateTime result = input.ToDateTimeOrDefault(defaultValue);

        // Assert
        Assert.AreEqual(defaultValue, result);
    }

    [TestMethod]
    public void IsNullOrWhiteSpace_NullString_ReturnsTrue()
    {
        // Arrange
        string input = null;

        // Act
        bool result = input.IsNullOrWhiteSpace();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsNullOrWhiteSpace_EmptyString_ReturnsTrue()
    {
        // Arrange
        string input = "";

        // Act
        bool result = input.IsNullOrWhiteSpace();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsNullOrWhiteSpace_WhiteSpaceString_ReturnsTrue()
    {
        // Arrange
        string input = "   ";

        // Act
        bool result = input.IsNullOrWhiteSpace();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsNullOrWhiteSpace_NormalString_ReturnsFalse()
    {
        // Arrange
        string input = "hello";

        // Act
        bool result = input.IsNullOrWhiteSpace();

        // Assert
        Assert.IsFalse(result);
    }
}