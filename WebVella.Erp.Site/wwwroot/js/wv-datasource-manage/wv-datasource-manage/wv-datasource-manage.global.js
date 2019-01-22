
(function(namespace,resourcesUrl){"use strict";
(function(Context, resourcesUrl){Context.store=function(){let t;return{getStore:function(){return t},setStore:function(e){t=e},getState:function(){return t&&t.getState()},mapDispatchToProps:function(e,n){Object.keys(n).forEach(o=>{const c=n[o];Object.defineProperty(e,o,{get:()=>(...e)=>c(...e)(t.dispatch,t.getState),configurable:!0,enumerable:!0})})},mapStateToProps:function(e,n){const o=(o,c)=>{const r=n(t.getState());Object.keys(r).forEach(t=>{e[t]=r[t]})};t.subscribe(()=>o()),o()}}}();
})(x,r);
})("WvDatasourceManage");