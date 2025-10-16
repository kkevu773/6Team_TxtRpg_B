using _6tTxtRpg;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace _6TxtRpg
{
    public abstract class Monster       //몬스터 기본 클래스
    {

        public string name;
        public float damage;
        public float armor;
        public float hp;
        public bool isDead = false;                                 //몬스터가 죽은지 아닌지 판별하는 변수
        public int level = 1;
        public float maxHP;
        private static Random random = new Random();

        public List<IMonsterSkill> skills = new List<IMonsterSkill>();      // 몬스터가 가지고 있는 스킬

        public void UseSkill(int index,Character player)                                    //해당하는 스킬을 쓰는 함수  0은 기본공격 1은 몬스터 특수공격이 기본세팅
        {
            if (TxtR.player == null)
            {
                Console.WriteLine("플레이어 정보가 존재하지 않습니다. (전투 로직 확인 필요)");
                return;
            }
            if (index >= 0 && index < skills.Count)
            {
                skills[index].Use(this,player);
            }
            else
            {
                Console.WriteLine("잘못된 스킬 번호입니다.");
            }
        }
        public void RandomAttack(Character player)      //몬스터가 가지고 있는 스킬을 랜덤하게 사
        {
            if (skills == null || skills.Count == 0)
            {
                Console.WriteLine($"{Tool.Josa(name.ToString(), "이", "가")} 사용할 스킬이 없습니다.");
                return;
            }

            int index = random.Next(skills.Count);
            skills[index].Use(this,player);
        }
        public abstract void DropItem();                                    //몬스터의 아이템을 드랍하는 함수

        public virtual void ShowInfo()                          //기본 몬스터 정보
        {
            Console.WriteLine($"Level : {level}");
            Console.WriteLine($"몬스터 이름 : {name}");
            Console.WriteLine($"공격력(DMG) : {damage}");
            Console.WriteLine($"방어력(DEF) : {armor}");
            Console.WriteLine($"체력(HP) : {hp}");
        }
        public void ShortInfo()                                 //전투에 사용할 몬스터 정보
        {

            Console.Write($"Lv.");
            Tool.ColorTxt(level.ToString(), Tool.color4);
            Console.Write($" {name}  HP ");
            Tool.ColorTxt(hp.ToString(),Tool.color4);
            Console.WriteLine();
        }
        public void Damaged(float damage)                   //몬스터 데미지 받는 함수       사용할때 호출하면 몬스터가 사망하고 isDead가 트루로 바뀜
        {
            if (damage > this.armor)
            {
                this.hp -= damage - this.armor;
                Console.Write($"{Tool.Josa(name.ToString(), "이", "가")} ");
                Tool.ColorTxt((damage - this.armor).ToString(), Tool.color2);
                Console.WriteLine("의 피해를 받았습니다");
                CheckHp();
            }
            else
            {
                Console.WriteLine($"{Tool.Josa(name.ToString(), "이", "가")} 방어했습니다.");
            }

        }
        public void CheckHp()                               //몬스터 체력체크
        {
            if (hp <= 0)
            {
                hp = 0;
                isDead = true;
                Console.WriteLine($"{Tool.Josa(name.ToString(), "이", "가")} 사망하였습니다.");
                DropItem();
            }
        }
        
        public class Goblin : Monster
        {
            public Goblin(int level)                     
            {
                this.level = random.Next(0, 3)+level; // 1~3 레벨
                this.name = "고블린";

                // 레벨별 스탯 조정
                this.armor = 3 + this.level;        // 기본 3 + 레벨
                this.damage = 5 + this.level * 2;   // 기본 5 + 레벨*2
                this.hp = 20 + this.level * 5;      // 기본 20 + 레벨*5
                this.maxHP = this.hp;
                skills.Add(new NormalAttack());
                skills.Add(new RockThorw());
            }
            public override void DropItem()             
            {
                Inventory.GetItem(ItemPreset.dropItemList[0]);
                Tool.ColorTxt(Tool.Josa(this.name,"이 ","가 ")+ Tool.Josa(ItemPreset.dropItemList[0].Name, "을", "를")+" 드랍했습니다",Tool.color5);
                
            }
        }

        public class Spider : Monster
        {
            public Spider(int level)
            {
                this.level = random.Next(0, 3) +level;
                this.name = "거미";

                this.armor = 1 + this.level;          // 기본 1 + 레벨
                this.damage = 8 + this.level;         // 기본 8 + 레벨
                this.hp = 10 + this.level * 3;        // 기본 10 + 레벨*3
                this.maxHP = this.hp;
                skills.Add(new NormalAttack());
                skills.Add(new Nip());
            }
            public override void DropItem()
            {
                Inventory.GetItem(ItemPreset.dropItemList[1]);
                Tool.ColorTxt(Tool.Josa(this.name, "이 ", "가 ") + Tool.Josa(ItemPreset.dropItemList[1].Name, "을", "를") + " 드랍했습니다", Tool.color5);
            }
        }

        public class Wolf : Monster
        {
            public Wolf(int level)
            {
                this.level = random.Next(0, 3) + level;
                this.name = "늑대";

                this.armor = 2 + this.level;          // 기본 2 + 레벨
                this.damage = 9 + this.level * 2;     // 기본 9 + 레벨*2
                this.hp = 15 + this.level * 5;        // 기본 15 + 레벨*5
                this.maxHP = this.hp;
                skills.Add(new NormalAttack());
                skills.Add(new Bite());
            }
            public override void DropItem()
            {
                Inventory.GetItem(ItemPreset.dropItemList[2]);
                Tool.ColorTxt(Tool.Josa(this.name, "이 ", "가 ") + Tool.Josa(ItemPreset.dropItemList[2].Name, "을", "를") + " 드랍했습니다", Tool.color5);
            }
        }
        public class WolfKing : Monster
        {
            public WolfKing()           //monsterList_.AddMonster(new Monster.WolfKing()); 이런식으로 생성하면 될듯
            {
                this.level = 20;
                this.name = "울부짖는 늑대왕";

                this.armor = 20;
                this.damage = 40;
                this.hp = 500;
                this.maxHP = this.hp;
                skills.Add(new NormalAttack());
                skills.Add(new Bite());
                skills.Add(new Howl());

            }
            public override void DropItem()
            {
                Inventory.GetItem(ItemPreset.dropItemList[3]);
                Tool.ColorTxt(Tool.Josa(this.name, "이 ", "가 ") + Tool.Josa(ItemPreset.dropItemList[3].Name, "을", "를") + " 드랍했습니다", Tool.color5);
            }
        }

    }
    public class MonsterList
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
        public void AddRandom(int level)   //몬스터 랜덤생성
        {

            int randomValue = monsterRandom.Next(3);

            switch (randomValue)
            {
                case 0:
                    this.AddMonster(new Monster.Wolf(level));
                    break;
                case 1:
                    this.AddMonster(new Monster.Goblin(level));
                    break;
                case 2:
                    this.AddMonster(new Monster.Spider(level));
                    break;
            }
        }
    }

    public interface IMonsterSkill          //몬스터 스킬 기본인터페이스
    {
        string Name { get; }
        void Use(Monster monster,Character player);
    }
    public class RockThorw : IMonsterSkill  
    {
        public string Name => "돌던지기";
        public void Use(Monster monster,Character player)
        {
            float damage = monster.damage + 3;
            float actualDamage = player.BlowPlayer(damage, player);
            Console.WriteLine($"{Tool.Josa(monster.name.ToString(), "이", "가")} {Tool.Josa(this.Name,"을","를")} 사용했습니다!!");
            Console.Write($"플레이어는 ");
            Tool.ColorTxt(actualDamage.ToString(), Tool.color2);
            Console.WriteLine("의 데미지를 입었습니다");
        }
    }

    public class Bite : IMonsterSkill
    {
        public string Name => "물기";
        public void Use(Monster monster,Character player)
        {
            float damage = monster.damage + 4;
            float actualDamage = player.BlowPlayer(damage,player);
            Console.WriteLine($"{Tool.Josa(monster.name.ToString(), "이", "가")} {Tool.Josa(this.Name, "을", "를")} 사용했습니다!!");
            Console.Write($"플레이어는 ");
            Tool.ColorTxt(actualDamage.ToString(), Tool.color2);
            Console.WriteLine("의 데미지를 입었습니다");

            //플레이어 피해를 입는 함수
        }
    }
    public class Nip : IMonsterSkill
    {
        public string Name => "깨물기";
        public void Use(Monster monster,Character player)
        {
            float damage = monster.damage + 2;
            float actualDamage = player.BlowPlayer(damage,player);
            Console.WriteLine($"{Tool.Josa(monster.name.ToString(), "이", "가")} {Tool.Josa(this.Name, "을", "를")} 사용했습니다!!");
            Console.Write($"플레이어는 ");
            Tool.ColorTxt(actualDamage.ToString(), Tool.color2);
            Console.WriteLine("의 데미지를 입었습니다");
        }
    }
    public class NormalAttack : IMonsterSkill
    {
        public string Name => "공격";
        public void Use(Monster monster, Character player)
        {
            float damage = monster.damage;
            float actualDamage = player.BlowPlayer(damage,player);
            Console.WriteLine($"{Tool.Josa(monster.name.ToString(), "이", "가")} {Tool.Josa(this.Name, "을", "를")} 사용했습니다!!");
            Console.Write($"플레이어는 ");
            Tool.ColorTxt(actualDamage.ToString(), Tool.color2);
            Console.WriteLine("의 데미지를 입었습니다");
        }
    }
    public class Howl : IMonsterSkill
    {
        public string Name => "울부짖기";
        public void Use(Monster monster, Character player)
        {

            monster.damage = monster.damage * 1.5f;
            
            Console.WriteLine($"{Tool.Josa(monster.name.ToString(), "이", "가")} {Tool.Josa(this.Name, "을", "를")} 사용했습니다!!");
            Console.WriteLine($"{monster.name}의 공격력이 상승했습니다.");
            
        }
    }
}
