using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeeksForLess_test.Models
{
    public class AddThemeViewModel
    {
        [Required]
        [Display(Name = "Название темы")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Текст темы")]
        public string Text { get; set; }

        [Display(Name = "Главная тема")]
        public string MainThemeId { get; set; }
        public IEnumerable<SelectListItem> MainTheme { get; set; }
    }

    public class ChangeThemeViewModel
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Заголовок темы")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Текст темы")]
        public string Text { get; set; }

        public long? MainThemeId { get; set; }

        [Display(Name = "Главная тема")]
        public IEnumerable<SelectListItem> MainTheme { get; set; }
    }
}