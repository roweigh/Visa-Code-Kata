using Xunit;
using Moq;
using CheckoutApi.Service;
using CheckoutApi.Repositories;
using CheckoutApi.Models;
using CheckoutApi.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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

    [Fact]
    public async Task EmptyBasket()
    {
        var result = await _service.CheckoutProducts("");
        Assert.Equal(0, result);
    }
}
