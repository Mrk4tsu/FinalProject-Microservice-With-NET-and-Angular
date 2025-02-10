namespace FN.ViewModel.Helper.API
{
    public class ApiSuccessResult<T> : ApiResult<T>
    {
        public ApiSuccessResult(T data)
        {
            Success = true;
            Data = data;
        }
        public ApiSuccessResult()
        {
            Success = true;
        }
    }
}
