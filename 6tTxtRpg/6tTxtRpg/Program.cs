using _6tTxtRpg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
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
                        OpenQuest.ShowQuest();
                        break;
                    case '7':
                        //저장하기 제작
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
        public List<Skills> WarriorSkill = new List<Skills>();// 전사 스킬리스트
        public List<Skills> MageSkill = new List<Skills>();//마법사 스킬리스트
        public List<Skills> BanditSkill = new List<Skills>();//도적 스킬리스트
        Random random = new Random(); //랜덤함수
        public string input; //입력값 저장용 변수
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
                if (job == "전사" || job == "마법사" || job == "도적" || job == "농부")
                {
                    Console.WriteLine($"{job}이 맞으십니까?");
                    string yes = Console.ReadLine();
                    if (yes == "네" || yes == "예" || yes == "ㅇㅇ" || yes == "ㅇ")
                    {
                        Console.Clear();
                        SetState(); //직업을 정함과 동시에 메서드 호출로 스탯 세팅
                        Skills.JobSkill(this); //직업을 정함과 동시에 메서드 호출로 스킬 세팅
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
                case "농부": // 히든직업: 최고의 생존력(HP/방어력)과 최저의 MP (노 스킬 콘셉트)
                    this.maxHp = 85; // 전사보다 약간 낮은 고체력
                    this.hp = 85;
                    this.maxMp = 30; // 가장 낮은 마력/스태미너 (노 스킬)
                    this.mp = 30;
                    this.damage = 20; // 낮은 기본 공격력 (단순 노동으로 얻은 힘)
                    this.defense = 4; // 비전사 직업 중 가장 높은 방어력
                    this.CriticalChance = 50; //스킬이 없어 운에 의존한다.
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
                else if (job == "농부")// 농부 (히든 - 노 스킬): 생존력과 방어력에 극단적으로 집중
                {
                    maxHp += 25; // 전사와 동일한 최대 HP 증가 (최고의 지구력)
                    hp = maxHp;
                    maxMp += 5;  // 도적과 동일한 최소 MP 증가 (스킬 부재 반영)
                    mp = maxMp;
                    damage += 7; // 마법사보다는 높고 도적보다는 낮은 공격력 증가 (성실함)
                    defense += 4; // 모든 직업 중 가장 높은 방어력 증가 (극강의 단단함)
                    Console.WriteLine($"축하합니다! {name}님이 레벨 {level}이 되셨습니다!");
                }
                if (OpenQuest.QuestList.Any(quest => quest.QuestName == "더욱 더 강해지기!"
                            && quest.IsStart))
                {
                    Quest? targetQuest = OpenQuest.QuestList.FirstOrDefault(quest => quest.QuestName == "더욱 더 강해지기!");
                    targetQuest?.Trigger();
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
                Console.WriteLine("나가시려면 0을 눌러주세요.");
                if (Console.ReadKey().KeyChar == '0')
                {
                    Console.Clear();
                    break;
                }
            }

        }
        public void ShortInfo() //전투에 사용할 정보창
        {
            Console.Write($"Lv.");
            Tool.ColorTxt(level.ToString(), Tool.color4);
            Console.Write($" {name} the {job}  HP ");
            if (hp >= maxHp * 0.5f)
            { Tool.ColorTxt(hp.ToString(), Tool.color3); }
            else if (hp <= maxHp * 0.5f && hp >= maxHp * 0.1f)
            { Tool.ColorTxt(hp.ToString(), Tool.color4); }
            else if (hp <= maxHp * 0.1f)
            { Tool.ColorTxt(hp.ToString(), Tool.color2); }
            Console.Write(" MP ");
            if (mp >= maxMp * 0.5f)
            { Tool.ColorTxt(mp.ToString(), Tool.color5); }
            else if (mp <= maxMp * 0.5f && hp >= maxHp * 0.1f)
            { Tool.ColorTxt(hp.ToString(), Tool.color4); }
            else if (mp <= maxMp * 0.1f)
            { Tool.ColorTxt(hp.ToString(), Tool.color2); }
            Console.WriteLine();
            Console.WriteLine();
            BuffList.PrintBuff();
        }
        public void SkillList() //스킬리스트 출력
        {
            int number = 0;
            //Console.WriteLine("=====스킬리스트=====");
            if (job == "전사")
            {
                foreach (Skills skills in WarriorSkill)
                {
                    number++;
                    Tool.ColorTxt(number.ToString(), Tool.color4);
                    Console.Write($" {skills.name} MP ");
                    Tool.ColorTxt(skills.mp.ToString(), Tool.color5);
                    Console.WriteLine();
                }
            }
            else if (job == "마법사")
            {
                foreach (Skills skills in MageSkill)
                {
                    number++;
                    Tool.ColorTxt(number.ToString(), Tool.color4);
                    Console.Write($" {skills.name} MP ");
                    Tool.ColorTxt(skills.mp.ToString(), Tool.color5);
                    Console.WriteLine();
                }
            }
            else if (job == "도적")
            {
                foreach (Skills skills in BanditSkill)
                {
                    number++;
                    Tool.ColorTxt(number.ToString(), Tool.color4);
                    Console.Write($" {skills.name} MP ");
                    Tool.ColorTxt(skills.mp.ToString(), Tool.color5);
                    Console.WriteLine();
                }
            }
            else if (job == "농부")
            {
                Console.WriteLine("농부는 그런거 모릅니다.");
                Console.WriteLine("농부는 곡괭이를 아무렇게나 휘둘렀습니다.");
            }
        }
        public void UseSkill(Monster monster)
        {
            input = Console.ReadLine();
            Console.Clear();
            if (job == "전사")
            {
                if (input == "1")
                {
                    WarriorSkill[0].UseWarriorSkills(TxtR.player, monster);
                }
                else if (input == "2")
                {
                    WarriorSkill[1].UseWarriorSkills(TxtR.player, monster);
                }
            }
            else if (job == "마법사")
            {
                if (input == "1")
                {
                    MageSkill[0].UseMageSkills(TxtR.player, monster);
                }
                else if (input == "2")
                {
                    MageSkill[1].UseMageSkills(TxtR.player, monster);
                }
            }
            else if (job == "도적")
            {
                if (input == "1")
                {
                    BanditSkill[0].UseBanditSkills(TxtR.player, monster);
                }
                else if (input == "2")
                {
                    BanditSkill[1].UseBanditSkills(TxtR.player, monster);
                }
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
    public class Skills //스킬 정보
    {

        public string name; //스킬이름
        public float state; //스킬능력치
        public int mp; //스킬사용시 소모mp
        Random random = new Random(); //랜덤함수
        float damage;
        float actualDamage;
        float defense;
        int heal;



        public Skills(string name, float state, int mp)
        {
            this.name = name;
            this.state = state;
            this.mp = mp;
        }
        public static void JobSkill(Character player) // 플레이어의 스킬리스트
        {
            switch (player.job)
            {
                case "전사":
                    player.WarriorSkill.Add(new Skills("힘껏치기", 1.5f, 10));
                    player.WarriorSkill.Add(new Skills("단단해지기", 0.5f, 7));
                    break;
                case "마법사":
                    player.MageSkill.Add(new Skills("화염구", 2.0f, 10));
                    player.MageSkill.Add(new Skills("회복", 0.2f, 15));
                    break;
                case "도적":
                    player.BanditSkill.Add(new Skills("암살", 1.3f, 20));
                    player.BanditSkill.Add(new Skills("흡혈", 0.8f, 15));
                    break;
            }
        }
        public void UseWarriorSkills(Character player, Monster monster) //전사스킬 사용 메서드
        {
            if (player.mp >= mp)
            {
                player.mp -= mp;
                if (player.input == "1") //힘껏치기
                {
                    damage = player.PlayerCri() * state;
                    actualDamage = player.BlowMonster(damage, monster);
                    Console.WriteLine($"{Tool.Josa(player.name, "이", "가")} {Tool.Josa(this.name, "을", "를")} 사용했습니다!!");
                    Console.WriteLine($"{monster.name}에게 {actualDamage}의 피해를 입혔습니다!");

                }
                else if (player.input == "2") //단단해지기
                {
                    player.defense = (int)(player.defense * state);
                    Console.WriteLine($"{Tool.Josa(player.name, "이", "가")} {Tool.Josa(this.name, "을", "를")} 사용했습니다!!");
                    Console.WriteLine($"{player.name}의 방어력이 {player.defense}가 되었습니다!");
                }
            }
        }
        public void UseMageSkills(Character player, Monster monster) //마법사스킬 사용 메서드
        {
            if (player.mp >= mp)
            {
                player.mp -= mp;
                if (player.input == "1") //화염구
                {
                    damage = player.PlayerCri() * state;
                    actualDamage = player.BlowMonster(damage, monster);
                    Console.WriteLine($"{Tool.Josa(player.name, "이", "가")} {Tool.Josa(this.name, "을", "를")} 사용했습니다!!");
                    Console.WriteLine($"{monster.name}에게 {actualDamage}의 피해를 입혔습니다!");
                }
                else if (player.input == "2") //회복
                {
                    heal = (int)(player.maxHp * state);
                    if (player.hp + heal >= player.maxHp) //회복량+기존체력이 최대체력보다 높을시 최대체력값으로 설정
                    {
                        player.hp = player.maxHp;
                        Console.WriteLine($"{Tool.Josa(player.name, "이", "가")} {Tool.Josa(this.name, "을", "를")} 사용했습니다!!");
                        Console.WriteLine($"{player.name}의 체력이 최대치가 되었습니다!");
                    }
                    else
                    {
                        player.hp += heal;
                        Console.WriteLine($"{Tool.Josa(player.name, "이", "가")} {Tool.Josa(this.name, "을", "를")} 사용했습니다!!");
                        Console.WriteLine($"{player.name}의 체력이 {heal}만큼 회복되었습니다!");
                    }
                }
            }
        }
        public void UseBanditSkills(Character player, Monster monster) //도적스킬 사용 메서드
        {
            if (player.mp >= mp)
            {
                player.mp -= mp;

                if (player.input == "1") //암살
                {
                    int holy = random.Next(1, 101);
                    if (holy <= 70) //스킬자체 크리티컬시스템을 넣어 이론상 4배딜 가능
                    {
                        damage = player.PlayerCri() * state * 2;
                        actualDamage = player.BlowMonster(damage, monster);
                        Console.WriteLine($"{Tool.Josa(player.name, "이", "가")} {Tool.Josa(this.name, "을", "를")} 사용했습니다!!");
                        Console.WriteLine("치명적인 일격으로 공격했습니다!");
                        Console.WriteLine($"{monster.name}에게 {actualDamage}의 피해를 입혔습니다!");
                    }
                    else //크리 안터지면 다른 일반 공격스킬과 같음
                    {
                        damage = player.PlayerCri() * state;
                        actualDamage = player.BlowMonster(damage, monster);
                        Console.WriteLine($"{Tool.Josa(player.name, "이", "가")} {Tool.Josa(this.name, "을", "를")} 사용했습니다!!");
                        Console.WriteLine($"{monster.name}에게 {actualDamage}의 피해를 입혔습니다!");
                    }
                }
                else if (player.input == "2") //흡혈
                {
                    damage = player.PlayerCri() * state;
                    actualDamage = player.BlowMonster(damage, monster);
                    int heal = (int)(actualDamage * 0.2f); //넣은 데미지의 20%만큼 회복
                    if (player.hp + heal >= player.maxHp)
                    {
                        player.hp = player.maxHp;
                        Console.WriteLine($"{Tool.Josa(player.name, "이", "가")} {Tool.Josa(this.name, "을", "를")} 사용했습니다!!");
                        Console.WriteLine($"{monster.name}에게 {actualDamage}의 피해를 입혔습니다!");
                        Console.WriteLine($"{player.name}의 체력이 최대치가 되었습니다!");
                    }
                    else
                    {
                        player.hp += heal;
                        Console.WriteLine($"{Tool.Josa(player.name, "이", "가")} {Tool.Josa(this.name, "을", "를")} 사용했습니다!!");
                        Console.WriteLine($"{monster.name}에게 {actualDamage}의 피해를 입혔습니다!");
                        Console.WriteLine($"{player.name}의 체력이 {heal}만큼 회복되었습니다!");
                    }
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
        Console.WriteLine();
        Tool.ColorTxt("7", Tool.color5);
        Console.WriteLine(".저장하기");
        Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
    }
}


