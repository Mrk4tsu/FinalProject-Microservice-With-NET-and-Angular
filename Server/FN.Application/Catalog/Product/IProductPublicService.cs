using FN.ViewModel.Catalog.Products;
using FN.ViewModel.Helper.API;
using FN.ViewModel.Helper.Paging;

namespace FN.Application.Catalog.Product
{
    public interface IProductPublicService
    {
        Task<ApiResult<PagedResult<ProductViewModel>>> GetProducts(ProductPagingRequest request);
        Task<ApiResult<ProductDetailViewModel>> GetProduct(int productId);
    }
}
