using ApiScheme;
using ApiScheme.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ApiServer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            /*var random = new Random();

            Parallel.Invoke(() =>
            {
                Parallel.For(0, 100, n =>
                {
                    var infos = new List<TransactionInfo>();
                    infos.Add(new TransactionInfo() { characterName = "User0", items = new CharacterItems() { gold = 1 } });
                    infos.Add(new TransactionInfo() { characterName = "User1", items = new CharacterItems() { gold = -1 } });
                    ApiScheme.Client.Api.Get<TransactionOut>(new TransactionIn() { infos = infos });
                });
            }, () =>
            {
                Parallel.For(0, 100, n =>
                {
                    var infos = new List<TransactionInfo>();
                    infos.Add(new TransactionInfo() { characterName = "User0", items = new CharacterItems() { gold = 1 } });
                    infos.Add(new TransactionInfo() { characterName = "User2", items = new CharacterItems() { gold = -1 } });
                    ApiScheme.Client.Api.Get<TransactionOut>(new TransactionIn() { infos = infos });
                });
            });*/

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}