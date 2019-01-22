
// WvLazyload: Custom Elements Define Library, ES Module/es2017 Target

import { defineCustomElement } from './wv-lazyload.core.js';
import { COMPONENTS } from './wv-lazyload.components.js';

export function defineCustomElements(win, opts) {
  return defineCustomElement(win, COMPONENTS, opts);
}
