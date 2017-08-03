using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TempletProject.Common;
using TempletProject.ViewModels;

namespace TempletProject.Controllers
{
    public class TestController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetTests([DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetTestViewModels().ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddTest([DataSourceRequest] DataSourceRequest request, TestViewModel model)
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

                    var entity = new TestModel()
                    {
                        CreatedDate = DateTime.Now,
                        Description = model.Description, 
                        Id= model.Id
                    };

                    try
                    {
                        repository.TestRepository.Add(entity);
                        repository.Commit();
                    }
                    catch (Exception e)
                    {
                        GetLogger().LogException(e, string.Format("AddTest({0})", model.Id));

                        return new JsonHttpStatusResult(new { Message = UserMessages.UNKNOWN_ERROR }, HttpStatusCode.InternalServerError);
                    }
                }
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));

        }

        [HttpPost]
        public ActionResult UpdateTest([DataSourceRequest] DataSourceRequest request, UpdateTestViewModel model)
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
                        if (!IsUniqueId(model.Id))
                        {
                            model.Id = model.OldId;
                            return new JsonHttpStatusResult(new { Message = UserMessages.FIELD_ID }, HttpStatusCode.InternalServerError);
                        }
                    }


                    var entity = new TestModel()
                    {
                        Description = model.Description,
                        Id = model.Id
                    };

                    try
                    {
                        repository.TestRepository.Update(model.OldId, entity);
                    }
                    catch (Exception e)
                    {
                        GetLogger().LogException(e, string.Format("UpdateTest({0})", model.Id));

                        return new JsonHttpStatusResult(new { Message = UserMessages.UNKNOWN_ERROR }, HttpStatusCode.InternalServerError);
                    }

                    repository.Commit();
               }
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));

        }

        private bool IsUniqueId(string id)
        {
            bool isUniqId = false;
            using (var repository = GetRepository())
            {
                isUniqId = repository.TestRepository.IsUniqueId(id);
            }

            return isUniqId;
        }

        [HttpPost]
        public ActionResult DeleteTest([DataSourceRequest] DataSourceRequest request, TestViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(model.Id))
                {
                    return new JsonHttpStatusResult(new { Message = UserMessages.FIELD_ID }, HttpStatusCode.InternalServerError);
                }

                using (var repository = GetRepository())
                {

                    var entity = repository.TestRepository
                                           .GetTestById(model.Id);

                    if (entity == null)
                    {
                        return new JsonHttpStatusResult(new { Message = UserMessages.ROW_NOT_FOUND_ERROR }, HttpStatusCode.InternalServerError);
                    }

                    try
                    {
                        repository.TestRepository.Remove(entity);
                        repository.Commit();
                    }
                    catch (Exception e)
                    {
                        GetLogger().LogException(e, string.Format("DeleteTest({0})", model.Id));

                        return new JsonHttpStatusResult(new { Message = UserMessages.UNKNOWN_ERROR }, HttpStatusCode.InternalServerError);
                    }
                }
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
         
        [NonAction]
        private ColumnValidationResult ValidateTaxonomyFields(TestViewModel report)
        {
            ColumnValidationResult result = new ColumnValidationResult();

            result.EntityId = report.Id;

            result = ValidationField("Description", report.Description, "שדה תיאור הינו שדה חובה", result);

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
         
        private TestViewModel CreateTestViewModel(TestModel model)
        {
            return new TestViewModel()
            {
                Description = model.Description,
                Id = model.Id,

            };
        }

        private TestViewModel[] GetTestViewModels()
        {
            TestModel[] testList;

            using (var repository = GetRepository())
            {
                testList = repository.TestRepository.GetTestEntities();
            }
            var testSelectmodel = new List<TestViewModel>();

            foreach (var test in testList)
            {
                testSelectmodel.Add(CreateTestViewModel(test));
            }
            return testSelectmodel.ToArray();

        }

    }
}