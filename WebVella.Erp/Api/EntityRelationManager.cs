using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Database;

namespace WebVella.Erp.Api
{
	public class EntityRelationManager
    {
		private DbContext suppliedContext = null;
		private DbContext CurrentContext
		{
			get
			{
				if (suppliedContext != null)
					return suppliedContext;
				else
					return DbContext.Current;
			}
		}

		public EntityRelationManager(DbContext currentContext = null)
		{
			if (currentContext != null)
				suppliedContext = currentContext;
		}

		#region << Validation >>  

		private enum ValidationType
        {
            Create, //indicates the relation will be created
            Update, //indicated the existing relation will be updated
            RelationsOnly //indicated the relation exists and should be only validated for correct entities and fields
        }

        private List<ErrorModel> ValidateRelation(EntityRelation relation, ValidationType validationType)
        {
            List<ErrorModel> errors = new List<ErrorModel>();
			   var entMan = new EntityManager();
				//Postgres column name width limit
				if(relation.Name.Length > 63)
					errors.Add(new ErrorModel("name", relation.Name, "Relation name length exceeded. Should be up to 63 chars!"));
            if (validationType == ValidationType.Update)
            {
                //we cannot update relation with missing Id (Guid.Empty means id is missing)
                //of if there is no relation with this id already                
                if (relation.Id == Guid.Empty)
                    errors.Add(new ErrorModel("id", null, "Id is required!"));
                else if (Read(relation.Id).Object == null)
                    errors.Add(new ErrorModel("id", relation.Id.ToString(), "Entity relation with such Id does not exist!"));
            }
            else if (validationType == ValidationType.Create)
            {
                //if id is null, them we later will assing one before create process
                //otherwise check if relation with same id already exists
                if (relation.Id != Guid.Empty && (Read(relation.Id).Object != null))
                    errors.Add(new ErrorModel("id", relation.Id.ToString(), "Entity relation with such Id already exist!"));

            }
            else if (validationType == ValidationType.RelationsOnly)
            {
                //no need to check anything, we need to check only Entities and Fields relations
                //this case is here only for readability
            }

            EntityRelation existingRelation = null;
            if (validationType == ValidationType.Create || validationType == ValidationType.Update)
            {
                //validate name
                // - if name string is correct
                // - then if relation with same name already exists
                var nameValidationErrors = ValidationUtility.ValidateName(relation.Name);
                if (nameValidationErrors.Count > 0)
                    errors.AddRange(nameValidationErrors);
                else
                {
                    existingRelation = Read(relation.Name).Object;
                    if (validationType == ValidationType.Create)
                    {
                        //if relation with same name alfready exists
                        if (existingRelation != null)
                            errors.Add(new ErrorModel("name", relation.Name, string.Format("Entity relation '{0}' exists already!", relation.Name)));
                    }
                    else if (validationType == ValidationType.Update)
                    {
                        //if relation with same name alfready and different Id already exists
                        if (existingRelation != null && existingRelation.Id != relation.Id)
                            errors.Add(new ErrorModel("name", relation.Name, string.Format("Entity relation '{0}' exists already!", relation.Name)));
                    }
                }
            }
            else if (validationType == ValidationType.RelationsOnly)
            {
                //no need to check anything, we need to check only Entities and Fields relations
                //this case is here only for readability
            }

            errors.AddRange(ValidationUtility.ValidateLabel(relation.Label));

            Entity originEntity = entMan.ReadEntity(relation.OriginEntityId).Object;
            Entity targetEntity = entMan.ReadEntity(relation.TargetEntityId).Object;
            Field originField = null;
            Field targetField = null;

            if (originEntity == null)
                errors.Add(new ErrorModel("originEntity", relation.OriginEntityId.ToString(), "The origin entity do not exist."));
            else
            {
                originField = originEntity.Fields.SingleOrDefault(x => x.Id == relation.OriginFieldId);
                if (originField == null)
                    errors.Add(new ErrorModel("originField", relation.OriginFieldId.ToString(), "The origin field do not exist."));
                if (!(originField is GuidField))
                    errors.Add(new ErrorModel("originField", relation.OriginFieldId.ToString(), "The origin field should be Unique Identifier (GUID) field."));
            }

            if (targetEntity == null)
                errors.Add(new ErrorModel("targetEntity", relation.TargetEntityId.ToString(), "The target entity do not exist."));
            else
            {
                targetField = targetEntity.Fields.SingleOrDefault(x => x.Id == relation.TargetFieldId);
                if (targetField == null)
                    errors.Add(new ErrorModel("targetField", relation.TargetFieldId.ToString(), "The target field do not exist."));
                if (!(targetField is GuidField))
                    errors.Add(new ErrorModel("targetField", relation.TargetFieldId.ToString(), "The target field should be Unique Identifier (GUID) field."));
            }


            //the second level validation requires no errors on first one
            //so if there are errors in first level we return them
            if (errors.Count > 0)
                return errors;


            if (validationType == ValidationType.Update)
            {
                if (existingRelation.RelationType != relation.RelationType)
                    errors.Add(new ErrorModel("relationType", relation.RelationType.ToString(),
                        "The initialy selected relation type is readonly and cannot be changed."));

                if (existingRelation.OriginEntityId != relation.OriginEntityId)
                    errors.Add(new ErrorModel("originEntityId", relation.OriginEntityId.ToString(),
                        "The origin entity differ from initial one. The initialy selected origin entity is readonly and cannot be changed."));

                if (existingRelation.OriginFieldId != relation.OriginFieldId)
                    errors.Add(new ErrorModel("originFieldId", relation.OriginFieldId.ToString(),
                        "The origin field differ from initial one. The initialy selected origin field is readonly and cannot be changed."));

                if (existingRelation.TargetEntityId != relation.TargetEntityId)
                    errors.Add(new ErrorModel("targetEntityId", relation.TargetEntityId.ToString(),
                        "The target entity differ from initial one. The initialy selected target entity is readonly and cannot be changed."));

                if (existingRelation.TargetFieldId != relation.TargetFieldId)
                    errors.Add(new ErrorModel("TargetFieldId", relation.TargetFieldId.ToString(),
                        "The target field differ from initial one. The initialy selected target field is readonly and cannot be changed."));
            }
            else if (validationType == ValidationType.Create)
            {
                if (relation.RelationType == EntityRelationType.OneToMany || relation.RelationType == EntityRelationType.OneToOne )
                {
                    //validate if target and origin field is same field for following relations
                    if (relation.OriginEntityId == relation.TargetEntityId && relation.OriginFieldId == relation.TargetFieldId)
                        errors.Add(new ErrorModel("", "", "The origin and target fields cannot be the same."));

                    //validate there is no other already existing relation with same parameters
                    foreach (var rel in Read().Object)
                    {
                        if (rel.OriginEntityId == relation.OriginEntityId && rel.TargetEntityId == relation.TargetEntityId &&
                            rel.OriginFieldId == relation.OriginFieldId && rel.TargetFieldId == relation.TargetFieldId)
                        {
                            errors.Add(new ErrorModel("", "", "There is already existing relation with same parameters."));
                        }
                    }

                }



                if (relation.RelationType == EntityRelationType.OneToOne || relation.RelationType == EntityRelationType.ManyToMany)
                {
                    if (!originField.Required)
                        errors.Add(new ErrorModel("originFieldId", relation.OriginFieldId.ToString(), "The origin field must be specified as Required"));

                    if (!originField.Unique)
                        errors.Add(new ErrorModel("originFieldId", relation.OriginFieldId.ToString(), "The origin field must be specified as Unique"));

                    if (!targetField.Required)
                        errors.Add(new ErrorModel("targetFieldId", relation.TargetFieldId.ToString(), "The target field must be specified as Required"));

                    if (!targetField.Unique)
                        errors.Add(new ErrorModel("targetFieldId", relation.TargetFieldId.ToString(), "The target field must be specified as Unique"));
                }

                if (relation.RelationType == EntityRelationType.OneToMany)
                {
                    if (!originField.Required)
                        errors.Add(new ErrorModel("originFieldId", relation.OriginFieldId.ToString(), "The origin field must be specified as Required"));

                    if (!originField.Unique)
                        errors.Add(new ErrorModel("originFieldId", relation.OriginFieldId.ToString(), "The origin field must be specified as Unique"));
                }
            }
            if (validationType == ValidationType.RelationsOnly)
            {
                if (relation.RelationType == EntityRelationType.OneToOne || relation.RelationType == EntityRelationType.ManyToMany)
                {
                    if (!originField.Required)
                        errors.Add(new ErrorModel("originFieldId", relation.OriginFieldId.ToString(), "The origin field must be specified as Required"));

                    if (!originField.Unique)
                        errors.Add(new ErrorModel("originFieldId", relation.OriginFieldId.ToString(), "The origin field must be specified as Unique"));

                    if (!targetField.Required)
                        errors.Add(new ErrorModel("targetFieldId", relation.TargetFieldId.ToString(), "The target field must be specified as Required"));

                    if (!targetField.Unique)
                        errors.Add(new ErrorModel("targetFieldId", relation.TargetFieldId.ToString(), "The target field must be specified as Unique"));
                }

                if (relation.RelationType == EntityRelationType.OneToMany)
                {
                    if (!originField.Required)
                        errors.Add(new ErrorModel("originFieldId", relation.OriginFieldId.ToString(), "The origin field must be specified as Required"));

                    if (!originField.Unique)
                        errors.Add(new ErrorModel("originFieldId", relation.OriginFieldId.ToString(), "The origin field must be specified as Unique"));
                }
            }

            return errors;
        }

