using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Common.Models
{
    public class ServiceResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public T Content { get; set; }

        internal ServiceResponse()
        {

        }

        public static ServiceResponse<T> Success(T content)
        {
            return new ServiceResponse<T>()
            {
                Content = content,
                IsSuccess = true
            };
        }

        public static ServiceResponse<T> Error(string error)
        {
            return new ServiceResponse<T>()
            {
                ErrorMessage = error
            };
        }
    }
}
