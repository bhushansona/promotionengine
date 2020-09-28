using NUnit.Framework;
using PromotionApp.BusinessLogic;
using Moq;
using Interfaces;
using PromotionApp.Model;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicUnitTests
{
    [TestFixture]
    public class ProcessCartUnitTests
    {
        private CartService _cartService;
        private Mock<IPromotionService> promotionService = new Mock<IPromotionService>();

        [SetUp]
        public void SetUp()
        {
            promotionService = new Mock<IPromotionService>();
        }

        [Test]
        public void ShouldProcessDiscountForAllItems()
        {
            // Arrange
            var skuId = "A";
            SetupSkuPromotion(skuId);

            // Act
            Cart cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem { SkuId = "A", OrderedQuantity = 3}
                }
            };
            var response = _cartService.CheckOut(cart);

            // Assert
            Assert.AreEqual(130, response["A"]);
        }

        [Test]
        public void ShouldNotProcessDiscountForExtraItems()
        {
            // Arrange
            var skuId = "A";
            SetupSkuPromotion(skuId);

            // Act
            Cart cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem { SkuId = "A", OrderedQuantity = 8}
                }
            };
            var response = _cartService.CheckOut(cart);

            // Assert
            Assert.AreEqual(360, response["A"]);
        }

        [Test]
        public void ShouldProcessDiscountForAllComboItems()
        {
            // Arrange
            var skuId = "C";
            SetupSkuPromotion(skuId);

            // Act
            Cart cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem { SkuId = "C", OrderedQuantity = 1},
                    new CartItem { SkuId = "D", OrderedQuantity = 1}
                }
            };
            var response = _cartService.CheckOut(cart);

            // Assert
            Assert.AreEqual(30, response[skuId]);
        }

        [Test]
        public void ShouldNotProcessComboDiscountIfPeerIsMissing()
        {
            // Arrange
            var skuId = "C";
            SetupSkuPromotion(skuId);
            _cartService = new CartService(promotionService.Object);

            // Act
            Cart cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem { SkuId = "C", OrderedQuantity = 1}
                }
            };
            var response = _cartService.CheckOut(cart);

            // Assert
            Assert.AreEqual(20, response[skuId]);
        }

        private void SetupSkuPromotion(string skuId)
        {
            Promotion promotion = new AvailablePromotions().GetPromotions()
                            .FirstOrDefault(p => p.Skus.Keys.Contains(skuId));
            promotionService.Setup(s => s.FindPromotions(It.IsAny<CartItem>())).Returns(promotion);
            _cartService = new CartService(promotionService.Object);
        }
    }
}
