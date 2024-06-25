using FluentValidation;

using project_signalr_api.Models.DTOs.Request;

namespace project_signalr_api.Validators;

public class CreateUsuarioRequestValidator : AbstractValidator<CreateUsuarioRequest>
{
    public CreateUsuarioRequestValidator()
    {
        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido");

        RuleFor(x => x.Contrasena)
            .NotEmpty().WithMessage("La contraseña es requerida");

        RuleFor(x => x.ConfirmarContrasena)
            .NotEmpty().WithMessage("La confirmación de la contraseña es requerida")
            .Equal(x => x.Contrasena).WithMessage("Las contraseñas no coinciden");
    }
}
