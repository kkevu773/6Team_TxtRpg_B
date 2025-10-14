using _6TxtRpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6tTxtRpg
{
    public class Shop
    {
        static void PrintShop(int index)
        {
            List<Item> shopItemList = ShopPreset.shopItemList[index];
            //상점 출력
            Console.WriteLine("================= 상점 ================");

            for (int i = 0; i < shopItemList.Count; i++)
            {
                Item itemShop = shopItemList[i];
                Console.WriteLine($"{i + 1}.[{itemShop.Name}] : [{itemShop.Price}]G");
            }
        }
    }

    public class ShopPreset
    {
        public static List<List<Item>> shopItemList= new List<List<Item>>()
        {
            new List<Item>(){ItemPreset.itemList[0], ItemPreset.itemList[0], ItemPreset.itemList[0] }
        };
    }
}
