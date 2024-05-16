using FluentValidation;
using project_api.Models.Dtos;
using project_api.Models.Entities;

namespace project_api.Validators
{
    public class ActividadValidator:AbstractValidator<ActividadesDto>
    {
        public ActividadValidator()
        {
            RuleFor(x => x.Titulo).NotEmpty().WithMessage("Debe ingresar un titulo");
            
            RuleFor(x => x.IdDepartamento).NotNull().WithMessage("Debe ingresar un departamento");
        }
    }
}
