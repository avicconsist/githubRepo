using System;
using TempletProject.Repositories.Common; 


namespace TempletProject.Repositories
{
    public interface IConsistRepositoryContext : IDisposable 
    {
        ITestRepository TestRepository { get; }  

        void Commit();
        void Rollback();
    }
}