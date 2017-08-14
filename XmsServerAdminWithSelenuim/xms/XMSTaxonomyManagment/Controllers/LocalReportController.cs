using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using XMSTaxonomyManagment.Common;
using XMSTaxonomyManagment.ViewModels;

namespace XMSTaxonomyManagment.Controllers
{
    public class LocalReportController : BaseController
    { 
        public JsonResult GetLocalReportsByTaxonomyId(string taxonomyId, [DataSourceRequest] DataSourceRequest request)
        {
            if (string.IsNullOrWhiteSpace(taxonomyId))
            {
                GetLogger().LogError(string.Format("GetLocalReportsByTaxonomyId fail : taxonomyId Is Null Or WhiteSpace"));

                return new JsonHttpStatusResult(new { Message = UserMessages.TAXONOMYID_ERROR }, HttpStatusCode.InternalServerError);
            }

            LocalReportModel[] LocalReports;
            PeriodTypeModel[] periodTypes;
            LocalEntityModel[] entitiesIdentifier;
            TaxonomyReportIdAndDescription[] sourceIds;
            var models = new List<LocalReportViewModel>();

            try
            {
                using (var repository = GetRepository())
                {
                    LocalReports = repository.LocalReportRepository.GetLocalReportsByTaxonomyId(taxonomyId);
                    periodTypes = repository.PeriodTypeRepository.GetPeriodTypes();
                    entitiesIdentifier = repository.LocalEntityRepository.GetLocalEntities();
                    sourceIds = repository.TaxonomyReportRepository.GetShortTaxonomyReportsByTaxonomyId(taxonomyId);
                }

                foreach (var entity in LocalReports)
                {

                    var periodTypeModel = PeriodTypeViewModelHelper.PeriodTypeToViewModel(entity.PeriodType, periodTypes);

                    if (periodTypeModel != null)
                    {
                        periodTypeModel.Description = periodTypeModel.Description != null ? periodTypeModel.Description : "";
                        periodTypeModel.PeriodType = periodTypeModel.PeriodType != null ? periodTypeModel.PeriodType : null;
                    }
                    else
                    {
                        periodTypeModel.PeriodType = null;
                        periodTypeModel.Description = "";
                    }

                    var entitiesIdentifierModel = new LocalEntityViewModel();

                    var entityIden = entitiesIdentifier.Where(x => x.Id == entity.EntityIdentifier).FirstOrDefault();

                    if (entityIden != null)
                    {
                        entitiesIdentifierModel.Id = entityIden.Id;
                        entitiesIdentifierModel.Description = entityIden.Description;
                    }
                    else
                    {
                        entitiesIdentifierModel.Id = "";
                        entitiesIdentifierModel.Description = "";
                    }

                    var sourceId = sourceIds.Where(x => x.Id == entity.SourceId).FirstOrDefault();

                    if (sourceId == null)
                    {
                        sourceId.Description = "";
                        sourceId.Id = "";
                    }

                    var sourceIdModel = new SourceIdViewModel()
                    {
                        Description = sourceId.Id + " - "+sourceId.Description  ,
                        Id = sourceId.Id
                    };

                     models.Add(new LocalReportViewModel()
                    {
                        Currency = entity.Currency,
                        Decimals = entity.Decimals,
                        DecimalDecimals = entity.DecimalDecimals,
                        Description = entity.Description,
                        EntityIdentifire = entitiesIdentifierModel,
                        EntitySchema = entity.EntitySchema,
                        EntryUri = entity.EntryUri,
                        FileName = entity.FileName,
                        Id = entity.Id,
                        IntegerDecimals = entity.IntegerDecimals,
                        MonetaryDecimals = entity.MonetaryDecimals,
                        PeriodType = periodTypeModel,
                        PureDecimals = entity.PureDecimals,
                        SharesDecimals = entity.SharesDecimals,
                        TaxonomyId = entity.TaxonomyId,
                        TnProcessorId = entity.TnProcessorId,
                        TnRevisionId = entity.TnRevisionId,
                        SourceId = sourceIdModel
                    });
                }
            }
            catch (Exception e)
            {
                GetLogger().LogException(e, string.Format("GetLocalReportsByTaxonomyId({0})", taxonomyId));

                return new JsonHttpStatusResult(new { Message = UserMessages.UNKNOWN_ERROR }, HttpStatusCode.InternalServerError);
            }

            return Json(models.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult AddLocalReport([DataSourceRequest] DataSourceRequest request, LocalReportViewModel model)
        {
            using (var repository = GetRepository())
            {
                if (model != null && ModelState.IsValid)
                {
                    if (!IsLocalReportUniqueId(model.TaxonomyId, model.Id))
                    {
                        return new JsonHttpStatusResult(new { Message = UserMessages.TAXSONOMY_IS_NOT_UNIQ_ID }, HttpStatusCode.InternalServerError);
                    }

                    var result = ValidateLocalTaxonomyReportFields(model);

                    if (result.ValidationMessages.Count > 0)
                    {
                        return new JsonHttpStatusResult(new { Message = result.ValidationMessages }, HttpStatusCode.InternalServerError);
                    }

                    var entity = new LocalReportModel()
                    {
                        Currency = model.Currency,
                        Decimals = model.Decimals,
                        DecimalDecimals = model.DecimalDecimals,
                        Description = model.Description,
                        EntityIdentifier = model.EntityIdentifire.Id,
                        EntitySchema = model.EntitySchema,
                        EntryUri = model.EntryUri,
                        FileName = model.FileName,
                        Id = model.Id,
                        IntegerDecimals = model.IntegerDecimals,
                        MonetaryDecimals = model.MonetaryDecimals,
                        PeriodType = model.PeriodType.PeriodType,
                        PureDecimals = model.PureDecimals,
                        SharesDecimals = model.SharesDecimals,
                        TaxonomyId = model.TaxonomyId,
                        TnProcessorId = model.TnProcessorId,
                        TnRevisionId = model.TnRevisionId,
                        SourceId = model.SourceId.Id,
                    };

                    try
                    {
                        repository.LocalReportRepository.Add(entity);
                        repository.Commit();

                    }
                    catch (Exception e)
                    {
                        GetLogger().LogException(e, string.Format("AddLocalReports({0})", entity.TaxonomyId));

                        return new JsonHttpStatusResult(new { Message = UserMessages.UNKNOWN_ERROR }, HttpStatusCode.InternalServerError);
                    }
                }
            }
            if (model.PeriodType.PeriodType == null)
            {
                model.PeriodType.Description = "";
            }
            if (model.EntityIdentifire.Id == null)
            {
                model.EntityIdentifire.Description = "";
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult UpdateLocalReport([DataSourceRequest] DataSourceRequest request, UpdateLocalReportViewModel model)
        {
            using (var repository = GetRepository())
            {
                 
                var result = ValidateLocalTaxonomyReportFields(model);

                if (result.ValidationMessages.Count > 0)
                {
                    return new JsonHttpStatusResult(new { Message = result.ValidationMessages }, HttpStatusCode.InternalServerError);
                }
                if (model.Id != model.OldId)
                {
                    if (!IsLocalReportUniqueId(model.TaxonomyId, model.Id))
                    {
                        model.Id = model.OldId;
                        return new JsonHttpStatusResult(new { Message = UserMessages.TAXSONOMY_IS_NOT_UNIQ_ID }, HttpStatusCode.InternalServerError);
                    }
                }
               
                var entity = new LocalReportModel()
                {
                    Currency = model.Currency,
                    Decimals = model.Decimals,
                    DecimalDecimals = model.DecimalDecimals,
                    Description = model.Description,
                    EntityIdentifier = model.EntityIdentifire==null?"":model.EntityIdentifire.Id,
                    EntitySchema = model.EntitySchema,
                    EntryUri = model.EntryUri,
                    FileName = model.FileName,
                    Id = model.Id,
                    IntegerDecimals = model.IntegerDecimals,
                    MonetaryDecimals = model.MonetaryDecimals,
                    PeriodType = model.PeriodType==null?null: model.PeriodType.PeriodType,
                    PureDecimals = model.PureDecimals,
                    SharesDecimals = model.SharesDecimals,
                    TaxonomyId = model.TaxonomyId,
                    TnProcessorId = model.TnProcessorId,
                    TnRevisionId = model.TnRevisionId,
                    SourceId = model.SourceId == null ? null : model.SourceId.Id,
                    UpdateDate = DateTime.Now
                };

                try
                {
                    repository.LocalReportRepository.Update(model.OldSourceId, model.OldId, entity);
                    repository.Commit();

                }
                catch (Exception e)
                {
                    GetLogger().LogException(e, string.Format("UpdateLocalReports({0})", entity.TaxonomyId));

                    return new JsonHttpStatusResult(new { Message = UserMessages.UNKNOWN_ERROR }, HttpStatusCode.InternalServerError);
                } 
            } 

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult DeleteLocalReport([DataSourceRequest] DataSourceRequest request, LocalReportViewModel model)
        {
            using (var repository = GetRepository())
            {
                if (model != null && ModelState.IsValid)
                {
                    if (string.IsNullOrWhiteSpace(model.Id))
                    {
                        return new JsonHttpStatusResult(new { Message = UserMessages.ID_ERROR }, HttpStatusCode.InternalServerError);
                    }

                    if (string.IsNullOrWhiteSpace(model.TaxonomyId))
                    {
                        return new JsonHttpStatusResult(new { Message = UserMessages.TAXONOMYID_ERROR }, HttpStatusCode.InternalServerError);
                    }

                    if (string.IsNullOrWhiteSpace(model.SourceId.Id))
                    {
                        return new JsonHttpStatusResult(new { Message = UserMessages.SOURCEID_ERROR }, HttpStatusCode.InternalServerError);
                    }

                    var entity = repository.LocalReportRepository.GetLocalReportsByTaxonomyId(model.TaxonomyId)
                        .FirstOrDefault(x => x.Id == model.Id);

                    if (entity == null)
                    {
                        return new JsonHttpStatusResult(new { Message = UserMessages.ROW_NOT_FOUND_ERROR }, HttpStatusCode.InternalServerError);
                    }

                    try
                    {
                        repository.LocalReportRepository.Remove(entity);
                        repository.Commit();

                    }
                    catch (Exception e)
                    {
                        GetLogger().LogException(e, string.Format("DeleteLocalReport({0})", model.TaxonomyId));

                        return new JsonHttpStatusResult(new { Message = UserMessages.UNKNOWN_ERROR }, HttpStatusCode.InternalServerError);
                    }
                } 
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [NonAction]
        private ColumnValidationResult ValidateLocalTaxonomyReportFields(LocalReportViewModel report)
        {
            ColumnValidationResult result = new ColumnValidationResult();

            result.EntityId = report.Id;

            result = ValidationField("Id", report.Id, "שדה קוד דיווח הינו שדה חובה", result);

            if (report.SourceId != null)
            {
                result = ValidationField("SourceId ", report.SourceId.Id, "שדה Source Id  הינו שדה חובה", result);

            }
            else
            {
                result.ValidationMessages.Add("שדה Source Id  הינו שדה חובה");
            }

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

        [NonAction]
        private bool IsLocalReportUniqueId(string taxonomyId, string id)
        {
            bool isUniqId = false;
            using (var repository = GetRepository())
            {
                isUniqId = repository.LocalReportRepository.IsUniqueLocalReportId(taxonomyId, id);
            }

            return isUniqId;
        }


    }
}