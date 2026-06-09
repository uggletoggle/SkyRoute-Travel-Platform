using FluentAssertions;
using FluentValidation.TestHelper;
using SkyRouteTravel.Application.Bookings;

namespace SkyRouteTravel.Tests.Bookings;

public class PassengerValidatorTests
{
    private readonly PassengerValidator _sut = new();

    // --- FullName ---

    [Fact]
    public void FullName_WhenEmpty_ShouldHaveValidationError()
    {
        var passenger = ValidPassenger();
        passenger.FullName = string.Empty;

        var result = _sut.TestValidate(passenger);

        result.ShouldHaveValidationErrorFor(p => p.FullName)
            .WithErrorMessage("Full Name is required.");
    }

    [Fact]
    public void FullName_WhenProvided_ShouldNotHaveValidationError()
    {
        var result = _sut.TestValidate(ValidPassenger());

        result.ShouldNotHaveValidationErrorFor(p => p.FullName);
    }

    // --- Email ---

    [Fact]
    public void Email_WhenEmpty_ShouldHaveValidationError()
    {
        var passenger = ValidPassenger();
        passenger.Email = string.Empty;

        var result = _sut.TestValidate(passenger);

        result.ShouldHaveValidationErrorFor(p => p.Email)
            .WithErrorMessage("Email is required.");
    }

    [Theory]
    [InlineData("notanemail")]
    [InlineData("missing@")]
    [InlineData("@nodomain.com")]
    public void Email_WhenInvalidFormat_ShouldHaveValidationError(string email)
    {
        var passenger = ValidPassenger();
        passenger.Email = email;

        var result = _sut.TestValidate(passenger);

        result.ShouldHaveValidationErrorFor(p => p.Email)
            .WithErrorMessage("A valid email is required.");
    }

    [Fact]
    public void Email_WhenValid_ShouldNotHaveValidationError()
    {
        var result = _sut.TestValidate(ValidPassenger());

        result.ShouldNotHaveValidationErrorFor(p => p.Email);
    }

    // --- DocumentType ---

    [Fact]
    public void DocumentType_WhenEmpty_ShouldHaveValidationError()
    {
        var passenger = ValidPassenger();
        passenger.DocumentType = string.Empty;

        var result = _sut.TestValidate(passenger);

        result.ShouldHaveValidationErrorFor(p => p.DocumentType)
            .WithErrorMessage("Document Type is required.");
    }

    [Theory]
    [InlineData("Drivers License")]
    [InlineData("ID Card")]
    [InlineData("passport")]
    public void DocumentType_WhenNotAllowedValue_ShouldHaveValidationError(string docType)
    {
        var passenger = ValidPassenger();
        passenger.DocumentType = docType;

        var result = _sut.TestValidate(passenger);

        result.ShouldHaveValidationErrorFor(p => p.DocumentType)
            .WithErrorMessage("Document Type must be either 'Passport Number' or 'National ID'.");
    }

    [Theory]
    [InlineData("Passport Number")]
    [InlineData("National ID")]
    public void DocumentType_WhenAllowedValue_ShouldNotHaveValidationError(string docType)
    {
        var passenger = ValidPassenger();
        passenger.DocumentType = docType;
        passenger.DocumentNumber = "AB123456";

        var result = _sut.TestValidate(passenger);

        result.ShouldNotHaveValidationErrorFor(p => p.DocumentType);
    }

    // --- DocumentNumber (Passport Number) ---

    [Theory]
    [InlineData("AB12")] // too short
    [InlineData("AB1234567890123456")] // too long
    [InlineData("AB-1234")] // invalid char
    public void DocumentNumber_WithPassportType_WhenInvalid_ShouldHaveValidationError(string docNumber)
    {
        var passenger = ValidPassenger();
        passenger.DocumentType = "Passport Number";
        passenger.DocumentNumber = docNumber;

        var result = _sut.TestValidate(passenger);

        result.ShouldHaveValidationErrorFor(p => p.DocumentNumber)
            .WithErrorMessage("Passport number must be between 6 and 15 alphanumeric characters.");
    }

    [Theory]
    [InlineData("AB1234")]        // 6 chars
    [InlineData("AB12345678")]    // 10 chars
    [InlineData("AB12345678901")] // 13 chars (edge)
    public void DocumentNumber_WithPassportType_WhenValid_ShouldNotHaveValidationError(string docNumber)
    {
        var passenger = ValidPassenger();
        passenger.DocumentType = "Passport Number";
        passenger.DocumentNumber = docNumber;

        var result = _sut.TestValidate(passenger);

        result.ShouldNotHaveValidationErrorFor(p => p.DocumentNumber);
    }

    // --- DocumentNumber (National ID) ---

    [Theory]
    [InlineData("AB1234")] // too short
    [InlineData("AB1234567890123456789")] // too long
    [InlineData("AB 1234567")] // space not allowed
    public void DocumentNumber_WithNationalIdType_WhenInvalid_ShouldHaveValidationError(string docNumber)
    {
        var passenger = ValidPassenger();
        passenger.DocumentType = "National ID";
        passenger.DocumentNumber = docNumber;

        var result = _sut.TestValidate(passenger);

        result.ShouldHaveValidationErrorFor(p => p.DocumentNumber)
            .WithErrorMessage("National ID must be between 8 and 20 alphanumeric characters (hyphens allowed).");
    }

    [Theory]
    [InlineData("AB123456")]     // 8 chars
    [InlineData("AB-12345678")] // with hyphen
    [InlineData("AB1234567890")] // 12 chars
    public void DocumentNumber_WithNationalIdType_WhenValid_ShouldNotHaveValidationError(string docNumber)
    {
        var passenger = ValidPassenger();
        passenger.DocumentType = "National ID";
        passenger.DocumentNumber = docNumber;

        var result = _sut.TestValidate(passenger);

        result.ShouldNotHaveValidationErrorFor(p => p.DocumentNumber);
    }

    // --- Helpers ---

    private static Passenger ValidPassenger() => new()
    {
        FullName = "Jane Doe",
        Email = "jane@example.com",
        DocumentType = "Passport Number",
        DocumentNumber = "AB123456"
    };
}
