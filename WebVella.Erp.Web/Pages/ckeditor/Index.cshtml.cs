using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebVella.Erp.Web.Pages
{
    public class JsAdminModel : PageModel
    {

        public JsAdminModel()
        {
            #region << Init LibraryCssFileList >>

            var coreLibCss = new List<string>(new string[] {
            "/jsadmin/plugins/webvella-core/lib/bootstrap/dist/css/bootstrap.css",
            "/jsadmin/plugins/webvella-core/lib/ngtoast/dist/ngtoast.css",
            "/jsadmin/plugins/webvella-core/lib/angular-loading-bar/build/loading-bar.css",
            "/jsadmin/plugins/webvella-core/lib/font-awesome/css/font-awesome.css",
            "/jsadmin/plugins/webvella-core/lib/offline/themes/offline-theme-default.css",
            "/jsadmin/plugins/webvella-core/lib/offline/themes/offline-language-english.css",
            "/jsadmin/plugins/webvella-core/lib/angular-xeditable/dist/css/xeditable.css",
            "/jsadmin/plugins/webvella-core/lib/ng-sortable/dist/ng-sortable.css",
            "/jsadmin/plugins/webvella-core/lib/ng-ckeditor/ng-ckeditor.css"
            });

            LibraryCssFileList.AddRange(coreLibCss);

            #endregion

            #region << Init LibraryJsFileList >>
            var coreLibJs = new List<string>(new string[] {
            "/jsadmin/plugins/webvella-core/lib/ckeditor/ckeditor.js",
            "/jsadmin/plugins/webvella-core/lib/angular/angular.js",
            "/jsadmin/plugins/webvella-core/lib/angular-i18n/angular-locale_en-us.js",
            "/jsadmin/plugins/webvella-core/lib/angular-animate/angular-animate.js",
            "/jsadmin/plugins/webvella-core/lib/angular-sanitize/angular-sanitize.js",
            "/jsadmin/plugins/webvella-core/lib/angular-aria/angular-aria.js",
            "/jsadmin/plugins/webvella-core/lib/angular-messages/angular-messages.js",
            "/jsadmin/plugins/webvella-core/lib/fastclick/lib/fastclick.js",
            "/jsadmin/plugins/webvella-core/lib/angular-cookies/angular-cookies.js",
            "/jsadmin/plugins/webvella-core/lib/angular-ui-router/release/angular-ui-router.js",
            "/jsadmin/plugins/webvella-core/lib/ngtoast/dist/ngToast.js",
            "/jsadmin/plugins/webvella-core/lib/angular-bootstrap/ui-bootstrap-tpls.js",
            "/jsadmin/plugins/webvella-core/lib/angular-xeditable/dist/js/xeditable.js",
            "/jsadmin/plugins/webvella-core/lib/moment/min/moment-with-locales.js",
            "/jsadmin/plugins/webvella-core/lib/ng-sortable/dist/ng-sortable.js",
            "/jsadmin/plugins/webvella-core/lib/checklist-model/checklist-model.js",
            "/jsadmin/plugins/webvella-core/lib/ng-file-upload/ng-file-upload-shim.js",
            "/jsadmin/plugins/webvella-core/lib/ng-file-upload/ng-file-upload.js",
            "/jsadmin/plugins/webvella-core/lib/offline/offline.min.js",
            "/jsadmin/plugins/webvella-core/lib/vendor-init.js",
            "/jsadmin/plugins/webvella-core/lib/ng-ckeditor/ng-ckeditor.js",
            "/jsadmin/plugins/webvella-core/lib/libphonenumber/libphonenumber.js",
            "/jsadmin/plugins/webvella-core/lib/ngstorage/ngstorage.js",
            "/jsadmin/plugins/webvella-core/lib/bootstrap-ui-datetime-picker/dist/datetime-picker.js",
            "/jsadmin/plugins/webvella-core/lib/angular-recursion/angular-recursion.js",
            "/jsadmin/plugins/webvella-core/lib/angular-loading-bar/build/loading-bar.js",
            "/jsadmin/plugins/webvella-core/lib/angular-ui-tree/dist/angular-ui-tree.js",
            "/jsadmin/plugins/webvella-core/lib/angular-bind-html-compile/angular-bind-html-compile.js",
            "/jsadmin/plugins/webvella-core/lib/oclazyload/dist/oclazyload.js",
            "/jsadmin/plugins/webvella-core/lib/ace-builds/src-min-noconflict/ace.js",
            "/jsadmin/plugins/webvella-core/lib/angular-ui-ace/ui-ace.js",
            "/jsadmin/plugins/webvella-core/lib/angular-translate/angular-translate.js",
            "/jsadmin/plugins/webvella-core/lib/moment-angular-date-format-parser/moment-angular-date-format-parser.js",
            "/jsadmin/plugins/webvella-core/lib/lodash/lodash.js"
            });

            LibraryJsFileList.AddRange(coreLibJs);
            #endregion

            #region << Init ModuleCssFileList >>

            var coreModuleCss = new List<string>(new string[] {
                "/jsadmin/plugins/webvella-core/css/core.module.css"
            });
            ModuleCssFileList.AddRange(coreModuleCss);
            #endregion

            #region << Init ModuleJsFileList >>

            var adminModuleJs = new List<string>(new string[] {
                "/jsadmin/plugins/webvella-admin/base.module.js",
                "/jsadmin/plugins/webvella-admin/entities.module.js",
                "/jsadmin/plugins/webvella-admin/topnav.controller.js",
                "/jsadmin/plugins/webvella-admin/sidebar.controller.js",
                "/jsadmin/plugins/webvella-admin/entity-details.module.js",
                "/jsadmin/plugins/webvella-admin/providers/sidebar.factory.js",
                "/jsadmin/plugins/webvella-admin/entity-relations.module.js",
                "/jsadmin/plugins/webvella-admin/entity-view-manage.module.js",
                "/jsadmin/plugins/webvella-admin/entity-view-manage-form.module.js",
                "/jsadmin/plugins/webvella-admin/entity-view-manage-actions.module.js",
                "/jsadmin/plugins/webvella-admin/entity-view-manage-sidebar.module.js",
                "/jsadmin/plugins/webvella-admin/entity-view-manage-data.module.js",
                "/jsadmin/plugins/webvella-admin/file-manager.module.js",
                "/jsadmin/plugins/webvella-admin/entity-fields.module.js",
                "/jsadmin/plugins/webvella-admin/entity-views.module.js",
                "/jsadmin/plugins/webvella-admin/entity-lists.module.js",
                "/jsadmin/plugins/webvella-admin/entity-list-manage.module.js",
                "/jsadmin/plugins/webvella-admin/entity-list-manage-columns.module.js",
                "/jsadmin/plugins/webvella-admin/entity-list-manage-query-sort.module.js",
                "/jsadmin/plugins/webvella-admin/entity-list-manage-actions.module.js",
                "/jsadmin/plugins/webvella-admin/entity-list-manage-data.module.js",
                "/jsadmin/plugins/webvella-admin/entity-trees.module.js",
                "/jsadmin/plugins/webvella-admin/entity-tree-manage.module.js",
                "/jsadmin/plugins/webvella-admin/entity-tree-nodes-manage.module.js",
                "/jsadmin/plugins/webvella-admin/areas.module.js",
                "/jsadmin/plugins/webvella-admin/users.module.js",
                "/jsadmin/plugins/webvella-admin/roles.module.js",
                "/jsadmin/plugins/webvella-admin/schedule-plans.module.js",
                "/jsadmin/plugins/webvella-admin/scheduled-jobs.module.js",
                "/jsadmin/plugins/webvella-admin/system-log.module.js",
                "/jsadmin/plugins/webvella-admin/plugins.module.js",
                "/jsadmin/plugins/webvella-admin/codegen.module.js"
            });

            var desktopModuleJs = new List<string>(new string[] {
                "/jsadmin/plugins/webvella-desktop/base.module.js",
                "/jsadmin/plugins/webvella-desktop/browse.module.js",
                "/jsadmin/plugins/webvella-desktop/providers/desktop.factory.js"
            });

            var areasModuleJs = new List<string>(new string[] {
                "/jsadmin/plugins/webvella-areas/base.module.js",
                "/jsadmin/plugins/webvella-areas/topnav.controller.js",
                "/jsadmin/plugins/webvella-areas/sidebar.controller.js",
                "/jsadmin/plugins/webvella-areas/area-list-general.module.js",
                "/jsadmin/plugins/webvella-areas/area-view-create.module.js",
                "/jsadmin/plugins/webvella-areas/detached-item-sidebar.controller.js",
                "/jsadmin/plugins/webvella-areas/area-view-general.module.js",
                "/jsadmin/plugins/webvella-areas/view-record-sidebar.controller.js",
                "/jsadmin/plugins/webvella-areas/providers/recursive-view.directive.js",
                "/jsadmin/plugins/webvella-areas/providers/recursive-list.directive.js",
                "/jsadmin/plugins/webvella-areas/providers/view-tree-select.directive.js",
                "/jsadmin/plugins/webvella-areas/providers/scroll.directive.js",
                "/jsadmin/plugins/webvella-areas/providers/dynamic-html.directive.js"
            });

            var coreModuleJs = new List<string>(new string[] {
                "/jsadmin/plugins/webvella-core/base.module.js",
                "/jsadmin/plugins/webvella-core/error.module.js",
                "/jsadmin/plugins/webvella-core/login.module.js",
                "/jsadmin/plugins/webvella-core/providers/core.service.js",
                "/jsadmin/plugins/webvella-core/providers/core.directive.js"
            });

            ModuleJsFileList.AddRange(coreModuleJs);
            ModuleJsFileList.AddRange(desktopModuleJs);
            ModuleJsFileList.AddRange(adminModuleJs);
            ModuleJsFileList.AddRange(areasModuleJs);
            #endregion  

            #region << Init AppDependencyInjections >>
            var adminAppDep = new List<string>(new string[] {"webvellaAdmin"});
            var desktopAppDep = new List<string>(new string[] {"webvellaDesktop"});
            var areasAppDep = new List<string>(new string[] {"webvellaAreas"});
            var coreAppDep = new List<string>(new string[] { "webvellaCore", "ngLoadScript" });

            AppDependencyInjections.AddRange(coreAppDep);
            AppDependencyInjections.AddRange(desktopAppDep);
            AppDependencyInjections.AddRange(adminAppDep);
            AppDependencyInjections.AddRange(areasAppDep);
            #endregion  

            LanguageFile = "/jsadmin/plugins/webvella-core/lang/" + Lang + ".js?v=" + CacheBreaker;
        }

        public List<string> LibraryCssFileList { get; private set; } = new List<string>();
        public List<string> ModuleCssFileList { get; private set; } = new List<string>();
        public List<string> LibraryJsFileList { get; private set; } = new List<string>();
        public List<string> ModuleJsFileList { get; private set; } = new List<string>();
        public List<string> AppDependencyInjections { get; private set; } = new List<string>();
        public string DevCssClass { get; private set; } = "debug";
        public string Lang { get; private set; } = "en";
        public string CacheBreaker { get; private set; } = "201710190001";
        public string LanguageFile { get; private set; }
        public bool DevelopmentMode { get; private set; } = true;
        public string CompanyName { get; private set; } = "Tefter.bg";
        public string CompanyLogo { get; private set; } = "";

        public void OnGet()
        {
			//TBD
        }
    }
}