import axios from 'axios';
import * as action from '../../store/actions';
import moment from 'moment';
function SubmitReplyForm(scope) {
    scope.isWarningVisible = false;
    if (!scope.minutes || scope.minutes === "0" || scope.minutes === 0) {
        scope.isWarningVisible = true;
        return;
    }
    let storeState = scope.store.getState();
    let requestConfig = {
        headers: {
            'Content-Type': 'application/json;charset=UTF-8',
            "Access-Control-Allow-Origin": "*",
        }
    };
    let siteRoot = storeState.siteRootUrl;
    let requestUrl = siteRoot + "/api/v3.0/p/project/pc-timelog-list/create";
    let requestBody = new Object();
    requestBody["minutes"] = scope.minutes;
    requestBody["loggedOn"] = scope.loggedOn;
    requestBody["body"] = scope.taskBody;
    requestBody["isBillable"] = scope.isBillable;
    requestBody["relatedRecords"] = storeState.relatedRecords;
    axios.post(requestUrl, requestBody, requestConfig)
        .then(function (response) {
        let actionPayload = {
            timelog: response.data.object
        };
        scope.addTimelog(actionPayload);
        scope.minutes = null;
        scope.isBillable = true;
        scope.taskBody = "";
        document.getElementById('wv-timelog-minutes').focus();
        document.getElementById('wv-timelog-minutes').value = '';
        document.getElementById('wv-timelog-minutes').blur();
        document.getElementById('wv-timelog-body').focus();
        document.getElementById('wv-timelog-body').value = '';
        document.getElementById('wv-timelog-body').blur();
        scope.isReplyBtnDisabled = true;
        scope.isReplyBoxVisible = false;
    })
        .catch(function (error) {
        if (error.response) {
            if (error.response.data) {
                alert(error.response.data);
            }
            else {
                alert(error.response.statusText);
            }
        }
        else if (error.message) {
            alert(error.message);
        }
        else {
            alert(error);
        }
    });
}
export class WvAddNew {
    constructor() {
        this.isReplyBoxVisible = false;
        this.isWarningVisible = false;
        this.minutes = null;
        this.isBillable = true;
        this.taskBody = "";
        this.loggedOn = null;
        this.datePickr = null;
    }
    componentWillLoad() {
        this.store.mapDispatchToProps(this, {
            addTimelog: action.addTimelog
        });
        this.loggedOn = moment().toISOString();
        this.isBillable = this.store.getState().isBillable;
    }
    componentDidLoad() {
        let scope = this;
        let getFormEl = document.getElementById("form-timelog-add");
        if (getFormEl) {
            getFormEl.addEventListener('keydown', function (ev) {
                if (ev.keyCode == 13 && ev.ctrlKey) {
                    document.getElementById("wv-timelog-minutes").blur();
                    document.getElementById("wv-timelog-body").blur();
                    SubmitReplyForm(scope);
                }
            });
            getFormEl.addEventListener('submit', function (ev) {
                ev.preventDefault();
                SubmitReplyForm(scope);
            });
        }
        window.setTimeout(function () {
            let flatPickrServerDateTimeFormat = "Z";
            let flatPickrUiDateTimeFormat = "d M Y";
            scope.datePickr = window.flatpickr("#wv-timelog-add-datetime", { time_24hr: true, dateFormat: flatPickrServerDateTimeFormat, enableTime: false, altInput: true, altFormat: flatPickrUiDateTimeFormat });
        }, 100);
    }
    ReplyLinkHandler(event) {
        event.preventDefault();
        let scope = this;
        if (scope.isReplyBoxVisible) {
            scope.isReplyBoxVisible = false;
        }
        else {
            scope.isReplyBoxVisible = true;
        }
    }
    minutesChange(ev) {
        var parsed = parseInt(ev.target.value);
        if (!isNaN(parsed) && parsed > 0) {
            this.minutes = parsed;
        }
        else {
            this.minutes = null;
        }
    }
    bodyChange(ev) {
        this.taskBody = ev.target.value;
    }
    loggedOnChange(ev) {
        this.loggedOn = ev.target.value;
    }
    billableChange(event) {
        event.preventDefault();
        event.stopPropagation();
        let scope = this;
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        scope.isBillable = value;
    }
    render() {
        let scope = this;
        let storeState = scope.store.getState();
        let currentUserImagePath = "/assets/avatar.png";
        let currentUserName = "anonymous";
        let currentUser = storeState.currentUser;
        if (currentUser) {
            if (currentUser["image"]) {
                currentUserImagePath = "/fs" + currentUser["image"];
            }
            currentUserName = currentUser["username"];
        }
        if (storeState.isDebug) {
            currentUserImagePath = "http://localhost:2202" + currentUserImagePath;
        }
        return (h("div", null,
            h("div", { class: "mb-4 " + (scope.isReplyBoxVisible ? "d-none" : "") },
                h("button", { type: "button", class: "btn btn-sm btn-white", onClick: (e) => scope.ReplyLinkHandler(e) }, "Add time")),
            h("div", { class: "post add-new mb-4 mt-0 " + (scope.isReplyBoxVisible ? "" : "d-none") },
                h("div", { class: "header" },
                    h("div", { class: "avatar" },
                        h("img", { src: currentUserImagePath })),
                    h("div", { class: "meta" },
                        h("div", { class: "title" },
                            h("span", { class: "username" }, currentUserName)))),
                h("div", { class: "body" },
                    h("form", { name: "PcPostListSaveForm", id: "form-timelog-add" },
                        h("div", { class: "row" },
                            h("div", { class: "col-4" },
                                h("div", { class: "form-group" },
                                    h("div", { class: "input-group input-group-sm" },
                                        h("span", { class: "input-group-prepend" },
                                            h("span", { class: "input-group-text" },
                                                h("i", { class: "far fa-calendar-alt" }))),
                                        h("input", { class: "form-control", id: "wv-timelog-add-datetime", name: "logged_on", value: scope.loggedOn, onChange: (e) => scope.loggedOnChange(e) })))),
                            h("div", { class: "col-4" },
                                h("div", { class: "form-group" },
                                    h("div", { class: "input-group input-group-sm" },
                                        h("input", { class: "form-control", id: "wv-timelog-minutes", name: "minutes", value: scope.minutes, onChange: (e) => scope.minutesChange(e) }),
                                        h("span", { class: "input-group-append" },
                                            h("span", { class: "input-group-text" }, "minutes"))))),
                            h("div", { class: "col-4" },
                                h("div", { class: "form-group form-check" },
                                    h("label", { class: "form-check-label" },
                                        h("input", { type: "checkbox", class: "form-check-input", value: "true", checked: scope.isBillable, onChange: (e) => scope.billableChange(e) }),
                                        "is billable")))),
                        h("div", { class: "form-group erp-field" },
                            h("textarea", { class: "form-control", id: "wv-timelog-body", onChange: (e) => scope.bodyChange(e) }, scope.taskBody)),
                        h("div", { class: "alert alert-danger " + (scope.isWarningVisible ? "" : "d-none") }, "Minutes are required or need to be more than 0"),
                        h("div", { class: "wv-field-html-toolbar mt-2" },
                            h("div", { class: "content" },
                                h("button", { type: "submit", class: "btn btn-sm btn-primary" }, "Submit"),
                                h("button", { type: "button", class: "btn btn-sm btn-link", onClick: (e) => scope.ReplyLinkHandler(e) }, "Cancel")),
                            h("div", { class: "note " }, "Ctrl+Enter to submit")))))));
    }
    static get is() { return "wv-add-timelog"; }
    static get properties() { return {
        "datePickr": {
            "state": true
        },
        "isBillable": {
            "state": true
        },
        "isReplyBoxVisible": {
            "state": true
        },
        "isWarningVisible": {
            "state": true
        },
        "loggedOn": {
            "state": true
        },
        "minutes": {
            "state": true
        },
        "store": {
            "context": "store"
        },
        "taskBody": {
            "state": true
        }
    }; }
}
