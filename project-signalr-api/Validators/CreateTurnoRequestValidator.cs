using FluentValidation;

using project_signalr_api.Models.DTOs.Request;
using project_signalr_api.Repositories;

namespace project_signalr_api.Validators;

public class CreateTurnoRequestValidator : AbstractValidator<CreateTurnoRequest>
{
    readonly TurnoRepository turnoRepository;

    public CreateTurnoRequestValidator(TurnoRepository turnoRepository)
    {
        this.turnoRepository = turnoRepository;

        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Folio)
            .NotEmpty().WithMessage("El folio es requerido");

        RuleFor(x => x.Folio)
            .MustAsync(async (x, cancellationToken) =>
            {
                var turno = await turnoRepository.GetByFolio(x);
                return turno is null;
            }).WithMessage("El folio ya existe");
    }
}
