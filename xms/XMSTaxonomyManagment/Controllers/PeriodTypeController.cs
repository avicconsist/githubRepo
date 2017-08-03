using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using XMSTaxonomyManagment.Controllers;
using XMSTaxonomyManagment.ViewModels;

namespace XMSTaxonomyManagment.Controllers
{

    public class PeriodTypeController : BaseController
    {

        public JsonResult GetPeriodTypes()
        {
            List<PeriodTypeModel> periodTypeList;
            using (var repository = GetRepository())
            {
                periodTypeList = repository.PeriodTypeRepository.GetPeriodTypes().ToList();
            }

            var model = new List<PeriodTypeViewModel>(); ;

            model = periodTypeList
                .Select(period =>
                new PeriodTypeViewModel()
                {
                    PeriodType = period.PeriodType,
                    Description = period.Description
                }).ToList();

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}

