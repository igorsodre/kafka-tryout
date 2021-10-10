using System;

namespace API.Extensions.BaseTypes
{
    public static class IntegerExtensions
    {
        public static TimeSpan Minutes(this int value)
        {
            return TimeSpan.FromMinutes(value);
        }
        
        public static TimeSpan Hours(this int value)
        {
            return TimeSpan.FromHours(value);
        }
        
        public static TimeSpan Days(this int value)
        {
            return TimeSpan.FromDays(value);
        }
    }
}
