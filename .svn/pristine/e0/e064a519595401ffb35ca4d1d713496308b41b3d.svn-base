using FluentValidation;

namespace AppApi.DTO.Models.Test
{
    public class TestRequest
    {
        public TestRequest()
        {

        }
        public string TestName { get; set; }
        public string TestCode { get; set; }
    }
    public class TestValidator : AbstractValidator<TestRequest>
    {
        public TestValidator()
        {
            RuleFor(x => x.TestName).NotEmpty().WithMessage("Tên không được để trống").EmailAddress().WithMessage("Test không đúng định dạng");
        }
    }
}