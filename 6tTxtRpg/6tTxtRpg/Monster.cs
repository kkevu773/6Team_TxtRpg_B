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
    class Monster
    {
        public string name;
        public float damage;
        public float armor;
        public float hp;
        public bool isDead = false;
        public virtual void ShowInfo()
        {
            Console.WriteLine($"몬스터 이름 : {name}");
            Console.WriteLine($"공격력(DMG) : {damage}");
            Console.WriteLine($"방어력(DEF) : {armor}");
            Console.WriteLine($"체력(HP) : {hp}");
        }
        public void Damaged(float damage)
        {
            if (damage > this.armor)
            {
                this.hp -= damage - this.armor;
                Console.WriteLine($"{name}이(가) {damage - this.armor}의 피해를 받았습니다");
                CheckHp();
            }

        }
        public void CheckHp()
        {
            if (hp <= 0)
            {
                isDead = true;
                Console.WriteLine($"{name}이(가) 사망하였습니다.");
            }
        }
        class Goblin : Monster
        {

            public Goblin()
            {
                this.name = "고블린";
                this.armor = 3;
                this.damage = 5;
                this.hp = 20;
            }
        }
        class Spider : Monster
        {
            public Spider()
            {
                this.name = "거미";
                this.armor = 1;
                this.damage = 8;
                this.hp = 10;
            }
        }
        class Wolf : Monster
        {
            public Wolf()
            {
                this.name = "늑대";
                this.armor = 2;
                this.damage = 9;
                this.hp = 15;
            }
        }
    }
    class MosterList()
    {
        public List<Monster> monsterList = new List<Monster>();


        public void RemoveMonter(Monster monster)
        {
            if (monster == null)
            {
                if (monsterList.Contains(monster))
                {
                    monsterList.Remove(monster);
                }
            }
        }

        public void AddMonster(Monster monster)
        {
            if (monster == null)
            {
                monsterList.Add(monster);
            }
        }
        public List<Monster> GetMonsters()
        {

            return monsterList;
        }
        public void AddRandom()
        {
            Random monsterRandom = new Random();

            monsterRandom.Next(3);
        }
    }

}
