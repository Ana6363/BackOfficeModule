﻿using BackOffice.Domain.Shared;

namespace BackOffice.Domain.Appointement
{
    public class AppointementId : EntityId
    {
        public AppointementId(Guid id) : base(id) { }

        public override string AsString()
        {
            return ObjValue.ToString();
        }

        protected override object CreateFromString(string text)
        {
            return new AppointementId(Guid.Parse(text));
        }

        public override bool Equals(object obj)
        {
            if (obj is AppointementId other)
            {
                return Value.Equals(other.Value);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
