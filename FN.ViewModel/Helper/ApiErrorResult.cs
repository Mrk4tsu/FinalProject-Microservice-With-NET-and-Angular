namespace FN.ViewModel.Helper
{
    public class ApiErrorResult<T> : ApiResult<T>
    {
        public string[] ValidationErrors { get; set; } = Array.Empty<string>();
        public ApiErrorResult()
        {
        }
        public ApiErrorResult(string message)
        {
            Success = false;
            Message = message;
            ValidationErrors = Array.Empty<string>();
        }
        public ApiErrorResult(string[] validationErrors)
        {
            Success = false;
            ValidationErrors = validationErrors;
        }
    }
}

