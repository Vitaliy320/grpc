using System;
using System.Collections.Generic;
using System.Text;

namespace GrpcGreeter.DbProviders
{
    static class StringExtentions
    {
        public static string WithParameters(this string format, params object[] parameters)
        {
            return string.Format(format, parameters);
        }
    }
}
