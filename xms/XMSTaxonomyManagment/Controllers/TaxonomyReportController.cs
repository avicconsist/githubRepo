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
    public class TaxonomyReportController : BaseController
    {

        public ActionResult Index()
        {
            TaxonomyModel[] taxonomies;
            PeriodTypeModel[] periodTypes;
            using (var repository = GetRepository())
            {
                taxonomies = repository.TaxonomyRepository.GetAllTaxonomies();
                periodTypes = repository.PeriodTypeRepository.GetPeriodTypes();
            }

            var taxonomyViewModels = new List<TaxonomyDescriptionViewModel>();


            var defaultPeriodType = periodTypes.FirstOrDefault();

            var defaultPeriodTypeViewModel = new PeriodTypeViewModel()
            {
                Description = defaultPeriodType.Description,
                PeriodType = defaultPeriodType.PeriodType
            };

            foreach (var TaxonomyReportModel in taxonomies)
            {
                taxonomyViewModels.Add(new TaxonomyDescriptionViewModel()
                {
                    Description = TaxonomyReportModel.Description,
                    TaxonomyId = TaxonomyReportModel.TaxonomyId
                });
            }

            var ViewModel = new TaxonomyReportMainViewModel()
            {
                Taxonomies = taxonomyViewModels.ToArray(),
                SelectedTaxonomy = taxonomyViewModels.FirstOrDefault(),
                PeriodTypeDefaultValue = defaultPeriodTypeViewModel
            };

            return View(ViewModel);
        }

        public JsonResult GetTaxonomyReportsByTaxonomyId([DataSourceRequest] DataSourceRequest request, string taxonomyId  )
        {
            if (string.IsNullOrWhiteSpace(taxonomyId))
            {
                GetLogger().LogError(string.Format("GetTaxonomyReportsByTaxonomyId fail : taxonomyId Is Null Or WhiteSpace"));

                return new JsonHttpStatusResult(new { Message = UserMessages.TAXONOMYID_ERROR },
                    HttpStatusCode.InternalServerError);
            }

            TaxonomyReportModel[] taxonomyReports;
            PeriodTypeModel[] periodTypes;
            var models = new List<TaxonomyReportViewModel>();

            try
            {
                using (var repository = GetRepository())
                {
                    taxonomyReports = repository.TaxonomyReportRepository.GetTaxonomyReportsByTaxonomyId(taxonomyId);

                    periodTypes = repository.PeriodTypeRepository.GetPeriodTypes();
                }

                foreach (var entity in taxonomyReports)
                {
                    var periodTypeModel = PeriodTypeViewModelHelper.PeriodTypeToViewModel(entity.PeriodType, periodTypes);

                    models.Add(new TaxonomyReportViewModel()
                    {
                        Currency = entity.Currency,
                        Decimals = entity.Decimals,
                        DecimalDecimals = entity.DecimalDecimals,
                        Description = entity.Description,
                        EntityIdentifire = entity.EntityIdentifier,
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
                    });
                }
            }
            catch (Exception e)
            {
                GetLogger().LogException(e, string.Format("GetTaxonomyReportsByTaxonomyId({0})", taxonomyId));

                return new JsonHttpStatusResult(new { Message = UserMessages.UNKNOWN_ERROR }, HttpStatusCode.InternalServerError);
            }

            return Json( models.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
           
        }

        public JsonResult GetTaxonomyReportsIdAndDescriptionByTaxonomyId([DataSourceRequest] DataSourceRequest request, TaxonomyReportViewModel model)
        {
            TaxonomyReportIdAndDescription[] sourceIds;

            using (var repository = GetRepository())
            {
                sourceIds = repository.TaxonomyReportRepository.GetShortTaxonomyReportsByTaxonomyId(model.TaxonomyId);
            }

            var models = new List<SourceIdViewModel>();

            models = sourceIds
                .Select(sourceId =>
                new SourceIdViewModel()
                {
                    Id = sourceId.Id,
                    Description = sourceId.Description
                }).ToList();

            return Json(models, JsonRequestBehavior.AllowGet);
        }
         

        [HttpPost]
        public ActionResult AddTaxonomyReports([DataSourceRequest] DataSourceRequest request, TaxonomyReportViewModel model)
        {
            using (var repository = GetRepository())
            {
                if (model != null && ModelState.IsValid)
                {
                    if (!IsTaxonomyReportUniqueId(model.TaxonomyId, model.Id))
                    {
                        return new JsonHttpStatusResult(new { Message = UserMessages.TAXSONOMY_IS_NOT_UNIQ_ID }, HttpStatusCode.InternalServerError);
                    }

                    var result = ValidateTaxonomyReportFields(model);

                    if (result.ValidationMessages.Count > 0)
                    {
                        return new JsonHttpStatusResult(new { Message = result.ValidationMessages }, HttpStatusCode.InternalServerError);
                    }

                    var entity = new TaxonomyReportModel()
                    {
                        Currency = model.Currency,
                        Decimals = model.Decimals,
                        DecimalDecimals = model.DecimalDecimals,
                        Description = model.Description,
                        EntityIdentifier = model.EntityIdentifire,
                        EntitySchema = model.EntitySchema,
                        EntryUri = model.EntryUri,
                        FileName = model.FileName,
                        Id = model.Id,
                        IntegerDecimals = model.IntegerDecimals,
                        MonetaryDecimals = model.MonetaryDecimals,
                        PeriodType = model.PeriodType.PeriodType.Value,
                        PureDecimals = model.PureDecimals,
                        SharesDecimals = model.SharesDecimals,
                        TaxonomyId = model.TaxonomyId,
                        TnProcessorId = model.TnProcessorId,
                        TnRevisionId = model.TnRevisionId,
                    };

                    try
                    {
                        repository.TaxonomyReportRepository.Add(entity);
                        repository.Commit();

                    }
                    catch (Exception e)
                    {
                        GetLogger().LogException(e, string.Format("AddTaxonomyReports({0})", model.TaxonomyId));
                        return new JsonHttpStatusResult(new { Message = UserMessages.UNKNOWN_ERROR }, HttpStatusCode.InternalServerError);
                    }
                }
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));

        }

        [HttpPost]
        public ActionResult UpdateTaxonomyReports([DataSourceRequest] DataSourceRequest request, TaxonomyReportViewModel model)
        {
            using (var repository = GetRepository())
            {
                if (model != null && ModelState.IsValid)
                {
                    var result = ValidateTaxonomyReportFields(model);

                    if (result.ValidationMessages.Count > 0)
                    {
                        return new JsonHttpStatusResult(new { Message = result.ValidationMessages }, HttpStatusCode.InternalServerError);
                    }

                    var entity = new TaxonomyReportModel()
                    {
                        Currency = model.Currency,
                        Decimals = model.Decimals,
                        DecimalDecimals = model.DecimalDecimals,
                        Description = model.Description,
                        EntityIdentifier = model.EntityIdentifire,
                        EntitySchema = model.EntitySchema,
                        EntryUri = model.EntryUri,
                        FileName = model.FileName,
                        Id = model.Id,
                        IntegerDecimals = model.IntegerDecimals,
                        MonetaryDecimals = model.MonetaryDecimals,
                        PeriodType = model.PeriodType.PeriodType.Value,
                        PureDecimals = model.PureDecimals,
                        SharesDecimals = model.SharesDecimals,
                        TaxonomyId = model.TaxonomyId,
                        TnProcessorId = model.TnProcessorId,
                        TnRevisionId = model.TnRevisionId,
                    };
                    try
                    {
                        repository.TaxonomyReportRepository.Update(entity);
                        repository.Commit();

                    }
                    catch (Exception e)
                    {
                        GetLogger().LogException(e, string.Format("UpdateTaxonomyReports({0})", model.TaxonomyId));

                        return new JsonHttpStatusResult(new { Message = UserMessages.UNKNOWN_ERROR }, HttpStatusCode.InternalServerError);
                    }
                } 
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult DeleteTaxonomyReport([DataSourceRequest] DataSourceRequest request, TaxonomyReportViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(model.TaxonomyId))
                {
                    return new JsonHttpStatusResult(new { Message = UserMessages.TAXONOMYID_ERROR }, HttpStatusCode.InternalServerError);
                }

                using (var repository = GetRepository())
                {

                    var entity = repository.TaxonomyReportRepository
                                           .GetTaxonomyReportsByTaxonomyId(model.TaxonomyId).FirstOrDefault(x => x.Id == model.Id);

                    if (entity == null)
                    {
                        return new JsonHttpStatusResult(new { Message = UserMessages.ROW_NOT_FOUND_ERROR }, HttpStatusCode.InternalServerError);
                    }

                    try
                    {
                        repository.TaxonomyReportRepository.Remove(entity);
                        repository.Commit();
                    }
                    catch (Exception e)
                    {
                        GetLogger().LogException(e, string.Format("DeleteTaxonomyReport({0})", model.TaxonomyId));

                        return new JsonHttpStatusResult(new { Message = UserMessages.UNKNOWN_ERROR }, HttpStatusCode.InternalServerError);
                    }
                }
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }



        [NonAction]
        private ColumnValidationResult ValidateTaxonomyReportFields(TaxonomyReportViewModel reportViewModel)
        {
            ColumnValidationResult result = new ColumnValidationResult();

            result.EntityId = reportViewModel.Id;
            result.TaxonomyId = reportViewModel.TaxonomyId;

            result = ValidationField("Id", reportViewModel.Id, "שדה קוד דיווח הינו שדה חובה", result);


            result = ValidationField("EntryUri", reportViewModel.EntryUri, "שדה Entry Uri הינו שדה חובה", result);


            result = ValidationField("FileName", reportViewModel.FileName, "שדה שם קובץ הינו שדה חובה", result);


            result = ValidationField("Description", reportViewModel.Description, "שדה תיאור דוח הינו שדה חובה", result);
 
            result = ValidationField("PeriodType", reportViewModel.PeriodType.Description, "שדה סוג תקופה הינו שדה חובה", result);
 
            result = ValidationField("EntitySchema", reportViewModel.EntitySchema, "שדה Entity Schema הינו שדה חובה", result);

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
        private bool IsTaxonomyReportUniqueId(string taxonomyId, string id)
        {
            bool isUniqId = false;
            using (var repository = GetRepository())
            {
                isUniqId = repository.TaxonomyReportRepository.IsUniqueTaxonomyReportId(taxonomyId, id);
            }

            return isUniqId;
        }

    }
}