using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TempletProject.Common;
using TempletProject.Models;
using TempletProject.ViewModels;

namespace TempletProject.Controllers
{
    public class ImgController : BaseController
    {



        public static List<ImgGridModel> items { get; set; }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetImgGridEntities([DataSourceRequest] DataSourceRequest request)
        {
            var items = new List<ImgGridModel>()
            {
                new ImgGridModel()
            {
                Id = "1",
                Subject1 = "30-",
                Image1 = "bubble4",
                Subject2 = "8-",
                Image2 = "bubble4",
                Subject3 = "27-",
                Image3 = "bubble4",
                Subject4 = "14-",
                Image4 = "bubble4",
                Subject5 = "5",
                Image5 = "bubble5",
                Subject6 = "11-",
                Image6 = "bubble4",
                Subject7 = "26-",
                Image7 = "bubble4",
                Subject8 = "20-",
                Image8 = "bubble4",
                Subject9 = "207",
                Image9 = "star",
                Subject10 = "10",
                Image10 = "bubble4",
                Subject11 = "6-",
                Image11 = "bubble4",
                Branch = "סניף 1"
            },
                     new ImgGridModel()
            {
                Id = "2",
                Subject1 = "30-",
                Image1 = "bubble4",
                Subject2 = "21",
                Image2 = "bubble2",
                Subject3 = "15-",
                Image3 = "bubble4",
                Subject4 = "26",
                Image4 = "bubble2",
                Subject5 = "3",
                Image5 = "bubble5",
                Subject6 = "933-",
                Image6 = "bubble1",
                Subject7 = "215",
                Image7 = "star",
                Subject8 = "23-",
                Image8 = "bubble4",
                Subject9 = "94-",
                Image9 = "bubble3",
                Subject10 = "20-",
                Image10 = "bubble4",
                Subject11 = "16-",
                Image11 = "bubble4",
                Branch = "סניף 2"
            },
                new ImgGridModel()
            {
                Id = "3",
                Subject1 = "78-",
                Image1 = "bubble3",
                Subject2 = "852-",
                Image2 = "bubble1",
                Subject3 = "13-",
                Image3 = "bubble4",
                Subject4 = "150-",
                Image4 = "bubble3",
                Subject5 = "0",
                Image5 = "bubble5",
                Subject6 = "95-",
                Image6 = "bubble3",
                Subject7 = "138-",
                Image7 = "bubble3",
                Subject8 = "51-",
                Image8 = "bubble3",
                Subject9 = "111-",
                Image9 = "bubble3",
                Subject10 = "24",
                Image10 = "bubble2",
                Subject11 = "158",
                Image11 = "bubble4",
                Branch = "סניף 3"
            },
            new ImgGridModel()
             {
                Id ="4",
                 Subject1 = "25-",
                 Image1 = "bubble3",
                 Subject2 = "23",
                 Image2 = "bubble2",
                 Subject3 = "1",
                 Image3 = "bubble5",
                 Subject4 = "185",
                 Image4 = "star",
                 Subject5 = "13",
                 Image5 = "bubble2",
                 Subject6 = "5",
                 Image6 = "bubble2",
                 Subject7 = "10",
                 Image7 = "bubble2",
                 Subject8 = "4",
                 Image8 = "bubble5",
                 Subject9 = "28",
                 Image9 = "bubble3",
                 Subject10 = "459-",
                 Image10 = "bubble1",
                 Subject11 = "117-",
                 Image11 = "bubble3",
                 Branch = "סניף 4"
             },
              new ImgGridModel()
             {
                Id ="5",
                 Subject1 = "22-",
                 Image1 = "bubble4",
                 Subject2 = "24",
                 Image2 = "bubble2",
                 Subject3 = "14",
                 Image3 = "bubble2",
                 Subject4 = "5",
                 Image4 = "bubble2",
                 Subject5 = "5",
                 Image5 = "bubble5",
                 Subject6 = "22-",
                 Image6 = "bubble4",
                 Subject7 = "384-",
                 Image7 = "bubble1",
                 Subject8 = "21-",
                 Image8 = "bubble4",
                 Subject9 = "137-",
                 Image9 = "bubble3",
                 Subject10 = "17",
                 Image10 = "bubble2",
                 Subject11 = "25",
                 Image11 = "bubble2",
                 Branch = "סניף 5"
             },
             new ImgGridModel()
             {
                Id ="6",
                 Subject1 = "6-",
                 Image1 = "bubble4",
                 Subject2 = "7-",
                 Image2 = "bubble4",
                 Subject3 = "24-",
                 Image3 = "bubble4",
                 Subject4 = "238",
                 Image4 = "star",
                 Subject5 = "5",
                 Image5 = "bubble5",
                 Subject6 = "87",
                 Image6 = "star",
                 Subject7 = "21-",
                 Image7 = "bubble4",
                 Subject8 = "1",
                 Image8 = "bubble5",
                 Subject9 = "77-",
                 Image9 = "bubble3",
                 Subject10 = "9",
                 Image10 = "bubble2",
                 Subject11 = "186-",
                 Image11 = "bubble3",
                 Branch = "סניף 6"
             }
            };
            return Json(items.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            //return Json(GetTestViewModels().ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddImgGridEntity([DataSourceRequest] DataSourceRequest request, ImgGridModelViewModel model)
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

                    var entity = new ImgGridModel()
                    {
                        Id = model.Id,
                        Branch = model.Branch,
                        Image1 = model.Image1,
                        Image10 = model.Image10,
                        Image11 = model.Image11,
                        Image2 = model.Image2,
                        Image3 = model.Image3,
                        Image4 = model.Image4,
                        Image5 = model.Image5,
                        Image6 = model.Image6,
                        Image7 = model.Image7,
                        Image8 = model.Image8,
                        Image9 = model.Image9,
                        Subject1 = model.Subject1,
                        Subject10 = model.Subject10,
                        Subject11 = model.Subject11,
                        Subject2 = model.Subject2,
                        Subject3 = model.Subject3,
                        Subject4 = model.Subject4,
                        Subject5 = model.Subject5,
                        Subject6 = model.Subject6,
                        Subject7 = model.Subject7,
                        Subject8 = model.Subject8,
                        Subject9 = model.Subject9
                    };

                    try
                    {
                        //repository.TestRepository.Add(entity);
                        //repository.Commit();
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
        public ActionResult UpdateImgGridEntity([DataSourceRequest] DataSourceRequest request, ImgGridModelViewModel model)
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
                     


                    var entity = new ImgGridModel()
                    {
                        Id = model.Id,
                        Branch = model.Branch,
                        Image1 = model.Image1,
                        Image10 = model.Image10,
                        Image11 = model.Image11,
                        Image2 = model.Image2,
                        Image3 = model.Image3,
                        Image4 = model.Image4,
                        Image5 = model.Image5,
                        Image6 = model.Image6,
                        Image7 = model.Image7,
                        Image8 = model.Image8,
                        Image9 = model.Image9,
                        Subject1 = model.Subject1,
                        Subject10 = model.Subject10,
                        Subject11 = model.Subject11,
                        Subject2 = model.Subject2,
                        Subject3 = model.Subject3,
                        Subject4 = model.Subject4,
                        Subject5 = model.Subject5,
                        Subject6 = model.Subject6,
                        Subject7 = model.Subject7,
                        Subject8 = model.Subject8,
                        Subject9 = model.Subject9
                    };

                    try
                    {
                        // repository.TestRepository.Update(model.OldId, entity);
                       // repository.Commit();
                    }
                    catch (Exception e)
                    {
                        GetLogger().LogException(e, string.Format("UpdateTest({0})", model.Id));

                        return new JsonHttpStatusResult(new { Message = UserMessages.UNKNOWN_ERROR }, HttpStatusCode.InternalServerError);
                    } 
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
        public ActionResult DeleteImgGridEntity([DataSourceRequest] DataSourceRequest request, ImgGridModelViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                
                //using (var repository = GetRepository())
                //{

                //    var entity = repository.TestRepository
                //                           .GetTestById(model.Id);

                //    if (entity == null)
                //    {
                //        return new JsonHttpStatusResult(new { Message = UserMessages.ROW_NOT_FOUND_ERROR }, HttpStatusCode.InternalServerError);
                //    }

                //    try
                //    {
                //        repository.TestRepository.Remove(entity);
                //        repository.Commit();
                //    }
                //    catch (Exception e)
                //    {
                //        GetLogger().LogException(e, string.Format("DeleteTest({0})", model.Id));

                //        return new JsonHttpStatusResult(new { Message = UserMessages.UNKNOWN_ERROR }, HttpStatusCode.InternalServerError);
                //    }
                //}
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [NonAction]
        private ColumnValidationResult ValidateTaxonomyFields(ImgGridModelViewModel report)
        {
            ColumnValidationResult result = new ColumnValidationResult();

            result.EntityId = report.Id;

            //result = ValidationField("Description", report.Description, "שדה תיאור הינו שדה חובה", result);

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

        private ImgGridModelViewModel CreateImgGridEntityViewModel(ImgGridModel model)
        {
            return new ImgGridModelViewModel()
            {
                Id = model.Id,
                Branch = model.Branch,
                Image1 = model.Image1,
                Image10 = model.Image10,
                Image11 = model.Image11,
                Image2 = model.Image2,
                Image3 = model.Image3,
                Image4 = model.Image4,
                Image5 = model.Image5,
                Image6 = model.Image6,
                Image7 = model.Image7,
                Image8 = model.Image8,
                Image9 = model.Image9,
                Subject1 = model.Subject1,
                Subject10 = model.Subject10,
                Subject11 = model.Subject11,
                Subject2 = model.Subject2,
                Subject3 = model.Subject3,
                Subject4 = model.Subject4,
                Subject5 = model.Subject5,
                Subject6 = model.Subject6,
                Subject7 = model.Subject7,
                Subject8 = model.Subject8,
                Subject9= model.Subject9
            };
        }

        private ImgGridModelViewModel[] GetTestViewModels()
        {
            //ImgGridModel[] testList;

            //using (var repository = GetRepository())
            //{
            // testList = repository.TestRepository.GetTestEntities();
            //}
             
            var testSelectmodel = new List<ImgGridModelViewModel>();

            foreach (var test in items)
            {
                testSelectmodel.Add(CreateImgGridEntityViewModel(test));
            }
            return testSelectmodel.ToArray();

        }

    }
}