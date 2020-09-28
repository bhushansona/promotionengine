using PromotionApp.Model;
using System.Collections.Generic;

namespace PromotionApp.BusinessLogic
{
    public class AvailablePromotions
    {
        private readonly List<Promotion> promotions = new List<Promotion>();

        public List<Promotion> GetPromotions()
        {
            promotions.Add(new Promotion
            {
                PromotionId = 1,
                Skus = new Dictionary<string, int> { { "A", 3 } },
                OfferPrice = 130,
                Type = PromotionType.QuantityDiscount
            });

            promotions.Add(new Promotion
            {
                PromotionId = 2,
                Skus = new Dictionary<string, int> { { "B", 2 } },
                OfferPrice = 45,
                Type = PromotionType.QuantityDiscount
            });

            promotions.Add(new Promotion
            {
                PromotionId = 3,
                Skus = new Dictionary<string, int> { { "C", 1 }, { "D", 1 } },
                OfferPrice = 30,
                Type = PromotionType.ComboOffer
            });

            return promotions;
        }
    }
}
