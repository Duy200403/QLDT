using FluentValidation;

namespace AppApi.DTO.Models.RoleDto
{
    public class RoleRequest
    {
        public RoleRequest()
        {

        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParentId { get; set; }
        // public string RoleCode { get; set; }

        public int OrderNumber { get; set; }
    }
    public class RoleValidator : AbstractValidator<RoleRequest>
    {
        public RoleValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không được để trống");
        }
    }

    public class RoleTreeRequest : RoleRequest
    {
        public Guid Id { get; set; }
        public List<RoleTreeRequest> Children { get; set; }
    }

}