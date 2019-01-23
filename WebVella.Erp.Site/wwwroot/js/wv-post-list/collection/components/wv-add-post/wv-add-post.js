import axios from 'axios';
import * as action from '../../store/actions';
function SubmitReplyForm(scope) {
    let storeState = scope.store.getState();
    let requestConfig = {
        headers: {
            'Content-Type': 'application/json;charset=UTF-8',
            "Access-Control-Allow-Origin": "*",
        }
    };
    let siteRoot = storeState.siteRootUrl;
    let requestUrl = siteRoot + "/api/v3.0/p/project/pc-post-list/create";
    let requestBody = new Object();
    switch (storeState.mode) {
        case "timelogs":
            requestBody["minutes"] = scope.minutes;
            requestBody["body"] = scope.body;
            requestBody["isBillable"] = scope.isBillable;
            break;
        default:
            requestBody["subject"] = "";
            requestBody["parentId"] = null;
            requestBody["body"] = scope.ckEditor.getData();
            break;
    }
    requestBody["relatedRecordId"] = storeState.relatedRecordId;
    requestBody["relatedRecords"] = storeState.relatedRecords;
    axios.post(requestUrl, requestBody, requestConfig)
        .then(function (response) {
        let actionPayload = {
            post: response.data.object
        };
        scope.addPost(actionPayload);
        if (scope.ckEditor) {
            scope.ckEditor.setData("");
        }
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
        this.isReplyBtnDisabled = true;
        this.ckEditor = null;
        this.minutes = null;
        this.isBillable = true;
        this.body = "";
    }
    componentWillLoad() {
        this.store.mapDispatchToProps(this, {
            addPost: action.addPost
        });
    }
    componentDidLoad() {
        let scope = this;
        let getFormEl = document.getElementById("form-add-post");
        if (getFormEl) {
            getFormEl.addEventListener('keyup', function (ev) {
                if (!(ev.keyCode == 13 && ev.ctrlKey)) {
                    var editorContent = scope.ckEditor.getData();
                    if (editorContent) {
                        scope.isReplyBtnDisabled = false;
                    }
                    else {
                        scope.isReplyBtnDisabled = true;
                    }
                }
            });
            getFormEl.addEventListener('keydown', function (ev) {
                if (ev.keyCode == 13 && ev.ctrlKey && !scope.isReplyBtnDisabled) {
                    SubmitReplyForm(scope);
                }
            });
            getFormEl.addEventListener('submit', function (ev) {
                ev.preventDefault();
                SubmitReplyForm(scope);
            });
        }
    }
    ReplyLinkHandler(event) {
        event.preventDefault();
        let scope = this;
        if (scope.isReplyBoxVisible) {
            scope.isReplyBoxVisible = false;
        }
        else if (!scope.ckEditor) {
            let storeState = scope.store.getState();
            scope.ckEditor = window.CKEDITOR.replace('reply-' + storeState.relatedRecordId, storeState.editorConfig);
            scope.ckEditor.on('instanceReady', function () {
                scope.isReplyBoxVisible = true;
                window.setTimeout(function () {
                    scope.ckEditor.focus();
                }, 100);
            });
        }
        else {
            scope.isReplyBoxVisible = true;
            window.setTimeout(function () {
                scope.ckEditor.focus();
            }, 100);
        }
    }
    minutesChange(ev) {
        var parsed = parseInt(ev.target.value);
        if (!isNaN(parsed) && parsed > 0) {
            this.minutes = parsed;
            this.isReplyBtnDisabled = false;
        }
        else {
            this.minutes = 0;
            this.isReplyBtnDisabled = true;
        }
    }
    bodyChange(ev) {
        this.body = ev.target.value;
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
            h("div", { class: "mb-2 " + (scope.isReplyBoxVisible ? "d-none" : "") },
                h("button", { type: "button", class: "btn btn-sm btn-white", onClick: (e) => scope.ReplyLinkHandler(e) }, "New Comment")),
            h("div", { class: "post add-new mb-2 mt-0 " + (scope.isReplyBoxVisible ? "" : "d-none") },
                h("div", { class: "header" },
                    h("div", { class: "avatar" },
                        h("img", { src: currentUserImagePath })),
                    h("div", { class: "meta" },
                        h("div", { class: "title" },
                            h("span", { class: "username" }, currentUserName)))),
                h("div", { class: "body" },
                    h("form", { name: "PcPostListSaveForm", id: "form-add-post" },
                        h("textarea", { id: "reply-" + storeState.relatedRecordId }),
                        h("div", { class: "wv-field-html-toolbar" },
                            h("div", { class: "content" },
                                h("button", { type: "submit", class: "btn btn-sm btn-primary", disabled: scope.isReplyBtnDisabled }, "Submit"),
                                h("button", { type: "button", class: "btn btn-sm btn-link", onClick: (e) => scope.ReplyLinkHandler(e) }, "Cancel")),
                            h("div", { class: "note" }, "Ctrl+Enter to submit")))))));
    }
    static get is() { return "wv-add-post"; }
    static get properties() { return {
        "body": {
            "state": true
        },
        "ckEditor": {
            "state": true
        },
        "isBillable": {
            "state": true
        },
        "isReplyBoxVisible": {
            "state": true
        },
        "isReplyBtnDisabled": {
            "state": true
        },
        "minutes": {
            "state": true
        },
        "store": {
            "context": "store"
        }
    }; }
}
