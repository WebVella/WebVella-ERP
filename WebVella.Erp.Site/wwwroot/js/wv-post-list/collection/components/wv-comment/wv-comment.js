import moment from 'moment';
import axios from 'axios';
import * as action from '../../store/actions';
export class WvComment {
    constructor() {
        this.comment = new Object();
    }
    componentWillLoad() {
        this.store.mapDispatchToProps(this, {
            removeComment: action.removeComment
        });
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
        requestBody["id"] = scope.comment["id"];
        axios.post(requestUrl, requestBody, requestConfig)
            .then(function () {
            let actionPayload = {
                parentId: scope.comment["parent_id"],
                commentId: scope.comment["id"]
            };
            scope.removeComment(actionPayload);
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
        let hrTime = moment(scope.comment["created_on"]).fromNow();
        let timeString = moment(scope.comment["created_on"]).format('YYYY MM DD HH:mm');
        let body = scope.comment["body"];
        let authorId = "";
        let currentUser = storeState.currentUser;
        let currentUserId = "";
        if (currentUser) {
            currentUserId = currentUser["id"];
        }
        if (scope.comment["$user_1n_comment"] && scope.comment["$user_1n_comment"][0]["image"]) {
            userImagePath = "/fs" + scope.comment["$user_1n_comment"][0]["image"];
        }
        if (scope.comment["$user_1n_comment"] && scope.comment["$user_1n_comment"][0]["username"]) {
            userName = scope.comment["$user_1n_comment"][0]["username"];
        }
        if (scope.comment["$user_1n_comment"] && scope.comment["$user_1n_comment"][0]["id"]) {
            authorId = scope.comment["$user_1n_comment"][0]["id"];
        }
        if (storeState.isDebug) {
            userImagePath = "http://localhost:2202" + userImagePath;
        }
        return (h("li", { class: "comment" },
            h("div", { class: "avatar" },
                h("img", { src: userImagePath })),
            h("div", { class: "meta" },
                h("div", { class: "header" },
                    h("div", { class: "title" },
                        h("span", { class: "username" }, userName)),
                    h("div", { class: "title-aux" },
                        h("span", { title: timeString }, hrTime),
                        h("span", { class: "actions" },
                            h("button", { type: "button", onClick: (e) => { if (window.confirm('Are you sure?'))
                                    scope.DeleteLinkHandler(e); }, class: "btn btn-link btn-sm go-red " + (authorId == currentUserId ? "" : "d-none") }, "Delete")))),
                h("div", { class: "body", innerHTML: body }))));
    }
    static get is() { return "wv-comment"; }
    static get properties() { return {
        "comment": {
            "type": "Any",
            "attr": "comment"
        },
        "store": {
            "context": "store"
        }
    }; }
}
