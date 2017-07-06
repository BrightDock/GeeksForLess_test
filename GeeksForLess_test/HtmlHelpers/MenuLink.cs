using System;
using System.Web.Mvc;
/// <summary>
/// Extension method for <see cref="HtmlHelper"/> to support highlighting the active tab on the default MVC menu
/// </summary>
/// <param name="htmlHelper"></param>
/// <param name="linkText">The text to display in the link</param>
/// <param name="actionName">Link target action name</param>
/// <param name="controllerName">Link target controller name</param>
/// <param name="activeClass">The CSS class to apply to the link if active</param>
/// <param name="checkAction">If true, checks the current action name to determine if the menu item is 'active', otherwise only the controller name is matched</param>
/// <param name="classes">The CSS classes to apply to all links</param>
/// <returns></returns>

namespace GeeksForLess_test.HtmlHelpers
{
    public static class MenuLinkMaker
    {
        public static MvcHtmlString MenuLink(this HtmlHelper htmlHelper, string linkText, string actionName,
            string controllerName, string activeClass, string[] classes = null, string id = "")
        {
            string currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("Action");
            string currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("Controller");
            bool isActive = false;

            if (String.Equals(string.IsNullOrEmpty(actionName) ? "Index" : actionName, 
                currentAction, StringComparison.OrdinalIgnoreCase)
                && String.Equals(controllerName, currentController, StringComparison.OrdinalIgnoreCase))
            {
                isActive = true;
            }

            return htmlHelper.RawActionLink(linkText, actionName, controllerName, null,
                    new { @class = ((classes != null ? string.Join(" ", classes) : "") + 
                    (isActive.Equals(true) ? ' ' + activeClass : "")), @id = id});
        }
    }
}