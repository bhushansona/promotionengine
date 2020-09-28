using System.Collections.Generic;

namespace PromotionApp.Model
{
    public class Cart
    {
        public int CartId { get; }

        public IEnumerable<CartItem> Items { get; set; }
    }

    public class CartItem
    {
        public string SkuId { get; set; }

        public int OrderedQuantity { get; set; }

        public bool IsPromotionApplied { get; set; }
    }
}
