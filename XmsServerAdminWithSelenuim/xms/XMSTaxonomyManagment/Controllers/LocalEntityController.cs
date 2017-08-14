
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic; 
using System.Net;
using System.Web.Mvc;
using XMSTaxonomyManagment.Common;
using XMSTaxonomyManagment.Controllers;
using XMSTaxonomyManagment.ViewModels;

namespace Xms_test.Controllers
{

    public class LocalEntityController : BaseController
    { 
        public ActionResult GetLocalEntities([DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetLocalEntitiesViewModels().ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEntityIdentifire()
        {
            return Json(GetLocalEntitiesViewModels(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddLocalEntity([DataSourceRequest] DataSourceRequest request, LocalEntityViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {

                using (var repository = GetRepository())
                {

                    var result = ValidateTaxonomyFields(model);

                    if (result.ValidationMessages.Count > 0)
                    {
                        return new JsonHttpStatusResult(new { Message = result.ValidationMessages }, HttpStatusCode.InternalServerError);
                    }
                    if (!IsLocalEntityUniqueId(model.Id))
                    {
                        return new JsonHttpStatusResult(new { Message = UserMessages.TAXSONOMY_IS_NOT_UNIQ_ID }, HttpStatusCode.InternalServerError);
                    }
                     
                    var entity = new LocalEntityModel()
                    {
                        Description = model.Description,
                        Id = model.Id
                    };

                    try
                    {
                        repository.LocalEntityRepository.Add(entity);
                    }
                    catch (Exception e)
                    {
                        GetLogger().LogException(e, string.Format("InsertLocalEntities({0})", model.Id));

                        return new JsonHttpStatusResult(new { Message = UserMessages.UNKNOWN_ERROR }, HttpStatusCode.InternalServerError);
                    }

                    repository.Commit();
                }

            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));

        }

        [HttpPost]
        public JsonResult UpdateLocalEntities([DataSourceRequest] DataSourceRequest request, UpdateLocalEntityViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                using (var repository = GetRepository())
                {
                    var result = ValidateTaxonomyFields(model);

                    if (result.ValidationMessages.Count > 0)
                    {
                        return new JsonHttpStatusResult(new { Message = result.ValidationMessages }, HttpStatusCode.InternalServerError);
                    }
                    if (model.Id != model.OldId)
                    {
                        if (!IsLocalEntityUniqueId(model.Id))
                        {
                            model.Id = model.OldId;
                            return new JsonHttpStatusResult(new { Message = UserMessages.TAXSONOMY_IS_NOT_UNIQ_ID }, HttpStatusCode.InternalServerError);
                        }
                    }
                    

                    var entity = new LocalEntityModel()
                    {
                        Description = model.Description,
                        Id = model.Id
                    };

                    try
                    {
                        repository.LocalEntityRepository.Update(model.OldId, entity);
                    }
                    catch (Exception e)
                    {
                        GetLogger().LogException(e, string.Format("UpdateTaxonomies({0})", model.Id));

                        return new JsonHttpStatusResult(new { Message = UserMessages.UNKNOWN_ERROR }, HttpStatusCode.InternalServerError);
                    }

                    repository.Commit();
                }
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));

        }
        [HttpPost]
        public JsonResult DeleteLocalEntity([DataSourceRequest] DataSourceRequest request, LocalEntityViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(model.Id))
                {
                    return new JsonHttpStatusResult(new { Message = UserMessages.TAXONOMYID_ERROR }, HttpStatusCode.InternalServerError);
                }

                using (var repository = GetRepository())
                { 
                    var entity = repository.LocalEntityRepository.GetLocalEntityById(model.Id);
                          
                    if (entity == null)
                    {
                        return new JsonHttpStatusResult(new { Message = UserMessages.ROW_NOT_FOUND_ERROR }, HttpStatusCode.InternalServerError);
                    }

                    try
                    {
                        repository.LocalEntityRepository.Remove(entity);
                        repository.Commit();
                    }
                    catch (Exception e)
                    {
                        GetLogger().LogException(e, string.Format("DeleteLocalEntity({0})", model.Id));

                        return new JsonHttpStatusResult(new { Message = UserMessages.UNKNOWN_ERROR }, HttpStatusCode.InternalServerError);
                    } 
                }
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));

        }

        [NonAction]
        private ColumnValidationResult ValidateTaxonomyFields(LocalEntityViewModel localEntity)
        {
            ColumnValidationResult result = new ColumnValidationResult();

            result.EntityId = localEntity.Id;

            result = ValidationField("Id", localEntity.Id, "שדה קוד ישות הינו שדה חובה", result);

            return result;
        }
        [NonAction]
        private ColumnValidationResult ValidationField(string ColumnId, string fieldValue, string error, ColumnValidationResult result)
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
            {
                result.IsValid = false;
                result.ValidationMessages.Add(error);
                result.ColumnId = ColumnId;
            } 

            return result;
        }
        private LocalEntityViewModel[] GetLocalEntitiesViewModels()
        {
            LocalEntityModel[] localEntities;
            using (var repository = GetRepository())
            {
                localEntities = repository.LocalEntityRepository.GetLocalEntities();
            }
            var localEntitiesModel = new List<LocalEntityViewModel>();

            foreach (var entity in localEntities)
            {
                localEntitiesModel.Add(new LocalEntityViewModel()
                {
                    Description = entity.Description,
                    Id = entity.Id
                });
            }
            return localEntitiesModel.ToArray();
        } 
        private bool IsLocalEntityUniqueId(string id)
        {
            bool isUniqId = false;
            using (var repository = GetRepository())
            {
                isUniqId = repository.LocalEntityRepository.IsLocalEntityUniqueId(id);
            }

            return isUniqId;
        }

    }
}

