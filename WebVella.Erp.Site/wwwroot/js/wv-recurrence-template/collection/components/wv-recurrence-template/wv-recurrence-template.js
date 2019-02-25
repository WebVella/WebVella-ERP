import RecurrenceTemplate from '../../models/RecurrenceTemplate';
export class WvDatasourceManage {
    constructor() {
        this.recurrenceTemplate = null;
        this.templateDefault = null;
        this.typeOptions = null;
        this.endTypeOptions = null;
        this.periodTypeOptions = null;
        this.changeTypeOptions = null;
        this.recurrence = new RecurrenceTemplate();
        this.typeSelectOptions = new Array();
        this.endTypeSelectOptions = new Array();
        this.periodTypeSelectOptions = new Array();
        this.changeTypeSelectOptions = new Array();
        this.hiddenValue = null;
        this.initialType = 0;
    }
    componentWillLoad() {
        if (this.recurrenceTemplate) {
            this.recurrence = JSON.parse(this.recurrenceTemplate);
        }
        else {
            this.recurrence = JSON.parse(this.templateDefault);
        }
        this.typeSelectOptions = JSON.parse(this.typeOptions);
        this.endTypeSelectOptions = JSON.parse(this.endTypeOptions);
        this.periodTypeSelectOptions = JSON.parse(this.periodTypeOptions);
        this.changeTypeSelectOptions = JSON.parse(this.changeTypeOptions);
        this.initialType = this.recurrence.type;
        this.hiddenValue = JSON.stringify(this.recurrence);
    }
    valueChangeHandler(ev, fieldName, dataType) {
        let scope = this;
        var inputValue = ev.target.value;
        switch (dataType) {
            case "number":
            case "int":
                scope.recurrence[fieldName] = parseInt(inputValue);
                break;
            case "date":
                scope.recurrence[fieldName] = inputValue;
                break;
            case "bool":
                if (ev.target.checked) {
                    scope.recurrence[fieldName] = true;
                }
                else {
                    scope.recurrence[fieldName] = false;
                }
                break;
            case "string":
                scope.recurrence[fieldName] = inputValue;
                break;
            default:
                break;
        }
        scope.hiddenValue = JSON.stringify(scope.recurrence);
    }
    render() {
        let scope = this;
        return (h("div", null,
            h("input", { type: "hidden", value: scope.hiddenValue, name: "recurrence_template" }),
            h("div", { class: "form-group erp-field label-horizontal row no-gutters form" },
                h("label", { class: "col-12 col-sm-auto col-form-label label-horizontal pr-0 pr-sm-2" }, "Repeat:"),
                h("div", { class: "col" },
                    h("select", { class: "form-control form-control-sm", onChange: (e) => scope.valueChangeHandler(e, "type", "int") }, scope.typeSelectOptions.map(function (option, index) {
                        return (h("option", { key: index, value: option.value, selected: option.value == scope.recurrence.type.toString() }, option.label));
                    })))),
            h("div", { class: "mt-3 " + (scope.recurrence.type === 7 ? "" : "d-none") },
                h("hr", { class: "divider mt-2 mb-2" }),
                h("div", { class: "form-group erp-field label-horizontal row no-gutters form" },
                    h("label", { class: "col-12 col-sm-auto col-form-label label-horizontal pr-0 pr-sm-2" }, "Repeat every:"),
                    h("div", { class: "col" },
                        h("div", { class: "input-group" },
                            h("input", { class: "form-control flex-grow-0", style: { "width": "80px" }, value: scope.recurrence.interval, onChange: (e) => scope.valueChangeHandler(e, "interval", "int") }),
                            h("span", { class: "input-group-prepend input-group-append" },
                                h("span", { class: "input-group-text p-0", style: { "width": "5px" } })),
                            h("select", { class: "form-control form-control-sm  flex-grow-0", style: { "width": "100px" }, onChange: (e) => scope.valueChangeHandler(e, "repeat_period_type", "int") }, scope.periodTypeSelectOptions.map(function (option, index) {
                                return (h("option", { key: index, value: option.value, selected: option.value == scope.recurrence.repeat_period_type.toString() }, option.label));
                            }))))),
                h("div", { class: "form-group erp-field label-horizontal row no-gutters form mt-3 " + (scope.recurrence.repeat_period_type === 4 ? "" : "d-none") },
                    h("label", { class: "col-12 col-sm-auto col-form-label label-horizontal pr-0 pr-sm-2" }),
                    h("div", { class: "col" },
                        h("div", { class: "form-check form-check-inline" },
                            h("input", { class: "form-check-input", type: "checkbox", id: "Monday", value: "true", checked: scope.recurrence.allow_monday, onChange: (e) => scope.valueChangeHandler(e, "allow_monday", "bool") }),
                            h("label", { class: "form-check-label", htmlFor: "Monday" }, "Monday")),
                        h("div", { class: "form-check form-check-inline" },
                            h("input", { class: "form-check-input", type: "checkbox", id: "Tuesday", value: "true", checked: scope.recurrence.allow_tuesday, onChange: (e) => scope.valueChangeHandler(e, "allow_tuesday", "bool") }),
                            h("label", { class: "form-check-label", htmlFor: "Tuesday" }, "Tuesday")),
                        h("div", { class: "form-check form-check-inline" },
                            h("input", { class: "form-check-input", type: "checkbox", id: "Wednesday", value: "true", checked: scope.recurrence.allow_wednesday, onChange: (e) => scope.valueChangeHandler(e, "allow_wednesday", "bool") }),
                            h("label", { class: "form-check-label", htmlFor: "Wednesday" }, "Wednesday")),
                        h("div", { class: "form-check form-check-inline" },
                            h("input", { class: "form-check-input", type: "checkbox", id: "Thursday", value: "true", checked: scope.recurrence.allow_thursday, onChange: (e) => scope.valueChangeHandler(e, "allow_thursday", "bool") }),
                            h("label", { class: "form-check-label", htmlFor: "Thursday" }, "Thursday")),
                        h("div", { class: "form-check form-check-inline" },
                            h("input", { class: "form-check-input", type: "checkbox", id: "Friday", value: "true", checked: scope.recurrence.allow_friday, onChange: (e) => scope.valueChangeHandler(e, "allow_friday", "bool") }),
                            h("label", { class: "form-check-label", htmlFor: "Friday" }, "Friday")),
                        h("div", { class: "form-check form-check-inline" },
                            h("input", { class: "form-check-input", type: "checkbox", id: "Saturday", value: "true", checked: scope.recurrence.allow_saturday, onChange: (e) => scope.valueChangeHandler(e, "allow_saturday", "bool") }),
                            h("label", { class: "form-check-label", htmlFor: "Saturday" }, "Saturday")),
                        h("div", { class: "form-check form-check-inline" },
                            h("input", { class: "form-check-input", type: "checkbox", id: "Sunday", value: "true", checked: scope.recurrence.allow_sunday, onChange: (e) => scope.valueChangeHandler(e, "allow_sunday", "bool") }),
                            h("label", { class: "form-check-label", htmlFor: "Sunday" }, "Sunday")))),
                h("div", { class: "form-group erp-field label-horizontal row no-gutters form mt-3 " + (scope.recurrence.repeat_period_type === 5 ? "" : "d-none") },
                    h("label", { class: "col-12 col-sm-auto col-form-label label-horizontal pr-0 pr-sm-2" }),
                    h("div", { class: "col" },
                        h("div", { class: "form-check form-check-inline" },
                            h("input", { class: "form-check-input", type: "radio", name: "monthPeriodOptions", id: "sameDate", value: "0", checked: scope.recurrence.repeat_month_type === 0, onChange: (e) => scope.valueChangeHandler(e, "repeat_month_type", "int") }),
                            h("label", { class: "form-check-label", htmlFor: "sameDate" }, "same date")),
                        h("div", { class: "form-check form-check-inline" },
                            h("input", { class: "form-check-input", type: "radio", name: "monthPeriodOptions", id: "sameWeekDay", value: "1", checked: scope.recurrence.repeat_month_type === 1, onChange: (e) => scope.valueChangeHandler(e, "repeat_month_type", "int") }),
                            h("label", { class: "form-check-label", htmlFor: "sameWeekDay" }, "same week day")))),
                h("hr", { class: "divider mt-2 mb-2" })),
            h("div", { class: "mt-3 " + (scope.initialType === 0 ? "d-none" : "") },
                h("div", { class: "form-group erp-field label-horizontal row no-gutters form" },
                    h("label", { class: "col-12 col-sm-auto col-form-label label-horizontal pr-0 pr-sm-2" }, "Apply to:"),
                    h("div", { class: "col" },
                        h("select", { class: "form-control form-control-sm", onChange: (e) => scope.valueChangeHandler(e, "recurrence_change_type", "int") }, scope.changeTypeSelectOptions.map(function (option, index) {
                            return (h("option", { key: index, value: option.value, selected: option.value == scope.recurrence.recurrence_change_type.toString() }, option.label));
                        })))))));
    }
    static get is() { return "wv-recurrence-template"; }
    static get properties() { return {
        "changeTypeOptions": {
            "type": String,
            "attr": "change-type-options"
        },
        "changeTypeSelectOptions": {
            "state": true
        },
        "endTypeOptions": {
            "type": String,
            "attr": "end-type-options"
        },
        "endTypeSelectOptions": {
            "state": true
        },
        "hiddenValue": {
            "state": true
        },
        "initialType": {
            "state": true
        },
        "periodTypeOptions": {
            "type": String,
            "attr": "period-type-options"
        },
        "periodTypeSelectOptions": {
            "state": true
        },
        "recurrence": {
            "state": true
        },
        "recurrenceTemplate": {
            "type": String,
            "attr": "recurrence-template"
        },
        "templateDefault": {
            "type": String,
            "attr": "template-default"
        },
        "typeOptions": {
            "type": String,
            "attr": "type-options"
        },
        "typeSelectOptions": {
            "state": true
        }
    }; }
}
