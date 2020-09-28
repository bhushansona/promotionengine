using Interfaces;
using PromotionApp.Model;
using System.Linq;
using System.Collections.Generic;
using System;

namespace PromotionApp.BusinessLogic
{
    public class CartService : ICartService
    {
        private readonly IPromotionService promotionService;
        private readonly ItemMaster master = new ItemMaster();

        public CartService(IPromotionService service)
        {
            this.promotionService = service;
        }

        public Dictionary<string, int> CheckOut(Cart cart)
        {
            var itemPromotions = new List<Promotion>();
            foreach (CartItem item in cart.Items)
            {
                var promo = promotionService.FindPromotions(item);
                if (promo != null && !itemPromotions.Any(p => p.PromotionId == promo.PromotionId))
                {
                    itemPromotions.Add(promo);
                }
            }

            Dictionary<string, int> finalOrder = new Dictionary<string, int>();
            foreach (CartItem item in cart.Items)
            {
                Promotion promo = itemPromotions.FirstOrDefault(p => p.Skus.Keys.Contains(item.SkuId));

                if (promo == null)
                {
                    finalOrder.Add(item.SkuId, item.OrderedQuantity * master.GetUnitPrice(item.SkuId));
                    continue;
                }
                int promoPrice = GetDiscountedPrice(item, promo, cart);
                finalOrder.Add(item.SkuId, promoPrice);
            }

            return finalOrder;
        }

        private int GetDiscountedPrice(CartItem item, Promotion promo, Cart cart)
        {
            var promoQuantity = promo.Skus[item.SkuId];
            if (promo.Skus.Count > 1)
            {
                return CalculateComboPrice(promo, cart);
            }
            var promoPrice = CalculatePromoPrice(promoQuantity, promo.OfferPrice, item);
            return promoPrice;
        }

        private int CalculateComboPrice(Promotion promo, Cart cart)
        {
            Dictionary<string, int> promoSkus = promo.Skus;
            int offerPrice = promo.OfferPrice;
            Dictionary<string, int> orderedSkus = GetOrderedCombos(cart, promoSkus);
            if (orderedSkus.Count <= 0)
            {
                return 0;
            }

            // Not all items in promotion available in cart. Do not apply promotion offer price.
            if (promoSkus.Count > orderedSkus.Count)
            {
                return ApplyUnitPriceInCombos(orderedSkus);
            }

            // Apply promotion price to applicable combos
            int comboPrice = ApplyComboPromotion(promoSkus, offerPrice, orderedSkus);

            // Apply unit price to remaining items without promotion
            int remainingPrice = ApplyUnitPriceInCombos(orderedSkus);

            // Mark cart item with applied promotion
            cart.Items.Where(p => !p.IsPromotionApplied).ToList()
                .ForEach(i =>
                {
                    if (orderedSkus.Keys.Contains(i.SkuId))
                    {
                        i.IsPromotionApplied = true;
                    }
                });

            return comboPrice + remainingPrice;
        }

        private int ApplyUnitPriceInCombos(Dictionary<string, int> orderedSkus)
        {
            var remainingPrice = 0;
            foreach (var sku in orderedSkus)
            {
                remainingPrice += sku.Value * master.GetUnitPrice(sku.Key);
            }

            return remainingPrice;
        }

        private static int ApplyComboPromotion(Dictionary<string, int> promoSkus, int offerPrice, Dictionary<string, int> orderedSkus)
        {
            var offerCount = 0;
            var comboPrice = 0;
            
            while (!orderedSkus.Values.Any(qty => qty == 0))
            {
                foreach (var sku in promoSkus)
                {
                    orderedSkus[sku.Key] -= sku.Value;
                }
                offerCount += 1;
            }
            comboPrice += offerPrice * offerCount;
            return comboPrice;
        }

        private static Dictionary<string, int> GetOrderedCombos(Cart cart, Dictionary<string, int> skus)
        {
            Dictionary<string, int> orderedSkus = new Dictionary<string, int>();
            foreach (var i in cart.Items.Where(p => !p.IsPromotionApplied))
            {
                if (skus.Keys.Contains(i.SkuId))
                {
                    orderedSkus.Add(i.SkuId, i.OrderedQuantity);
                }
            }

            return orderedSkus;
        }

        private int CalculatePromoPrice(int promoQuantity, int offerPrice, CartItem item)
        {
            var applicableQty = Convert.ToInt32(item.OrderedQuantity / promoQuantity) * promoQuantity;
            var applicablePrice = Convert.ToInt32(item.OrderedQuantity / promoQuantity) * offerPrice;
            var remainingQty = item.OrderedQuantity - applicableQty;
            var remainingPrice = remainingQty * master.GetUnitPrice(item.SkuId);
            item.IsPromotionApplied = true;
            return applicablePrice + remainingPrice;
        }
    }
}
