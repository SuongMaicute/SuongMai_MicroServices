using Microsoft.AspNetCore.Authentication;

namespace Suongmai.Services.OrderAPI.Util
{
    public class SuongMaiAuthenticationHandler: DelegatingHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public SuongMaiAuthenticationHandler( IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync (HttpRequestMessage request, CancellationToken cancellationToken) 
        {
            var token = await _contextAccessor.HttpContext.GetTokenAsync("access_token");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await base.SendAsync (request, cancellationToken);
        }


    }
}
