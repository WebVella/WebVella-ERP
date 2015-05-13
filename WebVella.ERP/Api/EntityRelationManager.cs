using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Storage;
using WebVella.ERP.Utilities.Dynamic;

namespace WebVella.ERP
{
    public class EntityRelationManager
    {
        public IStorageService StorageService { get; set; }

        public EntityRelationManager(IStorageService storageService)
        {
            StorageService = storageService;
        }
    }
}