        #endregion

        public EntityRelationResponse Read(string name)
        {
            EntityRelationResponse response = new EntityRelationResponse();
            response.Timestamp = DateTime.UtcNow;
            response.Object = null;

            if (string.IsNullOrWhiteSpace(name))
            {
                response.Success = false;
                response.Errors.Add(new ErrorModel("name", null, "The name argument is NULL."));
                return response;
            }

			var relations = Cache.GetRelations();
			if( relations != null )
			{
				response.Object = relations.SingleOrDefault(x => x.Name == name);
				response.Success = true;
				if(response.Object!=null)
					response.Message = "The entity relation was successfully returned!";
				else
					response.Message = string.Format("The entity relation '{0}' does not exist!", name);
				return response;
			}

            try
            {
				relations = Read().Object;
				response.Object = relations.SingleOrDefault(x => x.Name == name);
				response.Success = false;

                if (response.Object != null)
                {
                    response.Success = true;
                    response.Message = "The entity relation was successfully returned!";
                }
                else
                {
                    response.Success = true;
                    response.Message = string.Format("The entity relation '{0}' does not exist!", name);
                }
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
				
				if (ErpSettings.DevelopmentMode)
					response.Message = e.Message + e.StackTrace;

                return response;
            }
        }

        public EntityRelationResponse Read(Guid relationId)
        {
            EntityRelationResponse response = new EntityRelationResponse();
            response.Timestamp = DateTime.UtcNow;
            response.Object = null;
            response.Success = true;
            try
            {
				var relations = Cache.GetRelations();
				if (relations != null)
				{
					response.Object = relations.SingleOrDefault(x => x.Id == relationId);
					response.Success = true;
					if (response.Object != null)
						response.Message = "The entity relation was successfully returned!";
					else
						response.Message = string.Format("The entity relation with id '{0}' does not exist!", relationId);
					return response;
				}
				
				relations = Read().Object;
				response.Object = relations.SingleOrDefault( x=>x.Id == relationId);
                if (response.Object != null)
                {
                    response.Message = "The entity relation was successfully returned!";
                }
                else
                    response.Message = string.Format("The entity relation '{0}' does not exist!", relationId);

                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
				if (ErpSettings.DevelopmentMode)
					response.Message = e.Message + e.StackTrace;
                return response;
            }
        }

