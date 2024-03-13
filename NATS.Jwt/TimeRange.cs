using NATS.Jwt.Internal;

namespace NATS.Jwt
{
    public class TimeRange
    {
        internal TimeRangeJson ToTimeRangeJson()
        {
            return new TimeRangeJson
            {
                Start = Start,
                End = End,
            };
        }

        public string End { get; set; }

        public string Start { get; set; }
    }
}