import * as action from '../../store/actions';
import axios from 'axios';
export class WvDatasourceParamForm {
    constructor() {
        this.datasourceId = null;
    }
    componentWillLoad() {
        let scope = this;
        this.store.mapStateToProps(this, (state) => {
            return {
                datasourceId: state.datasourceId
            };
        });
        this.store.mapDispatchToProps(this, {
            setLibrary: action.setLibrary
        });
        if (!this.store.getState().library || this.store.getState().library.length === 0) {
            let apiUrl = this.store.getState().apiRootUrl;
            let requestConfig = {
                headers: {
                    'Content-Type': 'application/json;charset=UTF-8',
                    "Access-Control-Allow-Origin": "*",
                }
            };
            axios.get(apiUrl, requestConfig)
                .then(function (response) {
                scope.setLibrary(response.data);
            })
                .catch(function (error) {
                console.log(error);
                alert("Error occurred check console");
            });
        }
    }
    libraryVersionUpdate() {
        this.el.forceUpdate();
    }
    render() {
        if (!this.datasourceId) {
            return h("wv-datasource-step1", null);
        }
        else {
            return h("wv-datasource-step2", null);
        }
    }
    static get is() { return "wv-datasource-param-form"; }
    static get properties() { return {
        "datasourceId": {
            "state": true,
            "watchCallbacks": ["libraryVersionUpdate"]
        },
        "el": {
            "elementRef": true
        },
        "store": {
            "context": "store"
        }
    }; }
}
