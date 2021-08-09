using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.SimpleDependencyProvider
{
    public static class DependencyProvider
    {
        private const string dependencyNotProvidedExceptionString = "the Dependencyprovider does not have a method of providing an instance of {0}";

        private static readonly Dictionary<Type, Func<object>> providedOptions = new();

        public static void Add<T>(Func<T> providingFunction) where T : class
        {
            providedOptions.Add(typeof(T), providingFunction);
        }
        public static T Get<T>() where T : class
        {
            if (providedOptions.ContainsKey(typeof(T)))
            {
                return (T)providedOptions[typeof(T)].Invoke();
            }
            else
            {
                throw new KeyNotFoundException(string.Format(dependencyNotProvidedExceptionString, typeof(T).Name));
            }
        }
    }
}
