using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace F1WebAPI.Models
{
    public class ApiResponse
    {
        public ApiResponse()
        {

        }

        public bool Success { get; set; }
        public string Data { get; set; }
        public string ErrorMessage { get; set; }

    }
}