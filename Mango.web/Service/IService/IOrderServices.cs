﻿using Mango.web.Models;

namespace Mango.web.Service.IService
{
    public interface IOrderServices
    {
        Task<ResponseDto> CreateOrderAsync(CartDto cartDto);
       
    }
}