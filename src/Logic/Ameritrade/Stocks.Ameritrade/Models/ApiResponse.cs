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
    }
}
