using ApiScheme;
using ApiScheme.Scheme;
using ApiScheme.Server;
using ApiServer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ApiServer.Controllers
{
    public class ApiController : Controller
    {
        /// <summary>
        /// Handles Exceptions.
        /// The final fortification to display errors.
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            // Worst case. Unknown error.
            var e = filterContext.Exception;
            filterContext.Result = Json(new Out(){ exception = e.GetType().FullName, message = e.Message }, JsonRequestBehavior.AllowGet);
            filterContext.ExceptionHandled = true;
            base.OnException(filterContext);
        }

        /// <summary>
        /// Called by clients.
        /// Invokes internal API and return results.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public ActionResult Call(string name = null, string json = null)
        {
            // Tries to call internal API.
            var inName = name + "In";
            var outName = name + "Out";
            var inFullName = "ApiScheme.Scheme." + inName;
            var assembly = Assembly.Load("ApiScheme");
            var inType = assembly.GetType(inFullName);
            var i = JsonConvert.DeserializeObject(json, inType);
            try
            {
                var res = GetType().GetMethod(name).Invoke(this, new object[] { i });
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (TargetInvocationException e)
            {
                // Bad case. Exception not handled gracefully.
                if (e.InnerException != null)
                    return Json(new Out() { exception = e.InnerException.GetType().FullName, message = e.InnerException.Message }, JsonRequestBehavior.AllowGet);
                return Json(new Out() { exception = e.GetType().FullName, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                // Very bad case. Unknown error.
                return Json(new Out() { exception = e.GetType().FullName, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        // ----- Internal API implementations below -----

        /// <summary>
        /// Calculates a + b = c.
        /// Debugging purposes only.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public PlusOut Plus(PlusIn i)
        {
            return new PlusOut() { c = i.a + i.b, echo = i.echo };
        }

        /// <summary>
        /// Throws an TestApiException.
        /// Debugging purposes only.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public GetExceptionOut GetException(GetExceptionIn i)
        {
            throw new ApiScheme.TestApiException("Exception thrown because requested.");
        }



        // ----- Production -----

        /// <summary>
        /// Gets Characters of an User.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates a Character for a User.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
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
                if (user.Characters.Count >= 3)
                    throw new ApiMaxReachedException("Max characters exceeded.");

                var character = new Character() { UserId = user.UserId, Name = i.name };
                db.Characters.Add(character);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    throw new ApiNotUniqueException("Failed to insert a Character.");
                }
            }
            return new CreateCharacterOut();
        }
	}
}