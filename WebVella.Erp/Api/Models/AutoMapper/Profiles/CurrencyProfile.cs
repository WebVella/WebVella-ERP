using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Dynamic;
using System.Linq;
using WebVella.Erp.Jobs;

namespace WebVella.Erp.Api.Models.AutoMapper.Profiles
{
	public class CurrencyProfile : Profile
	{
		public CurrencyProfile()
		{
			CreateMap<Currency, CurrencyType>().ConvertUsing(source => ToCurrencyType(source));
			CreateMap<Currency, SelectOption>().ConvertUsing(source => ToSelectOption(source));
		}

		private static CurrencyType ToCurrencyType(Currency src)
		{
			if (src == null)
				return null;

			var decimalDigits = 2;
			if (src.SubUnitToUnit == 1000) {
				decimalDigits = 3;
			}

			var symbol = src.IsoCode.ToUpperInvariant();
			if (src.AlternateSymbols.Count > 0) {
				symbol = src.AlternateSymbols[0];
			}

			var symPlacement = CurrencySymbolPlacement.After;
			if (src.SymbolFirst) {
				symPlacement = CurrencySymbolPlacement.Before;
			}

			CurrencyType model = new CurrencyType()
			{
				Code = src.IsoCode.ToUpperInvariant(),
				DecimalDigits = decimalDigits,
				Name = src.Name,
				NamePlural = src.Name,
				Rounding = 0,
				SymbolNative = src.Symbol,
				Symbol = symbol,
				SymbolPlacement = symPlacement
			};

			return model;
		}

		private static SelectOption ToSelectOption(Currency src)
		{
			if (src == null)
				return null;

			var model = new SelectOption()
			{
				Value = src.IsoCode.ToUpperInvariant(),
				Label = src.Name
			};

			return model;
		}

	}
}
