using System;
using System.Collections.Generic;
using System.Linq;
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
        public Character(string name, string job) //생성자
        {
            this.name = name;
            this.job = job;
            this.level = 1;
            this.maxHp = 100;
            this.hp = 100;
            this.maxMp = 100;
            this.mp = 100;
            this.damage = 10;
            this.defense = 5;
            this.gold = 1500;
        }
        public void YourName() //이름 정하기
        {
            while (true)
            {
                Console.Write("이름을 입력해주세요(1~6 글자 제한)");
                string input = Console.ReadLine();
                name = input;
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
                Console.Write("직업을 선택해주세요(전사, 마법사, 궁수)");
                string input = Console.ReadLine();
                job = input;
                if (job == "전사" || job == "마법사" || job == "궁수")
                {
                    Console.WriteLine($"{job}이 맞으십니까?");
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
        public void ShowInfo() //정보창
        {
            Console.WriteLine($"Name: {name}{job}");
            Console.WriteLine($"Level: {level}");
            Console.WriteLine($"Hp: {hp}");
            Console.WriteLine($"Attack: {damage}");
            Console.WriteLine($"Defense: {defense}");
            Console.WriteLine($"Exp: {exp}");
            Console.WriteLine($"Gold: {gold}");
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

            Console.WriteLine("직업을 선택 해주세요.");

            return player;
        }
    }
}
