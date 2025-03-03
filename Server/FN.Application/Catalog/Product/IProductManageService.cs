using FN.ViewModel.Catalog.Products;
using FN.ViewModel.Helper.API;
using FN.ViewModel.Helper.Paging;

namespace FN.Application.Catalog.Product
{
    public interface IProductManageService
    {
        Task<ApiResult<PagedResult<ProductViewModel>>> GetProducts(ProductPagingRequest request, int userId);
        Task<ApiResult<PagedResult<ProductViewModel>>> TrashProducts(ProductPagingRequest request, int userId);
        Task<ApiResult<int>> Create(CreateProductRequest request, int userId);
        Task<ApiResult<bool>> Update(ItemUpdateRequest request, int itemId, int userId);
        Task<ApiResult<bool>> UpdatePrice(int productId, decimal newPrice);
    }
}