        public EntityRelationListResponse Read(List<DbEntity> storageEntityList = null)
        {
            EntityRelationListResponse response = new EntityRelationListResponse();
            response.Timestamp = DateTime.UtcNow;
            response.Object = null;

            try
            {
				var relations = Cache.GetRelations();
				if (relations != null)
				{
					response.Object = relations;
					response.Hash = Cache.GetRelationsHash();
					response.Success = true;
					response.Message = null;
					return response;
				}

				relations = CurrentContext.RelationRepository.Read().Select(x => x.MapTo<EntityRelation>()).ToList();

				List<DbEntity> dbEntities = storageEntityList;
				if(dbEntities == null) {
					dbEntities = new DbEntityRepository(CurrentContext).Read();
				}
				foreach( EntityRelation relation in relations )
				{
					var originEntity = dbEntities.Single(x => x.Id == relation.OriginEntityId);
					var targetEntity = dbEntities.Single(x => x.Id == relation.TargetEntityId);
					relation.OriginEntityName = originEntity.Name;
					relation.TargetEntityName = targetEntity.Name;
					relation.OriginFieldName = originEntity.Fields.Single(x => x.Id == relation.OriginFieldId).Name;
					relation.TargetFieldName = targetEntity.Fields.Single(x => x.Id == relation.TargetFieldId).Name;
				}

				if( relations != null )
					Cache.AddRelations(relations);

				//we use instance from cache as return value, because in cache we deepcopy collection
				response.Object = Cache.GetRelations();
				response.Hash = Cache.GetRelationsHash();
				response.Success = true;
                response.Message = null;
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Object = null;
				response.Hash = null;
                response.Timestamp = DateTime.UtcNow;
				if (ErpSettings.DevelopmentMode)
					response.Message = e.Message + e.StackTrace;
                return response;
            }
        }

