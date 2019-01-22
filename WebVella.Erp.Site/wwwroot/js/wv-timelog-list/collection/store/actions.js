import * as actionType from "./action-types";
export function addTimelog(payload) { return dispatch => dispatch({ type: actionType.ADD_TIMELOG, payload: payload }); }
;
export function removeTimelog(payload) { return dispatch => dispatch({ type: actionType.REMOVE_TIMELOG, payload: payload }); }
;
