
using AbmProductos.Services.Interfaces;
using AbmProductos.Entities;
using AbmProductos.Models.DTOs.Requests;
using AbmProductos.Repositories.Interfaces;

using AbmProductos.Models.DTOs.Reponses;

namespace AbmProductos.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }
    public List<ProductForReadDto> GetAllProducts()
    {
        var products = _repository.GetAllProducts();
        var result = new List<ProductForReadDto>();

        foreach (var product in products)
        {
            result.Add(new ProductForReadDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            });
        }

        return result;
    }
    public ProductForReadDto? GetProductById(int id)
    {
        var product = _repository.GetProductById(id);

        if (product == null)
            return null;

        return new ProductForReadDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }
    public ProductForReadDto CreateProduct(ProductForCreateDto dto)
    {
        if (_repository.GetAllProducts().Any(product =>
             string.Equals(product.Name, dto.Name,
                 StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException(
                "Ya existe un producto con ese nombre.");
        }
        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price
        };

        _repository.AddProduct(product);

        return new ProductForReadDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }
    public void UpdateProduct(int id, ProductForUpdateDto dto)
    {
        var product = _repository.GetProductById(id);

        if (product == null)
            return;
        if (_repository.GetAllProducts().Any(other =>
                other.Id != id &&
                string.Equals(other.Name, dto.Name,
                    StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException(
                "Ya existe un producto con ese nombre.");
        }
        product.Name = dto.Name;
        product.Price = dto.Price;

        _repository.UpdateProduct(product);
    }
    public void DeleteProduct(int id)
    {
        var product = _repository.GetProductById(id);

        if (product == null)
            return;

        _repository.DeleteProduct(product);
    }
    public List<ProductForReadDto> SearchProductsByName(string name)
    {
        var products = _repository.SearchProductsByName(name);

        return products.Select(product => new ProductForReadDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        }).ToList();
    }
    public ProductStatsDto GetStats()
    {
        var products = _repository.GetAllProducts();

        if (products.Count == 0)
        {
            return new ProductStatsDto
            {
                Total = 0,
                AveragePrice = 0,
                MostExpensiveName = string.Empty
            };
        }

        return new ProductStatsDto
        {
            Total = products.Count,
            AveragePrice = products.Average(product => product.Price),
            MostExpensiveName = products.OrderByDescending(product => product.Price).First().Name
        };
    }
}