using FluentValidation;
using project_signalr_api.Models.DTOs.Request;
using project_signalr_api.Repositories;

namespace project_signalr_api.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    readonly AdministradorRepository administradorRepository;

    public LoginRequestValidator(AdministradorRepository administradorRepository)
    {
        this.administradorRepository = administradorRepository;

        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El nombre de usuario es requerido");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es requerida");

        RuleFor(x => x)
            .MustAsync(async (loginRequest, cancellationToken) =>
            {
                var user = await administradorRepository.Login(loginRequest.Username!, loginRequest.Password!);
                return user != null;
            })
            .WithMessage("Credenciales incorrectas");
    }
}
