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
            RuleFor(request => request.Description).NotEmpty().WithMessage("Description is mandatory.");
        }
    }
}