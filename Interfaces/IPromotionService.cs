using PromotionApp.Model;
using System.Collections.Generic;

namespace Interfaces
{
    public interface IPromotionService
    {
        Promotion FindPromotions(CartItem item);

        List<Promotion> GetAvailablePromotions();

        bool AddNewPromotion(Promotion promotion);
    }
}
