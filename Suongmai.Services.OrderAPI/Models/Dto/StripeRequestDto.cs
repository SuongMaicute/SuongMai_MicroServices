﻿
namespace Suongmai.Services.OrderAPI.Models.Dto
{
    public class StripeRequestDto
    {
        public string StripeSessionUrl { get; set; }
        public string StripeSessionId { get; set; }
        public string ApprovedUrl { get; set; }
        public string CancelUrl { get; private set; }
        public OrderHeaderDto OrderHeader { get; set; }
    }
}
