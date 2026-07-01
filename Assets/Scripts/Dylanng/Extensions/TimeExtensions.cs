using System;

public static class TimeExtensions
{
    public static string ToTimeFormat(this int totalSeconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(totalSeconds);

        return timeSpan.TotalHours >= 1
            ? $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}"
            : $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }
    
    public static int ToMinutes(this int totalSeconds)
    {
        return totalSeconds / 60;
    }
    
    public static float ToHoursFromSec(this int totalSeconds)
    {
        return totalSeconds / 3600f;
    }
    
    public static float ToHoursFromMin(this float totalMinutes)
    {
        return totalMinutes / 60f;
    }
    
    public static float ToHoursFromMin(this int totalMinutes)
    {
        return totalMinutes / 60f;
    }
}