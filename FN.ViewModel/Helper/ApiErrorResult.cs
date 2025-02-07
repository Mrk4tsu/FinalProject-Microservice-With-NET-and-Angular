namespace FN.ViewModel.Helper
{
    public class ApiErrorResult<T> : ApiResult<T>
    {
        public string[] ValidationErrors { get; set; }
        public ApiErrorResult()
        {
        }
        public ApiErrorResult(string message)
        {
            Success = false;
            Message = message;
        }
        public ApiErrorResult(string[] validationErrors)
        {
            Success = false;
            ValidationErrors = validationErrors;
        }
    }
}
