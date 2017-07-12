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
        public ActionResult ChangeComment(long? ID)
        {
            var returnUrl = Request.UrlReferrer.LocalPath;
            if (ID.HasValue) {
                var db = new GeeksForLessTestDBEntities();
                var messageRepeatTo = new ChangeCommentViewModel();
                var message = db.Themes_messages.FirstOrDefault(m => m.Id == ID.Value);

                messageRepeatTo.Message = message;
                messageRepeatTo.ReplyToList = new List<SelectListItem>() {
                new SelectListItem() { Text = "Выбрать ответ", Value = "null", Selected = true } };
                messageRepeatTo.ReplyToList = messageRepeatTo.ReplyToList.Concat(db.Themes_messages
                    .Where(m => m.Id != ID.Value && m.Theme == message.Theme && m.Publication_date < message.Publication_date)
                    .Select(rTo => new SelectListItem() {
                        Text = rTo.Text,
                        Value = rTo.Id.ToString(),
                        Selected = message.Reply_to == rTo.Id ? true : false
                    }));
                if (messageRepeatTo.ReplyToList.Where(m => m.Selected == true).ToList().Count > 1)
                {
                    messageRepeatTo.ReplyToList.First().Selected = false;
                    messageRepeatTo.ReplyToId = int.Parse(messageRepeatTo.ReplyToList.Last().Value);
                }
                messageRepeatTo.Id = ID.Value;

                ViewBag.ReturnUrl = returnUrl;
                return PartialView(messageRepeatTo);
            }
            return PartialView(null);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangeComment(ChangeCommentViewModel model, string returnUrl)
        {
            if (model == null || !model.Id.HasValue)
            {
                return RedirectToLocal(returnUrl);
            }

            var db = new GeeksForLessTestDBEntities();

            var Comment = db.Themes_messages.FirstOrDefault(m => m.Id == model.Id.Value);

            if (Comment != null)
            {
                Comment.Reply_to = model.ReplyToId;
                Comment.Text = model.Message.Text;
            }
            db.SaveChanges();


            var theme = Comment.Themes;
            var Messages = db.Themes_messages.Where(themeMessage => themeMessage.Theme == theme.Id);
            var MessageLikes = new List<CommentLikesView>();
            foreach (var message in Messages)
            {
                var Likes = db.Likes.Where(Like => Like.Target == message.Id/* && Like.Target_type == 1*/);
                MessageLikes.Add(new CommentLikesView() { Comment = message, Likes = Likes });
            }

            ViewBag.ReturnUrl = returnUrl;
            ViewBag.user = db.AspNetUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.Theme = theme;
            return PartialView("Messages", MessageLikes);
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
            return RedirectToAction("Index", "Home");
        }
    }
}