using FluentValidation;
using SnowaTec.Test.Domain.Entities.Customer;
using SnowaTec.Test.Domain.Validation.Field;

namespace SnowaTec.Test.Domain.Validation
{
    public class CustomerValidator : AbstractValidator<Client>
    {
        public CustomerValidator()
        {
            //RuleFor(customer => customer.Firstname).NotNull();
            //RuleFor(customer => customer.Lastname).NotNull();
            //RuleFor(customer => customer.Email).NotNull().EmailAddress();
            //RuleFor(customer => customer.DateOfBirth).NotNull();
            //RuleFor(customer => customer.PhoneNumber).NotNull();
            //RuleFor(customer => customer.PhoneNumber).Must(phone => CheckPhoneNumber.IsValid(phone)).WithMessage("not a valid phone number.");
            //RuleFor(customer => customer.BankAccountNumber).NotNull();
        }
    }
}
