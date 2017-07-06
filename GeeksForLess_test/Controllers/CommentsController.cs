using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeeksForLess_test.Models;
using System.Threading.Tasks;
using GeeksForLess_test.Extensions;

namespace GeeksForLess_test.Controllers
{
    public class CommentsController : Controller
    {
        // GET: Comments
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult AddComment(long? ID)
        {
            if (!ID.HasValue)
            {
                return View();
            }

            var db = new GeeksForLessTestDBEntities();

            var theme = db.Themes.Where(Theme => Theme.Id == ID).FirstOrDefault();
            var user = db.AspNetUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            
            var themeThemeMessage = new CommentViewModel() { Theme = theme.Id, Author = user };
            return PartialView(themeThemeMessage);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddComment(CommentViewModel model)
        {
//            ViewBag.str = model.Reply_to + ' ' + model.Text + ' ' + model.Theme + ' ' + model.Author.Id;

            if (!ModelState.IsValid || model == null)
            {
                return PartialView(new CommentViewModel());
            }

            var db = new GeeksForLessTestDBEntities();
            long temp;
            long.TryParse(model.Reply_to.ToString(), out temp);

            var message = new Themes_messages()
            {
                Theme = model.Theme,
                Text = model.Text,
                Reply_to = temp == 0 ? null : (long?)temp,
                Publication_date = DateTime.Now,
                Author = User.Identity.GetIdOfUser()
            };

            db.Themes_messages.Add(message);
            await db.SaveChangesAsync();
            return RedirectToAction("GetTheme", "Themes", new { ID = model.Theme });
        }

        [Authorize]
        [HttpGet]
        public ActionResult ChangeComment(long? ID)
        {
            long? messageId = ID;

            if (messageId != null) {
                var db = new GeeksForLessTestDBEntities();
                var messageRepeatTo = new ChangeCommentViewModel();
                var message = db.Themes_messages.FirstOrDefault(m => m.Id == messageId);

                messageRepeatTo.Message = message;
                messageRepeatTo.ReplyToList = new List<SelectListItem>() {
                new SelectListItem() { Text = "Выбрать ответ", Value = "0", Selected = true, Disabled = true } };
                messageRepeatTo.ReplyToList = messageRepeatTo.ReplyToList.Concat(db.Themes_messages
                    .Where(m => m.Id != messageId && m.Theme == message.Theme)
                    .Select(rTo => new SelectListItem() {
                        Text = rTo.Text,
                        Value = rTo.Id.ToString(),
                        Selected = message.Themes_messages2.Id == rTo.Id ? true : false
                    }));

                return PartialView(messageRepeatTo);
            }
            return PartialView(null);
        }

        [Authorize]
        [HttpPost]
        public bool ChangeComment(ChangeCommentViewModel model)
        {
            
            return true;
        }

        public async Task<ActionResult> RemoveComment(long? ID)
        {
            var db = new GeeksForLessTestDBEntities();

            db.Themes_messages.Remove(db.Themes_messages.FirstOrDefault(message => message.Id == ID));

            await db.SaveChangesAsync();

            return View();
        }
    }
}