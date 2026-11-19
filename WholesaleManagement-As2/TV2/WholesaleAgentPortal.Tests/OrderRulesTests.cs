using Xunit;
using WholesaleAgentPortal.Services;

namespace WholesaleAgentPortal.Tests
{
    public class OrderRulesTests
    {
        // Test 1: Calculate order total with multiple items
        [Fact]
        public void CalculateOrderTotal_MultipleItems_ReturnsCorrectTotal()
        {
            // Arrange
            var submitted = new[]
            {
                new OrderLineRequest(1, 2),
                new OrderLineRequest(2, 3)
            };

            var inventory = new[]
            {
                new StockInformation(1, "Item A", 10, 10000m),
                new StockInformation(2, "Item B", 10, 15000m)
            };

            // Act
            var result = OrderRules.Validate(submitted, inventory);

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal(65000m, result.Total);
        }

        
        [Fact]
        public void PlaceOrder_InsufficientStock_RejectsOrder()
        {
            // Arrange
            var submitted = new[]
            {
                new OrderLineRequest(1, 5)
            };

            var inventory = new[]
            {
                new StockInformation(1, "Item A", 2, 10000m)
            };

            // Act
            var result = OrderRules.Validate(submitted, inventory);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Not enough stock", result.Error);
            Assert.Empty(result.Lines);
        }
        [Fact]
        public void PlaceOrder_DuplicateItems_ValidatesTotalQuantity()
        {
            // Arrange
            var submitted = new[]
            {
                new OrderLineRequest(1, 3),
                new OrderLineRequest(1, 3)
            };

            var inventory = new[]
            {
                new StockInformation(1, "Item A", 5, 10000m)
            };

            // Act
            var result = OrderRules.Validate(submitted, inventory);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Not enough stock", result.Error);
            Assert.Empty(result.Lines);
        }
        [Fact]
        public void PlaceOrder_DuplicateItems_CombinesQuantities()
        {
            // Arrange
            var submitted = new[]
            {
                new OrderLineRequest(1, 2),
                new OrderLineRequest(1, 3)
            };

            var inventory = new[]
            {
                new StockInformation(1, "Item A", 5, 10000m)
            };

            // Act
            var result = OrderRules.Validate(submitted, inventory);

            // Assert
            Assert.True(result.IsValid);
            Assert.Single(result.Lines);
            Assert.Equal(5, result.Lines[0].Quantity);
            Assert.Equal(50000m, result.Total);
        }
        // Test 5: Đơn hàng không có sản phẩm phải bị từ chối.
        [Fact]
        public void Validate_EmptyOrder_RejectsOrder()
        {
            // Arrange: Không gửi dòng sản phẩm nào.
            var submitted = Array.Empty<OrderLineRequest>();

            var inventory = new[]
            {
                new StockInformation(1, "Item A", 10, 10000m)
            };

            // Act: Gọi logic kiểm tra đơn hàng.
            var result = OrderRules.Validate(submitted, inventory);

            // Assert: Đơn không hợp lệ và không có dòng hàng được chuẩn bị.
            Assert.False(result.IsValid);
            Assert.Contains("between 1 and 20 rows", result.Error);
            Assert.Empty(result.Lines);
        }

        // Test 6: Số lượng bằng 0 phải bị từ chối.
        [Fact]
        public void Validate_ZeroQuantity_RejectsOrder()
        {
            // Arrange
            var submitted = new[]
            {
                new OrderLineRequest(1, 0)
            };

            var inventory = new[]
            {
                new StockInformation(1, "Item A", 10, 10000m)
            };

            // Act
            var result = OrderRules.Validate(submitted, inventory);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("quantity between 1 and 5,000", result.Error);
            Assert.Empty(result.Lines);
        }

        // Test 7: Số lượng âm phải bị từ chối.
        [Fact]
        public void Validate_NegativeQuantity_RejectsOrder()
        {
            // Arrange
            var submitted = new[]
            {
                new OrderLineRequest(1, -2)
            };

            var inventory = new[]
            {
                new StockInformation(1, "Item A", 10, 10000m)
            };

            // Act
            var result = OrderRules.Validate(submitted, inventory);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("quantity between 1 and 5,000", result.Error);
            Assert.Empty(result.Lines);
        }

        // Test 8: Sản phẩm không còn trong danh mục phải bị từ chối.
        [Fact]
        public void Validate_ItemNotFound_RejectsOrder()
        {
            // Arrange: Đặt sản phẩm 99, nhưng danh mục chỉ có sản phẩm 1.
            var submitted = new[]
            {
                new OrderLineRequest(99, 1)
            };

            var inventory = new[]
            {
                new StockInformation(1, "Item A", 10, 10000m)
            };

            // Act
            var result = OrderRules.Validate(submitted, inventory);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("no longer available", result.Error);
            Assert.Empty(result.Lines);
        }
    }
}