        public EntityRelationResponse Create(EntityRelation relation)
        {
			if (!string.IsNullOrWhiteSpace(relation.Name))
			{
				relation.Name = relation.Name.Trim();
			}

			EntityRelationResponse response = new EntityRelationResponse();
            response.Timestamp = DateTime.UtcNow;
            response.Object = relation;

			bool hasPermisstion = SecurityContext.HasMetaPermission();
			if (!hasPermisstion)
			{
				response.StatusCode = HttpStatusCode.Forbidden;
				response.Success = false;
				response.Message = "User have no permissions to manipulate erp meta.";
				response.Errors.Add(new ErrorModel { Message = "Access denied." });
				return response;
			}

			response.Errors = ValidateRelation(relation, ValidationType.Create);
            if (response.Errors.Count > 0)
            {
                response.Success = false;
                response.Message = "The entity relation was not created. Validation error occurred!";
                return response;
            }

            try
            {
                var storageRelation = relation.MapTo<DbEntityRelation>();

				storageRelation.Name = storageRelation.Name.Trim();

				if (storageRelation.Id == Guid.Empty)
                    storageRelation.Id = Guid.NewGuid();

                var success = CurrentContext.RelationRepository.Create(storageRelation);
				Cache.Clear();
                if (success)
                {
                    response.Success = true;
                    response.Message = "The entity relation was successfully created!";
                    return response;
                }
                else
                {

                    response.Success = false;
                    response.Message = "The entity relation was not created! An internal error occurred!";
                    return response;
                }
            }
            catch (Exception e)
            {
				Cache.Clear();
                response.Success = false;
                response.Object = relation;
                response.Timestamp = DateTime.UtcNow;
				
				if (ErpSettings.DevelopmentMode)
					response.Message = e.Message + e.StackTrace;
				else
					response.Message = "The entity relation was not created. An internal error occurred!";

                return response;
            }
        }

        public EntityRelationResponse Update(EntityRelation relation)
        {
            EntityRelationResponse response = new EntityRelationResponse();
            response.Timestamp = DateTime.UtcNow;
            response.Object = relation;

			bool hasPermisstion = SecurityContext.HasMetaPermission();
			if (!hasPermisstion)
			{
				response.StatusCode = HttpStatusCode.Forbidden;
				response.Success = false;
				response.Message = "User have no permissions to manipulate erp meta.";
				response.Errors.Add(new ErrorModel { Message = "Access denied." });
				return response;
			}

			response.Errors = ValidateRelation(relation, ValidationType.Update);
            if (response.Errors.Count > 0)
            {
                response.Success = false;
                response.Message = "The entity relation was not updated. Validation error occurred!";
                return response;
            }

            try
            {
                var storageRelation = relation.MapTo<DbEntityRelation>();
				storageRelation.Name = storageRelation.Name.Trim();
				var success = CurrentContext.RelationRepository.Update(storageRelation);
				Cache.Clear();
                if (success)
                {
                    response.Success = true;
                    response.Message = "The entity relation was successfully updated!";
                    return response;
                }
                else
                {

                    response.Success = false;
                    response.Message = "The entity relation was not updated! An internal error occurred!";
                    return response;
                }
            }
            catch (Exception e)
            {
                Cache.Clear();
				response.Success = false;
                response.Object = relation;
                response.Timestamp = DateTime.UtcNow;
				
				if (ErpSettings.DevelopmentMode)
					response.Message = e.Message + e.StackTrace;
				else
					response.Message = "The entity relation was not updated. An internal error occurred!";

                return response;
            }
        }

        public EntityRelationResponse Delete(Guid relationId)
        {
            EntityRelationResponse response = new EntityRelationResponse();
            response.Timestamp = DateTime.UtcNow;
            response.Success = false;
            response.Object = null;

			bool hasPermisstion = SecurityContext.HasMetaPermission();
			if (!hasPermisstion)
			{
				response.StatusCode = HttpStatusCode.Forbidden;
				response.Success = false;
				response.Message = "User have no permissions to manipulate erp meta.";
				response.Errors.Add(new ErrorModel { Message = "Access denied." });
				return response;
			}

			try
			{

                var storageRelation = CurrentContext.RelationRepository.Read(relationId);
				Cache.Clear();
                if (storageRelation != null)
                {
					CurrentContext.RelationRepository.Delete(relationId);
                    response.Object = storageRelation.MapTo<EntityRelation>();
                    response.Success = true;
                    response.Message = "The entity relation was deleted!";
                    return response;
                }
                else
                {
                    response.Message = string.Format("The entity relation was not deleted! No instance with specified id ({0}) was found!", relationId);
                    return response;
                }
            }
            catch (Exception e)
            {
				Cache.Clear();
				
				if (ErpSettings.DevelopmentMode)
					response.Message = string.Format("Relation ID: {0}, /r/nMessage:{1}/r/nStackTrace:{2}", relationId, e.Message, e.StackTrace);
				else
					response.Message = "The entity relation was not delete. An internal error occurred!";

                return response;
            }
        }

    }
}
