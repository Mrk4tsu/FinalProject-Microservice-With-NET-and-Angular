using FN.ViewModel.Catalog.Categories;
using FN.ViewModel.Helper.API;

namespace FN.Application.Catalog.Categories
{
    public interface ICategoryService
    {
        Task<ApiResult<int>> Create(CategoryCreateUpdateRequest request);
        Task<ApiResult<bool>> Update(CategoryCreateUpdateRequest request, byte categoryId);
        Task<ApiResult<List<CategoryViewModel>>> List();
    }
}
