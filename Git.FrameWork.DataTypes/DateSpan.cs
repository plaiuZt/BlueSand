namespace Git.Framework.DataTypes
{
    using Git.Framework.DataTypes.ExtensionMethods;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class DateSpan
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime <End>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime <Start>k__BackingField;

        public DateSpan(DateTime Start, DateTime End)
        {
            if (Start > End)
            {
                throw new ArgumentException(Start.ToString() + " is after " + End.ToString());
            }
            this.Start = Start;
            this.End = End;
        }

        public override bool Equals(object obj)
        {
            return ((obj is DateSpan) && (((DateSpan) obj) == this));
        }

        public override int GetHashCode()
        {
            return (this.End.GetHashCode() & this.Start.GetHashCode());
        }

        public DateSpan Intersection(DateSpan Span)
        {
            if (Span.IsNull())
            {
                return null;
            }
            if (!this.Overlap(Span))
            {
                return null;
            }
            DateTime start = (Span.Start > this.Start) ? Span.Start : this.Start;
            return new DateSpan(start, (Span.End < this.End) ? Span.End : this.End);
        }

        public static DateSpan operator +(DateSpan Span1, DateSpan Span2)
        {
            if (Span1.IsNull() && Span2.IsNull())
            {
                return null;
            }
            if (Span1.IsNull())
            {
                return new DateSpan(Span2.Start, Span2.End);
            }
            if (Span2.IsNull())
            {
                return new DateSpan(Span1.Start, Span1.End);
            }
            DateTime start = (Span1.Start < Span2.Start) ? Span1.Start : Span2.Start;
            return new DateSpan(start, (Span1.End > Span2.End) ? Span1.End : Span2.End);
        }

        public static bool operator ==(DateSpan Span1, DateSpan Span2)
        {
            if ((Span1 == null) && (Span2 == null))
            {
                return true;
            }
            if ((Span1 == null) || (Span2 == null))
            {
                return false;
            }
            return ((Span1.Start == Span2.Start) && (Span1.End == Span2.End));
        }

        public static bool operator !=(DateSpan Span1, DateSpan Span2)
        {
            return !(Span1 == Span2);
        }

        public bool Overlap(DateSpan Span)
        {
            return ((((this.Start >= Span.Start) && (this.Start < Span.End)) || ((this.End <= Span.End) && (this.End > Span.Start))) || ((this.Start <= Span.Start) && (this.End >= Span.End)));
        }

        public override string ToString()
        {
            return ("Start: " + this.Start.ToString() + " End: " + this.End.ToString());
        }

        public virtual DateTime End { get; protected set; }

        public virtual DateTime Start { get; protected set; }
    }
}

