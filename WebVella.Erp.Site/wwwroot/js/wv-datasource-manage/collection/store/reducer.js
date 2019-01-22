import * as actionType from "./action-types";
const initialState = {};
const rootReducer = (state = initialState, action) => {
    let newState = Object.assign({}, state);
    switch (action.type) {
        case actionType.SET_LIBRARY:
            {
                newState.library = action.payload;
                newState.libraryVersion++;
            }
            return newState;
        case actionType.SET_DATASOURCE:
            {
                newState.datasourceId = action.payload.datasourceId;
                newState.pageDatasourceId = action.payload.pageDatasourceId;
                newState.pageDatasourceName = action.payload.pageDatasourceName;
                newState.pageDatasourceParams = action.payload.pageDatasourceParams;
            }
            return newState;
        default:
            return state;
    }
};
export default rootReducer;
