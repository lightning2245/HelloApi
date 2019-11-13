using System;

namespace Hello.Common
{    
    public class Validators
    {        
        public static T ThrowArgNullExcIfNull<T>(T argument, string argumentName) where T : class
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
            return argument;
        }
        
        public static DateTime DateToUtc(DateTime argument, string argumentName)
        {
            if (argument.Kind == DateTimeKind.Unspecified)
            {
                throw new ArgumentException(argumentName);
            }

            if (argument.Kind == DateTimeKind.Local)
            {
                return argument.ToUniversalTime();
            }

            return argument;
        }
        
        public static DateTime DateToLocal(DateTime argument, string argumentName)
        {
            if (argument.Kind == DateTimeKind.Unspecified)
            {
                throw new ArgumentException(argumentName);
            }

            if (argument.Kind == DateTimeKind.Utc)
            {
                return argument.ToLocalTime();
            }

            return argument;
        }

    }
}
