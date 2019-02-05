
// WvRecurrenceTemplate: Custom Elements Define Library, ES Module/es2017 Target

import { defineCustomElement } from './wv-recurrence-template.core.js';
import { COMPONENTS } from './wv-recurrence-template.components.js';

export function defineCustomElements(win, opts) {
  return defineCustomElement(win, COMPONENTS, opts);
}
