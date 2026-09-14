using WebVella.Erp.Exceptions;

namespace WebVella.Erp.Api.Models.Mapping;

internal class ErrorModelToValidationErrorStrategy : IMapStrategy<ErrorModel, ValidationError>
{
	public ValidationError Map(ErrorModel source)
	{
		if (source == null)
			return null;

		return new ValidationError(source.Key ?? "id", source.Message);
	}
}
