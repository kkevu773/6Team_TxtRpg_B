using _6tTxtRpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace _6TxtRpg // 이쪽에 만들기
{
    internal class TxtR //쉬운 디버깅을 위해 위로 뻈습니다.
    {
        public static Character player;

        public static void Main(string[] args)
        {
            var intro = new Intro();
            player = intro.IntroA();

            MonsterList monsterList = new MonsterList();
            Battle battle = new Battle(player, monsterList);
            switch (Console.ReadKey(true).Key)//디버깅하려고 임시로 넣은거라 로직 바꾸셔도 됩니다.
            {
                case ConsoleKey.D2:
                    battle.RunBattle();
                    break;
                case ConsoleKey.D3:
                    player.level = 1;
                    player.gold = 10000;
                    Inventory.GetItem(ItemPreset.itemList[1]);
                    Inventory.GetItem(ItemPreset.dropItemList[0]);
                    Inventory.GetItem(ItemPreset.dropItemList[0]);
                    Inventory.GetItem(ItemPreset.dropItemList[0]);
                    Shop.ShopInput();
                    break;
                case ConsoleKey.D4:
                    Inventory.GetItem(ItemPreset.itemList[1]);
                    Inventory.GetItem(ItemPreset.itemList[2]);
                    Inventory.GetItem(ItemPreset.itemList[3]);
                    Inventory.GetItem(ItemPreset.itemList[4]);
                    Inventory.InventoryInput();
                    break;
                default:
                    break;
            }
            /*/Battle 사용법
            MonsterList monsterList = new MonsterList(); //캐릭터와 몬스터 리스트의 인자값이 필요해서 선행으로 생성해줘야 합니다.
            //그 외 다른 방법으로도 인자값을 넣을 수 있으면 상관없음.
            Battle battle = new Battle(player,monsterList); //배틀 생성. 생성될때 배틀장면이 한번 작동합니다.
            battle.RunBattle(player,monsterList);//생성한 뒤에 다른곳에 배틀장면이 필요하면 이렇게 부르면 됩니다.
            */
        }
    }
    public class Character
    {
        //기본 인터페이스 구성
        public string name; // 이름
        public string job; // 직업
        public int level;  //레벨
        public int maxHp; // 최대체력
        public int hp; // 현재체력
        public int maxMp; // 최대마나
        public int mp; // 현재마나
        public int damage; // 공격력
        public int defense; // 방어력
        public int exp; // 경험치
        public int gold; // 골드
        public Character() //생성자
        {
        }
        public void YourName() //이름 정하기
        {
            while (true)
            {
                Console.Write("이름을 입력해주세요(1~6 글자 제한)");
                Console.WriteLine();
                name = Console.ReadLine();
                if (name.Length <= 6)
                {
                    Console.WriteLine($"{name}님 이 맞으십니까?");
                    string yes = Console.ReadLine();
                    if (yes == "네" || yes == "예" || yes == "ㅇㅇ" || yes == "ㅇ")
                    {
                        Console.Clear();
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("다시 입력해주세요.");
                    }
                }
            }
        }
        public void YourJob() //직업 정하기
        {
            while (true)
            {
                Console.Write("직업을 선택해주세요(전사, 마법사, 도적)");
                Console.WriteLine();
                string input = Console.ReadLine();
                job = input;
                if (job == "전사" || job == "마법사" || job == "도적")
                {
                    Console.WriteLine($"{job}이 맞으십니까?");
                    string yes = Console.ReadLine();
                    if (yes == "네" || yes == "예" || yes == "ㅇㅇ" || yes == "ㅇ")
                    {
                        Console.Clear();
                        SetState(); //직업을 정함과 동시에 메서드 호출로 스탯 세팅
                        break;
                    }
                    else
                    {
                        Console.WriteLine("다시 입력해주세요.");
                    }
                }

            }

        }
        public void SetState() // 스탯 세팅
        {
            this.level = 1;
            this.exp = 0;
            this.gold = 1500;
            switch (job)
            {
                case "전사": //전사값을 받았을경우 스텟
                    this.maxHp = 150;
                    this.hp = 150;
                    this.maxMp = 100;
                    this.mp = 100;
                    this.damage = 30;
                    this.defense = 20;

                    break;
                case "마법사": //마법사값을 받았을경우 스텟
                    this.maxHp = 100;
                    this.hp = 100;
                    this.maxMp = 200;
                    this.mp = 200;
                    this.damage = 30;
                    this.defense = 5;
                    break;
                case "도적": //도적값을 받았을경우 스텟
                    this.maxHp = 120;
                    this.hp = 120;
                    this.maxMp = 70;
                    this.mp = 70;
                    this.damage = 50;
                    this.defense = 10;
                    break;
            }
        }
        public void ShowInfo() //정보창
        {
            while (true)
            {
                Console.WriteLine($"Name: {name} {job}"); //이름, 직업
                Console.WriteLine($"Level: {level}"); //레벨
                Console.WriteLine($"Hp: {hp}/{maxHp}"); //체력/최대체력
                Console.WriteLine($"Mp: {mp}/{maxMp}"); //마나/최대마나
                Console.WriteLine($"Attack: {damage}"); //공격력
                Console.WriteLine($"Defense: {defense}"); //방어력
                Console.WriteLine($"Exp: {exp}"); //경험치
                Console.WriteLine($"Gold: {gold}"); //골드
                Console.WriteLine();
                Console.WriteLine("나가시려면 0을 입력해주세요.");
                string output = Console.ReadLine();
                if (output == "0")
                {
                    break;
                }
            }

        }
        public void ShortInfo() //전투에 사용할 정보창
        {
            Console.WriteLine($"Lv.{level} {name} the {job}  HP: {hp} MP: {mp}");
        }
        public float BlowPlayer(float damage ,Character player) //몬스터가 캐릭터를 공격하는 메서드
        {
            float actualDamage = damage - player.defense;
            if (actualDamage < 0)
            {
                actualDamage = 0;
            }
            player.hp -= (int)actualDamage;

            return actualDamage;
        }


    }
    public class Intro
    {
        // 캐릭터를 만들어 반환
        public Character IntroA()
        {
            Console.WriteLine("스파르타 텍스트 알피지에 오신 것을 환영합니다.");
            var player = new Character();
            player.YourName();
            player.YourJob();
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.\n이제 전투를 시작할 수 있습니다.\n\n1. 상태 보기\n2. 전투 시작\n3. 상점\n4.인벤토리");
            Console.WriteLine("\n원하시는 행동을 입력해주세요.\n>>  ");            
            return player;
        }
    }
}
