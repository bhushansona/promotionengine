using System.Collections.Generic;

namespace PromotionApp.Model
{
    public class Promotion
    {
        public int PromotionId { get; set; }
        public PromotionType Type { get; set; }

        /// <summary>
        /// Collection of Sku and Quantity participating in promotion.
        /// </summary>
        public Dictionary<string, int> Skus { get; set; }
        
        public int OfferPrice { get; set; }
    }

    public enum PromotionType
    {
        ComboOffer,
        QuantityDiscount,
        SeasonOffer,
        None
    }
}
