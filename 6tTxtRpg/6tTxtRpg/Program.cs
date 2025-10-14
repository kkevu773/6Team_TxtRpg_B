using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace _6TxtRpg // 이쪽에 만들기
{
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
                name = Console.ReadLine();
                if (name.Length <= 6)
                {
                    Console.WriteLine($"{name}님 이 맞으십니까?");
                    string yes = Console.ReadLine();
                    if (yes == "네" || yes == "예" || yes == "ㅇㅇ" || yes == "ㅇ")
                    {
                        Console.WriteLine("확인되었습니다.");
                        break;
                    }
                    else
                    {
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
                string input = Console.ReadLine();
                job = input;
                if (job == "전사" || job == "마법사" || job == "도적")
                {
                    Console.WriteLine($"{job}이 맞으십니까?");
                    string yes = Console.ReadLine();
                    if (yes == "네" || yes == "예" || yes == "ㅇㅇ" || yes == "ㅇ")
                    {
                        Console.WriteLine("확인되었습니다.");
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
                Console.WriteLine($"Name: {name}{job}");
                Console.WriteLine($"Level: {level}");
                Console.WriteLine($"Hp: {hp}/{maxHp}");
                Console.WriteLine($"Mp: {mp}/{maxMp}");
                Console.WriteLine($"Attack: {damage}");
                Console.WriteLine($"Defense: {defense}");
                Console.WriteLine($"Exp: {exp}");
                Console.WriteLine($"Gold: {gold}");
            }

        }
        public void ShortInfo() //전투에 사용할 정보창
        {
            Console.WriteLine($"Lv.{level} {name} the {job}  HP: {hp} MP: {mp}");
        }
        public void BlowPlayer(Monster monster) //몬스터가 캐릭터를 공격하는 메서드
        {
            float actualDamage = monster.damage - this.defense;
            if (actualDamage < 0)
            {
                actualDamage = 0;
            }
            this.hp -= (int)actualDamage;
        }
        

    }
    internal class TxtR
    {


        public static void Main(string[] args)
        {
            var intro = new Intro();
            Character player = intro.IntroA();

        }
    }


    public class Intro
    {
        // 캐릭터를 만들어 반환
        public Character IntroA()
        {

            Console.WriteLine("스파르타 텍스트 알피지에 오신 것을 환영합니다.");

            var player = new Character("", "");
            player.YourName();
            player.YourJob();

            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.\n이제 전투를 시작할 수 있습니다.\n\n1. 상태 보기\n2. 전투 시작");
            Console.WriteLine("\n원하시는 행동을 입력해주세요.\n>>  ");
            return player;
            MonsterList monList = new MonsterList();//배틀용 몬스터리스트 소환
            Battle battle = new Battle(player, monList.monsterList);//배틀코드

        }
    }
}
