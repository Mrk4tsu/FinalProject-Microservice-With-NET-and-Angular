namespace FN.Utilities
{
    public class TimeHelperBuilder : ITimeHelperBuilder
    {
        private DateTime _dateTime;
        public TimeHelperBuilder(DateTime dateTime)
        {
            _dateTime = dateTime;
        }
        public DateTime Build()
        {
            return _dateTime;
        }

        public void RemoveTick()
        {
            _dateTime = new DateTime(_dateTime.Year, _dateTime.Month, _dateTime.Day,
                                _dateTime.Hour, _dateTime.Minute, _dateTime.Second);
        }

        public void SetLocalTime()
        {
            _dateTime = _dateTime.ToLocalTime();
        }

        public void SetTimeZone(string timeZoneId)
        {
            _dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(_dateTime, timeZoneId);
        }
    }
    public interface ITimeHelperBuilder
    {
        void SetTimeZone(string timeZoneId);
        void SetLocalTime();
        void RemoveTick();
        DateTime Build();
    }
}
