using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Database;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Plugins.SDK.Services
{
    public class CodeGenService
    {
        private EntityManager entMan = new EntityManager();
        private RecordManager recMan = new RecordManager();
        private string OldDbConnectionString;
        private string defaultCulture = "en-US";

        public CodeGenService() : this("en-US")
        {
        }

        public CodeGenService(string defaultCulture)
        {
            this.defaultCulture = defaultCulture;
        }

        public MetaChangeResponseModel EvaluateMetaChanges(string connectionString, List<string> entityRecordsToCompare,
                    bool includeEntityMeta, bool includeEntityRelations, bool includeRoles, bool includeApplications,
                    List<string> NNRelationsRecordsToCompare)
        {
            ValidationException valEx = new ValidationException();

            var response = new MetaChangeResponseModel();
            if (string.IsNullOrEmpty(connectionString))
            {
                valEx.AddError("connectionString", "Connection string is required");
                throw valEx;
            }

            OldDbConnectionString = connectionString;

            var changeRow = new MetaChangeModel();
            //Entity
            var currentEntityList = new List<DbEntity>();
            var oldEntityList = new List<DbEntity>();
            var oldEntityDictionary = new Dictionary<Guid, DbEntity>();
            var oldEntityProcessedDictionary = new Dictionary<Guid, bool>();
            //Field
            var oldEntityFieldsList = new List<DbBaseField>();
            var oldEntityFieldsDictionary = new Dictionary<Guid, DbBaseField>();
            var oldEntityFieldsProcessedDictionary = new Dictionary<Guid, bool>();

            //Relations
            var currentRelationsList = new List<DbEntityRelation>();
            var oldRelationsList = new List<DbEntityRelation>();

            //Roles
            var currentRoleList = new List<EntityRecord>();
            var oldRoleList = new List<EntityRecord>();
            var oldRolesDictionary = new Dictionary<Guid, EntityRecord>();
            var oldRolesProcessedDictionary = new Dictionary<Guid, bool>();

            var queryRole = new EntityQuery("role");

            #region << Get elements >>
            currentEntityList = DbContext.Current.EntityRepository.Read();
            currentRelationsList = DbContext.Current.RelationRepository.Read();
            currentRoleList = DbContext.Current.RecordRepository.Find(queryRole);

            oldEntityList = ReadOldEntities();
            oldRelationsList = ReadOldRelations();
            oldRoleList = ReadOldRoles();
            #endregion

            #region << Init >>
            foreach (var entity in oldEntityList)
            {
                oldEntityDictionary[entity.Id] = entity;
                foreach (var field in entity.Fields)
                {
                    oldEntityFieldsList.Add(field);
                    oldEntityFieldsDictionary[field.Id] = field;
                }
            }
            #endregion

            if (includeEntityRelations)
            {
                //Relations should be deleted before entities
                foreach (var relation in oldRelationsList)
                {
                    if (!currentRelationsList.Any(x => x.Id == relation.Id))
                    {
                        //// DELETED
                        /////////////////////////////////////////////////////
                        changeRow = new MetaChangeModel();
                        changeRow.Element = "relation";
                        changeRow.Type = "deleted";
                        changeRow.Name = relation.Name;
                        response.Changes.Add(changeRow);
                        response.Code += DeleteRelationCode(relation);
                    }
                }
            }

            if (includeEntityMeta)
            {
                #region << Process entity >>

                #region << Logic >>
                foreach (var entity in currentEntityList)
                {
                    if (!oldEntityDictionary.ContainsKey(entity.Id))
                    {
                        //// CREATED
                        /////////////////////////////////////////////////////
                        changeRow = new MetaChangeModel();
                        changeRow.Element = "entity";
                        changeRow.Type = "created";
                        changeRow.Name = entity.Name;
                        response.Changes.Add(changeRow);
                        string entityCode = "";
                        CreateEntityCode(entity, out entityCode);
                        response.Code += entityCode;
                    }
                    else
                    {
                        //// POSSIBLE UPDATE
                        /////////////////////////////////////////////////////
                        var changeCheckResponse = UpdateEntityCode(entity, oldEntityDictionary[entity.Id]);
                        if (changeCheckResponse.HasUpdate)
                        {
                            //1.1 Updated
                            changeRow = new MetaChangeModel();
                            changeRow.Element = "entity";
                            changeRow.Type = "updated";
                            changeRow.Name = entity.Name;
                            changeRow.ChangeList = changeCheckResponse.ChangeList;
                            response.Changes.Add(changeRow);
                            response.Code += changeCheckResponse.Code;
                        }


                        // MARK ID AS PROCESSED
                        /////////////////////////////////////////////////////
                        oldEntityProcessedDictionary[entity.Id] = true;
                    }

                }

                foreach (var entity in oldEntityList)
                {
                    if (!oldEntityProcessedDictionary.ContainsKey(entity.Id))
                    {
                        //// DELETED
                        /////////////////////////////////////////////////////
                        changeRow = new MetaChangeModel();
                        changeRow.Element = "entity";
                        changeRow.Type = "deleted";
                        changeRow.Name = entity.Name;
                        response.Changes.Add(changeRow);
                        response.Code += DeleteEntityCode(entity);
                    }
                }
                #endregion

                #endregion
            }

            if (includeEntityRelations)
            {
                #region << Process relations >>


                foreach (var relation in currentRelationsList)
                {
                    var oldRelation = oldRelationsList.SingleOrDefault(x => x.Id == relation.Id);
                    if (oldRelation == null)
                    {
                        //// CREATED
                        /////////////////////////////////////////////////////
                        var changeCode = CreateRelationCode(relation);
                        changeRow = new MetaChangeModel();
                        changeRow.Element = "relation";
                        changeRow.Type = "created";
                        changeRow.Name = relation.Name;
                        changeRow.ChangeList = new List<string>();
                        if (changeCode == string.Empty)
                        {
                            changeRow.ChangeList.Add(@"<span class='go-gray'>No code will be generated. It is automatically created, in the entity creation process</span>");
                        }
                        response.Changes.Add(changeRow);
                        response.Code += changeCode;
                    }
                    else
                    {
                        //// POSSIBLE UPDATE
                        /////////////////////////////////////////////////////
                        var changeCheckResponse = UpdateRelationCode(relation, oldRelation);
                        if (changeCheckResponse.HasUpdate)
                        {
                            //1.1 Updated
                            changeRow = new MetaChangeModel();
                            changeRow.Element = "relation";
                            changeRow.Type = "updated";
                            changeRow.Name = relation.Name;
                            changeRow.ChangeList = changeCheckResponse.ChangeList;
                            response.Changes.Add(changeRow);
                            response.Code += changeCheckResponse.Code;
                        }
                    }

                }

                #endregion
            }

            if (includeRoles)
            {
                #region << Process roles >>

                #region << Init >>
                foreach (var role in oldRoleList)
                {
                    oldRolesDictionary[(Guid)role["id"]] = role;
                }
                #endregion

                #region << Logic >>
                foreach (var role in currentRoleList)
                {
                    if (!oldRolesDictionary.ContainsKey((Guid)role["id"]))
                    {
                        //// CREATED
                        /////////////////////////////////////////////////////
                        changeRow = new MetaChangeModel();
                        changeRow.Element = "role";
                        changeRow.Type = "created";
                        changeRow.Name = (string)role["name"];
                        response.Changes.Add(changeRow);
                        response.Code += CreateRoleCode(role);
                    }
                    else
                    {
                        //// POSSIBLE UPDATE
                        /////////////////////////////////////////////////////
                        var changeCheckResponse = UpdateRoleCode(role, oldRolesDictionary[(Guid)role["id"]]);
                        if (changeCheckResponse.HasUpdate)
                        {
                            //1.1 Updated
                            changeRow = new MetaChangeModel();
                            changeRow.Element = "role";
                            changeRow.Type = "updated";
                            changeRow.Name = (string)role["name"];
                            changeRow.ChangeList = changeCheckResponse.ChangeList;
                            response.Changes.Add(changeRow);
                            response.Code += changeCheckResponse.Code;
                        }

                        // MARK ID AS PROCESSED
                        /////////////////////////////////////////////////////
                        oldRolesProcessedDictionary[(Guid)role["id"]] = true;
                    }

                }

                foreach (var role in oldRoleList)
                {
                    if (!oldRolesProcessedDictionary.ContainsKey((Guid)role["id"]))
                    {
                        //// DELETED
                        /////////////////////////////////////////////////////
                        changeRow = new MetaChangeModel();
                        changeRow.Element = "role";
                        changeRow.Type = "deleted";
                        changeRow.Name = (string)role["name"];
                        response.Changes.Add(changeRow);
                        response.Code += DeleteRoleCode(role);
                    }
                }

                #endregion

                #endregion
            }

            if (includeApplications)
            {
                #region <-- read/load data -->
                //pages
                var oldPages = ReadOldPages();
                var currentPages = new PageService().GetAll();

                //page body nodes
                var oldBodyNodes = ReadOldPageBodyNodes();
                var currentBodyNodes = ReadCurrentPageBodyNodes();

                //apps
                var oldApps = ReadOldApplications();
                var currentApps = new AppService().GetAllApplications();

                //sitemap areas
                var oldSitemapAreas = ReadOldSitemapAreas();
                var currentSitemapAreas = ReadCurrentSitemapAreas();

                //sitemap groups
                var oldSitemapGroups = ReadOldSitemapGroups();
                var currentSitemapGroups = ReadCurrentSitemapGroups();

                //sitemap nodes
                var oldSitemapNodes = ReadOldSitemapNodes();
                var currentSitemapNodes = ReadCurrentSitemapNodes();

                //datasources
                var oldDataSources = ReadOldDataSources();
                var currentDataSources = ReadCurrentDataSources();

                //page datasources
                var oldPageDataSources = ReadOldPageDataSources();
                var currentPageDataSources = ReadCurrentPageDataSources();

                #endregion

                #region <-- generate create/update code -->

                //create and update apps
                foreach (var app in currentApps)
                {
                    var oldApp = oldApps.SingleOrDefault(x => x.Id == app.Id);
                    if (oldApp == null)
                    {
                        //// CREATED
                        /////////////////////////////////////////////////////
                        changeRow = new MetaChangeModel();
                        changeRow.Element = "app";
                        changeRow.Type = "created";
                        changeRow.Name = app.Name;
                        response.Changes.Add(changeRow);

                        response.Code += CreateAppCode(app);
                    }
                    else
                    {
                        //// POSSIBLE UPDATE
                        /////////////////////////////////////////////////////
                        var changeCheckResponse = UpdateAppCode(app, oldApp);
                        if (changeCheckResponse.HasUpdate)
                        {
                            //1.1 Updated
                            changeRow = new MetaChangeModel();
                            changeRow.Element = "app";
                            changeRow.Type = "updated";
                            changeRow.Name = app.Name;
                            changeRow.ChangeList = changeCheckResponse.ChangeList;
                            response.Changes.Add(changeRow);
                            response.Code += changeCheckResponse.Code;
                        }
                    }
                }

                //create and update sitemap areas
                foreach (var area in currentSitemapAreas)
                {
                    var oldArea = oldSitemapAreas.SingleOrDefault(x => x.Id == area.Id);
                    if (oldArea == null)
                    {
                        //// CREATED
                        /////////////////////////////////////////////////////
                        changeRow = new MetaChangeModel();
                        changeRow.Element = "sitemap area";
                        changeRow.Type = "created";
                        changeRow.Name = $"{area.Name}";
                        response.Changes.Add(changeRow);
                        response.Code += CreateSitemapAreaCode(area);
                    }
                    else
                    {
                        //// POSSIBLE UPDATE
                        /////////////////////////////////////////////////////
                        var changeCheckResponse = UpdateSitemapAreaCode(area, oldArea);
                        if (changeCheckResponse.HasUpdate)
                        {
                            changeRow = new MetaChangeModel();
                            changeRow.Element = "sitemap area";
                            changeRow.Type = "updated";
                            changeRow.Name = $"{area.Name}";
                            changeRow.ChangeList = changeCheckResponse.ChangeList;
                            response.Changes.Add(changeRow);
                            response.Code += changeCheckResponse.Code;
                        }
                    }
                }

                //create and update sitemap groups
                foreach (var group in currentSitemapGroups)
                {
                    var oldGroup = oldSitemapGroups.SingleOrDefault(x => x.Id == group.Id);
                    if (oldGroup == null)
                    {
                        //// CREATED
                        /////////////////////////////////////////////////////
                        changeRow = new MetaChangeModel();
                        changeRow.Element = "sitemap group";
                        changeRow.Type = "created";
                        changeRow.Name = $"{group.Name}";
                        response.Changes.Add(changeRow);

                        Guid areaId = currentSitemapAreas.Where(x => x.Groups.Any(z => z.Id == oldGroup.Id)).First().Id;
                        response.Code += CreateSitemapGroupCode(areaId, group);
                    }
                    else
                    {
                        //// POSSIBLE UPDATE
                        /////////////////////////////////////////////////////
                        var changeCheckResponse = UpdateSitemapGroupCode(group, oldGroup);
                        if (changeCheckResponse.HasUpdate)
                        {
                            changeRow = new MetaChangeModel();
                            changeRow.Element = "sitemap group";
                            changeRow.Type = "updated";
                            changeRow.Name = $"{group.Name}";
                            changeRow.ChangeList = changeCheckResponse.ChangeList;
                            response.Changes.Add(changeRow);
                            response.Code += changeCheckResponse.Code;
                        }
                    }
                }

                //create and update sitemap nodes
                foreach (var node in currentSitemapNodes)
                {
                    var oldNode = oldSitemapNodes.SingleOrDefault(x => x.Id == node.Id);
                    if (oldNode == null)
                    {
                        //// CREATED
                        /////////////////////////////////////////////////////
                        changeRow = new MetaChangeModel();
                        changeRow.Element = "sitemap node";
                        changeRow.Type = "created";
                        changeRow.Name = $"{node.Name}";
                        response.Changes.Add(changeRow);
                        Guid areaId = currentSitemapAreas.Where(x => x.Nodes.Any(z => z.Id == node.Id)).First().Id;
                        response.Code += CreateSitemapNodeCode(areaId, node);
                    }
                    else
                    {
                        //// POSSIBLE UPDATE
                        /////////////////////////////////////////////////////
                        Guid areaId = currentSitemapAreas.Where(x => x.Nodes.Any(z => z.Id == node.Id)).First().Id;
                        var changeCheckResponse = UpdateSitemapNodeCode(areaId, node, oldNode);
                        if (changeCheckResponse.HasUpdate)
                        {
                            changeRow = new MetaChangeModel();
                            changeRow.Element = "sitemap node";
                            changeRow.Type = "updated";
                            changeRow.Name = $"{node.Name}";
                            changeRow.ChangeList = changeCheckResponse.ChangeList;
                            response.Changes.Add(changeRow);
                            response.Code += changeCheckResponse.Code;
                        }
                    }
                }

                //create and update pages
                foreach (var page in currentPages)
                {
                    var oldPage = oldPages.SingleOrDefault(x => x.Id == page.Id);
                    if (oldPage == null)
                    {
                        //// CREATED
                        /////////////////////////////////////////////////////
                        changeRow = new MetaChangeModel();
                        changeRow.Element = "page";
                        changeRow.Type = "created";
                        changeRow.Name = $"{page.Name}({page.Label})";
                        response.Changes.Add(changeRow);

                        response.Code += CreateErpPageCode(page);
                    }
                    else
                    {
                        //// POSSIBLE UPDATE
                        /////////////////////////////////////////////////////
                        var changeCheckResponse = UpdateErpPageCode(page, oldPage);
                        if (changeCheckResponse.HasUpdate)
                        {
                            //1.1 Updated
                            changeRow = new MetaChangeModel();
                            changeRow.Element = "page";
                            changeRow.Type = "updated";
                            changeRow.Name = $"{page.Name}({page.Label})";
                            changeRow.ChangeList = changeCheckResponse.ChangeList;
                            response.Changes.Add(changeRow);
                            response.Code += changeCheckResponse.Code;
                        }
                    }
                }

                //create and update page body nodes
                {
                    Queue<PageBodyNode> queue = new Queue<PageBodyNode>();

                    foreach (var node in currentBodyNodes.Where(x => x.ParentId == null))
                        QueuePageBodyNode(node, currentBodyNodes, queue);

                    //nodes in queue are ordered in that way from parent to child, 
                    //so referential problems during create and update should be eliminated
                    while (queue.Count > 0)
                    {
                        PageBodyNode bodyNode = queue.Dequeue();
                        var page = currentPages.Single(x => x.Id == bodyNode.PageId);
                        var oldBodyNode = oldBodyNodes.SingleOrDefault(x => x.Id == bodyNode.Id);
                        if (oldBodyNode == null)
                        {
                            //// CREATED
                            /////////////////////////////////////////////////////
                            changeRow = new MetaChangeModel();
                            changeRow.Element = "page body node";
                            changeRow.Type = "created";
                            changeRow.Name = $"{bodyNode.Id}({page.Name})";
                            response.Changes.Add(changeRow);
                            response.Code += CreatePageBodyNodeCode(bodyNode, page.Name);
                        }
                        else
                        {
                            //// POSSIBLE UPDATE
                            /////////////////////////////////////////////////////
                            var changeCheckResponse = UpdatePageBodyNodeCode(bodyNode, oldBodyNode, page.Name);
                            if (changeCheckResponse.HasUpdate)
                            {
                                //1.1 Updated
                                changeRow = new MetaChangeModel();
                                changeRow.Element = "page body node";
                                changeRow.Type = "updated";
                                changeRow.Name = $"{bodyNode.Id}({page.Name})";
                                changeRow.ChangeList = changeCheckResponse.ChangeList;
                                response.Changes.Add(changeRow);
                                response.Code += changeCheckResponse.Code;
                            }
                        }
                    }
                }

                //create and update data sources
                foreach (var ds in currentDataSources)
                {
                    var oldDS = oldDataSources.SingleOrDefault(x => x.Id == ds.Id);
                    if (oldDS == null)
                    {
                        //// CREATED
                        /////////////////////////////////////////////////////
                        changeRow = new MetaChangeModel();
                        changeRow.Element = "data source";
                        changeRow.Type = "created";
                        changeRow.Name = $"{ds.Name}";
                        response.Changes.Add(changeRow);
                        response.Code += CreateDatabaseDataSourceCode(ds);
                    }
                    else
                    {
                        //// POSSIBLE UPDATE
                        /////////////////////////////////////////////////////
                        var changeCheckResponse = UpdateDatabaseDataSourceCode(ds, oldDS);
                        if (changeCheckResponse.HasUpdate)
                        {
                            //1.1 Updated
                            changeRow = new MetaChangeModel();
                            changeRow.Element = "data source";
                            changeRow.Type = "updated";
                            changeRow.Name = $"{ds.Name}";
                            changeRow.ChangeList = changeCheckResponse.ChangeList;
                            response.Changes.Add(changeRow);
                            response.Code += changeCheckResponse.Code;
                        }
                    }
                }

                //create and update page datasources
                foreach (var ds in currentPageDataSources)
                {
                    var oldDS = oldPageDataSources.SingleOrDefault(x => x.Id == ds.Id);
                    if (oldDS == null)
                    {
                        //// CREATED
                        /////////////////////////////////////////////////////
                        changeRow = new MetaChangeModel();
                        changeRow.Element = "page data source";
                        changeRow.Type = "created";
                        changeRow.Name = $"{ds.Name}";
                        response.Changes.Add(changeRow);
                        response.Code += CreatePageDataSourceCode(ds);
                    }
                    else
                    {
                        //// POSSIBLE UPDATE
                        /////////////////////////////////////////////////////
                        var changeCheckResponse = UpdatePageDataSourceCode(ds, oldDS);
                        if (changeCheckResponse.HasUpdate)
                        {
                            //1.1 Updated
                            changeRow = new MetaChangeModel();
                            changeRow.Element = "page data source";
                            changeRow.Type = "updated";
                            changeRow.Name = $"{ds.Name}";
                            changeRow.ChangeList = changeCheckResponse.ChangeList;
                            response.Changes.Add(changeRow);
                            response.Code += changeCheckResponse.Code;
                        }
                    }
                }

                #endregion


                #region <-- generate delete code -->

                //delete page data sources
                foreach (var oldDS in oldPageDataSources)
                {
                    if (!currentPageDataSources.Any(x => x.Id == oldDS.Id))
                    {
                        //// DELETED
                        /////////////////////////////////////////////////////
                        changeRow = new MetaChangeModel();
                        changeRow.Element = "page data source";
                        changeRow.Type = "deleted";
                        changeRow.Name = oldDS.Name;
                        response.Changes.Add(changeRow);
                        response.Code += DeletePageDataSourceCode(oldDS);
                    }
                }

                //delete data sources
                foreach (var oldDS in oldDataSources)
                {
                    if (!currentDataSources.Any(x => x.Id == oldDS.Id))
                    {
                        //// DELETED
                        /////////////////////////////////////////////////////
                        changeRow = new MetaChangeModel();
                        changeRow.Element = "data source";
                        changeRow.Type = "deleted";
                        changeRow.Name = oldDS.Name;
                        response.Changes.Add(changeRow);
                        response.Code += DeleteDatabaseDataSourceCode(oldDS);
                    }
                }

                //delete page body nodes
                {
                    Stack<PageBodyNode> deleteStack = new Stack<PageBodyNode>();
                    Queue<PageBodyNode> processQueue = new Queue<PageBodyNode>();

                    foreach (var node in oldBodyNodes.Where(x => x.ParentId == null))
                        processQueue.Enqueue(node);

                    while (processQueue.Count > 0)
                    {
                        PageBodyNode node = processQueue.Dequeue();
                        deleteStack.Push(node);

                        foreach (var childNode in oldBodyNodes.Where(x => x.ParentId == node.Id))
                        {
                            //if deleteStack already contains this nodeId, that mean a cyclic structure exists
                            if (deleteStack.Any(x => x.Id == childNode.Id))
                                throw new Exception($"Cyclic body node structure found between: '{node.Id}' and '{childNode.Id}' .");

                            processQueue.Enqueue(childNode);
                        }
                    }

                    //delete page body nodes
                    while (deleteStack.Count > 0)
                    {
                        var oldBodyNode = deleteStack.Pop();
                        if (!currentBodyNodes.Any(x => x.Id == oldBodyNode.Id))
                        {
                            var page = oldPages.Single(x => x.Id == oldBodyNode.PageId);
                            //// DELETED
                            /////////////////////////////////////////////////////
                            changeRow = new MetaChangeModel();
                            changeRow.Element = "page body node";
                            changeRow.Type = "deleted";
                            changeRow.Name = oldBodyNode.Id.ToString();
                            response.Changes.Add(changeRow);
                            response.Code += DeletePageBodyNodeCode(oldBodyNode, page.Name);
                        }
                    }

                    //we load page body nodes again because delete is recursive
                    //and deleting one node may delete other node which are moved 
                    //to another branch of the nodes tree, such nodes will be
                    //created in code for create and update
                    oldBodyNodes = ReadOldPageBodyNodes();
                    currentBodyNodes = ReadCurrentPageBodyNodes();
                }

                //delete pages
                foreach (var oldPage in oldPages)
                {
                    if (!currentPages.Any(x => x.Id == oldPage.Id))
                    {
                        //// DELETED
                        /////////////////////////////////////////////////////
                        changeRow = new MetaChangeModel();
                        changeRow.Element = "page";
                        changeRow.Type = "deleted";
                        changeRow.Name = $"{oldPage.Name}({oldPage.Label})";
                        response.Changes.Add(changeRow);
                        response.Code += DeleteErpPageCode(oldPage);
                    }
                }

                //delete sitemap area nodes
                foreach (var oldNode in oldSitemapNodes)
                {
                    if (!currentSitemapNodes.Any(x => x.Id == oldNode.Id))
                    {
                        //// DELETED
                        /////////////////////////////////////////////////////
                        changeRow = new MetaChangeModel();
                        changeRow.Element = "sitemap node";
                        changeRow.Type = "deleted";
                        changeRow.Name = oldNode.Name;
                        response.Changes.Add(changeRow);
                        response.Code += DeleteSitemapNodeCode(oldNode);
                    }
                }

                //delete sitemap area groups
                foreach (var oldGroup in oldSitemapGroups)
                {
                    if (!currentSitemapGroups.Any(x => x.Id == oldGroup.Id))
                    {
                        //// DELETED
                        /////////////////////////////////////////////////////
                        changeRow = new MetaChangeModel();
                        changeRow.Element = "sitemap group";
                        changeRow.Type = "deleted";
                        changeRow.Name = oldGroup.Name;
                        response.Changes.Add(changeRow);
                        response.Code += DeleteSitemapGroupCode(oldGroup);
                    }
                }

                //delete sitemap area
                foreach (var oldArea in oldSitemapAreas)
                {
                    if (!currentSitemapAreas.Any(x => x.Id == oldArea.Id))
                    {
                        //// DELETED
                        /////////////////////////////////////////////////////
                        changeRow = new MetaChangeModel();
                        changeRow.Element = "sitemap area";
                        changeRow.Type = "deleted";
                        changeRow.Name = oldArea.Name;
                        response.Changes.Add(changeRow);
                        response.Code += DeleteSitemapAreaCode(oldArea);
                    }
                }

                //delete sitemap group
                foreach (var oldGroup in oldSitemapGroups)
                {
                    if (!currentSitemapGroups.Any(x => x.Id == oldGroup.Id))
                    {
                        //// DELETED
                        /////////////////////////////////////////////////////
                        changeRow = new MetaChangeModel();
                        changeRow.Element = "sitemap group";
                        changeRow.Type = "deleted";
                        changeRow.Name = oldGroup.Name;
                        response.Changes.Add(changeRow);
                        response.Code += DeleteSitemapGroupCode(oldGroup);
                    }
                }

                //delete apps
                foreach (var oldApp in oldApps)
                {
                    if (!currentApps.Any(x => x.Id == oldApp.Id))
                    {
                        //// DELETED
                        /////////////////////////////////////////////////////
                        changeRow = new MetaChangeModel();
                        changeRow.Element = "app";
                        changeRow.Type = "deleted";
                        changeRow.Name = oldApp.Name;
                        response.Changes.Add(changeRow);
                        response.Code += DeleteAppCode(oldApp);
                    }
                }



                #endregion

            }

            if (entityRecordsToCompare != null && entityRecordsToCompare.Count > 0)
            {

                foreach (var id in entityRecordsToCompare)
                {
                    if (id == null)
                        continue;

                    //compare only if entity exists in both databases
                    Guid entityId = new Guid(id);
                    if (oldEntityDictionary.ContainsKey(entityId) && currentEntityList.Any(x => x.Id == entityId))
                    {
                        List<EntityRecord> recordsToCreate = new List<EntityRecord>();
                        List<EntityRecord> recordsToUpdate = new List<EntityRecord>();
                        List<EntityRecord> recordsToDelete = new List<EntityRecord>();

                        DbEntity oldEntity = oldEntityDictionary[entityId];
                        DbEntity currentEntity = currentEntityList.First(x => x.Id == entityId);
                        CompareEntityRecords(oldEntity, currentEntity, recordsToCreate, recordsToUpdate, recordsToDelete);

                        foreach (var rec in recordsToCreate)
                        {
                            changeRow = new MetaChangeModel();
                            changeRow.Element = "record";
                            changeRow.Type = "created";
                            changeRow.Name = $"{rec["id"]} ({currentEntity.Name})";
                            response.Changes.Add(changeRow);
                            response.Code += CreateRecordCode(rec, currentEntity);
                        }

                        foreach (var rec in recordsToUpdate)
                        {
                            changeRow = new MetaChangeModel();
                            changeRow.Element = "record";
                            changeRow.Type = "updated";
                            changeRow.Name = $"{rec["id"]} ({currentEntity.Name})";
                            response.Changes.Add(changeRow);
                            response.Code += UpdateRecordCode(rec, currentEntity);
                        }

                        foreach (var rec in recordsToDelete)
                        {
                            changeRow = new MetaChangeModel();
                            changeRow.Element = "record";
                            changeRow.Type = "deleted";
                            changeRow.Name = $"{rec["id"]} ({currentEntity.Name})";
                            response.Changes.Add(changeRow);
                            response.Code += DeleteRecordCode(rec, currentEntity);
                        }
                    }
                    else if (!oldEntityDictionary.ContainsKey(entityId) && currentEntityList.Any(x => x.Id == entityId))
                    {
                        DbEntity currentEntity = currentEntityList.First(x => x.Id == entityId);
                        var currentRecords = recMan.Find(new EntityQuery(currentEntity.Name)).Object.Data;
                        foreach (var rec in currentRecords)
                        {
                            changeRow = new MetaChangeModel();
                            changeRow.Element = "record";
                            changeRow.Type = "created";
                            changeRow.Name = $"{rec["id"]} ({currentEntity.Name})";
                            response.Changes.Add(changeRow);
                            response.Code += CreateRecordCode(rec, currentEntity);
                        }
                    }


                }
            }

            if (NNRelationsRecordsToCompare != null && NNRelationsRecordsToCompare.Count > 0)
            {

                foreach (var id in NNRelationsRecordsToCompare)
                {
                    if (id == null)
                        continue;

                    //compare only if relation exists in both databases
                    Guid relationId = new Guid(id);
                    var relation = new EntityRelationManager().Read(relationId).Object;
                    if (relation == null)
                        throw new Exception("Relation not found");

                    List<DatabaseNNRelationRecord> recordsToCreate = new List<DatabaseNNRelationRecord>();
                    List<DatabaseNNRelationRecord> recordsToDelete = new List<DatabaseNNRelationRecord>();

                    var oldRelationRecords = ReadOldNNRelationRecords(relation);
                    var currentRelationRecords = ReadCurrentNNRelationRecords(relation);

                    //Create all records = existing in current but not in old
                    foreach (var relRecord in currentRelationRecords)
                    {
                        if (!oldRelationRecords.Any(x => x.OriginId == relRecord.OriginId && x.TargetId == relRecord.TargetId))
                        {
                            recordsToCreate.Add(relRecord);
                        }
                    }

                    //Delete all records = existing in old but not in current
                    foreach (var relRecord in oldRelationRecords)
                    {
                        if (!currentRelationRecords.Any(x => x.OriginId == relRecord.OriginId && x.TargetId == relRecord.TargetId))
                        {
                            recordsToDelete.Add(relRecord);
                        }
                    }


                    foreach (var rec in recordsToCreate)
                    {
                        changeRow = new MetaChangeModel();
                        changeRow.Element = "relation record";
                        changeRow.Type = "created";
                        changeRow.Name = $"{relation.Name}";
                        changeRow.ChangeList = new List<string> { $"{rec.OriginId} <> {rec.TargetId}" };
                        response.Changes.Add(changeRow);
                        response.Code += CreateNNRelationRecordCode(relation, rec.OriginId, rec.TargetId);
                    }

                    foreach (var rec in recordsToDelete)
                    {
                        changeRow = new MetaChangeModel();
                        changeRow.Element = "relation record";
                        changeRow.Type = "deleted";
                        changeRow.Name = $"{relation.Name}";
                        changeRow.ChangeList = new List<string> { $"{rec.OriginId} <> {rec.TargetId}" };
                        response.Changes.Add(changeRow);
                        response.Code += DeleteNNRelationRecordCode(relation, rec.OriginId, rec.TargetId);
                    }
                }

            }

            return response;
        }

        #region << Read data >>

        private List<DbEntity> ReadOldEntities()
        {
            using (NpgsqlConnection con = new NpgsqlConnection(OldDbConnectionString))
            {
                try
                {
                    con.Open();
                    NpgsqlCommand command = new NpgsqlCommand("SELECT json FROM entities;", con);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {

                        JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
                        List<DbEntity> entities = new List<DbEntity>();
                        while (reader.Read())
                        {
                            DbEntity entity = JsonConvert.DeserializeObject<DbEntity>(reader[0].ToString(), settings);
                            entities.Add(entity);
                        }



                        reader.Close();
                        return entities;
                    }
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private List<DbEntityRelation> ReadOldRelations()
        {
            using (NpgsqlConnection con = new NpgsqlConnection(OldDbConnectionString))
            {
                try
                {
                    con.Open();
                    NpgsqlCommand command = new NpgsqlCommand("SELECT json FROM entity_relations;", con);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {

                        JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
                        List<DbEntityRelation> relations = new List<DbEntityRelation>();
                        while (reader.Read())
                        {
                            DbEntityRelation relation = JsonConvert.DeserializeObject<DbEntityRelation>(reader[0].ToString(), settings);
                            relations.Add(relation);
                        }



                        reader.Close();
                        return relations;
                    }
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private List<EntityRecord> ReadOldEntityRecords(string entityName)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(OldDbConnectionString))
            {
                try
                {
                    con.Open();
                    NpgsqlCommand command = new NpgsqlCommand($"SELECT * FROM rec_{entityName};", con);
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table.AsRecordList();
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private List<EntityRecord> ReadOldRoles()
        {
            using (NpgsqlConnection con = new NpgsqlConnection(OldDbConnectionString))
            {
                try
                {
                    con.Open();
                    NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM rec_role;", con);
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table.AsRecordList();
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private List<App> ReadOldApplications()
        {
            AppService appService = new AppService(OldDbConnectionString);
            return appService.GetAllApplications(useCache: false);
        }

        private List<ErpPage> ReadOldPages()
        {
            PageService pageService = new PageService(OldDbConnectionString);
            return pageService.GetAll(useCache: false);
        }

        private List<PageBodyNode> ReadOldPageBodyNodes(Guid pageId)
        {
            PageService pageService = new PageService(OldDbConnectionString);
            return pageService.GetPageNodes(pageId);
        }

        private List<SitemapArea> ReadOldSitemapAreas()
        {
            AppService appService = new AppService(OldDbConnectionString);
            var apps = appService.GetAllApplications(useCache: false);
            List<SitemapArea> areas = new List<SitemapArea>();
            foreach (var app in apps)
            {
                if (app.Sitemap != null && app.Sitemap.Areas != null)
                    areas.AddRange(app.Sitemap.Areas);
            }
            return areas;
        }

        private List<SitemapArea> ReadCurrentSitemapAreas()
        {
            AppService appService = new AppService();
            var apps = appService.GetAllApplications();
            List<SitemapArea> areas = new List<SitemapArea>();
            foreach (var app in apps)
            {
                if (app.Sitemap != null && app.Sitemap.Areas != null)
                    areas.AddRange(app.Sitemap.Areas);
            }
            return areas;
        }

        private List<SitemapGroup> ReadOldSitemapGroups()
        {
            AppService appService = new AppService(OldDbConnectionString);
            var apps = appService.GetAllApplications(useCache: false);
            List<SitemapGroup> groups = new List<SitemapGroup>();
            foreach (var app in apps)
            {
                if (app.Sitemap != null && app.Sitemap.Areas != null)
                {
                    foreach (var area in app.Sitemap.Areas)
                        if (area.Groups != null)
                            groups.AddRange(area.Groups);
                }
            }
            return groups;
        }

        private List<SitemapGroup> ReadCurrentSitemapGroups()
        {
            AppService appService = new AppService();
            var apps = appService.GetAllApplications();
            List<SitemapGroup> groups = new List<SitemapGroup>();
            foreach (var app in apps)
            {
                if (app.Sitemap != null && app.Sitemap.Areas != null)
                {
                    foreach (var area in app.Sitemap.Areas)
                        if (area.Groups != null)
                            groups.AddRange(area.Groups);
                }
            }
            return groups;
        }

        private List<SitemapNode> ReadOldSitemapNodes()
        {
            AppService appService = new AppService(OldDbConnectionString);
            var apps = appService.GetAllApplications(useCache: false);
            List<SitemapNode> nodes = new List<SitemapNode>();
            foreach (var app in apps)
            {
                if (app.Sitemap != null && app.Sitemap.Areas != null)
                {
                    foreach (var area in app.Sitemap.Areas)
                        if (area.Nodes != null)
                            nodes.AddRange(area.Nodes);
                }
            }
            return nodes;
        }

        private List<SitemapNode> ReadCurrentSitemapNodes()
        {
            AppService appService = new AppService();
            var apps = appService.GetAllApplications();
            List<SitemapNode> nodes = new List<SitemapNode>();
            foreach (var app in apps)
            {
                if (app.Sitemap != null && app.Sitemap.Areas != null)
                {
                    foreach (var area in app.Sitemap.Areas)
                        if (area.Nodes != null)
                            nodes.AddRange(area.Nodes);
                }
            }
            return nodes;
        }

        private List<PageBodyNode> ReadOldPageBodyNodes()
        {
            PageService pageService = new PageService(OldDbConnectionString);
            return pageService.GetAllBodyNodes();
        }

        private List<PageBodyNode> ReadCurrentPageBodyNodes()
        {
            PageService pageService = new PageService();
            return pageService.GetAllBodyNodes();
        }

        private void QueuePageBodyNode(PageBodyNode bodyNode, List<PageBodyNode> allBodyNodes, Queue<PageBodyNode> queue)
        {
            queue.Enqueue(bodyNode);

            foreach (var childNode in allBodyNodes.Where(x => x.ParentId == bodyNode.Id))
                QueuePageBodyNode(childNode, allBodyNodes, queue);


        }

        private List<DatabaseDataSource> ReadCurrentDataSources()
        {
            using (DbConnection con = DbContext.Current.CreateConnection())
            {
                var command = con.CreateCommand(@"SELECT * FROM public.data_source");
                DataTable dt = new DataTable();
                new NpgsqlDataAdapter(command).Fill(dt);

                List<DatabaseDataSource> result = new List<DatabaseDataSource>();
                foreach (DataRow row in dt.Rows)
                    result.Add(row.MapTo<DatabaseDataSource>());

                return result;
            }
        }

        private List<DatabaseDataSource> ReadOldDataSources()
        {
            using (NpgsqlConnection con = new NpgsqlConnection(OldDbConnectionString))
            {
                try
                {
                    con.Open();
                    var command = new NpgsqlCommand(@"SELECT * FROM public.data_source", con);
                    DataTable dt = new DataTable();
                    new NpgsqlDataAdapter(command).Fill(dt);

                    List<DatabaseDataSource> result = new List<DatabaseDataSource>();
                    foreach (DataRow row in dt.Rows)
                        result.Add(row.MapTo<DatabaseDataSource>());

                    return result;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private List<PageDataSource> ReadCurrentPageDataSources()
        {
            using (DbConnection con = DbContext.Current.CreateConnection())
            {
                var command = con.CreateCommand(@"SELECT * FROM app_page_data_source");
                DataTable dt = new DataTable();
                new NpgsqlDataAdapter(command).Fill(dt);

                List<PageDataSource> result = new List<PageDataSource>();
                foreach (DataRow row in dt.Rows)
                    result.Add(row.MapTo<PageDataSource>());

                return result;
            }
        }

        private List<PageDataSource> ReadOldPageDataSources()
        {
            using (NpgsqlConnection con = new NpgsqlConnection(OldDbConnectionString))
            {
                try
                {
                    con.Open();
                    var command = new NpgsqlCommand(@"SELECT * FROM app_page_data_source", con);
                    DataTable dt = new DataTable();
                    new NpgsqlDataAdapter(command).Fill(dt);

                    List<PageDataSource> result = new List<PageDataSource>();
                    foreach (DataRow row in dt.Rows)
                        result.Add(row.MapTo<PageDataSource>());

                    return result;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private List<DatabaseNNRelationRecord> ReadCurrentNNRelationRecords(EntityRelation relation)
        {
            using (DbConnection con = DbContext.Current.CreateConnection())
            {
                var command = con.CreateCommand($"SELECT * FROM public.rel_{relation.Name}");
                DataTable dt = new DataTable();
                new NpgsqlDataAdapter(command).Fill(dt);

                List<DatabaseNNRelationRecord> result = new List<DatabaseNNRelationRecord>();
                foreach (DataRow row in dt.Rows)
                    result.Add(row.MapTo<DatabaseNNRelationRecord>());

                return result;
            }
        }

        private List<DatabaseNNRelationRecord> ReadOldNNRelationRecords(EntityRelation relation)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(OldDbConnectionString))
            {
                try
                {
                    con.Open();
                    //As relation tables are created after the first relation creation, we need first to check
                    //if the table exists

                    var teCommand = new NpgsqlCommand($"SELECT EXISTS(SELECT FROM pg_tables WHERE schemaname = 'public' AND tablename = 'rel_{relation.Name}');", con);
                    DataTable dt1 = new DataTable();
                    new NpgsqlDataAdapter(teCommand).Fill(dt1);
                    bool isTableExists = false;
                    if (dt1.Rows.Count > 0)
                    {
                        isTableExists = dt1.Rows[0].ItemArray[0].MapTo<bool>();
                    }

                    if (!isTableExists)
                        return new List<DatabaseNNRelationRecord>();

                    teCommand.Cancel();

                    var command = new NpgsqlCommand($"SELECT * FROM public.rel_{relation.Name}", con);
                    DataTable dt = new DataTable();
                    new NpgsqlDataAdapter(command).Fill(dt);

                    List<DatabaseNNRelationRecord> result = new List<DatabaseNNRelationRecord>();
                    foreach (DataRow row in dt.Rows)
                        result.Add(row.MapTo<DatabaseNNRelationRecord>());

                    return result;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        #endregion

        #region << Entity >>


        private void CreateEntityCode(DbEntity entity, out string entityResponse)
        {
            entityResponse = "";
            //escape some possible quotes
            if (entity.Label != null)
                entity.Label = entity.Label.Replace("\"", "\\\"");

            var response = "" +
$"#region << ***Create entity*** Entity name: {entity.Name} >>\n" +
"{\n" +
    "\t#region << entity >>\n" +
    "\t{\n" +
        "\t\tvar entity = new InputEntity();\n" +
        "\t\tvar systemFieldIdDictionary = new Dictionary<string,Guid>();\n" +
        //Generate system fields
        $"\t\tsystemFieldIdDictionary[\"id\"] = new Guid(\"{entity.Fields.Single(x => x.Name == "id").Id}\");\n";
            response +=
            $"\t\tentity.Id = new Guid(\"{entity.Id}\");\n" +
            $"\t\tentity.Name = \"{entity.Name}\";\n" +
            $"\t\tentity.Label = \"{entity.Label}\";\n" +
            $"\t\tentity.LabelPlural = \"{entity.LabelPlural}\";\n" +
            $"\t\tentity.System = {(entity.System).ToString().ToLowerInvariant()};\n" +
            $"\t\tentity.IconName = \"{entity.IconName}\";\n" +
            $"\t\tentity.Color = \"{entity.Color}\";\n" +
            (entity.RecordScreenIdField.HasValue ? $"\t\tentity.RecordScreenIdField = new Guid(\"{entity.RecordScreenIdField}\");\n" : $"\t\tentity.RecordScreenIdField = null;\n") +
            "\t\tentity.RecordPermissions = new RecordPermissions();\n" +
            "\t\tentity.RecordPermissions.CanCreate = new List<Guid>();\n" +
            "\t\tentity.RecordPermissions.CanRead = new List<Guid>();\n" +
            "\t\tentity.RecordPermissions.CanUpdate = new List<Guid>();\n" +
            "\t\tentity.RecordPermissions.CanDelete = new List<Guid>();\n" +
            "\t\t//Create\n";
            foreach (var permId in entity.RecordPermissions.CanCreate)
            {
                response += $"\t\tentity.RecordPermissions.CanCreate.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t\t//READ\n";
            foreach (var permId in entity.RecordPermissions.CanRead)
            {
                response += $"\t\tentity.RecordPermissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t\t//UPDATE\n";
            foreach (var permId in entity.RecordPermissions.CanUpdate)
            {
                response += $"\t\tentity.RecordPermissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t\t//DELETE\n";
            foreach (var permId in entity.RecordPermissions.CanDelete)
            {
                response += $"\t\tentity.RecordPermissions.CanDelete.Add(new Guid(\"{permId}\"));\n";
            }

            response += "\t\t{\n" +
                "\t\t\tvar response = entMan.CreateEntity(entity, systemFieldIdDictionary);\n" +
                "\t\t\tif (!response.Success)\n" +
                    $"\t\t\t\tthrow new Exception(\"System error 10050. Entity: {entity.Name} creation Message: \" + response.Message);\n" +
            "\t\t}\n" +
        "\t}\n" +
        "\t#endregion\n" +
    "}\n" +
    "#endregion\n\n";

            //foreach field generate createField and add

            foreach (var field in entity.Fields)
            {
                if (field.Name == "id")
                    continue;

                response += CreateFieldCode(field, entity.Id, entity.Name);
            }

            entityResponse = response;
        }

        private UpdateCheckResponse UpdateEntityCode(DbEntity currentEntity, DbEntity oldEntity)
        {
            var response = new UpdateCheckResponse();

            #region << General >>
            var changeGeneralResponse = UpdateGeneralEntityCode(currentEntity, oldEntity);
            if (changeGeneralResponse.HasUpdate)
            {
                response.HasUpdate = true;
                foreach (var change in changeGeneralResponse.ChangeList)
                {
                    response.ChangeList.Add(change);
                }
                response.Code += changeGeneralResponse.Code;
            }
            #endregion

            #region << fields >>
            //prepare the old fields dictionary
            var entityOldFieldsDictionary = new Dictionary<Guid, DbBaseField>();
            var entityProcessedFieldsDictionary = new Dictionary<Guid, bool>();
            foreach (var field in oldEntity.Fields)
            {
                entityOldFieldsDictionary[field.Id] = field;
            }

            foreach (var field in currentEntity.Fields)
            {
                if (!entityOldFieldsDictionary.ContainsKey(field.Id))
                {
                    if (field.Name != "id")
                    {
                        //// CREATED
                        /////////////////////////////////////////////////////
                        response.HasUpdate = true;
                        response.ChangeList.Add($"<span class='go-green label-block'>field</span>  new field <span class='go-red'>{field.Name}</span> was created.</span>");
                        response.Code += CreateFieldCode(field, currentEntity.Id, currentEntity.Name);
                    }

                    // MARK ID AS PROCESSED
                    /////////////////////////////////////////////////////
                    entityProcessedFieldsDictionary[field.Id] = true;
                }
                else
                {
                    //// POSSIBLE UPDATE
                    /////////////////////////////////////////////////////		
                    var changeCheckResponse = UpdateFieldCode(field, entityOldFieldsDictionary[field.Id], currentEntity);
                    if (changeCheckResponse.HasUpdate)
                    {
                        response.HasUpdate = true;
                        foreach (var change in changeCheckResponse.ChangeList)
                        {
                            response.ChangeList.Add(change);
                        }
                        response.Code += changeCheckResponse.Code;
                    }

                    // MARK ID AS PROCESSED
                    /////////////////////////////////////////////////////
                    entityProcessedFieldsDictionary[field.Id] = true;
                }
            }
            foreach (var field in oldEntity.Fields)
            {
                if (!entityProcessedFieldsDictionary.ContainsKey(field.Id))
                {
                    //// DELETED
                    /////////////////////////////////////////////////////
                    response.HasUpdate = true;
                    response.ChangeList.Add($"<span class='go-green label-block'>field</span>  field <span class='go-red'>{field.Name}</span> was deleted.</span>");
                    response.Code += DeleteFieldCode(field, currentEntity.Id, currentEntity.Name);
                }
            }
            #endregion

            return response;
        }

        private string DeleteEntityCode(DbEntity entity)
        {
            var response =
        $"#region << ***Delete entity*** Entity Name: {entity.Name} >>\n" +
        "{\n" +
            "\t{\n" +
                $"\t\tvar response = entMan.DeleteEntity(new Guid(\"{entity.Id}\"));\n" +
                $"\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entity.Name} Delete. Message:\" + response.Message);\n" +
            "\t}\n" +

        "}\n" +
        "#endregion\n\n";

            return response;
        }

        private UpdateCheckResponse UpdateGeneralEntityCode(DbEntity currentEntity, DbEntity oldEntity)
        {
            var response = new UpdateCheckResponse();
            string code = string.Empty;
            var hasUpdate = false;
            //escape possible double quotes
            if (currentEntity.Label != null)
                currentEntity.Label = currentEntity.Label.Replace("\"", "\\\"");

            //escape the old entity the same way so the update check is correct
            if (oldEntity.Label != null)
                oldEntity.Label = oldEntity.Label.Replace("\"", "\\\"");

            #region << General details >>
            code =
            $"#region << ***Update entity*** Entity name: {currentEntity.Name} >>\n" +
            "{\n" +
                "\tvar updateObject = new InputEntity();\n" +
                $"\tupdateObject.Id = new Guid(\"{currentEntity.Id}\");\n";
            //name
            if (currentEntity.Name != oldEntity.Name)
            {
                hasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>name</span>  from <span class='go-red'>{oldEntity.Name}</span> to <span class='go-red'>{currentEntity.Name}</span>");
            }
            code += $"\tupdateObject.Name = \"{currentEntity.Name}\";\n";

            //label
            if (currentEntity.Label != oldEntity.Label)
            {
                hasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>label</span>  from <span class='go-red'>{oldEntity.Label}</span> to <span class='go-red'>{currentEntity.Label}</span>");
            }
            code += $"\tupdateObject.Label = \"{currentEntity.Label}\";\n";

            //LabelPlural
            if (currentEntity.LabelPlural != oldEntity.LabelPlural)
            {
                hasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>LabelPlural</span>  from <span class='go-red'>{oldEntity.LabelPlural}</span> to <span class='go-red'>{currentEntity.LabelPlural}</span>");
            }
            code += $"\tupdateObject.LabelPlural = \"{currentEntity.LabelPlural}\";\n";

            //System
            if (currentEntity.System != oldEntity.System)
            {
                hasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>System</span>  from <span class='go-red'>{oldEntity.System}</span> to <span class='go-red'>{currentEntity.System}</span>");
            }
            code += $"\tupdateObject.System = {(currentEntity.System).ToString().ToLowerInvariant()};\n";

            //IconName
            if (currentEntity.IconName != oldEntity.IconName)
            {
                hasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>IconName</span>  from <span class='go-red'>{oldEntity.IconName}</span> to <span class='go-red'>{currentEntity.IconName}</span>");
            }
            code += $"\tupdateObject.IconName = \"{currentEntity.IconName}\";\n";

            //Color
            var currentColor = "";
            var oldColor = "";
            if (!String.IsNullOrWhiteSpace(currentEntity.Color))
            {
                currentColor = currentEntity.Color;
            }
            if (!String.IsNullOrWhiteSpace(oldEntity.Color))
            {
                oldColor = oldEntity.Color;
            }
            if (currentColor != oldColor)
            {
                hasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>Color</span>  from <span class='go-red'>{oldColor}</span> to <span class='go-red'>{currentColor}</span>");
            }
            code += $"\tupdateObject.Color = \"{currentColor}\";\n";

            //RecordScreenIdField
            if (currentEntity.RecordScreenIdField != oldEntity.RecordScreenIdField)
            {
                hasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>IconName</span>  from <span class='go-red'>{oldEntity.RecordScreenIdField}</span> to <span class='go-red'>{currentEntity.RecordScreenIdField}</span>");
            }
            code += (currentEntity.RecordScreenIdField.HasValue ? $"\t\tupdateObject.RecordScreenIdField = new Guid(\"{currentEntity.RecordScreenIdField}\");\n" : $"\t\tupdateObject.RecordScreenIdField = null;\n");

            //RecordPermissions
            var recordPermissionsChanged = false;
            var oldCanReadDictionary = new Dictionary<Guid, bool>();
            var oldCanCreateDictionary = new Dictionary<Guid, bool>();
            var oldCanUpdateDictionary = new Dictionary<Guid, bool>();
            var oldCanDeleteDictionary = new Dictionary<Guid, bool>();

            #region << prepare old status dictionaries >>
            foreach (var permission in oldEntity.RecordPermissions.CanRead)
            {
                oldCanReadDictionary[permission] = true;
            }

            foreach (var permission in oldEntity.RecordPermissions.CanCreate)
            {
                oldCanCreateDictionary[permission] = true;
            }

            foreach (var permission in oldEntity.RecordPermissions.CanUpdate)
            {
                oldCanUpdateDictionary[permission] = true;
            }

            foreach (var permission in oldEntity.RecordPermissions.CanDelete)
            {
                oldCanDeleteDictionary[permission] = true;
            }
            #endregion

            #region << Check for change and generate permissions code >>
            code += $"\tupdateObject.RecordPermissions = new RecordPermissions();\n" +
            $"\tupdateObject.RecordPermissions.CanRead = new List<Guid>();\n" +
            $"\tupdateObject.RecordPermissions.CanCreate = new List<Guid>();\n" +
            $"\tupdateObject.RecordPermissions.CanUpdate = new List<Guid>();\n" +
            $"\tupdateObject.RecordPermissions.CanDelete = new List<Guid>();\n";

            foreach (var permission in currentEntity.RecordPermissions.CanRead)
            {
                if (!oldCanReadDictionary.ContainsKey(permission))
                {
                    recordPermissionsChanged = true;
                }
                code += $"\tupdateObject.RecordPermissions.CanRead.Add(new Guid(\"{permission}\"));\n";
            }

            foreach (var permission in currentEntity.RecordPermissions.CanCreate)
            {
                if (!oldCanCreateDictionary.ContainsKey(permission))
                {
                    recordPermissionsChanged = true;
                }
                code += $"\tupdateObject.RecordPermissions.CanCreate.Add(new Guid(\"{permission}\"));\n";
            }

            foreach (var permission in currentEntity.RecordPermissions.CanUpdate)
            {
                if (!oldCanUpdateDictionary.ContainsKey(permission))
                {
                    recordPermissionsChanged = true;
                }
                code += $"\tupdateObject.RecordPermissions.CanUpdate.Add(new Guid(\"{permission}\"));\n";
            }

            foreach (var permission in currentEntity.RecordPermissions.CanDelete)
            {
                if (!oldCanDeleteDictionary.ContainsKey(permission))
                {
                    recordPermissionsChanged = true;
                }
                code += $"\tupdateObject.RecordPermissions.CanDelete.Add(new Guid(\"{permission}\"));\n";
            }
            if (recordPermissionsChanged)
            {
                hasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>permissions</span>  record permissions were changes</span>");
            }

            #endregion

            code +=
                "\tvar updateEntityResult = entMan.UpdateEntity(updateObject);\n" +
                "\tif (!updateEntityResult.Success)\n" +
                "\t{\n" +
                    $"\t\tthrow new Exception(\"System error 10060. Entity update with name : {oldEntity.Name}. Message:\" + updateEntityResult.Message);\n" +
                "\t}\n" +
            "}\n" +
            "#endregion\n\n";

            #endregion

            response.Code = code;
            response.HasUpdate = hasUpdate;
            return response;
        }
        #endregion

        #region << Field >>

        private string CreateFieldCode(DbBaseField field, Guid entityId, string entityName)
        {
            var response = "";
            //escape quotes where they can possible be
            if (field.HelpText != null)
                field.HelpText = field.HelpText.Replace("\"", "\\\"");
            if (field.PlaceholderText != null)
                field.PlaceholderText = field.PlaceholderText.Replace("\"", "\\\"");
            if (field.Label != null)
                field.Label = field.Label.Replace("\"", "\\\"");
            if (field.Description != null)
                field.Description = field.Description.Replace("\"", "\\\"");
            switch (field.GetFieldType())
            {
                case FieldType.AutoNumberField:
                    response += CreateAutoNumberFieldCode(field as DbAutoNumberField, entityId, entityName);
                    break;
                case FieldType.CheckboxField:
                    response += CreateCheckboxFieldCode(field as DbCheckboxField, entityId, entityName);
                    break;
                case FieldType.CurrencyField:
                    response += CreateCurrencyFieldCode(field as DbCurrencyField, entityId, entityName);
                    break;
                case FieldType.DateField:
                    response += CreateDateFieldCode(field as DbDateField, entityId, entityName);
                    break;
                case FieldType.DateTimeField:
                    response += CreateDateTimeFieldCode(field as DbDateTimeField, entityId, entityName);
                    break;
                case FieldType.EmailField:
                    response += CreateEmailFieldCode(field as DbEmailField, entityId, entityName);
                    break;
                case FieldType.FileField:
                    response += CreateFileFieldCode(field as DbFileField, entityId, entityName);
                    break;
                case FieldType.ImageField:
                    response += CreateImageFieldCode(field as DbImageField, entityId, entityName);
                    break;
                case FieldType.HtmlField:
                    response += CreateHtmlFieldCode(field as DbHtmlField, entityId, entityName);
                    break;
                case FieldType.MultiLineTextField:
                    response += CreateMultiLineTextFieldCode(field as DbMultiLineTextField, entityId, entityName);
                    break;
                case FieldType.GeographyField:
                    response += CreateGeographyFieldCode(field as DbGeographyField, entityId, entityName);
                    break;
                case FieldType.MultiSelectField:
                    response += CreateMultiSelectFieldCode(field as DbMultiSelectField, entityId, entityName);
                    break;
                case FieldType.NumberField:
                    response += CreateNumberFieldCode(field as DbNumberField, entityId, entityName);
                    break;
                case FieldType.PasswordField:
                    response += CreatePasswordFieldCode(field as DbPasswordField, entityId, entityName);
                    break;
                case FieldType.PercentField:
                    response += CreatePercentFieldCode(field as DbPercentField, entityId, entityName);
                    break;
                case FieldType.PhoneField:
                    response += CreatePhoneFieldCode(field as DbPhoneField, entityId, entityName);
                    break;
                case FieldType.GuidField:
                    response += CreateGuidFieldCode(field as DbGuidField, entityId, entityName);
                    break;
                case FieldType.SelectField:
                    response += CreateSelectFieldCode(field as DbSelectField, entityId, entityName);
                    break;
                case FieldType.TextField:
                    response += CreateTextFieldCode(field as DbTextField, entityId, entityName);
                    break;
                case FieldType.UrlField:
                    response += CreateUrlFieldCode(field as DbUrlField, entityId, entityName);
                    break;
            }

            return response;
        }

        private string CreateAutoNumberFieldCode(DbAutoNumberField field, Guid entityId, string entityName)
        {
            var response = string.Empty;
            response =
            $"#region << ***Create field*** Entity: {entityName} Field Name: {field.Name} >>\n" +
            "{\n" +
            "\tInputAutoNumberField autonumberField = new InputAutoNumberField();\n" +
            $"\tautonumberField.Id = new Guid(\"{field.Id}\");\n" +
            $"\tautonumberField.Name = \"{field.Name}\";\n" +
            $"\tautonumberField.Label = \"{field.Label}\";\n";
            if (field.PlaceholderText == null)
            {
                response += $"\tautonumberField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tautonumberField.PlaceholderText = \"{field.PlaceholderText}\";\n";
            }
            if (field.Description == null)
            {
                response += $"\tautonumberField.Description = null;\n";
            }
            else
            {
                response += $"\tautonumberField.Description = \"{field.Description}\";\n";
            }
            if (field.HelpText == null)
            {
                response += $"\tautonumberField.HelpText = null;\n";
            }
            else
            {
                response += $"\tautonumberField.HelpText = \"{field.HelpText}\";\n";
            }

            response +=
            $"\tautonumberField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
            $"\tautonumberField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
            $"\tautonumberField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\tautonumberField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\tautonumberField.System = {(field.System).ToString().ToLowerInvariant()};\n";
            if (field.DefaultValue == null)
            {
                response += $"\tautonumberField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\tautonumberField.DefaultValue = Decimal.Parse(\"{field.DefaultValue}\");\n";
            }
            if (field.DisplayFormat == null)
            {
                response += $"\tautonumberField.DisplayFormat = null;\n";
            }
            else
            {
                response += $"\tautonumberField.DisplayFormat = \"{field.DisplayFormat}\";\n";
            }
            if (field.StartingNumber == null)
            {
                response += $"\tautonumberField.StartingNumber = null;\n";
            }
            else
            {
                response += $"\tautonumberField.StartingNumber = Decimal.Parse(\"{field.StartingNumber}\");\n";
            }

            response +=
            $"\tautonumberField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            "\tautonumberField.Permissions = new FieldPermissions();\n" +
            "\tautonumberField.Permissions.CanRead = new List<Guid>();\n" +
            "\tautonumberField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";

            foreach (var permId in field.Permissions.CanRead)
            {
                response += $"\tautonumberField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in field.Permissions.CanUpdate)
            {
                response += $"\tautonumberField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), autonumberField, false);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";

            return response;
        }

        private string CreateCheckboxFieldCode(DbCheckboxField field, Guid entityId, string entityName)
        {
            var response = string.Empty;
            response =
            $"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
            "{\n" +
                "\tInputCheckboxField checkboxField = new InputCheckboxField();\n" +
                $"\tcheckboxField.Id = new Guid(\"{field.Id}\");\n" +
                $"\tcheckboxField.Name = \"{field.Name}\";\n" +
                $"\tcheckboxField.Label = \"{field.Label}\";\n";
            if (field.PlaceholderText == null)
            {
                response += $"\tcheckboxField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tcheckboxField.PlaceholderText = \"{field.PlaceholderText}\";\n";
            }
            if (field.Description == null)
            {
                response += $"\tcheckboxField.Description = null;\n";
            }
            else
            {
                response += $"\tcheckboxField.Description = \"{field.Description}\";\n";
            }
            if (field.HelpText == null)
            {
                response += $"\tcheckboxField.HelpText = null;\n";
            }
            else
            {
                response += $"\tcheckboxField.HelpText = \"{field.HelpText}\";\n";
            }

            response +=
            $"\tcheckboxField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
            $"\tcheckboxField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
            $"\tcheckboxField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\tcheckboxField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\tcheckboxField.System = {(field.System).ToString().ToLowerInvariant()};\n" +
            $"\tcheckboxField.DefaultValue = {(field.DefaultValue).ToString().ToLowerInvariant()};\n" +
            $"\tcheckboxField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            "\tcheckboxField.Permissions = new FieldPermissions();\n" +
            "\tcheckboxField.Permissions.CanRead = new List<Guid>();\n" +
            "\tcheckboxField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in field.Permissions.CanRead)
            {
                response += $"\tcheckboxField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in field.Permissions.CanUpdate)
            {
                response += $"\tcheckboxField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), checkboxField, false);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";

            return response;
        }

        private string CreateCurrencyFieldCode(DbCurrencyField field, Guid entityId, string entityName)
        {
            var response = string.Empty;
            response =
            $"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
            "{\n" +
            "\tInputCurrencyField currencyField = new InputCurrencyField();\n" +
            $"\tcurrencyField.Id = new Guid(\"{field.Id}\");\n" +
            $"\tcurrencyField.Name = \"{field.Name}\";\n" +
            $"\tcurrencyField.Label =  \"{field.Label}\";\n";
            if (field.PlaceholderText == null)
            {
                response += $"\tcurrencyField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tcurrencyField.PlaceholderText = \"{field.PlaceholderText}\";\n";
            }
            if (field.Description == null)
            {
                response += $"\tcurrencyField.Description = null;\n";
            }
            else
            {
                response += $"\tcurrencyField.Description = \"{field.Description}\";\n";
            }
            if (field.HelpText == null)
            {
                response += $"\tcurrencyField.HelpText = null;\n";
            }
            else
            {
                response += $"\tcurrencyField.HelpText = \"{field.HelpText}\";\n";
            }

            response +=
            $"\tcurrencyField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
            $"\tcurrencyField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
            $"\tcurrencyField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\tcurrencyField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\tcurrencyField.System = {(field.System).ToString().ToLowerInvariant()};\n";
            if (field.DefaultValue == null)
            {
                response += $"\tcurrencyField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\tcurrencyField.DefaultValue = Decimal.Parse(\"{field.DefaultValue}\");\n";
            }
            if (field.MinValue == null)
            {
                response += $"\tcurrencyField.MinValue = null;\n";
            }
            else
            {
                response += $"\tcurrencyField.MinValue = Decimal.Parse(\"{field.MinValue}\");\n";
            }
            if (field.MaxValue == null)
            {
                response += $"\tcurrencyField.MaxValue = null;\n";
            }
            else
            {
                response += $"\tcurrencyField.MaxValue = Decimal.Parse(\"{field.MaxValue}\");\n";
            }

            response +=
            $"\tcurrencyField.Currency = WebVella.Erp.Utilities.Helpers.GetCurrencyType(\"{field.Currency.Code}\");\n" +
            $"\tcurrencyField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            "\tcurrencyField.Permissions = new FieldPermissions();\n" +
            "\tcurrencyField.Permissions.CanRead = new List<Guid>();\n" +
            "\tcurrencyField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in field.Permissions.CanRead)
            {
                response += $"\tcurrencyField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in field.Permissions.CanUpdate)
            {
                response += $"\tcurrencyField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), currencyField, false);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";


            return response;
        }

        private string CreateDateFieldCode(DbDateField field, Guid entityId, string entityName)
        {
            var response = string.Empty;
            response =

        $"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
        "{\n" +
            "\tInputDateField dateField = new InputDateField();\n" +
            $"\tdateField.Id =  new Guid(\"{field.Id}\");\n" +
            $"\tdateField.Name = \"{field.Name}\";\n" +
            $"\tdateField.Label = \"{field.Label}\";\n";
            if (field.PlaceholderText == null)
            {
                response += $"\tdateField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tdateField.PlaceholderText = \"{field.PlaceholderText}\";\n";
            }
            if (field.Description == null)
            {
                response += $"\tdateField.Description = null;\n";
            }
            else
            {
                response += $"\tdateField.Description = \"{field.Description}\";\n";
            }
            if (field.HelpText == null)
            {
                response += $"\tdateField.HelpText = null;\n";
            }
            else
            {
                response += $"\tdateField.HelpText = \"{field.HelpText}\";\n";
            }

            response +=
            $"\tdateField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
            $"\tdateField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
            $"\tdateField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\tdateField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\tdateField.System = {(field.System).ToString().ToLowerInvariant()};\n";
            if (field.UseCurrentTimeAsDefaultValue || field.DefaultValue == null)
            {
                response += $"\tdateField.DefaultValue = null;\n";
            }
            else
            {
                response += ($"\ttry{{ dateField.DefaultValue = DateTime.Parse(\"{field.DefaultValue}\"); }}" +
                    $"catch{{ dateField.DefaultValue = DateTime.Parse(\"{field.DefaultValue}\", new CultureInfo(\"{defaultCulture}\") ); }}\n");
            }
            if (field.Format == null)
            {
                response += $"\tdateField.Format = null;\n";
            }
            else
            {
                response += $"\tdateField.Format = \"{field.Format}\";\n";
            }

            response +=
            $"\tdateField.UseCurrentTimeAsDefaultValue = {(field.UseCurrentTimeAsDefaultValue).ToString().ToLowerInvariant()};\n" +
            $"\tdateField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            "\tdateField.Permissions = new FieldPermissions();\n" +
            "\tdateField.Permissions.CanRead = new List<Guid>();\n" +
            "\tdateField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";

            foreach (var permId in field.Permissions.CanRead)
            {
                response += $"\tdateField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in field.Permissions.CanUpdate)
            {
                response += $"\tdateField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), dateField, false);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";


            return response;
        }

        private string CreateDateTimeFieldCode(DbDateTimeField field, Guid entityId, string entityName)
        {
            var response = string.Empty;
            response =

$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
"{\n" +
    "\tInputDateTimeField datetimeField = new InputDateTimeField();\n" +
    $"\tdatetimeField.Id =  new Guid(\"{field.Id}\");\n" +
    $"\tdatetimeField.Name = \"{field.Name}\";\n" +
    $"\tdatetimeField.Label = \"{field.Label}\";\n";
            if (field.PlaceholderText == null)
            {
                response += $"\tdatetimeField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tdatetimeField.PlaceholderText = \"{field.PlaceholderText}\";\n";
            }
            if (field.Description == null)
            {
                response += $"\tdatetimeField.Description = null;\n";
            }
            else
            {
                response += $"\tdatetimeField.Description = \"{field.Description}\";\n";
            }
            if (field.HelpText == null)
            {
                response += $"\tdatetimeField.HelpText = null;\n";
            }
            else
            {
                response += $"\tdatetimeField.HelpText = \"{field.HelpText}\";\n";
            }

            response +=
            $"\tdatetimeField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
            $"\tdatetimeField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
            $"\tdatetimeField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\tdatetimeField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\tdatetimeField.System = {(field.System).ToString().ToLowerInvariant()};\n";
            if (field.UseCurrentTimeAsDefaultValue || field.DefaultValue == null)
            {
                response += $"\tdatetimeField.DefaultValue = null;\n";
            }
            else
            {

                response += ($"\ttry{{ datetimeField.DefaultValue = DateTime.Parse(\"{field.DefaultValue}\"); }}" +
                    $"catch{{ datetimeField.DefaultValue = DateTime.Parse(\"{field.DefaultValue}\", new CultureInfo(\"{defaultCulture}\") ); }}\n");
            }
            if (field.Format == null)
            {
                response += $"\tdatetimeField.Format = null;\n";
            }
            else
            {
                response += $"\tdatetimeField.Format = \"{field.Format}\";\n";
            }

            response +=
            $"\tdatetimeField.UseCurrentTimeAsDefaultValue = {(field.UseCurrentTimeAsDefaultValue).ToString().ToLowerInvariant()};\n" +
            $"\tdatetimeField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            "\tdatetimeField.Permissions = new FieldPermissions();\n" +
            "\tdatetimeField.Permissions.CanRead = new List<Guid>();\n" +
            "\tdatetimeField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";

            foreach (var permId in field.Permissions.CanRead)
            {
                response += $"\tdatetimeField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in field.Permissions.CanUpdate)
            {
                response += $"\tdatetimeField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), datetimeField, false);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";


            return response;
        }

        private string CreateEmailFieldCode(DbEmailField field, Guid entityId, string entityName)
        {
            var response = string.Empty;
            response =

$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
"{\n" +
    "\tInputEmailField emailField = new InputEmailField();\n" +
    $"\temailField.Id = new Guid(\"{field.Id}\");\n" +
    $"\temailField.Name = \"{field.Name}\";\n" +
    $"\temailField.Label = \"{field.Label}\";\n";
            if (field.PlaceholderText == null)
            {
                response += $"\temailField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\temailField.PlaceholderText = \"{field.PlaceholderText}\";\n";
            }
            if (field.Description == null)
            {
                response += $"\temailField.Description = null;\n";
            }
            else
            {
                response += $"\temailField.Description = \"{field.Description}\";\n";
            }
            if (field.HelpText == null)
            {
                response += $"\temailField.HelpText = null;\n";
            }
            else
            {
                response += $"\temailField.HelpText = \"{field.HelpText}\";\n";
            }

            response +=
            $"\temailField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
            $"\temailField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
            $"\temailField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\temailField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\temailField.System = {(field.System).ToString().ToLowerInvariant()};\n";
            if (field.DefaultValue == null)
            {
                response += $"\temailField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\temailField.DefaultValue = \"{field.DefaultValue}\";\n";
            }
            if (field.MaxLength == null)
            {
                response += $"\temailField.MaxLength = null;\n";
            }
            else
            {
                response += $"\temailField.MaxLength = Int32.Parse(\"{field.MaxLength}\");\n";
            }
            response +=
            $"\temailField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\temailField.Permissions = new FieldPermissions();\n" +
            $"\temailField.Permissions.CanRead = new List<Guid>();\n" +
            $"\temailField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in field.Permissions.CanRead)
            {
                response += $"\temailField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in field.Permissions.CanUpdate)
            {
                response += $"\temailField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), emailField, false);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";


            return response;
        }

        private string CreateFileFieldCode(DbFileField field, Guid entityId, string entityName)
        {
            var response = "";

            response =
$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
"{\n" +
    $"\tInputFileField fileField = new InputFileField();\n" +
    $"\tfileField.Id = new Guid(\"{field.Id}\");\n" +
    $"\tfileField.Name = \"{field.Name}\";\n" +
    $"\tfileField.Label = \"{field.Label}\";\n";
            if (field.PlaceholderText == null)
            {
                response += $"\tfileField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tfileField.PlaceholderText = \"{field.PlaceholderText}\";\n";
            }
            if (field.Description == null)
            {
                response += $"\tfileField.Description = null;\n";
            }
            else
            {
                response += $"\tfileField.Description = \"{field.Description}\";\n";
            }
            if (field.HelpText == null)
            {
                response += $"\tfileField.HelpText = null;\n";
            }
            else
            {
                response += $"\tfileField.HelpText = \"{field.HelpText}\";\n";
            }

            response +=
            $"\tfileField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
            $"\tfileField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
            $"\tfileField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\tfileField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\tfileField.System = {(field.System).ToString().ToLowerInvariant()};\n";
            if (field.DefaultValue == null)
            {
                response += $"\tfileField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\tfileField.DefaultValue =\"{field.DefaultValue}\";\n";
            }
            response +=
            $"\tfileField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\tfileField.Permissions = new FieldPermissions();\n" +
            $"\tfileField.Permissions.CanRead = new List<Guid>();\n" +
            $"\tfileField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in field.Permissions.CanRead)
            {
                response += $"\tfileField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in field.Permissions.CanUpdate)
            {
                response += $"\tfileField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), fileField, false);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";

            return response;
        }

        private string CreateHtmlFieldCode(DbHtmlField field, Guid entityId, string entityName)
        {
            var response = "";

            response =
$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
"{\n" +
    $"\tInputHtmlField htmlField = new InputHtmlField();\n" +
    $"\thtmlField.Id = new Guid(\"{field.Id}\");\n" +
    $"\thtmlField.Name = \"{field.Name}\";\n" +
    $"\thtmlField.Label = \"{field.Label}\";\n";
            if (field.PlaceholderText == null)
            {
                response += $"\thtmlField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\thtmlField.PlaceholderText = \"{field.PlaceholderText}\";\n";
            }
            if (field.Description == null)
            {
                response += $"\thtmlField.Description = null;\n";
            }
            else
            {
                response += $"\thtmlField.Description = \"{field.Description}\";\n";
            }
            if (field.HelpText == null)
            {
                response += $"\thtmlField.HelpText = null;\n";
            }
            else
            {
                response += $"\thtmlField.HelpText = \"{field.HelpText}\";\n";
            }

            response +=
            $"\thtmlField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
            $"\thtmlField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
            $"\thtmlField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\thtmlField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\thtmlField.System = {(field.System).ToString().ToLowerInvariant()};\n";
            if (field.DefaultValue == null)
            {
                response += $"\thtmlField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\thtmlField.DefaultValue = \"{field.DefaultValue}\";\n";
            }
            response +=
            $"\thtmlField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\thtmlField.Permissions = new FieldPermissions();\n" +
            $"\thtmlField.Permissions.CanRead = new List<Guid>();\n" +
            $"\thtmlField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in field.Permissions.CanRead)
            {
                response += $"\thtmlField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in field.Permissions.CanUpdate)
            {
                response += $"\thtmlField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), htmlField, false);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";
            return response;
        }

        private string CreateImageFieldCode(DbImageField field, Guid entityId, string entityName)
        {
            var response = "";
            response =
            $"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
            "{\n" +
            $"\tInputImageField imageField = new InputImageField();\n" +
            $"\timageField.Id = new Guid(\"{field.Id}\");\n" +
            $"\timageField.Name = \"{field.Name}\";\n" +
            $"\timageField.Label = \"{field.Label}\";\n";
            if (field.PlaceholderText == null)
            {
                response += $"\timageField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\timageField.PlaceholderText = \"{field.PlaceholderText}\";\n";
            }
            if (field.Description == null)
            {
                response += $"\timageField.Description = null;\n";
            }
            else
            {
                response += $"\timageField.Description = \"{field.Description}\";\n";
            }
            if (field.HelpText == null)
            {
                response += $"\timageField.HelpText = null;\n";
            }
            else
            {
                response += $"\timageField.HelpText = \"{field.HelpText}\";\n";
            }

            response +=
            $"\timageField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
            $"\timageField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
            $"\timageField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\timageField.Auditable =  {(field.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\timageField.System = {(field.System).ToString().ToLowerInvariant()};\n";
            if (field.DefaultValue == null)
            {
                response += $"\timageField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\timageField.DefaultValue =\"{field.DefaultValue}\";\n";
            }
            response +=
            $"\timageField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\timageField.Permissions = new FieldPermissions();\n" +
            $"\timageField.Permissions.CanRead = new List<Guid>();\n" +
            $"\timageField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in field.Permissions.CanRead)
            {
                response += $"\timageField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in field.Permissions.CanUpdate)
            {
                response += $"\timageField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), imageField, false);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";
            return response;
        }
        private string CreateGeographyFieldCode(DbGeographyField field, Guid entityId, string entityName)
        {
            var response = "";
            response =
$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
"{\n" +
    $"\tInputGeographyField geometryField = new InputGeographyField();\n" +
    $"\tgeometryField.Id = new Guid(\"{field.Id}\");\n" +
    $"\tgeometryField.Name = \"{field.Name}\";\n" +
    $"\tgeometryField.Label = \"{field.Label}\";\n";
            if (field.PlaceholderText == null)
            {
                response += $"\tgeometryField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tgeometryField.PlaceholderText = \"{field.PlaceholderText}\";\n";
            }
            if (field.Description == null)
            {
                response += $"\tgeometryField.Description = null;\n";
            }
            else
            {
                response += $"\tgeometryField.Description = \"{field.Description}\";\n";
            }
            if (field.HelpText == null)
            {
                response += $"\tgeometryField.HelpText = null;\n";
            }
            else
            {
                response += $"\tgeometryField.HelpText = \"{field.HelpText}\";\n";
            }

            response +=
            $"\tgeometryField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
            $"\tgeometryField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
            $"\tgeometryField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\tgeometryField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\tgeometryField.System = {(field.System).ToString().ToLowerInvariant()};\n";

            if (field.Format.HasValue)
            {
                response +=
                    $"\tgeometryField.Format = WebVella.Erp.Api.Models.GeographyFieldFormat.{field.Format.Value};\n";
            }
            else
            {
                response +=
                    $"\tgeometryField.Format = WebVella.Erp.Api.Models.GeographyFieldFormat.GeoJSON;\n";
            }
            response +=
                $"\tgeometryField.SRID = {field.SRID};\n";

            if (field.DefaultValue == null)
            {
                response += $"\tgeometryField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\tgeometryField.DefaultValue = \"{field.DefaultValue}\";\n";
            }
            if (field.MaxLength == null)
            {
                response += $"\tgeometryField.MaxLength = null;\n";
            }
            else
            {
                response += $"\tgeometryField.MaxLength = Int32.Parse(\"{field.MaxLength}\");\n";
            }
            if (field.VisibleLineNumber == null)
            {
                response += $"\tgeometryField.VisibleLineNumber = null;\n";
            }
            else
            {
                response += $"\tgeometryField.VisibleLineNumber = Int32.Parse(\"{field.VisibleLineNumber}\");\n";
            }
            response +=
            $"\tgeometryField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\tgeometryField.Permissions = new FieldPermissions();\n" +
            $"\tgeometryField.Permissions.CanRead = new List<Guid>();\n" +
            $"\tgeometryField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in field.Permissions.CanRead)
            {
                response += $"\tgeometryField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in field.Permissions.CanUpdate)
            {
                response += $"\tgeometryField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), geometryField, false);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";

            return response;
        }
        private string CreateMultiLineTextFieldCode(DbMultiLineTextField field, Guid entityId, string entityName)
        {
            var response = "";
            response =
$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
"{\n" +
    $"\tInputMultiLineTextField textareaField = new InputMultiLineTextField();\n" +
    $"\ttextareaField.Id = new Guid(\"{field.Id}\");\n" +
    $"\ttextareaField.Name = \"{field.Name}\";\n" +
    $"\ttextareaField.Label = \"{field.Label}\";\n";
            if (field.PlaceholderText == null)
            {
                response += $"\ttextareaField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\ttextareaField.PlaceholderText = \"{field.PlaceholderText}\";\n";
            }
            if (field.Description == null)
            {
                response += $"\ttextareaField.Description = null;\n";
            }
            else
            {
                response += $"\ttextareaField.Description = \"{field.Description}\";\n";
            }
            if (field.HelpText == null)
            {
                response += $"\ttextareaField.HelpText = null;\n";
            }
            else
            {
                response += $"\ttextareaField.HelpText = \"{field.HelpText}\";\n";
            }

            response +=
            $"\ttextareaField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
            $"\ttextareaField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
            $"\ttextareaField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\ttextareaField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\ttextareaField.System = {(field.System).ToString().ToLowerInvariant()};\n";
            if (field.DefaultValue == null)
            {
                response += $"\ttextareaField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\ttextareaField.DefaultValue = \"{field.DefaultValue}\";\n";
            }
            if (field.MaxLength == null)
            {
                response += $"\ttextareaField.MaxLength = null;\n";
            }
            else
            {
                response += $"\ttextareaField.MaxLength = Int32.Parse(\"{field.MaxLength}\");\n";
            }
            if (field.VisibleLineNumber == null)
            {
                response += $"\ttextareaField.VisibleLineNumber = null;\n";
            }
            else
            {
                response += $"\ttextareaField.VisibleLineNumber = Int32.Parse(\"{field.VisibleLineNumber}\");\n";
            }
            response +=
            $"\ttextareaField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\ttextareaField.Permissions = new FieldPermissions();\n" +
            $"\ttextareaField.Permissions.CanRead = new List<Guid>();\n" +
            $"\ttextareaField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in field.Permissions.CanRead)
            {
                response += $"\ttextareaField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in field.Permissions.CanUpdate)
            {
                response += $"\ttextareaField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), textareaField, false);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";

            return response;
        }

        private string CreateMultiSelectFieldCode(DbMultiSelectField field, Guid entityId, string entityName)
        {
            var response = "";
            response =
$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
"{\n" +
    $"\tInputMultiSelectField multiSelectField = new InputMultiSelectField();\n" +
    $"\tmultiSelectField.Id = new Guid(\"{field.Id}\");\n" +
    $"\tmultiSelectField.Name = \"{field.Name}\";\n" +
    $"\tmultiSelectField.Label = \"{field.Label}\";\n";
            if (field.PlaceholderText == null)
            {
                response += $"\tmultiSelectField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tmultiSelectField.PlaceholderText = \"{field.PlaceholderText}\";\n";
            }
            if (field.Description == null)
            {
                response += $"\tmultiSelectField.Description = null;\n";
            }
            else
            {
                response += $"\tmultiSelectField.Description = \"{field.Description}\";\n";
            }
            if (field.HelpText == null)
            {
                response += $"\tmultiSelectField.HelpText = null;\n";
            }
            else
            {
                response += $"\tmultiSelectField.HelpText = \"{field.HelpText}\";\n";
            }

            response +=
    $"\tmultiSelectField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
    $"\tmultiSelectField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
    $"\tmultiSelectField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
    $"\tmultiSelectField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
    $"\tmultiSelectField.System = {(field.System).ToString().ToLowerInvariant()};\n" +
    "\tmultiSelectField.DefaultValue = ";
            var defaultValues = field.DefaultValue.ToList();
            if (defaultValues.Count > 0)
            {
                response += "new List<string>() {";

                for (int i = 0; i < defaultValues.Count; i++)
                {
                    response += "\"" + defaultValues[i] + "\"";
                    if (i < defaultValues.Count - 1)
                    {
                        response += ",";
                    }
                }
                response += "};\n";
            }
            else
            {
                response += "null;\n";
            }
            response += "\tmultiSelectField.Options = ";
            var fieldOptions = field.Options.ToList();
            if (fieldOptions.Count > 0)
            {
                response += "new List<SelectOption>\n\t{\n";
                for (int i = 0; i < fieldOptions.Count; i++)
                {
                    response += $"\t\tnew SelectOption() {{ Label = \"{fieldOptions[i].Label}\", Value = \"{fieldOptions[i].Value}\" , IconClass = \"{fieldOptions[i].IconClass}\", Color = \"{fieldOptions[i].Color}\"}}";
                    if (i < fieldOptions.Count - 1)
                    {
                        response += ",\n";
                    }
                }
                response += "\n\t};\n";
            }
            else
            {
                response += "null;\n";
            }
            response +=

            $"\tmultiSelectField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\tmultiSelectField.Permissions = new FieldPermissions();\n" +
            $"\tmultiSelectField.Permissions.CanRead = new List<Guid>();\n" +
            $"\tmultiSelectField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in field.Permissions.CanRead)
            {
                response += $"\tmultiSelectField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in field.Permissions.CanUpdate)
            {
                response += $"\tmultiSelectField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), multiSelectField, false);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";

            return response;
        }

        private string CreateNumberFieldCode(DbNumberField field, Guid entityId, string entityName)
        {
            var response = string.Empty;
            response =
            $"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
            "{\n" +
                $"\tInputNumberField numberField = new InputNumberField();\n" +
                $"\tnumberField.Id = new Guid(\"{field.Id}\");\n" +
                $"\tnumberField.Name = \"{field.Name}\";\n" +
                $"\tnumberField.Label = \"{field.Label}\";\n";
            if (field.PlaceholderText == null)
            {
                response += $"\tnumberField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tnumberField.PlaceholderText = \"{field.PlaceholderText}\";\n";
            }
            if (field.Description == null)
            {
                response += $"\tnumberField.Description = null;\n";
            }
            else
            {
                response += $"\tnumberField.Description = \"{field.Description}\";\n";
            }
            if (field.HelpText == null)
            {
                response += $"\tnumberField.HelpText = null;\n";
            }
            else
            {
                response += $"\tnumberField.HelpText = \"{field.HelpText}\";\n";
            }

            response +=
            $"\tnumberField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
            $"\tnumberField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
            $"\tnumberField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\tnumberField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\tnumberField.System = {(field.System).ToString().ToLowerInvariant()};\n";
            if (field.DefaultValue == null)
            {
                response += $"\tnumberField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\tnumberField.DefaultValue = Decimal.Parse(\"{field.DefaultValue}\");\n";
            }
            if (field.MinValue == null)
            {
                response += $"\tnumberField.MinValue = null;\n";
            }
            else
            {
                response += $"\tnumberField.MinValue = Decimal.Parse(\"{field.MinValue}\");\n";
            }
            if (field.MaxValue == null)
            {
                response += $"\tnumberField.MaxValue = null;\n";
            }
            else
            {
                response += $"\tnumberField.MaxValue = Decimal.Parse(\"{field.MaxValue}\");\n";
            }
            response += $"\tnumberField.DecimalPlaces = byte.Parse(\"{field.DecimalPlaces}\");\n";
            response +=

            $"\tnumberField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\tnumberField.Permissions = new FieldPermissions();\n" +
            $"\tnumberField.Permissions.CanRead = new List<Guid>();\n" +
            $"\tnumberField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in field.Permissions.CanRead)
            {
                response += $"\tnumberField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in field.Permissions.CanUpdate)
            {
                response += $"\tnumberField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), numberField, false);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";

            return response;
        }

        private string CreatePasswordFieldCode(DbPasswordField field, Guid entityId, string entityName)
        {
            var response = string.Empty;
            response =
            $"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
            "{\n" +
                $"\tInputPasswordField passwordField = new InputPasswordField();\n" +
                $"\tpasswordField.Id = new Guid(\"{field.Id}\");\n" +
                $"\tpasswordField.Name = \"{field.Name}\";\n" +
                $"\tpasswordField.Label = \"{field.Label}\";\n";
            if (field.PlaceholderText == null)
            {
                response += $"\tpasswordField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tpasswordField.PlaceholderText = \"{field.PlaceholderText}\";\n";
            }
            if (field.Description == null)
            {
                response += $"\tpasswordField.Description = null;\n";
            }
            else
            {
                response += $"\tpasswordField.Description = \"{field.Description}\";\n";
            }
            if (field.HelpText == null)
            {
                response += $"\tpasswordField.HelpText = null;\n";
            }
            else
            {
                response += $"\tpasswordField.HelpText = \"{field.HelpText}\";\n";
            }

            response +=
            $"\tpasswordField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
            $"\tpasswordField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
            $"\tpasswordField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\tpasswordField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\tpasswordField.System = {(field.System).ToString().ToLowerInvariant()};\n";
            if (field.MinLength == null)
            {
                response += $"\tpasswordField.MinLength = null;\n";
            }
            else
            {
                response += $"\tpasswordField.MinLength = Int32.Parse(\"{field.MinLength}\");\n";
            }
            if (field.MaxLength == null)
            {
                response += $"\tpasswordField.MaxLength = null;\n";
            }
            else
            {
                response += $"\tpasswordField.MaxLength = Int32.Parse(\"{field.MaxLength}\");\n";
            }
            response +=
            $"\tpasswordField.Encrypted = {(field.Encrypted).ToString().ToLowerInvariant()};\n" +
            $"\tpasswordField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\tpasswordField.Permissions = new FieldPermissions();\n" +
            $"\tpasswordField.Permissions.CanRead = new List<Guid>();\n" +
            $"\tpasswordField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in field.Permissions.CanRead)
            {
                response += $"\tpasswordField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in field.Permissions.CanUpdate)
            {
                response += $"\tpasswordField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), passwordField, false);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";

            return response;
        }

        private string CreatePercentFieldCode(DbPercentField field, Guid entityId, string entityName)
        {
            var response = string.Empty;
            response =
$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
"{\n" +
    $"\tInputPercentField percentField = new InputPercentField();\n" +
    $"\tpercentField.Id = new Guid(\"{field.Id}\");\n" +
    $"\tpercentField.Name = \"{field.Name}\";\n" +
    $"\tpercentField.Label = \"{field.Label}\";\n";
            if (field.PlaceholderText == null)
            {
                response += $"\tpercentField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tpercentField.PlaceholderText = \"{field.PlaceholderText}\";\n";
            }
            if (field.Description == null)
            {
                response += $"\tpercentField.Description = null;\n";
            }
            else
            {
                response += $"\tpercentField.Description = \"{field.Description}\";\n";
            }
            if (field.HelpText == null)
            {
                response += $"\tpercentField.HelpText = null;\n";
            }
            else
            {
                response += $"\tpercentField.HelpText = \"{field.HelpText}\";\n";
            }

            response +=
            $"\tpercentField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
            $"\tpercentField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
            $"\tpercentField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\tpercentField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\tpercentField.System = {(field.System).ToString().ToLowerInvariant()};\n";
            if (field.DefaultValue == null)
            {
                response += $"\tpercentField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\tpercentField.DefaultValue = Decimal.Parse(\"{field.DefaultValue}\");\n";
            }
            if (field.MinValue == null)
            {
                response += $"\tpercentField.MinValue = null;\n";
            }
            else
            {
                response += $"\tpercentField.MinValue = Decimal.Parse(\"{field.MinValue}\");\n";
            }
            if (field.MaxValue == null)
            {
                response += $"\tpercentField.MaxValue = null;\n";
            }
            else
            {
                response += $"\tpercentField.MaxValue = Decimal.Parse(\"{field.MaxValue}\");\n";
            }
            response += $"\tpercentField.DecimalPlaces = byte.Parse(\"{field.DecimalPlaces}\");\n";
            response +=
            $"\tpercentField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\tpercentField.Permissions = new FieldPermissions();\n" +
            $"\tpercentField.Permissions.CanRead = new List<Guid>();\n" +
            $"\tpercentField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in field.Permissions.CanRead)
            {
                response += $"\tpercentField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in field.Permissions.CanUpdate)
            {
                response += $"\tpercentField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), percentField, false);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";


            return response;
        }

        private string CreatePhoneFieldCode(DbPhoneField field, Guid entityId, string entityName)
        {
            var response = string.Empty;
            response =
        $"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
        "{\n" +
            $"\tInputPhoneField phoneField = new InputPhoneField();\n" +
            $"\tphoneField.Id = new Guid(\"{field.Id}\");\n" +
            $"\tphoneField.Name = \"{field.Name}\";\n" +
            $"\tphoneField.Label =  \"{field.Label}\";\n";
            if (field.PlaceholderText == null)
            {
                response += $"\tphoneField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tphoneField.PlaceholderText = \"{field.PlaceholderText}\";\n";
            }
            if (field.Description == null)
            {
                response += $"\tphoneField.Description = null;\n";
            }
            else
            {
                response += $"\tphoneField.Description = \"{field.Description}\";\n";
            }
            if (field.HelpText == null)
            {
                response += $"\tphoneField.HelpText = null;\n";
            }
            else
            {
                response += $"\tphoneField.HelpText = \"{field.HelpText}\";\n";
            }

            response +=
            $"\tphoneField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
            $"\tphoneField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
            $"\tphoneField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\tphoneField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\tphoneField.System = {(field.System).ToString().ToLowerInvariant()};\n";
            if (field.DefaultValue == null)
            {
                response += $"\tphoneField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\tphoneField.DefaultValue = \"{field.DefaultValue}\";\n";
            }
            if (field.MaxLength == null)
            {
                response += $"\tphoneField.MaxLength = null;\n";
            }
            else
            {
                response += $"\tphoneField.MaxLength = Int32.Parse(\"{field.MaxLength}\");\n";
            }
            if (field.Format == null)
            {
                response += $"\tphoneField.Format = null;\n";
            }
            else
            {
                response += $"\tphoneField.Format = \"{field.Format}\";\n";
            }
            response +=
            $"\tphoneField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\tphoneField.Permissions = new FieldPermissions();\n" +
            $"\tphoneField.Permissions.CanRead = new List<Guid>();\n" +
            $"\tphoneField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in field.Permissions.CanRead)
            {
                response += $"\tphoneField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in field.Permissions.CanUpdate)
            {
                response += $"\tphoneField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), phoneField, false);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";

            return response;
        }

        private string CreateGuidFieldCode(DbGuidField field, Guid entityId, string entityName)
        {
            var response = string.Empty;
            response =
$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
"{\n" +
    $"\tInputGuidField guidField = new InputGuidField();\n" +
    $"\tguidField.Id = new Guid(\"{field.Id}\");\n" +
    $"\tguidField.Name = \"{field.Name}\";\n" +
    $"\tguidField.Label = \"{field.Label}\";\n";
            if (field.PlaceholderText == null)
            {
                response += $"\tguidField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tguidField.PlaceholderText = \"{field.PlaceholderText}\";\n";
            }
            if (field.Description == null)
            {
                response += $"\tguidField.Description = null;\n";
            }
            else
            {
                response += $"\tguidField.Description = \"{field.Description}\";\n";
            }
            if (field.HelpText == null)
            {
                response += $"\tguidField.HelpText = null;\n";
            }
            else
            {
                response += $"\tguidField.HelpText = \"{field.HelpText}\";\n";
            }

            response +=
            $"\tguidField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
            $"\tguidField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
            $"\tguidField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\tguidField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\tguidField.System = {(field.System).ToString().ToLowerInvariant()};\n";
            if (field.DefaultValue == null)
            {
                response += $"\tguidField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\tguidField.DefaultValue = Guid.Parse(\"{field.DefaultValue}\");\n";
            }

            response +=

            $"\tguidField.GenerateNewId = {(field.GenerateNewId).ToString().ToLowerInvariant()};\n" +
            $"\tguidField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\tguidField.Permissions = new FieldPermissions();\n" +
            $"\tguidField.Permissions.CanRead = new List<Guid>();\n" +
            $"\tguidField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in field.Permissions.CanRead)
            {
                response += $"\tguidField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in field.Permissions.CanUpdate)
            {
                response += $"\tguidField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), guidField, false);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";

            return response;
        }

        private string CreateSelectFieldCode(DbSelectField field, Guid entityId, string entityName)
        {
            var response = string.Empty;
            response =

$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
"{\n" +
    $"\tInputSelectField dropdownField = new InputSelectField();\n" +
    $"\tdropdownField.Id = new Guid(\"{field.Id}\");\n" +
    $"\tdropdownField.Name = \"{field.Name}\";\n" +
    $"\tdropdownField.Label = \"{field.Label}\";\n";
            if (field.PlaceholderText == null)
            {
                response += $"\tdropdownField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tdropdownField.PlaceholderText = \"{field.PlaceholderText}\";\n";
            }
            if (field.Description == null)
            {
                response += $"\tdropdownField.Description = null;\n";
            }
            else
            {
                response += $"\tdropdownField.Description = \"{field.Description}\";\n";
            }
            if (field.HelpText == null)
            {
                response += $"\tdropdownField.HelpText = null;\n";
            }
            else
            {
                response += $"\tdropdownField.HelpText = \"{field.HelpText}\";\n";
            }

            response +=
            $"\tdropdownField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
            $"\tdropdownField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
            $"\tdropdownField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\tdropdownField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\tdropdownField.System = {(field.System).ToString().ToLowerInvariant()};\n";
            if (field.DefaultValue == null)
            {
                response += $"\tdropdownField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\tdropdownField.DefaultValue = \"{field.DefaultValue}\";\n";
            }

            response += "\tdropdownField.Options = ";
            var fieldOptions = field.Options.ToList();
            if (fieldOptions.Count > 0)
            {
                response += "new List<SelectOption>\n\t{\n";
                for (int i = 0; i < fieldOptions.Count; i++)
                {
                    response += $"\t\tnew SelectOption() {{ Label = \"{fieldOptions[i].Label}\", Value = \"{fieldOptions[i].Value}\", IconClass = \"{fieldOptions[i].IconClass}\", Color = \"{fieldOptions[i].Color}\"}}";
                    if (i < fieldOptions.Count - 1)
                    {
                        response += ",\n";
                    }
                }
                response += "\n\t};\n";
            }
            else
            {
                response += "null;\n";
            }
            response +=
            $"\tdropdownField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\tdropdownField.Permissions = new FieldPermissions();\n" +
            $"\tdropdownField.Permissions.CanRead = new List<Guid>();\n" +
            $"\tdropdownField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in field.Permissions.CanRead)
            {
                response += $"\tdropdownField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in field.Permissions.CanUpdate)
            {
                response += $"\tdropdownField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), dropdownField, false);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";

            return response;
        }

        private string CreateTextFieldCode(DbTextField field, Guid entityId, string entityName)
        {
            var response = string.Empty;
            response =
            $"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
            "{\n" +
                $"\tInputTextField textboxField = new InputTextField();\n" +
            $"\ttextboxField.Id = new Guid(\"{field.Id}\");\n" +
            $"\ttextboxField.Name = \"{field.Name}\";\n" +
            $"\ttextboxField.Label = \"{field.Label}\";\n";
            if (field.PlaceholderText == null)
            {
                response += $"\ttextboxField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\ttextboxField.PlaceholderText = \"{field.PlaceholderText}\";\n";
            }
            if (field.Description == null)
            {
                response += $"\ttextboxField.Description = null;\n";
            }
            else
            {
                response += $"\ttextboxField.Description = \"{field.Description}\";\n";
            }
            if (field.HelpText == null)
            {
                response += $"\ttextboxField.HelpText = null;\n";
            }
            else
            {
                response += $"\ttextboxField.HelpText = \"{field.HelpText}\";\n";
            }

            response +=
            $"\ttextboxField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
            $"\ttextboxField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
            $"\ttextboxField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\ttextboxField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\ttextboxField.System = {(field.System).ToString().ToLowerInvariant()};\n";
            if (field.DefaultValue == null)
            {
                response += $"\ttextboxField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\ttextboxField.DefaultValue = \"{field.DefaultValue?.Replace("\"", "\\\"")}\";\n";
            }
            if (field.MaxLength == null)
            {
                response += $"\ttextboxField.MaxLength = null;\n";
            }
            else
            {
                response += $"\ttextboxField.MaxLength = Int32.Parse(\"{field.MaxLength}\");\n";
            }
            response +=

            $"\ttextboxField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\ttextboxField.Permissions = new FieldPermissions();\n" +
            $"\ttextboxField.Permissions.CanRead = new List<Guid>();\n" +
            $"\ttextboxField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in field.Permissions.CanRead)
            {
                response += $"\ttextboxField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in field.Permissions.CanUpdate)
            {
                response += $"\ttextboxField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), textboxField, false);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";
            return response;
        }

        private string CreateUrlFieldCode(DbUrlField field, Guid entityId, string entityName)
        {
            var response = string.Empty;
            response =

        $"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
        "{\n" +
            $"\tInputUrlField urlField = new InputUrlField();\n" +
            $"\turlField.Id = new Guid(\"{field.Id}\");\n" +
            $"\turlField.Name = \"{field.Name}\";\n" +
            $"\turlField.Label = \"{field.Label}\";\n";
            if (field.PlaceholderText == null)
            {
                response += $"\turlField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\turlField.PlaceholderText = \"{field.PlaceholderText}\";\n";
            }
            if (field.Description == null)
            {
                response += $"\turlField.Description = null;\n";
            }
            else
            {
                response += $"\turlField.Description = \"{field.Description}\";\n";
            }
            if (field.HelpText == null)
            {
                response += $"\turlField.HelpText = null;\n";
            }
            else
            {
                response += $"\turlField.HelpText = \"{field.HelpText}\";\n";
            }

            response +=
            $"\turlField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
            $"\turlField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
            $"\turlField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\turlField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\turlField.System = {(field.System).ToString().ToLowerInvariant()};\n";
            if (field.DefaultValue == null)
            {
                response += $"\turlField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\turlField.DefaultValue = \"{field.DefaultValue}\";\n";
            }
            if (field.MaxLength == null)
            {
                response += $"\turlField.MaxLength = null;\n";
            }
            else
            {
                response += $"\turlField.MaxLength = Int32.Parse(\"{field.MaxLength}\");\n";
            }
            response +=
            $"\turlField.OpenTargetInNewWindow = {(field.OpenTargetInNewWindow).ToString().ToLowerInvariant()};\n" +
            $"\turlField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\turlField.Permissions = new FieldPermissions();\n" +
            $"\turlField.Permissions.CanRead = new List<Guid>();\n" +
            $"\turlField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in field.Permissions.CanRead)
            {
                response += $"\turlField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in field.Permissions.CanUpdate)
            {
                response += $"\turlField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), urlField, false);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";

            return response;
        }

        private string CreateTreeSelectFieldCode(DbTreeSelectField field, Guid entityId, string entityName)
        {
            var response = string.Empty;
            response =
$"#region << ***Create field***  Entity: {entityName} Field Name: {field.Name} >>\n" +
"{\n" +
    $"\tInputTreeSelectField treeSelectField = new InputTreeSelectField();\n" +
    $"\ttreeSelectField.Id = new Guid(\"{field.Id}\");\n" +
    $"\ttreeSelectField.Name = \"{field.Name}\";\n" +
    $"\ttreeSelectField.Label = \"{field.Label}\";\n";
            if (field.PlaceholderText == null)
            {
                response += $"\ttreeSelectField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\ttreeSelectField.PlaceholderText = \"{field.PlaceholderText}\";\n";
            }
            if (field.Description == null)
            {
                response += $"\ttreeSelectField.Description = null;\n";
            }
            else
            {
                response += $"\ttreeSelectField.Description = \"{field.Description}\";\n";
            }
            if (field.HelpText == null)
            {
                response += $"\ttreeSelectField.HelpText = null;\n";
            }
            else
            {
                response += $"\ttreeSelectField.HelpText = \"{field.HelpText}\";\n";
            }

            response +=
            $"\ttreeSelectField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
            $"\ttreeSelectField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
            $"\ttreeSelectField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\ttreeSelectField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\ttreeSelectField.System = {(field.System).ToString().ToLowerInvariant()};\n";

            response +=
            $"\ttreeSelectField.RelatedEntityId = new Guid(\"{field.RelatedEntityId}\");\n" +
            $"\ttreeSelectField.RelationId = new Guid(\"{field.RelationId}\");\n" +
            $"\ttreeSelectField.SelectedTreeId = new Guid(\"{field.SelectedTreeId}\");\n" +
            $"\ttreeSelectField.SelectionTarget = \"{field.SelectionTarget}\";\n" +
            $"\ttreeSelectField.SelectionType = \"{field.SelectionType}\";\n" +


            $"\ttreeSelectField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\ttreeSelectField.Permissions = new FieldPermissions();\n" +
            $"\ttreeSelectField.Permissions.CanRead = new List<Guid>();\n" +
            $"\ttreeSelectField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in field.Permissions.CanRead)
            {
                response += $"\ttreeSelectField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in field.Permissions.CanUpdate)
            {
                response += $"\ttreeSelectField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), treeSelectField, false);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";

            return response;
        }


        private string DeleteFieldCode(DbBaseField field, Guid entityId, string entityName)
        {
            var response =

        $"#region << ***Delete field*** Entity: {entityName} Field Name: {field.Name} >>\n" +
        "{\n" +
            "\t{\n" +
                $"\t\tvar response = entMan.DeleteField(new Guid(\"{entityId}\"),new Guid(\"{field.Id}\"));\n" +
                $"\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Delete field failed for Field: {field.Name} Entity: {entityName}. Message:\" + response.Message);\n" +
            "\t}\n" +

        "}\n" +
        "#endregion\n\n";

            return response;
        }

        private UpdateCheckResponse UpdateFieldCode(DbBaseField currentField, DbBaseField oldField, DbEntity currentEntity)
        {
            var response = new UpdateCheckResponse();
            string code = string.Empty;
            var hasUpdate = false;

            //escape possible double quotes
            if (currentField.HelpText != null)
                currentField.HelpText = currentField.HelpText.Replace("\"", "\\\"");
            if (currentField.PlaceholderText != null)
                currentField.PlaceholderText = currentField.PlaceholderText.Replace("\"", "\\\"");
            if (currentField.Label != null)
                currentField.Label = currentField.Label.Replace("\"", "\\\"");
            if (currentField.Description != null)
                currentField.Description = currentField.Description.Replace("\"", "\\\"");
            //escape the old field values the same way so the update check is correct later
            if (oldField.HelpText != null)
                oldField.HelpText = oldField.HelpText.Replace("\"", "\\\"");
            if (oldField.PlaceholderText != null)
                oldField.PlaceholderText = oldField.PlaceholderText.Replace("\"", "\\\"");
            if (oldField.Label != null)
                oldField.Label = oldField.Label.Replace("\"", "\\\"");
            if (oldField.Description != null)
                oldField.Description = oldField.Description.Replace("\"", "\\\"");

            //Check if field type is updated
            if (currentField.GetFieldType() != oldField.GetFieldType())
            {
                response.Code = "";
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>field</span>  with name <span class='go-red'>{oldField.Name}</span> has its type updated from <span class='go-red'>{oldField.GetFieldType()}</span> to <span class='go-red'>{currentField.GetFieldType()}</span>");
                return response;
            }

            switch (currentField.GetFieldType())
            {
                case FieldType.AutoNumberField:
                    {
                        var responseCode = UpdateAutoNumberFieldCode(currentField as DbAutoNumberField, oldField as DbAutoNumberField, currentEntity.Id, currentEntity.Name);
                        if (responseCode != string.Empty)
                        {
                            code = responseCode;
                            hasUpdate = true;
                        }
                    }
                    break;
                case FieldType.CheckboxField:
                    {
                        var responseCode = UpdateCheckboxFieldCode(currentField as DbCheckboxField, oldField as DbCheckboxField, currentEntity.Id, currentEntity.Name);
                        if (responseCode != string.Empty)
                        {
                            code = responseCode;
                            hasUpdate = true;
                        }
                    }
                    break;
                case FieldType.CurrencyField:
                    {
                        var responseCode = UpdateCurrencyFieldCode(currentField as DbCurrencyField, oldField as DbCurrencyField, currentEntity.Id, currentEntity.Name);
                        if (responseCode != string.Empty)
                        {
                            code = responseCode;
                            hasUpdate = true;
                        }
                    }
                    break;
                case FieldType.DateField:
                    {
                        var responseCode = UpdateDateFieldCode(currentField as DbDateField, oldField as DbDateField, currentEntity.Id, currentEntity.Name);
                        if (responseCode != string.Empty)
                        {
                            code = responseCode;
                            hasUpdate = true;
                        }
                    }
                    break;
                case FieldType.DateTimeField:
                    {
                        var responseCode = UpdateDateTimeFieldCode(currentField as DbDateTimeField, oldField as DbDateTimeField, currentEntity.Id, currentEntity.Name);
                        if (responseCode != string.Empty)
                        {
                            code = responseCode;
                            hasUpdate = true;
                        }
                    }
                    break;
                case FieldType.EmailField:
                    {
                        var responseCode = UpdateEmailFieldCode(currentField as DbEmailField, oldField as DbEmailField, currentEntity.Id, currentEntity.Name);
                        if (responseCode != string.Empty)
                        {
                            code = responseCode;
                            hasUpdate = true;
                        }
                    }
                    break;
                case FieldType.FileField:
                    {
                        var responseCode = UpdateFileFieldCode(currentField as DbFileField, oldField as DbFileField, currentEntity.Id, currentEntity.Name);
                        if (responseCode != string.Empty)
                        {
                            code = responseCode;
                            hasUpdate = true;
                        }
                    }
                    break;
                case FieldType.ImageField:
                    {
                        var responseCode = UpdateImageFieldCode(currentField as DbImageField, oldField as DbImageField, currentEntity.Id, currentEntity.Name);
                        if (responseCode != string.Empty)
                        {
                            code = responseCode;
                            hasUpdate = true;
                        }
                    }
                    break;
                case FieldType.HtmlField:
                    {
                        var responseCode = UpdateHtmlFieldCode(currentField as DbHtmlField, oldField as DbHtmlField, currentEntity.Id, currentEntity.Name);
                        if (responseCode != string.Empty)
                        {
                            code = responseCode;
                            hasUpdate = true;
                        }
                    }
                    break;
                case FieldType.MultiLineTextField:
                    {
                        var responseCode = UpdateMultiLineTextFieldCode(currentField as DbMultiLineTextField, oldField as DbMultiLineTextField, currentEntity.Id, currentEntity.Name);
                        if (responseCode != string.Empty)
                        {
                            code = responseCode;
                            hasUpdate = true;
                        }
                    }
                    break;
                case FieldType.GeographyField:
                    {
                        var responseCode = UpdateGeographyFieldCode(currentField as DbGeographyField, oldField as DbGeographyField, currentEntity.Id, currentEntity.Name);

                        if (responseCode != string.Empty)
                        {
                            code = responseCode;
                            hasUpdate = true;
                        }
                    }
                    break;
                case FieldType.MultiSelectField:
                    {
                        var responseCode = UpdateMultiSelectFieldCode(currentField as DbMultiSelectField, oldField as DbMultiSelectField, currentEntity.Id, currentEntity.Name);
                        if (responseCode != string.Empty)
                        {
                            code = responseCode;
                            hasUpdate = true;
                        }
                    }
                    break;
                case FieldType.NumberField:
                    {
                        var responseCode = UpdateNumberFieldCode(currentField as DbNumberField, oldField as DbNumberField, currentEntity.Id, currentEntity.Name);
                        if (responseCode != string.Empty)
                        {
                            code = responseCode;
                            hasUpdate = true;
                        }
                    }
                    break;
                case FieldType.PasswordField:
                    {
                        var responseCode = UpdatePasswordFieldCode(currentField as DbPasswordField, oldField as DbPasswordField, currentEntity.Id, currentEntity.Name);
                        if (responseCode != string.Empty)
                        {
                            code = responseCode;
                            hasUpdate = true;
                        }
                    }
                    break;
                case FieldType.PercentField:
                    {
                        var responseCode = UpdatePercentFieldCode(currentField as DbPercentField, oldField as DbPercentField, currentEntity.Id, currentEntity.Name);
                        if (responseCode != string.Empty)
                        {
                            code = responseCode;
                            hasUpdate = true;
                        }
                    }
                    break;
                case FieldType.PhoneField:
                    {
                        var responseCode = UpdatePhoneFieldCode(currentField as DbPhoneField, oldField as DbPhoneField, currentEntity.Id, currentEntity.Name);
                        if (responseCode != string.Empty)
                        {
                            code = responseCode;
                            hasUpdate = true;
                        }
                    }
                    break;
                case FieldType.GuidField:
                    {
                        var responseCode = UpdateGuidFieldCode(currentField as DbGuidField, oldField as DbGuidField, currentEntity.Id, currentEntity.Name);
                        if (responseCode != string.Empty)
                        {
                            code = responseCode;
                            hasUpdate = true;
                        }
                    }
                    break;
                case FieldType.SelectField:
                    {
                        var responseCode = UpdateSelectFieldCode(currentField as DbSelectField, oldField as DbSelectField, currentEntity.Id, currentEntity.Name);
                        if (responseCode != string.Empty)
                        {
                            code = responseCode;
                            hasUpdate = true;
                        }
                    }
                    break;
                case FieldType.TextField:
                    {
                        var responseCode = UpdateTextFieldCode(currentField as DbTextField, oldField as DbTextField, currentEntity.Id, currentEntity.Name);
                        if (responseCode != string.Empty)
                        {
                            code = responseCode;
                            hasUpdate = true;
                        }
                    }
                    break;
                case FieldType.UrlField:
                    {
                        var responseCode = UpdateUrlFieldCode(currentField as DbUrlField, oldField as DbUrlField, currentEntity.Id, currentEntity.Name);
                        if (responseCode != string.Empty)
                        {
                            code = responseCode;
                            hasUpdate = true;
                        }
                    }
                    break;
            }

            response.Code = code;
            response.HasUpdate = hasUpdate;
            response.ChangeList.Add($"<span class='go-green label-block'>field</span>  with name <span class='go-red'>{oldField.Name}</span> was updated");
            return response;
        }

        private string UpdateAutoNumberFieldCode(DbAutoNumberField currentField, DbAutoNumberField oldField, Guid entityId, string entityName)
        {
            var response = string.Empty;
            var hasUpdate = false;
            #region << Code >>
            response =
            $"#region << ***Update field***   Entity: {entityName} Field Name: {currentField.Name} >>\n" +
            "{\n" +
            $"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
            "\tInputAutoNumberField autonumberField = new InputAutoNumberField();\n" +
            $"\tautonumberField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
            $"\tautonumberField.Name = \"{currentField.Name}\";\n" +
            $"\tautonumberField.Label = \"{currentField.Label}\";\n";
            if (currentField.PlaceholderText == null)
            {
                response += $"\tautonumberField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tautonumberField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
            }
            if (currentField.Description == null)
            {
                response += $"\tautonumberField.Description = null;\n";
            }
            else
            {
                response += $"\tautonumberField.Description = \"{currentField.Description}\";\n";
            }
            if (currentField.HelpText == null)
            {
                response += $"\tautonumberField.HelpText = null;\n";
            }
            else
            {
                response += $"\tautonumberField.HelpText = \"{currentField.HelpText}\";\n";
            }

            response +=
            $"\tautonumberField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
            $"\tautonumberField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
            $"\tautonumberField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\tautonumberField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\tautonumberField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
            if (currentField.DefaultValue == null)
            {
                response += $"\tautonumberField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\tautonumberField.DefaultValue = Decimal.Parse(\"{currentField.DefaultValue}\");\n";
            }
            if (currentField.DisplayFormat == null)
            {
                response += $"\tautonumberField.DisplayFormat = null;\n";
            }
            else
            {
                response += $"\tautonumberField.DisplayFormat = \"{currentField.DisplayFormat}\";\n";
            }
            if (currentField.StartingNumber == null)
            {
                response += $"\tautonumberField.StartingNumber = null;\n";
            }
            else
            {
                response += $"\tautonumberField.StartingNumber = Decimal.Parse(\"{currentField.StartingNumber}\");\n";
            }
            response +=
            $"\tautonumberField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            "\tautonumberField.Permissions = new FieldPermissions();\n" +
            "\tautonumberField.Permissions.CanRead = new List<Guid>();\n" +
            "\tautonumberField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";

            foreach (var permId in currentField.Permissions.CanRead)
            {
                response += $"\tautonumberField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in currentField.Permissions.CanUpdate)
            {
                response += $"\tautonumberField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), autonumberField);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";
            #endregion
            #region << Update check >>
            if (oldField == null) //oldField is null where its field type is different from currentField
            {
                hasUpdate = true;
            }
            else if (currentField.Name != oldField.Name)
            {
                hasUpdate = true;
            }
            else if (currentField.Label != oldField.Label)
            {
                hasUpdate = true;
            }
            else if (currentField.PlaceholderText != oldField.PlaceholderText)
            {
                hasUpdate = true;
            }
            else if (currentField.Description != oldField.Description)
            {
                hasUpdate = true;
            }
            else if (currentField.HelpText != oldField.HelpText)
            {
                hasUpdate = true;
            }
            else if (currentField.Required != oldField.Required)
            {
                hasUpdate = true;
            }
            else if (currentField.Unique != oldField.Unique)
            {
                hasUpdate = true;
            }
            else if (currentField.Searchable != oldField.Searchable)
            {
                hasUpdate = true;
            }
            else if (currentField.Auditable != oldField.Auditable)
            {
                hasUpdate = true;
            }
            else if (currentField.System != oldField.System)
            {
                hasUpdate = true;
            }
            else if (currentField.DefaultValue != oldField.DefaultValue)
            {
                hasUpdate = true;
            }
            else if (currentField.DisplayFormat != oldField.DisplayFormat)
            {
                hasUpdate = true;
            }
            else if (currentField.StartingNumber != oldField.StartingNumber)
            {
                hasUpdate = true;
            }
            else if (currentField.EnableSecurity != oldField.EnableSecurity)
            {
                hasUpdate = true;
            }
            else
            {
                // Permissions change check
                if (CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions))
                {
                    hasUpdate = true;
                }
            }
            #endregion

            if (!hasUpdate)
            {
                return string.Empty;
            }

            return response;
        }

        private string UpdateCheckboxFieldCode(DbCheckboxField currentField, DbCheckboxField oldField, Guid entityId, string entityName)
        {
            var response = string.Empty;
            var hasUpdate = false;
            #region << Code >>
            response =
            $"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
            "{\n" +
            $"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
            "\tInputCheckboxField checkboxField = new InputCheckboxField();\n" +
            $"\tcheckboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
            $"\tcheckboxField.Name = \"{currentField.Name}\";\n" +
            $"\tcheckboxField.Label = \"{currentField.Label}\";\n";
            if (currentField.PlaceholderText == null)
            {
                response += $"\tcheckboxField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tcheckboxField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
            }
            if (currentField.Description == null)
            {
                response += $"\tcheckboxField.Description = null;\n";
            }
            else
            {
                response += $"\tcheckboxField.Description = \"{currentField.Description}\";\n";
            }
            if (currentField.HelpText == null)
            {
                response += $"\tcheckboxField.HelpText = null;\n";
            }
            else
            {
                response += $"\tcheckboxField.HelpText = \"{currentField.HelpText}\";\n";
            }

            response +=
            $"\tcheckboxField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
            $"\tcheckboxField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
            $"\tcheckboxField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\tcheckboxField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\tcheckboxField.System = {(currentField.System).ToString().ToLowerInvariant()};\n" +
            $"\tcheckboxField.DefaultValue = {(currentField.DefaultValue).ToString().ToLowerInvariant()};\n" +
            $"\tcheckboxField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            "\tcheckboxField.Permissions = new FieldPermissions();\n" +
            "\tcheckboxField.Permissions.CanRead = new List<Guid>();\n" +
            "\tcheckboxField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in currentField.Permissions.CanRead)
            {
                response += $"\tcheckboxField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in currentField.Permissions.CanUpdate)
            {
                response += $"\tcheckboxField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), checkboxField);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";
            #endregion

            #region << Update check >>
            if (oldField == null) //oldField is null where its field type is different from currentField
            {
                hasUpdate = true;
            }
            else if (currentField.Name != oldField.Name)
            {
                hasUpdate = true;
            }
            else if (currentField.Label != oldField.Label)
            {
                hasUpdate = true;
            }
            else if (currentField.PlaceholderText != oldField.PlaceholderText)
            {
                hasUpdate = true;
            }
            else if (currentField.Description != oldField.Description)
            {
                hasUpdate = true;
            }
            else if (currentField.HelpText != oldField.HelpText)
            {
                hasUpdate = true;
            }
            else if (currentField.Required != oldField.Required)
            {
                hasUpdate = true;
            }
            else if (currentField.Unique != oldField.Unique)
            {
                hasUpdate = true;
            }
            else if (currentField.Searchable != oldField.Searchable)
            {
                hasUpdate = true;
            }
            else if (currentField.Auditable != oldField.Auditable)
            {
                hasUpdate = true;
            }
            else if (currentField.System != oldField.System)
            {
                hasUpdate = true;
            }
            else if (currentField.DefaultValue != oldField.DefaultValue)
            {
                hasUpdate = true;
            }
            else if (currentField.EnableSecurity != oldField.EnableSecurity)
            {
                hasUpdate = true;
            }
            else
            {
                // Permissions change check
                if (CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions))
                {
                    hasUpdate = true;
                }
            }
            #endregion

            if (!hasUpdate)
            {
                return string.Empty;
            }


            return response;
        }

        private string UpdateCurrencyFieldCode(DbCurrencyField currentField, DbCurrencyField oldField, Guid entityId, string entityName)
        {
            var response = string.Empty;
            var hasUpdate = false;
            #region << Code >>
            response =
            $"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
            "{\n" +
                $"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
                "\tInputCurrencyField currencyField = new InputCurrencyField();\n" +
                $"\tcurrencyField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
                $"\tcurrencyField.Name = \"{currentField.Name}\";\n" +
                $"\tcurrencyField.Label =  \"{currentField.Label}\";\n";
            if (currentField.PlaceholderText == null)
            {
                response += $"\tcurrencyField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tcurrencyField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
            }
            if (currentField.Description == null)
            {
                response += $"\tcurrencyField.Description = null;\n";
            }
            else
            {
                response += $"\tcurrencyField.Description = \"{currentField.Description}\";\n";
            }
            if (currentField.HelpText == null)
            {
                response += $"\tcurrencyField.HelpText = null;\n";
            }
            else
            {
                response += $"\tcurrencyField.HelpText = \"{currentField.HelpText}\";\n";
            }

            response +=
            $"\tcurrencyField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
            $"\tcurrencyField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
            $"\tcurrencyField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\tcurrencyField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\tcurrencyField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
            if (currentField.DefaultValue == null)
            {
                response += $"\tcurrencyField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\tcurrencyField.DefaultValue = Decimal.Parse(\"{currentField.DefaultValue}\");\n";
            }
            if (currentField.MinValue == null)
            {
                response += $"\tcurrencyField.MinValue = null;\n";
            }
            else
            {
                response += $"\tcurrencyField.MinValue = Decimal.Parse(\"{currentField.MinValue}\");\n";
            }
            if (currentField.MaxValue == null)
            {
                response += $"\tcurrencyField.MaxValue = null;\n";
            }
            else
            {
                response += $"\tcurrencyField.MaxValue = Decimal.Parse(\"{currentField.MaxValue}\");\n";
            }

            response +=
            $"\tcurrencyField.Currency = WebVella.Erp.Utilities.Helpers.GetCurrencyType(\"{currentField.Currency.Code}\");\n" +
            $"\tcurrencyField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            "\tcurrencyField.Permissions = new FieldPermissions();\n" +
            "\tcurrencyField.Permissions.CanRead = new List<Guid>();\n" +
            "\tcurrencyField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in currentField.Permissions.CanRead)
            {
                response += $"\tcurrencyField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in currentField.Permissions.CanUpdate)
            {
                response += $"\tcurrencyField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), currencyField);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";
            #endregion

            #region << Update check >>
            if (oldField == null) //oldField is null where its field type is different from currentField
            {
                hasUpdate = true;
            }
            else if (currentField.Name != oldField.Name)
            {
                hasUpdate = true;
            }
            else if (currentField.Label != oldField.Label)
            {
                hasUpdate = true;
            }
            else if (currentField.PlaceholderText != oldField.PlaceholderText)
            {
                hasUpdate = true;
            }
            else if (currentField.Description != oldField.Description)
            {
                hasUpdate = true;
            }
            else if (currentField.HelpText != oldField.HelpText)
            {
                hasUpdate = true;
            }
            else if (currentField.Required != oldField.Required)
            {
                hasUpdate = true;
            }
            else if (currentField.Unique != oldField.Unique)
            {
                hasUpdate = true;
            }
            else if (currentField.Searchable != oldField.Searchable)
            {
                hasUpdate = true;
            }
            else if (currentField.Auditable != oldField.Auditable)
            {
                hasUpdate = true;
            }
            else if (currentField.System != oldField.System)
            {
                hasUpdate = true;
            }
            else if (currentField.DefaultValue != oldField.DefaultValue)
            {
                hasUpdate = true;
            }
            else if (currentField.MinValue != oldField.MinValue)
            {
                hasUpdate = true;
            }
            else if (currentField.MaxValue != oldField.MaxValue)
            {
                hasUpdate = true;
            }
            else if (currentField.Currency.Code != oldField.Currency.Code)
            {
                hasUpdate = true;
            }
            else if (currentField.EnableSecurity != oldField.EnableSecurity)
            {
                hasUpdate = true;
            }
            else
            {
                // Permissions change check
                if (CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions))
                {
                    hasUpdate = true;
                }
            }
            #endregion

            if (!hasUpdate)
            {
                return string.Empty;
            }



            return response;
        }

        private string UpdateDateFieldCode(DbDateField currentField, DbDateField oldField, Guid entityId, string entityName)
        {
            var response = string.Empty;
            var hasUpdate = false;

            #region << Code >>
            response =
            $"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
            "{\n" +
                $"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
                "\tInputDateField dateField = new InputDateField();\n" +
                $"\tdateField.Id =  currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
                $"\tdateField.Name = \"{currentField.Name}\";\n" +
                $"\tdateField.Label = \"{currentField.Label}\";\n";
            if (currentField.PlaceholderText == null)
            {
                response += $"\tdateField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tdateField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
            }
            if (currentField.Description == null)
            {
                response += $"\tdateField.Description = null;\n";
            }
            else
            {
                response += $"\tdateField.Description = \"{currentField.Description}\";\n";
            }
            if (currentField.HelpText == null)
            {
                response += $"\tdateField.HelpText = null;\n";
            }
            else
            {
                response += $"\tdateField.HelpText = \"{currentField.HelpText}\";\n";
            }

            response +=
                $"\tdateField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
                $"\tdateField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
                $"\tdateField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
                $"\tdateField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
                $"\tdateField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
            if (currentField.DefaultValue == null)
            {
                response += $"\tdateField.DefaultValue = null;\n";
            }
            else
            {
                response += ($"\ttry{{ dateField.DefaultValue = DateTime.Parse(\"{currentField.DefaultValue}\"); }}" +
                    $"catch{{ dateField.DefaultValue = DateTime.Parse(\"{currentField.DefaultValue}\", new CultureInfo(\"{defaultCulture}\") ); }}\n");
            }
            if (currentField.Format == null)
            {
                response += $"\tdateField.Format = null;\n";
            }
            else
            {
                response += $"\tdateField.Format = \"{currentField.Format}\";\n";
            }

            response +=
            $"\tdateField.UseCurrentTimeAsDefaultValue = {(currentField.UseCurrentTimeAsDefaultValue).ToString().ToLowerInvariant()};\n" +
            $"\tdateField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            "\tdateField.Permissions = new FieldPermissions();\n" +
            "\tdateField.Permissions.CanRead = new List<Guid>();\n" +
            "\tdateField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";

            foreach (var permId in currentField.Permissions.CanRead)
            {
                response += $"\tdateField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in currentField.Permissions.CanUpdate)
            {
                response += $"\tdateField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), dateField);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";
            #endregion

            #region << Update check >>
            if (oldField == null) //oldField is null where its field type is different from currentField
            {
                hasUpdate = true;
            }
            else if (currentField.Name != oldField.Name)
            {
                hasUpdate = true;
            }
            else if (currentField.Label != oldField.Label)
            {
                hasUpdate = true;
            }
            else if (currentField.PlaceholderText != oldField.PlaceholderText)
            {
                hasUpdate = true;
            }
            else if (currentField.Description != oldField.Description)
            {
                hasUpdate = true;
            }
            else if (currentField.HelpText != oldField.HelpText)
            {
                hasUpdate = true;
            }
            else if (currentField.Required != oldField.Required)
            {
                hasUpdate = true;
            }
            else if (currentField.Unique != oldField.Unique)
            {
                hasUpdate = true;
            }
            else if (currentField.Searchable != oldField.Searchable)
            {
                hasUpdate = true;
            }
            else if (currentField.Auditable != oldField.Auditable)
            {
                hasUpdate = true;
            }
            else if (currentField.System != oldField.System)
            {
                hasUpdate = true;
            }
            else if (currentField.Format != oldField.Format)
            {
                hasUpdate = true;
            }
            else if (currentField.UseCurrentTimeAsDefaultValue != oldField.UseCurrentTimeAsDefaultValue)
            {
                hasUpdate = true;
            }
            else if (currentField.UseCurrentTimeAsDefaultValue && oldField.DefaultValue.HasValue)
            {
                hasUpdate = true;
            }
            else if (currentField.EnableSecurity != oldField.EnableSecurity)
            {
                hasUpdate = true;
            }
            else
            {
                // Permissions change check
                if (CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions))
                {
                    hasUpdate = true;
                }
            }
            #endregion

            if (!hasUpdate)
            {
                return string.Empty;
            }

            return response;
        }

        private string UpdateDateTimeFieldCode(DbDateTimeField currentField, DbDateTimeField oldField, Guid entityId, string entityName)
        {
            var response = string.Empty;
            var hasUpdate = false;

            #region << Code >>
            response =

            $"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
            "{\n" +
                $"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
                "\tInputDateTimeField datetimeField = new InputDateTimeField();\n" +
                $"\tdatetimeField.Id =  currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
                $"\tdatetimeField.Name = \"{currentField.Name}\";\n" +
                $"\tdatetimeField.Label = \"{currentField.Label}\";\n";
            if (currentField.PlaceholderText == null)
            {
                response += $"\tdatetimeField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tdatetimeField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
            }
            if (currentField.Description == null)
            {
                response += $"\tdatetimeField.Description = null;\n";
            }
            else
            {
                response += $"\tdatetimeField.Description = \"{currentField.Description}\";\n";
            }
            if (currentField.HelpText == null)
            {
                response += $"\tdatetimeField.HelpText = null;\n";
            }
            else
            {
                response += $"\tdatetimeField.HelpText = \"{currentField.HelpText}\";\n";
            }

            response +=
                $"\tdatetimeField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
                $"\tdatetimeField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
                $"\tdatetimeField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
                $"\tdatetimeField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
                $"\tdatetimeField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";

            if (!currentField.UseCurrentTimeAsDefaultValue)
            {
                if (currentField.DefaultValue == null)
                {
                    response += $"\tdatetimeField.DefaultValue = null;\n";
                }
                else
                {
                    response += ($"\ttry{{ datetimeField.DefaultValue = DateTime.Parse(\"{currentField.DefaultValue}\"); }}" +
                        $"catch{{ datetimeField.DefaultValue = DateTime.Parse(\"{currentField.DefaultValue}\", new CultureInfo(\"{defaultCulture}\") ); }}\n");
                }
            }
            else
            {
                response += $"\tdatetimeField.DefaultValue = null;\n";
            }

            if (currentField.Format == null)
            {
                response += $"\tdatetimeField.Format = null;\n";
            }
            else
            {
                response += $"\tdatetimeField.Format = \"{currentField.Format}\";\n";
            }

            response +=
            $"\tdatetimeField.UseCurrentTimeAsDefaultValue = {(currentField.UseCurrentTimeAsDefaultValue).ToString().ToLowerInvariant()};\n" +
            $"\tdatetimeField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            "\tdatetimeField.Permissions = new FieldPermissions();\n" +
            "\tdatetimeField.Permissions.CanRead = new List<Guid>();\n" +
            "\tdatetimeField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";

            foreach (var permId in currentField.Permissions.CanRead)
            {
                response += $"\tdatetimeField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in currentField.Permissions.CanUpdate)
            {
                response += $"\tdatetimeField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), datetimeField);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";

            #endregion

            #region << Update check >>
            if (oldField == null) //oldField is null where its field type is different from currentField
            {
                hasUpdate = true;
            }
            else if (currentField.Name != oldField.Name)
            {
                hasUpdate = true;
            }
            else if (currentField.Label != oldField.Label)
            {
                hasUpdate = true;
            }
            else if (currentField.PlaceholderText != oldField.PlaceholderText)
            {
                hasUpdate = true;
            }
            else if (currentField.Description != oldField.Description)
            {
                hasUpdate = true;
            }
            else if (currentField.HelpText != oldField.HelpText)
            {
                hasUpdate = true;
            }
            else if (currentField.Required != oldField.Required)
            {
                hasUpdate = true;
            }
            else if (currentField.Unique != oldField.Unique)
            {
                hasUpdate = true;
            }
            else if (currentField.Searchable != oldField.Searchable)
            {
                hasUpdate = true;
            }
            else if (currentField.Auditable != oldField.Auditable)
            {
                hasUpdate = true;
            }
            else if (currentField.System != oldField.System)
            {
                hasUpdate = true;
            }
            else if (currentField.Format != oldField.Format)
            {
                hasUpdate = true;
            }
            else if (currentField.UseCurrentTimeAsDefaultValue != oldField.UseCurrentTimeAsDefaultValue)
            {
                hasUpdate = true;
            }
            else if (currentField.UseCurrentTimeAsDefaultValue && oldField.DefaultValue.HasValue)
            {
                hasUpdate = true;
            }
            else if (currentField.EnableSecurity != oldField.EnableSecurity)
            {
                hasUpdate = true;
            }
            else
            {
                // Permissions change check
                if (CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions))
                {
                    hasUpdate = true;
                }
            }
            #endregion

            if (!hasUpdate)
            {
                return string.Empty;
            }

            return response;
        }

        private string UpdateEmailFieldCode(DbEmailField currentField, DbEmailField oldField, Guid entityId, string entityName)
        {
            var response = string.Empty;
            var hasUpdate = false;
            response =

            #region << Code >>
            $"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
            "{\n" +
                $"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
                "\tInputEmailField emailField = new InputEmailField();\n" +
                $"\temailField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
                $"\temailField.Name = \"{currentField.Name}\";\n" +
                $"\temailField.Label = \"{currentField.Label}\";\n";
            if (currentField.PlaceholderText == null)
            {
                response += $"\temailField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\temailField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
            }
            if (currentField.Description == null)
            {
                response += $"\temailField.Description = null;\n";
            }
            else
            {
                response += $"\temailField.Description = \"{currentField.Description}\";\n";
            }
            if (currentField.HelpText == null)
            {
                response += $"\temailField.HelpText = null;\n";
            }
            else
            {
                response += $"\temailField.HelpText = \"{currentField.HelpText}\";\n";
            }

            response +=
                $"\temailField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
                $"\temailField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
                $"\temailField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
                $"\temailField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
                $"\temailField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
            if (currentField.DefaultValue == null)
            {
                response += $"\temailField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\temailField.DefaultValue = \"{currentField.DefaultValue}\";\n";
            }
            if (currentField.MaxLength == null)
            {
                response += $"\temailField.MaxLength = null;\n";
            }
            else
            {
                response += $"\temailField.MaxLength = Int32.Parse(\"{currentField.MaxLength}\");\n";
            }
            response +=
            $"\temailField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\temailField.Permissions = new FieldPermissions();\n" +
            $"\temailField.Permissions.CanRead = new List<Guid>();\n" +
            $"\temailField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in currentField.Permissions.CanRead)
            {
                response += $"\temailField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in currentField.Permissions.CanUpdate)
            {
                response += $"\temailField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), emailField);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";
            #endregion

            #region << Update check >>
            if (oldField == null) //oldField is null where its field type is different from currentField
            {
                hasUpdate = true;
            }
            else if (currentField.Name != oldField.Name)
            {
                hasUpdate = true;
            }
            else if (currentField.Label != oldField.Label)
            {
                hasUpdate = true;
            }
            else if (currentField.PlaceholderText != oldField.PlaceholderText)
            {
                hasUpdate = true;
            }
            else if (currentField.Description != oldField.Description)
            {
                hasUpdate = true;
            }
            else if (currentField.HelpText != oldField.HelpText)
            {
                hasUpdate = true;
            }
            else if (currentField.Required != oldField.Required)
            {
                hasUpdate = true;
            }
            else if (currentField.Unique != oldField.Unique)
            {
                hasUpdate = true;
            }
            else if (currentField.Searchable != oldField.Searchable)
            {
                hasUpdate = true;
            }
            else if (currentField.Auditable != oldField.Auditable)
            {
                hasUpdate = true;
            }
            else if (currentField.System != oldField.System)
            {
                hasUpdate = true;
            }
            else if (currentField.DefaultValue != oldField.DefaultValue)
            {
                hasUpdate = true;
            }
            else if (currentField.MaxLength != oldField.MaxLength)
            {
                hasUpdate = true;
            }
            else if (currentField.EnableSecurity != oldField.EnableSecurity)
            {
                hasUpdate = true;
            }
            else
            {
                // Permissions change check
                if (CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions))
                {
                    hasUpdate = true;
                }
            }
            #endregion

            if (!hasUpdate)
            {
                return string.Empty;
            }

            return response;
        }

        private string UpdateFileFieldCode(DbFileField currentField, DbFileField oldField, Guid entityId, string entityName)
        {
            var response = "";
            var hasUpdate = false;
            #region << Code >>
            response =
            $"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
            "{\n" +
                $"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
                $"\tInputFileField fileField = new InputFileField();\n" +
                $"\tfileField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
                $"\tfileField.Name = \"{currentField.Name}\";\n" +
                $"\tfileField.Label = \"{currentField.Label}\";\n";
            if (currentField.PlaceholderText == null)
            {
                response += $"\tfileField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tfileField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
            }
            if (currentField.Description == null)
            {
                response += $"\tfileField.Description = null;\n";
            }
            else
            {
                response += $"\tfileField.Description = \"{currentField.Description}\";\n";
            }
            if (currentField.HelpText == null)
            {
                response += $"\tfileField.HelpText = null;\n";
            }
            else
            {
                response += $"\tfileField.HelpText = \"{currentField.HelpText}\";\n";
            }

            response +=
                $"\tfileField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
                $"\tfileField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
                $"\tfileField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
                $"\tfileField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
                $"\tfileField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
            if (currentField.DefaultValue == null)
            {
                response += $"\tfileField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\tfileField.DefaultValue = \"{currentField.DefaultValue}\";\n";
            }
            response +=
            $"\tfileField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\tfileField.Permissions = new FieldPermissions();\n" +
            $"\tfileField.Permissions.CanRead = new List<Guid>();\n" +
            $"\tfileField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in currentField.Permissions.CanRead)
            {
                response += $"\tfileField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in currentField.Permissions.CanUpdate)
            {
                response += $"\tfileField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), fileField);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";

            #endregion

            #region << Update check >>
            if (oldField == null) //oldField is null where its field type is different from currentField
            {
                hasUpdate = true;
            }
            else if (currentField.Name != oldField.Name)
            {
                hasUpdate = true;
            }
            else if (currentField.Label != oldField.Label)
            {
                hasUpdate = true;
            }
            else if (currentField.PlaceholderText != oldField.PlaceholderText)
            {
                hasUpdate = true;
            }
            else if (currentField.Description != oldField.Description)
            {
                hasUpdate = true;
            }
            else if (currentField.HelpText != oldField.HelpText)
            {
                hasUpdate = true;
            }
            else if (currentField.Required != oldField.Required)
            {
                hasUpdate = true;
            }
            else if (currentField.Unique != oldField.Unique)
            {
                hasUpdate = true;
            }
            else if (currentField.Searchable != oldField.Searchable)
            {
                hasUpdate = true;
            }
            else if (currentField.Auditable != oldField.Auditable)
            {
                hasUpdate = true;
            }
            else if (currentField.System != oldField.System)
            {
                hasUpdate = true;
            }
            else if (currentField.DefaultValue != oldField.DefaultValue)
            {
                hasUpdate = true;
            }
            else if (currentField.EnableSecurity != oldField.EnableSecurity)
            {
                hasUpdate = true;
            }
            else
            {

                // Permissions change check
                if (CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions))
                {
                    hasUpdate = true;
                }
            }
            #endregion

            if (!hasUpdate)
            {
                return string.Empty;
            }

            return response;
        }

        private string UpdateImageFieldCode(DbImageField currentField, DbImageField oldField, Guid entityId, string entityName)
        {
            var response = "";
            var hasUpdate = false;

            #region << Code >>
            response =
            $"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
            "{\n" +
                $"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
                $"\tInputImageField imageField = new InputImageField();\n" +
                $"\timageField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
                $"\timageField.Name = \"{currentField.Name}\";\n" +
                $"\timageField.Label = \"{currentField.Label}\";\n";
            if (currentField.PlaceholderText == null)
            {
                response += $"\timageField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\timageField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
            }
            if (currentField.Description == null)
            {
                response += $"\timageField.Description = null;\n";
            }
            else
            {
                response += $"\timageField.Description = \"{currentField.Description}\";\n";
            }
            if (currentField.HelpText == null)
            {
                response += $"\timageField.HelpText = null;\n";
            }
            else
            {
                response += $"\timageField.HelpText = \"{currentField.HelpText}\";\n";
            }

            response +=
            $"\timageField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
            $"\timageField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
            $"\timageField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\timageField.Auditable =  {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\timageField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
            if (currentField.DefaultValue == null)
            {
                response += $"\timageField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\timageField.DefaultValue = \"{currentField.DefaultValue}\";\n";
            }
            response +=
            $"\timageField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\timageField.Permissions = new FieldPermissions();\n" +
            $"\timageField.Permissions.CanRead = new List<Guid>();\n" +
            $"\timageField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in currentField.Permissions.CanRead)
            {
                response += $"\timageField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in currentField.Permissions.CanUpdate)
            {
                response += $"\timageField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), imageField);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";
            #endregion

            #region << Update check >>
            if (oldField == null) //oldField is null where its field type is different from currentField
            {
                hasUpdate = true;
            }
            else if (currentField.Name != oldField.Name)
            {
                hasUpdate = true;
            }
            else if (currentField.Label != oldField.Label)
            {
                hasUpdate = true;
            }
            else if (currentField.PlaceholderText != oldField.PlaceholderText)
            {
                hasUpdate = true;
            }
            else if (currentField.Description != oldField.Description)
            {
                hasUpdate = true;
            }
            else if (currentField.HelpText != oldField.HelpText)
            {
                hasUpdate = true;
            }
            else if (currentField.Required != oldField.Required)
            {
                hasUpdate = true;
            }
            else if (currentField.Unique != oldField.Unique)
            {
                hasUpdate = true;
            }
            else if (currentField.Searchable != oldField.Searchable)
            {
                hasUpdate = true;
            }
            else if (currentField.Auditable != oldField.Auditable)
            {
                hasUpdate = true;
            }
            else if (currentField.System != oldField.System)
            {
                hasUpdate = true;
            }
            else if (currentField.DefaultValue != oldField.DefaultValue)
            {
                hasUpdate = true;
            }
            else if (currentField.EnableSecurity != oldField.EnableSecurity)
            {
                hasUpdate = true;
            }
            else
            {
                // Permissions change check
                if (CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions))
                {
                    hasUpdate = true;
                }
            }
            #endregion

            if (!hasUpdate)
            {
                return string.Empty;
            }

            return response;
        }

        private string UpdateHtmlFieldCode(DbHtmlField currentField, DbHtmlField oldField, Guid entityId, string entityName)
        {
            var response = "";
            var hasUpdate = false;
            #region << Code >>
            response =
            $"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
            "{\n" +
                $"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
                $"\tInputHtmlField htmlField = new InputHtmlField();\n" +
                $"\thtmlField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
                $"\thtmlField.Name = \"{currentField.Name}\";\n" +
                $"\thtmlField.Label = \"{currentField.Label}\";\n";
            if (currentField.PlaceholderText == null)
            {
                response += $"\thtmlField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\thtmlField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
            }
            if (currentField.Description == null)
            {
                response += $"\thtmlField.Description = null;\n";
            }
            else
            {
                response += $"\thtmlField.Description = \"{currentField.Description}\";\n";
            }
            if (currentField.HelpText == null)
            {
                response += $"\thtmlField.HelpText = null;\n";
            }
            else
            {
                response += $"\thtmlField.HelpText = \"{currentField.HelpText}\";\n";
            }

            response +=
                $"\thtmlField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
                $"\thtmlField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
                $"\thtmlField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
                $"\thtmlField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
                $"\thtmlField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
            if (currentField.DefaultValue == null)
            {
                response += $"\thtmlField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\thtmlField.DefaultValue = \"{currentField.DefaultValue}\";\n";
            }
            response +=
            $"\thtmlField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\thtmlField.Permissions = new FieldPermissions();\n" +
            $"\thtmlField.Permissions.CanRead = new List<Guid>();\n" +
            $"\thtmlField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in currentField.Permissions.CanRead)
            {
                response += $"\thtmlField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in currentField.Permissions.CanUpdate)
            {
                response += $"\thtmlField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), htmlField);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";

            #endregion

            #region << Update check >>
            if (oldField == null) //oldField is null where its field type is different from currentField
            {
                hasUpdate = true;
            }
            else if (currentField.Name != oldField.Name)
            {
                hasUpdate = true;
            }
            else if (currentField.Label != oldField.Label)
            {
                hasUpdate = true;
            }
            else if (currentField.PlaceholderText != oldField.PlaceholderText)
            {
                hasUpdate = true;
            }
            else if (currentField.Description != oldField.Description)
            {
                hasUpdate = true;
            }
            else if (currentField.HelpText != oldField.HelpText)
            {
                hasUpdate = true;
            }
            else if (currentField.Required != oldField.Required)
            {
                hasUpdate = true;
            }
            else if (currentField.Unique != oldField.Unique)
            {
                hasUpdate = true;
            }
            else if (currentField.Searchable != oldField.Searchable)
            {
                hasUpdate = true;
            }
            else if (currentField.Auditable != oldField.Auditable)
            {
                hasUpdate = true;
            }
            else if (currentField.System != oldField.System)
            {
                hasUpdate = true;
            }
            else if (currentField.DefaultValue != oldField.DefaultValue)
            {
                hasUpdate = true;
            }
            else if (currentField.EnableSecurity != oldField.EnableSecurity)
            {
                hasUpdate = true;
            }
            else
            {
                // Permissions change check
                if (CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions))
                {
                    hasUpdate = true;
                }
            }
            #endregion

            if (!hasUpdate)
            {
                return string.Empty;
            }


            return response;
        }

        private string UpdateMultiLineTextFieldCode(DbMultiLineTextField currentField, DbMultiLineTextField oldField, Guid entityId, string entityName)
        {
            var response = "";
            var hasUpdate = false;

            #region << Code >>
            response =
            $"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
            "{\n" +
                $"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
                $"\tInputMultiLineTextField textareaField = new InputMultiLineTextField();\n" +
                $"\ttextareaField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
                $"\ttextareaField.Name = \"{currentField.Name}\";\n" +
                $"\ttextareaField.Label = \"{currentField.Label}\";\n";
            if (currentField.PlaceholderText == null)
            {
                response += $"\ttextareaField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\ttextareaField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
            }
            if (currentField.Description == null)
            {
                response += $"\ttextareaField.Description = null;\n";
            }
            else
            {
                response += $"\ttextareaField.Description = \"{currentField.Description}\";\n";
            }
            if (currentField.HelpText == null)
            {
                response += $"\ttextareaField.HelpText = null;\n";
            }
            else
            {
                response += $"\ttextareaField.HelpText = \"{currentField.HelpText}\";\n";
            }

            response +=
            $"\ttextareaField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
            $"\ttextareaField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
            $"\ttextareaField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\ttextareaField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\ttextareaField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
            if (currentField.DefaultValue == null)
            {
                response += $"\ttextareaField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\ttextareaField.DefaultValue = \"{currentField.DefaultValue}\";\n";
            }
            if (currentField.MaxLength == null)
            {
                response += $"\ttextareaField.MaxLength = null;\n";
            }
            else
            {
                response += $"\ttextareaField.MaxLength = Int32.Parse(\"{currentField.MaxLength}\");\n";
            }
            if (currentField.VisibleLineNumber == null)
            {
                response += $"\ttextareaField.VisibleLineNumber = null;\n";
            }
            else
            {
                response += $"\ttextareaField.VisibleLineNumber = Int32.Parse(\"{currentField.VisibleLineNumber}\");\n";
            }

            response +=
            $"\ttextareaField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\ttextareaField.Permissions = new FieldPermissions();\n" +
            $"\ttextareaField.Permissions.CanRead = new List<Guid>();\n" +
            $"\ttextareaField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in currentField.Permissions.CanRead)
            {
                response += $"\ttextareaField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in currentField.Permissions.CanUpdate)
            {
                response += $"\ttextareaField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), textareaField);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";

            #endregion

            #region << Update check >>
            if (oldField == null) //oldField is null where its field type is different from currentField
            {
                hasUpdate = true;
            }
            else if (currentField.Name != oldField.Name)
            {
                hasUpdate = true;
            }
            else if (currentField.Label != oldField.Label)
            {
                hasUpdate = true;
            }
            else if (currentField.PlaceholderText != oldField.PlaceholderText)
            {
                hasUpdate = true;
            }
            else if (currentField.Description != oldField.Description)
            {
                hasUpdate = true;
            }
            else if (currentField.HelpText != oldField.HelpText)
            {
                hasUpdate = true;
            }
            else if (currentField.Required != oldField.Required)
            {
                hasUpdate = true;
            }
            else if (currentField.Unique != oldField.Unique)
            {
                hasUpdate = true;
            }
            else if (currentField.Searchable != oldField.Searchable)
            {
                hasUpdate = true;
            }
            else if (currentField.Auditable != oldField.Auditable)
            {
                hasUpdate = true;
            }
            else if (currentField.System != oldField.System)
            {
                hasUpdate = true;
            }
            else if (currentField.DefaultValue != oldField.DefaultValue)
            {
                hasUpdate = true;
            }
            else if (currentField.MaxLength != oldField.MaxLength)
            {
                hasUpdate = true;
            }
            else if (currentField.VisibleLineNumber != oldField.VisibleLineNumber)
            {
                hasUpdate = true;
            }
            else if (currentField.EnableSecurity != oldField.EnableSecurity)
            {
                hasUpdate = true;
            }
            else
            {
                // Permissions change check
                if (CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions))
                {
                    hasUpdate = true;
                }
            }
            #endregion

            if (!hasUpdate)
            {
                return string.Empty;
            }

            return response;
        }
        private string UpdateGeographyFieldCode(DbGeographyField currentField, DbGeographyField oldField, Guid entityId, string entityName)
        {
            var response = "";
            var hasUpdate = false;

            #region << Code >>
            response =
            $"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
            "{\n" +
                $"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
                $"\tInputGeographyField geometryField = new InputGeographyField();\n" +
                $"\tgeometryField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
                $"\tgeometryField.Name = \"{currentField.Name}\";\n" +
                $"\tgeometryField.Label = \"{currentField.Label}\";\n";
            if (currentField.PlaceholderText == null)
            {
                response += $"\tgeometryField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tgeometryField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
            }
            if (currentField.Description == null)
            {
                response += $"\tgeometryField.Description = null;\n";
            }
            else
            {
                response += $"\tgeometryField.Description = \"{currentField.Description}\";\n";
            }
            if (currentField.HelpText == null)
            {
                response += $"\tgeometryField.HelpText = null;\n";
            }
            else
            {
                response += $"\tgeometryField.HelpText = \"{currentField.HelpText}\";\n";
            }

            response +=
            $"\tgeometryField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
            $"\tgeometryField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
            $"\tgeometryField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\tgeometryField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\tgeometryField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";

            if (currentField.Format.HasValue)
            {
                response +=
                    $"\tgeometryField.Format = WebVella.Erp.Api.Models.GeographyFieldFormat.{currentField.Format.Value};\n";
            }
            else
            {
                response +=
                    $"\tgeometryField.Format = WebVella.Erp.Api.Models.GeographyFieldFormat.GeoJSON;\n";
            }
            response +=
                $"\tgeometryField.SRID = {currentField.SRID};\n";
            if (currentField.DefaultValue == null)
            {
                response += $"\tgeometryField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\tgeometryField.DefaultValue = \"{currentField.DefaultValue}\";\n";
            }
            if (currentField.MaxLength == null)
            {
                response += $"\tgeometryField.MaxLength = null;\n";
            }
            else
            {
                response += $"\tgeometryField.MaxLength = Int32.Parse(\"{currentField.MaxLength}\");\n";
            }
            if (currentField.VisibleLineNumber == null)
            {
                response += $"\tgeometryField.VisibleLineNumber = null;\n";
            }
            else
            {
                response += $"\tgeometryField.VisibleLineNumber = Int32.Parse(\"{currentField.VisibleLineNumber}\");\n";
            }

            response +=
            $"\tgeometryField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\tgeometryField.Permissions = new FieldPermissions();\n" +
            $"\tgeometryField.Permissions.CanRead = new List<Guid>();\n" +
            $"\tgeometryField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in currentField.Permissions.CanRead)
            {
                response += $"\tgeometryField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in currentField.Permissions.CanUpdate)
            {
                response += $"\tgeometryField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), geometryField);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";

            #endregion

            #region << Update check >>
            if (oldField == null) //oldField is null where its field type is different from currentField
            {
                hasUpdate = true;
            }
            else if (currentField.Name != oldField.Name)
            {
                hasUpdate = true;
            }
            else if (currentField.Label != oldField.Label)
            {
                hasUpdate = true;
            }
            else if (currentField.PlaceholderText != oldField.PlaceholderText)
            {
                hasUpdate = true;
            }
            else if (currentField.Description != oldField.Description)
            {
                hasUpdate = true;
            }
            else if (currentField.HelpText != oldField.HelpText)
            {
                hasUpdate = true;
            }
            else if (currentField.Required != oldField.Required)
            {
                hasUpdate = true;
            }
            else if (currentField.Unique != oldField.Unique)
            {
                hasUpdate = true;
            }
            else if (currentField.Searchable != oldField.Searchable)
            {
                hasUpdate = true;
            }
            else if (currentField.Auditable != oldField.Auditable)
            {
                hasUpdate = true;
            }
            else if (currentField.System != oldField.System)
            {
                hasUpdate = true;
            }
            else if (currentField.DefaultValue != oldField.DefaultValue)
            {
                hasUpdate = true;
            }
            else if (currentField.MaxLength != oldField.MaxLength)
            {
                hasUpdate = true;
            }
            else if (currentField.VisibleLineNumber != oldField.VisibleLineNumber)
            {
                hasUpdate = true;
            }
            else if (currentField.EnableSecurity != oldField.EnableSecurity)
            {
                hasUpdate = true;
            }
            else
            {
                // Permissions change check
                if (CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions))
                {
                    hasUpdate = true;
                }
            }
            #endregion

            if (!hasUpdate)
            {
                return string.Empty;
            }

            return response;
        }

        private string UpdateMultiSelectFieldCode(DbMultiSelectField currentField, DbMultiSelectField oldField, Guid entityId, string entityName)
        {
            var response = "";
            var hasUpdate = false;
            #region << Code >>
            response =
            $"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
            "{\n" +
                $"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
                $"\tInputMultiSelectField multiSelectField = new InputMultiSelectField();\n" +
                $"\tmultiSelectField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
                $"\tmultiSelectField.Name = \"{currentField.Name}\";\n" +
                $"\tmultiSelectField.Label = \"{currentField.Label}\";\n";
            if (currentField.PlaceholderText == null)
            {
                response += $"\tmultiSelectField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tmultiSelectField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
            }
            if (currentField.Description == null)
            {
                response += $"\tmultiSelectField.Description = null;\n";
            }
            else
            {
                response += $"\tmultiSelectField.Description = \"{currentField.Description}\";\n";
            }
            if (currentField.HelpText == null)
            {
                response += $"\tmultiSelectField.HelpText = null;\n";
            }
            else
            {
                response += $"\tmultiSelectField.HelpText = \"{currentField.HelpText}\";\n";
            }

            response +=
                $"\tmultiSelectField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
                $"\tmultiSelectField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
                $"\tmultiSelectField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
                $"\tmultiSelectField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
                $"\tmultiSelectField.System = {(currentField.System).ToString().ToLowerInvariant()};\n" +
                "\tmultiSelectField.DefaultValue = ";
            var defaultValues = currentField.DefaultValue.ToList();
            if (defaultValues.Count > 0)
            {
                response += "new List<string>() {";

                for (int i = 0; i < defaultValues.Count; i++)
                {
                    response += "\"" + defaultValues[i] + "\"";
                    if (i < defaultValues.Count - 1)
                    {
                        response += ",";
                    }
                }
                response += "};\n";
            }
            else
            {
                response += "null;\n";
            }
            response += "\tmultiSelectField.Options = ";
            var fieldOptions = currentField.Options.ToList();
            if (fieldOptions.Count > 0)
            {
                response += "new List<SelectOption>\n\t{\n";
                for (int i = 0; i < fieldOptions.Count; i++)
                {
                    response += $"\t\tnew SelectOption() {{ Label = \"{fieldOptions[i].Label}\", Value = \"{fieldOptions[i].Value}\" , IconClass = \"{fieldOptions[i].IconClass}\", Color = \"{fieldOptions[i].Color}\"}}";
                    if (i < fieldOptions.Count - 1)
                    {
                        response += ",\n";
                    }
                }
                response += "\n\t};\n";
            }
            else
            {
                response += "null;\n";
            }
            response +=

            $"\tmultiSelectField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\tmultiSelectField.Permissions = new FieldPermissions();\n" +
            $"\tmultiSelectField.Permissions.CanRead = new List<Guid>();\n" +
            $"\tmultiSelectField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in currentField.Permissions.CanRead)
            {
                response += $"\tmultiSelectField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in currentField.Permissions.CanUpdate)
            {
                response += $"\tmultiSelectField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), multiSelectField);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";
            #endregion

            #region << Update check >>
            if (oldField == null) //oldField is null where its field type is different from currentField
            {
                hasUpdate = true;
            }
            else if (currentField.Name != oldField.Name)
            {
                hasUpdate = true;
            }
            else if (currentField.Label != oldField.Label)
            {
                hasUpdate = true;
            }
            else if (currentField.PlaceholderText != oldField.PlaceholderText)
            {
                hasUpdate = true;
            }
            else if (currentField.Description != oldField.Description)
            {
                hasUpdate = true;
            }
            else if (currentField.HelpText != oldField.HelpText)
            {
                hasUpdate = true;
            }
            else if (currentField.Required != oldField.Required)
            {
                hasUpdate = true;
            }
            else if (currentField.Unique != oldField.Unique)
            {
                hasUpdate = true;
            }
            else if (currentField.Searchable != oldField.Searchable)
            {
                hasUpdate = true;
            }
            else if (currentField.Auditable != oldField.Auditable)
            {
                hasUpdate = true;
            }
            else if (currentField.System != oldField.System)
            {
                hasUpdate = true;
            }
            else if (currentField.EnableSecurity != oldField.EnableSecurity)
            {
                hasUpdate = true;
            }
            else if (currentField.Options.Count != oldField.Options.Count)
            {
                hasUpdate = true;
            }
            else
            {
                var oldDefaultValuesDictionary = new Dictionary<string, bool>();
                var newOptionsDictionary = new Dictionary<string, string>();
                //create dictionary
                foreach (var value in oldField.DefaultValue.ToList())
                {
                    oldDefaultValuesDictionary[value] = true;
                }
                foreach (var value in currentField.DefaultValue.ToList())
                {
                    if (!oldDefaultValuesDictionary.ContainsKey(value))
                    {
                        hasUpdate = true;
                    }
                }

                var oldOptionsDictionary = new Dictionary<string, string>();
                //create dictionary
                foreach (var value in oldField.Options.ToList())
                {
                    oldOptionsDictionary[value.Label] = value.Value;
                }
                foreach (var value in currentField.Options.ToList())
                {
                    newOptionsDictionary[value.Label] = value.Value;
                    if (!oldOptionsDictionary.ContainsKey(value.Label) || oldOptionsDictionary[value.Label] != value.Value)
                    {
                        hasUpdate = true;
                    }
                }
                foreach (var value in oldField.Options.ToList())
                {
                    if (!newOptionsDictionary.ContainsKey(value.Label) || newOptionsDictionary[value.Label] != value.Value)
                    {
                        hasUpdate = true;
                    }
                }

                // Permissions change check
                if (CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions))
                {
                    hasUpdate = true;
                }
            }
            #endregion

            if (!hasUpdate)
            {
                return string.Empty;
            }

            return response;
        }

        private string UpdateNumberFieldCode(DbNumberField currentField, DbNumberField oldField, Guid entityId, string entityName)
        {
            var response = string.Empty;
            var hasUpdate = false;
            #region << Code >>
            response =
            $"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
            "{\n" +
                $"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
                $"\tInputNumberField numberField = new InputNumberField();\n" +
                $"\tnumberField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
                $"\tnumberField.Name = \"{currentField.Name}\";\n" +
                $"\tnumberField.Label = \"{currentField.Label}\";\n";
            if (currentField.PlaceholderText == null)
            {
                response += $"\tnumberField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tnumberField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
            }
            if (currentField.Description == null)
            {
                response += $"\tnumberField.Description = null;\n";
            }
            else
            {
                response += $"\tnumberField.Description = \"{currentField.Description}\";\n";
            }
            if (currentField.HelpText == null)
            {
                response += $"\tnumberField.HelpText = null;\n";
            }
            else
            {
                response += $"\tnumberField.HelpText = \"{currentField.HelpText}\";\n";
            }

            response +=
            $"\tnumberField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
            $"\tnumberField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
            $"\tnumberField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\tnumberField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\tnumberField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
            if (currentField.DefaultValue == null)
            {
                response += $"\tnumberField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\tnumberField.DefaultValue = Decimal.Parse(\"{currentField.DefaultValue}\");\n";
            }
            if (currentField.MinValue == null)
            {
                response += $"\tnumberField.MinValue = null;\n";
            }
            else
            {
                response += $"\tnumberField.MinValue = Decimal.Parse(\"{currentField.MinValue}\");\n";
            }
            if (currentField.MaxValue == null)
            {
                response += $"\tnumberField.MaxValue = null;\n";
            }
            else
            {
                response += $"\tnumberField.MaxValue = Decimal.Parse(\"{currentField.MaxValue}\");\n";
            }
            response += $"\tnumberField.DecimalPlaces = byte.Parse(\"{currentField.DecimalPlaces}\");\n";
            response +=
            $"\tnumberField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\tnumberField.Permissions = new FieldPermissions();\n" +
            $"\tnumberField.Permissions.CanRead = new List<Guid>();\n" +
            $"\tnumberField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in currentField.Permissions.CanRead)
            {
                response += $"\tnumberField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in currentField.Permissions.CanUpdate)
            {
                response += $"\tnumberField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), numberField);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";

            #endregion

            #region << Update check >>
            if (oldField == null) //oldField is null where its field type is different from currentField
            {
                hasUpdate = true;
            }
            else if (currentField.Name != oldField.Name)
            {
                hasUpdate = true;
            }
            else if (currentField.Label != oldField.Label)
            {
                hasUpdate = true;
            }
            else if (currentField.PlaceholderText != oldField.PlaceholderText)
            {
                hasUpdate = true;
            }
            else if (currentField.Description != oldField.Description)
            {
                hasUpdate = true;
            }
            else if (currentField.HelpText != oldField.HelpText)
            {
                hasUpdate = true;
            }
            else if (currentField.Required != oldField.Required)
            {
                hasUpdate = true;
            }
            else if (currentField.Unique != oldField.Unique)
            {
                hasUpdate = true;
            }
            else if (currentField.Searchable != oldField.Searchable)
            {
                hasUpdate = true;
            }
            else if (currentField.Auditable != oldField.Auditable)
            {
                hasUpdate = true;
            }
            else if (currentField.System != oldField.System)
            {
                hasUpdate = true;
            }
            else if (currentField.DefaultValue != oldField.DefaultValue)
            {
                hasUpdate = true;
            }
            else if (currentField.MinValue != oldField.MinValue)
            {
                hasUpdate = true;
            }
            else if (currentField.MaxValue != oldField.MaxValue)
            {
                hasUpdate = true;
            }
            else if (currentField.DecimalPlaces != oldField.DecimalPlaces)
            {
                hasUpdate = true;
            }
            else if (currentField.EnableSecurity != oldField.EnableSecurity)
            {
                hasUpdate = true;
            }
            else
            {
                // Permissions change check
                if (CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions))
                {
                    hasUpdate = true;
                }
            }
            #endregion

            if (!hasUpdate)
            {
                return string.Empty;
            }

            return response;
        }

        private string UpdatePasswordFieldCode(DbPasswordField currentField, DbPasswordField oldField, Guid entityId, string entityName)
        {
            var response = string.Empty;
            var hasUpdate = false;

            #region << Code >>
            response =
$"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
"{\n" +
    $"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
    $"\tInputPasswordField passwordField = new InputPasswordField();\n" +
    $"\tpasswordField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
    $"\tpasswordField.Name = \"{currentField.Name}\";\n" +
    $"\tpasswordField.Label = \"{currentField.Label}\";\n";
            if (currentField.PlaceholderText == null)
            {
                response += $"\tpasswordField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tpasswordField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
            }
            if (currentField.Description == null)
            {
                response += $"\tpasswordField.Description = null;\n";
            }
            else
            {
                response += $"\tpasswordField.Description = \"{currentField.Description}\";\n";
            }
            if (currentField.HelpText == null)
            {
                response += $"\tpasswordField.HelpText = null;\n";
            }
            else
            {
                response += $"\tpasswordField.HelpText = \"{currentField.HelpText}\";\n";
            }

            response +=
            $"\tpasswordField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
            $"\tpasswordField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
            $"\tpasswordField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\tpasswordField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\tpasswordField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";

            if (currentField.MinLength == null)
            {
                response += $"\tpasswordField.MinLength = null;\n";
            }
            else
            {
                response += $"\tpasswordField.MinLength = Int32.Parse(\"{currentField.MinLength}\");\n";
            }
            if (currentField.MaxLength == null)
            {
                response += $"\tpasswordField.MaxLength = null;\n";
            }
            else
            {
                response += $"\tpasswordField.MaxLength = Int32.Parse(\"{currentField.MaxLength}\");\n";
            }
            response +=

            $"\tpasswordField.Encrypted = {(currentField.Encrypted).ToString().ToLowerInvariant()};\n" +
            $"\tpasswordField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\tpasswordField.Permissions = new FieldPermissions();\n" +
            $"\tpasswordField.Permissions.CanRead = new List<Guid>();\n" +
            $"\tpasswordField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in currentField.Permissions.CanRead)
            {
                response += $"\tpasswordField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in currentField.Permissions.CanUpdate)
            {
                response += $"\tpasswordField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), passwordField);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";

            #endregion

            #region << Update check >>
            if (oldField == null) //oldField is null where its field type is different from currentField
            {
                hasUpdate = true;
            }
            else if (currentField.Name != oldField.Name)
            {
                hasUpdate = true;
            }
            else if (currentField.Label != oldField.Label)
            {
                hasUpdate = true;
            }
            else if (currentField.PlaceholderText != oldField.PlaceholderText)
            {
                hasUpdate = true;
            }
            else if (currentField.Description != oldField.Description)
            {
                hasUpdate = true;
            }
            else if (currentField.HelpText != oldField.HelpText)
            {
                hasUpdate = true;
            }
            else if (currentField.Required != oldField.Required)
            {
                hasUpdate = true;
            }
            else if (currentField.Unique != oldField.Unique)
            {
                hasUpdate = true;
            }
            else if (currentField.Searchable != oldField.Searchable)
            {
                hasUpdate = true;
            }
            else if (currentField.Auditable != oldField.Auditable)
            {
                hasUpdate = true;
            }
            else if (currentField.System != oldField.System)
            {
                hasUpdate = true;
            }
            else if (currentField.MinLength != oldField.MinLength)
            {
                hasUpdate = true;
            }
            else if (currentField.MaxLength != oldField.MaxLength)
            {
                hasUpdate = true;
            }
            else if (currentField.Encrypted != oldField.Encrypted)
            {
                hasUpdate = true;
            }
            else if (currentField.EnableSecurity != oldField.EnableSecurity)
            {
                hasUpdate = true;
            }
            else
            {
                // Permissions change check
                if (CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions))
                {
                    hasUpdate = true;
                }
            }
            #endregion

            if (!hasUpdate)
            {
                return string.Empty;
            }
            return response;
        }

        private string UpdatePercentFieldCode(DbPercentField currentField, DbPercentField oldField, Guid entityId, string entityName)
        {
            var response = string.Empty;
            var hasUpdate = false;

            #region << Code >>
            response =
            $"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
            "{\n" +
                $"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
                $"\tInputPercentField percentField = new InputPercentField();\n" +
                $"\tpercentField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
                $"\tpercentField.Name = \"{currentField.Name}\";\n" +
                $"\tpercentField.Label = \"{currentField.Label}\";\n";
            if (currentField.PlaceholderText == null)
            {
                response += $"\tpercentField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tpercentField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
            }
            if (currentField.Description == null)
            {
                response += $"\tpercentField.Description = null;\n";
            }
            else
            {
                response += $"\tpercentField.Description = \"{currentField.Description}\";\n";
            }
            if (currentField.HelpText == null)
            {
                response += $"\tpercentField.HelpText = null;\n";
            }
            else
            {
                response += $"\tpercentField.HelpText = \"{currentField.HelpText}\";\n";
            }

            response +=
            $"\tpercentField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
            $"\tpercentField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
            $"\tpercentField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\tpercentField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\tpercentField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
            if (currentField.DefaultValue == null)
            {
                response += $"\tpercentField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\tpercentField.DefaultValue = Decimal.Parse(\"{currentField.DefaultValue}\");\n";
            }
            if (currentField.MinValue == null)
            {
                response += $"\tpercentField.MinValue = null;\n";
            }
            else
            {
                response += $"\tpercentField.MinValue = Decimal.Parse(\"{currentField.MinValue}\");\n";
            }
            if (currentField.MaxValue == null)
            {
                response += $"\tpercentField.MaxValue = null;\n";
            }
            else
            {
                response += $"\tpercentField.MaxValue = Decimal.Parse(\"{currentField.MaxValue}\");\n";
            }
            response += $"\tpercentField.DecimalPlaces = byte.Parse(\"{currentField.DecimalPlaces}\");\n";
            response +=
            $"\tpercentField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\tpercentField.Permissions = new FieldPermissions();\n" +
            $"\tpercentField.Permissions.CanRead = new List<Guid>();\n" +
            $"\tpercentField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in currentField.Permissions.CanRead)
            {
                response += $"\tpercentField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in currentField.Permissions.CanUpdate)
            {
                response += $"\tpercentField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), percentField);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";

            #endregion

            #region << Update check >>
            if (oldField == null) //oldField is null where its field type is different from currentField
            {
                hasUpdate = true;
            }
            else if (currentField.Name != oldField.Name)
            {
                hasUpdate = true;
            }
            else if (currentField.Label != oldField.Label)
            {
                hasUpdate = true;
            }
            else if (currentField.PlaceholderText != oldField.PlaceholderText)
            {
                hasUpdate = true;
            }
            else if (currentField.Description != oldField.Description)
            {
                hasUpdate = true;
            }
            else if (currentField.HelpText != oldField.HelpText)
            {
                hasUpdate = true;
            }
            else if (currentField.Required != oldField.Required)
            {
                hasUpdate = true;
            }
            else if (currentField.Unique != oldField.Unique)
            {
                hasUpdate = true;
            }
            else if (currentField.Searchable != oldField.Searchable)
            {
                hasUpdate = true;
            }
            else if (currentField.Auditable != oldField.Auditable)
            {
                hasUpdate = true;
            }
            else if (currentField.System != oldField.System)
            {
                hasUpdate = true;
            }
            else if (currentField.DefaultValue != oldField.DefaultValue)
            {
                hasUpdate = true;
            }
            else if (currentField.MinValue != oldField.MinValue)
            {
                hasUpdate = true;
            }
            else if (currentField.MaxValue != oldField.MaxValue)
            {
                hasUpdate = true;
            }
            else if (currentField.DecimalPlaces != oldField.DecimalPlaces)
            {
                hasUpdate = true;
            }
            else if (currentField.EnableSecurity != oldField.EnableSecurity)
            {
                hasUpdate = true;
            }
            else
            {
                // Permissions change check
                if (CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions))
                {
                    hasUpdate = true;
                }
            }
            #endregion

            if (!hasUpdate)
            {
                return string.Empty;
            }


            return response;
        }

        private string UpdatePhoneFieldCode(DbPhoneField currentField, DbPhoneField oldField, Guid entityId, string entityName)
        {
            var response = string.Empty;
            var hasUpdate = false;

            #region << Code >>
            response =
        $"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
        "{\n" +
            $"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
            $"\tInputPhoneField phoneField = new InputPhoneField();\n" +
            $"\tphoneField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
            $"\tphoneField.Name = \"{currentField.Name}\";\n" +
            $"\tphoneField.Label =  \"{currentField.Label}\";\n";
            if (currentField.PlaceholderText == null)
            {
                response += $"\tphoneField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tphoneField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
            }
            if (currentField.Description == null)
            {
                response += $"\tphoneField.Description = null;\n";
            }
            else
            {
                response += $"\tphoneField.Description = \"{currentField.Description}\";\n";
            }
            if (currentField.HelpText == null)
            {
                response += $"\tphoneField.HelpText = null;\n";
            }
            else
            {
                response += $"\tphoneField.HelpText = \"{currentField.HelpText}\";\n";
            }

            response +=
            $"\tphoneField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
            $"\tphoneField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
            $"\tphoneField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\tphoneField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\tphoneField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
            if (currentField.DefaultValue == null)
            {
                response += $"\tphoneField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\tphoneField.DefaultValue = \"{currentField.DefaultValue}\";\n";
            }
            if (currentField.MaxLength == null)
            {
                response += $"\tphoneField.MaxLength = null;\n";
            }
            else
            {
                response += $"\tphoneField.MaxLength = Int32.Parse(\"{currentField.MaxLength}\");\n";
            }
            if (currentField.Format == null)
            {
                response += $"\tphoneField.Format = null;\n";
            }
            else
            {
                response += $"\tphoneField.Format = \"{currentField.Format}\";\n";
            }
            response +=
            $"\tphoneField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\tphoneField.Permissions = new FieldPermissions();\n" +
            $"\tphoneField.Permissions.CanRead = new List<Guid>();\n" +
            $"\tphoneField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in currentField.Permissions.CanRead)
            {
                response += $"\tphoneField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in currentField.Permissions.CanUpdate)
            {
                response += $"\tphoneField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), phoneField);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";

            #endregion

            #region << Update check >>
            if (oldField == null) //oldField is null where its field type is different from currentField
            {
                hasUpdate = true;
            }
            else if (currentField.Name != oldField.Name)
            {
                hasUpdate = true;
            }
            else if (currentField.Label != oldField.Label)
            {
                hasUpdate = true;
            }
            else if (currentField.PlaceholderText != oldField.PlaceholderText)
            {
                hasUpdate = true;
            }
            else if (currentField.Description != oldField.Description)
            {
                hasUpdate = true;
            }
            else if (currentField.HelpText != oldField.HelpText)
            {
                hasUpdate = true;
            }
            else if (currentField.Required != oldField.Required)
            {
                hasUpdate = true;
            }
            else if (currentField.Unique != oldField.Unique)
            {
                hasUpdate = true;
            }
            else if (currentField.Searchable != oldField.Searchable)
            {
                hasUpdate = true;
            }
            else if (currentField.Auditable != oldField.Auditable)
            {
                hasUpdate = true;
            }
            else if (currentField.System != oldField.System)
            {
                hasUpdate = true;
            }
            else if (currentField.DefaultValue != oldField.DefaultValue)
            {
                hasUpdate = true;
            }
            else if (currentField.MaxLength != oldField.MaxLength)
            {
                hasUpdate = true;
            }
            else if (currentField.Format != oldField.Format)
            {
                hasUpdate = true;
            }
            else if (currentField.EnableSecurity != oldField.EnableSecurity)
            {
                hasUpdate = true;
            }
            else
            {
                // Permissions change check
                if (CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions))
                {
                    hasUpdate = true;
                }
            }
            #endregion

            if (!hasUpdate)
            {
                return string.Empty;
            }

            return response;
        }

        private string UpdateGuidFieldCode(DbGuidField currentField, DbGuidField oldField, Guid entityId, string entityName)
        {
            var response = string.Empty;
            var hasUpdate = false;

            #region << Code >>
            response =
$"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
"{\n" +
    $"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
    $"\tInputGuidField guidField = new InputGuidField();\n" +
    $"\tguidField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
    $"\tguidField.Name = \"{currentField.Name}\";\n" +
    $"\tguidField.Label = \"{currentField.Label}\";\n";
            if (currentField.PlaceholderText == null)
            {
                response += $"\tguidField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tguidField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
            }
            if (currentField.Description == null)
            {
                response += $"\tguidField.Description = null;\n";
            }
            else
            {
                response += $"\tguidField.Description = \"{currentField.Description}\";\n";
            }
            if (currentField.HelpText == null)
            {
                response += $"\tguidField.HelpText = null;\n";
            }
            else
            {
                response += $"\tguidField.HelpText = \"{currentField.HelpText}\";\n";
            }

            response +=
            $"\tguidField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
            $"\tguidField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
            $"\tguidField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\tguidField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\tguidField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
            if (currentField.DefaultValue == null)
            {
                response += $"\tguidField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\tguidField.DefaultValue = Guid.Parse(\"{currentField.DefaultValue}\");\n";
            }

            response +=
            $"\tguidField.GenerateNewId = {(currentField.GenerateNewId).ToString().ToLowerInvariant()};\n" +
            $"\tguidField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\tguidField.Permissions = new FieldPermissions();\n" +
            $"\tguidField.Permissions.CanRead = new List<Guid>();\n" +
            $"\tguidField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in currentField.Permissions.CanRead)
            {
                response += $"\tguidField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in currentField.Permissions.CanUpdate)
            {
                response += $"\tguidField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), guidField);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";
            #endregion

            #region << Update check >>
            if (oldField == null) //oldField is null where its field type is different from currentField
            {
                hasUpdate = true;
            }
            else if (currentField.Name != oldField.Name)
            {
                hasUpdate = true;
            }
            else if (currentField.Label != oldField.Label)
            {
                hasUpdate = true;
            }
            else if (currentField.PlaceholderText != oldField.PlaceholderText)
            {
                hasUpdate = true;
            }
            else if (currentField.Description != oldField.Description)
            {
                hasUpdate = true;
            }
            else if (currentField.HelpText != oldField.HelpText)
            {
                hasUpdate = true;
            }
            else if (currentField.Required != oldField.Required)
            {
                hasUpdate = true;
            }
            else if (currentField.Unique != oldField.Unique)
            {
                hasUpdate = true;
            }
            else if (currentField.Searchable != oldField.Searchable)
            {
                hasUpdate = true;
            }
            else if (currentField.Auditable != oldField.Auditable)
            {
                hasUpdate = true;
            }
            else if (currentField.System != oldField.System)
            {
                hasUpdate = true;
            }
            else if (currentField.DefaultValue != oldField.DefaultValue)
            {
                hasUpdate = true;
            }
            else if (currentField.GenerateNewId != oldField.GenerateNewId)
            {
                hasUpdate = true;
            }
            else if (currentField.EnableSecurity != oldField.EnableSecurity)
            {
                hasUpdate = true;
            }
            else
            {
                if (CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions))
                {
                    hasUpdate = true;
                }
            }
            #endregion

            if (!hasUpdate)
            {
                return string.Empty;
            }

            return response;
        }

        private string UpdateSelectFieldCode(DbSelectField currentField, DbSelectField oldField, Guid entityId, string entityName)
        {
            var response = string.Empty;
            var hasUpdate = false;

            #region << Code >>
            response =

$"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
"{\n" +
    $"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
    $"\tInputSelectField dropdownField = new InputSelectField();\n" +
    $"\tdropdownField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
    $"\tdropdownField.Name = \"{currentField.Name}\";\n" +
    $"\tdropdownField.Label = \"{currentField.Label}\";\n";
            if (currentField.PlaceholderText == null)
            {
                response += $"\tdropdownField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\tdropdownField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
            }
            if (currentField.Description == null)
            {
                response += $"\tdropdownField.Description = null;\n";
            }
            else
            {
                response += $"\tdropdownField.Description = \"{currentField.Description}\";\n";
            }
            if (currentField.HelpText == null)
            {
                response += $"\tdropdownField.HelpText = null;\n";
            }
            else
            {
                response += $"\tdropdownField.HelpText = \"{currentField.HelpText}\";\n";
            }

            response +=
            $"\tdropdownField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
            $"\tdropdownField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
            $"\tdropdownField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\tdropdownField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\tdropdownField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
            if (currentField.DefaultValue == null)
            {
                response += $"\tdropdownField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\tdropdownField.DefaultValue = \"{currentField.DefaultValue}\";\n";
            }

            response += "\tdropdownField.Options = ";
            var fieldOptions = currentField.Options.ToList();
            if (fieldOptions.Count > 0)
            {
                response += "new List<SelectOption>\n\t{\n";
                for (int i = 0; i < fieldOptions.Count; i++)
                {
                    response += $"\t\tnew SelectOption() {{ Label = \"{fieldOptions[i].Label}\", Value = \"{fieldOptions[i].Value}\" , IconClass = \"{fieldOptions[i].IconClass}\", Color = \"{fieldOptions[i].Color}\"}}";
                    if (i < fieldOptions.Count - 1)
                    {
                        response += ",\n";
                    }
                }
                response += "\n\t};\n";
            }
            else
            {
                response += "null;\n";
            }
            response +=
            $"\tdropdownField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\tdropdownField.Permissions = new FieldPermissions();\n" +
            $"\tdropdownField.Permissions.CanRead = new List<Guid>();\n" +
            $"\tdropdownField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in currentField.Permissions.CanRead)
            {
                response += $"\tdropdownField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in currentField.Permissions.CanUpdate)
            {
                response += $"\tdropdownField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), dropdownField);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";

            #endregion

            #region << Update check >>
            if (oldField == null) //oldField is null where its field type is different from currentField
            {
                hasUpdate = true;
            }
            else if (currentField.Name != oldField.Name)
            {
                hasUpdate = true;
            }
            else if (currentField.Label != oldField.Label)
            {
                hasUpdate = true;
            }
            else if (currentField.PlaceholderText != oldField.PlaceholderText)
            {
                hasUpdate = true;
            }
            else if (currentField.Description != oldField.Description)
            {
                hasUpdate = true;
            }
            else if (currentField.HelpText != oldField.HelpText)
            {
                hasUpdate = true;
            }
            else if (currentField.Required != oldField.Required)
            {
                hasUpdate = true;
            }
            else if (currentField.Unique != oldField.Unique)
            {
                hasUpdate = true;
            }
            else if (currentField.Searchable != oldField.Searchable)
            {
                hasUpdate = true;
            }
            else if (currentField.Auditable != oldField.Auditable)
            {
                hasUpdate = true;
            }
            else if (currentField.System != oldField.System)
            {
                hasUpdate = true;
            }
            else if (currentField.DefaultValue != oldField.DefaultValue)
            {
                hasUpdate = true;
            }
            else if (currentField.EnableSecurity != oldField.EnableSecurity)
            {
                hasUpdate = true;
            }
            else if (currentField.Options.Count != oldField.Options.Count)
            {
                hasUpdate = true;
            }
            else
            {
                foreach (var option in oldField.Options.ToList())
                {
                    try
                    {
                        var currentFieldOption = currentField.Options.SingleOrDefault(x => x.Value == option.Value);
                        if (currentFieldOption == null || currentFieldOption.Label != option.Label ||
                            currentFieldOption.Color != option.Color || currentFieldOption.IconClass != option.IconClass)
                        {
                            hasUpdate = true;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Entity: {entityName} Field: {currentField.Name} Options processing error: {ex.Message}");
                    }
                }

                // Permissions change check
                if (CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions))
                {
                    hasUpdate = true;
                }
            }
            #endregion

            if (!hasUpdate)
            {
                return string.Empty;
            }


            return response;
        }

        private string UpdateTextFieldCode(DbTextField currentField, DbTextField oldField, Guid entityId, string entityName)
        {
            var response = string.Empty;
            var hasUpdate = false;

            #region << Code >>

            response =
$"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
"{\n" +
        $"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
    $"\tInputTextField textboxField = new InputTextField();\n" +
            $"\ttextboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
            $"\ttextboxField.Name = \"{currentField.Name}\";\n" +
            $"\ttextboxField.Label = \"{currentField.Label}\";\n";
            if (currentField.PlaceholderText == null)
            {
                response += $"\ttextboxField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\ttextboxField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
            }
            if (currentField.Description == null)
            {
                response += $"\ttextboxField.Description = null;\n";
            }
            else
            {
                response += $"\ttextboxField.Description = \"{currentField.Description}\";\n";
            }
            if (currentField.HelpText == null)
            {
                response += $"\ttextboxField.HelpText = null;\n";
            }
            else
            {
                response += $"\ttextboxField.HelpText = \"{currentField.HelpText}\";\n";
            }

            response +=
            $"\ttextboxField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
            $"\ttextboxField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
            $"\ttextboxField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\ttextboxField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\ttextboxField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
            if (currentField.DefaultValue == null)
            {
                response += $"\ttextboxField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\ttextboxField.DefaultValue = \"{currentField.DefaultValue}\";\n";
            }
            if (currentField.MaxLength == null)
            {
                response += $"\ttextboxField.MaxLength = null;\n";
            }
            else
            {
                response += $"\ttextboxField.MaxLength = Int32.Parse(\"{currentField.MaxLength}\");\n";
            }
            response +=
            $"\ttextboxField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\ttextboxField.Permissions = new FieldPermissions();\n" +
            $"\ttextboxField.Permissions.CanRead = new List<Guid>();\n" +
            $"\ttextboxField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in currentField.Permissions.CanRead)
            {
                response += $"\ttextboxField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in currentField.Permissions.CanUpdate)
            {
                response += $"\ttextboxField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), textboxField);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";

            #endregion

            #region << Update check >>
            if (oldField == null) //oldField is null where its field type is different from currentField
            {
                hasUpdate = true;
            }
            else if (currentField.Name != oldField.Name)
            {
                hasUpdate = true;
            }
            else if (currentField.Label != oldField.Label)
            {
                hasUpdate = true;
            }
            else if (currentField.PlaceholderText != oldField.PlaceholderText)
            {
                hasUpdate = true;
            }
            else if (currentField.Description != oldField.Description)
            {
                hasUpdate = true;
            }
            else if (currentField.HelpText != oldField.HelpText)
            {
                hasUpdate = true;
            }
            else if (currentField.Required != oldField.Required)
            {
                hasUpdate = true;
            }
            else if (currentField.Unique != oldField.Unique)
            {
                hasUpdate = true;
            }
            else if (currentField.Searchable != oldField.Searchable)
            {
                hasUpdate = true;
            }
            else if (currentField.Auditable != oldField.Auditable)
            {
                hasUpdate = true;
            }
            else if (currentField.System != oldField.System)
            {
                hasUpdate = true;
            }
            else if (currentField.DefaultValue != oldField.DefaultValue)
            {
                hasUpdate = true;
            }
            else if (currentField.MaxLength != oldField.MaxLength)
            {
                hasUpdate = true;
            }
            else if (currentField.EnableSecurity != oldField.EnableSecurity)
            {
                hasUpdate = true;
            }
            else
            {
                // Permissions change check
                if (CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions))
                {
                    hasUpdate = true;
                }
            }
            #endregion

            if (!hasUpdate)
            {
                return string.Empty;
            }

            return response;
        }

        private string UpdateUrlFieldCode(DbUrlField currentField, DbUrlField oldField, Guid entityId, string entityName)
        {
            var response = string.Empty;
            var hasUpdate = false;

            #region << Code >>
            response =

        $"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
        "{\n" +
            $"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
            $"\tInputUrlField urlField = new InputUrlField();\n" +
            $"\turlField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
            $"\turlField.Name = \"{currentField.Name}\";\n" +
            $"\turlField.Label = \"{currentField.Label}\";\n";
            if (currentField.PlaceholderText == null)
            {
                response += $"\turlField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\turlField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
            }
            if (currentField.Description == null)
            {
                response += $"\turlField.Description = null;\n";
            }
            else
            {
                response += $"\turlField.Description = \"{currentField.Description}\";\n";
            }
            if (currentField.HelpText == null)
            {
                response += $"\turlField.HelpText = null;\n";
            }
            else
            {
                response += $"\turlField.HelpText = \"{currentField.HelpText}\";\n";
            }

            response +=
            $"\turlField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
            $"\turlField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
            $"\turlField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\turlField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\turlField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";
            if (currentField.DefaultValue == null)
            {
                response += $"\turlField.DefaultValue = null;\n";
            }
            else
            {
                response += $"\turlField.DefaultValue = \"{currentField.DefaultValue}\";\n";
            }
            if (currentField.MaxLength == null)
            {
                response += $"\turlField.MaxLength = null;\n";
            }
            else
            {
                response += $"\turlField.MaxLength = Int32.Parse(\"{currentField.MaxLength}\");\n";
            }
            response +=
            $"\turlField.OpenTargetInNewWindow = {(currentField.OpenTargetInNewWindow).ToString().ToLowerInvariant()};\n" +
            $"\turlField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\turlField.Permissions = new FieldPermissions();\n" +
            $"\turlField.Permissions.CanRead = new List<Guid>();\n" +
            $"\turlField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in currentField.Permissions.CanRead)
            {
                response += $"\turlField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in currentField.Permissions.CanUpdate)
            {
                response += $"\turlField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), urlField);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";
            #endregion

            #region << Update check >>
            if (oldField == null) //oldField is null where its field type is different from currentField
            {
                hasUpdate = true;
            }
            else if (currentField.Name != oldField.Name)
            {
                hasUpdate = true;
            }
            else if (currentField.Label != oldField.Label)
            {
                hasUpdate = true;
            }
            else if (currentField.PlaceholderText != oldField.PlaceholderText)
            {
                hasUpdate = true;
            }
            else if (currentField.Description != oldField.Description)
            {
                hasUpdate = true;
            }
            else if (currentField.HelpText != oldField.HelpText)
            {
                hasUpdate = true;
            }
            else if (currentField.Required != oldField.Required)
            {
                hasUpdate = true;
            }
            else if (currentField.Unique != oldField.Unique)
            {
                hasUpdate = true;
            }
            else if (currentField.Searchable != oldField.Searchable)
            {
                hasUpdate = true;
            }
            else if (currentField.Auditable != oldField.Auditable)
            {
                hasUpdate = true;
            }
            else if (currentField.System != oldField.System)
            {
                hasUpdate = true;
            }
            else if (currentField.DefaultValue != oldField.DefaultValue)
            {
                hasUpdate = true;
            }
            else if (currentField.MaxLength != oldField.MaxLength)
            {
                hasUpdate = true;
            }
            else if (currentField.OpenTargetInNewWindow != oldField.OpenTargetInNewWindow)
            {
                hasUpdate = true;
            }
            else if (currentField.EnableSecurity != oldField.EnableSecurity)
            {
                hasUpdate = true;
            }
            else
            {
                // Permissions change check
                if (CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions))
                {
                    hasUpdate = true;
                }
            }
            #endregion

            if (!hasUpdate)
            {
                return string.Empty;
            }

            return response;
        }

        private string UpdateTreeSelectFieldCode(DbTreeSelectField currentField, DbTreeSelectField oldField, Guid entityId, string entityName)
        {
            var response = string.Empty;
            var hasUpdate = false;

            #region << Code >>
            response =
$"#region << ***Update field***  Entity: {entityName} Field Name: {currentField.Name} >>\n" +
"{\n" +
    $"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
    $"\tInputTreeSelectField treeSelectField = new InputTreeSelectField();\n" +
    $"\ttreeSelectField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
    $"\ttreeSelectField.Name = \"{currentField.Name}\";\n" +
    $"\ttreeSelectField.Label = \"{currentField.Label}\";\n";
            if (currentField.PlaceholderText == null)
            {
                response += $"\ttreeSelectField.PlaceholderText = null;\n";
            }
            else
            {
                response += $"\ttreeSelectField.PlaceholderText = \"{currentField.PlaceholderText}\";\n";
            }
            if (currentField.Description == null)
            {
                response += $"\ttreeSelectField.Description = null;\n";
            }
            else
            {
                response += $"\ttreeSelectField.Description = \"{currentField.Description}\";\n";
            }
            if (currentField.HelpText == null)
            {
                response += $"\ttreeSelectField.HelpText = null;\n";
            }
            else
            {
                response += $"\ttreeSelectField.HelpText = \"{currentField.HelpText}\";\n";
            }

            response +=
            $"\ttreeSelectField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
            $"\ttreeSelectField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
            $"\ttreeSelectField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
            $"\ttreeSelectField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
            $"\ttreeSelectField.System = {(currentField.System).ToString().ToLowerInvariant()};\n";

            response +=
            $"\ttreeSelectField.RelatedEntityId = new Guid(\"{currentField.RelatedEntityId}\");\n" +
            $"\ttreeSelectField.RelationId = new Guid(\"{currentField.RelationId}\");\n" +
            $"\ttreeSelectField.SelectedTreeId = new Guid(\"{currentField.SelectedTreeId}\");\n" +
            $"\ttreeSelectField.SelectionTarget = \"{currentField.SelectionTarget}\";\n" +
            $"\ttreeSelectField.SelectionType = \"{currentField.SelectionType}\";\n" +


            $"\ttreeSelectField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
            $"\ttreeSelectField.Permissions = new FieldPermissions();\n" +
            $"\ttreeSelectField.Permissions.CanRead = new List<Guid>();\n" +
            $"\ttreeSelectField.Permissions.CanUpdate = new List<Guid>();\n" +
            "\t//READ\n";
            foreach (var permId in currentField.Permissions.CanRead)
            {
                response += $"\ttreeSelectField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
            }
            response += "\t//UPDATE\n";
            foreach (var permId in currentField.Permissions.CanUpdate)
            {
                response += $"\ttreeSelectField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
            }
            response +=
            "\t{\n" +
                $"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), treeSelectField);\n" +
                "\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
            "\t}\n" +
        "}\n" +
        "#endregion\n\n";
            #endregion

            #region << Update check >>
            if (oldField == null) //oldField is null where its field type is different from currentField
            {
                hasUpdate = true;
            }
            else if (currentField.Name != oldField.Name)
            {
                hasUpdate = true;
            }
            else if (currentField.Label != oldField.Label)
            {
                hasUpdate = true;
            }
            else if (currentField.PlaceholderText != oldField.PlaceholderText)
            {
                hasUpdate = true;
            }
            else if (currentField.Description != oldField.Description)
            {
                hasUpdate = true;
            }
            else if (currentField.HelpText != oldField.HelpText)
            {
                hasUpdate = true;
            }
            else if (currentField.Required != oldField.Required)
            {
                hasUpdate = true;
            }
            else if (currentField.Unique != oldField.Unique)
            {
                hasUpdate = true;
            }
            else if (currentField.Searchable != oldField.Searchable)
            {
                hasUpdate = true;
            }
            else if (currentField.Auditable != oldField.Auditable)
            {
                hasUpdate = true;
            }
            else if (currentField.System != oldField.System)
            {
                hasUpdate = true;
            }
            else if (currentField.RelatedEntityId != oldField.RelatedEntityId)
            {
                hasUpdate = true;
            }
            else if (currentField.RelationId != oldField.RelationId)
            {
                hasUpdate = true;
            }
            else if (currentField.SelectedTreeId != oldField.SelectedTreeId)
            {
                hasUpdate = true;
            }
            else if (currentField.SelectionTarget != oldField.SelectionTarget)
            {
                hasUpdate = true;
            }
            else if (currentField.SelectionType != oldField.SelectionType)
            {
                hasUpdate = true;
            }
            else if (currentField.EnableSecurity != oldField.EnableSecurity)
            {
                hasUpdate = true;
            }
            else
            {
                // Permissions change check
                if (CheckFieldPermissionsHasUpdate(oldField.Permissions, currentField.Permissions))
                {
                    hasUpdate = true;
                }
            }
            #endregion

            if (!hasUpdate)
            {
                return string.Empty;
            }

            return response;
        }

        #endregion // End field region

        #region << Relations >>
        private string CreateRelationCode(DbEntityRelation relationRecord)
        {
            var originEntity = entMan.ReadEntity(relationRecord.OriginEntityId).Object;
            var originField = originEntity.Fields.Single(x => x.Id == relationRecord.OriginFieldId);
            var targetEntity = entMan.ReadEntity(relationRecord.TargetEntityId).Object;
            var targetField = targetEntity.Fields.Single(x => x.Id == relationRecord.TargetFieldId);

            //escape possible double quotes
            if (relationRecord.Label != null)
                relationRecord.Label = relationRecord.Label.Replace("\"", "\\\"");
            if (relationRecord.Description != null)
                relationRecord.Description = relationRecord.Description.Replace("\"", "\\\"");

            var response =
            $"#region << ***Create relation*** Relation name: {relationRecord.Name} >>\n" +
            "{\n" +
                "\tvar relation = new EntityRelation();\n" +
                $"\tvar originEntity = entMan.ReadEntity(new Guid(\"{originEntity.Id}\")).Object;\n" +
                $"\tvar originField = originEntity.Fields.SingleOrDefault(x => x.Name == \"{originField.Name}\");\n" +
                $"\tvar targetEntity = entMan.ReadEntity(new Guid(\"{targetEntity.Id}\")).Object;\n" +
                $"\tvar targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == \"{targetField.Name}\");\n" +
                $"\trelation.Id = new Guid(\"{relationRecord.Id}\");\n" +
                $"\trelation.Name =  \"{relationRecord.Name}\";\n" +
                $"\trelation.Label = \"{relationRecord.Label}\";\n";
            if (relationRecord.Description == null)
            {
                response += $"\trelation.Description = null;\n";
            }
            else
            {
                response += $"\trelation.Description = \"{relationRecord.Description}\";\n";
            }

            response +=
            $"\trelation.System =  {(relationRecord.System).ToString().ToLowerInvariant()};\n";
            if (relationRecord.RelationType == EntityRelationType.OneToOne)
            {
                response += $"\trelation.RelationType = EntityRelationType.OneToOne;\n";
            }
            else if (relationRecord.RelationType == EntityRelationType.OneToMany)
            {
                response += $"\trelation.RelationType = EntityRelationType.OneToMany;\n";
            }
            else if (relationRecord.RelationType == EntityRelationType.ManyToMany)
            {
                response += $"\trelation.RelationType = EntityRelationType.ManyToMany;\n";
            }
            response +=
            $"\trelation.OriginEntityId = originEntity.Id;\n" +
            $"\trelation.OriginEntityName = originEntity.Name;\n" +
            $"\trelation.OriginFieldId = originField.Id;\n" +
            $"\trelation.OriginFieldName = originField.Name;\n" +
            $"\trelation.TargetEntityId = targetEntity.Id;\n" +
            $"\trelation.TargetEntityName = targetEntity.Name;\n" +
            $"\trelation.TargetFieldId = targetField.Id;\n" +
            $"\trelation.TargetFieldName = targetField.Name;\n" +
            "\t{\n" +
                $"\t\tvar response = relMan.Create(relation);\n" +
                $"\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Relation: {relationRecord.Name} Create. Message:\" + response.Message);\n" +
            "\t}\n" +

        "}\n" +
        "#endregion\n\n";

            return response;
        }

        private string DeleteRelationCode(DbEntityRelation relationRecord)
        {
            var response =
        $"#region << ***Delete relation*** Relation name: {relationRecord.Name} >>\n" +
        "{\n" +
            "\t{\n" +
                $"\t\tvar response = relMan.Delete(new Guid(\"{relationRecord.Id}\"));\n" +
                $"\t\tif (!response.Success)\n" +
                    $"\t\t\tthrow new Exception(\"System error 10060. Relation: {relationRecord.Name} Delete. Message:\" + response.Message);\n" +
            "\t}\n" +

        "}\n" +
        "#endregion\n\n";

            return response;
        }

        private UpdateCheckResponse UpdateRelationCode(DbEntityRelation currentRelation, DbEntityRelation oldRelation)
        {
            var response = new UpdateCheckResponse();
            var originEntity = entMan.ReadEntity(currentRelation.OriginEntityId).Object;
            var originField = originEntity.Fields.Single(x => x.Id == currentRelation.OriginFieldId);
            var targetEntity = entMan.ReadEntity(currentRelation.TargetEntityId).Object;
            var targetField = targetEntity.Fields.Single(x => x.Id == currentRelation.TargetFieldId);
            string code = string.Empty;
            var hasUpdate = false;
            //escape possible double quotes
            if (currentRelation.Label != null)
                currentRelation.Label = currentRelation.Label.Replace("\"", "\\\"");
            if (currentRelation.Description != null)
                currentRelation.Description = currentRelation.Description.Replace("\"", "\\\"");
            //escape the old to for update check sake
            if (oldRelation.Label != null)
                oldRelation.Label = oldRelation.Label.Replace("\"", "\\\"");
            if (oldRelation.Description != null)
                oldRelation.Description = oldRelation.Description.Replace("\"", "\\\"");

            code = $"#region << ***Update relation*** Relation name: {currentRelation.Name} >>\n" +
            "{\n" +
                "\tvar relation = new EntityRelation();\n" +
                $"\tvar originEntity = entMan.ReadEntity(new Guid(\"{originEntity.Id}\")).Object;\n" +
                $"\tvar originField = originEntity.Fields.SingleOrDefault(x => x.Name == \"{originField.Name}\");\n" +
                $"\tvar targetEntity = entMan.ReadEntity(new Guid(\"{targetEntity.Id}\")).Object;\n" +
                $"\tvar targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == \"{targetField.Name}\");\n" +
                $"\trelation.Id = new Guid(\"{currentRelation.Id}\");\n";

            //name
            if (currentRelation.Name != oldRelation.Name)
            {
                hasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>name</span>  from <span class='go-red'>{oldRelation.Name}</span> to <span class='go-red'>{currentRelation.Name}</span>");
            }
            code += $"\trelation.Name = \"{currentRelation.Name}\";\n";

            //label
            if (currentRelation.Label != oldRelation.Label)
            {
                hasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>label</span>  from <span class='go-red'>{oldRelation.Label}</span> to <span class='go-red'>{currentRelation.Label}</span>");
            }

            code += $"\trelation.Label = \"{currentRelation.Label}\";\n";

            //description
            if (currentRelation.Description != oldRelation.Description &&
                !(currentRelation.Description == null && oldRelation.Description == "") &&
                !(currentRelation.Description == "" && oldRelation.Description == null))
            {
                hasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>description</span>  from <span class='go-red'>{oldRelation.Description}</span> to <span class='go-red'>{currentRelation.Description}</span>");
            }
            if (currentRelation.Description == null)
            {
                code += $"\trelation.Description = null;\n";
            }
            else
            {
                code += $"\trelation.Description = \"{currentRelation.Description}\";\n";
            }

            //system
            if (currentRelation.System != oldRelation.System)
            {
                hasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>system</span>  from <span class='go-red'>{(oldRelation.System).ToString().ToLowerInvariant()}</span> to <span class='go-red'>{(currentRelation.System).ToString().ToLowerInvariant()}</span>");
            }
            code += $"\trelation.System = {(currentRelation.System).ToString().ToLowerInvariant()};\n";

            //relation type
            if (currentRelation.RelationType != oldRelation.RelationType)
            {
                hasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>relationType</span>  from <span class='go-red'>{oldRelation.RelationType}</span> to <span class='go-red'>{currentRelation.RelationType}</span>");
            }
            if (currentRelation.RelationType == EntityRelationType.OneToOne)
            {
                code += $"\trelation.RelationType = EntityRelationType.OneToOne;\n";
            }
            else if (currentRelation.RelationType == EntityRelationType.OneToMany)
            {
                code += $"\trelation.RelationType = EntityRelationType.OneToMany;\n";
            }
            else if (currentRelation.RelationType == EntityRelationType.ManyToMany)
            {
                code += $"\trelation.RelationType = EntityRelationType.ManyToMany;\n";
            }


            //originEntityId
            if (currentRelation.OriginEntityId != oldRelation.OriginEntityId)
            {
                hasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>OriginEntityId</span>  from <span class='go-red'>{oldRelation.OriginEntityId}</span> to <span class='go-red'>{currentRelation.OriginEntityId}</span>");
            }
            code += $"\trelation.OriginEntityId = originEntity.Id;\n";

            //OriginFieldId
            if (currentRelation.OriginFieldId != oldRelation.OriginFieldId)
            {
                hasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>OriginFieldId</span>  from <span class='go-red'>{oldRelation.OriginFieldId}</span> to <span class='go-red'>{currentRelation.OriginFieldId}</span>");
            }
            code += $"\trelation.OriginFieldId = originField.Id;\n";

            //TargetEntityId
            if (currentRelation.TargetEntityId != oldRelation.TargetEntityId)
            {
                hasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>TargetEntityId</span>  from <span class='go-red'>{oldRelation.TargetEntityId}</span> to <span class='go-red'>{currentRelation.TargetEntityId}</span>");
            }
            code += $"\trelation.TargetEntityId = targetEntity.Id;\n";

            //TargetFieldId
            if (currentRelation.TargetFieldId != oldRelation.TargetFieldId)
            {
                hasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>TargetFieldId</span>  from <span class='go-red'>{oldRelation.TargetFieldId}</span> to <span class='go-red'>{currentRelation.TargetFieldId}</span>");
            }
            code += $"\trelation.TargetFieldId = targetField.Id;\n";
            code +=
            "\t{\n" +
                $"\tvar response = relMan.Update(relation);\n" +
                $"\tif (!response.Success)\n" +
                    $"\t\tthrow new Exception(\"System error 10060. Failed update relation: {currentRelation.Name}. Message:\" + response.Message);\n" +
            "\t}\n" +


            "}\n" +
            "#endregion\n\n";


            response.Code = code;
            response.HasUpdate = hasUpdate;
            return response;

        }
        #endregion

        #region << Role >>
        private string CreateRoleCode(EntityRecord role)
        {

            //escape possible double quotes
            if (role["description"] != null)
                role["description"] = ((string)role["description"]).Replace("\"", "\\\"");

            var response = "" +
$"#region << ***Create role*** Role name: {(string)role["name"]} >>\n" +
"{\n" +
    "\tvar role = new EntityRecord();\n" +
    $"\trole[\"id\"] = new Guid(\"{(Guid)role["id"]}\");\n" +
    $"\trole[\"name\"] = \"{(string)role["name"]}\";\n" +
    $"\trole[\"description\"] = \"{(string)role["description"]}\";\n" +
    "\tvar createRoleResult = recMan.CreateRecord(\"role\", role);\n" +
    "\tif (!createRoleResult.Success)\n" +
    "\t{\n" +
        $"\t\tthrow new Exception(\"System error 10060. Role create with name : {(string)role["name"]}. Message:\" + createRoleResult.Message);\n" +
    "\t}\n" +
"}\n" +
"#endregion\n\n";

            return response;
        }

        private string DeleteRoleCode(EntityRecord role)
        {
            var response = "" +
$"#region << ***Delete role*** Role name: {(string)role["name"]} >>\n" +
"{\n" +
    $"\tvar deleteRoleResult = recMan.DeleteRecord(\"role\", new Guid(\"{(Guid)role["id"]}\"));\n" +
    "\tif (!deleteRoleResult.Success)\n" +
    "\t{\n" +
        $"\t\tthrow new Exception(\"System error 10060. Role delete with name : {(string)role["name"]}. Message:\" + deleteRoleResult.Message);\n" +
    "\t}\n" +
"}\n" +
"#endregion\n\n";

            return response;
        }

        private UpdateCheckResponse UpdateRoleCode(EntityRecord currentRole, EntityRecord oldRole)
        {
            var response = new UpdateCheckResponse();
            string code = string.Empty;
            var hasUpdate = false;
            //escape possible double quotes
            if (currentRole["description"] != null)
                currentRole["description"] = ((string)currentRole["description"]).Replace("\"", "\\\"");
            //escape the for update check sake
            if (oldRole["description"] != null)
                oldRole["description"] = ((string)oldRole["description"]).Replace("\"", "\\\"");

            code =
$"#region << ***Update role*** Role name: {(string)currentRole["name"]} >>\n" +
"{\n" +
    "\tvar patchObject = new EntityRecord();\n" +
    $"\tpatchObject[\"id\"] = new Guid(\"{(Guid)currentRole["id"]}\");\n";

            //name
            if ((string)currentRole["name"] != (string)oldRole["name"])
            {
                hasUpdate = true;
                code += $"\tpatchObject[\"name\"] = \"{(string)currentRole["name"]}\";\n";
                response.ChangeList.Add($"<span class='go-green label-block'>name</span>  from <span class='go-red'>{(string)oldRole["name"]}</span> to <span class='go-red'>{(string)currentRole["name"]}</span>");
            }
            //label	
            if ((string)currentRole["description"] != (string)oldRole["description"])
            {
                hasUpdate = true;
                code += $"\tpatchObject[\"description\"] = \"{(string)currentRole["description"]}\";\n";
                response.ChangeList.Add($"<span class='go-green label-block'>description</span> from <span class='go-red'>{(string)oldRole["description"]}</span> to <span class='go-red'>{(string)currentRole["description"]}</span>");
            }

            code +=
                "\tvar updateRoleResult = recMan.UpdateRecord(\"role\", patchObject);\n" +
                "\tif (!updateRoleResult.Success)\n" +
                "\t{\n" +
                    $"\t\tthrow new Exception(\"System error 10060. Role update with name : {(string)currentRole["name"]}. Message:\" + updateRoleResult.Message);\n" +
                "\t}\n" +
            "}\n" +
            "#endregion\n\n";


            response.Code = code;
            response.HasUpdate = hasUpdate;
            return response;

        }
        #endregion

        #region << Helpers >>
        private bool CheckFieldPermissionsHasUpdate(DbFieldPermissions oldFieldPermissions, DbFieldPermissions currentFieldPermissions)
        {
            var response = false;

            #region << Fast check based on count >>
            if (oldFieldPermissions.CanRead.Count != currentFieldPermissions.CanRead.Count)
            {
                return true;
            }
            if (oldFieldPermissions.CanUpdate.Count != currentFieldPermissions.CanUpdate.Count)
            {
                return true;
            }
            #endregion

            var oldFieldCanReadPermissionDictionary = new Dictionary<Guid, bool>();
            var oldFieldCanUpdatePermissionDictionary = new Dictionary<Guid, bool>();
            var currentFieldCanReadPermissionDictionary = new Dictionary<Guid, bool>();
            var currentFieldCanUpdatePermissionDictionary = new Dictionary<Guid, bool>();

            #region << Fill dictionaries >>
            foreach (var permission in oldFieldPermissions.CanRead)
            {
                oldFieldCanReadPermissionDictionary[permission] = true;
            }
            foreach (var permission in oldFieldPermissions.CanUpdate)
            {
                oldFieldCanUpdatePermissionDictionary[permission] = true;
            }
            foreach (var permission in currentFieldPermissions.CanRead)
            {
                currentFieldCanReadPermissionDictionary[permission] = true;
            }
            foreach (var permission in currentFieldPermissions.CanUpdate)
            {
                currentFieldCanUpdatePermissionDictionary[permission] = true;
            }
            #endregion

            #region << Check if all current permissions are existing in the old Field >>

            foreach (var permission in currentFieldPermissions.CanRead)
            {
                if (!oldFieldCanReadPermissionDictionary.ContainsKey(permission))
                {
                    return true;
                }
            }
            foreach (var permission in currentFieldPermissions.CanUpdate)
            {
                if (!oldFieldCanUpdatePermissionDictionary.ContainsKey(permission))
                {
                    return true;
                }
            }
            #endregion

            #region << Check if all old permissions are existing in the current Field >>

            foreach (var permission in oldFieldPermissions.CanRead)
            {
                if (!currentFieldCanReadPermissionDictionary.ContainsKey(permission))
                {
                    return true;
                }
            }
            foreach (var permission in oldFieldPermissions.CanUpdate)
            {
                if (!currentFieldCanUpdatePermissionDictionary.ContainsKey(permission))
                {
                    return true;
                }
            }
            #endregion

            return response;
        }
        #endregion

        #region << Apps >>

        private string CreateAppCode(App app)
        {
            var response = $"#region << ***Create app*** App name: {app.Name} >>\n" +
            "{\n" +
                $"\tvar id = new Guid(\"{app.Id.ToString()}\");\n" +
                $"\tvar name = \"{app.Name}\";\n" +
                $"\tvar label = \"{app.Label}\";\n" +
                (app.Description != null ? $"\tvar description = \"{app.Description}\";\n" : "\tstring description = null;\n") +
                (app.IconClass != null ? $"\tvar iconClass = \"{app.IconClass}\";\n" : "\tstring iconClass = null;\n") +
                (app.Author != null ? $"\tvar author = \"{app.Author}\";\n" : "\tstring author = null;\n") +
                (app.Color != null ? $"\tvar color = \"{app.Color}\";\n" : "\tstring color = null;\n") +
                $"\tvar weight = {app.Weight};\n" +
                $"\tvar access = new List<Guid>();\n";
            foreach (Guid roleId in app.Access)
                response += $"\taccess.Add( new Guid(\"{roleId.ToString()}\") );\n";

            response += "\n\tnew WebVella.Erp.Web.Services.AppService().CreateApplication(id,name,label,description,iconClass,author,color,weight,access,WebVella.Erp.Database.DbContext.Current.Transaction);\n" +
            "}\n" +
            "#endregion\n\n";

            return response;
        }

        private string DeleteAppCode(App app)
        {
            return $"#region << ***Delete app*** App name: {app.Name} >>\n" +
                    "{\n" +
                        $"\n\tnew WebVella.Erp.Web.Services.AppService().DeleteApplication( new Guid(\"{app.Id}\"),WebVella.Erp.Database.DbContext.Current.Transaction, cascade: false );\n" +
                    "}\n" +
                    "#endregion\n\n";
        }

        private UpdateCheckResponse UpdateAppCode(App currentApp, App oldApp)
        {
            var response = new UpdateCheckResponse();
            response.Code = string.Empty;
            response.HasUpdate = false;

            if (currentApp.Name != oldApp.Name)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>name</span>  from <span class='go-red'>{oldApp.Name}</span> to <span class='go-red'>{currentApp.Name}</span>");
            }

            if (currentApp.Label != oldApp.Label)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>label</span>  from <span class='go-red'>{oldApp.Label}</span> to <span class='go-red'>{currentApp.Label}</span>");
            }

            if (currentApp.Description != oldApp.Description)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>description</span>  from <span class='go-red'>{oldApp.Description}</span> to <span class='go-red'>{currentApp.Description}</span>");
            }

            if (currentApp.IconClass != oldApp.IconClass)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>iconClass</span>  from <span class='go-red'>{oldApp.IconClass}</span> to <span class='go-red'>{currentApp.IconClass}</span>");
            }

            if (currentApp.Author != oldApp.Author)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>author</span>  from <span class='go-red'>{oldApp.Author}</span> to <span class='go-red'>{currentApp.Author}</span>");
            }

            if (currentApp.Color != oldApp.Color)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>color</span>  from <span class='go-red'>{oldApp.Color}</span> to <span class='go-red'>{currentApp.Color}</span>");
            }

            if (currentApp.Weight != oldApp.Weight)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>weight</span>  from <span class='go-red'>{oldApp.Weight}</span> to <span class='go-red'>{currentApp.Weight}</span>");
            }

            if (currentApp.Access != null && oldApp.Access != null)
            {
                bool accessChanged = currentApp.Access.Count != oldApp.Access.Count;
                if (!accessChanged)
                {
                    foreach (Guid id in currentApp.Access)
                    {
                        if (!oldApp.Access.Any(x => x == id))
                        {
                            accessChanged = true;
                            break;
                        }
                    }
                }
                if (accessChanged)
                {
                    response.ChangeList.Add($"<span class='go-green label-block'>access</span>  app access role list changes</span>");
                    response.HasUpdate = true;
                }
            }
            else if (!(currentApp.Access == null && oldApp.Access == null))
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>access</span>  app access role list changes</span>");
            }

            if (response.HasUpdate)
            {
                response.Code +=
                $"#region << ***Update app*** App name: {currentApp.Name} >>\n" +
                "{\n" +
                    $"\tvar id = new Guid(\"{currentApp.Id.ToString()}\");\n" +
                    $"\tvar name = \"{currentApp.Name}\";\n" +
                    $"\tvar label = \"{currentApp.Label}\";\n" +
                    (currentApp.Description != null ? $"\tvar description = \"{currentApp.Description}\";\n" : "\tstring description = null;\n") +
                    (currentApp.IconClass != null ? $"\tvar iconClass = \"{currentApp.IconClass}\";\n" : "\tstring iconClass = null;\n") +
                    (currentApp.Author != null ? $"\tvar author = \"{currentApp.Author}\";\n" : "\tstring author = null;\n") +
                    (currentApp.Color != null ? $"\tvar color = \"{currentApp.Color}\";\n" : "\tstring color = null;\n") +
                    $"\tvar weight = {currentApp.Weight};\n" +
                    $"\tvar access = new List<Guid>();\n";
                foreach (Guid roleId in currentApp.Access)
                    response.Code += $"\taccess.Add( new Guid(\"{roleId.ToString()}\") );\n";

                response.Code += "\n\tnew WebVella.Erp.Web.Services.AppService().UpdateApplication(id,name,label,description,iconClass,author,color,weight,access,WebVella.Erp.Database.DbContext.Current.Transaction);\n" +
            "}\n" +
            "#endregion\n\n";
            }

            return response;
        }

        #endregion

        #region <<SitemapArea>>

        private string CreateSitemapAreaCode(SitemapArea area)
        {
            var response = $"#region << ***Create sitemap area*** Sitemap area name: {area.Name} >>\n" +
            "{\n" +
                $"\tvar id = new Guid(\"{area.Id.ToString()}\");\n" +
                $"\tvar appId = new Guid(\"{area.AppId.ToString()}\");\n" +
                $"\tvar name = \"{area.Name}\";\n" +
                $"\tvar label = \"{area.Label}\";\n" +
                $"\tvar description = @\"{area.Description.EscapeMultiline()}\";\n" +
                $"\tvar iconClass = \"{area.IconClass}\";\n" +
                $"\tvar color = \"{area.Color}\";\n" +
                $"\tvar weight = {area.Weight};\n" +
                $"\tvar showGroupNames = {area.ShowGroupNames.ToString().ToLower()};\n" +
                $"\tvar access = new List<Guid>();\n";

            foreach (Guid roleId in area.Access)
                response += $"\taccess.Add( new Guid(\"{roleId.ToString()}\") );\n";

            response += $"\tvar labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();\n";
            foreach (var res in area.LabelTranslations ?? new List<TranslationResource>())
                response += $"\tlabelTranslations.Add( new WebVella.Erp.Web.Models.TranslationResource{{ Locale=\"{res.Locale}\", Key= \"{res.Key}\", Value= @\"{res.Value.EscapeMultiline()}\"  }} );\n";

            response += $"\tvar descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();\n";
            foreach (var res in area.DescriptionTranslations ?? new List<TranslationResource>())
                response += $"\tdescriptionTranslations.Add( new WebVella.Erp.Web.Models.TranslationResource{{ Locale=\"{res.Locale}\", Key= \"{res.Key}\", Value= @\"{res.Value.EscapeMultiline()}\"  }} );\n";


            response += "\n\tnew WebVella.Erp.Web.Services.AppService().CreateArea(id,appId,name,label,labelTranslations,description,descriptionTranslations,iconClass,color,weight,showGroupNames,access,WebVella.Erp.Database.DbContext.Current.Transaction);\n" +
            "}\n" +
            "#endregion\n\n";

            return response;
        }

        private UpdateCheckResponse UpdateSitemapAreaCode(SitemapArea currentArea, SitemapArea oldArea)
        {
            var response = new UpdateCheckResponse();
            response.Code = string.Empty;
            response.HasUpdate = false;

            if (currentArea.AppId != oldArea.AppId)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap area AppId</span>  from <span class='go-red'>{oldArea.AppId}</span> to <span class='go-red'>{currentArea.AppId}</span>");
            }

            if (currentArea.Name != oldArea.Name)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap area name</span>  from <span class='go-red'>{oldArea.Name}</span> to <span class='go-red'>{currentArea.Name}</span>");
            }

            if (currentArea.Label != oldArea.Label)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap area label</span>  from <span class='go-red'>{oldArea.Label}</span> to <span class='go-red'>{currentArea.Label}</span>");
            }

            if (currentArea.Description.EscapeMultiline() != oldArea.Description.EscapeMultiline())
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap area description</span>  from <span class='go-red'>{oldArea.Description}</span> to <span class='go-red'>{currentArea.Description}</span>");
            }

            if (currentArea.IconClass != oldArea.IconClass)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap area IconClass</span>  from <span class='go-red'>{oldArea.IconClass}</span> to <span class='go-red'>{currentArea.IconClass}</span>");
            }

            if (currentArea.Weight != oldArea.Weight)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap area weight</span>  from <span class='go-red'>{oldArea.Weight}</span> to <span class='go-red'>{currentArea.Weight}</span>");
            }

            if (currentArea.Color != oldArea.Color)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap area color</span>  from <span class='go-red'>{oldArea.Color}</span> to <span class='go-red'>{currentArea.Color}</span>");
            }

            if (currentArea.ShowGroupNames != oldArea.ShowGroupNames)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap area ShowGroupNames</span>  from <span class='go-red'>{oldArea.ShowGroupNames}</span> to <span class='go-red'>{currentArea.ShowGroupNames}</span>");
            }

            if (currentArea.Access != null && oldArea.Access != null)
            {
                bool accessChanged = currentArea.Access.Count != oldArea.Access.Count;
                if (!accessChanged)
                {
                    foreach (Guid id in currentArea.Access)
                    {
                        if (!oldArea.Access.Any(x => x == id))
                        {
                            accessChanged = true;
                            break;
                        }
                    }
                }
                if (accessChanged)
                {
                    response.ChangeList.Add($"<span class='go-green label-block'>sitemap area access</span>  access role list changed</span>");
                    response.HasUpdate = true;
                }
            }
            else if (!(currentArea.Access == null && oldArea.Access == null))
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap area access</span>  access role list changed</span>");
            }

            if (currentArea.LabelTranslations != null && oldArea.LabelTranslations != null)
            {
                bool translationChanged = currentArea.LabelTranslations.Count != oldArea.LabelTranslations.Count;
                if (!translationChanged)
                {
                    foreach (var translation in currentArea.LabelTranslations)
                    {
                        if (!oldArea.LabelTranslations.Any(x => x.Locale == translation.Locale && x.Key == translation.Key && x.Value == translation.Value))
                        {
                            translationChanged = true;
                            break;
                        }
                    }
                }
                if (translationChanged)
                {
                    response.ChangeList.Add($"<span class='go-green label-block'>sitemap area LabelTranslations</span> LabelTranslations changed</span>");
                    response.HasUpdate = true;
                }
            }
            else if (!(currentArea.LabelTranslations == null && oldArea.LabelTranslations == null))
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap area LabelTranslations</span> LabelTranslations changed</span>");
            }

            if (currentArea.DescriptionTranslations != null && oldArea.DescriptionTranslations != null)
            {
                bool translationChanged = currentArea.DescriptionTranslations.Count != oldArea.DescriptionTranslations.Count;
                if (!translationChanged)
                {
                    foreach (var translation in currentArea.DescriptionTranslations)
                    {
                        if (!oldArea.DescriptionTranslations.Any(x => x.Locale == translation.Locale && x.Key == translation.Key && x.Value == translation.Value))
                        {
                            translationChanged = true;
                            break;
                        }
                    }
                }
                if (translationChanged)
                {
                    response.ChangeList.Add($"<span class='go-green label-block'>sitemap area DescriptionTranslations</span> DescriptionTranslations changed</span>");
                    response.HasUpdate = true;
                }
            }
            else if (!(currentArea.DescriptionTranslations == null && oldArea.DescriptionTranslations == null))
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap area DescriptionTranslations</span> DescriptionTranslations changed</span>");
            }

            if (response.HasUpdate)
            {
                response.Code += $"#region << ***Update sitemap area*** Sitemap area name: {currentArea.Name} >>\n" +
            "{\n" +
                $"\tvar id = new Guid(\"{currentArea.Id.ToString()}\");\n" +
                $"\tvar appId = new Guid(\"{currentArea.AppId.ToString()}\");\n" +
                $"\tvar name = \"{currentArea.Name}\";\n" +
                $"\tvar label = \"{currentArea.Label}\";\n" +
                $"\tvar description = @\"{currentArea.Description.EscapeMultiline()}\";\n" +
                $"\tvar iconClass = \"{currentArea.IconClass}\";\n" +
                $"\tvar color = \"{currentArea.Color}\";\n" +
                $"\tvar weight = {currentArea.Weight};\n" +
                $"\tvar showGroupNames = {currentArea.ShowGroupNames.ToString().ToLower()};\n" +
                $"\tvar access = new List<Guid>();\n";

                foreach (Guid roleId in currentArea.Access)
                    response.Code += $"\taccess.Add( new Guid(\"{roleId.ToString()}\") );\n";

                response.Code += $"\tvar labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();\n";
                foreach (var res in currentArea.LabelTranslations ?? new List<TranslationResource>())
                    response.Code += $"\tlabelTranslations.Add( new WebVella.Erp.Web.Models.TranslationResource{{ Locale=\"{res.Locale}\", Key= \"{res.Key}\", Value= @\"{res.Value.EscapeMultiline()}\"  }} );\n";

                response.Code += $"\tvar descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();\n";
                foreach (var res in currentArea.DescriptionTranslations ?? new List<TranslationResource>())
                    response.Code += $"\tdescriptionTranslations.Add( new WebVella.Erp.Web.Models.TranslationResource{{ Locale=\"{res.Locale}\", Key= \"{res.Key}\", Value= @\"{res.Value.EscapeMultiline()}\"  }} );\n";

                response.Code += "\n\tnew WebVella.Erp.Web.Services.AppService().UpdateArea(id,appId,name,label,labelTranslations,description,descriptionTranslations,iconClass,color,weight,showGroupNames,access,WebVella.Erp.Database.DbContext.Current.Transaction);\n" +
                "}\n" +
                "#endregion\n\n";
            }

            return response;
        }

        private string DeleteSitemapAreaCode(SitemapArea area)
        {
            return $"#region << ***Delete sitemap area*** Area name: {area.Name} >>\n" +
                    "{\n" +
                        $"\n\tnew WebVella.Erp.Web.Services.AppService().DeleteArea( new Guid(\"{area.Id}\"),WebVella.Erp.Database.DbContext.Current.Transaction, cascade: false );\n" +
                    "}\n" +
                    "#endregion\n\n";
        }

        #endregion

        #region <<SitemapNode>>

        private string CreateSitemapNodeCode(Guid areaId, SitemapNode node)
        {
            var response = $"#region << ***Create sitemap node*** Sitemap node name: {node.Name} >>\n" +
            "{\n" +
                $"\tvar id = new Guid(\"{node.Id.ToString()}\");\n" +
                (node.ParentId.HasValue ? $"\tvar parentId = new Guid(\"{node.ParentId.ToString()}\");\n" : $"\tGuid? parentId = null;\n") +
                $"\tvar areaId = new Guid(\"{areaId.ToString()}\");\n" +
                (node.EntityId.HasValue ? $"\tGuid? entityId = new Guid(\"{node.EntityId}\");\n" : $"\tGuid? entityId = null;\n") +
                $"\tvar name = \"{node.Name}\";\n" +
                $"\tvar label = \"{node.Label}\";\n" +
                $"\tvar url = \"{node.Url}\";\n" +
                $"\tvar iconClass = \"{node.IconClass}\";\n" +
                $"\tvar weight = {node.Weight};\n" +
                $"\tvar type = ((int){(int)node.Type});\n" +
                $"\tvar access = new List<Guid>();\n";
            foreach (Guid roleId in node.Access)
                response += $"\taccess.Add( new Guid(\"{roleId.ToString()}\") );\n";

            response += $"\tvar entityListPages = new List<Guid>();\n";
            foreach (Guid pageId in node.EntityListPages)
                response += $"\tentityListPages.Add( new Guid(\"{pageId.ToString()}\") );\n";

            response += $"\tvar entityCreatePages = new List<Guid>();\n";
            foreach (Guid pageId in node.EntityCreatePages)
                response += $"\tentityCreatePages.Add( new Guid(\"{pageId.ToString()}\") );\n";

            response += $"\tvar entityDetailsPages = new List<Guid>();\n";
            foreach (Guid pageId in node.EntityDetailsPages)
                response += $"\tentityDetailsPages.Add( new Guid(\"{pageId.ToString()}\") );\n";

            response += $"\tvar entityManagePages = new List<Guid>();\n";
            foreach (Guid pageId in node.EntityManagePages)
                response += $"\tentityManagePages.Add( new Guid(\"{pageId.ToString()}\") );\n";


            response += $"\tvar labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();\n";
            foreach (var res in node.LabelTranslations ?? new List<TranslationResource>())
                response += $"\tlabelTranslations.Add( new WebVella.Erp.Web.Models.TranslationResource{{ Locale=\"{res.Locale}\", Key= \"{res.Key}\", Value= @\"{res.Value.EscapeMultiline()}\"  }} );\n";

            response += "\n\tnew WebVella.Erp.Web.Services.AppService().CreateAreaNode(id,areaId,name,label,labelTranslations,iconClass,url,type,entityId,weight,access,entityListPages,entityCreatePages,entityDetailsPages,entityManagePages,WebVella.Erp.Database.DbContext.Current.Transaction,parentId);\n" +
            "}\n" +
            "#endregion\n\n";

            return response;
        }

        private UpdateCheckResponse UpdateSitemapNodeCode(Guid areaId, SitemapNode currentNode, SitemapNode oldNode)
        {
            var response = new UpdateCheckResponse();
            response.Code = string.Empty;
            response.HasUpdate = false;

            if (currentNode.Name != oldNode.Name)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap node name</span>  from <span class='go-red'>{oldNode.Name}</span> to <span class='go-red'>{currentNode.Name}</span>");
            }

            if (currentNode.ParentId != oldNode.ParentId)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap node parent</span>  from <span class='go-red'>{oldNode.ParentId}</span> to <span class='go-red'>{currentNode.ParentId}</span>");
            }

            if (currentNode.Label != oldNode.Label)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap node label</span>  from <span class='go-red'>{oldNode.Label}</span> to <span class='go-red'>{currentNode.Label}</span>");
            }

            if (currentNode.IconClass != oldNode.IconClass)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap node IconClass</span>  from <span class='go-red'>{oldNode.IconClass}</span> to <span class='go-red'>{currentNode.IconClass}</span>");
            }

            if (currentNode.Url != oldNode.Url)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap node url</span>  from <span class='go-red'>{oldNode.Url}</span> to <span class='go-red'>{currentNode.Url}</span>");
            }

            if (currentNode.Weight != oldNode.Weight)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap node weight</span>  from <span class='go-red'>{oldNode.Weight}</span> to <span class='go-red'>{currentNode.Weight}</span>");
            }

            if (currentNode.Type != oldNode.Type)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap node type</span>  from <span class='go-red'>{oldNode.Type}</span> to <span class='go-red'>{currentNode.Type}</span>");
            }

            if (currentNode.Access != null && oldNode.Access != null)
            {
                bool accessChanged = currentNode.Access.Count != oldNode.Access.Count;
                if (!accessChanged)
                {
                    foreach (Guid id in currentNode.Access)
                    {
                        if (!oldNode.Access.Any(x => x == id))
                        {
                            accessChanged = true;
                            break;
                        }
                    }
                }
                if (accessChanged)
                {
                    response.ChangeList.Add($"<span class='go-green label-block'>sitemap node access</span>  access role list changed</span>");
                    response.HasUpdate = true;
                }
            }
            else if (!(currentNode.Access == null && oldNode.Access == null))
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap node access</span>  access role list changed</span>");
            }

            if (currentNode.EntityListPages.Count > 0 && oldNode.EntityListPages.Count > 0)
            {
                bool pageChanged = currentNode.EntityListPages.Count != oldNode.EntityListPages.Count;
                if (!pageChanged)
                {
                    foreach (Guid id in currentNode.EntityListPages)
                    {
                        if (!oldNode.EntityListPages.Any(x => x == id))
                        {
                            pageChanged = true;
                            break;
                        }
                    }
                }
                if (pageChanged)
                {
                    response.ChangeList.Add($"<span class='go-green label-block'>sitemap node pages</span>  entity list array changed</span>");
                    response.HasUpdate = true;
                }
            }
            else if (!(currentNode.EntityListPages.Count == 0 && oldNode.EntityListPages.Count == 0))
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap node pages</span>  entity list array changed</span>");
            }

            if (currentNode.EntityCreatePages.Count > 0 && oldNode.EntityCreatePages.Count > 0)
            {
                bool pageChanged = currentNode.EntityCreatePages.Count != oldNode.EntityCreatePages.Count;
                if (!pageChanged)
                {
                    foreach (Guid id in currentNode.EntityCreatePages)
                    {
                        if (!oldNode.EntityCreatePages.Any(x => x == id))
                        {
                            pageChanged = true;
                            break;
                        }
                    }
                }
                if (pageChanged)
                {
                    response.ChangeList.Add($"<span class='go-green label-block'>sitemap node pages</span>  entity create array changed</span>");
                    response.HasUpdate = true;
                }
            }
            else if (!(currentNode.EntityCreatePages.Count == 0 && oldNode.EntityCreatePages.Count == 0))
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap node pages</span>  entity create array changed</span>");
            }


            if (currentNode.EntityDetailsPages.Count > 0 && oldNode.EntityDetailsPages.Count > 0)
            {
                bool pageChanged = currentNode.EntityDetailsPages.Count != oldNode.EntityDetailsPages.Count;
                if (!pageChanged)
                {
                    foreach (Guid id in currentNode.EntityDetailsPages)
                    {
                        if (!oldNode.EntityDetailsPages.Any(x => x == id))
                        {
                            pageChanged = true;
                            break;
                        }
                    }
                }
                if (pageChanged)
                {
                    response.ChangeList.Add($"<span class='go-green label-block'>sitemap node pages</span>  entity details array changed</span>");
                    response.HasUpdate = true;
                }
            }
            else if (!(currentNode.EntityDetailsPages.Count == 0 && oldNode.EntityDetailsPages.Count == 0))
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap node pages</span>  entity details array changed</span>");
            }

            if (currentNode.EntityManagePages.Count > 0 && oldNode.EntityManagePages.Count > 0)
            {
                bool pageChanged = currentNode.EntityManagePages.Count != oldNode.EntityManagePages.Count;
                if (!pageChanged)
                {
                    foreach (Guid id in currentNode.EntityManagePages)
                    {
                        if (!oldNode.EntityManagePages.Any(x => x == id))
                        {
                            pageChanged = true;
                            break;
                        }
                    }
                }
                if (pageChanged)
                {
                    response.ChangeList.Add($"<span class='go-green label-block'>sitemap node pages</span>  entity manage array changed</span>");
                    response.HasUpdate = true;
                }
            }
            else if (!(currentNode.EntityManagePages.Count == 0 && oldNode.EntityManagePages.Count == 0))
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap node pages</span>  entity manage array changed</span>");
            }

            if (currentNode.LabelTranslations != null && oldNode.LabelTranslations != null)
            {
                bool translationChanged = currentNode.LabelTranslations.Count != oldNode.LabelTranslations.Count;
                if (!translationChanged)
                {
                    foreach (var translation in currentNode.LabelTranslations)
                    {
                        if (!oldNode.LabelTranslations.Any(x => x.Locale == translation.Locale && x.Key == translation.Key && x.Value == translation.Value))
                        {
                            translationChanged = true;
                            break;
                        }
                    }
                }
                if (translationChanged)
                {
                    response.ChangeList.Add($"<span class='go-green label-block'>sitemap node LabelTranslations</span> LabelTranslations changes</span>");
                    response.HasUpdate = true;
                }
            }
            else if (!(currentNode.LabelTranslations == null && oldNode.LabelTranslations == null))
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap node LabelTranslations</span> LabelTranslations changes</span>");
            }

            if (response.HasUpdate)
            {
                response.Code += $"#region << ***Update sitemap node*** Sitemap node name: {currentNode.Name} >>\n" +
            "{\n" +
                $"\tvar id = new Guid(\"{currentNode.Id.ToString()}\");\n" +
                (currentNode.ParentId.HasValue ? $"\tvar parentId = new Guid(\"{currentNode.ParentId.ToString()}\");\n" : $"\tGuid? parentId = null;\n") +
                $"\tvar areaId = new Guid(\"{areaId.ToString()}\");\n" +
                (currentNode.EntityId.HasValue ? $"\tGuid? entityId = new Guid(\"{currentNode.EntityId}\");\n" : $"\tGuid? entityId = null;\n") +
                $"\tvar name = \"{currentNode.Name}\";\n" +
                $"\tvar label = \"{currentNode.Label}\";\n" +
                $"\tvar url = \"{currentNode.Url}\";\n" +
                $"\tvar iconClass = \"{currentNode.IconClass}\";\n" +
                $"\tvar weight = {currentNode.Weight};\n" +
                $"\tvar type = ((int){(int)currentNode.Type});\n" +
                $"\tvar access = new List<Guid>();\n";
                foreach (Guid roleId in currentNode.Access)
                    response.Code += $"\taccess.Add( new Guid(\"{roleId.ToString()}\") );\n";

                response.Code += $"\tvar entityListPages = new List<Guid>();\n";
                foreach (Guid pageId in currentNode.EntityListPages)
                    response.Code += $"\tentityListPages.Add( new Guid(\"{pageId.ToString()}\") );\n";

                response.Code += $"\tvar entityCreatePages = new List<Guid>();\n";
                foreach (Guid pageId in currentNode.EntityCreatePages)
                    response.Code += $"\tentityCreatePages.Add( new Guid(\"{pageId.ToString()}\") );\n";

                response.Code += $"\tvar entityDetailsPages = new List<Guid>();\n";
                foreach (Guid pageId in currentNode.EntityDetailsPages)
                    response.Code += $"\tentityDetailsPages.Add( new Guid(\"{pageId.ToString()}\") );\n";

                response.Code += $"\tvar entityManagePages = new List<Guid>();\n";
                foreach (Guid pageId in currentNode.EntityManagePages)
                    response.Code += $"\tentityManagePages.Add( new Guid(\"{pageId.ToString()}\") );\n";

                response.Code += $"\tvar labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();\n";
                foreach (var res in currentNode.LabelTranslations ?? new List<TranslationResource>())
                    response.Code += $"\tlabelTranslations.Add( new WebVella.Erp.Web.Models.TranslationResource{{ Locale=\"{res.Locale}\", Key= \"{res.Key}\", Value= @\"{res.Value.EscapeMultiline()}\"  }} );\n";

                response.Code += "\n\tnew WebVella.Erp.Web.Services.AppService().UpdateAreaNode(id,areaId,name,label,labelTranslations,iconClass,url,type,entityId,weight,access,entityListPages,entityCreatePages,entityDetailsPages,entityManagePages,WebVella.Erp.Database.DbContext.Current.Transaction,parentId);\n" +
                "}\n" +
                "#endregion\n\n";
            }

            return response;
        }

        private string DeleteSitemapNodeCode(SitemapNode node)
        {
            return $"#region << ***Delete sitemap node*** Node name: {node.Name} >>\n" +
                    "{\n" +
                        $"\n\tnew WebVella.Erp.Web.Services.AppService().DeleteAreaNode( new Guid(\"{node.Id}\"),WebVella.Erp.Database.DbContext.Current.Transaction );\n" +
                    "}\n" +
                    "#endregion\n\n";
        }


        #endregion

        #region <<SitemapGroup>>

        private string CreateSitemapGroupCode(Guid areaId, SitemapGroup group)
        {
            var response = $"#region << ***Create sitemap group*** Sitemap group name: {group.Name} >>\n" +
                "{\n" +
                    $"\tvar id = new Guid(\"{group.Id.ToString()}\");\n" +
                    $"\tvar areaId = new Guid(\"{areaId.ToString()}\");\n" +
                    $"\tvar name = \"{group.Name}\";\n" +
                    $"\tvar label = \"{group.Label}\";\n" +
                    $"\tvar weight = {group.Weight};\n" +
                    $"\tvar renderRoles = new List<Guid>();\n";
            foreach (Guid roleId in group.RenderRoles)
                response += $"\trenderRoles.Add( new Guid(\"{roleId.ToString()}\") );\n";

            response += $"\tvar labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();\n";
            foreach (var res in group.LabelTranslations ?? new List<TranslationResource>())
                response += $"\tlabelTranslations.Add( new WebVella.Erp.Web.Models.TranslationResource{{ Locale=\"{res.Locale}\", Key= \"{res.Key}\", Value= @\"{res.Value.EscapeMultiline()}\"  }} );\n";

            response += "\n\tnew WebVella.Erp.Web.Services.AppService().CreateAreaGroup(id,areaId,name,label,labelTranslations,weight,renderRoles,WebVella.Erp.Database.DbContext.Current.Transaction);\n" +
            "}\n" +
            "#endregion\n\n";

            return response;
        }

        private UpdateCheckResponse UpdateSitemapGroupCode(SitemapGroup currentGroup, SitemapGroup oldGroup)
        {
            var response = new UpdateCheckResponse();
            response.Code = string.Empty;
            response.HasUpdate = false;

            if (currentGroup.Name != oldGroup.Name)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap group name</span>  from <span class='go-red'>{oldGroup.Name}</span> to <span class='go-red'>{currentGroup.Name}</span>");
            }

            if (currentGroup.Label != oldGroup.Label)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap group label</span>  from <span class='go-red'>{oldGroup.Label}</span> to <span class='go-red'>{currentGroup.Label}</span>");
            }

            if (currentGroup.Weight != oldGroup.Weight)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap group weight</span>  from <span class='go-red'>{oldGroup.Weight}</span> to <span class='go-red'>{currentGroup.Weight}</span>");
            }

            if (currentGroup.RenderRoles != null && currentGroup.RenderRoles != null)
            {
                bool accessChanged = currentGroup.RenderRoles.Count != oldGroup.RenderRoles.Count;
                if (!accessChanged)
                {
                    foreach (Guid id in currentGroup.RenderRoles)
                    {
                        if (!oldGroup.RenderRoles.Any(x => x == id))
                        {
                            accessChanged = true;
                            break;
                        }
                    }
                }
                if (accessChanged)
                {
                    response.ChangeList.Add($"<span class='go-green label-block'>sitemap group RenderRoles</span>  render role list changed</span>");
                    response.HasUpdate = true;
                }
            }
            else if (!(currentGroup.RenderRoles == null && oldGroup.RenderRoles == null))
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap group RenderRoles</span>  render  role list changed</span>");
            }

            if (currentGroup.LabelTranslations != null && oldGroup.LabelTranslations != null)
            {
                bool translationChanged = currentGroup.LabelTranslations.Count != oldGroup.LabelTranslations.Count;
                if (!translationChanged)
                {
                    foreach (var translation in currentGroup.LabelTranslations)
                    {
                        if (!oldGroup.LabelTranslations.Any(x => x.Locale == translation.Locale && x.Key == translation.Key && x.Value == translation.Value))
                        {
                            translationChanged = true;
                            break;
                        }
                    }
                }
                if (translationChanged)
                {
                    response.ChangeList.Add($"<span class='go-green label-block'>sitemap group LabelTranslations</span> LabelTranslations changes</span>");
                    response.HasUpdate = true;
                }
            }
            else if (!(currentGroup.LabelTranslations == null && oldGroup.LabelTranslations == null))
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>sitemap group LabelTranslations</span> LabelTranslations changes</span>");
            }

            if (response.HasUpdate)
            {
                response.Code = $"#region << ***Update sitemap group*** Sitemap group name: {currentGroup.Name} >>\n" +
                "{\n" +
                    $"\tvar id = new Guid(\"{currentGroup.Id.ToString()}\");\n" +
                    $"\tvar areaId = new Guid(\"{currentGroup.ToString()}\");\n" +
                    $"\tvar name = \"{currentGroup.Name}\";\n" +
                    $"\tvar label = \"{currentGroup.Label}\";\n" +
                    $"\tvar weight = {currentGroup.Weight};\n" +
                    $"\tvar renderRoles = new List<Guid>();\n";
                foreach (Guid roleId in currentGroup.RenderRoles)
                    response.Code += $"\trenderRoles.Add( new Guid(\"{roleId.ToString()}\") );\n";

                response.Code += $"\tvar labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();\n";
                foreach (var res in currentGroup.LabelTranslations ?? new List<TranslationResource>())
                    response.Code += $"\tlabelTranslations.Add( new WebVella.Erp.Web.Models.TranslationResource{{ Locale=\"{res.Locale}\", Key= \"{res.Key}\", Value= @\"{res.Value.EscapeMultiline()}\"  }} );\n";


                response.Code += "\n\tnew WebVella.Erp.Web.Services.AppService().UpdateAreaGroup(id,areaId,name,label,labelTranslations,weight,renderRoles,WebVella.Erp.Database.DbContext.Current.Transaction);\n" +
                "}\n" +
                "#endregion\n\n";
            }

            return response;
        }

        private string DeleteSitemapGroupCode(SitemapGroup group)
        {
            return $"#region << ***Delete sitemap group*** Sitemap group name: {group.Name} >>\n" +
                    "{\n" +
                        $"\n\tnew WebVella.Erp.Web.Services.PageService().DeleteAreaGroup( new Guid(\"{group.Id}\"),WebVella.Erp.Database.DbContext.Current.Transaction);\n" +
                    "}\n" +
                    "#endregion\n\n";
        }

        #endregion

        #region << ErpPage >>>

        private string CreateErpPageCode(ErpPage page)
        {
            var response = $"#region << ***Create page*** Page name: {page.Name} >>\n" +
            "{\n" +
                $"\tvar id = new Guid(\"{page.Id.ToString()}\");\n" +
                $"\tvar name = @\"{page.Name}\";\n" +
                (page.Label != null ? $"\tvar label = \"{page.Label}\";\n" : "\tstring label = null;\n") +
                (page.IconClass != null ? $"\tvar iconClass = \"{page.IconClass}\";\n" : "\tstring iconClass = null;\n") +
                $"\tvar system = {page.System.ToString().ToLower()};\n" +
                $"\tvar layout = @\"{page.Layout}\";\n" +
                $"\tvar weight = {page.Weight};\n" +
                $"\tvar type = (PageType)((int){(int)page.Type});\n" +
                $"\tvar isRazorBody = {page.IsRazorBody.ToString().ToLower()};\n" +
                (page.AppId.HasValue ? $"\tGuid? appId = new Guid(\"{page.AppId}\");\n" : $"\tGuid? appId = null;\n") +
                (page.EntityId.HasValue ? $"\tGuid? entityId = new Guid(\"{page.EntityId}\");\n" : $"\tGuid? entityId = null;\n") +
                (page.NodeId.HasValue ? $"\tGuid? nodeId = new Guid(\"{page.NodeId}\");\n" : $"\tGuid? nodeId = null;\n") +
                (page.AreaId.HasValue ? $"\tGuid? areaId = new Guid(\"{page.AreaId}\");\n" : $"\tGuid? areaId = null;\n") +
                (page.RazorBody != null ? $"\tstring razorBody = @\"{page.RazorBody.EscapeMultiline()}\";\n" : $"\tstring razorBody = null;\n") +
                $"\tvar labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();\n";

            foreach (var res in page.LabelTranslations ?? new List<TranslationResource>())
                response += $"\tlabelTranslations.Add( new WebVella.Erp.Web.Models.TranslationResource{{ Locale=\"{res.Locale}\", Key= \"{res.Key}\", Value= @\"{res.Value.EscapeMultiline()}\"  }} );\n";

            response += "\n\tnew WebVella.Erp.Web.Services.PageService().CreatePage(id,name,label,labelTranslations,iconClass,system,weight,type,appId,entityId,nodeId,areaId,isRazorBody,razorBody,layout,WebVella.Erp.Database.DbContext.Current.Transaction);\n" +
            "}\n" +
            "#endregion\n\n";

            return response;
        }

        private string DeleteErpPageCode(ErpPage page)
        {
            return $"#region << ***Delete page*** Page name: {page.Name} >>\n" +
                    "{\n" +
                        $"\n\tnew WebVella.Erp.Web.Services.PageService().DeletePage( new Guid(\"{page.Id}\"),WebVella.Erp.Database.DbContext.Current.Transaction, cascade: false );\n" +
                    "}\n" +
                    "#endregion\n\n";
        }

        private UpdateCheckResponse UpdateErpPageCode(ErpPage currentPage, ErpPage oldPage)
        {
            var response = new UpdateCheckResponse();
            response.Code = string.Empty;
            response.HasUpdate = false;

            if (currentPage.Name != oldPage.Name)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>name</span>  from <span class='go-red'>{oldPage.Name}</span> to <span class='go-red'>{currentPage.Name}</span>");
            }

            if (currentPage.Label != oldPage.Label)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>label</span>  from <span class='go-red'>{oldPage.Label}</span> to <span class='go-red'>{currentPage.Label}</span>");
            }

            if (currentPage.IconClass != oldPage.IconClass)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>IconClass</span>  from <span class='go-red'>{oldPage.IconClass}</span> to <span class='go-red'>{currentPage.IconClass}</span>");
            }

            if (currentPage.Layout != oldPage.Layout)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>layout</span>  from <span class='go-red'>{oldPage.Layout}</span> to <span class='go-red'>{currentPage.Layout}</span>");
            }

            if (currentPage.Weight != oldPage.Weight)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>weight</span>  from <span class='go-red'>{oldPage.Weight}</span> to <span class='go-red'>{currentPage.Weight}</span>");
            }

            if (currentPage.Type != oldPage.Type)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>type</span>  from <span class='go-red'>{oldPage.Type}</span> to <span class='go-red'>{currentPage.Type}</span>");
            }

            if (currentPage.System != oldPage.System)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>system</span>  from <span class='go-red'>{oldPage.System}</span> to <span class='go-red'>{currentPage.System}</span>");
            }

            if (currentPage.IsRazorBody != oldPage.IsRazorBody)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>IsRazorBody</span>  from <span class='go-red'>{oldPage.IsRazorBody}</span> to <span class='go-red'>{currentPage.IsRazorBody}</span>");
            }

            if (currentPage.RazorBody.EscapeMultiline() != oldPage.RazorBody.EscapeMultiline())
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>RazorBody</span>  razor body changes</span>");
            }

            if (currentPage.AppId != oldPage.AppId)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>AppId</span>  from <span class='go-red'>{oldPage.AppId}</span> to <span class='go-red'>{currentPage.AppId}</span>");
            }

            if (currentPage.EntityId != oldPage.EntityId)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>EntityId</span>  from <span class='go-red'>{oldPage.EntityId}</span> to <span class='go-red'>{currentPage.EntityId}</span>");
            }

            if (currentPage.AreaId != oldPage.AreaId)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>AreaId</span>  from <span class='go-red'>{oldPage.AreaId}</span> to <span class='go-red'>{currentPage.AreaId}</span>");
            }

            if (currentPage.NodeId != oldPage.NodeId)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>NodeId</span>  from <span class='go-red'>{oldPage.NodeId}</span> to <span class='go-red'>{currentPage.NodeId}</span>");
            }

            if (currentPage.LabelTranslations != null && oldPage.LabelTranslations != null)
            {
                bool translationChanged = currentPage.LabelTranslations.Count != oldPage.LabelTranslations.Count;
                if (!translationChanged)
                {
                    foreach (var translation in currentPage.LabelTranslations)
                    {
                        if (!oldPage.LabelTranslations.Any(x => x.Locale == translation.Locale && x.Key == translation.Key && x.Value == translation.Value))
                        {
                            translationChanged = true;
                            break;
                        }
                    }
                }
                if (translationChanged)
                {
                    response.ChangeList.Add($"<span class='go-green label-block'>LabelTranslations</span> LabelTranslations changes</span>");
                    response.HasUpdate = true;
                }
            }
            else if (!(currentPage.LabelTranslations == null && oldPage.LabelTranslations == null))
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>LabelTranslations</span> LabelTranslations changes</span>");
            }

            if (response.HasUpdate)
            {
                response.Code = $"#region << ***Update page*** Page name: {currentPage.Name} >>\n" +
                "{\n" +
                    $"\tvar id = new Guid(\"{currentPage.Id.ToString()}\");\n" +
                    $"\tvar name = @\"{currentPage.Name}\";\n" +
                    (currentPage.Label != null ? $"\tvar label = \"{currentPage.Label}\";\n" : "\tstring label = null;\n") +
                    (currentPage.IconClass != null ? $"\tvar iconClass = \"{currentPage.IconClass}\";\n" : "\tstring iconClass = null;\n") +
                    $"\tvar system = {currentPage.System.ToString().ToLower()};\n" +
                    $"\tvar layout = @\"{currentPage.Layout}\";\n" +
                    $"\tvar weight = {currentPage.Weight};\n" +
                    $"\tvar type = (PageType)((int){(int)currentPage.Type});\n" +
                    $"\tvar isRazorBody = {currentPage.IsRazorBody.ToString().ToLower()};\n" +
                    (currentPage.AppId.HasValue ? $"\tvar appId = new Guid(\"{currentPage.AppId}\");\n" : $"\tGuid? appId = null;\n") +
                    (currentPage.EntityId.HasValue ? $"\tvar entityId = new Guid(\"{currentPage.EntityId}\");\n" : $"\tGuid? entityId = null;\n") +
                    (currentPage.NodeId.HasValue ? $"\tvar nodeId = new Guid(\"{currentPage.NodeId}\");\n" : $"\tGuid? nodeId = null;\n") +
                    (currentPage.AreaId.HasValue ? $"\tvar areaId = new Guid(\"{currentPage.AreaId}\");\n" : $"\tGuid? areaId = null;\n") +
                    (currentPage.RazorBody != null ? $"\tstring razorBody = @\"{currentPage.RazorBody.EscapeMultiline()}\";\n" : $"\tstring razorBody = null;\n") +

                    $"\tvar labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();\n";
                foreach (var res in currentPage.LabelTranslations ?? new List<TranslationResource>())
                    response.Code += $"\tlabelTranslations.Add( new WebVella.Erp.Web.Models.TranslationResource{{ Locale=\"{res.Locale}\", Key= \"{res.Key}\", Value= @\"{res.Value.EscapeMultiline()}\"  }} );\n";

                response.Code += "\n\tnew WebVella.Erp.Web.Services.PageService().UpdatePage(id,name,label,labelTranslations,iconClass,system,weight,type,appId,entityId,nodeId,areaId,isRazorBody,razorBody,layout,WebVella.Erp.Database.DbContext.Current.Transaction);\n" +
                "}\n" +
                "#endregion\n\n";
            }

            return response;
        }

        #endregion

        #region << PageBodyNode >>>

        private string CreatePageBodyNodeCode(PageBodyNode node, string pageName)
        {
            var response =
            $"#region << ***Create page body node*** Page name: {pageName}  id: {node.Id} >>\n" +
            "{\n" +
                $"\tvar id = new Guid(\"{node.Id.ToString()}\");\n" +
                (node.ParentId.HasValue ? $"\tGuid? parentId = new Guid(\"{node.ParentId}\");\n" : $"\tGuid? parentId = null;\n") +
                (node.NodeId.HasValue ? $"\tGuid? nodeId = new Guid(\"{node.NodeId}\");\n" : $"\tGuid? nodeId = null;\n") +
                $"\tvar pageId = new Guid(\"{node.PageId}\");\n" +
                (node.ComponentName != null ? $"\tvar componentName = \"{node.ComponentName}\";\n" : "\tstring componentName = null;\n") +
                (node.ContainerId != null ? $"\tvar containerId = \"{node.ContainerId}\";\n" : "\tstring containerId = null;\n") +
                (node.Options != null ? $"\tvar options = @\"{node.Options.EscapeMultiline()}\";\n" : "\tstring options = null;\n") +
                $"\tvar weight = {node.Weight};\n" +
                "\n\tnew WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id,parentId,pageId,nodeId,weight,componentName,containerId,options,WebVella.Erp.Database.DbContext.Current.Transaction);\n" +
            "}\n" +
            "#endregion\n\n";

            return response;
        }

        private UpdateCheckResponse UpdatePageBodyNodeCode(PageBodyNode currentNode, PageBodyNode oldNode, string pageName)
        {
            var response = new UpdateCheckResponse();
            response.Code = string.Empty;
            response.HasUpdate = false;

            if (currentNode.ParentId != oldNode.ParentId)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>page node ParentId</span>  from <span class='go-red'>{oldNode.ParentId}</span> to <span class='go-red'>{currentNode.ParentId}</span>");
            }

            if (currentNode.NodeId != oldNode.NodeId)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>page node NodeId</span>  from <span class='go-red'>{oldNode.NodeId}</span> to <span class='go-red'>{currentNode.NodeId}</span>");
            }

            if (currentNode.PageId != oldNode.PageId)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>page node PageId</span>  from <span class='go-red'>{oldNode.PageId}</span> to <span class='go-red'>{currentNode.PageId}</span>");
            }

            if (currentNode.Weight != oldNode.Weight)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>page node weight</span>  from <span class='go-red'>{oldNode.Weight}</span> to <span class='go-red'>{currentNode.Weight}</span>");
            }

            if (currentNode.ComponentName != oldNode.ComponentName)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>page node ComponentName</span>  from <span class='go-red'>{oldNode.ComponentName}</span> to <span class='go-red'>{currentNode.ComponentName}</span>");
            }

            if (currentNode.Options.EscapeMultiline() != oldNode.Options.EscapeMultiline())
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>page node Options</span>  from <span class='go-red'>{oldNode.Options}</span> to <span class='go-red'>{currentNode.Options}</span>");
            }

            if (currentNode.ContainerId != oldNode.ContainerId)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>page node ContainerId</span>  from <span class='go-red'>{oldNode.ContainerId}</span> to <span class='go-red'>{currentNode.ContainerId}</span>");
            }

            if (response.HasUpdate)
            {
                response.Code =
                $"#region << ***Update page body node*** Page: {pageName} ID: {currentNode.Id} >>\n" +
                "{\n" +
                    $"\tvar id = new Guid(\"{currentNode.Id.ToString()}\");\n" +
                    (currentNode.ParentId.HasValue ? $"\tGuid? parentId = new Guid(\"{currentNode.ParentId}\");\n" : $"\tGuid? parentId = null;\n") +
                    (currentNode.NodeId.HasValue ? $"\tGuid? nodeId = new Guid(\"{currentNode.NodeId}\");\n" : $"\tGuid? nodeId = null;\n") +
                    $"\tGuid pageId = new Guid(\"{currentNode.PageId}\");\n" +
                    (currentNode.ComponentName != null ? $"\tvar componentName = \"{currentNode.ComponentName}\";\n" : "\tstring componentName = null;\n") +
                    (currentNode.ContainerId != null ? $"\tvar containerId = \"{currentNode.ContainerId}\";\n" : "\tstring containerId = null;\n") +
                    (currentNode.Options != null ? $"\tvar options = @\"{currentNode.Options.EscapeMultiline()}\";\n" : "\tstring options = null;\n") +
                    $"\tvar weight = {currentNode.Weight};\n" +
                    "\n\tnew WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id,parentId,pageId,nodeId,weight,componentName,containerId,options,WebVella.Erp.Database.DbContext.Current.Transaction);\n" +
                "}\n" +
                "#endregion\n\n";
            }

            return response;
        }

        private string DeletePageBodyNodeCode(PageBodyNode node, string pageName)
        {
            return $"#region << ***Delete page body node*** Page name: {pageName} ID: {node.Id} >>\n" +
                    "{\n" +
                        $"\n\tnew WebVella.Erp.Web.Services.PageService().DeletePageBodyNode( new Guid(\"{node.Id}\"),WebVella.Erp.Database.DbContext.Current.Transaction, cascade: false );\n" +
                    "}\n" +
                    "#endregion\n\n";
        }

        #endregion

        #region << DatabaseDataSources >>

        private string CreateDatabaseDataSourceCode(DatabaseDataSource ds)
        {
            var parametersJson = JsonConvert.SerializeObject(ds.Parameters ?? new List<DataSourceParameter>()).EscapeMultiline();
            var fieldsJson = JsonConvert.SerializeObject(ds.Fields ?? new List<DataSourceModelFieldMeta>()).EscapeMultiline();
            var response =
            $"#region << ***Create data source*** Name: {ds.Name} >>\n" +
            "{\n" +
                $"\tvar id = new Guid(\"{ds.Id.ToString()}\");\n" +
                $"\tvar name = @\"{ds.Name}\";\n" +
                $"\tvar description = @\"{ds.Description.EscapeMultiline()}\";\n" +
                $"\tvar eqlText = @\"{ds.EqlText.EscapeMultiline()}\";\n" +
                $"\tvar sqlText = @\"{ds.SqlText.EscapeMultiline()}\";\n" +
                $"\tvar parametersJson = @\"{parametersJson}\";\n" +
                $"\tvar fieldsJson = @\"{fieldsJson}\";\n" +
                $"\tvar weight = {ds.Weight};\n" +
                $"\tvar entityName =  @\"{ds.EntityName}\";\n" +
                "\n\tnew WebVella.Erp.Database.DbDataSourceRepository().Create(id,name,description,weight,eqlText,sqlText,parametersJson,fieldsJson,entityName);\n" +
            "}\n" +
            "#endregion\n\n";

            return response;
        }

        private UpdateCheckResponse UpdateDatabaseDataSourceCode(DatabaseDataSource currentDS, DatabaseDataSource oldDS)
        {
            var response = new UpdateCheckResponse();
            response.Code = string.Empty;
            response.HasUpdate = false;

            if (currentDS.Name != oldDS.Name)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>data source name</span>  from <span class='go-red'>{oldDS.Name}</span> to <span class='go-red'>{currentDS.Name}</span>");
            }

            if (currentDS.Description.EscapeMultiline() != oldDS.Description.EscapeMultiline())
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>data source description</span>  from <span class='go-red'>{oldDS.Description}</span> to <span class='go-red'>{currentDS.Description}</span>");
            }

            if (currentDS.Weight != oldDS.Weight)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>data source weight</span>  from <span class='go-red'>{oldDS.Weight}</span> to <span class='go-red'>{currentDS.Weight}</span>");
            }

            if (currentDS.SqlText.EscapeMultiline() != oldDS.SqlText.EscapeMultiline())
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>data source sql</span>  from <span class='go-red'>{oldDS.SqlText}</span> to <span class='go-red'>{currentDS.SqlText}</span>");
            }

            if (currentDS.EqlText.EscapeMultiline() != oldDS.EqlText.EscapeMultiline())
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>data source eql</span>  from <span class='go-red'>{oldDS.EqlText}</span> to <span class='go-red'>{currentDS.EqlText}</span>");
            }

            if (currentDS.EntityName != oldDS.EntityName)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>data source entityName</span>  from <span class='go-red'>{oldDS.EntityName}</span> to <span class='go-red'>{currentDS.EntityName}</span>");
            }

            var currentParametersJson = JsonConvert.SerializeObject(currentDS.Parameters ?? new List<DataSourceParameter>()).EscapeMultiline();
            var currentFieldsJson = JsonConvert.SerializeObject(currentDS.Fields ?? new List<DataSourceModelFieldMeta>()).EscapeMultiline();
            var oldParametersJson = JsonConvert.SerializeObject(oldDS.Parameters ?? new List<DataSourceParameter>()).EscapeMultiline();
            var oldFieldsJson = JsonConvert.SerializeObject(oldDS.Fields ?? new List<DataSourceModelFieldMeta>()).EscapeMultiline();

            if (currentParametersJson != oldParametersJson)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>data source parameters</span>  from <span class='go-red'>{oldParametersJson}</span> to <span class='go-red'>{currentParametersJson}</span>");
            }

            if (currentFieldsJson != oldFieldsJson)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>data source fields</span>  from <span class='go-red'>{oldFieldsJson}</span> to <span class='go-red'>{currentFieldsJson}</span>");
            }


            if (response.HasUpdate)
            {

                response.Code =
                $"#region << ***Update data source*** Name: {currentDS.Name} >>\n" +
                "{\n" +
                    $"\tvar id = new Guid(\"{currentDS.Id.ToString()}\");\n" +
                    $"\tvar name = @\"{currentDS.Name}\";\n" +
                    $"\tvar description = @\"{currentDS.Description.EscapeMultiline()}\";\n" +
                    $"\tvar eqlText = @\"{currentDS.EqlText.EscapeMultiline()}\";\n" +
                    $"\tvar sqlText = @\"{currentDS.SqlText.EscapeMultiline()}\";\n" +
                    $"\tvar parametersJson = @\"{currentParametersJson}\";\n" +
                    $"\tvar fieldsJson = @\"{currentFieldsJson}\";\n" +
                    $"\tvar weight = {currentDS.Weight};\n" +
                    $"\tvar entityName =  @\"{currentDS.EntityName}\";\n" +
                    "\n\tnew WebVella.Erp.Database.DbDataSourceRepository().Update(id,name,description,weight,eqlText,sqlText,parametersJson,fieldsJson,entityName);\n" +
                "}\n" +
                "#endregion\n\n";
            }

            return response;
        }

        private string DeleteDatabaseDataSourceCode(DatabaseDataSource ds)
        {
            return $"#region << ***Delete data source *** Name: {ds.Name} >>\n" +
                    "{\n" +
                        $"\n\tnew WebVella.Erp.Api.DataSourceManager().Delete( new Guid(\"{ds.Id}\") );\n" +
                    "}\n" +
                    "#endregion\n\n";
        }

        #endregion

        #region << PageDataSources >>

        private string CreatePageDataSourceCode(PageDataSource ds)
        {
            var parametersJson = JsonConvert.SerializeObject(ds.Parameters ?? new List<DataSourceParameter>()).EscapeMultiline();
            var response =
            $"#region << ***Create page data source*** Name: {ds.Name} >>\n" +
            "{\n" +
                $"\tvar id = new Guid(\"{ds.Id.ToString()}\");\n" +
                $"\tvar pageId = new Guid(\"{ds.PageId.ToString()}\");\n" +
                $"\tvar dataSourceId = new Guid(\"{ds.DataSourceId.ToString()}\");\n" +
                $"\tvar name = @\"{ds.Name}\";\n" +
                $"\tvar parameters = @\"{parametersJson}\";\n" +
                "\n\tnew WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId,name,parameters,WebVella.Erp.Database.DbContext.Current.Transaction );\n" +
            "}\n" +
            "#endregion\n\n";

            return response;
        }

        private UpdateCheckResponse UpdatePageDataSourceCode(PageDataSource currentDS, PageDataSource oldDS)
        {
            var response = new UpdateCheckResponse();
            response.Code = string.Empty;
            response.HasUpdate = false;

            if (currentDS.Name != oldDS.Name)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>page data source name</span>  from <span class='go-red'>{oldDS.Name}</span> to <span class='go-red'>{currentDS.Name}</span>");
            }

            if (currentDS.PageId != oldDS.PageId)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>page data source PageId</span>  from <span class='go-red'>{oldDS.PageId}</span> to <span class='go-red'>{currentDS.PageId}</span>");
            }

            if (currentDS.DataSourceId != oldDS.DataSourceId)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>page data source DataSourceId</span>  from <span class='go-red'>{oldDS.DataSourceId}</span> to <span class='go-red'>{currentDS.DataSourceId}</span>");
            }

            var currentParametersJson = JsonConvert.SerializeObject(currentDS.Parameters ?? new List<DataSourceParameter>()).EscapeMultiline();
            var oldParametersJson = JsonConvert.SerializeObject(oldDS.Parameters ?? new List<DataSourceParameter>()).EscapeMultiline();

            if (currentParametersJson != oldParametersJson)
            {
                response.HasUpdate = true;
                response.ChangeList.Add($"<span class='go-green label-block'>page data source parameters</span>  from <span class='go-red'>{oldParametersJson}</span> to <span class='go-red'>{currentParametersJson}</span>");
            }

            if (response.HasUpdate)
            {

                response.Code =
                $"#region << ***Update page data source*** Name: {currentDS.Name} >>\n" +
                "{\n" +
                    $"\tvar id = new Guid(\"{currentDS.Id.ToString()}\");\n" +
                    $"\tvar pageId = new Guid(\"{currentDS.PageId.ToString()}\");\n" +
                    $"\tvar dataSourceId = new Guid(\"{currentDS.DataSourceId.ToString()}\");\n" +
                    $"\tvar name = @\"{currentDS.Name}\";\n" +
                    $"\tvar parameters = @\"{currentParametersJson}\";\n" +
                    "\n\tnew WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).UpdatePageDataSource(id, pageId, dataSourceId,name,parameters,WebVella.Erp.Database.DbContext.Current.Transaction );\n" +
                "}\n" +
                "#endregion\n\n";
            }

            return response;
        }

        private string DeletePageDataSourceCode(PageDataSource ds)
        {
            return $"#region << ***Delete page data source *** Name: {ds.Name} >>\n" +
                    "{\n" +
                        $"\n\tnew WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).DeletePageDataSource(new Guid(\"{ds.Id}\"),WebVella.Erp.Database.DbContext.Current.Transaction);\n" +
                    "}\n" +
                    "#endregion\n\n";
        }
        #endregion

        #region << Entity Records >>

        private void CompareEntityRecords(DbEntity oldEntity, DbEntity currentEntity, List<EntityRecord> recordsToCreate, List<EntityRecord> recordsToUpdate, List<EntityRecord> recordsToDelete)
        {
            List<string> fieldsToRemoveFromOldEntity = new List<string>();
            foreach (var field in oldEntity.Fields)
            {
                if (!currentEntity.Fields.Any(x => x.Id == field.Id))
                    fieldsToRemoveFromOldEntity.Add(field.Name);
            }

            var oldRecordLists = ReadOldEntityRecords(oldEntity.Name);

            //if any, cleanup old records from fields which don't exist in new entity meta 
            if (fieldsToRemoveFromOldEntity.Any())
            {
                foreach (var rec in oldRecordLists)
                {
                    foreach (var fieldName in fieldsToRemoveFromOldEntity)
                        rec.Properties.Remove(fieldName);
                }
            }


            var oldRecordsDict = oldRecordLists.AsRecordDictionary();
            var entityResult = recMan.Find(new EntityQuery(currentEntity.Name));
            if (!entityResult.Success)
                throw new Exception(entityResult.Message);
            var currentRecordsDict = entityResult.Object.Data.AsRecordDictionary();

            foreach (var key in currentRecordsDict.Keys)
            {
                if (!oldRecordsDict.ContainsKey(key))
                {
                    var currentRec = currentRecordsDict[key];
                    recordsToCreate.Add(currentRec);
                }
                else
                {
                    var currentRec = currentRecordsDict[key];
                    var oldRec = oldRecordsDict[key];
                    var currentRecJson = JsonConvert.SerializeObject(currentRec);
                    var oldRecJson = JsonConvert.SerializeObject(oldRec);
                    if (JsonUtility.NormalizeJsonString(currentRecJson) != JsonUtility.NormalizeJsonString(oldRecJson))
                        recordsToUpdate.Add(currentRec);
                }
            }

            foreach (var key in oldRecordsDict.Keys)
            {
                if (!currentRecordsDict.ContainsKey(key))
                {
                    var oldRec = oldRecordsDict[key];
                    recordsToDelete.Add(oldRec);
                }
            }
        }

        private string CreateRecordCode(EntityRecord rec, DbEntity currentEntity)
        {

            var response = $"#region << ***Create record*** Id: {rec["id"]} ({currentEntity.Name}) >>\n" +
            "{\n" +
                $"\tvar json = @\"{JsonConvert.SerializeObject(rec, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }).EscapeMultiline()}\";\n" +
                $"\tEntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);\n" +
                $"\tvar result = recMan.CreateRecord(\"{currentEntity.Name}\", rec);\n" +
                $"\tif( !result.Success ) throw new Exception(result.Message);\n" +
            "}\n" +
            "#endregion\n\n";

            return response;
        }

        private string UpdateRecordCode(EntityRecord rec, DbEntity currentEntity)
        {
            if (currentEntity.Name == "user")
            {
                var response = $"#region << ***Update record*** Id: {rec["id"]} ({currentEntity.Name}) >>\n" +
                    "//IMPORTANT: User update code generation is NOT SUPPORTED, because password is encrypted and cannot be restored. \n" +
                    "#endregion\n\n";
                return response;
            }
            else
            {
                var response = $"#region << ***Update record*** Id: {rec["id"]} ({currentEntity.Name}) >>\n" +
                "{\n" +
                    $"\tvar json = @\"{JsonConvert.SerializeObject(rec, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }).EscapeMultiline()}\";\n" +
                    $"\tEntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);\n" +
                    $"\tvar result = recMan.UpdateRecord(\"{currentEntity.Name}\", rec);\n" +
                    $"\tif( !result.Success ) throw new Exception(result.Message);\n" +
                "}\n" +
                "#endregion\n\n";

                return response;
            }
        }

        private string DeleteRecordCode(EntityRecord rec, DbEntity currentEntity)
        {
            var response = $"#region << ***Delete record*** Id: {rec["id"]} ({currentEntity.Name}) >>\n" +
            "{\n" +
                $"\tvar id = new Guid(\"{rec["id"]}\");\n" +
                $"\tvar result = recMan.DeleteRecord(\"{currentEntity.Name}\", id);\n" +
                $"\tif( !result.Success ) throw new Exception(\"Failed delete record {rec["id"]}\");\n" +
            "}\n" +
            "#endregion\n\n";

            return response;
        }
        #endregion

        #region << Relation Records >>

        private string CreateNNRelationRecordCode(EntityRelation relation, Guid originId, Guid targetId)
        {

            var response = $"#region << ***Create NN relation record*** Relation: {relation.Label} >>\n" +
            "{\n" +
                $"\tvar result = recMan.CreateRelationManyToManyRecord(new Guid(\"{relation.Id}\"), new Guid(\"{originId}\"), new Guid(\"{targetId}\"));\n" +
                $"\tif( !result.Success ) throw new Exception(result.Message);\n" +
            "}\n" +
            "#endregion\n\n";

            return response;
        }

        private string DeleteNNRelationRecordCode(EntityRelation relation, Guid originId, Guid targetId)
        {
            var response = $"#region << ***Delete NN relation record*** Relation: {relation.Label} >>\n" +
            "{\n" +
                $"\tvar result = recMan.RemoveRelationManyToManyRecord(new Guid(\"{relation.Id}\"), new Guid(\"{originId}\"), new Guid(\"{targetId}\"));\n" +
                $"\tif( !result.Success ) throw new Exception(result.Message);\n" +
            "}\n" +
            "#endregion\n\n";

            return response;
        }
        #endregion
    }

    internal static class Extensions
    {
        public static string EscapeMultiline(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            return str.Replace("\"", "\"\"").Replace(Environment.NewLine, "\n");
        }
    }

    class JsonUtility
    {
        public static string NormalizeJsonString(string json)
        {
            // Parse json string into JObject.
            var parsedObject = JObject.Parse(json);

            // Sort properties of JObject.
            var normalizedObject = SortPropertiesAlphabetically(parsedObject);

            // Serialize JObject .
            return JsonConvert.SerializeObject(normalizedObject);
        }

        private static JObject SortPropertiesAlphabetically(JObject original)
        {
            var result = new JObject();

            foreach (var property in original.Properties().ToList().OrderBy(p => p.Name))
            {
                var value = property.Value as JObject;

                if (value != null)
                {
                    value = SortPropertiesAlphabetically(value);
                    result.Add(property.Name, value);
                }
                else
                {
                    result.Add(property.Name, property.Value);
                }
            }

            return result;
        }
    }
}
