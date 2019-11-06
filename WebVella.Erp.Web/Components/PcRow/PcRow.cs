using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Web.Components
{
	[PageComponent(Label = "Row", Library = "WebVella", Description = "Layout Grid Row", Version = "0.0.1", IconClass = "fas fa-columns")]
	public class PcRow : PageComponent
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcRow([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcRowOptions
		{

			[JsonProperty(PropertyName = "flex_vertical_alignment")]
			public WvFlexVerticalAlignmentType FlexVerticalAlignment { get; set; } = WvFlexVerticalAlignmentType.None;

			[JsonProperty(PropertyName = "flex_horizontal_alignment")]
			public WvFlexHorizontalAlignmentType FlexHorizontalAlignment { get; set; } = WvFlexHorizontalAlignmentType.None;

			[JsonProperty(PropertyName = "no_gutters")]
			public bool NoGutters { get; set; } = false;

			[JsonProperty(PropertyName = "class")]
			public string Class { get; set; } = "";

			[JsonProperty(PropertyName = "visible_columns")]
			public int VisibleColumns { get; set; } = 2;

			#region << container1 >>
			[JsonProperty(PropertyName = "container1_id")]
			public string Container1Id { get; set; } = "column1";

			[JsonProperty(PropertyName = "container1_span")]
			public int? Container1Span { get; set; } = 0; // 0 will render "col", null will not render the class

			[JsonProperty(PropertyName = "container1_span_sm")]
			public int? Container1SpanSm { get; set; } = null; // 0 will render "col_sm", null will not render the class

			[JsonProperty(PropertyName = "container1_span_md")]
			public int? Container1SpanMd { get; set; } = null; // 0 will render "col_md", null will not render the class

			[JsonProperty(PropertyName = "container1_span_lg")]
			public int? Container1SpanLg { get; set; } = null; // 0 will render "col_lg", null will not render the class

			[JsonProperty(PropertyName = "container1_span_xl")]
			public int? Container1SpanXl { get; set; } = null; // 0 will render "col_xl", null will not render the class

			[JsonProperty(PropertyName = "container1_offset")]
			public int? Container1Offset { get; set; } = null; // null will render the class

			[JsonProperty(PropertyName = "container1_offset_sm")]
			public int? Container1OffsetSm { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container1_offset_md")]
			public int? Container1OffsetMd { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container1_offset_lg")]
			public int? Container1OffsetLg { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container1_offset_xl")]
			public int? Container1OffsetXl { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container1_flex_self_align")]
			public WvFlexSelfAlignType Container1FlexSelftAlign { get; set; } = WvFlexSelfAlignType.None;

			[JsonProperty(PropertyName = "container1_flex_order")]
			public int? Container1FlexOrder { get; set; } = null; // will not render the class
			#endregion

			#region << container2 >>
			[JsonProperty(PropertyName = "container2_id")]
			public string Container2Id { get; set; } = "column2";

			[JsonProperty(PropertyName = "container2_span")]
			public int? Container2Span { get; set; } = 0; // 0 will render "col", null will not render the class

			[JsonProperty(PropertyName = "container2_span_sm")]
			public int? Container2SpanSm { get; set; } = null; // 0 will render "col_sm", null will not render the class

			[JsonProperty(PropertyName = "container2_span_md")]
			public int? Container2SpanMd { get; set; } = null; // 0 will render "col_md", null will not render the class

			[JsonProperty(PropertyName = "container2_span_lg")]
			public int? Container2SpanLg { get; set; } = null; // 0 will render "col_lg", null will not render the class

			[JsonProperty(PropertyName = "container2_span_xl")]
			public int? Container2SpanXl { get; set; } = null; // 0 will render "col_xl", null will not render the class

			[JsonProperty(PropertyName = "container2_offset")]
			public int? Container2Offset { get; set; } = null; // null will render the class

			[JsonProperty(PropertyName = "container2_offset_sm")]
			public int? Container2OffsetSm { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container2_offset_md")]
			public int? Container2OffsetMd { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container2_offset_lg")]
			public int? Container2OffsetLg { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container2_offset_xl")]
			public int? Container2OffsetXl { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container2_flex_self_align")]
			public WvFlexSelfAlignType Container2FlexSelftAlign { get; set; } = WvFlexSelfAlignType.None;

			[JsonProperty(PropertyName = "container2_flex_order")]
			public int? Container2FlexOrder { get; set; } = null; // will not render the class
			#endregion

			#region << container3 >>
			[JsonProperty(PropertyName = "container3_id")]
			public string Container3Id { get; set; } = "column3";

			[JsonProperty(PropertyName = "container3_span")]
			public int? Container3Span { get; set; } = 0; // 0 will render "col", null will not render the class

			[JsonProperty(PropertyName = "container3_span_sm")]
			public int? Container3SpanSm { get; set; } = null; // 0 will render "col_sm", null will not render the class

			[JsonProperty(PropertyName = "container3_span_md")]
			public int? Container3SpanMd { get; set; } = null; // 0 will render "col_md", null will not render the class

			[JsonProperty(PropertyName = "container3_span_lg")]
			public int? Container3SpanLg { get; set; } = null; // 0 will render "col_lg", null will not render the class

			[JsonProperty(PropertyName = "container3_span_xl")]
			public int? Container3SpanXl { get; set; } = null; // 0 will render "col_xl", null will not render the class

			[JsonProperty(PropertyName = "container3_offset")]
			public int? Container3Offset { get; set; } = null; // null will render the class

			[JsonProperty(PropertyName = "container3_offset_sm")]
			public int? Container3OffsetSm { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container3_offset_md")]
			public int? Container3OffsetMd { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container3_offset_lg")]
			public int? Container3OffsetLg { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container3_offset_xl")]
			public int? Container3OffsetXl { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container3_flex_self_align")]
			public WvFlexSelfAlignType Container3FlexSelftAlign { get; set; } = WvFlexSelfAlignType.None;

			[JsonProperty(PropertyName = "container3_flex_order")]
			public int? Container3FlexOrder { get; set; } = null; // will not render the class
			#endregion

			#region << container4 >>
			[JsonProperty(PropertyName = "container4_id")]
			public string Container4Id { get; set; } = "column4";

			[JsonProperty(PropertyName = "container4_span")]
			public int? Container4Span { get; set; } = 0; // 0 will render "col", null will not render the class

			[JsonProperty(PropertyName = "container4_span_sm")]
			public int? Container4SpanSm { get; set; } = null; // 0 will render "col_sm", null will not render the class

			[JsonProperty(PropertyName = "container4_span_md")]
			public int? Container4SpanMd { get; set; } = null; // 0 will render "col_md", null will not render the class

			[JsonProperty(PropertyName = "container4_span_lg")]
			public int? Container4SpanLg { get; set; } = null; // 0 will render "col_lg", null will not render the class

			[JsonProperty(PropertyName = "container4_span_xl")]
			public int? Container4SpanXl { get; set; } = null; // 0 will render "col_xl", null will not render the class

			[JsonProperty(PropertyName = "container4_offset")]
			public int? Container4Offset { get; set; } = null; // null will render the class

			[JsonProperty(PropertyName = "container4_offset_sm")]
			public int? Container4OffsetSm { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container4_offset_md")]
			public int? Container4OffsetMd { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container4_offset_lg")]
			public int? Container4OffsetLg { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container4_offset_xl")]
			public int? Container4OffsetXl { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container4_flex_self_align")]
			public WvFlexSelfAlignType Container4FlexSelftAlign { get; set; } = WvFlexSelfAlignType.None;

			[JsonProperty(PropertyName = "container4_flex_order")]
			public int? Container4FlexOrder { get; set; } = null; // will not render the class
			#endregion

			#region << container5 >>
			[JsonProperty(PropertyName = "container5_id")]
			public string Container5Id { get; set; } = "column5";

			[JsonProperty(PropertyName = "container5_span")]
			public int? Container5Span { get; set; } = 0; // 0 will render "col", null will not render the class

			[JsonProperty(PropertyName = "container5_span_sm")]
			public int? Container5SpanSm { get; set; } = null; // 0 will render "col_sm", null will not render the class

			[JsonProperty(PropertyName = "container5_span_md")]
			public int? Container5SpanMd { get; set; } = null; // 0 will render "col_md", null will not render the class

			[JsonProperty(PropertyName = "container5_span_lg")]
			public int? Container5SpanLg { get; set; } = null; // 0 will render "col_lg", null will not render the class

			[JsonProperty(PropertyName = "container5_span_xl")]
			public int? Container5SpanXl { get; set; } = null; // 0 will render "col_xl", null will not render the class

			[JsonProperty(PropertyName = "container5_offset")]
			public int? Container5Offset { get; set; } = null; // null will render the class

			[JsonProperty(PropertyName = "container5_offset_sm")]
			public int? Container5OffsetSm { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container5_offset_md")]
			public int? Container5OffsetMd { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container5_offset_lg")]
			public int? Container5OffsetLg { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container5_offset_xl")]
			public int? Container5OffsetXl { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container5_flex_self_align")]
			public WvFlexSelfAlignType Container5FlexSelftAlign { get; set; } = WvFlexSelfAlignType.None;

			[JsonProperty(PropertyName = "container5_flex_order")]
			public int? Container5FlexOrder { get; set; } = null; // will not render the class
			#endregion

			#region << container6 >>
			[JsonProperty(PropertyName = "container6_id")]
			public string Container6Id { get; set; } = "column6";

			[JsonProperty(PropertyName = "container6_span")]
			public int? Container6Span { get; set; } = 0; // 0 will render "col", null will not render the class

			[JsonProperty(PropertyName = "container6_span_sm")]
			public int? Container6SpanSm { get; set; } = null; // 0 will render "col_sm", null will not render the class

			[JsonProperty(PropertyName = "container6_span_md")]
			public int? Container6SpanMd { get; set; } = null; // 0 will render "col_md", null will not render the class

			[JsonProperty(PropertyName = "container6_span_lg")]
			public int? Container6SpanLg { get; set; } = null; // 0 will render "col_lg", null will not render the class

			[JsonProperty(PropertyName = "container6_span_xl")]
			public int? Container6SpanXl { get; set; } = null; // 0 will render "col_xl", null will not render the class

			[JsonProperty(PropertyName = "container6_offset")]
			public int? Container6Offset { get; set; } = null; // null will render the class

			[JsonProperty(PropertyName = "container6_offset_sm")]
			public int? Container6OffsetSm { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container6_offset_md")]
			public int? Container6OffsetMd { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container6_offset_lg")]
			public int? Container6OffsetLg { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container6_offset_xl")]
			public int? Container6OffsetXl { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container6_flex_self_align")]
			public WvFlexSelfAlignType Container6FlexSelftAlign { get; set; } = WvFlexSelfAlignType.None;

			[JsonProperty(PropertyName = "container6_flex_order")]
			public int? Container6FlexOrder { get; set; } = null; // will not render the class
			#endregion

			#region << container7 >>
			[JsonProperty(PropertyName = "container7_id")]
			public string Container7Id { get; set; } = "column7";

			[JsonProperty(PropertyName = "container7_span")]
			public int? Container7Span { get; set; } = 0; // 0 will render "col", null will not render the class

			[JsonProperty(PropertyName = "container7_span_sm")]
			public int? Container7SpanSm { get; set; } = null; // 0 will render "col_sm", null will not render the class

			[JsonProperty(PropertyName = "container7_span_md")]
			public int? Container7SpanMd { get; set; } = null; // 0 will render "col_md", null will not render the class

			[JsonProperty(PropertyName = "container7_span_lg")]
			public int? Container7SpanLg { get; set; } = null; // 0 will render "col_lg", null will not render the class

			[JsonProperty(PropertyName = "container7_span_xl")]
			public int? Container7SpanXl { get; set; } = null; // 0 will render "col_xl", null will not render the class

			[JsonProperty(PropertyName = "container7_offset")]
			public int? Container7Offset { get; set; } = null; // null will render the class

			[JsonProperty(PropertyName = "container7_offset_sm")]
			public int? Container7OffsetSm { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container7_offset_md")]
			public int? Container7OffsetMd { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container7_offset_lg")]
			public int? Container7OffsetLg { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container7_offset_xl")]
			public int? Container7OffsetXl { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container7_flex_self_align")]
			public WvFlexSelfAlignType Container7FlexSelftAlign { get; set; } = WvFlexSelfAlignType.None;

			[JsonProperty(PropertyName = "container7_flex_order")]
			public int? Container7FlexOrder { get; set; } = null; // will not render the class
			#endregion

			#region << container8 >>
			[JsonProperty(PropertyName = "container8_id")]
			public string Container8Id { get; set; } = "column8";

			[JsonProperty(PropertyName = "container8_span")]
			public int? Container8Span { get; set; } = 0; // 0 will render "col", null will not render the class

			[JsonProperty(PropertyName = "container8_span_sm")]
			public int? Container8SpanSm { get; set; } = null; // 0 will render "col_sm", null will not render the class

			[JsonProperty(PropertyName = "container8_span_md")]
			public int? Container8SpanMd { get; set; } = null; // 0 will render "col_md", null will not render the class

			[JsonProperty(PropertyName = "container8_span_lg")]
			public int? Container8SpanLg { get; set; } = null; // 0 will render "col_lg", null will not render the class

			[JsonProperty(PropertyName = "container8_span_xl")]
			public int? Container8SpanXl { get; set; } = null; // 0 will render "col_xl", null will not render the class

			[JsonProperty(PropertyName = "container8_offset")]
			public int? Container8Offset { get; set; } = null; // null will render the class

			[JsonProperty(PropertyName = "container8_offset_sm")]
			public int? Container8OffsetSm { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container8_offset_md")]
			public int? Container8OffsetMd { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container8_offset_lg")]
			public int? Container8OffsetLg { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container8_offset_xl")]
			public int? Container8OffsetXl { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container8_flex_self_align")]
			public WvFlexSelfAlignType Container8FlexSelftAlign { get; set; } = WvFlexSelfAlignType.None;

			[JsonProperty(PropertyName = "container8_flex_order")]
			public int? Container8FlexOrder { get; set; } = null; // will not render the class
			#endregion

			#region << container9 >>
			[JsonProperty(PropertyName = "container9_id")]
			public string Container9Id { get; set; } = "column9";

			[JsonProperty(PropertyName = "container9_span")]
			public int? Container9Span { get; set; } = 0; // 0 will render "col", null will not render the class

			[JsonProperty(PropertyName = "container9_span_sm")]
			public int? Container9SpanSm { get; set; } = null; // 0 will render "col_sm", null will not render the class

			[JsonProperty(PropertyName = "container9_span_md")]
			public int? Container9SpanMd { get; set; } = null; // 0 will render "col_md", null will not render the class

			[JsonProperty(PropertyName = "container9_span_lg")]
			public int? Container9SpanLg { get; set; } = null; // 0 will render "col_lg", null will not render the class

			[JsonProperty(PropertyName = "container9_span_xl")]
			public int? Container9SpanXl { get; set; } = null; // 0 will render "col_xl", null will not render the class

			[JsonProperty(PropertyName = "container9_offset")]
			public int? Container9Offset { get; set; } = null; // null will render the class

			[JsonProperty(PropertyName = "container9_offset_sm")]
			public int? Container9OffsetSm { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container9_offset_md")]
			public int? Container9OffsetMd { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container9_offset_lg")]
			public int? Container9OffsetLg { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container9_offset_xl")]
			public int? Container9OffsetXl { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container9_flex_self_align")]
			public WvFlexSelfAlignType Container9FlexSelftAlign { get; set; } = WvFlexSelfAlignType.None;

			[JsonProperty(PropertyName = "container9_flex_order")]
			public int? Container9FlexOrder { get; set; } = null; // will not render the class
			#endregion

			#region << container10 >>
			[JsonProperty(PropertyName = "container10_id")]
			public string Container10Id { get; set; } = "column10";

			[JsonProperty(PropertyName = "container10_span")]
			public int? Container10Span { get; set; } = 0; // 0 will render "col", null will not render the class

			[JsonProperty(PropertyName = "container10_span_sm")]
			public int? Container10SpanSm { get; set; } = null; // 0 will render "col_sm", null will not render the class

			[JsonProperty(PropertyName = "container10_span_md")]
			public int? Container10SpanMd { get; set; } = null; // 0 will render "col_md", null will not render the class

			[JsonProperty(PropertyName = "container10_span_lg")]
			public int? Container10SpanLg { get; set; } = null; // 0 will render "col_lg", null will not render the class

			[JsonProperty(PropertyName = "container10_span_xl")]
			public int? Container10SpanXl { get; set; } = null; // 0 will render "col_xl", null will not render the class

			[JsonProperty(PropertyName = "container10_offset")]
			public int? Container10Offset { get; set; } = null; // null will render the class

			[JsonProperty(PropertyName = "container10_offset_sm")]
			public int? Container10OffsetSm { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container10_offset_md")]
			public int? Container10OffsetMd { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container10_offset_lg")]
			public int? Container10OffsetLg { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container10_offset_xl")]
			public int? Container10OffsetXl { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container10_flex_self_align")]
			public WvFlexSelfAlignType Container10FlexSelftAlign { get; set; } = WvFlexSelfAlignType.None;

			[JsonProperty(PropertyName = "container10_flex_order")]
			public int? Container10FlexOrder { get; set; } = null; // will not render the class
			#endregion

			#region << container11 >>
			[JsonProperty(PropertyName = "container11_id")]
			public string Container11Id { get; set; } = "column11";

			[JsonProperty(PropertyName = "container11_span")]
			public int? Container11Span { get; set; } = 0; // 0 will render "col", null will not render the class

			[JsonProperty(PropertyName = "container11_span_sm")]
			public int? Container11SpanSm { get; set; } = null; // 0 will render "col_sm", null will not render the class

			[JsonProperty(PropertyName = "container11_span_md")]
			public int? Container11SpanMd { get; set; } = null; // 0 will render "col_md", null will not render the class

			[JsonProperty(PropertyName = "container11_span_lg")]
			public int? Container11SpanLg { get; set; } = null; // 0 will render "col_lg", null will not render the class

			[JsonProperty(PropertyName = "container11_span_xl")]
			public int? Container11SpanXl { get; set; } = null; // 0 will render "col_xl", null will not render the class

			[JsonProperty(PropertyName = "container11_offset")]
			public int? Container11Offset { get; set; } = null; // null will render the class

			[JsonProperty(PropertyName = "container11_offset_sm")]
			public int? Container11OffsetSm { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container11_offset_md")]
			public int? Container11OffsetMd { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container11_offset_lg")]
			public int? Container11OffsetLg { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container11_offset_xl")]
			public int? Container11OffsetXl { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container11_flex_self_align")]
			public WvFlexSelfAlignType Container11FlexSelftAlign { get; set; } = WvFlexSelfAlignType.None;

			[JsonProperty(PropertyName = "container11_flex_order")]
			public int? Container11FlexOrder { get; set; } = null; // will not render the class
			#endregion

			#region << container12 >>
			[JsonProperty(PropertyName = "container12_id")]
			public string Container12Id { get; set; } = "column12";

			[JsonProperty(PropertyName = "container12_span")]
			public int? Container12Span { get; set; } = 0; // 0 will render "col", null will not render the class

			[JsonProperty(PropertyName = "container12_span_sm")]
			public int? Container12SpanSm { get; set; } = null; // 0 will render "col_sm", null will not render the class

			[JsonProperty(PropertyName = "container12_span_md")]
			public int? Container12SpanMd { get; set; } = null; // 0 will render "col_md", null will not render the class

			[JsonProperty(PropertyName = "container12_span_lg")]
			public int? Container12SpanLg { get; set; } = null; // 0 will render "col_lg", null will not render the class

			[JsonProperty(PropertyName = "container12_span_xl")]
			public int? Container12SpanXl { get; set; } = null; // 0 will render "col_xl", null will not render the class

			[JsonProperty(PropertyName = "container12_offset")]
			public int? Container12Offset { get; set; } = null; // null will render the class

			[JsonProperty(PropertyName = "container12_offset_sm")]
			public int? Container12OffsetSm { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container12_offset_md")]
			public int? Container12OffsetMd { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container12_offset_lg")]
			public int? Container12OffsetLg { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container12_offset_xl")]
			public int? Container12OffsetXl { get; set; } = null; // 0 or null will render no device specific span

			[JsonProperty(PropertyName = "container12_flex_self_align")]
			public WvFlexSelfAlignType Container12FlexSelftAlign { get; set; } = WvFlexSelfAlignType.None;

			[JsonProperty(PropertyName = "container12_flex_order")]
			public int? Container12FlexOrder { get; set; } = null; // will not render the class
			#endregion

		}

		private PcRowOptions InitOptions(JObject options = null)
		{
			var optionsObject = new PcRowOptions();
			if (options != null)
			{
				optionsObject = JsonConvert.DeserializeObject<PcRowOptions>(options.ToString());
			}
			return optionsObject;
		}

		public async Task<IViewComponentResult> InvokeAsync(PageComponentContext context)
		{
			ErpPage currentPage = null;
			try
			{
				#region << Init >>
				if (context.Node == null)
				{
					return await Task.FromResult<IViewComponentResult>(Content("Error: The node Id is required to be set as query parameter 'nid', when requesting this component"));
				}

				var pageFromModel = context.DataModel.GetProperty("Page");
				if (pageFromModel == null)
				{
					return await Task.FromResult<IViewComponentResult>(Content("Error: PageModel cannot be null"));
				}
				else if (pageFromModel is ErpPage)
				{
					currentPage = (ErpPage)pageFromModel;
				}
				else
				{
					return await Task.FromResult<IViewComponentResult>(Content("Error: PageModel does not have Page property or it is not from ErpPage Type"));
				}


				var optionsObject = InitOptions(context.Options);

				var componentMeta = new PageComponentLibraryService().GetComponentMeta(context.Node.ComponentName);

				#endregion

				ViewBag.Node = context.Node;
				ViewBag.ComponentMeta = componentMeta;
				ViewBag.RequestContext = ErpRequestContext;
				ViewBag.AppContext = ErpAppContext.Current;
				ViewBag.Options = optionsObject;
				ViewBag.ComponentContext = context;

				ViewBag.FlexSelfAlignTypeOptions = WebVella.TagHelpers.Utilities.ModelExtensions.GetEnumAsSelectOptions<WvFlexSelfAlignType>(); 
				ViewBag.FlexVerticalAlignmentOptions = WebVella.TagHelpers.Utilities.ModelExtensions.GetEnumAsSelectOptions<WvFlexVerticalAlignmentType>(); 

				ViewBag.FlexHorizontalAlignmentOptions = WebVella.TagHelpers.Utilities.ModelExtensions.GetEnumAsSelectOptions<WvFlexHorizontalAlignmentType>(); 

				switch (context.Mode)
				{
					case ComponentMode.Display:
						return await Task.FromResult<IViewComponentResult>(View("Display"));
					case ComponentMode.Design:
						return await Task.FromResult<IViewComponentResult>(View("Design"));
					case ComponentMode.Options:
						return await Task.FromResult<IViewComponentResult>(View("Options"));
					case ComponentMode.Help:
						return await Task.FromResult<IViewComponentResult>(View("Help"));
					default:
						ViewBag.Error = new ValidationException()
						{
							Message = "Unknown component mode"
						};
						return await Task.FromResult<IViewComponentResult>(View("Error"));
				}

			}
			catch (ValidationException ex)
			{
				ViewBag.Error = ex;
				return await Task.FromResult<IViewComponentResult>(View("Error"));
			}
			catch (Exception ex)
			{
				ViewBag.Error = new ValidationException()
				{
					Message = ex.Message
				};
				return await Task.FromResult<IViewComponentResult>(View("Error"));
			}
		}
	}
}
