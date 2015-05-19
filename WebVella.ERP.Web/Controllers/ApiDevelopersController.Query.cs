using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;
using System.Net;
using Newtonsoft.Json.Linq;
using WebVella.ERP.Utilities.Dynamic;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebVella.ERP.Web.Controllers
{
    public partial class ApiDevelopersController : ApiControllerBase
    {
        public ApiDevelopersController(IERPService service) : base(service)
        {
        }

        [AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/meta/developers/query/create-sample-query-data-structure")]
        public IActionResult CreateSampleQueryDataStructure()
        {
            Guid userId = new Guid("f5588278-c0a1-4865-ac94-41dfa09bf8ac");
            Guid authorId = new Guid("031cca7c-1da4-48d3-a8e6-8e9315643b13");

            Guid postEntityId = new Guid("3fc00d85-7864-464c-8a8e-bfb85f7134e0");
            Guid authorEntityId = new Guid("59cc62ca-bf44-46f1-bdc8-daa4a538a567");
            Guid categoryEntityId = new Guid("8cf4c663-5029-4b18-b5df-108af707584c");

            Guid postCategoriesRelationId = new Guid("5f45fdde-b569-4b47-b667-63ff3c8bb423");
            Guid postAuthorRelationId = new Guid("2f7e77fd-a748-4cdd-8906-01bf8a46a664");

            EntityManager em = new EntityManager(service.StorageService);
            EntityRelationManager rm = new EntityRelationManager(service.StorageService);
          
            #region << Create Entities >>

            //delete entities and create new one
            em.DeleteEntity(postEntityId);
            em.DeleteEntity(categoryEntityId);
            em.DeleteEntity(authorEntityId);

            InputEntity inputPostEntity = new InputEntity();
            inputPostEntity.Id = postEntityId;
            inputPostEntity.Name = "query_test_post";
            inputPostEntity.Label = "Post";
            inputPostEntity.LabelPlural = "Posts";
            inputPostEntity.System = false;
            inputPostEntity.RecordPermissions = new RecordPermissions();
            inputPostEntity.RecordPermissions.CanCreate.Add(userId);
            inputPostEntity.RecordPermissions.CanDelete.Add(userId);
            inputPostEntity.RecordPermissions.CanRead.Add(userId);
            inputPostEntity.RecordPermissions.CanUpdate.Add(userId);
            Entity postEntity = null;
            {
                var result = em.CreateEntity(inputPostEntity);
                if (!result.Success)
                    return DoResponse(result);

                postEntity = result.Object;
            }

            InputEntity inputAuthorEntity = new InputEntity();
            inputAuthorEntity.Id = authorEntityId;
            inputAuthorEntity.Name = "query_test_author";
            inputAuthorEntity.Label = "Author";
            inputAuthorEntity.LabelPlural = "Authors";
            inputAuthorEntity.System = false;
            inputAuthorEntity.RecordPermissions = new RecordPermissions();
            inputAuthorEntity.RecordPermissions.CanCreate.Add(userId);
            inputAuthorEntity.RecordPermissions.CanDelete.Add(userId);
            inputAuthorEntity.RecordPermissions.CanRead.Add(userId);
            inputAuthorEntity.RecordPermissions.CanUpdate.Add(userId);
            Entity authorEntity = null;
            {
                var result = em.CreateEntity(inputAuthorEntity);
                if (!result.Success)
                    return DoResponse(result);

                authorEntity = result.Object;
            }

            InputEntity inputCategoryEntity = new InputEntity();
            inputCategoryEntity.Id = categoryEntityId;
            inputCategoryEntity.Name = "query_test_category";
            inputCategoryEntity.Label = "Category";
            inputCategoryEntity.LabelPlural = "Categories";
            inputCategoryEntity.System = false;
            inputCategoryEntity.RecordPermissions = new RecordPermissions();
            inputCategoryEntity.RecordPermissions.CanCreate.Add(userId);
            inputCategoryEntity.RecordPermissions.CanDelete.Add(userId);
            inputCategoryEntity.RecordPermissions.CanRead.Add(userId);
            inputCategoryEntity.RecordPermissions.CanUpdate.Add(userId);
            Entity categoryEntity = null;
            {
                var result = em.CreateEntity(inputCategoryEntity);
                if (!result.Success)
                    return DoResponse(result);

                categoryEntity = result.Object;
            }

            #endregion

            #region << Create Author entity fields >>

            TextField authorName = new TextField();
            authorName.Id = Guid.NewGuid();
            authorName.Name = "name";
            authorName.Label = "Name";
            authorName.Required = true;
            authorName.Unique = true;
            authorName.DefaultValue = string.Empty;
            authorName.System = false;
            {
                var result = em.CreateField(authorEntity.Id.Value, authorName);
                if (!result.Success)
                    return DoResponse(result);
            }

            #endregion

            #region << Create Post entity fields >>

            TextField postTitle = new TextField();
            postTitle.Id = Guid.NewGuid();
            postTitle.Name = "title";
            postTitle.Label = "Title";
            postTitle.Required = true;
            postTitle.Unique = false;
            postTitle.DefaultValue = string.Empty;
            postTitle.System = false;
            {
                var result = em.CreateField(postEntity.Id.Value, postTitle);
                if (!result.Success)
                    return DoResponse(result);
            }

            TextField postContent = new TextField();
            postContent.Id = Guid.NewGuid();
            postContent.Name = "content";
            postContent.Label = "Content";
            postContent.Required = false;
            postContent.Unique = false;
            postContent.DefaultValue = string.Empty;
            postContent.System = false;
            {
                var result = em.CreateField(postEntity.Id.Value, postContent);
                if (!result.Success)
                    return DoResponse(result);
            }

            GuidField postAuthor = new GuidField();
            postAuthor.Id = Guid.NewGuid();
            postAuthor.Name = "author";
            postAuthor.Label = "Author";
            postAuthor.Required = true;
            postAuthor.Unique = false;
            postAuthor.DefaultValue = null;
            postAuthor.GenerateNewId = true;
            postAuthor.System = false;
            {
                var result = em.CreateField(postEntity.Id.Value, postAuthor);
                if (!result.Success)
                    return DoResponse(result);
            }

            #endregion

            #region << Create Category entity fields >>

            TextField categoryName = new TextField();
            categoryName.Id = Guid.NewGuid();
            categoryName.Name = "name";
            categoryName.Label = "Name";
            categoryName.Required = true;
            categoryName.Unique = true;
            categoryName.DefaultValue = string.Empty;
            categoryName.System = false;
            {
                var result = em.CreateField(categoryEntity.Id.Value, categoryName);
                if (!result.Success)
                    return DoResponse(result);
            }

            #endregion

            #region << Create relations >>
            
            //reload entities with all fields
            postEntity = em.ReadEntity(postEntityId).Object;
            categoryEntity = em.ReadEntity(categoryEntityId).Object;
            authorEntity = em.ReadEntity(authorEntityId).Object;

            //drop all relations
            rm.Delete(postCategoriesRelationId);
            rm.Delete(postAuthorRelationId);

            EntityRelation postCategoriesRelation = new EntityRelation();
            postCategoriesRelation.Id = postCategoriesRelationId;
            postCategoriesRelation.Name = "query_test_post_categories";
            postCategoriesRelation.Label = "Post categories";
            postCategoriesRelation.RelationType = EntityRelationType.ManyToMany;
            postCategoriesRelation.TargetEntityId = postEntity.Id.Value;
            postCategoriesRelation.TargetFieldId = postEntity.Fields.Single(x => x.Name == "id").Id.Value;
            postCategoriesRelation.OriginEntityId = categoryEntity.Id.Value;
            postCategoriesRelation.OriginFieldId = categoryEntity.Fields.Single(x => x.Name == "id").Id.Value;
            {
                var result = rm.Create(postCategoriesRelation);
                if (!result.Success)
                    return DoResponse(result);
            }


            EntityRelation postAuthorRelation = new EntityRelation();
            postAuthorRelation.Id = postAuthorRelationId;
            postAuthorRelation.Name = "query_test_post_author";
            postAuthorRelation.Label = "Post author";
            postAuthorRelation.RelationType = EntityRelationType.OneToMany;
            postAuthorRelation.OriginEntityId = authorEntity.Id.Value;
            postAuthorRelation.OriginFieldId = authorEntity.Fields.Single(x => x.Name == "id").Id.Value;
            postAuthorRelation.TargetEntityId = postEntity.Id.Value;
            postAuthorRelation.TargetFieldId = postAuthor.Id.Value;
            {
                var result = rm.Create(postAuthorRelation);
                if (!result.Success)
                    return DoResponse(result);
            }

            #endregion


            EntityRecord author = new EntityRecord();
            author["id"] = authorId;
            author["name"] = "Test author name";
            author["created_by"] = userId;
            author["last_modified_by"] = userId;
            author["created_on"] = DateTime.UtcNow;
            RecordManager recMan = new RecordManager(service);
            {
                SingleRecordResponse result = recMan.CreateRecord("query_test_author", author);
                if (!result.Success)
                    return DoResponse(result);
            }

            return Json("ok");

        }

        [AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/meta/developers/query/execute-sample-query")]
        public IActionResult ExecuteSampleQuery()
        {
            QueryResponse response = new QueryResponse();
            response.Success = true;
            response.Timestamp = DateTime.UtcNow;
            response.Message = "ExecuteSampleQuery:DONE";
            return Json(response);
        }



        //[AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/meta/entity")]
        //public IActionResult CreateEntity([FromBody]EntityRecord obj)
        //{
        //	var h = obj.GetProperties();

        //	var t = Json(obj);


        //	return Json(obj);
        //}


    }
}

/*
EntityManager em = new EntityManager(service.StorageService);
            EntityRelationManager rm = new EntityRelationManager(service.StorageService);

            // create relation
            var userEntity = em.ReadEntity("user");
            var areaEntity = em.ReadEntity("area");
            var roleEntity = em.ReadEntity("role");

            EntityRelation create = new EntityRelation();
            create.Name = "area_user_create_by";
            create.Label = "area_user_create_by label";
            create.Description = "area_user_create_by description";
            create.System = true;
            create.OriginEntityId = areaEntity.Object.Id.Value;
            create.OriginFieldId = areaEntity.Object.Fields.Single(x => x.Name == "created_by").Id.Value;

            create.TargetEntityId = userEntity.Object.Id.Value;
            create.TargetFieldId = userEntity.Object.Fields.Single(x => x.Name == "id").Id.Value;

            return DoResponse(rm.Create(create));


            return DoResponse(rm.Read("area_user_create_by"));

            // Guid recId = Guid.NewGuid();

            // EntityRecord record = new EntityRecord();
            // record["id"] = recId;
            // record["email"] = "test email";
            // RecordManager rm = new RecordManager(service);
            // rm.CreateRecord("user", record);



            // var queryObject = EntityQuery.QueryEQ("id", recId );
            // EntityQuery query = new EntityQuery("user", "id,email", queryObject);
            // var result = rm.Find(query);

            //return Json(result);*/
