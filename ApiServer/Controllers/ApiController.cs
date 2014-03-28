using ApiScheme.Scheme;
using ApiScheme.Server;
using ApiServer.Models;
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



        // ----- Production -----

        public GetCharactersOut GetCharacters(GetCharactersIn i)
        {
            using (var db = new MyDbContext())
            {
                var characters = new List<CharacterInfo>();
                var user = db.Users.FirstOrDefault(u => u.UserId == i.userId);
                if (user != null)
                    characters = user.Characters.Select(c => {
                        return new CharacterInfo() { userId = c.UserId, name = c.Name };
                    }).ToList();
                return new GetCharactersOut() { characters = characters };
            }
        }

        public CreateCharacterOut CreateCharacter(CreateCharacterIn i)
        {
            using (var db = new MyDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.UserId == i.userId);
                if (user == null)
                {
                    user = new User() { UserId = i.userId };
                    db.Users.Add(user);
                }
                var character = new Character() { UserId = user.UserId, Name = i.name };
                db.Characters.Add(character);
                db.SaveChanges();
            }
            return new CreateCharacterOut();
        }
	}
}