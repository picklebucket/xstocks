using Stocks.Ameritrade.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Ameritrade.Helpers
{
    static class ApiResponseHelper
    {
        public static ApiResponse<T> Success<T>(T content)
        {
            return new ApiResponse<T>()
            {
                Content = content,
                IsSuccess = true
            };
        }

        public static ApiResponse<T> Error<T>(string error)
        {
            return new ApiResponse<T>()
            {
                ErrorMessage = error
            };
        }
    }
}
