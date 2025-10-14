using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace _6TxtRpg
{
    public class Monster
    {
        public string name;
        public float damage;
        public float armor;
        public float hp;
        public bool isDead = false;
        public int level = 1;
        public float maxHP;
        Random random = new Random();
        public virtual void ShowInfo()            //기본 몬스터 정보
        {
            Console.WriteLine($"Level : {level}");
            Console.WriteLine($"몬스터 이름 : {name}");
            Console.WriteLine($"공격력(DMG) : {damage}");
            Console.WriteLine($"방어력(DEF) : {armor}");
            Console.WriteLine($"체력(HP) : {hp}");
        }
        public void ShortInfo()                     //전투에 사용할 몬스터 정보
        {
            Console.WriteLine($"Lv.{level} {name}  HP {hp}");
        }
        public void Damaged(float damage)       //몬스터 데미지 받는 함수 사용할때 호출하면 몬스터가 사망하고 isDead가 트루로 바뀜
        {
            if (damage > this.armor)
            {
                this.hp -= damage - this.armor;
                Console.WriteLine($"{name}이(가) {damage - this.armor}의 피해를 받았습니다");
                CheckHp();
            }

        }
        public void CheckHp()          //몬스터 체력체크
        {
            if (hp <= 0)
            {
                isDead = true;
                Console.WriteLine($"{name}이(가) 사망하였습니다.");
            }
        }

        public class Goblin : Monster
        {
            public Goblin()   // 생성자에서 Random 객체 받기
            {
                this.level = random.Next(1, 4); // 1~3 레벨
                this.name = "고블린";

                // 레벨별 스탯 조정
                this.armor = 3 + level;        // 기본 3 + 레벨
                this.damage = 5 + level * 2;   // 기본 5 + 레벨*2
                this.hp = 20 + level * 5;      // 기본 20 + 레벨*5
                this.maxHP = this.hp;
            }
        }

        public class Spider : Monster
        {
            public Spider()
            {
                this.level = random.Next(1, 4);
                this.name = "거미";

                this.armor = 1 + level;          // 기본 1 + 레벨
                this.damage = 8 + level;         // 기본 8 + 레벨
                this.hp = 10 + level * 3;        // 기본 10 + 레벨*3
                this.maxHP = this.hp;
            }
        }

        public class Wolf : Monster
        {
            public Wolf()
            {
                this.level = random.Next(1, 4);
                this.name = "늑대";

                this.armor = 2 + level;          // 기본 2 + 레벨
                this.damage = 9 + level * 2;     // 기본 9 + 레벨*2
                this.hp = 15 + level * 5;        // 기본 15 + 레벨*5
                this.maxHP = this.hp;
            }
        }

    }
    class MonsterList
    {
        public List<Monster> monsterList = new List<Monster>();        //전투에 사용할 몬스터 리스트
        private Random monsterRandom = new Random();

        public void RemoveMonter(Monster monster)   //몬스터 리스트에서 지우기
        {
            if (monster != null && monsterList.Contains(monster))
            {
                
                    monsterList.Remove(monster);
                
            }
        }

        public void AddMonster(Monster monster)     //몬스터 직접생성
        {
            if (monster != null)
            {
                monsterList.Add(monster);
            }
        }
        public List<Monster> GetMonsters()          //몬스터 리스트 직접가져오기
        {

            return monsterList;
        }
        public void AddRandom()   //몬스터 랜덤생성
        {
            
            int randomValue = monsterRandom.Next(3);

            switch (randomValue)
            {
                case 0:
                    this.AddMonster(new Monster.Wolf());
                    break;
                case 1:
                    this.AddMonster(new Monster.Goblin());
                    break;
                case 2:
                    this.AddMonster(new Monster.Spider());
                    break;
            }
        }


    }

}
