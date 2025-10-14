using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;
using System.Runtime.Serialization;
using System.Security;
using _6TxtRpg;

namespace _6TxtRpg
{
    class Battle // 전투기능 작업
    {
        List<Monster> fightMonsters = new List<Monster>();
        Random random = new Random();//공용으로 쓸 랜덤
        byte monNum = 0;//몬스터수. 다른 메서드에서 쓸거 같아서 뻄.
        bool isBattle = false; //전투상태인지 체크.
        Phase currentPhase = Phase.Unknown;//페이즈 체크용 변수
        int startHp;
        enum Phase
        {
            Waiting,
            CharATK,
            CharATKFin,
            MonsterATK,
            CharWin,
            MonWin,
            Unknown,
        }
        public Battle(Character character, MonsterList monsters) //반드시 받아야 되는 인자값을 위해 생성자로 만듬.
        { //몬스터 1~4마리의 정보가 필요할 것 같다.
            monNum = (byte)random.Next(0, 4);//등장 몬스터 수. 작은 수니까 byte로 처리했다. 0~3까지 계산.
            for (int i = 0; i >= monNum; ++i)//몬스터 수만큼 반복.
            {
                monsters.OrderBy(monsters => random.Next());//몬스터 리스트 한번 섞어줌.
                fightMonsters.Add(monsters[0]);//0번 몬스터만 리스트에 추가한다.
            }
            startHp = character.hp;
            isBattle = true;
            currentPhase = Phase.Waiting;

            while (isBattle) //여기서부터 반복구문
            {
                if (currentPhase == Phase.CharATKFin || currentPhase == Phase.MonsterATK)
                { BattleMsg("Battle!!", ConsoleColor.DarkRed); }
                ShowMon();
                ShowChar(character);
                if (currentPhase == Phase.Waiting)//선택페이즈
                {
                    Console.WriteLine("1. 공격");
                    BattleMenuKey();
                }
                else if (currentPhase == Phase.CharATK)//공격페이즈
                { AtkMenu(character); }
                else if (currentPhase == Phase.CharATKFin)
                { NextButton("취소", Phase.MonsterATK); }
                else if (currentPhase == Phase.MonsterATK)
                {
                    MonATK(character);
                    currentPhase = Phase.Waiting;
                }
                MonDead(monNum);//몬스터가 죽었는지 확인
                if (character.hp <= 0)//플레이어가 죽었는지 확인
                {
                    currentPhase = Phase.MonWin;
                    isBattle = false;
                }
            }

            BattleMsg("Battle!! - Result", ConsoleColor.DarkRed);
            Console.WriteLine();
            if (currentPhase == Phase.CharWin)
            {
                BattleMsg("Victory", ConsoleColor.Green);
                Console.WriteLine();
                Console.WriteLine($"던전에서 몬스터를 {monNum}마리 잡았습니다.");
                Console.WriteLine();
                BattleResult(character);
            }
            else if (currentPhase == Phase.MonWin)
            {
                BattleMsg("You Lose", ConsoleColor.DarkRed);
                Console.WriteLine();
                BattleResult(character);
            }
        }
        void ShowMon()//몬스터 정보 출력 함수 //TODO:띄어쓰기 처리 안해놨음. 나중에 디버깅하면서 수정해야 됨.
        {
            for (int i = 0; i >= monNum; ++i)
            {
                if (fightMonsters[i].hp <= 0)//사망시 색 바꿈
                { Console.ForegroundColor = ConsoleColor.DarkGray; }
                if (currentPhase == Phase.CharATK)
                { Console.Write($"{i}."); }
                if (fightMonsters[i].hp <= 0)//사망시 Dead표시
                {
                    Console.WriteLine($"LV.{fightMonsters[i].level} {fightMonsters[i].name} Hp Dead");//TODO:몬스터 레벨변수 생기면 주석해제
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine($"LV.{fightMonsters[i].level} {fightMonsters[i].name} Hp {fightMonsters[i].hp} / {fightMonsters[i].maxHP}");//배열에 넣은 몬스터 출력
            }
        }
        void ShowChar(Character character)//플레이어 정보출력 함수. 가독성을 위해 일단 뺐다.
        {
            Console.WriteLine($"LV.{character.level} {character.name} ({character.job}");
            Console.WriteLine($"Hp {character.hp} / {character.hp}");
            Console.WriteLine();
        }
        void BattleMsg(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine();
            Console.WriteLine(msg);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }
        void WrongMsg()//잘못된 키 입력시 나오는 함수
        { Console.WriteLine("잘못된 입력입니다"); }//키 입력이 잘못될 시 나오는 메세지
        void TypeMsg()//메세지 입력문구
        {
            Console.WriteLine("대상을 선택해주세요.");
            Console.Write(">> ");
        }
        void BattleMenuKey()//배틀 매뉴 함수. 역시 가독성을 위해 뺐다.
        {
            TypeMsg();
            switch (Console.ReadKey().Key) //숫자만 눌러도 작동하게 ReadKey로 처리했습니다.
            {
                case ConsoleKey.D1://공격키
                    currentPhase = Phase.CharATK;//몬스터 이름앞에 숫자가 나옴.
                    break;
                default:
                    WrongMsg();
                    break;
            }
        }
        void AtkMenu(Character character)//몬스터 공격 선택 함수
        {
            TypeMsg();
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    CharAtk(character, 0);
                    break;
                case ConsoleKey.D2:
                    CharAtk(character, 1);
                    break;
                case ConsoleKey.D3:
                    CharAtk(character, 2);
                    break;
                case ConsoleKey.D4:
                    CharAtk(character, 3);
                    break;
                default:
                    WrongMsg();
                    break;
            }
        }

