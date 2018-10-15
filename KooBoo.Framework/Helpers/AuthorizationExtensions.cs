using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace KooBoo.Framework.Helpers
{
    public static class AuthorizationExtensions
    {
        /// <summary>
        /// Determines whether the specified action is authorized by examining all <see cref="IAuthorizationFilter"/> attributes.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="action">The action method to be called.</param>
        /// <returns>
        ///     <c>true</c> if the specified helper is authorized; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAuthorized<TController>(this HtmlHelper helper, Expression<Action<TController>> action)
        {
            bool returnValue = false;

            var call = action.Body as MethodCallExpression;

            if (call == null) return false;

            var authorizeAttributes = call.GetAttributes<IAuthorizationFilter>();
            if (authorizeAttributes.Length == 0) return true;

            var controllerContext = helper.ViewContext.Controller.ControllerContext;
            var controllerDescriptor = new ReflectedControllerDescriptor(typeof(TController));
            var actionDescriptor = new ReflectedActionDescriptor(call.Method, call.Method.Name, controllerDescriptor);

            returnValue = authorizeAttributes.Any(a => IsAuthorized(a, controllerContext, actionDescriptor));

            return returnValue;
        }

        /// <summary>
        /// Determines whether the specified authorization filter is authorized.
        /// </summary>
        /// <param name="authorizationFilter">The authorization filter.</param>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionDescriptor">The action descriptor.</param>
        /// <returns>
        ///     <c>true</c> if the specified authorization filter is authorized; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsAuthorized(IAuthorizationFilter authorizationFilter, ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var authorizationContext = new AuthorizationContext(controllerContext, actionDescriptor);

            authorizationFilter.OnAuthorization(authorizationContext);

            return (authorizationContext.Result == null);
        }
    } 
}