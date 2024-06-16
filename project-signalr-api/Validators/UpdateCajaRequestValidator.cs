using FluentValidation;

using project_signalr_api.Models.DTOs.Request;
using project_signalr_api.Repositories;

namespace project_signalr_api.Validators;

public class UpdateCajaRequestValidator : AbstractValidator<UpdateCajaRequest>
{
    readonly CajaRepository repository;

    public UpdateCajaRequestValidator(CajaRepository repository)
    {
        this.repository = repository;

        RuleFor(request => request.IdCaja)
            .MustAsync((id, token) => repository.Exists(id))
            .WithMessage("La caja no existe.");
    }
}
