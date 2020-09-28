using Interfaces;
using PromotionApp.BusinessLogic;
using Unity;

namespace Factory
{
    public class CartFactory
    {
        private readonly IUnityContainer container = new UnityContainer();

        public CartFactory()
        {
            container.RegisterType<ICartService, CartService>();
            container.RegisterType<IPromotionService, PromotionService>();
        }

        public ICartService CreateCart()
        {
            return container.Resolve<ICartService>();
        }

        public IPromotionService CreatePromotions()
        {
            return container.Resolve<IPromotionService>();
        }
    }
}
