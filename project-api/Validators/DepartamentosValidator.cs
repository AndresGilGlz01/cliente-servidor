using FluentValidation;
using project_api.Models.Dtos;

namespace project_api.Validators
{
    public class DepartamentosValidator:AbstractValidator<DepartamentosDto>
    {
        public DepartamentosValidator()
        {
            RuleFor(x=>x.Username).NotEmpty().WithMessage("Debe agregar un Usuario para el departamento");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Debe ingresar una contraña");
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("Debe ingresar un nombre para el departamento");
            
        }
    }
}
