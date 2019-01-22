import * as actionType from "./action-types";
export function setDrake(payload) { return dispatch => dispatch({ type: actionType.SET_DRAKE, payload: payload }); }
;
export function addDrakeContainerId(payload) { return dispatch => dispatch({ type: actionType.ADD_DRAKE_CONTAINER_ID, payload: payload }); }
;
export function setActiveNode(payload) { return dispatch => dispatch({ type: actionType.SET_ACTIVE_NODE, payload: payload }); }
;
export function hoverNode(payload) { return dispatch => dispatch({ type: actionType.HOVER_NODE, payload: payload }); }
;
export function hoverContainer(payload) { return dispatch => dispatch({ type: actionType.HOVER_CONTAINER, payload: payload }); }
;
export function setNodeCreation(payload) { return dispatch => dispatch({ type: actionType.SET_NODE_CREATION, payload: payload }); }
;
export function addNode(payload) { return dispatch => dispatch({ type: actionType.ADD_NODE, payload: payload }); }
;
export function removeNode(payload) { return dispatch => dispatch({ type: actionType.REMOVE_NODE, payload: payload }); }
;
export function setOptionsModalState(payload) { return dispatch => dispatch({ type: actionType.SET_OPTIONS_MODAL_STATE, payload: payload }); }
;
export function setHelpModalState(payload) { return dispatch => dispatch({ type: actionType.SET_HELP_MODAL_STATE, payload: payload }); }
;
export function addReloadNodeIds(payload) { return dispatch => dispatch({ type: actionType.ADD_RELOAD_NODE_IDS, payload: payload }); }
;
export function removeReloadNodeIds(payload) { return dispatch => dispatch({ type: actionType.REMOVE_RELOAD_NODE_IDS, payload: payload }); }
;
export function updateNodeOptions(payload) { return dispatch => dispatch({ type: actionType.UPDATE_NODE_OPTIONS, payload: payload }); }
;
export function updatePageNodes(payload) { return dispatch => dispatch({ type: actionType.UPDATE_PAGE_NODES, payload: payload }); }
;
