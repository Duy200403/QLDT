using AppApi.Entities.Models;
using FluentValidation;

namespace AppApi.DTO.Models.ApiRoleMapping
{
    public class ApiRoleMappingRequest
    {
        public ApiRoleMappingRequest()
        {

        }
        public Guid? Id { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public List<AllowedRole> LstAllowedRoles { get; set; }
    }
    public class ApiRoleMappingValidator : AbstractValidator<ApiRoleMappingRequest>
    {
        public ApiRoleMappingValidator()
        {
            RuleFor(x => x.Controller).NotEmpty().WithMessage("Controller không được để trống");
        }
    }
}