import { configureStore } from '../../store/index';
import WvDsManageStore from '../../models/WvDsManageStore';
import * as action from '../../store/actions';
function setDatasourceHandler(scope) {
    let payload = {
        datasourceId: scope.datasourceId,
        pageDatasourceId: scope.pageDatasourceId,
        pageDatasourceName: scope.pageDatasourceName,
        pageDatasourceParams: JSON.parse(scope.pageDatasourceParams)
    };
    scope.setDatasource(payload);
}
export class WvDatasourceManage {
    constructor() {
        this.show = false;
        this.datasourceId = null;
        this.pageDatasourceId = null;
        this.pageDatasourceName = null;
        this.pageDatasourceParams = null;
        this.apiRootUrl = "";
    }
    componentWillLoad() {
        var initStore = new WvDsManageStore();
        initStore.apiRootUrl = this.apiRootUrl;
        initStore.datasourceId = this.datasourceId;
        initStore.pageDatasourceId = this.pageDatasourceId;
        initStore.pageDatasourceName = this.pageDatasourceName;
        initStore.pageDatasourceParams = JSON.parse(this.pageDatasourceParams);
        this.store.setStore(configureStore(initStore));
        this.store.mapDispatchToProps(this, {
            setDatasource: action.setDatasource
        });
    }
    componentWillUpdate() {
        setDatasourceHandler(this);
    }
    render() {
        if (this.show) {
            return h("wv-datasource-param-form", null);
        }
        else {
            return null;
        }
    }
    static get is() { return "wv-datasource-manage"; }
    static get properties() { return {
        "apiRootUrl": {
            "type": String,
            "attr": "api-root-url"
        },
        "datasourceId": {
            "type": String,
            "attr": "datasource-id"
        },
        "pageDatasourceId": {
            "type": String,
            "attr": "page-datasource-id"
        },
        "pageDatasourceName": {
            "type": String,
            "attr": "page-datasource-name"
        },
        "pageDatasourceParams": {
            "type": String,
            "attr": "page-datasource-params"
        },
        "show": {
            "type": Boolean,
            "attr": "show"
        },
        "store": {
            "context": "store"
        }
    }; }
}
