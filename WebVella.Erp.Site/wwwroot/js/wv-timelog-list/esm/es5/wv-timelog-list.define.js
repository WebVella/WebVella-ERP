
// WvTimelogList: Custom Elements Define Library, ES Module/es5 Target

import { defineCustomElement } from './wv-timelog-list.core.js';
import { COMPONENTS } from './wv-timelog-list.components.js';

export function defineCustomElements(win, opts) {
  return defineCustomElement(win, COMPONENTS, opts);
}
