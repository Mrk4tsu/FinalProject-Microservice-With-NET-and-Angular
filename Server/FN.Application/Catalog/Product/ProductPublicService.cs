using AutoMapper;
using AutoMapper.QueryableExtensions;
using FN.DataAccess;
using FN.ViewModel.Catalog.Products;
using FN.ViewModel.Helper.API;
using FN.ViewModel.Helper.Paging;
using Microsoft.EntityFrameworkCore;

namespace FN.Application.Catalog.Product
{
    public class ProductPublicService : IProductPublicService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        public ProductPublicService(AppDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }
        public async Task<ApiResult<PagedResult<ProductViewModel>>> GetProducts(ProductPagingRequest request)
        {
           
            return new ApiSuccessResult<PagedResult<ProductViewModel>>();
        }
    }
}
