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
        public string name;
        public string job;
        public int level;
        public int hp;
        public int damage;
        public int defense;
        public int exp;
        public int gold;
        public Character(string name, string job) //생성자
        {
            this.name = name;
            this.job = job;
            this.level = 1;
            this.hp = 100;
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
