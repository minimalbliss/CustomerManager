using CustomerManager.Api.Data;
using CustomerManager.Models.Models;
using FluentValidation;

namespace CustomerManager.Api.Services.Validators
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.Id)
                .MustAsync(async (id, cancellationToken) =>
                {
                    if (id == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return await _unitOfWork.CustomerRepository.GetByIdAsync(id, cancellationToken) != null;
                    }
                })
                .WithMessage("Customer does not exist.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MinimumLength(3)
                .WithMessage("Name must be at least 3 characters.")
                .MaximumLength(50)
                .WithMessage("Name must not exceed 50 characters.")
                .MustAsync(async (c, name, cancellationToken) =>
                {
                    Customer? existingCustomer = await _unitOfWork.CustomerRepository.GetByNameAsync(name);
                    return existingCustomer is null || existingCustomer.Id == c.Id;
                })
                .WithMessage("Name already exists.");

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Email is not valid.")
                .MaximumLength(150)
                .WithMessage("Email address must not exceed 150 characters.")
                .MustAsync(async (c, email, cancellationToken) =>
                {
                    Customer? existingCustomer = await _unitOfWork.CustomerRepository.GetByEmailAsync(email);
                    return existingCustomer is null || existingCustomer.Id == c.Id;
                })
                .WithMessage("Email already exists.");

            RuleFor(x => x.Phone)
                .Matches(@"^(?:0|\+?44)(?:\d\s?){9,10}$")
                .When(x => !string.IsNullOrEmpty(x.Phone))
                .WithMessage("Phone number is not valid.");

            RuleFor(x => x.PostCode)
                .Matches("^[A-Za-z]{1,2}[0-9A-Za-z]{1,2}[ ]?[0-9]{0,1}[A-Za-z]{2}$")
                .When(x => !string.IsNullOrEmpty(x.PostCode))
                .WithMessage("Postcode is not valid.");

            RuleFor(x => x.Country)
                .Must(country => string.IsNullOrEmpty(country) || country.Length >= 2 && country.Length <= 50)
                .WithMessage("Country must be between 2 and 50 characters if provided.");
        }
    }
}
