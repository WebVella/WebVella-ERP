import moment from 'moment';
export class WvPost {
    constructor() {
        this.record = new Object();
        this.reloadPostIndex = 1;
    }
    componentWillLoad() {
        this.store.mapStateToProps(this, (state) => {
            return {
                reloadPostIndex: state.reloadPostIndex
            };
        });
        this.store.mapDispatchToProps(this, {});
    }
    render() {
        let scope = this;
        let storeState = scope.store.getState();
        let userImagePath = "/assets/avatar.png";
        let userName = "system";
        let hrTime = moment(scope.record["created_on"]).fromNow();
        let timeString = moment(scope.record["created_on"]).format('YYYY MM DD HH:mm');
        let subject = scope.record["subject"];
        let body = scope.record["body"];
        if (scope.record["$user_1n_feed_item"] && scope.record["$user_1n_feed_item"][0]["image"]) {
            userImagePath = "/fs" + scope.record["$user_1n_feed_item"][0]["image"];
        }
        if (scope.record["$user_1n_feed_item"] && scope.record["$user_1n_feed_item"][0]["username"]) {
            userName = scope.record["$user_1n_feed_item"][0]["username"];
        }
        if (storeState.isDebug) {
            userImagePath = "http://localhost:2202" + userImagePath;
        }
        let icon = "";
        switch (scope.record["type"]) {
            case "system":
                icon = "<i class='mr-1 fa fa-cog go-teal'></i>";
                break;
            case "task":
                icon = "<i class='mr-1 fas fa-user-cog go-teal'></i>";
                break;
            case "case":
                icon = "<i class='mr-1 fa fa-file go-teal'></i>";
                break;
            case "timelog":
                icon = "<i class='mr-1 far fa-clock go-teal'></i>";
                break;
            case "comment":
                icon = "<i class='mr-1 far fa-comment go-teal'></i>";
                break;
            default:
                break;
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
                        h("span", { innerHTML: icon }),
                        " ",
                        h("span", { title: timeString }, hrTime)))),
            h("div", { class: "body", innerHTML: body })));
    }
    static get is() { return "wv-feed"; }
    static get properties() { return {
        "record": {
            "type": "Any",
            "attr": "record"
        },
        "reloadPostIndex": {
            "state": true
        },
        "store": {
            "context": "store"
        }
    }; }
}
