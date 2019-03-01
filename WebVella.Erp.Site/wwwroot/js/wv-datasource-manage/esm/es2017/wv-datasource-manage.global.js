
export default function appGlobal(n, x, w, d, r, h) {(function(Context, resourcesUrl){Context.store=(()=>{let t;return{getStore:()=>t,setStore:e=>{t=e},getState:()=>t&&t.getState(),mapDispatchToProps:(e,r)=>{Object.keys(r).forEach(o=>{const s=r[o];Object.defineProperty(e,o,{get:()=>(...e)=>t.dispatch(s(...e)),configurable:!0,enumerable:!0})})},mapStateToProps:(e,r)=>{const o=(o,s)=>{const a=r(t.getState());Object.keys(a).forEach(t=>{e[t]=a[t]})},s=t.subscribe(()=>o());return o(),s}}})();
})(x,r);
}