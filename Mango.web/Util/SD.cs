﻿using System.Formats.Asn1;

namespace Mango.web.Util
{
    public class SD
    {
        public static string CouponAPIBase { get;set; }
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
