using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using Yahoo.Yui.Compressor;

namespace WebVella.Erp.Web.TagHelpers
{
    [HtmlTargetElement("wv-field-data-csv")]
    [RestrictChildren("wv-field-prepend", "wv-field-append")]
    public class WvFieldDataCsv : WvFieldBase
    {

        [HtmlAttributeName("height")]
        public string Height { get; set; } = "140px";

        [HtmlAttributeName("delimiter-field-name")]
        public string DelimiterFieldName { get; set; } = "";

        [HtmlAttributeName("delimiter-value")]
        public ErpDataCsvDelimiterType DelimiterValue { get; set; } = ErpDataCsvDelimiterType.COMMA;

        [HtmlAttributeName("has-header-field-name")]
        public string HasHeaderFieldName { get; set; } = "";

        [HtmlAttributeName("has-header-value")]
        public bool HasHeaderValue { get; set; } = true;

        [HtmlAttributeName("lang")]
        public string Lang { get; set; } = "en";


        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (!isVisible)
            {
                output.SuppressOutput();
                return;
            }
            #region << Init >>
            var initSuccess = InitField(context, output);

            if (!initSuccess)
            {
                return;
            }

            if (String.IsNullOrWhiteSpace(DelimiterFieldName))
            {
                DelimiterFieldName = Name + "_delimiter";
            }

            if (String.IsNullOrWhiteSpace(HasHeaderFieldName))
            {
                HasHeaderFieldName = Name + "_has_header";
            }

            #endregion

