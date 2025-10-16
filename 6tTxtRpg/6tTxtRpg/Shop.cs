using _6TxtRpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public static void ShopInput()
        {
            bool flag = true;
            bool flagShop = true;
            bool flagBuy = true;
            bool flagSell = true;

            List<Item> shopItemList = ShopPreset.shopItemList[TxtR.player.level - 1];

            while (flag) // 상점 선택지(구매,판매,떠나기)
            {
                Console.Clear();
                Console.WriteLine("1.구매");
                Console.WriteLine("2.판매");
                Console.WriteLine("3.강화");
                Console.WriteLine("0.떠나기");
                Console.WriteLine("원하는 행동의 번호를 입력해 주세요.");

                string inpuString = Console.ReadLine();
                int input = (int.TryParse(inpuString, out int value1)) ? value1 : -1; // 입력을 정수로 변환, 실패시 정수 -1 반환

                if (input == 0)
                {
                    // 상점 떠나기
                    flag = false;
                    Console.Clear();
                }
                else if (input == 1)
                {
                    Console.Clear();
                    //구매 메뉴 연결
                    while (flagBuy)
                    {                        
                        //PrintShop(TxtR.player.level - 1);   // 플레이어 레벨에 따라 상점 선정
                        PrintShop(0);   //레벨업 구현 전까지 임시 적용
                        Console.WriteLine("구매 할 아이템의 번호를 입력해주세요.");
                        Console.WriteLine("나가기는 0번");

                        inpuString = Console.ReadLine();
                        input = (int.TryParse(inpuString, out int value2)) ? value2 : -1; // 입력을 정수로 변환, 실패시 정수 -1 반환

                        if (input == 0)
                        {
                            // 구매 창 떠나기
                            flagBuy = false;
                        }
                        else if (input <= shopItemList.Count && input > 0)
                        {
                            //아이템 구매
                            Console.Clear();
                            shopItemList[input -1 ].BuyItem();
                                                
                        }
                        else
                        {
                            // 인벤토리 아이템 수를 넘어가는 수, 이외의 값들
                            Console.Clear();
                            Console.WriteLine("잘못된 입력입니다.");
                        }
                    }

                    //이후 인벤토리 출력갱신 필요
                }
                else if (input == 2)
                {
                    Console.Clear();
                    //판매 메뉴 연결
                    while (flagSell)
                    {                       
                        Inventory.PrintInventory();
                        Console.WriteLine("판매 할 아이템의 번호를 입력해주세요.");
                        Console.WriteLine("나가기는 0번");

                        inpuString = Console.ReadLine();
                        input = (int.TryParse(inpuString, out int value2)) ? value2 : -1; // 입력을 정수로 변환, 실패시 정수 -1 반환

                        if (input == 0)
                        {
                            // 판매 창 떠나기
                            flagSell = false;
                        }
                        else if (input <= Inventory.Inven.Count && input > 0)
                        {
                            //아이템 판매
                            Console.Clear();
                            Inventory.Inven[input - 1].SellItem();                         
                        }
                        else
                        {
                            // 인벤토리 아이템 수를 넘어가는 수, 이외의 값들
                            Console.Clear();
                            Console.WriteLine("잘못된 입력입니다.");
                        }
                    }
                }
                else if(input == 3)
                {
                    Console.Clear();
                    EnchantInput();
                }
                else
                {
                    // 이외의 값들
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
        }
        public static void EnchantItem(Item item)
        {
            Item reqItem = new Item("none",0, Status.None, 0, 0, ItemType.Etc, 0, "");
            int reqAmount = 0;
            switch (item.Enchant)
            {
                case 0:
                    reqItem = ItemPreset.dropItemList[0];
                    reqAmount = 3;
                    break;
                case 1:
                    reqItem = ItemPreset.dropItemList[1];
                    reqAmount = 2;
                    break;
                case 2:
                    reqItem = ItemPreset.dropItemList[2];
                    reqAmount = 1;
                    break;
            }
            Console.WriteLine($"강화에는 다음 아이템이 필요합니다. [{reqItem.Name} : {reqAmount}] ");

            //인벤토리에 강화소재 보유 시
            if(Inventory.Inven.Contains(reqItem) && Inventory.Inven.Find(t => t == reqItem).Amount >= reqAmount && item.Enchant < 3)
            {
                Console.WriteLine("강화 하시겠습니까?\n1.예\n2.아니오");
                string inputString = Console.ReadLine();

                if(inputString == "1")
                {
                    Console.Clear();
                    Console.WriteLine($"{item.Name} 강화 성공! 능력치 {item.EffectNum} -> {(int)(item.EffectNum * 1.2f)}");
                    item.EnchantItem();

                    for (int i = 0; i < reqAmount; i++)
                    {
                        Inventory.LoseItem(reqItem);
                    }
                    
                    
                }
                else
                {
                    //강화 창 종료
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("강화 불가.");
            }
        }

        public static void EnchantInput()
        {
            bool flag = true;
            while (flag)
            {               

                //강화 가능한 아이템 선별
                List<Item> echantableItemList = new List<Item>();
                foreach (Item item in Inventory.Inven)
                {
                    if(item.Type == ItemType.Head ||
                        item.Type == ItemType.Body ||
                        item.Type == ItemType.Weapon ||
                        item.Type == ItemType.ExtraWeapon)
                    {
                        if(item.Enchant < 3)
                        {
                            echantableItemList.Add(item);
                        }
                    }
                }

                for(int i = 0; i < echantableItemList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. [{echantableItemList[i].Name}] [+{echantableItemList[i].Enchant}]");
                }

                Console.WriteLine("강화 할 아이템의 번호를 입력해주세요.\n0.떠나기");

                string inpuString = Console.ReadLine();
                int input = (int.TryParse(inpuString, out int value)) ? value : -1; // 입력을 정수로 변환, 실패시 정수 -1 반환

                if (input == 0)
                {
                    // 강화선택 떠나기
                    flag = false;
                    break;
                }
                else if (input <= echantableItemList.Count && input > 0)
                {
                    //인벤토리의 아이템 강화
                    string usedItemName = Inventory.Inven[input - 1].Name;
                    Console.Clear();
                    EnchantItem(Inventory.Inven.Find(t => t == echantableItemList[input - 1]));
                    //이후 인벤토리 출력갱신 필요
                }
                else
                {
                    // 인벤토리 아이템 수를 넘어가는 수, 이외의 값들
                    Console.Clear();
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
