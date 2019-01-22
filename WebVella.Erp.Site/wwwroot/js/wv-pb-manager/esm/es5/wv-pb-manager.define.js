
// WvPbManager: Custom Elements Define Library, ES Module/es5 Target

import { defineCustomElement } from './wv-pb-manager.core.js';
import { COMPONENTS } from './wv-pb-manager.components.js';

export function defineCustomElements(win, opts) {
  return defineCustomElement(win, COMPONENTS, opts);
}
