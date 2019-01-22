import * as actionType from "./action-types";
import _ from "lodash";
const initialState = {};
const rootReducer = (state = initialState, action) => {
    let newState = Object.assign({}, state);
    switch (action.type) {
        case actionType.ADD_POST:
            {
                switch (action.payload.mode) {
                    default:
                        newState.posts = [...newState.posts, action.payload.post];
                        break;
                }
            }
            newState.reloadPostIndex++;
            return newState;
        case actionType.REMOVE_POST:
            {
                switch (action.payload.mode) {
                    default:
                        let filteredPosts = _.filter(newState.posts, function (record) { return record.id != action.payload.postId; });
                        newState.posts = [...filteredPosts];
                        break;
                }
            }
            newState.reloadPostIndex++;
            return newState;
        case actionType.ADD_COMMENT:
            {
                switch (action.payload.mode) {
                    default:
                        var postIndex = _.findIndex(newState.posts, function (record) { return record.id === action.payload.comment.parent_id; });
                        if (postIndex === -1) {
                            console.error("postId not found");
                        }
                        else {
                            newState.posts[postIndex]["nodes"] = [...newState.posts[postIndex]["nodes"], action.payload.comment];
                        }
                        break;
                }
            }
            newState.reloadPostIndex++;
            return newState;
        case actionType.REMOVE_COMMENT:
            {
                switch (action.payload.mode) {
                    default:
                        var postIndex = _.findIndex(newState.posts, function (record) { return record.id === action.payload.parentId; });
                        if (postIndex === -1) {
                            console.error("postId not found");
                        }
                        else {
                            let filteredPostNodes = _.filter(newState.posts[postIndex]["nodes"], function (record) { return record.id !== action.payload.commentId; });
                            newState.posts[postIndex]["nodes"] = [...filteredPostNodes];
                        }
                        break;
                }
            }
            newState.reloadPostIndex++;
            return newState;
        default:
            return state;
    }
};
export default rootReducer;
