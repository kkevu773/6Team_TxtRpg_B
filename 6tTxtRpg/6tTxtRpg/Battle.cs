using _6TxtRpg;

namespace _6TxtRpg
{
    class Battle // 전투기능 작업
    {
        //List<Monster> monsters = new List<Monster>();
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
            Console.ForegroundColor = ConsoleColor.White;
            monsters.monsterList.Clear();
            monNum = (byte)random.Next(1, 5);//등장 몬스터 수. 작은 수니까 byte로 처리했다. 0~3까지 계산.
            for (int i = 0; i < monNum; ++i)//몬스터 수만큼 반복.
            { monsters.AddRandom(); }

            startHp = character.hp;
            isBattle = true;
            currentPhase = Phase.Waiting;
            while (isBattle) //여기서부터 반복구문
            {
                Console.Clear();
                if (currentPhase == Phase.CharATK )
                {
                    BattleMsg("Battle!!", ConsoleColor.DarkRed);
                }
                if (currentPhase != Phase.MonsterATK)
                {
                    if (currentPhase != Phase.CharATK)
                    { Console.WriteLine(); }
                    ShowMon(monsters);
                    Console.WriteLine();
                    ShowChar(character);
                }
                if (currentPhase == Phase.Waiting)//선택페이즈
                {
                    Console.WriteLine("1. 공격");
                    Console.WriteLine();
                    BattleMenuKey();
                }
                else if (currentPhase == Phase.CharATK)//공격페이즈
                {
                    Console.WriteLine("0. 취소");
                    Console.WriteLine();
                    AtkMenu(character, monsters);
                }
                else if (currentPhase == Phase.CharATKFin)
                { NextButton("다음", "", Phase.MonsterATK); }
                else if (currentPhase == Phase.MonsterATK)
                {
                    MonATK(character, monsters);
                    currentPhase = Phase.Waiting;
                }
                MonDead(monsters, monNum);//몬스터가 죽었는지 확인
                if (character.hp <= 0)//플레이어가 죽었는지 확인
                {
                    currentPhase = Phase.MonWin;
                    isBattle = false;
                }
            }
            Console.Clear();
            BattleMsg("Battle!! - Result", ConsoleColor.DarkRed);
            if (currentPhase == Phase.CharWin)
            {
                BattleMsg("Victory", ConsoleColor.Green);
                Console.WriteLine($"던전에서 몬스터를 {monNum}마리 잡았습니다.");
                Console.WriteLine();
                BattleResult(character);
            }
            else if (currentPhase == Phase.MonWin)
            {
                BattleMsg("You Lose", ConsoleColor.DarkRed);
                BattleResult(character);
            }
        }
        void ShowMon(MonsterList monsters)//몬스터 정보 출력 함수 //TODO:띄어쓰기 처리 안해놨음. 나중에 디버깅하면서 수정해야 됨.
        {
            for (int i = 0; i < monNum; ++i)
            {
                if (monsters.monsterList[i].hp <= 0)//사망시 색 바꿈
                { Console.ForegroundColor = ConsoleColor.DarkGray; }
                if (currentPhase == Phase.CharATK)
                { Console.Write($"{i + 1} "); }
                if (monsters.monsterList[i].hp <= 0)//사망시 Dead표시
                {
                    Console.WriteLine($"LV.{monsters.monsterList[i].level} {monsters.monsterList[i].name} Hp Dead");//TODO:몬스터 레벨변수 생기면 주석해제
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                { monsters.monsterList[i].ShortInfo(); }//배열에 넣은 몬스터 출력
            }
        }
        void ShowChar(Character character)//플레이어 정보출력 함수. 가독성을 위해 일단 뺐다.
        {
            Console.WriteLine($"LV.{character.level} {character.name} ({character.job})");
            Console.WriteLine($"Hp {character.hp} / {character.hp}");
            Console.WriteLine();
        }
        void BattleMsg(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }
        void WrongMsg()//잘못된 키 입력시 나오는 함수
        {
            Console.WriteLine();
            Console.WriteLine("잘못된 입력입니다");
            Console.ReadKey(true);
        }//키 입력이 잘못될 시 나오는 메세지
        void TypeMsg(string msg)//메세지 입력문구
        {
            Console.WriteLine(msg);
            Console.Write(">> ");
        }
        void BattleMenuKey()//배틀 매뉴 함수. 역시 가독성을 위해 뺐다.
        {
            TypeMsg("원하시는 행동을 입력해주세요.");
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
        void AtkMenu(Character character, MonsterList monsters)//몬스터 공격 선택 함수
        {
            TypeMsg("대상을 선택해주세요.");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    CharAtk(character, monsters, 0);
                    break;
                case ConsoleKey.D2:
                    CharAtk(character, monsters, 1);
                    break;
                case ConsoleKey.D3:
                    CharAtk(character, monsters, 2);
                    break;
                case ConsoleKey.D4:
                    CharAtk(character, monsters, 3);
                    break;
                case ConsoleKey.D0:
                    currentPhase = Phase.Waiting;
                    break;
                default:
                    WrongMsg();
                    break;
            }
        }

        void CharAtk(Character character, MonsterList monsters, byte num)//몬스터 공격 판정용 함수
        {

            if (monNum >= num + 1) //몬스터가 있음.
            {
                if (monsters.monsterList[num].hp <= 0) //만약 몬스터가 죽었을 때
                { WrongMsg();}
                else
                {
                    Console.Clear();
                    BattleMsg("Battle!!", ConsoleColor.DarkRed);
                    float beforehit = monsters.monsterList[num].hp;
                    int error = (int)Math.Ceiling((float)character.damage * 0.1f); //오차범위 처리.
                    int charDamage = random.Next(character.damage - error, character.damage + error + 1);
                    Console.WriteLine($"{character.name}의 공격!");
                    Console.WriteLine($"Lv.{monsters.monsterList[num].level} {monsters.monsterList[num].name}을 맞췄습니다. (데미지 : {charDamage})");
                    Console.WriteLine();
                    monsters.monsterList[num].Damaged(charDamage);
                    Console.WriteLine();
                    Console.WriteLine($"Lv.{monsters.monsterList[num].level} {monsters.monsterList[num].name}");
                    Console.Write($"HP {beforehit} -> ");
                    if (monsters.monsterList[num].hp <= 0)
                    { Console.Write("Dead");}
                    else
                    { Console.Write($"Hp {monsters.monsterList[num].hp} "); }
                    Console.WriteLine();
                    Console.WriteLine();
                    NextButton("다음", "", Phase.CharATKFin);
                }
            }
            else
            { WrongMsg(); }
        }
        void NextButton(string message, string typeMsg, Phase nextPhase)
        {
            Console.WriteLine($"0.{message}");
            TypeMsg(typeMsg);
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
            {
                Console.WriteLine();
                TypeMsg("대상을 선택해주세요.");
            }
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
        void MonATK(Character character, MonsterList monsters)//몬스터 순서대로 공격 메서드
        {
            for (int i = 0; i < monNum; i++)
            {
                if (monsters.monsterList[i].hp > 0)//안 죽었을때만 플레이어 공격.
                { MonAtkMsg(character, monsters.monsterList[i]);}
            }
        }
        void MonAtkMsg(Character character, Monster monster)//몬스터 공격시 나오는 메세지
        {
            Console.Clear();
            BattleMsg("Battle!!", ConsoleColor.DarkRed);
            Console.WriteLine($"{monster.name}의 공격!");
            //monster.RandomAttack();
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
            Console.WriteLine();
            NextButton("다음", false);
        }
        void MonDead(MonsterList monsters, int num)
        {
            if (num <= 1)
            {
                if (monsters.monsterList[0].hp <= 0)
                { WinEnd(); }
            }
            else if (num <= 2)
            {
                if (monsters.monsterList[0].hp <= 0 && monsters.monsterList[1].hp <= 0)
                { WinEnd(); }
            }
            else if (num <= 3)
            {
                if (monsters.monsterList[0].hp <= 0 && monsters.monsterList[1].hp <= 0 && monsters.monsterList[2].hp <= 0)
                { WinEnd(); }
            }
            else if (num <= 4)
            {
                if (monsters.monsterList[0].hp <= 0 && monsters.monsterList[1].hp <= 0 && monsters.monsterList[2].hp <= 0 && monsters.monsterList[3].hp <= 0)
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