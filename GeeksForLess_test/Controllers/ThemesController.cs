﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeeksForLess_test.Models;
using PagedList.Mvc;
using PagedList;
using System.Threading.Tasks;

namespace GeeksForLess_test.Controllers
{
    public class ThemesController : Controller
    {
        // GET: Themes
        [HttpGet]
        public ActionResult Index(int? page)
        {
            var db = new GeeksForLessTestDBEntities();
            var themesList = new List<ThemeModel>();

            foreach (var theme in db.Themes)
            {
                var themeMessages = db.Themes_messages.Where(themeMessage => themeMessage.Theme == theme.Id);
                var themeLikes = db.Likes.Where(Like => Like.Target == theme.Id/* && Like.Target_type == 1*/);

                themesList.Add(new ThemeModel() { Theme = theme, Messages = themeMessages, Likes = themeLikes });
            }
            int pageNumber = (page ?? 1);
            return PartialView(themesList.ToPagedList(pageNumber, Properties.Settings.Default.GET_ITEMS_COUNT));
        }

        [HttpGet]
        public ActionResult GetTheme(long ID = 0)
        {
            if (ID == 0) {
                return View("Error");
            }

            var db = new GeeksForLessTestDBEntities();
            
            var theme = db.Themes.FirstOrDefault(Theme => Theme.Id == ID);
            var likes = db.Likes.Where(Like => Like.Target == theme.Id);

            var Messages = db.Themes_messages.Where(themeMessage => themeMessage.Theme == theme.Id);
            var MessageLikes = new List<CommentLikesView>();
            foreach (var message in Messages)
            {
                var Likes = db.Likes.Where(Like => Like.Target == message.Id/* && Like.Target_type == 1*/);
                MessageLikes.Add(new CommentLikesView() { Comment = message, Likes = Likes });
            }

            ViewBag.user = db.AspNetUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var themeThemeMessage = new ThemeMessagesModel() { Theme = theme, MessagesLikes = MessageLikes, Likes = likes };
            return View(themeThemeMessage);
            
        }

        [HttpGet]
        public ActionResult _PopularThemes()
        {
            var db = new GeeksForLessTestDBEntities();
            var themesList = new List<ThemeModel>();

            foreach (var theme in db.Themes)
            {
                var themeMessages = db.Themes_messages.Where(themeMessage => themeMessage.Theme == theme.Id);
                var themeLikes = db.Likes.Where(Like => Like.Target == theme.Id/* && Like.Target_type == 1*/);

                themesList.Add(new ThemeModel() { Theme = theme, Messages = themeMessages, Likes = themeLikes });
            }
            return PartialView(themesList);
        }

        [Authorize]
        [HttpGet]
        public ActionResult AddTheme()
        {
            var db = new GeeksForLessTestDBEntities();
            var themeView = new AddThemeViewModel();

            themeView.MainTheme = new List<SelectListItem>() {
                new SelectListItem() { Text = "Привязать к теме", Value = "0", Selected = true, Disabled = true } };
            themeView.MainTheme = themeView.MainTheme.Concat(db.Themes.Where(theme => theme.AspNetUsers.UserName == User.Identity.Name)
                .Select(themes => new SelectListItem() {
                Text = themes.Name,
                Value = themes.Id.ToString()}));
            return View(themeView);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddTheme(AddThemeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var db = new GeeksForLessTestDBEntities())
            {
                long temp;
                long.TryParse(model.MainThemeId, out temp);
                string authorID = db.AspNetUsers.Where(user => user.UserName == User.Identity.Name).FirstOrDefault().Id;
                var theme = new Themes
                {
                    Name = model.Name,
                    Text = model.Text,
                    Main_theme = temp == 0 ? null : (long?)temp,
                    Publication_date = DateTime.Now,
                    Author = authorID
                };

                db.Themes.Add(theme);
                await db.SaveChangesAsync();
                return RedirectToAction("GetTheme", "Themes", new { ID = theme.Id });
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult ChangeTheme(long ID = 0)
        {
            if (ID == 0)
            {
                return View("Error");
            }

            var db = new GeeksForLessTestDBEntities();

            var Theme = db.Themes.Join(db.AspNetUsers,
                T => T.Author,
                A => A.Id,
                (T, A) => new { Theme = T, Author = A }
                ).Where(themes => themes.Theme.Id == ID)
                .Select(TA => new ChangeThemeViewModel() {
                    Text = TA.Theme.Text,
                    Name = TA.Theme.Name
                })
                .FirstOrDefault();

            var themeView = new ChangeThemeViewModel();
            themeView.MainTheme = new List< SelectListItem > () {
                new SelectListItem() { Text = "Привязать к теме", Value = "0", Selected = true, Disabled = true } };
            themeView.MainTheme = themeView.MainTheme.Concat(db.Themes.Where(theme => theme.AspNetUsers.UserName == User.Identity.Name && theme.Id != ID)
            .Select(themes => new SelectListItem()
            {
                Text = themes.Name,
                Value = themes.Id.ToString(),
                Selected = (themeView.MainThemeId == themes.Id.ToString() ? true : false)
            }));
            themeView.Id = ID;
            themeView.Name = Theme.Name;
            themeView.Text = Theme.Text;

            return View(themeView);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeTheme(ChangeThemeViewModel model)
        {
            Themes Theme;
            long temp;
            long.TryParse(model.MainThemeId, out temp);

            var db = new GeeksForLessTestDBEntities();
            Theme = db.Themes.Find(model.Id);
            var mainTheme = db.Themes.Find(temp);
                
            if (Theme != null)
            {
                Theme.Main_theme = mainTheme.Id;
                Theme.Name = model.Name;
                Theme.Text = model.Text;
            }
                
            db.SaveChanges();

            return RedirectToAction("GetTheme", "Themes", new { ID = Theme.Id });
        }

        public async Task<ActionResult> RemoveTheme(long ID)
        {
            var db = new GeeksForLessTestDBEntities();

            db.Themes.Remove(db.Themes.FirstOrDefault(theme => theme.Id == ID));

            await db.SaveChangesAsync();

            return RedirectToAction("Index", "Themes");
        }

        private void AddErrors(int result)
        {
            ModelState.AddModelError("", new Exception(result.ToString()));
        }
    }
}