﻿namespace UNDEFINED.Areas.Admin.Models
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }


        public static OperationResult NotFound(string msg = "Item Not Found")
        {
            return new OperationResult
            {
                Success = false,
                Message = msg
            };
        }

        public static OperationResult Succeeded(string msg = "Operation completed Successfully!")
        {
            return new OperationResult
            {
                Success = true,
                Message = msg
            };
        }

    }
}
