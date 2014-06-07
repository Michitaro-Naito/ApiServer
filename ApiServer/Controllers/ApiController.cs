using ApiScheme;
using ApiScheme.Scheme;
using ApiScheme.Server;
using ApiServer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
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
            filterContext.Result = Json(new Out(){ exception = e.GetType().FullName, message = e.ToString() }, JsonRequestBehavior.AllowGet);
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
                    return Json(new Out() { exception = e.InnerException.GetType().FullName, message = e.InnerException.ToString() }, JsonRequestBehavior.AllowGet);
                return Json(new Out() { exception = e.GetType().FullName, message = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                // Very bad case. Unknown error.
                return Json(new Out() { exception = e.GetType().FullName, message = e.ToString() }, JsonRequestBehavior.AllowGet);
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

        public GetBlacklistOut GetBlacklist(GetBlacklistIn i)
        {
            using (var db = new MyDbContext())
            {
                var ids = db.BannedIds.OrderBy(id=>id.UserId).Skip(50 * i.page).Take(50).ToList();
                return new GetBlacklistOut() { infos = ids.Select(id => new BannedIdInfo() { userId = id.UserId }).ToList() };
            }
        }

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
                db.SaveChanges();
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException)
                {
                    throw new ApiNotUniqueException("Failed to insert a Character.");
                }
            }
            return new CreateCharacterOut();
        }

        //[DebuggerStepThrough]
        public TransactionOut Transaction(TransactionIn i)
        {
            var retry = false;
            for (var n = 0; n < 1000; n++)
            {
                using (var db = new MyDbContext())
                {
                    try
                    {
                        i.infos.ForEach(info =>
                        {
                            var target = db.Characters.Single(c => c.Name == info.characterName);
                            if(info.items != null)
                                target.Items = target.Items.AddResult(info.items);
                        });
                    }
                    catch (InvalidOperationException e)
                    {
                        throw new ApiOutOfRangeException(e.Message);
                    }

                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {
                        retry = true;
                    }
                }
                if (!retry)
                    break;
                Thread.Sleep(10);
            }
            if (retry)
                throw new ApiBusyForNowException("Max retry exceeded.");
            return new TransactionOut();
        }

        public AddPlayLogOut AddPlayLog(AddPlayLogIn i)
        {
            using (var db = new MyDbContext())
            {
                try
                {
                    var log = PlayLog.FromInfo(i.log);
                    db.PlayLogs.Add(log);
                    db.SaveChanges();
                    return new AddPlayLogOut() { id = log.PlayLogId };
                }
                catch
                {
                    throw new ApiNotUniqueException("Failed to insert a PlayLog");
                }
            }
        }

        public GetPlayLogsOut GetPlayLogs(GetPlayLogsIn i)
        {
            using (var db = new MyDbContext())
            {
                var logs = db.PlayLogs
                    .OrderByDescending(l => l.PlayLogId)
                    .Skip(50 * i.page)
                    .Take(50)
                    .ToList()
                    .Select(l => l.ToInfo() /*new PlayLogInfo() { id = l.PlayLogId, created = l.Created, roomName = l.RoomName, fileName = l.FileName }*/)
                    .ToList();
                return new GetPlayLogsOut() { playLogs = logs };
            }
        }

        public GetPlayLogByIdOut GetPlayLogById(GetPlayLogByIdIn i)
        {
            using (var db = new MyDbContext())
            {
                PlayLogInfo info = null;
                var log = db.PlayLogs.Find(i.id);
                if (log != null)
                    info = log.ToInfo() /*new PlayLogInfo() { id = log.PlayLogId, created = log.Created, roomName = log.RoomName, fileName = log.FileName };*/;
                return new GetPlayLogByIdOut() { playLog = info };
            }
        }

        public ReportMessageOut ReportMessage(ReportMessageIn i)
        {
            Debug.WriteLine(i);
            using (var db = new MyDbContext())
            {
                var report = new Report()
                {
                    UserId = i.userId,
                    Note = i.note,
                    JsonMessages = JsonConvert.SerializeObject(i.messages)
                };
                db.Reports.Add(report);
                db.SaveChanges();
            }
            return new ReportMessageOut();
        }

        public ReportGameServerStatusOut ReportGameServerStatus(ReportGameServerStatusIn i)
        {
            Debug.WriteLine(i);
            if (i.status == null)
                throw new ApiException("status must not be null.");
            using (var db = new MyDbContext())
            {
                var status = db.GameServerStatuses.FirstOrDefault(s => s.Name == i.status.name);
                if (status == null)
                {
                    status = new ApiServer.Models.GameServerStatus();
                    db.GameServerStatuses.Add(status);
                }
                status.Updated = DateTime.UtcNow;
                status.Host = i.status.host;
                status.Port = i.status.port;
                status.Name = i.status.name;
                status.Players = i.status.players;
                status.MaxPlayers = i.status.maxPlayers;
                status.FramesPerInterval = i.status.framesPerInterval;
                status.ReportIntervalSeconds = i.status.reportIntervalSeconds;
                status.MaxElapsedSeconds = i.status.maxElapsedSeconds;

                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    throw new ApiBusyForNowException("Failed to update GameServerStatus. Maybe ApiServer is busy for now.");
                }
            }
            return new ReportGameServerStatusOut();
        }

        public GetGameServersOut GetGameServers(GetGameServersIn i)
        {
            using (var db = new MyDbContext())
            {
                var ago = DateTime.UtcNow.AddSeconds(-20);
                var servers = db.GameServerStatuses.Where(s=>s.Updated > ago).Select(s => new ApiScheme.Scheme.GameServerStatus()
                {
                    host = s.Host,
                    port = s.Port,
                    name = s.Name,
                    players = s.Players,
                    maxPlayers = s.MaxPlayers,
                    framesPerInterval = s.FramesPerInterval,
                    reportIntervalSeconds = s.ReportIntervalSeconds,
                    maxElapsedSeconds = s.MaxElapsedSeconds
                }).ToList();
                return new GetGameServersOut() { servers = servers };
            }
        }

        public GetStatisticsOut GetStatistics(GetStatisticsIn i)
        {
            using (var db = new MyDbContext())
            {
                return new GetStatisticsOut()
                {
                    users = db.Users.Count(),
                    characters = db.Characters.Count(),
                    playings = db.GameServerStatuses.Sum(s => s.Players),
                    playlogs = db.PlayLogs.Count()
                };
            }
        }
	}
}