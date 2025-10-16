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
        public static Character player = new Character();


        public static void Main(string[] args)
        {
            Console.ForegroundColor = Tool.color1;
            player.YourName();
            player.YourJob();
            var intro = new Intro();
            MonsterList monsterList = new MonsterList();
            Battle battle = new Battle(player, monsterList);
            Quest quest = new Quest();

            while (true)
            {
                Console.Clear();
                intro.IntroA(battle.Stage);
                switch (Console.ReadKey().KeyChar)//디버깅하려고 임시로 넣은거라 로직 바꾸셔도 됩니다.
                {
                    case '1':
                        player.ShowInfo();
                        break;
                    case '2':
                        battle.RunBattle(false);
                        break;
                    case '3':
                        battle.RunBattle(true);
                        break;
                    case '4':
                        player.level = 1;
                        player.gold = 10000;
                        Inventory.GetItem(ItemPreset.itemList[1]);
                        Inventory.GetItem(ItemPreset.dropItemList[0]);
                        Inventory.GetItem(ItemPreset.dropItemList[0]);
                        Inventory.GetItem(ItemPreset.dropItemList[0]);
                        Shop.ShopInput();
                        break;
                    case '5':
                        Inventory.GetItem(ItemPreset.itemList[1]);
                        Inventory.GetItem(ItemPreset.itemList[2]);
                        Inventory.GetItem(ItemPreset.itemList[3]);
                        Inventory.GetItem(ItemPreset.itemList[6]);
                        Inventory.InventoryInput();
                        break;
                    case '6':
                        quest.ShowQuest();
                        break;
                    default:
                        break;
                }
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
        public int CriticalChance; // 치명타 확률
        public List<Skills> skill = new List<Skills>(); //스킬리스트
        Random random = new Random(); //랜덤함수
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
                    this.maxHp = 100;
                    this.hp = 100;
                    this.maxMp = 50;
                    this.mp = 50;
                    this.damage = 25;
                    this.defense = 5;
                    this.CriticalChance = 10;

                    break;
                case "마법사": //마법사값을 받았을경우 스텟
                    this.maxHp = 50;
                    this.hp = 50;
                    this.maxMp = 100;
                    this.mp = 100;
                    this.damage = 15;
                    this.defense = 0;
                    this.CriticalChance = 20;
                    break;
                case "도적": //도적값을 받았을경우 스텟
                    this.maxHp = 60;
                    this.hp = 60;
                    this.maxMp = 35;
                    this.mp = 35;
                    this.damage = 35;
                    this.defense = 0;
                    this.CriticalChance = 20;
                    break;
            }
        }
        public void levelUp()
        {
            if (exp >= level * 15) //경험치가 레벨*100이상이면 레벨업
            {
                exp -= level * 15;
                level++;
                if (job == "전사")
                {
                    maxHp += 25;
                    hp = maxHp;
                    maxMp += 10;
                    mp = maxMp;
                    damage += 10;
                    defense += 3;
                    Console.WriteLine($"축하합니다! {name}님이 레벨 {level}이 되셨습니다!");
                }
                else if (job == "마법사")
                {
                    maxHp += 10;
                    hp = maxHp;
                    maxMp += 25;
                    mp = maxMp;
                    damage += 5;
                    defense += 2;
                    Console.WriteLine($"축하합니다! {name}님이 레벨 {level}이 되셨습니다!");
                }
                else if (job == "도적")
                {
                    maxHp += 10;
                    hp = maxHp;
                    maxMp += 5;
                    mp = maxMp;
                    damage += 13;
                    defense += 2;
                    Console.WriteLine($"축하합니다! {name}님이 레벨 {level}이 되셨습니다!");
                }
            }
        }
        public void ShowInfo() //정보창
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Name: {name} {job}"); //이름, 직업
                Console.WriteLine($"Level: {level}"); //레벨
                Console.WriteLine($"Hp: {hp}/{maxHp}"); //체력/최대체력
                Console.WriteLine($"Mp: {mp}/{maxMp}"); //마나/최대마나
                Console.WriteLine($"Attack: {damage}"); //공격력
                Console.WriteLine($"Defense: {defense}"); //방어력
                Console.WriteLine($"Exp: {exp}"); //경험치
                Console.WriteLine($"Gold: {gold}"); //골드
                Console.WriteLine();
                BuffList.PrintBuff();
                Console.WriteLine("나가시려면 0을 입력해주세요.");
                string output = Console.ReadLine();
                if (output == "0")
                {
                    Console.Clear();
                    break;
                }
            }

        }
        public void ShortInfo() //전투에 사용할 정보창
        {
            Console.WriteLine($"Lv.{level} {name} the {job}  HP: {hp} MP: {mp}");
            BuffList.PrintBuff();
        }
        public void SkillList() //스킬리스트 출력
        {
            Console.WriteLine("=====스킬리스트=====");
            foreach (var skills in skill)
            {
                Console.WriteLine($"스킬이름:{skills.name} 소모MP:{skills.mp}");
            }
        }
        public float BlowPlayer(float damage, Character player) //몬스터가 캐릭터를 공격하는 메서드
        {
            float actualDamage = damage - player.defense;
            if (actualDamage < 0)
            {
                actualDamage = 0;
            }
            player.hp -= (int)actualDamage;
            if (player.hp < 0)
            {
                player.hp = 0;
            }

            return actualDamage;
        }
        public float BlowMonster(float damage, Monster monster) //캐릭터가 몬스터를 공격하는 메서드
        {
            float actualDamage = damage - monster.armor;
            if (actualDamage < 0)
            {
                actualDamage = 0;
            }
            monster.hp -= (int)actualDamage;

            return actualDamage;
        }
        public int PlayerCri()//1에서 100까지의 랜덤 숫자를 뽑고 해당숫자가 직업별 크리티컬 확률보다
        {                      //낮을시 크리티컬 발동
            int cri = damage;
            int shit = random.Next(1, 101);
            if (CriticalChance >= shit)
            {
                Console.WriteLine("치명적인 일격으로 공격했습니다!");
                cri = damage * 2;
            }
            return cri;
        }



    }
    public class Skills : Character //스킬 정보
    {

        public string name; //스킬이름
        public float state; //스킬능력치
        public int mp; //스킬사용시 소모mp
        Random random = new Random(); //랜덤함수

        public Skills(string name, float state, int mp)
        {
            this.name = name;
            this.state = state;
            this.mp = mp;
        }
        public void JobSkill(Character player) // 플레이어의 스킬리스트
        {
            if (player.job == "전사")
            {
                player.skill.Add(new Skills("힘껏치기", 1.5f, 10));
                player.skill.Add(new Skills("단단해지기", 1.5f, 7));
            }
            else if (player.job == "마법사")
            {
                player.skill.Add(new Skills("화염구", 2.0f, 10));
                player.skill.Add(new Skills("회복", 0.2f, 15));
            }
            if (player.job == "도적")
            {
                player.skill.Add(new Skills("암살", 1.3f, 20));
                player.skill.Add(new Skills("흡혈", 0.8f, 15));
            }
        }
        public void UseSkills(Character player, Monster monster) //스킬 사용 메서드
        {
            if (player.mp >= mp)
            {
                player.mp -= mp;
                if (name == "힘껏치기")
                {
                    float damage = PlayerCri() * state;
                    float actualDamage = player.BlowMonster(damage, monster);
                    Console.WriteLine($"{Tool.Josa(player.name.ToString(), "이", "가")} {Tool.Josa(this.name, "을", "를")} 사용했습니다!!");
                    Console.WriteLine($"{monster.name}에게 {actualDamage}의 피해를 입혔습니다!");
                }
                else if (name == "단단해지기")
                {
                    player.defense = (int)(player.defense * state);
                    Console.WriteLine($"{Tool.Josa(player.name.ToString(), "이", "가")} {Tool.Josa(this.name, "을", "를")} 사용했습니다!!");
                    Console.WriteLine($"{player.name}의 방어력이 {player.defense}가 되었습니다!");
                }
                else if (name == "화염구")
                {
                    float damage = PlayerCri() * state;
                    float actualDamage = player.BlowMonster(damage, monster);
                    Console.WriteLine($"{Tool.Josa(player.name.ToString(), "이", "가")} {Tool.Josa(this.name, "을", "를")} 사용했습니다!!");
                    Console.WriteLine($"{monster.name}에게 {actualDamage}의 피해를 입혔습니다!");
                }
                else if (name == "회복")
                {
                    int heal = (int)(player.maxHp * state);
                    if (player.hp + heal >= player.maxHp)
                    {
                        player.hp = player.maxHp;
                        Console.WriteLine($"{Tool.Josa(player.name.ToString(), "이", "가")} {Tool.Josa(this.name, "을", "를")} 사용했습니다!!");
                        Console.WriteLine($"{player.name}의 체력이 최대치가 되었습니다!");
                    }
                    else
                    {
                        player.hp += heal;
                        Console.WriteLine($"{Tool.Josa(player.name.ToString(), "이", "가")} {Tool.Josa(this.name, "을", "를")} 사용했습니다!!");
                        Console.WriteLine($"{player.name}의 체력이 {heal}만큼 회복되었습니다!");
                    }
                }
                else if (name == "암살")
                {
                    int holy = random.Next(1, 101);
                    if (holy <= 70)
                    {
                        float damage = PlayerCri() * state * 2;
                        float actualDamage = player.BlowMonster(damage, monster);
                        Console.WriteLine($"{Tool.Josa(player.name.ToString(), "이", "가")} {Tool.Josa(this.name, "을", "를")} 사용했습니다!!");
                        Console.WriteLine("치명적인 일격으로 공격했습니다!");
                        Console.WriteLine($"{monster.name}에게 {actualDamage}의 피해를 입혔습니다!");
                    }
                    else
                    {
                        float damage = PlayerCri() * state;
                        float actualDamage = player.BlowMonster(damage, monster);
                        Console.WriteLine($"{Tool.Josa(player.name.ToString(), "이", "가")} {Tool.Josa(this.name, "을", "를")} 사용했습니다!!");
                        Console.WriteLine($"{monster.name}에게 {actualDamage}의 피해를 입혔습니다!");
                    }

                }

            }
        }
    }


    public class Intro
    {
        public void IntroA(int stage)
        {
            Console.WriteLine("스파르타 텍스트 알피지에 오신 것을 환영합니다.");
            Console.Write("스파르타 던전에 오신 여러분 환영합니다.\n이제 전투를 시작할 수 있습니다.\n\n");
            Tool.ColorTxt("1", Tool.color5);
            Console.Write(".상태 보기\n");
            Tool.ColorTxt("2", Tool.color5);
            Console.Write(".전투 시작 (현재 진행 : ");
            Tool.ColorTxt(stage.ToString(), Tool.color4);
            Console.WriteLine(" 층)");
            Tool.ColorTxt("3", Tool.color5);
            Console.Write(".");
            if (stage <= 1)
            { Console.WriteLine("연습하기"); }
            else
            {
                Tool.ColorTxt((stage - 1).ToString(), Tool.color4);
                Console.WriteLine("층에서 연습");
            }
            Tool.ColorTxt("4", Tool.color5);
            Console.Write(".상점\n");
            Tool.ColorTxt("5", Tool.color5);
            Console.Write(".인벤토리\n");
            Tool.ColorTxt("6", Tool.color5);
            Console.WriteLine(".퀘스트");
            Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
        }
    }
}
    
