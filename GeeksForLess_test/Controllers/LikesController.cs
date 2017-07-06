using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeeksForLess_test.Models
{
    public class LikesController : Controller
    {
        // GET: Likes
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddLike(long? ID)
        {
            var db = new GeeksForLessTestDBEntities();
            var likes = db.Likes.Where(m => m.Target == ID).ToList();
            var userLike = db.Likes.FirstOrDefault(m => m.Target == ID.Value && m.AspNetUsers.UserName == User.Identity.Name);
            if (!ID.HasValue)
            {
                return PartialView("_LikesPart", new Tuple<IEnumerable<Likes>, long>(likes, ID.Value));
            }

            if (userLike == null) {
                var user = db.AspNetUsers.FirstOrDefault(m => m.UserName == User.Identity.Name).Id;
                db.Likes.Add(new Likes() { Target = ID.Value, Like_author = user, Target_type = 2 });
                db.SaveChanges();
            }
            likes = db.Likes.Where(m => m.Target == ID).ToList();

            return PartialView("_LikesPart", new Tuple<IEnumerable<Likes>, long>(likes, ID.Value));
        }

        public ActionResult RemoveLike(long? ID)
        {
            var db = new GeeksForLessTestDBEntities();
            var likes = db.Likes.Where(m => m.Target == ID).ToList();
            var userLike = db.Likes.FirstOrDefault(m => m.Target == ID.Value && m.AspNetUsers.UserName == User.Identity.Name);
            if (!ID.HasValue)
            {
                return PartialView("_LikesPart", new Tuple<IEnumerable<Likes>, long>(likes, ID.Value));
            }

            if (userLike != null)
            {
                db.Entry(userLike).State = EntityState.Deleted;
                db.SaveChanges();
            }
            likes = db.Likes.Where(m => m.Target == ID).ToList();

            return PartialView("_LikesPart", new Tuple<IEnumerable<Likes>, long>(likes, ID.Value));
        }
    }
}