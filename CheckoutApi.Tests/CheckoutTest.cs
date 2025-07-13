using Xunit;
using Moq;
using CheckoutApi.Service;
using CheckoutApi.Repositories;
using CheckoutApi.Models;
namespace CheckoutApi.Tests;

public class CheckoutTest
{
    private readonly Mock<IProductRepository> _mockRepo;
    private readonly ProductService _service;
    public CheckoutTest()
    {
        _mockRepo = new Mock<IProductRepository>();
        _service = new ProductService(_mockRepo.Object);
    }

    // Helper function to generate prices indicated by Kata09
    private List<Product> GetTestProducts()
    {
        return new List<Product>
        {
            new Product {
                Id = 1,
                ProductName = "A",
                UnitPrice = 50,
                DiscountQuantity = 3,
                DiscountPrice = 130
            },
            new Product {
                Id = 2,
                ProductName = "B",
                UnitPrice = 30,
                DiscountQuantity = 2,
                DiscountPrice = 45
            },
            new Product {
                Id = 3,
                ProductName = "C",
                UnitPrice = 20,
            },
            new Product {
                Id = 4,
                ProductName = "D",
                UnitPrice = 15,
            }
        };
    }

    // Helper function to call CheckoutProducts function
    private async Task<double> Price(string basket)
    {
        return await _service.CheckoutProducts(basket);
    }

    [Fact]
    public async Task TestTotals()
    {
        _mockRepo.Setup(r => r.GetAll()).ReturnsAsync(GetTestProducts());

        Assert.Equal(0, await Price(""));
        Assert.Equal(50, await Price("A"));
        Assert.Equal(80, await Price("AB"));
        Assert.Equal(115, await Price("CDBA"));

        Assert.Equal(100, await Price("AA"));
        Assert.Equal(130, await Price("AAA"));
        Assert.Equal(180, await Price("AAAA"));
        Assert.Equal(230, await Price("AAAAA"));
        Assert.Equal(260, await Price("AAAAAA"));

        Assert.Equal(160, await Price("AAAB"));
        Assert.Equal(175, await Price("AAABB"));
        Assert.Equal(190, await Price("AAABBD"));
        Assert.Equal(190, await Price("DABABA"));
    }

    [Fact]
    public async Task TestIncremental()
    {
        _mockRepo.Setup(r => r.GetAll()).ReturnsAsync(GetTestProducts());
        var scanned = "";

        Assert.Equal(0 , await Price(scanned));
        scanned += "A"; Assert.Equal(50, await Price(scanned));
        scanned += "B"; Assert.Equal(80, await Price(scanned));
        scanned += "A"; Assert.Equal(130, await Price(scanned));
        scanned += "A"; Assert.Equal(160, await Price(scanned));
        scanned += "B"; Assert.Equal(175, await Price(scanned));
    }
}
