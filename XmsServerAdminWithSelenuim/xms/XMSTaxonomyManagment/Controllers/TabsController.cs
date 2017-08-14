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
    public class TabsController : BaseController
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

            var ViewModel = new TaxonomyMainViewModel()
            {
                TaxonomyReport = new TaxonomyReportMainViewModel()
                {
                    Taxonomies = taxonomyViewModels.ToArray(),
                    SelectedTaxonomy = taxonomyViewModels.FirstOrDefault(),
                    PeriodTypeDefaultValue = defaultPeriodTypeViewModel
                },
                LocalReport = new LocalReportMainViewModel()
                {
                    Taxonomies = taxonomyViewModels.ToArray(),
                    SelectedTaxonomy = taxonomyViewModels.FirstOrDefault()
                }
            }; 

            return View(ViewModel);
        }

    }
}