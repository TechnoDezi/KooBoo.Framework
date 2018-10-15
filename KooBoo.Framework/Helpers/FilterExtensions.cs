using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace KooBoo.Framework.Helpers
{
    public static class FilterExtensions
    {
        /// <summary>
        /// Returns specified attributes for an action descriptor.
        /// </summary>
        /// <param name="call">The call.</param>
        /// <returns>Returns specified attributes for an action method along with the action's controller.</returns>
        public static TAttribute[] GetAttributes<TAttribute>(this ControllerDescriptor controller)
        {
            return controller.GetCustomAttributes(typeof(TAttribute), true)
                .Cast<TAttribute>()
                .ToArray();
        }

        /// <summary>
        /// Returns specified attributes for an action descriptor.
        /// </summary>
        /// <param name="call">The call.</param>
        /// <returns>Returns specified attributes for an action method along with the action's controller.</returns>
        public static TAttribute[] GetAttributes<TAttribute>(this ActionDescriptor action)
        {
            return action.GetCustomAttributes(typeof(TAttribute), true)
                .Cast<TAttribute>()
                .ToArray();
        }

        /// <summary>
        /// Returns specified attributes for an action method along with the action's controller.
        /// </summary>
        /// <param name="call">The call.</param>
        /// <returns>Returns specified attributes for an action method along with the action's controller.</returns>
        public static TAttribute[] GetAttributes<TAttribute>(this MethodCallExpression call)
        {
            return call.Object.Type.GetCustomAttributes(typeof(TAttribute), true)
                .Union(call.Method.GetCustomAttributes(typeof(TAttribute), true))
                .Cast<TAttribute>()
                .ToArray();
        }
    }
}