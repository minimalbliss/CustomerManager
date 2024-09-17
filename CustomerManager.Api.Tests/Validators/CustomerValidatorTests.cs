using CustomerManager.Api.Data;
using CustomerManager.Api.Services.Validators;
using CustomerManager.Models.Models;
using FluentValidation.TestHelper;
using Moq;

namespace CustomerManager.Api.Tests.Validators
{
    public class CustomerValidatorTests
    {
        private CustomerValidator _validator;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<ICustomerRepository> _customerRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(uow => uow.CustomerRepository).Returns(_customerRepositoryMock.Object);
            _validator = new CustomerValidator(_unitOfWorkMock.Object);
        }

        [Test]
        public async Task ShouldHaveErrorWhenNameIsNotProvided()
        {
            // Arrange
            Customer customer = new();

            // Act
            TestValidationResult<Customer> result = await _validator.TestValidateAsync(customer);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Name);
        }

        [Test]
        public async Task ShouldHaveErrorWhenNameExceedsMaxLength()
        {
            // Arrange
            Customer customer = new() { Name = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." };

            // Act
            TestValidationResult<Customer> result = await _validator.TestValidateAsync(customer);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Name);
        }

        [Test]
        public async Task ShouldHaveErrorWhenEmailIsNotProvided()
        {
            // Arrange
            Customer customer = new();

            // Act
            TestValidationResult<Customer> result = await _validator.TestValidateAsync(customer);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Email);
        }

        [Test]
        public async Task ShouldHaveErrorWhenEmailIsInvalid()
        {
            // Arrange
            Customer customer = new() { Email = "invalidemail" };

            // Act
            TestValidationResult<Customer> result = await _validator.TestValidateAsync(customer);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Email);
        }

        [Test]
        public async Task ShouldNotHaveErrorWhenPhoneIsNotProvided()
        {
            // Arrange
            Customer customer = new();

            // Act
            TestValidationResult<Customer> result = await _validator.TestValidateAsync(customer);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.Phone);
        }

        [Test]
        public async Task ShouldHaveErrorWhenPhoneIsInvalid()
        {
            // Arrange
            Customer customer = new() { Phone = "123" };

            // Act
            TestValidationResult<Customer> result = await _validator.TestValidateAsync(customer);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Phone);
        }

        [Test]
        public async Task ShouldNotHaveErrorWhenPostCodeIsNotProvided()
        {
            // Arrange
            Customer customer = new();

            // Act
            TestValidationResult<Customer> result = await _validator.TestValidateAsync(customer);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.PostCode);
        }

        [Test]
        public async Task ShouldHaveErrorWhenPostCodeIsLessThanMinLength()
        {
            // Arrange
            Customer customer = new() { PostCode = "1234" };

            // Act
            TestValidationResult<Customer> result = await _validator.TestValidateAsync(customer);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.PostCode);
        }

        [Test]
        public async Task ShouldNotHaveErrorWhenCountryIsNotProvided()
        {
            // Arrange
            Customer customer = new();

            // Act
            TestValidationResult<Customer> result = await _validator.TestValidateAsync(customer);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.Country);
        }

        [Test]
        public async Task ShouldHaveErrorWhenCountryExceedsMaxLength()
        {
            // Arrange
            Customer customer = new() { Country = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." };

            // Act
            TestValidationResult<Customer> result = await _validator.TestValidateAsync(customer);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Country);
        }
    }
}