        void CharAtk(Character character, byte num)//몬스터 공격 판정용 함수
        {
            if (monNum < num) //몬스터가 없을 때
            { WrongMsg(); }
            else
            {
                if (fightMonsters[num].hp <= 0) //만약 몬스터가 죽었을 때
                { WrongMsg(); }
                else
                {
                    int error = (int)Math.Ceiling((float)character.damage * 0.1f); //오차범위 처리.
                    int charDamage = random.Next(character.damage - error, character.damage + error + 1);
                    Console.WriteLine($"{character.name}의 공격!");
                    Console.WriteLine($"Lv.{fightMonsters[num].level} {fightMonsters[num].name}을 맞췄습니다. (데미지 : {charDamage})");
                    Console.WriteLine();
                    Console.WriteLine($"Lv.{fightMonsters[num].level} {fightMonsters[num].name}");
                    Console.Write($"HP {fightMonsters[num].hp} -> ");
                    fightMonsters[num].hp -= charDamage;
                    if (fightMonsters[num].hp <= 0)
                    { Console.Write("Dead"); }
                    else
                    { Console.Write($"Hp {fightMonsters[num].hp} "); }
                    Console.WriteLine();
                    NextButton("다음", Phase.CharATKFin);
                }
            }
        }
        void NextButton(string message, Phase nextPhase)
        {
            Console.WriteLine($"0.{message}");
            TypeMsg();
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D0:
                    currentPhase = nextPhase;
                    break;
                default:
                    WrongMsg();
                    break;
            }
        }
        void NextButton(string message, bool isMsgOn)
        {
            Console.WriteLine($"0.{message}");
            if (isMsgOn)
            { TypeMsg(); }
            else
            { Console.Write(">> "); }
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D0:
                    break;
                default:
                    WrongMsg();
                    break;
            }
        }
        void MonATK(Character character)//몬스터 순서대로 공격 메서드
        {
            for (int i = 0; i < monNum; i++)
            {
                if (fightMonsters[i].hp > 0)//안 죽었을때만 플레이어 공격.
                { MonAtkMsg(character, fightMonsters[i]); }
            }
        }
        void MonAtkMsg(Character character, Monster monster)//몬스터 공격시 나오는 메세지
        {
            Console.WriteLine($"{monster.name}의 공격!");
            Console.WriteLine($"Lv.{character.level} {character.name}을 맞췄습니다. (데미지 : {monster.damage})");
            Console.WriteLine();
            Console.WriteLine($"Lv.{character.level} {character.name}");
            Console.Write($"HP {character.hp} -> ");
            character.hp -= (int)monster.damage;
            if (character.hp <= 0)
            { Console.Write("Dead"); }
            else
            { Console.Write($"Hp {character.hp} "); }
            Console.WriteLine();
            NextButton("다음", true);
        }
        void MonDead(byte num)
        {
            if (num <= 0)
            {
                if (fightMonsters[0].hp <= 0)
                { WinEnd(); }
            }
            else if (num <= 1)
            {
                if (fightMonsters[0].hp <= 0 && fightMonsters[1].hp <= 0)
                { WinEnd(); }
            }
            else if (num <= 2)
            {
                if (fightMonsters[0].hp <= 0 && fightMonsters[1].hp <= 0 && fightMonsters[2].hp <= 0)
                { WinEnd(); }
            }
            else if (num <= 3)
            {
                if (fightMonsters[0].hp <= 0 && fightMonsters[1].hp <= 0 && fightMonsters[2].hp <= 0 && fightMonsters[3].hp <= 0)
                { WinEnd(); }
            }
        }
        void WinEnd()
        {
            currentPhase = Phase.CharWin;
            isBattle = false;
        }
        void BattleResult(Character character)
        {
            Console.WriteLine($"LV.{character.level} {character.name}");
            Console.WriteLine($"HP {startHp} -> {character.hp}");
            Console.WriteLine();
            NextButton("다음", false);
        }

    }
}