using PromotionApp.Model;
using System.Collections.Generic;

namespace Interfaces
{
    public interface ICartService
    {
        Dictionary<string, int> CheckOut(Cart cart);
    }
}
