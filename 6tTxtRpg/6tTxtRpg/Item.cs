using _6tTxtRpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace _6TxtRpg
{
    public enum ItemType { Head, Body, Weapon, ExtraWeapon, HpPotion, MpPotion, Buff, Etc }
    public enum Status { Hp, Mp, Exp, Level, Atk, Def, BonusHp, BonusMp, BonusAtk, BonusDef, None }

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

        public int Enchant;

        //생성
        public Item(string name, int enchant, Status effectStatus, int effectNum, int amount, ItemType type, int price, string itemScript)
        {
            Name = name;
            EffectStatus = effectStatus;
            EffectNum = effectNum;
            Amount = amount;
            Type = type;
            Price = price;
            IsUsing = false;
            ItemScript = itemScript;
            Enchant = enchant;
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
                        Inventory.equipments[ItemType.Head] = this;
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
                        Inventory.equipments[ItemType.Body] = this;
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
                        Inventory.equipments[ItemType.Weapon] = this;
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
                        Inventory.equipments[ItemType.ExtraWeapon] = this;
                    }
                    break;

                case ItemType.HpPotion:
                    AddAmount(-1);
                    break;

                case ItemType.MpPotion:
                    AddAmount(-1);
                    break;

                case ItemType.Buff:
                    BuffList.GetBuff(this);
                    AddAmount(-1);
                    break;
            }


            //아이템 수치 적용
            if (Type != ItemType.Buff)
            {
                switch (EffectStatus)
                {
                    case Status.Hp:
                        TxtR.player.hp += EffectNum;
                        if (TxtR.player.hp > TxtR.player.maxHp) TxtR.player.hp = TxtR.player.maxHp;
                        break;
                    case Status.Mp:
                        TxtR.player.mp += EffectNum;
                        if (TxtR.player.mp > TxtR.player.maxMp) TxtR.player.mp = TxtR.player.maxMp;
                        break;
                    case Status.Exp:
                        // 여기에 케릭터 경험치 획득 메소드 연결
                        break;
                    case Status.Level:
                        // 사용 미정
                        break;
                    case Status.Atk:
                        TxtR.player.damage += EffectNum;
                        break;
                    case Status.Def:
                        TxtR.player.defense += EffectNum;
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
            }

            // 장비 사용 중  처리
            if (Type == ItemType.Weapon ||
                Type == ItemType.ExtraWeapon ||
                Type == ItemType.Head ||
                Type == ItemType.Body)
            {
                IsUsing = true;

                if (OpenQuest.QuestList.Any(quest => quest.QuestName == "장비를 장착해보자"
                            && quest.IsStart))
                {
                    Quest? targetQuest = OpenQuest.QuestList.FirstOrDefault(quest => quest.QuestName == "장비를 장착해보자");
                    targetQuest?.Trigger();
                }
                Console.WriteLine("퀘스트처리if문 동작");
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
                        TxtR.player.damage -= temp.EffectNum;
                        break;
                    case Status.Def:
                        TxtR.player.defense -= temp.EffectNum;
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

        public void EnchantItem()
        {
            if (Enchant < 3)
            {
                Enchant++;

                int temp = EffectNum;
                EffectNum += (int)(EffectNum * 0.2f); //능력치 20% 증가

                //이전 능력치 적용 제거, 새 능력치 적용
                if (IsUsing)
                {
                    switch (EffectStatus)
                    {
                        case Status.BonusHp:
                            TxtR.player.maxHp -= temp;
                            TxtR.player.maxHp += EffectNum;
                            break;
                        case Status.BonusMp:
                            TxtR.player.maxMp -= temp;
                            TxtR.player.maxMp += EffectNum;
                            break;
                        case Status.Atk:
                            TxtR.player.damage -= temp;
                            TxtR.player.damage += EffectNum;
                            break;
                        case Status.Def:
                            TxtR.player.defense -= temp;
                            TxtR.player.defense += EffectNum;
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("오류: 최대 강화 입니다.");
            }
        }

    }
    public class Inventory
    {

        public static int invenPage = 0;

        public static Dictionary<ItemType, Item?> equipments = new Dictionary<ItemType, Item?>() {//TODO:저장
            { ItemType.Head, null },
            { ItemType.Body, null },
            { ItemType.Weapon, null },
            { ItemType.ExtraWeapon, null },
        };

        public static List<Item> Inven = new List<Item>() { };//TODO:저장

        public static void GetItem(Item item)
        {
            if (Inventory.Inven.Contains(item))
            {
                Inventory.Inven.Find(targetItem => targetItem == item).Amount++;

                if (item.Amount <= 0)
                {
                    Inventory.Inven.Remove(item);
                }
            }
            else
            {
                Inventory.Inven.Add(item);
            }
        }

        public static void LoseItem(Item item)
        {
            if (Inventory.Inven.Contains(item))
            {
                Inventory.Inven.Find(targetItem => targetItem == item).Amount--;

                if (item.Amount <= 0)
                {
                    Inventory.Inven.Remove(item);
                }
            }
            else
            {
                //Console.WriteLine("인벤토리에 아이템이 존재하지 않음");
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
            List<Item> printItemList = Inven.GetRange(invenPage * 9, (Inven.Count-invenPage*9 >= 9 ? 9 : (Inven.Count - invenPage * 9)));

            Console.WriteLine("================= 장비 ================");
            int num = 1;
            foreach (KeyValuePair<ItemType, Item> item in equipments)
            {
                Console.WriteLine("[{0}: {1}]", item.Key.ToString(), (item.Value == null ? "빈칸" : item.Value.Name + " {+ " + item.Value.Enchant + "} (해제: -" + num + ")"));
                num++;
            }

            //인벤 출력
            Console.WriteLine("================= 인벤토리 ================");

            for (int i = 0; i < printItemList.Count; i++)
            {
                Item itemInven = printItemList[i];
                string itemEffect = "";
                switch (itemInven.EffectStatus)
                {
                    case Status.Hp:
                        itemEffect += "[체력" + itemInven.EffectNum + " 회복]";
                        break;
                    case Status.Mp:
                        itemEffect += "[마나" + itemInven.EffectNum + " 회복]";
                        break;
                    case Status.Exp:
                        itemEffect += "[EXP" + itemInven.EffectNum + " 증가]";
                        break;
                    case Status.Level:
                        itemEffect += "[LV" + itemInven.EffectNum + " 증가]";
                        break;
                    case Status.Atk:
                        itemEffect += "[데미지" + itemInven.EffectNum + " 증가]";
                        break;
                    case Status.Def:
                        itemEffect += "[방어" + itemInven.EffectNum + " 증가]";
                        break;
                    case Status.BonusMp:
                        itemEffect += "[최대마나" + itemInven.EffectNum + " 증가]";
                        break;
                    case Status.BonusHp:
                        itemEffect += "[최대체력" + itemInven.EffectNum + " 증가]";
                        break;
                    case Status.BonusAtk:
                        break;
                    case Status.BonusDef:
                        break;
                }
                Console.WriteLine($"{i + 1}.[{itemInven.Name}]+{itemInven.Enchant}: [{itemInven.Amount}] {(itemInven.IsUsing == true ? "[E]" : " ")} / {itemEffect} [{itemInven.Price}G]");
            }

        }

        public static void moveInvenPage(bool next)
        {
            int maxPage = (Inven.Count() / 9) + (Inven.Count() % 9 > 0 ? 1 : 0);

            if (next)
            {
                invenPage++;

                if (invenPage >= maxPage -1)
                {
                    invenPage = maxPage - 1;
                }

                if(Inven.Count == 0)
                {
                    invenPage = 0;
                }
            }
            else 
            {
                invenPage--;

                if (invenPage < 0)
                {
                    invenPage = 0;
                }
            }

        }

        public static void DisarmEquip(ItemType type)
        {
            Item temp = equipments[type];

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
                        TxtR.player.damage -= temp.EffectNum;
                        break;
                    case Status.Def:
                        TxtR.player.defense -= temp.EffectNum;
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

                Console.WriteLine($"{temp.Name} 장비 해제");
                Inventory.Inven.Find(it => it == temp).IsUsing = false;
                Inventory.equipments[type] = null;
            }
        }

        public static void InventoryInput()
        {
            bool flag = true;
            Console.Clear();
            PrintInventory();

            while (flag)
            {
                Console.WriteLine($"0.나가기\n이전페이지[<] {invenPage + 1}/{(Inven.Count() / 9) + (Inven.Count() % 9 > 0 ? 1 : 0)} [>]다음페이지\n사용 할 아이템의 번호를 입력해주세요.");

                ConsoleKeyInfo keyInfo = Console.ReadKey();
                char inputChar = keyInfo.KeyChar;

                if(keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    moveInvenPage(false);
                    Console.Clear();
                    PrintInventory();
                    continue;

                }
                else if (keyInfo.Key == ConsoleKey.RightArrow)
                {
                    moveInvenPage(true);
                    Console.Clear();
                    PrintInventory();
                    continue;
                }

                int input = (int.TryParse(inputChar.ToString(), out int value)) ? value : -99; // 입력을 정수로 변환, 실패시 정수 -1 반환

                if (input == 0)
                {
                    // 인벤토리 떠나기
                    flag = false;
                    Console.Clear();
                    break;
                }
                else if (input <= (Inventory.Inven.Count - invenPage * 9) && input > 0)
                {
                    //인벤토리의 아이템 사용
                    string usedItemName = Inventory.Inven[(input + invenPage * 9) - 1].Name;
                    Console.Clear(); // 아이템 사용 메세지 출력 위헤 먼저 삭제
                    int invenNum = Inventory.Inven.Count;

                    Inventory.Inven[(input + invenPage * 9) - 1].UseItem();

                    //사용 후 아이템 페이지가 줄어들면
                    if (Inventory.Inven.Count - invenPage * 9 == 0 && Inventory.Inven.Count < invenNum)
                    {
                        invenPage--;
                    }
                    PrintInventory();
                    //이후 인벤토리 출력갱신 필요
                }
                else if (input >= -4 && input <= -1)
                {
                    switch (input)
                    {
                        case -1:
                            //머리 해제
                            DisarmEquip(ItemType.Head);
                            break;
                        case -2:
                            //몸통 해제
                            DisarmEquip(ItemType.Body);
                            break;
                        case -3:
                            //무기 해제
                            DisarmEquip(ItemType.Weapon);
                            break;
                        case -4:
                            //보조무기 해제
                            DisarmEquip(ItemType.ExtraWeapon);
                            break;
                    }

                    Console.Clear();
                    PrintInventory();
                }
                else
                {
                    // 인벤토리 아이템 수를 넘어가는 수, 이외의 값들
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다.");
                    PrintInventory();
                }
            }
        }
    }

    public class ItemPreset
    {
        public static List<Item> itemList = new List<Item>() {
            new Item("강철검", 0, Status.Atk, 5, 1, ItemType.Weapon, 100, "강철로 만들어진 검입니다."),
            new Item("강철도끼", 0, Status.Atk, 7, 1, ItemType.Weapon, 150, "강철로 만들어진 도끼입니다."),
            new Item("버클러", 0, Status.Def, 5, 1, ItemType.ExtraWeapon, 200, "동그란 소형 방패입니다."),
            new Item("가죽투구", 0, Status.Def, 3, 1, ItemType.Head, 150, "몬스터의 가죽으로 만든 투구입니다."),
            new Item("가죽갑옷", 0, Status.Def, 6, 1, ItemType.Body, 200, "질긴 가죽을 덧댄 갑옷입니다."),
            new Item("분노의 영약", 3, Status.Atk, 10, 1, ItemType.Buff, 100, "일시적으로 공격력이 상승하는 물약입니다.")
        };

        public static List<Item> dropItemList = new List<Item>() {
            new Item("고블린의 귀", 0, Status.None, 0, 1, ItemType.Etc, 1, "고블린 토벌의 증표로 사용됩니다."),
            new Item("거미의 독샘", 0, Status.None, 0, 1, ItemType.Etc, 3, "거미 토벌의 증표로 사용됩니다."),
            new Item("늑대의 송곳니", 0, Status.None, 0, 1, ItemType.Etc, 9, "늑대 토벌의 증표로 사용됩니다."),
            new Item("늑대왕의 발톱", 0, Status.None, 0, 1, ItemType.Etc, 20, "늑대왕 토벌의 증표로 사용됩니다.")
        };

        public static List<Item> testItemList = new List<Item>() {
            new Item("쓸만한 방패", 0, Status.Def, 5, 1, ItemType.ExtraWeapon, 500, "테스트 아이템입니다."),
            new Item("소형 체력포션", 0, Status.Hp, 10, 1, ItemType.HpPotion, 100, "테스트 아이템입니다."),
              new Item("소형 마나포션", 0, Status.Mp, 10, 1, ItemType.MpPotion, 100, "테스트 아이템입니다.")
        };


    }
}
