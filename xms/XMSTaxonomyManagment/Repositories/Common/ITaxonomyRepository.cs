using XMSTaxonomyManagment.ViewModels;

namespace XMSTaxonomyManagment.Repositories.Common
{
    public interface ITaxonomyRepository
    {
        void Add(TaxonomyModel entity);
        void Remove(TaxonomyModel entity);
        void Update(string oldTaxonomyId,TaxonomyModel entity);
        TaxonomyModel GetTaxonomyById(string taxonomyId);  
        TaxonomyModel[] GetAllTaxonomies(); 
        bool IsUniqueId(string taxonomyId);
    }
}