import moment from 'moment';
import axios from 'axios';
import * as action from '../../store/actions';
function RenderPostFooter(props) {
    let scope = props.scope;
    let storeState = scope.store.getState();
    if (!scope.post["nodes"]) {
        scope.post["nodes"] = new Array();
    }
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
    return (h("div", { class: "footer" },
        h("ul", { class: "comment-list" },
            scope.post["nodes"].map(function (comment) {
                return (h("wv-comment", { key: comment["id"], comment: comment }));
            }),
            h("li", { class: "comment add-new " + (scope.isReplyBoxVisible ? "" : "d-none") },
                h("div", { class: "avatar" },
                    h("img", { src: currentUserImagePath })),
                h("div", { class: "meta" },
                    h("div", { class: "header" },
                        h("div", { class: "title" },
                            h("span", { class: "username" }, currentUserName))),
                    h("div", { class: "body" },
                        h("form", { name: "PcPostListSaveForm", id: "form-" + storeState.relatedRecordId + "-" + scope.post["id"] },
                            h("textarea", { id: "reply-" + storeState.relatedRecordId + "-" + scope.post["id"] }),
                            h("div", { class: "wv-field-html-toolbar" },
                                h("div", { class: "content" },
                                    h("button", { type: "submit", class: "btn btn-sm btn-primary", disabled: scope.isReplyBtnDisabled }, "Submit"),
                                    h("button", { type: "button", class: "btn btn-sm btn-link", onClick: (e) => scope.ReplyLinkHandler(e) }, "Cancel")),
                                h("div", { class: "note" }, "Ctrl+Enter to submit")))))))));
}
function SubmitReplyForm(scope) {
    let storeState = scope.store.getState();
    let editorContent = scope.ckEditor.getData();
    let requestConfig = {
        headers: {
            'Content-Type': 'application/json;charset=UTF-8',
            "Access-Control-Allow-Origin": "*",
        }
    };
    let siteRoot = storeState.siteRootUrl;
    let requestUrl = siteRoot + "/api/v3.0/p/project/pc-post-list/create";
    let requestBody = new Object();
    requestBody["relatedRecordId"] = storeState.relatedRecordId;
    requestBody["relatedRecords"] = storeState.relatedRecords;
    requestBody["body"] = editorContent;
    requestBody["parentId"] = scope.post["id"];
    axios.post(requestUrl, requestBody, requestConfig)
        .then(function (response) {
        let actionPayload = {
            comment: response.data.object
        };
        scope.addComment(actionPayload);
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
export class WvPost {
    constructor() {
        this.post = new Object();
        this.isReplyBoxVisible = false;
        this.isReplyBtnDisabled = true;
        this.ckEditor = null;
        this.reloadPostIndex = 1;
    }
    componentWillLoad() {
        this.store.mapStateToProps(this, (state) => {
            return {
                reloadPostIndex: state.reloadPostIndex,
            };
        });
        this.store.mapDispatchToProps(this, {
            removePost: action.removePost,
            addComment: action.addComment
        });
    }
    ReplyLinkHandler(event) {
        event.preventDefault();
        let scope = this;
        if (scope.isReplyBoxVisible) {
            scope.isReplyBoxVisible = false;
        }
        else if (!scope.ckEditor) {
            let storeState = scope.store.getState();
            scope.ckEditor = window.CKEDITOR.replace('reply-' + storeState.relatedRecordId + "-" + scope.post["id"], storeState.editorConfig);
            scope.ckEditor.on('instanceReady', function () {
                scope.isReplyBoxVisible = true;
                window.setTimeout(function () {
                    scope.ckEditor.focus();
                    let getFormEl = document.getElementById("form-" + storeState.relatedRecordId + "-" + scope.post["id"]);
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
    DeleteLinkHandler(event) {
        event.preventDefault();
        let scope = this;
        let storeState = scope.store.getState();
        let requestConfig = {
            headers: {
                'Content-Type': 'application/json;charset=UTF-8',
                "Access-Control-Allow-Origin": "*",
            }
        };
        let siteRoot = storeState.siteRootUrl;
        let requestUrl = siteRoot + "/api/v3.0/p/project/pc-post-list/delete";
        let requestBody = new Object();
        requestBody["id"] = scope.post["id"];
        axios.post(requestUrl, requestBody, requestConfig)
            .then(function () {
            let actionPayload = {
                postId: scope.post["id"]
            };
            scope.removePost(actionPayload);
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
    render() {
        let scope = this;
        let storeState = scope.store.getState();
        let userImagePath = "/assets/avatar.png";
        let userName = "system";
        let hrTime = moment(scope.post["created_on"]).fromNow();
        let timeString = moment(scope.post["created_on"]).format('YYYY MM DD HH:mm');
        let subject = scope.post["subject"];
        let body = scope.post["body"];
        let authorId = "";
        let currentUser = storeState.currentUser;
        let currentUserId = "";
        if (currentUser) {
            currentUserId = currentUser["id"];
        }
        if (scope.post["$user_1n_comment"] && scope.post["$user_1n_comment"][0]["image"]) {
            userImagePath = "/fs" + scope.post["$user_1n_comment"][0]["image"];
        }
        if (scope.post["$user_1n_comment"] && scope.post["$user_1n_comment"][0]["username"]) {
            userName = scope.post["$user_1n_comment"][0]["username"];
        }
        if (scope.post["$user_1n_comment"] && scope.post["$user_1n_comment"][0]["id"]) {
            authorId = scope.post["$user_1n_comment"][0]["id"];
        }
        if (storeState.isDebug) {
            userImagePath = "http://localhost:2202" + userImagePath;
        }
        return (h("div", { class: "post" },
            h("div", { class: "header" },
                h("div", { class: "avatar" },
                    h("img", { src: userImagePath })),
                h("div", { class: "meta" },
                    h("div", { class: "title" },
                        h("span", { class: "username" }, userName),
                        " ",
                        h("span", { innerHTML: subject })),
                    h("div", { class: "title-aux" },
                        h("span", { title: timeString }, hrTime),
                        h("span", { class: "actions" },
                            h("button", { type: "button", onClick: (e) => scope.ReplyLinkHandler(e), class: "btn btn-link btn-sm " }, "Reply"),
                            h("button", { type: "button", onClick: (e) => { if (window.confirm('Are you sure?'))
                                    scope.DeleteLinkHandler(e); }, class: "btn btn-link btn-sm go-red " + (authorId == currentUserId ? "" : "d-none") }, "Delete"))))),
            h("div", { class: "body", innerHTML: body }),
            h(RenderPostFooter, { scope: scope })));
    }
    static get is() { return "wv-post"; }
    static get properties() { return {
        "ckEditor": {
            "state": true
        },
        "isReplyBoxVisible": {
            "state": true
        },
        "isReplyBtnDisabled": {
            "state": true
        },
        "post": {
            "type": "Any",
            "attr": "post"
        },
        "reloadPostIndex": {
            "state": true
        },
        "store": {
            "context": "store"
        }
    }; }
}
