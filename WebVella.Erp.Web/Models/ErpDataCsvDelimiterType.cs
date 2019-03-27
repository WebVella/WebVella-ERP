using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public enum ErpDataCsvDelimiterType
    {
        [SelectOption(Label = "comma")]
        COMMA = 0,
        [SelectOption(Label = "tab")]
        TAB = 1,
    }
}
