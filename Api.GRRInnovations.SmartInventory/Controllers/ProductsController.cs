using Api.GRRInnovations.SmartInventory.Domain.Entities;
using Api.GRRInnovations.SmartInventory.Domain.Wrappers.In;
using Api.GRRInnovations.SmartInventory.Domain.Wrappers.Out;
using Api.GRRInnovations.SmartInventory.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.GRRInnovations.SmartInventory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WrapperInProduct<ProductModel> dto)
        {
            var wrapperIn = await dto.Result();
            var model = await _productService.CreateAsync(wrapperIn);

            var response = await WrapperOutProduct.From(model).ConfigureAwait(false);
            return new OkObjectResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? name,
            [FromQuery] Guid? categoryId,
            [FromQuery] Guid? supplierId)
        {
            var products = await _productService.GetAllAsync(new Interfaces.Repositories.ProductOptions
            {
                FilterCategoriesUids = categoryId is not null ? new List<Guid> { categoryId.Value } : null,
                FilterNames = name is not null ? new List<string> { name } : null,
                FilterSupplierUids = supplierId is not null ? new List<Guid> { supplierId.Value } : null
            });

            var response = await WrapperOutProduct.From(products).ConfigureAwait(false);
            return new OkObjectResult(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productService.GetByIdAsync(id);

            var response = await WrapperOutProduct.From(product).ConfigureAwait(false);
            return new OkObjectResult(response);
        }
    }
}
