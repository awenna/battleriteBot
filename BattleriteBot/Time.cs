using System;

namespace BattleriteBot
{
    public interface ITime
    {
        DateTime Now();
    }

    public static class Time
    {
        public static DateTime Now()
        {
            return DateTime.Now;
        }
    }
}
