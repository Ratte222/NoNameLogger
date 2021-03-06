
namespace NoNameLogger.Enums
{
    public enum RollingInterval
    {
        //     The log file will never roll; no time period information will be appended to
        //     the log file name.
        Infinite = 0,
        //     Roll every year. Filenames will have a four-digit year appended in the pattern
        //     yyyy
        //     .
        Year = 1,
        //     Roll every calendar month. Filenames will have
        //     yyyyMM
        //     appended.
        Month = 2,
        //     Roll every day. Filenames will have
        //     yyyyMMdd
        //     appended.
        Day = 3,
        //     Roll every hour. Filenames will have
        //     yyyyMMddHH
        //     appended.
        Hour = 4,
        //     Roll every minute. Filenames will have
        //     yyyyMMddHHmm
        //     appended.
        Minute = 5
    }
}
