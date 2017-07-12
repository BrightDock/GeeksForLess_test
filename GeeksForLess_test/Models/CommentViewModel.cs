using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeeksForLess_test.Models
{
    public class CommentViewModel
    {
        [Required]
        public long Theme { get; set; }

        public AspNetUsers Author { get; set; }

        public long? Reply_to { get; set; }

        [Required]
        public string Text { get; set; }
    }

    public class ChangeCommentViewModel
    {
        public long? Id { get; set; }

        [Required]
        [Display(Name = "Текст сообщения")]
        public Themes_messages Message { get; set; }

        public string ReturnUrl { get; set; }

        public long? ReplyToId { get; set; }

        [Required]
        [Display(Name = "Ответ к")]
        public IEnumerable<SelectListItem> ReplyToList { get; set; }
    }

    public class CommentLikesView
    {
        [Display(Name = "Сообщение")]
        public Themes_messages Comment { get; set; }

        [Display(Name = "Лайки")]
        public IEnumerable<Likes> Likes { get; set; }
    }
}