            #region << Render >>
            if (Mode == FieldRenderMode.Form)
            {
                //>> Tab
                var tabEl = new TagBuilder("div");
                tabEl.AddCssClass("card tabs");
                tabEl.Attributes.Add("id", $"card-{FieldId}");

                //>> Tab > Header
                var tabHeaderEl = new TagBuilder("div");
                tabHeaderEl.AddCssClass("card-header");

                //>> Tab > Header > Ul
                var tabHeaderUlEl = new TagBuilder("ul");
                tabHeaderUlEl.AddCssClass("nav nav-tabs");

                //>> Tab > Header > Ul > LiData
                var tabHeaderUlLiDataEl = new TagBuilder("li");
                tabHeaderUlLiDataEl.AddCssClass("nav-item");
                var textCsv = "CSV text";
                if (Lang == "bg")
                {
                    textCsv = "CSV текст";
                }
                tabHeaderUlLiDataEl.InnerHtml.AppendHtml($"<a class=\"nav-link active\" href=\"#\" data-tab-id=\"csv\">{textCsv}</a>");
                tabHeaderUlEl.InnerHtml.AppendHtml(tabHeaderUlLiDataEl);

                //>> Tab > Header > Ul > LiPreview
                var tabHeaderUlLiPreviewEl = new TagBuilder("li");
                tabHeaderUlLiPreviewEl.AddCssClass("nav-item");
                var textPreview = "Table Preview";
                if (Lang == "bg")
                {
                    textPreview = "Табличен Преглед";
                }
                tabHeaderUlLiPreviewEl.InnerHtml.AppendHtml($"<a class=\"nav-link\" href=\"#\" data-tab-id=\"preview\">{textPreview}</a>");
                tabHeaderUlEl.InnerHtml.AppendHtml(tabHeaderUlLiPreviewEl);

                //<< Tab > Header > Ul
                tabHeaderEl.InnerHtml.AppendHtml(tabHeaderUlEl);

                //<< Tab > Header
                tabEl.InnerHtml.AppendHtml(tabHeaderEl);


                //>> Tab > Body
                var tabBodyEl = new TagBuilder("div");
                tabBodyEl.AddCssClass("card-body p-2");


                //>> Tab > Body > TabContent
                var tabBodyTabContentEl = new TagBuilder("div");
                tabBodyTabContentEl.AddCssClass("tab-content");


                ///////////////////////////
                /// CSV
                /////////////////////////////////////////////////

                //>> Tab > Body > TabContent >> CsvPane
                var tabBodyTabContentCsvPaneEl = new TagBuilder("div");
                tabBodyTabContentCsvPaneEl.AddCssClass("tab-pane active");
                tabBodyTabContentCsvPaneEl.Attributes.Add("id", $"{FieldId}-tab-csv");

                //Control
                var inputEl = new TagBuilder("textarea");
                var inputElCssClassList = new List<string>();
                inputElCssClassList.Add("form-control erp-data-csv");
                if (!String.IsNullOrWhiteSpace(Height))
                {
                    inputEl.Attributes.Add("style", $"height:{Height};");
                }
                else
                {
                    inputEl.Attributes.Add("style", $"height:140px;");
                }
                inputEl.InnerHtml.AppendHtml((Value ?? "").ToString());
                inputEl.Attributes.Add("id", $"textarea-{FieldId}");
                inputEl.Attributes.Add("name", Name);

                if (Access == FieldAccess.Full || Access == FieldAccess.FullAndCreate)
                {
                    if (Required)
                    {
                        inputEl.Attributes.Add("required", null);
                    }
                    if (!String.IsNullOrWhiteSpace(Placeholder))
                    {
                        inputEl.Attributes.Add("placeholder", Placeholder);
                    }
                }
                else if (Access == FieldAccess.ReadOnly)
                {
                    inputEl.Attributes.Add("readonly", null);
                }

                if (ValidationErrors.Count > 0)
                {
                    inputElCssClassList.Add("is-invalid");
                }

                inputEl.Attributes.Add("class", String.Join(' ', inputElCssClassList));
                tabBodyTabContentCsvPaneEl.InnerHtml.AppendHtml(inputEl);

                var auxInputWrapper = new TagBuilder("div");
                auxInputWrapper.AddCssClass("mt-2 d-flex align-items-center");

                //hasHeader
                var hasHeaderWrapper = new TagBuilder("div");
                hasHeaderWrapper.AddCssClass("form-check form-check-inline mr-5");

                var hasHeaderCheck = new TagBuilder("input");
                hasHeaderCheck.Attributes.Add("name", HasHeaderFieldName);
                hasHeaderCheck.Attributes.Add("type", "hidden");
                if (HasHeaderValue)
                {
                    hasHeaderCheck.Attributes.Add("value", "true");
                }
                else
                {
                    hasHeaderCheck.Attributes.Add("value", "false");
                }
                hasHeaderWrapper.InnerHtml.AppendHtml(hasHeaderCheck);

                var inputFake = new TagBuilder("input");
                inputFake.AddCssClass("form-check-input");
                inputFake.Attributes.Add("id", $"input-hasheader-fake-{FieldId}");
                inputFake.Attributes.Add("type", "checkbox");
                inputFake.Attributes.Add("value", "true");
                inputFake.Attributes.Add("data-field-name", HasHeaderFieldName);
                if (HasHeaderValue)
                {
                    inputFake.Attributes.Add("checked", "checked");
                }
                hasHeaderWrapper.InnerHtml.AppendHtml(inputFake);

                var labelWrapper = new TagBuilder("label");
                labelWrapper.AddCssClass("form-check-label");
                labelWrapper.Attributes.Add("for", "input-hasheader-fake-" + FieldId);
                var textHeadHeader = "data has header row";
                if (Lang == "bg")
                {
                    textHeadHeader = "данните включват заглавен ред";
                }
                labelWrapper.InnerHtml.AppendHtml(textHeadHeader);

                hasHeaderWrapper.InnerHtml.AppendHtml(labelWrapper);
                auxInputWrapper.InnerHtml.AppendHtml(hasHeaderWrapper);


                //Delimiter > Label
                var delimiterLabel = new TagBuilder("div");
                delimiterLabel.AddCssClass("form-check form-check-inline");
                var delimiterLabelText = "Delimiter";
                if (Lang == "bg")
                {
                    delimiterLabelText = "Разделител";
                }
                delimiterLabel.InnerHtml.AppendHtml($"<label class='go-gray form-check-label'>{delimiterLabelText}: </label>");
                auxInputWrapper.InnerHtml.AppendHtml(delimiterLabel);


                //Delimiter > Comma
                var delimiterComma = new TagBuilder("div");
                delimiterComma.AddCssClass("form-check form-check-inline");

                var delimiterCommaInputEl = new TagBuilder("input");
                delimiterCommaInputEl.AddCssClass("form-check-input");
                delimiterCommaInputEl.Attributes.Add("type", "radio");
                delimiterCommaInputEl.Attributes.Add("name", DelimiterFieldName);
                delimiterCommaInputEl.Attributes.Add("id", $"{FieldId}-delimiter-comma");
                delimiterCommaInputEl.Attributes.Add("value", "comma");
                if (DelimiterValue == ErpDataCsvDelimiterType.COMMA)
                {
                    delimiterCommaInputEl.Attributes.Add("checked", "checked");
                }
                delimiterComma.InnerHtml.AppendHtml(delimiterCommaInputEl);

                var delimiterCommaLabelEl = new TagBuilder("label");
                delimiterCommaLabelEl.AddCssClass("form-check-label");
                delimiterCommaLabelEl.Attributes.Add("for", $"{FieldId}-delimiter-comma");
                var textComma = "comma";
                if (Lang == "bg")
                {
                    textComma = "запетая";
                }
                delimiterCommaLabelEl.InnerHtml.Append(textComma);

                delimiterComma.InnerHtml.AppendHtml(delimiterCommaLabelEl);

                auxInputWrapper.InnerHtml.AppendHtml(delimiterComma);

                //Delimiter > Tab
                var delimiterTab = new TagBuilder("div");
                delimiterTab.AddCssClass("form-check form-check-inline");

                var delimiterTabInputEl = new TagBuilder("input");
                delimiterTabInputEl.AddCssClass("form-check-input");
                delimiterTabInputEl.Attributes.Add("type", "radio");
                delimiterTabInputEl.Attributes.Add("name", DelimiterFieldName);
                delimiterTabInputEl.Attributes.Add("id", $"{FieldId}-delimiter-tab");
                delimiterTabInputEl.Attributes.Add("value", "tab");
                if (DelimiterValue == ErpDataCsvDelimiterType.TAB)
                {
                    delimiterTabInputEl.Attributes.Add("checked", "checked");
                }
                delimiterTab.InnerHtml.AppendHtml(delimiterTabInputEl);

                var delimiterTabLabelEl = new TagBuilder("label");
                delimiterTabLabelEl.AddCssClass("form-check-label");
                delimiterTabLabelEl.Attributes.Add("for", $"{FieldId}-delimiter-tab");
                var textTab = "tab";
                if (Lang == "bg")
                {
                    textTab = "табулация";
                }
                delimiterTabLabelEl.InnerHtml.Append(textTab);
                delimiterTab.InnerHtml.AppendHtml(delimiterTabLabelEl);

                auxInputWrapper.InnerHtml.AppendHtml(delimiterTab);

                tabBodyTabContentCsvPaneEl.InnerHtml.AppendHtml(auxInputWrapper);


                //<< Tab > Body > TabContent >> CsvPane
                tabBodyTabContentEl.InnerHtml.AppendHtml(tabBodyTabContentCsvPaneEl);

                ///////////////////////////
                /// PREVIEW
                /////////////////////////////////////////////////

                //>> Tab > Body > TabContent >> PreviewPane
                var tabBodyTabContentPreviewPaneEl = new TagBuilder("div");
                tabBodyTabContentPreviewPaneEl.AddCssClass("tab-pane");
                tabBodyTabContentPreviewPaneEl.Attributes.Add("id", $"{FieldId}-tab-preview");

                //>> Tab > Body > TabContent >> PreviewPane >> DoubleScroll
                var tabBodyTabContentPreviewPaneDoublescrollEl = new TagBuilder("div");
                tabBodyTabContentPreviewPaneDoublescrollEl.AddCssClass("doublescroll");

                //>> Tab > Body > TabContent >> PreviewPane >> DoubleScroll >> Wrapper 1
                var tabBodyTabContentPreviewPaneDoublescrollWrapper1El = new TagBuilder("div");
                tabBodyTabContentPreviewPaneDoublescrollWrapper1El.AddCssClass("doublescroll-wrapper1");
                tabBodyTabContentPreviewPaneDoublescrollWrapper1El.InnerHtml.AppendHtml("<div class='doublescroll-inner1'></div>");
                tabBodyTabContentPreviewPaneDoublescrollEl.InnerHtml.AppendHtml(tabBodyTabContentPreviewPaneDoublescrollWrapper1El);

                //>> Tab > Body > TabContent >> PreviewPane >> DoubleScroll >> Wrapper 2
                var tabBodyTabContentPreviewPaneDoublescrollWrapper2El = new TagBuilder("div");
                tabBodyTabContentPreviewPaneDoublescrollWrapper2El.AddCssClass("doublescroll-wrapper2");
                tabBodyTabContentPreviewPaneDoublescrollWrapper2El.InnerHtml.AppendHtml("<div class='doublescroll-inner2'><div class='preview'><div class='loading-pane'><i class='fa fa-spin fa-spinner'></i></div></div></div>");
                tabBodyTabContentPreviewPaneDoublescrollEl.InnerHtml.AppendHtml(tabBodyTabContentPreviewPaneDoublescrollWrapper2El);

                //<< Tab > Body > TabContent >> PreviewPane >> DoubleScroll
                tabBodyTabContentPreviewPaneEl.InnerHtml.AppendHtml(tabBodyTabContentPreviewPaneDoublescrollEl);


                //<< Tab > Body > TabContent >> PreviewPane
                tabBodyTabContentEl.InnerHtml.AppendHtml(tabBodyTabContentPreviewPaneEl);

                //<< Tab > Body > TabContent
                tabBodyEl.InnerHtml.AppendHtml(tabBodyTabContentEl);

                //<< Tab > Body
                tabEl.InnerHtml.AppendHtml(tabBodyEl);

                //<< Tab
                output.Content.AppendHtml(tabEl);

                var jsCompressor = new JavaScriptCompressor();
                #region << Init Scripts >>
                var tagHelperInitialized = false;
                var fileName = "form.js";
                if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvFieldDataCsv) + fileName))
                {
                    var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(WvFieldDataCsv) + fileName];
                    tagHelperInitialized = tagHelperContext.Initialized;
                }
                if (!tagHelperInitialized)
                {
                    var scriptContent = FileService.GetEmbeddedTextResource(fileName, "WebVella.Erp.Web.TagHelpers.WvFieldDataCsv");
                    var scriptEl = new TagBuilder("script");
                    scriptEl.Attributes.Add("type", "text/javascript");
                    scriptEl.InnerHtml.AppendHtml(jsCompressor.Compress(scriptContent));
                    output.PostContent.AppendHtml(scriptEl);

                    ViewContext.HttpContext.Items[typeof(WvFieldDataCsv) + fileName] = new WvTagHelperContext()
                    {
                        Initialized = true
                    };

                }
                #endregion

                #region << Add Inline Init Script for this instance >>
                var initScript = new TagBuilder("script");
                initScript.Attributes.Add("type", "text/javascript");
                var scriptTemplate = @"
						$(function(){
							DataCsvFormInit(""{{FieldId}}"",""{{Name}}"",""{{DelimiterFieldName}}"",""{{HasHeaderFieldName}}"",""{{Lang}}"");
						});";
                scriptTemplate = scriptTemplate.Replace("{{FieldId}}", FieldId.Value.ToString());
                scriptTemplate = scriptTemplate.Replace("{{Name}}", Name);
                scriptTemplate = scriptTemplate.Replace("{{DelimiterFieldName}}", DelimiterFieldName);
                scriptTemplate = scriptTemplate.Replace("{{HasHeaderFieldName}}", HasHeaderFieldName);
                scriptTemplate = scriptTemplate.Replace("{{Lang}}", Lang);

                var fieldConfig = new WvFieldTextareaConfig()
                {
                    ApiUrl = ApiUrl,
                    CanAddValues = Access == FieldAccess.FullAndCreate ? true : false
                };

                scriptTemplate = scriptTemplate.Replace("{{ConfigJson}}", JsonConvert.SerializeObject(fieldConfig));

                initScript.InnerHtml.AppendHtml(jsCompressor.Compress(scriptTemplate));

                output.PostContent.AppendHtml(initScript);
                #endregion


            }
            else if (Mode == FieldRenderMode.Display || Mode == FieldRenderMode.Simple || (Mode == FieldRenderMode.InlineEdit && Access == FieldAccess.ReadOnly))
            {

                if (!String.IsNullOrWhiteSpace(Value))
                {
                    //>> Ds
                    var dsEl = new TagBuilder("div");
                    dsEl.AddCssClass("doublescroll");
                    dsEl.Attributes.Add("id", "doublescroll-" + FieldId);

                    //>> Ds >> Wrapper 1
                    var dsWrapper1El = new TagBuilder("div");
                    dsWrapper1El.AddCssClass("doublescroll-wrapper1");
                    dsWrapper1El.InnerHtml.AppendHtml("<div class='doublescroll-inner1'></div>");
                    dsEl.InnerHtml.AppendHtml(dsWrapper1El);

                    //>> Ds >> Wrapper 2
                    var dsWrapper2El = new TagBuilder("div");
                    dsWrapper2El.AddCssClass("doublescroll-wrapper2");

                    //>> Ds >> Wrapper 2 >> inner 2
                    var dsWrapper2Inner2El = new TagBuilder("div");
                    dsWrapper2Inner2El.AddCssClass("doublescroll-inner2");

                    //>> Ds >> Wrapper 2 >> inner 2 >> preview
                    var dsWrapper2Inner2PreviewEl = new TagBuilder("div");
                    dsWrapper2Inner2PreviewEl.AddCssClass("preview");

                    ////////////////////////////////////////////////////////////////////////////////////////////////////
                    ///Table generation

                    var records = new List<dynamic>();
                    try
                    {
                        var delimiterName = "";
                        if (DelimiterValue == ErpDataCsvDelimiterType.TAB)
                            delimiterName = "tab";

                        records = new RenderService().GetCsvData(Value, HasHeaderValue, delimiterName);

                        if (records.Count > 0)
                        {

                            //>> table
                            var tableEl = new TagBuilder("table");
                            tableEl.AddCssClass("table table-bordered table-hover table-sm");
                            var columns = new List<GridColumn>();
                            var sample = (IDictionary<string, object>)records[0];
                            var index = 1;
                            foreach (var prop in sample.Keys)
                            {
                                columns.Add(new GridColumn()
                                {
                                    Label = HasHeaderValue ? prop : "Field" + index
                                });
                                index++;
                            }

                            if (HasHeaderValue)
                            {
                                //>> table > thead
                                var theadEl = new TagBuilder("thead");
                                var theadTrEl = new TagBuilder("tr");
                                foreach (var column in columns)
                                {
                                    var thEl = new TagBuilder("th");
                                    thEl.InnerHtml.Append(column.Label);
                                    theadTrEl.InnerHtml.AppendHtml(thEl);
                                    index++;
                                }

                                theadEl.InnerHtml.AppendHtml(theadTrEl);
                                tableEl.InnerHtml.AppendHtml(theadEl);
                            }



                            //>> table > tbody
                            var tbodyEl = new TagBuilder("tbody");
                            foreach (IDictionary<string, object> row in records)
                            {
                                var tbodyTrEl = new TagBuilder("tr");
                                foreach (var column in columns)
                                {
                                    var tbodyTdEl = new TagBuilder("td");
                                    tbodyTdEl.InnerHtml.Append((string)row[column.Label]);
                                    tbodyTrEl.InnerHtml.AppendHtml(tbodyTdEl);
                                }
                                tbodyEl.InnerHtml.AppendHtml(tbodyTrEl);
                            }
                            tableEl.InnerHtml.AppendHtml(tbodyEl);

                            dsWrapper2Inner2PreviewEl.InnerHtml.AppendHtml(tableEl);
                        }
                        else
                        {
                            var alertEl = new TagBuilder("div");
                            alertEl.AddCssClass("alert alert-info p-2");
                            if (Lang == "bg")
                            {
                                alertEl.InnerHtml.Append("Няма открити записи");
                            }
                            else
                            {
                                alertEl.InnerHtml.Append("No records found in data");
                            }
                            dsWrapper2Inner2PreviewEl.InnerHtml.AppendHtml(alertEl);
                        }

                    }
                    catch
                    {
                        var alertEl = new TagBuilder("div");
                        alertEl.AddCssClass("alert alert-danger p-2");
                        if (Lang == "bg")
                        {
                            alertEl.InnerHtml.Append("Грешен формат на данните. Опитайте с друг разделител.");
                        }
                        else
                        {
                            alertEl.InnerHtml.Append("Error in parsing data. Check another delimiter");
                        }
                        dsWrapper2Inner2PreviewEl.InnerHtml.AppendHtml(alertEl);
                    }


                    ////////////////////////////////////////////////////////////////////////////////////////////////////

                    //<< Ds >> Wrapper 2 >> inner 2 >> preview
                    dsWrapper2Inner2El.InnerHtml.AppendHtml(dsWrapper2Inner2PreviewEl);

                    //<< Ds >> Wrapper 2 >> inner 2
                    dsWrapper2El.InnerHtml.AppendHtml(dsWrapper2Inner2El);

                    //<< Ds >> Wrapper 2
                    dsEl.InnerHtml.AppendHtml(dsWrapper2El);
                    //<< Ds
                    output.Content.AppendHtml(dsEl);

                    var jsCompressor = new JavaScriptCompressor();
                    #region << Init Scripts >>
                    var tagHelperInitialized = false;
                    var fileName = "display.js";
                    if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvFieldDataCsv) + fileName))
                    {
                        var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(WvFieldDataCsv) + fileName];
                        tagHelperInitialized = tagHelperContext.Initialized;
                    }
                    if (!tagHelperInitialized)
                    {
                        var scriptContent = FileService.GetEmbeddedTextResource(fileName, "WebVella.Erp.Web.TagHelpers.WvFieldDataCsv");
                        var scriptEl = new TagBuilder("script");
                        scriptEl.Attributes.Add("type", "text/javascript");
                        scriptEl.InnerHtml.AppendHtml(jsCompressor.Compress(scriptContent));
                        output.PostContent.AppendHtml(scriptEl);

                        ViewContext.HttpContext.Items[typeof(WvFieldDataCsv) + fileName] = new WvTagHelperContext()
                        {
                            Initialized = true
                        };

                    }
                    #endregion

                    #region << Add Inline Init Script for this instance >>
                    var initScript = new TagBuilder("script");
                    initScript.Attributes.Add("type", "text/javascript");
                    var scriptTemplate = @"
						$(function(){
							DataCsvDisplayInit(""{{FieldId}}"");
						});";
                    scriptTemplate = scriptTemplate.Replace("{{FieldId}}", FieldId.Value.ToString());

                    var fieldConfig = new WvFieldTextareaConfig()
                    {
                        ApiUrl = ApiUrl,
                        CanAddValues = Access == FieldAccess.FullAndCreate ? true : false
                    };

                    scriptTemplate = scriptTemplate.Replace("{{ConfigJson}}", JsonConvert.SerializeObject(fieldConfig));

                    initScript.InnerHtml.AppendHtml(jsCompressor.Compress(scriptTemplate));

                    output.PostContent.AppendHtml(initScript);
                    #endregion



                }
                else
                {
                    output.Content.AppendHtml(EmptyValEl);
                }
            }
            else if (Mode == FieldRenderMode.InlineEdit)
            {

                #region << View Wrapper >>
                {
                    var viewWrapperEl = new TagBuilder("div");
                    viewWrapperEl.AddCssClass("input-group view-wrapper erp-data-csv");
                    viewWrapperEl.Attributes.Add("title", "double click to edit");
                    viewWrapperEl.Attributes.Add("id", $"view-{FieldId}");
                    //Prepend
                    if (PrependHtml.Count > 0)
                    {
                        var viewInputPrepend = new TagBuilder("span");
                        viewInputPrepend.AddCssClass("input-group-prepend erp-data-csv");
                        foreach (var htmlString in PrependHtml)
                        {
                            viewInputPrepend.InnerHtml.AppendHtml(htmlString);
                        }
                        viewWrapperEl.InnerHtml.AppendHtml(viewInputPrepend);
                    }
                    //Control
                    var dsEl = new TagBuilder("div");
                    dsEl.AddCssClass("form-control erp-data-csv doublescroll");
                    dsEl.Attributes.Add("id", "doublescroll-" + FieldId);


                    //>> Ds >> Wrapper 1
                    var dsWrapper1El = new TagBuilder("div");
                    dsWrapper1El.AddCssClass("doublescroll-wrapper1");
                    dsWrapper1El.InnerHtml.AppendHtml("<div class='doublescroll-inner1'></div>");
                    dsEl.InnerHtml.AppendHtml(dsWrapper1El);

                    //>> Ds >> Wrapper 2
                    var dsWrapper2El = new TagBuilder("div");
                    dsWrapper2El.AddCssClass("doublescroll-wrapper2");

                    //>> Ds >> Wrapper 2 >> inner 2
                    var dsWrapper2Inner2El = new TagBuilder("div");
                    dsWrapper2Inner2El.AddCssClass("doublescroll-inner2");

                    //>> Ds >> Wrapper 2 >> inner 2 >> preview
                    var dsWrapper2Inner2PreviewEl = new TagBuilder("div");
                    dsWrapper2Inner2PreviewEl.AddCssClass("preview");

                    ////////////////////////////////////////////////////////////////////////////////////////////////////
                    ///Table generation

                    var records = new List<dynamic>();
                    try
                    {
                        var delimiterName = "";
                        if (DelimiterValue == ErpDataCsvDelimiterType.TAB)
                            delimiterName = "tab";

                        records = new RenderService().GetCsvData(Value, HasHeaderValue, delimiterName);

                        if (records.Count > 0)
                        {

                            //>> table
                            var tableEl = new TagBuilder("table");
                            tableEl.AddCssClass("table table-bordered table-hover table-sm");
                            var columns = new List<GridColumn>();
                            var sample = (IDictionary<string, object>)records[0];
                            var index = 1;
                            foreach (var prop in sample.Keys)
                            {
                                columns.Add(new GridColumn()
                                {
                                    Label = HasHeaderValue ? prop : "Field" + index
                                });
                                index++;
                            }

                            if (HasHeaderValue)
                            {
                                //>> table > thead
                                var theadEl = new TagBuilder("thead");
                                var theadTrEl = new TagBuilder("tr");
                                foreach (var column in columns)
                                {
                                    var thEl = new TagBuilder("th");
                                    thEl.InnerHtml.Append(column.Label);
                                    theadTrEl.InnerHtml.AppendHtml(thEl);
                                    index++;
                                }

                                theadEl.InnerHtml.AppendHtml(theadTrEl);
                                tableEl.InnerHtml.AppendHtml(theadEl);
                            }



                            //>> table > tbody
                            var tbodyEl = new TagBuilder("tbody");
                            foreach (IDictionary<string, object> row in records)
                            {
                                var tbodyTrEl = new TagBuilder("tr");
                                foreach (var column in columns)
                                {
                                    var tbodyTdEl = new TagBuilder("td");
                                    tbodyTdEl.InnerHtml.Append((string)row[column.Label]);
                                    tbodyTrEl.InnerHtml.AppendHtml(tbodyTdEl);
                                }
                                tbodyEl.InnerHtml.AppendHtml(tbodyTrEl);
                            }
                            tableEl.InnerHtml.AppendHtml(tbodyEl);

                            dsWrapper2Inner2PreviewEl.InnerHtml.AppendHtml(tableEl);
                        }
                        else
                        {
                            var alertEl = new TagBuilder("div");
                            alertEl.AddCssClass("alert alert-info p-2");
                            if (Lang == "bg")
                            {
                                alertEl.InnerHtml.Append("Няма открити записи");
                            }
                            else
                            {
                                alertEl.InnerHtml.Append("No records found in data");
                            }
                            dsWrapper2Inner2PreviewEl.InnerHtml.AppendHtml(alertEl);
                        }

                    }
                    catch
                    {
                        var alertEl = new TagBuilder("div");
                        alertEl.AddCssClass("alert alert-danger p-2");
                        if (Lang == "bg")
                        {
                            alertEl.InnerHtml.Append("Грешен формат на данните. Опитайте с друг разделител.");
                        }
                        else
                        {
                            alertEl.InnerHtml.Append("Error in parsing data. Check another delimiter");
                        }
                        dsWrapper2Inner2PreviewEl.InnerHtml.AppendHtml(alertEl);
                    }


                    ////////////////////////////////////////////////////////////////////////////////////////////////////

                    //<< Ds >> Wrapper 2 >> inner 2 >> preview
                    dsWrapper2Inner2El.InnerHtml.AppendHtml(dsWrapper2Inner2PreviewEl);

                    //<< Ds >> Wrapper 2 >> inner 2
                    dsWrapper2El.InnerHtml.AppendHtml(dsWrapper2Inner2El);

                    //<< Ds >> Wrapper 2
                    dsEl.InnerHtml.AppendHtml(dsWrapper2El);


                    viewWrapperEl.InnerHtml.AppendHtml(dsEl);

                    //Append
                    var viewInputActionEl = new TagBuilder("span");
                    viewInputActionEl.AddCssClass("input-group-append erp-data-csv");
                    foreach (var htmlString in AppendHtml)
                    {
                        viewInputActionEl.InnerHtml.AppendHtml(htmlString);
                    }
                    viewInputActionEl.InnerHtml.AppendHtml("<button type=\"button\" class='btn btn-white action' title='edit'><i class='fa fa-fw fa-pencil-alt'></i></button>");
                    viewWrapperEl.InnerHtml.AppendHtml(viewInputActionEl);

                    output.Content.AppendHtml(viewWrapperEl);
                }
                #endregion

                #region << Edit Wrapper>>
                {
                    var editWrapperEl = new TagBuilder("div");
                    editWrapperEl.Attributes.Add("id", $"edit-{FieldId}");
                    editWrapperEl.Attributes.Add("style", $"display:none;");
                    editWrapperEl.AddCssClass("edit-wrapper");

                    var editInputGroupEl = new TagBuilder("div");
                    editInputGroupEl.AddCssClass("input-group");
                    //Prepend
                    if (PrependHtml.Count > 0)
                    {
                        var editInputPrepend = new TagBuilder("span");
                        editInputPrepend.AddCssClass("input-group-prepend  erp-data-csv");
                        foreach (var htmlString in PrependHtml)
                        {
                            editInputPrepend.InnerHtml.AppendHtml(htmlString);
                        }
                        editInputGroupEl.InnerHtml.AppendHtml(editInputPrepend);
                    }
                    //Control

                    var editInputEl = new TagBuilder("textarea");
                    editInputEl.AddCssClass("form-control erp-data-csv");

                    if (!String.IsNullOrWhiteSpace(Height))
                    {
                        editInputEl.Attributes.Add("style", $"height:{Height};");
                    }
                    if (Required)
                    {
                        editInputEl.Attributes.Add("required", null);
                    }
                    if (!String.IsNullOrWhiteSpace(Placeholder))
                    {
                        editInputEl.Attributes.Add("placeholder", Placeholder);
                    }
                    editInputEl.InnerHtml.AppendHtml((Value ?? "").ToString());
                    editInputGroupEl.InnerHtml.AppendHtml(editInputEl);



                    //Append
                    var editInputGroupAppendEl = new TagBuilder("span");
                    editInputGroupAppendEl.AddCssClass("input-group-append erp-data-csv");

                    foreach (var htmlString in AppendHtml)
                    {
                        editInputGroupAppendEl.InnerHtml.AppendHtml(htmlString);
                    }
                    editInputGroupAppendEl.InnerHtml.AppendHtml("<button type=\"button\" class='btn btn-white save' title='save'><i class='fa fa-fw fa-check go-green'></i></button>");
                    editInputGroupAppendEl.InnerHtml.AppendHtml("<button type=\"button\" class='btn btn-white cancel' title='cancel'><i class='fa fa-fw fa-times go-gray'></i></button>");

                    editInputGroupEl.InnerHtml.AppendHtml(editInputGroupAppendEl);
                    editWrapperEl.InnerHtml.AppendHtml(editInputGroupEl);

                    output.Content.AppendHtml(editWrapperEl);
                }
                #endregion

                var jsCompressor = new JavaScriptCompressor();
                #region << Init Scripts >>
                var tagHelperInitialized = false;
                var fileName = "inline-edit.js";
                if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvFieldDataCsv) + fileName))
                {
                    var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(WvFieldDataCsv) + fileName];
                    tagHelperInitialized = tagHelperContext.Initialized;
                }
                if (!tagHelperInitialized)
                {
                    var scriptContent = FileService.GetEmbeddedTextResource(fileName, "WebVella.Erp.Web.TagHelpers.WvFieldDataCsv");
                    var scriptEl = new TagBuilder("script");
                    scriptEl.Attributes.Add("type", "text/javascript");
                    scriptEl.InnerHtml.AppendHtml(jsCompressor.Compress(scriptContent));
                    output.PostContent.AppendHtml(scriptEl);

                    ViewContext.HttpContext.Items[typeof(WvFieldDataCsv) + fileName] = new WvTagHelperContext()
                    {
                        Initialized = true
                    };

                }
                #endregion

                #region << Add Inline Init Script for this instance >>
                var initScript = new TagBuilder("script");
                initScript.Attributes.Add("type", "text/javascript");
                var scriptTemplate = @"
						$(function(){
							DataCsvInlineEditInit(""{{FieldId}}"",""{{Name}}"",""{{EntityName}}"",""{{RecordId}}"",{{ConfigJson}});
						});";
                scriptTemplate = scriptTemplate.Replace("{{FieldId}}", (FieldId ?? null).ToString());
                scriptTemplate = scriptTemplate.Replace("{{Name}}", Name);
                scriptTemplate = scriptTemplate.Replace("{{EntityName}}", EntityName);
                scriptTemplate = scriptTemplate.Replace("{{RecordId}}", (RecordId ?? null).ToString());

                var fieldConfig = new WvFieldTextareaConfig()
                {
                    ApiUrl = ApiUrl,
                    CanAddValues = Access == FieldAccess.FullAndCreate ? true : false
                };

                scriptTemplate = scriptTemplate.Replace("{{ConfigJson}}", JsonConvert.SerializeObject(fieldConfig));

                initScript.InnerHtml.AppendHtml(jsCompressor.Compress(scriptTemplate));

                output.PostContent.AppendHtml(initScript);
                #endregion

            }
            #endregion


            //Finally
            if (SubInputEl != null)
            {
                output.PostContent.AppendHtml(SubInputEl);
            }

            return;
        }

    }
}
