
using XMSTaxonomyManagment.ViewModels;

namespace XMSTaxonomyManagment.Repositories.Common
{
    public interface ILocalEntityRepository
    {

        LocalEntityModel[] GetLocalEntities();
        LocalEntityModel GetLocalEntityById(string localEntityId); 
        void Add(LocalEntityModel entity);
        void Update(string oldId, LocalEntityModel entity);
        void Remove(LocalEntityModel entity);
        bool IsLocalEntityUniqueId(string id);
    }
}
