using FluentValidation;
using System.Text.RegularExpressions;

namespace SkyRouteTravel.Application.Bookings;

public class PassengerValidator : AbstractValidator<Passenger>
{
    public PassengerValidator()
    {
        RuleFor(p => p.FullName)
            .NotEmpty().WithMessage("Full Name is required.");

        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.");

        RuleFor(p => p.DocumentType)
            .NotEmpty().WithMessage("Document Type is required.")
            .Must(type => type == "Passport Number" || type == "National ID")
            .WithMessage("Document Type must be either 'Passport Number' or 'National ID'.");

        // Contextual validation: Validate DocumentNumber based on DocumentType
        RuleFor(p => p.DocumentNumber)
            .NotEmpty().WithMessage("Document Number is required.")
            .Custom((docNum, context) =>
            {
                var passenger = context.InstanceToValidate;
                if (passenger.DocumentType == "Passport Number")
                {
                    // Basic Passport Number validation: Alphanumeric, 6 to 15 chars
                    if (!Regex.IsMatch(docNum ?? "", @"^[a-zA-Z0-9]{6,15}$"))
                    {
                        context.AddFailure("DocumentNumber", "Passport number must be between 6 and 15 alphanumeric characters.");
                    }
                }
                else if (passenger.DocumentType == "National ID")
                {
                    // Basic National ID validation: Alphanumeric or hyphens, 8 to 20 chars
                    if (!Regex.IsMatch(docNum ?? "", @"^[a-zA-Z0-9-]{8,20}$"))
                    {
                        context.AddFailure("DocumentNumber", "National ID must be between 8 and 20 alphanumeric characters (hyphens allowed).");
                    }
                }
            });
    }
}
