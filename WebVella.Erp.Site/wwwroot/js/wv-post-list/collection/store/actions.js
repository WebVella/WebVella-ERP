import * as actionType from "./action-types";
export function addPost(payload) { return dispatch => dispatch({ type: actionType.ADD_POST, payload: payload }); }
;
export function removePost(payload) { return dispatch => dispatch({ type: actionType.REMOVE_POST, payload: payload }); }
;
export function addComment(payload) { return dispatch => dispatch({ type: actionType.ADD_COMMENT, payload: payload }); }
;
export function removeComment(payload) { return dispatch => dispatch({ type: actionType.REMOVE_COMMENT, payload: payload }); }
;
