using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Ameritrade.Models
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public T Content { get; set; }

        public static ApiResponse<T> Success(T content)
        {
            return new ApiResponse<T>()
            {
                Content = content,
                IsSuccess = true
            };
        }

        public static ApiResponse<T> Error(string error)
        {
            return new ApiResponse<T>()
            {
                ErrorMessage = error
            };
        }
    }
}
