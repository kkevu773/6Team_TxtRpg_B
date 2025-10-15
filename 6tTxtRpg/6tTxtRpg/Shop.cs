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
        public static void PrintShop(int index)
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

        public static void ShopInput(Character player)
        {
            bool flag = true;
            bool flagShop = true;
            bool flagBuy = true;
            bool flagSell = true;

            while (flagShop)
            {
                Console.WriteLine("사용 할 아이템의 번호를 입력해주세요.");

                string inpuString = Console.ReadLine();
                int input = (int.TryParse(inpuString, out int value)) ? value : -1; // 입력을 정수로 변환, 실패시 정수 -1 반환

                if (input == 0)
                {
                    // 인벤토리 떠나기
                    flag = false;
                    break;
                }
                else if (input <= Inventory.Inven.Count)
                {
                    //인벤토리의 아이템 사용
                    Inventory.Inven[input - 1].UseItem(player);
                    //이후 인벤토리 출력갱신 필요
                }
                else
                {
                    // 인벤토리 아이템 수를 넘어가는 수, 이외의 값들
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
            while (flag)
            {
                Console.WriteLine("사용 할 아이템의 번호를 입력해주세요.");

                string inpuString = Console.ReadLine();
                int input = (int.TryParse(inpuString, out int value)) ? value : -1; // 입력을 정수로 변환, 실패시 정수 -1 반환

                if (input == 0)
                {
                    // 인벤토리 떠나기
                    flag = false;
                    break;
                }
                else if (input <= Inventory.Inven.Count)
                {
                    //인벤토리의 아이템 사용
                    Inventory.Inven[input - 1].UseItem(player);
                    //이후 인벤토리 출력갱신 필요
                }
                else
                {
                    // 인벤토리 아이템 수를 넘어가는 수, 이외의 값들
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
        }
    }

    public class ShopPreset
    {
        public static List<List<Item>> shopItemList= new List<List<Item>>()
        {
            new List<Item>(){ItemPreset.itemList[0], ItemPreset.itemList[0], ItemPreset.itemList[0]}
        };
    }
}
