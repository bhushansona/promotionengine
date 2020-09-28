using PromotionApp.Model;
using System.Collections.Generic;
using System.Linq;

namespace PromotionApp.BusinessLogic
{
    public class ItemMaster
    {
        private readonly List<Item> items = new List<Item>();

        public List<Item> GetItems()
        {
            items.Add(new Item
            {
                SkuId = "A",
                UnitPrice = 50
            });

            items.Add(new Item
            {
                SkuId = "B",
                UnitPrice = 30
            });

            items.Add(new Item
            {
                SkuId = "C",
                UnitPrice = 20
            });

            items.Add(new Item
            {
                SkuId = "D",
                UnitPrice = 15
            });

            return items;
        }

        public int GetUnitPrice(string skuId)
        {
            var item = (items.Count <= 0 ? GetItems() : items).FirstOrDefault(i => i.SkuId == skuId);
            return item == null ? 0 : item.UnitPrice;
        }
    }
}
