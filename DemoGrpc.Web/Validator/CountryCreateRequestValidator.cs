using DemoGrpc.Protobufs;
using DemoGrpc.Protobufs.V1;
using FluentValidation;

namespace DemoGrpc.Web.Validator
{
    public class CountryCreateRequestValidator : AbstractValidator<CountryCreateRequest>
    {
        public CountryCreateRequestValidator()
        {
            RuleFor(request => request.Name).NotEmpty().WithMessage("Name is mandatory.");
            RuleFor(request => request.Description).MinimumLength(5).WithMessage("Description is mandatory and be longer than 5 characters");
        }
    }
}