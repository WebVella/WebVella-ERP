import moment from 'moment';
import axios from 'axios';
import * as action from '../../store/actions';
export class WvPost {
    constructor() {
        this.post = new Object();
        this.isReplyBoxVisible = false;
        this.isReplyBtnDisabled = true;
        this.reloadPostIndex = 1;
    }
    componentWillLoad() {
        this.store.mapStateToProps(this, (state) => {
            return {
                reloadPostIndex: state.reloadPostIndex,
            };
        });
        this.store.mapDispatchToProps(this, {
            removeTimelog: action.removeTimelog
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
        let requestUrl = siteRoot + "/api/v3.0/p/project/pc-timelog-list/delete";
        let requestBody = new Object();
        requestBody["id"] = scope.post["id"];
        axios.post(requestUrl, requestBody, requestConfig)
            .then(function () {
            let actionPayload = {
                timelogId: scope.post["id"]
            };
            scope.removeTimelog(actionPayload);
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
        let timeString = moment(scope.post["created_on"]).format('DD MMM YYYY HH:mm');
        let logString = moment(scope.post["logged_on"]).format('DD MMM YYYY');
        let subject = scope.post["subject"];
        let body = scope.post["body"];
        let authorId = "";
        let currentUser = storeState.currentUser;
        let currentUserId = "";
        if (currentUser) {
            currentUserId = currentUser["id"];
        }
        if (scope.post["$user_1n_timelog"] && scope.post["$user_1n_timelog"][0]["image"]) {
            userImagePath = "/fs" + scope.post["$user_1n_timelog"][0]["image"];
        }
        if (scope.post["$user_1n_timelog"] && scope.post["$user_1n_timelog"][0]["username"]) {
            userName = scope.post["$user_1n_timelog"][0]["username"];
        }
        if (scope.post["$user_1n_timelog"] && scope.post["$user_1n_timelog"][0]["id"]) {
            authorId = scope.post["$user_1n_timelog"][0]["id"];
        }
        let billableString = "nonbillable";
        if (scope.post["is_billable"] || scope.post["is_billable"] === "true") {
            billableString = "billable";
        }
        subject = "logged <strong>" + scope.post["minutes"] + " " + billableString + "</strong> minutes on <strong>" + logString + "</strong>";
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
                            h("button", { type: "button", onClick: (e) => { if (window.confirm('Are you sure?'))
                                    scope.DeleteLinkHandler(e); }, class: "btn btn-link btn-sm go-red " + (authorId == currentUserId ? "" : "d-none") }, "Delete"))))),
            h("div", { class: "body", innerHTML: body })));
    }
    static get is() { return "wv-timelog"; }
    static get properties() { return {
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
