
// WvDatasourceManage: Custom Elements Define Library, ES Module/es2017 Target

import { defineCustomElement } from './wv-datasource-manage.core.js';
import { COMPONENTS } from './wv-datasource-manage.components.js';

export function defineCustomElements(win, opts) {
  return defineCustomElement(win, COMPONENTS, opts);
}
