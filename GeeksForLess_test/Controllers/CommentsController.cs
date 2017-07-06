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
                return PartialView(new CommentViewModel());
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

            if (!ModelState.IsValid)
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
        public ActionResult ChangeComment(long? ID, string returnUrl)
        {
            if (ID.HasValue) {
                var db = new GeeksForLessTestDBEntities();
                var messageRepeatTo = new ChangeCommentViewModel();
                var message = db.Themes_messages.FirstOrDefault(m => m.Id == ID);

                messageRepeatTo.Message = message;
                messageRepeatTo.ReplyToList = new List<SelectListItem>() {
                new SelectListItem() { Text = "Выбрать ответ", Value = "null", Selected = true } };
                messageRepeatTo.ReplyToList = messageRepeatTo.ReplyToList.Concat(db.Themes_messages
                    .Where(m => m.Id != ID && m.Theme == message.Theme && m.Publication_date < message.Publication_date)
                    .Select(rTo => new SelectListItem() {
                        Text = rTo.Text,
                        Value = rTo.Id.ToString(),
                        Selected = message.Reply_to == rTo.Id ? true : false
                    }));

                ViewBag.ReturnUrl = returnUrl;
                return PartialView(messageRepeatTo);
            }
            return PartialView(null);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangeComment(ChangeCommentViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToLocal(returnUrl);
            }

            Themes_messages Comment;
            var db = new GeeksForLessTestDBEntities();
            Comment = db.Themes_messages.Find(model.Message.Id);
            var ReplyTo = db.Themes.Find(model.ReplyToId.Id);

            if (Comment != null)
            {
                Comment.Reply_to = ReplyTo.Id;
                Comment.Text = model.Message.Text;
            }

            db.SaveChanges();

            return PartialView(Comment);
        }

        public async Task<ActionResult> RemoveComment(long? ID)
        {
            var db = new GeeksForLessTestDBEntities();
            var comment = db.Themes_messages.Find(ID);
            var theme = comment.Themes;

            db.Themes_messages.Remove(comment);

            await db.SaveChangesAsync();

            return RedirectToAction("GetTheme", "Themes", new { ID = theme.Id });
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Themes");
        }
    }
}