using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.ApplicationCore.Services.Exceptions
{
    public static class ExceptionProvider
    {
        private const string incorrectIdMessage = "the given Id of {0} was not found in {1}{(3==true) ? 'this may be because of lazy loading'}";
        private const string nullPointerMessage = "null found where an instance of {0} was expected. could not be found when looking for value {2} as {1}";

        public static IndexOutOfRangeException GetIncorrectIdAsIndexOutOfRangeException(int idThatCanNotBeFound, string nameOfGroupInWhichItShouldBeFound, bool canBeCausedByLazyLoading)
        {
            return new IndexOutOfRangeException(string.Format(incorrectIdMessage, idThatCanNotBeFound, nameOfGroupInWhichItShouldBeFound, canBeCausedByLazyLoading));
        }

        internal static Exception GetNullPointerException(string typeWhichIsNull, string proppertyCheckedFor, string valueCheckedFor)
        {
            return new NullReferenceException(string.Format(nullPointerMessage, typeWhichIsNull, proppertyCheckedFor, valueCheckedFor));
        }
    }
}
