
// WvSitemapManager: Custom Elements Define Library, ES Module/es2017 Target

import { defineCustomElement } from './wv-sitemap-manager.core.js';
import { COMPONENTS } from './wv-sitemap-manager.components.js';

export function defineCustomElements(win, opts) {
  return defineCustomElement(win, COMPONENTS, opts);
}
