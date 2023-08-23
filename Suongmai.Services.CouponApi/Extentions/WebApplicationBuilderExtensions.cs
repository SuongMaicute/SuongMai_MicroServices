using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Text;

namespace Suongmai.Services.CouponApi.Extentions
{
	public static class WebApplicationBuilderExtensions
	{
		public static WebApplicationBuilder AppAuthentication(this WebApplicationBuilder builder)
		{
			var secret = builder.Configuration.GetValue<string>("Apisettings:Secret");
			var Issuer = builder.Configuration.GetValue<string>("Apisettings:Issuer");
			var Audience = builder.Configuration.GetValue<string>("Apisettings:Audience");
			var key = Encoding.ASCII.GetBytes(secret);
			builder.Services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

			}).AddJwtBearer(x => {
				x.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = true,
					ValidIssuer = Issuer,
					ValidAudience = Audience,
					ValidateAudience = true,
				};
			});
			return builder;
		}
	}
}
