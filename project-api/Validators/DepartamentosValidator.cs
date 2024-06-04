using FluentValidation;
using project_api.Models.Dtos;
using System.Text.RegularExpressions;

namespace project_api.Validators
{
    public class DepartamentosValidator : AbstractValidator<DepartamentosDto>
    {
        public DepartamentosValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Debe agregar un Usuario para el departamento");
            RuleFor(x => x.Password)
        .NotEmpty()
        .WithMessage("Debe ingresar una contraseña")
        .MinimumLength(5)
        .WithMessage("La contraseña debe tener al menos 5 caracteres");
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("Debe ingresar un nombre para el departamento");
            RuleFor(x => x.Username).Must(ValidarCorreo).WithMessage("Debe ingresar un correo valido");
        }

        public bool ValidarCorreo(string correo)
        {
            string patron = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]*equipo3[a-zA-Z0-9.-]*\.[a-zA-Z]{2,}$";

            return Regex.IsMatch(correo, patron);
        }
    }
}
