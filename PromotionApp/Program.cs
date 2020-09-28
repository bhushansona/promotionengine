using Interfaces;
using PromotionApp.Model;
using System;
using System.Collections.Generic;

namespace PromotionApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Add items to your Cart!!");
            Cart cart = new Cart();
            cart.Items = new List<CartItem>
            {
                new CartItem { SkuId = "A", OrderedQuantity = 3},
                new CartItem { SkuId = "B", OrderedQuantity = 5},
                new CartItem { SkuId = "C", OrderedQuantity = 1},
                new CartItem { SkuId = "D", OrderedQuantity = 1}
            };

            Factory.CartFactory factory = new Factory.CartFactory();
            ICartService process = factory.CreateCart();

            var finalOrder = process.CheckOut(cart);

            Console.WriteLine("Your final order is ");
            var total = 0; 
            foreach(var order in finalOrder)
            {
                Console.WriteLine(order.Key + " : " + order.Value);
                total += order.Value;
            }
            Console.WriteLine("Total : " + total);
            Console.ReadLine();
        }
    }
}
