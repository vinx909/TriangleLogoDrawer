using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.Editor
{
    public static class DependencyProvider
    {
        public static Dictionary<Type, Func<object>> providedOptions = new Dictionary<Type, Func<object>>();

        public static void Add(Type newReturnType, Func<object> newProvidingMethod)
        {
            providedOptions.Add(newReturnType, newProvidingMethod);
        }
        public static T Provide<T>() where T : class
        {
            if (providedOptions.ContainsKey(typeof(T)))
            {
                return (T)providedOptions[typeof(T)]();
            }
            return null;
        }
    }
}
