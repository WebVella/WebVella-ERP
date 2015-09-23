using System;

namespace WebVella.ERP.Api.Models
{
    public enum QueryType
    {
        EQ,
        NOT,
        LT,
        LTE,
        GT,
        GTE,
        AND,
        OR,
        CONTAINS,
        STARTSWITH,
        REGEX
    }
}