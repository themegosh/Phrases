using System;
using System.Web.Mvc;

namespace Phrases.Controllers
{
    public class LayoutInjectorAttribute : ActionFilterAttribute
    {
        private readonly string _masterName;
        public LayoutInjectorAttribute(string masterName)
        {
            _masterName = masterName;
        }

        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
            var result = actionExecutedContext.Result as ViewResult;
            if (result != null)
            {
                result.MasterName = _masterName;
            }
        }
    }
}