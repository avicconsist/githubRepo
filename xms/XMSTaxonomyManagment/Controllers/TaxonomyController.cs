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
    public class TaxonomyController : BaseController
    {
        // GET: Taxonomy
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetTaxonomies([DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetTaxonomyViewModels().ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddTaxonomies([DataSourceRequest] DataSourceRequest request, TaxonomyViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                using (var repository = GetRepository())
                {
                    if (!IsTaxonomyUniqueId(model.TaxonomyId))
                    {
                        return new JsonHttpStatusResult(new { Message = UserMessages.TAXSONOMY_IS_NOT_UNIQ_ID }, HttpStatusCode.InternalServerError);
                    }

                    var result = ValidateTaxonomyFields(model);

                    if (result.ValidationMessages.Count > 0)
                    {
                        return new JsonHttpStatusResult(new { Message = result.ValidationMessages }, HttpStatusCode.InternalServerError);
                    }

                    var entity = new TaxonomyModel()
                    {
                        Currency = model.Currency,
                        DecimalDecimals = model.DecimalDecimals,
                        Decimals = model.Decimals,
                        Description = model.Description,
                        EntityIdentifier = model.EntityIdentifier,
                        EntitySchema = model.EntitySchema,
                        IntegerDecimals = model.IntegerDecimals,
                        MonetaryDecimals = model.MonetaryDecimals,
                        PureDecimals = model.PureDecimals,
                        SharesDecimals = model.SharesDecimals,
                        TaxonomyCreationDate = model.TaxonomyCreationDate,
                        TaxonomyDate = model.TaxonomyDate,
                        TaxonomyId = model.TaxonomyId,
                        TnProcessorId = model.TnProcessorId,
                        TnRevisionId = model.TnRevisionId
                    };
                     
                    try
                    {
                        repository.TaxonomyRepository.Add(entity);
                        repository.Commit(); 
                    }
                    catch (Exception e)
                    {
                        GetLogger().LogException(e, string.Format("InsertTaxonomies({0})", model.TaxonomyId));

                        return new JsonHttpStatusResult(new { Message = UserMessages.UNKNOWN_ERROR }, HttpStatusCode.InternalServerError);
                    } 
                }
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));

        }

        [HttpPost]
        public ActionResult UpdateTaxonomies([DataSourceRequest] DataSourceRequest request, UpdateTaxonomyViewModel model)
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

                    if (model.TaxonomyId != model.OldTaxonomyId)
                    {
                        if (!IsTaxonomyUniqueId(model.TaxonomyId))
                        {
                            model.TaxonomyId = model.OldTaxonomyId;
                            return new JsonHttpStatusResult(new { Message = UserMessages.TAXSONOMY_IS_NOT_UNIQ_ID }, HttpStatusCode.InternalServerError);
                        }
                    } 

                    var entity = new TaxonomyModel()
                    {
                        Currency = model.Currency,
                        DecimalDecimals = model.DecimalDecimals,
                        Decimals = model.Decimals,
                        Description = model.Description,
                        EntityIdentifier = model.EntityIdentifier,
                        EntitySchema = model.EntitySchema,
                        IntegerDecimals = model.IntegerDecimals,
                        MonetaryDecimals = model.MonetaryDecimals,
                        PureDecimals = model.PureDecimals,
                        SharesDecimals = model.SharesDecimals,
                        TaxonomyCreationDate = model.TaxonomyCreationDate,
                        TaxonomyDate = model.TaxonomyDate,
                        TaxonomyId = model.TaxonomyId,
                        TnProcessorId = model.TnProcessorId,
                        TnRevisionId = model.TnRevisionId,
                    };

                    try
                    {
                        repository.TaxonomyRepository.Update(model.OldTaxonomyId, entity);
                        repository.Commit();
                    }
                    catch (Exception e)
                    {
                        GetLogger().LogException(e, string.Format("UpdateTaxonomies({0})", model.TaxonomyId));

                        return new JsonHttpStatusResult(new { Message = UserMessages.UNKNOWN_ERROR }, HttpStatusCode.InternalServerError);
                    }

                }
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult DeleteTaxonomies([DataSourceRequest] DataSourceRequest request, TaxonomyViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(model.TaxonomyId))
                {
                    return new JsonHttpStatusResult(new { Message = UserMessages.TAXONOMYID_ERROR }, HttpStatusCode.InternalServerError);
                }

                using (var repository = GetRepository())
                {  

                    var entity = repository.TaxonomyRepository
                                           .GetTaxonomyById(model.TaxonomyId);                                         

                    if (entity == null)
                    {
                        return new JsonHttpStatusResult(new { Message = UserMessages.ROW_NOT_FOUND_ERROR }, HttpStatusCode.InternalServerError);
                    }

                    try
                    {
                        repository.TaxonomyRepository.Remove(entity);
                        repository.Commit();
                    }
                    catch (Exception e)
                    {
                        GetLogger().LogException(e, string.Format("DeleteTaxonomy({0})", model.TaxonomyId));

                        return new JsonHttpStatusResult(new { Message = UserMessages.UNKNOWN_ERROR }, HttpStatusCode.InternalServerError);
                    } 
                }
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public bool IsTaxonomyUniqueId(string taxonomyId)
        {
            bool isUniqId = false;
            using (var repository = GetRepository())
            {
                isUniqId = repository.TaxonomyRepository.IsUniqueId(taxonomyId);
            }

            return isUniqId;
        }




        [NonAction]
        private ColumnValidationResult ValidateTaxonomyFields(TaxonomyViewModel report)
        {
            ColumnValidationResult result = new ColumnValidationResult();

            result.EntityId = report.TaxonomyId;

            result = ValidationField("TaxonomyId", report.TaxonomyId, "שדה קוד טקסונומיה הינו שדה חובה", result);
            
            result = ValidationField("Description", report.Description, "שדה תיאור טקסונומיה הינו שדה חובה", result);

            result = ValidationDateField("TaxonomyDate", report.TaxonomyDate, "שדה תאריך טקסונומיה הינו שדה חובה", result);

            result = ValidationField("EntityIdentifier", report.EntityIdentifier, "שדה מזהה ישות הינו שדה חובה", result);

            result = ValidationField("Currency", report.Currency, "שדה מטבע הינו שדה חובה", result);

            result = ValidationField("Decimals", report.Decimals, "שדה Decimals הינו שדה חובה", result);

            result = ValidationField("EntitySchema", report.EntitySchema, "שדה סכמת ישות הינו שדה חובה", result);

            result = ValidationDateField("TaxonomyCreationDate", report.TaxonomyCreationDate, "שדה תאריך יצירה הינו שדה חובה", result);

            result = ValidationField("TnProcessorId", report.TnProcessorId, "שדה TnProcessorId הינו שדה חובה", result);

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
        private ColumnValidationResult ValidationDateField(string ColumnId, DateTime datevalue, string error, ColumnValidationResult result)
        {
            if (datevalue == DateTime.MinValue)
            {
                result.IsValid = false;
                result.ValidationMessages.Add(error);
                result.ColumnId = ColumnId;
            }
            return result;
        }


        private TaxonomyViewModel CreateTaxonomyViewModel(TaxonomyModel model)
        {
            return new TaxonomyViewModel()
            {
                Description = model.Description,
                TaxonomyId = model.TaxonomyId,
                Currency = model.Currency,
                DecimalDecimals = model.DecimalDecimals,
                Decimals = model.Decimals,
                EntityIdentifier = model.EntityIdentifier,
                EntitySchema = model.EntitySchema,
                IntegerDecimals = model.IntegerDecimals,
                MonetaryDecimals = model.MonetaryDecimals,
                PureDecimals = model.PureDecimals,
                SharesDecimals = model.SharesDecimals,
                TaxonomyCreationDate = model.TaxonomyCreationDate,
                TaxonomyDate = model.TaxonomyDate,
                TnProcessorId = model.TnProcessorId,
                TnRevisionId = model.TnRevisionId

            };
        }
        private TaxonomyViewModel[] GetTaxonomyViewModels()
        {
            TaxonomyModel[] taxonomiesList;
            using (var repository = GetRepository())
            {
                taxonomiesList = repository.TaxonomyRepository.GetAllTaxonomies();
            }
            var taxonomySelectmodel = new List<TaxonomyViewModel>();

            foreach (var taxonomy in taxonomiesList)
            {
                taxonomySelectmodel.Add(CreateTaxonomyViewModel(taxonomy));
            }
            return taxonomySelectmodel.ToArray();

        }

    }
}