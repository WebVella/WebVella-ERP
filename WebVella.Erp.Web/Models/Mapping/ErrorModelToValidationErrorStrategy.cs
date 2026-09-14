using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.Mapping;
using WebVella.Erp.Exceptions;

namespace WebVella.Erp.Web.Models.Mapping;

internal class ErrorModelToValidationErrorStrategy : IMapStrategy<ErrorModel, ValidationError>
{
	public ValidationError Map(ErrorModel data)
	{
		if (data == null)
			return null;

		return new ValidationError(data.Key, data.Message);
	}
}
