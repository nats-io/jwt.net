using System;

namespace NATS.Jwt.Internal
{
    internal sealed class DurationJson
    {
        public const long NanosPerMilli = 1_000_000L;
        public const long NanosPerSecond = 1_000_000_000L;
        public const long NanosPerMinute = NanosPerSecond * 60L;
        public const long NanosPerHour = NanosPerMinute * 60L;
        public const long NanosPerDay = NanosPerHour * 24L;

        public static readonly DurationJson Zero = new DurationJson(0L);
        public static readonly DurationJson One = new DurationJson(1L);

        /// <summary>
        /// Gets the value of the duration in nanoseconds
        /// </summary>
        public long Nanos { get; }

        /// <summary>
        /// Gets the value of the duration in milliseconds, truncating any nano portion
        /// </summary>
        public int Millis => Convert.ToInt32(Nanos / NanosPerMilli);

        private DurationJson(long nanos)
        {
            Nanos = nanos;
        }

        /// <summary>
        /// Create a Duration from nanoseconds
        /// </summary>
        public static DurationJson OfNanos(long nanos)
        {
            return new DurationJson(nanos);
        } 

        /// <summary>
        /// Create a Duration from milliseconds
        /// </summary>
        public static DurationJson OfMillis(long millis)
        {
            return new DurationJson(millis * NanosPerMilli);
        } 

        /// <summary>
        /// Create a Duration from seconds
        /// </summary>
        public static DurationJson OfSeconds(long seconds)
        {
            return new DurationJson(seconds * NanosPerSecond);
        }

        /// <summary>
        /// Create a Duration from minutes
        /// </summary>
        public static DurationJson OfMinutes(long minutes)
        {
            return new DurationJson(minutes * NanosPerMinute);
        }

        /// <summary>
        /// Create a Duration from hours
        /// </summary>
        public static DurationJson OfHours(long hours)
        {
            return new DurationJson(hours * NanosPerHour);
        }

        /// <summary>
        /// Create a Duration from days
        /// </summary>
        public static DurationJson OfDays(long days)
        {
            return new DurationJson(days * NanosPerDay);
        }

        /// <summary>
        /// Is the value equal to 0
        /// </summary>
        /// <returns>true if value is 0</returns>
        public bool IsZero()
        {
            return Nanos == 0;
        }

        /// <summary>
        /// Is the value negative (less than zero)
        /// </summary>
        /// <returns>true if value is negative</returns>
        public bool IsNegative()
        {
            return Nanos < 0;
        }

        /// <summary>
        /// Is the value positive (greater than zero)
        /// </summary>
        /// <returns>true if value is positive</returns>
        public bool IsPositive()
        {
            return Nanos > 0;
        }
        
        public override bool Equals(object obj)
        {
            return Equals(obj as DurationJson);
        }

        private bool Equals(DurationJson other)
        {
            return other != null && Nanos == other.Nanos;
        }

        public override int GetHashCode()
        {
            return Nanos.GetHashCode();
        }

        public override string ToString()
        {
            return Nanos.ToString();
        }
    }

    internal static class DurationJsonConvert
    {
        internal static DurationJson ToDurationJson(this TimeSpan timeSpan)
        {
            return DurationJson.OfMillis(timeSpan.Ticks / TimeSpan.TicksPerMillisecond);
        }
    }
}
