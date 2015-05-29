using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.ERP.Storage.Mongo
{
    public static class MongoTypeConvertor
    {
        private const int PRECISION_OFFSET = 1000000;

        public static long? ToLong(object number)
        {
            if (number == null)
                return null;

            if (!IsNumber(number))
                throw new ArgumentException("'number' argument is not a number type");

            if (number is sbyte)
                return Convert.ToInt64( (long)((sbyte)number)) * PRECISION_OFFSET;
            else if (number is byte)
                return Convert.ToInt64((long)((byte)number)) * PRECISION_OFFSET;
            else if (number is short)
                return Convert.ToInt64((long)((short)number)) * PRECISION_OFFSET;
            else if (number is ushort)
                return Convert.ToInt64((long)((ushort)number)) * PRECISION_OFFSET;
            else if (number is int)
                return Convert.ToInt64((long)((int)number)) * PRECISION_OFFSET;
            else if (number is uint)
                return Convert.ToInt64((long)((uint)number)) * PRECISION_OFFSET;
            else if (number is long)
                return Convert.ToInt64(((long)number)) * PRECISION_OFFSET;
            else if (number is ulong)
                return Convert.ToInt64(((ulong)number)) * PRECISION_OFFSET;
            else if (number is float)
                return Convert.ToInt64(((float)number)) * PRECISION_OFFSET;
            else if (number is double)
                return Convert.ToInt64(((double)number)) * PRECISION_OFFSET;
            else if (number is decimal)
                return Convert.ToInt64(((decimal)number)) * PRECISION_OFFSET;
            else
                throw new Exception("Object type is not supported");
        }

        public static decimal? LongToDecimal(object longNumber)
        {
            if (longNumber == null)
                return null;

            if (!(longNumber is long))
                throw new Exception("longNumber argument should be long type");

            return Convert.ToDecimal(longNumber) / 1000000;
        }

        public static bool IsNumber(object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }
    }
}
