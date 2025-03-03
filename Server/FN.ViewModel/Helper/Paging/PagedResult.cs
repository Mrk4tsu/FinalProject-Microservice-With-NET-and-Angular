namespace FN.ViewModel.Helper.Paging
{
    public class PagedResult<T> : PagedResultBase
    {
        public List<T> Items { get; set; } = new List<T>();
    }
}
