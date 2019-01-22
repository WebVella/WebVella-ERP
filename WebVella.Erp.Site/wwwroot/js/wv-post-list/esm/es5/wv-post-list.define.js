
// WvPostList: Custom Elements Define Library, ES Module/es5 Target

import { defineCustomElement } from './wv-post-list.core.js';
import { COMPONENTS } from './wv-post-list.components.js';

export function defineCustomElements(win, opts) {
  return defineCustomElement(win, COMPONENTS, opts);
}
