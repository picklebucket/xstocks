using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Common.Models
{
    public class ServiceResponse<T> : ServiceResponse
    {
        public T Content { get; set; }

        private ServiceResponse(bool isSuccess, T content = default, string errorMessage = null)
            : base(isSuccess, errorMessage)
        {
            Content = content;
        }

        public static ServiceResponse<T> Success(T content)
        {
            return new ServiceResponse<T>(true, content);
        }

        public new static ServiceResponse<T> Error(string error)
        {
            return new ServiceResponse<T>(false, default, error);
        }
    }

    public class ServiceResponse
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

        protected ServiceResponse(bool isSuccess, string errorMessage = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static ServiceResponse Success()
        {
            return new ServiceResponse(true);
        }

        public static ServiceResponse Error(string error)
        {
            return new ServiceResponse(false, error);
        }
    }
}
