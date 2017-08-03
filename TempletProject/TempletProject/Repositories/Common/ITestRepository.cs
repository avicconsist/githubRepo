
using TempletProject.ViewModels; 

namespace TempletProject.Repositories.Common
{
    public interface ITestRepository
    { 
        TestModel[] GetTestEntities();
        TestModel GetTestById(string id);
        void Add(TestModel entity);
        void Update(string oldId, TestModel entity);
        void Remove(TestModel entity);
        bool IsUniqueId(string id);
    }
}
