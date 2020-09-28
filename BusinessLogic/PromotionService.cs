using System.Collections.Generic;
using Interfaces;
using PromotionApp.Model;
using System.Linq;

namespace PromotionApp.BusinessLogic
{
    public class PromotionService : IPromotionService
    {
        private readonly List<Promotion> promotions;

        public PromotionService()
        {
            AvailablePromotions availablePromotions = new AvailablePromotions();
            promotions = availablePromotions.GetPromotions();
        }

        public bool AddNewPromotion(Promotion promotion)
        {
            promotions.Add(promotion);
            return true;
        }

        public Promotion FindPromotions(CartItem item)
        {
            var itemPromotions = promotions.FirstOrDefault(p => p.Skus.Keys.Contains(item.SkuId) && 
                                                        p.Skus.Values.Any(s => s <= item.OrderedQuantity));
            return itemPromotions;
        }

        public List<Promotion> GetAvailablePromotions()
        {
            return promotions;
        }
    }
}
