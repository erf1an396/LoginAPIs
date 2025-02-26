using static System.Runtime.InteropServices.JavaScript.JSType;

namespace loginAPP.ViewModel
{
    public class ResultModel
    {

        public bool Success { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }

        public ResultModel(bool success, string message, object data)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}

    
