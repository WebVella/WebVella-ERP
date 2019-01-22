import * as actionType from "./action-types";
import _ from "lodash";
const initialState = {};
const rootReducer = (state = initialState, action) => {
    let newState = Object.assign({}, state);
    switch (action.type) {
        case actionType.ADD_TIMELOG:
            {
                newState.records.unshift(action.payload.timelog);
                newState.reloadPostIndex++;
            }
            return newState;
        case actionType.REMOVE_TIMELOG:
            {
                let filteredRecords = _.filter(newState.records, function (record) { return record.id != action.payload.timelogId; });
                newState.records = [...filteredRecords];
                newState.reloadPostIndex++;
            }
            return newState;
        default:
            return state;
    }
};
export default rootReducer;
