using FN.ViewModel.Catalog.Products;
using FN.ViewModel.Helper.API;
using FN.ViewModel.Helper.Paging;

namespace FN.Application.Catalog.Product
{
    public interface IProductManageService
    {
        Task<ApiResult<PagedResult<ProductViewModel>>> GetProducts();
    }
}
