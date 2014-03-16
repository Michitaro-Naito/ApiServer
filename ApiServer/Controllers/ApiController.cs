using ApiScheme.Scheme;
using ApiScheme.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ApiServer.Controllers
{
    public class ApiController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            var e = filterContext.Exception;
            //filterContext.Result = Json(new { error = e.GetType().Name + ": " + e.Message }, JsonRequestBehavior.AllowGet);
            filterContext.Result = Json(new Out(){ exception = e.GetType().FullName, message = e.Message }, JsonRequestBehavior.AllowGet);
            filterContext.ExceptionHandled = true;
            base.OnException(filterContext);
        }

        public ActionResult Call(string name = null, string json = null)
        {
            var inName = name + "In";
            var outName = name + "Out";
            var inFullName = "ApiScheme.Scheme." + inName;
            var assembly = Assembly.Load("ApiScheme");
            var inType = assembly.GetType(inFullName);//.GetTypes().First(t => t.Name == inName);
            var i = JsonConvert.DeserializeObject(json, inType);
            try
            {
                var res = GetType().GetMethod(name).Invoke(this, new object[] { i });
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (TargetInvocationException e)
            {
                if (e.InnerException != null)
                    throw e.InnerException;
                throw e;
            }
        }

        public PlusOut Plus(PlusIn i)
        {
            return new PlusOut() { c = i.a + i.b, echo = i.echo };
        }

        public GetExceptionOut GetException(GetExceptionIn i)
        {
            ApiScheme.TestApiException e;
            throw new ApiScheme.TestApiException("Exception thrown because requested.");
            //throw new Exception("Exception thrown because requested.");
        }
	}
}