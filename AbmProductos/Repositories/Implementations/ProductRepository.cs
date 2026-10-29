using AbmProductos.Data;
using AbmProductos.Entities;
using AbmProductos.Repositories.Interfaces;

namespace AbmProductos.Repositories.Implementations;

public class ProductRepository : IProductRepository
{
    private readonly AbmProductosContext _context;

    public ProductRepository(AbmProductosContext context)
    {
        _context = context;
    }

    public List<Product> GetAllProducts()
    {
        return _context.Products.ToList();
    }

    public Product? GetProductById(int id)
    {
        return _context.Products.FirstOrDefault(p => p.Id == id);
    }

    public void AddProduct(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public void UpdateProduct(Product product)
    {
        var existingProduct = GetProductById(product.Id);

        if (existingProduct == null) return;

        existingProduct.Name = product.Name;
        existingProduct.Price = product.Price;

        _context.SaveChanges();
    }

    public void DeleteProduct(Product product)
    {
        var existingProduct = GetProductById(product.Id);

        if (existingProduct == null) return;

        _context.Products.Remove(existingProduct);
        _context.SaveChanges();
    }

    public List<Product> SearchProductsByName(string name)
    {
        return GetAllProducts()
            .Where(product => product.Name.Contains(
                name, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}    