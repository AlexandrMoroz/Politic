using Microsoft.Owin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace WebApplication6.Models
{
    public static class Helper
    {
        public static MvcHtmlString RawActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            var repID = Guid.NewGuid().ToString();
            var lnk = ajaxHelper.ActionLink(repID, actionName, controllerName, routeValues, ajaxOptions, htmlAttributes);
            return MvcHtmlString.Create(lnk.ToString().Replace(repID, linkText));
        }

        public static MvcHtmlString RawActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            var repID = Guid.NewGuid().ToString();
            var lnk = htmlHelper.ActionLink(repID, actionName, controllerName, routeValues, htmlAttributes);
            return MvcHtmlString.Create(lnk.ToString().Replace(repID, linkText));
        }

        public static string RenderPartialView(this Controller controller, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        public static MvcHtmlString PageLinks(this HtmlHelper html,
        PageInfo pageInfo, Func<int, string> pageUrl, object htmlAttributes)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pageInfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                // если текущая страница, то выделяем ее,
                // например, добавляя класс
                if (i == pageInfo.PageNumber)
                {
                }
                tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }

        public static JsonResult JsonValidation(this ModelStateDictionary state)
        {
            return new JsonResult {
                Data = new {
                    
                    Tag = "ValidationError",
                    State = from e in state
                            where e.Value.Errors.Count > 0
                            select new {
                                Name = e.Key,
                                Errors = e.Value.Errors.Select(x => x.ErrorMessage)
                                  .Concat(e.Value.Errors.Where(x => x.Exception != null).Select(x => x.Exception.Message))
                            }
                }
            };
        }


    }

    public class PostCreateViewModel
    {
        [Required(ErrorMessage = "Введите назнание статьи")]
        [StringLength(500, MinimumLength = 6, ErrorMessage = "Название должно должно иметь минимум 6 знаков")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Введите 3 или более тега через запятую")]
        [TagsMinLengthValidateAttribute(ErrorMessage = "Должно быть более {0} тегов", MinLength = 3)]
        [TagsMaxLengthValidateAttribute(ErrorMessage = "Должно быть Meнее {0} тегов", MaxLength = 10)]
        [RegularExpression("[^a-zа-яA-ZА-Я0-9,\\s]", ErrorMessage = "Тег может содержать только пробелы, запятые, цифры, латинские и кирилические символы ")]
        public string Tags { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int PersonId { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int PostType { get; set; }
        [IsImageValidateAtteibute(ErrorMessage = "Разрешена загрузка только изображений формата jpg,gif,png")]
        public HttpPostedFileBase[] Files { get; set; }
        [MustHaveOneElementAttribute(ErrorMessage = "Пост должен содержать хотябы один раздел текста или картинки")]
        public Dictionary<string, string> Order { get; set; }

    }
    public class MustHaveOneElementAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as IDictionary;
            if (list != null)
            {
                return list.Count > 0;
            }
            return false;
        }
    }
    public class TagsMinLengthValidateAttribute : ValidationAttribute
    {
        public int MinLength { get; set; }
        public TagsMinLengthValidateAttribute()
        {
            MinLength = 3;
        }
        public override bool IsValid(object value)
        {
            var str = value as String;
            if (str != null)
            {
                return str.Split(',').Count() >= MinLength;
            }

            return false;
        }
    }
    public class TagsMaxLengthValidateAttribute : ValidationAttribute
    {
        public int MaxLength { get; set; }
        public TagsMaxLengthValidateAttribute()
        {
            MaxLength = 10;
        }
        public override bool IsValid(object value)
        {
            var str = value as String;
            if (str != null)
            {
                return str.Split(',').Count() < MaxLength;
            }

            return false;
        }
    }
    public class IsImageValidateAtteibute : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            var files = value as HttpPostedFileBase[];
            if (files == null)
            {
                return true;
            }
            foreach (var item in files)
            {
                if (!ValidateFile(item))
                {
                    return false;
                }
            }
            return true;
        }
        public bool ValidateFile(HttpPostedFileBase file)
        {
            switch (file.ContentType)
            {
                // Example: return valid = true for following file types:
                case ("image.gif"):
                case ("image/jpg"):
                case ("image/jpeg"):
                case ("image/png"):
                    return true;

                // Otherwise if anything else, return false
                default: return false;
            }
        }
    }

    public class PageInfo
    {
        public int PageNumber { get; set; } // номер текущей страницы
        public int PageSize { get; set; } // кол-во объектов на странице
        public int TotalItems { get; set; } // всего объектов
        public int TotalPages  // всего страниц
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
    }
    public class SearchStringViewModel
    {
        [MinLength(4, ErrorMessage = "Строка должна содержать минимум 4 символа")]
        [MaxLength(20, ErrorMessage = "Строка должна содержать максимум 20 символов")]
        public string SearchString { get; set; }
        public int Id { get; set; }
    }
    public class PersonIndexViewModel
    {
        public Dictionary<Position, List<Person>> PeoplePerPosition { get; set; }
    }
}