
// WvFeedList: Custom Elements Define Library, ES Module/es5 Target

import { defineCustomElement } from './wv-feed-list.core.js';
import { COMPONENTS } from './wv-feed-list.components.js';

export function defineCustomElements(win, opts) {
  return defineCustomElement(win, COMPONENTS, opts);
}
