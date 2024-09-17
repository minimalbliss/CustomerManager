using CustomerManager.Api.Data;
using CustomerManager.Api.Services.Interfaces;
using CustomerManager.Models.Models;
using FluentValidation;
using FluentValidation.Results;
using LanguageExt.Common;

namespace CustomerManager.Api.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CustomerService> _logger;
        private readonly IValidator<Customer> _customerValidator;

        public CustomerService(
            IUnitOfWork unitOfWork,
            IValidator<Customer> customerValidator,
            ILoggerFactory loggerFactory)
        {
            _unitOfWork = unitOfWork;
            _customerValidator = customerValidator;
            _logger = loggerFactory.CreateLogger<CustomerService>();
        }

        public async Task<Result<bool>> AddAsync(Customer item, CancellationToken cancellationToken)
        {
            try
            {
                ValidationResult validationResult = await _customerValidator.ValidateAsync(item, cancellationToken);
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Validation failed while adding customer");
                    return new Result<bool>(new ValidationException(validationResult.Errors));

                }
                if (await _unitOfWork.CustomerRepository.AddAsync(item, cancellationToken))
                {
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    return new Result<bool>(true);
                }
                return new Result<bool>(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding customer");
                throw;
            }
        }

        public async Task<Customer?> FindAsync(Customer item, CancellationToken cancellationToken)
        {
            try
            {
                return await _unitOfWork.CustomerRepository.FindAsync(item, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while finding customer");
                throw;
            }
        }

        public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _unitOfWork.CustomerRepository.GetAllAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all customers");
                throw;
            }
        }

        public async Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _unitOfWork.CustomerRepository.GetByIdAsync(id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting customer by id");
                throw;
            }
        }

        public async Task<Result<bool>> UpdateAsync(Customer item, CancellationToken cancellationToken)
        {
            try
            {
                ValidationResult validationResult = await _customerValidator.ValidateAsync(item);
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Validation failed while updating customer");
                    return new Result<bool>(new ValidationException(validationResult.Errors));
                }

                await _unitOfWork.CustomerRepository.Update(item, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating customer");
                throw;
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                Customer? customer = await _unitOfWork.CustomerRepository.GetByIdAsync(id, cancellationToken);
                if (customer is null)
                {
                    _logger.LogWarning($"Validation failed deleting a customer. Customer id: {id} not found");
                    ValidationResult validationResult = new(new List<ValidationFailure>
                    {
                        new ("Id", "Customer not found")
                    });
                    return new Result<bool>(new ValidationException(validationResult.Errors));
                }

                await _unitOfWork.CustomerRepository.Delete(customer, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting customer");
                throw;
            }
        }
    }
}
