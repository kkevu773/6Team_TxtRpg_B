using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6TxtRpg
{
    public enum ItemType { Head, Body, Weapon, ExtraWeapon, HpPotion, MpPotion, Etc }
    public enum Status { Hp, Mp, Exp, Level, Atk, Def, BonusHp, BonusMp, BonusAtk, BonusDef , None}

    public class Item
    {
        public string Name { get; private set; }            //아이템 이름
        public Status EffectStatus { get; private set; }    //아이템 효과 대상 스테이터스
        public int EffectNum { get; private set; }          //아이템 효과 적용 수치
        public int Amount { get; set; }                     //보유중인 아이템 수
        public ItemType Type { get; private set; }          //아이템 타입
        public int Price { get; private set; }              // 아이템 가격
        public bool IsUsing { get; set; }                   //아이템(장비) 장착 여부
        public string ItemScript { get; private set; }      //아이템 스크립트

        //생성
        public Item(string name, Status effectStatus, int effectNum, int amount, ItemType type, int price, string itemScript)
        {
            Name = name;
            EffectStatus = effectStatus;
            EffectNum = effectNum;
            Amount = amount;
            Type = type;
            Price = price;
            IsUsing = false;
            ItemScript = itemScript;
        }

        //아이템 사용
        public void UseItem()
        {
            Item? temp = null;

            Console.WriteLine($"{Name} 사용");

            switch (Type)   //아이템 타입에 따라 구별
            {
                case ItemType.Head:
                    if (Inventory.equipments[ItemType.Head] == null)
                    {
                        Inventory.equipments[ItemType.Head] = this;
                    }
                    else
                    {
                        temp = Inventory.equipments[ItemType.Head]; //이전에 장착된 장비
                    }
                    break;

                case ItemType.Body:
                    if (Inventory.equipments[ItemType.Body] == null)
                    {
                        Inventory.equipments[ItemType.Body] = this;
                    }
                    else
                    {
                        temp = Inventory.equipments[ItemType.Body];
                    }
                    break;

                case ItemType.Weapon:
                    if (Inventory.equipments[ItemType.Weapon] == null)
                    {
                        Inventory.equipments[ItemType.Weapon] = this;
                    }
                    else
                    {
                        temp = Inventory.equipments[ItemType.Weapon];
                    }
                    break;

                case ItemType.ExtraWeapon:
                    if (Inventory.equipments[ItemType.ExtraWeapon] == null)
                    {
                        Inventory.equipments[ItemType.ExtraWeapon] = this;
                    }
                    else
                    {
                        temp = Inventory.equipments[ItemType.ExtraWeapon];
                    }
                    break;

                case ItemType.HpPotion:
                    AddAmount(-1);
                    break;

                case ItemType.MpPotion:
                    AddAmount(-1);
                    break;
            }


            //아이템 수치 적용
            switch (EffectStatus)
            {
                case Status.Hp:
                    TxtR.player.hp += EffectNum;
                    if(TxtR.player.hp > TxtR.player.maxHp) TxtR.player.hp = TxtR.player.maxHp;
                    break;
                case Status.Mp:
                    TxtR.player.mp += EffectNum;
                    if (TxtR.player.hp > TxtR.player.maxMp) TxtR.player.mp = TxtR.player.maxMp;
                    break;
                case Status.Exp:
                    // 여기에 케릭터 경험치 획득 메소드 연결
                    break;
                case Status.Level:
                    // 사용 미정
                    break;
                case Status.Atk:
                    // 사용 미정
                    break;
                case Status.Def:
                    // 사용 미정
                    break;
                case Status.BonusMp:
                    TxtR.player.maxMp += EffectNum;
                    break;
                case Status.BonusHp:
                    TxtR.player.maxHp += EffectNum;
                    break;
                case Status.BonusAtk:
                    // 미구현
                    break;
                case Status.BonusDef:
                    //
                    // 미구현
                    break;
            }

            // 장비 사용 중  처리
            if (Type == ItemType.Weapon ||
                Type == ItemType.ExtraWeapon ||
                Type == ItemType.Head ||
                Type == ItemType.Body)
            {
                IsUsing = true;
            }

            //이전 장비 수치 제거
            if (temp != null)
            {
                // 장비 추가 효과 제거
                switch (temp.EffectStatus)
                {
                    case Status.Hp:
                        //TxtR.player.hp += EffectNum;
                        break;
                    case Status.Mp:
                        break;
                    case Status.Exp:
                        break;
                    case Status.Level:
                        break;
                    case Status.Atk:
                        break;
                    case Status.Def:
                        break;
                    case Status.BonusMp:
                        TxtR.player.maxMp -= temp.EffectNum;
                        break;
                    case Status.BonusHp:
                        TxtR.player.maxHp -= temp.EffectNum;
                        break;
                    case Status.BonusAtk:
                        break;
                    case Status.BonusDef:
                        break;
                }

                Console.WriteLine($"{Name} 장비, {temp.Name} 장비 해제");
                Inventory.Inven.Find(it => it == temp).IsUsing = false;
            }           
        }

        public void AddAmount(int num)
        {
            Amount += num;

            if (Amount <= 0)
            {
                Inventory.Inven.Remove(this);
            }
        }

        public void BuyItem()
        {
            if (Price <= TxtR.player.gold)
            {
                if (Inventory.Inven.Contains(this))
                {
                    int idx = Inventory.Inven.FindIndex(n => n == this);
                    Inventory.Inven[idx].Amount++;
                }
                else
                {
                    Inventory.Inven.Add(this);
                }
                TxtR.player.gold -= Price;
                Console.WriteLine($"{this.Name} 구매 , 남은 골드 {TxtR.player.gold}G");
            }
            else
            {
                Console.WriteLine("골드가 부족합니다.");
            }
        }

        public void SellItem()
        {
            if (Inventory.Inven.Contains(this))
            {
                int idx = Inventory.Inven.FindIndex(n => n == this);
                if (Inventory.Inven[idx].Amount > 1)
                {
                    Inventory.Inven[idx].Amount--;
                }
                else
                {
                    Inventory.Inven.Remove(Inventory.Inven[idx]);
                }
                TxtR.player.gold += (int)(Price * 0.8f);
                Console.WriteLine($"{this.Name} 판매 , 보유 골드 {TxtR.player.gold}G");
            }
            else
            {
                Console.WriteLine("오류 : 보유하고 있지 않은 아이템입니다.");
            }

        }

    }
    public class Inventory
    {
        public static Dictionary<ItemType, Item?> equipments = new Dictionary<ItemType, Item?>() {
            { ItemType.Head, null },
            { ItemType.Body, null },
            { ItemType.Weapon, null },
            { ItemType.ExtraWeapon, null },
        };

        public static List<Item> Inven = new List<Item>() { };

        public static void GetItem(Item item)
        {
            if (Inventory.Inven.Contains(item))
            {
                Inventory.Inven.Find(targetItem => targetItem == item).Amount++;
            }
            else
            {
                Inventory.Inven.Add(item);
            }
        }

        public static string SaveInvenInfo()
        {
            string output = "";

            if (Inventory.Inven.Count > 0)
            {
                foreach (Item item in Inventory.Inven)
                {
                    output += ($"{item.Name}[{item.Amount},");
                }
            }
            return output;
        }

        public static void PrintInventory()
        {
            //장비 출력
            Console.WriteLine("================= 장비 ================");
            foreach (KeyValuePair<ItemType, Item> item in equipments)
            {
                Console.WriteLine("[{0}: {1}]", item.Key.ToString(), (item.Value == null ? "빈칸" : item.Value.Name));
            }

            //인벤 출력
            Console.WriteLine("================= 인벤토리 ================");

            for (int i = 0; i < Inven.Count; i++)
            {
                Item itemInven = Inven[i];
                Console.WriteLine($"{i + 1}.[{itemInven.Name}]: [{itemInven.Amount}] {(itemInven.IsUsing == true ? "[E]" : " ")}");
            }

        }

        public static void InventoryInput()
        {
            bool flag = true;
            Console.Clear();
            PrintInventory();

            while (flag)
            {
                Console.WriteLine("사용 할 아이템의 번호를 입력해주세요.");

                string inpuString = Console.ReadLine();
                int input = (int.TryParse(inpuString, out int value)) ? value : -1; // 입력을 정수로 변환, 실패시 정수 -1 반환

                if(input == 0)
                {
                    // 인벤토리 떠나기
                    flag = false;
                    break;
                }
                else if (input <= Inventory.Inven.Count && input > 0)
                {
                    //인벤토리의 아이템 사용
                    string usedItemName = Inventory.Inven[input - 1].Name;
                    Console.Clear();
                    Inventory.Inven[input - 1].UseItem();
                    PrintInventory();
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

    public class ItemPreset
    {
        public static List<Item> itemList = new List<Item>() {
            new Item("TestItem", Status.Atk, 999, 1, ItemType.Weapon, 999, "테스트 아이템입니다."),
            new Item("테스트무기", Status.Atk, 999, 1, ItemType.Weapon, 999, "테스트 아이템입니다."),
            new Item("테스트보조무기", Status.Atk, 999, 1, ItemType.Weapon, 999, "테스트 아이템입니다."),
            new Item("테스트투구", Status.Atk, 999, 1, ItemType.Weapon, 999, "테스트 아이템입니다."),
            new Item("테스트갑옷", Status.Atk, 999, 1, ItemType.Weapon, 999, "테스트 아이템입니다.")
        };

        public static List<Item> dropItemList = new List<Item>() {
            new Item("고블린드랍", Status.None, 0, 1, ItemType.Etc, 0, "고블린의 드랍아이템"),
            new Item("거미드랍", Status.None, 0, 1, ItemType.Etc, 0, "거미의 드랍아이템"),
            new Item("늑대드랍", Status.None, 0, 1, ItemType.Etc, 0, "늑대의 드랍아이템")
        };


    }
}
