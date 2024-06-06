using FluentValidation;

using project_signalr_api.Models.DTOs.Request;
using project_signalr_api.Repositories;

namespace project_signalr_api.Validators;

public class UpdateTurnoRequestValidator : AbstractValidator<UpdateTurnoRequest>
{
    readonly TurnoRepository turnoRepository;
    readonly CajaRepository cajaRepository;

    public UpdateTurnoRequestValidator(TurnoRepository turnoRepository, CajaRepository cajaRepository)
    {
        this.turnoRepository = turnoRepository;
        this.cajaRepository = cajaRepository;

        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.IdTurno)
            .NotEmpty().WithMessage("El id del turno es requerido")
            .MustAsync(async (idTurno, cancellationToken) =>
            {
                var turno = await turnoRepository.GetById(idTurno);
                return turno is not null;
            })
            .WithMessage("El turno no existe");

        RuleFor(x => x.IdCaja)
            .NotEmpty().WithMessage("El id de la caja es requerido")
            .MustAsync(async (idCaja, cancellationToken) =>
            {
                var caja = await cajaRepository.GetById(idCaja);
                return caja is not null;
            })
            .WithMessage("La caja no existe");

        RuleFor(x => x.Estado)  
            .NotEmpty().WithMessage("El estado es requerido")
            .Must(x => x == "Pendiente" || x == "Atendido" || x == "Atendiendo")
            .WithMessage("El estado debe ser 'Pendiente', 'Atendido' o 'Atendiendo'");
    }
}
