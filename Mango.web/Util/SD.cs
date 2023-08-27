using System.Formats.Asn1;

namespace Mango.web.Util
{
    public class SD
    {
        public static string CouponAPIBase { get;set; }
        public static string ProductAPIBase { get;set; }

		public static string AuthAPIBase { get; set; }
        public static string ShoppingCartAPI { get; set; }
        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";
        public const string TokenCookie = "JwtToken";

        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
