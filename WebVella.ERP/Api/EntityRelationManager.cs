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
        private IStorageService storageService;
        private IStorageEntityRelationRepository relationRepository;
        private IStorageEntityRepository entityRepository;

        public EntityRelationManager(IStorageService storageService)
        {
            if (storageService == null)
                throw new ArgumentNullException("storageService", "The storage service is required.");

            this.storageService = storageService;
            relationRepository = storageService.GetEntityRelationRepository();
            entityRepository = storageService.GetEntityRepository();
        }

        #region <--- Validation --->

        private enum ValidationType
        {
            Create, //indicates the relation will be created
            Update, //indicated the existing relation will be updated
            RelationsOnly //indicated the relation exists and should be only validated for correct entities and fields
        }

        private List<ErrorModel> ValidateRelation(EntityRelation relation, ValidationType validationType)
        {
            List<ErrorModel> errors = new List<ErrorModel>();

            if (validationType == ValidationType.Update)
            {
                //we cannot update relation with missing Id (Guid.Empty means id is missing)
                //of if there is no relation with this id already                
                if (relation.Id == Guid.Empty)
                    errors.Add(new ErrorModel("id", null, "Id is required!"));
                else if (relationRepository.Read(relation.Id) == null)
                    errors.Add(new ErrorModel("id", relation.Id.ToString(), "Entity with such Id does not exist!"));
            }
            else if (validationType == ValidationType.Create)
            {
                //if id is null, them we later will assing one before create process
                //otherwise check if relation with same id already exists
                if (relation.Id != Guid.Empty && (relationRepository.Read(relation.Id) != null))
                    errors.Add(new ErrorModel("id", relation.Id.ToString(), "Entity with such Id already exist!"));

            }
            else if (validationType == ValidationType.RelationsOnly)
            {
                //no need to check anything, we need to check only Entities and Fields relations
                //this case is here only for readability
            }

            IStorageEntityRelation existingRelation = null;
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
                    existingRelation = relationRepository.Read(relation.Name);
                    if (validationType == ValidationType.Create)
                    {
                        //if relation with same name alfready exists
                        if (existingRelation != null)
                            errors.Add(new ErrorModel("name", relation.Name, "Relation with such Name exists already!"));
                    }
                    else if (validationType == ValidationType.Update)
                    {
                        //if relation with same name alfready and different Id already exists
                        if (existingRelation != null && existingRelation.Id != relation.Id)
                            errors.Add(new ErrorModel("name", relation.Name, "Relation with such Name exists already!"));
                    }
                }
            }
            else if (validationType == ValidationType.RelationsOnly)
            {
                //no need to check anything, we need to check only Entities and Fields relations
                //this case is here only for readability
            }

            errors.AddRange(ValidationUtility.ValidateLabel(relation.Label));

            IStorageEntity originEntity = entityRepository.Read(relation.OriginEntityId);
            IStorageEntity targetEntity = entityRepository.Read(relation.OriginEntityId);
            IStorageField originField = null;
            IStorageField targetField = null;

            if (originEntity != null)
                errors.Add(new ErrorModel("originEntity", relation.OriginEntityId.ToString(), "The origin entity do not exist."));
            else
            {
                originField = originEntity.Fields.SingleOrDefault(x => x.Id == relation.OriginFieldId);
                if (originField == null)
                    errors.Add(new ErrorModel("originField", relation.OriginFieldId.ToString(), "The origin field do not exist."));
                if (!(originField is IStorageGuidField))
                    errors.Add(new ErrorModel("originField", relation.OriginFieldId.ToString(), "The origin field should be Unique Identifier (GUID) field."));
            }

            if (targetEntity != null)
                errors.Add(new ErrorModel("targetEntity", relation.TargetEntityId.ToString(), "The target entity do not exist."));
            else
            {
                targetField = targetEntity.Fields.SingleOrDefault(x => x.Id == relation.TargetFieldId);
                if (targetField == null)
                    errors.Add(new ErrorModel("targetField", relation.TargetFieldId.ToString(), "The target field do not exist."));
                if (!(targetField is IStorageGuidField))
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
                //validate there is no other already existing relation with same parameters
                foreach( var rel in relationRepository.Read() )
                {
                    if (rel.OriginEntityId == relation.OriginEntityId && rel.TargetEntityId == relation.TargetEntityId &&
                        rel.OriginFieldId == relation.OriginFieldId && rel.TargetFieldId == relation.TargetFieldId)
                    {
                        errors.Add(new ErrorModel("", "", "There is already existing relation with same parameters."));
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
    }
}

/*
 public enum EntityRelationType
    {
        /// <summary>
        /// 1. Origin field should be an unique, required Guid field
        /// 2. Target field should be an unique, required Guid field
        /// 3. Target field record values should match one origin record field values
        /// </summary>
        OneToOne = 1,

        /// <summary>
        /// 1. Origin field should be an unique,required Guid field
        /// 2. Target field should be a Guid field 
        /// 3. Target field record values should match atleast one origin record field values or null if field value is not required
        /// </summary>
        OneToMany = 2,

        /// <summary>
        /// 1. Origin field should be an unique, required Guid field
        /// 2. Target field should be an unique, required Guid field
        /// </summary>
        ManyToMany = 3
    }

    */
