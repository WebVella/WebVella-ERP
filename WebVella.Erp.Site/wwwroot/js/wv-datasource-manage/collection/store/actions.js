import * as actionType from "./action-types";
export function setLibrary(payload) { return dispatch => dispatch({ type: actionType.SET_LIBRARY, payload: payload }); }
;
export function setDatasource(payload) { return dispatch => dispatch({ type: actionType.SET_DATASOURCE, payload: payload }); }
;
