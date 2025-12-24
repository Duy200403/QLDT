using AppApi.DTO.Models.ApiRoleMapping;
using FluentValidation;

namespace AppApi.DTO.Models.MenuItemDto
{
    public class MenuItemRequest
    {
        public MenuItemRequest()
        {

        }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }
        public string ParentId { get; set; }
        public int OrderNumber { get; set; }
        public List<ApiRoleMappingRequest> ApiRoleMappings { get; set; }
    }
    public class MenuItemValidator : AbstractValidator<MenuItemRequest>
    {
        public MenuItemValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Tiêu đề không được để trống");
        }
    }
}