 
using System;
using System.Diagnostics;
using System.Web.Mvc;
using TempletProject.Common;
using TempletProject.Repositories;


namespace TempletProject.Controllers
{
    public class ControllerLog : ILogger
    {
        public void LogError(string message)
        {
            Debug.WriteLine(message);
        }

        public void LogException(Exception e, string message = null)
        {
            Debug.WriteLine(e.Message, message);
        }

        public void LogWarnnig(string message)
        {
            Debug.WriteLine(message);
        }

        public void WriteLine(string value)
        {
            
        }
    }

    public class BaseController : Controller
    {
        protected IConsistRepositoryContext GetRepository()
        {
            return new ConsistRepositoryContext();
        }

        protected ILogger GetLogger()
        {
            return new ControllerLog();
        }

        public BaseController()
        {
            
        }
         
    }
}