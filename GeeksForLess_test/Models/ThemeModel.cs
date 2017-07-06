using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeeksForLess_test.Models
{
    public class ThemeModel
    {
        public Themes Theme { get; set; }
        public IEnumerable<Themes_messages> Messages { get; set; }
        public IEnumerable<Likes> Likes { get; set; }
    }

    public class ThemeMessagesModel
    {
        public Themes Theme { get; set; }
        public IEnumerable<Likes> Likes { get; set; }
        public IEnumerable<MessagesLikesView> MessagesLikes { get; set; }
    }
}