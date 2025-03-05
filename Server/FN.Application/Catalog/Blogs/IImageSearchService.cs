namespace FN.Application.Catalog.Blogs
{
    public interface IImageSearchService
    {
        Task<string> GetGameThumbnailAsync(string query);
    }
}
