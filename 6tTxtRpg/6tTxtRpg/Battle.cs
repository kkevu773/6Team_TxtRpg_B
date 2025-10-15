using _6tTxtRpg;
using _6TxtRpg;

namespace _6TxtRpg
{
    class Battle // 전투기능 작업
    {
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
        { RunBattle(character, monsters); }
        public void RunBattle(Character character, MonsterList monsters)
        {
            Console.ForegroundColor = Tool.color1;
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
                if (currentPhase == Phase.CharATK)
                { BattleMsg("Battle!!", Tool.color2); }
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
                    Tool.ColorTxt("1", Tool.color5);
                    Console.WriteLine(". 공격");
                    Console.WriteLine();
                    BattleMenuKey();
                }
                else if (currentPhase == Phase.CharATK)//공격페이즈
                {
                    Tool.ColorTxt("0", Tool.color5);
                    Console.WriteLine(". 취소");
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
            BattleMsg("Battle!! - Result", Tool.color2);
            if (currentPhase == Phase.CharWin)
            {
                BattleMsg("Victory", Tool.color3);
                Console.WriteLine($"던전에서 몬스터를 {monNum}마리 잡았습니다.");
                Console.WriteLine();
                BattleResult(character);
            }
            else if (currentPhase == Phase.MonWin)
            {
                BattleMsg("You Lose", Tool.color2);
                BattleResult(character);
            }
        }
        void ShowMon(MonsterList monsters)//몬스터 정보 출력 함수 //TODO:띄어쓰기 처리 안해놨음. 나중에 디버깅하면서 수정해야 됨.
        {
            for (int i = 0; i < monNum; ++i)
            {

                if (currentPhase == Phase.CharATK)
                { Tool.ColorTxt((i + 1).ToString(), Tool.color5); }
                Console.Write(" ");
                if (monsters.monsterList[i].hp <= 0)//사망시 Dead표시
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"LV.{monsters.monsterList[i].level} {monsters.monsterList[i].name} Hp Dead");//TODO:몬스터 레벨변수 생기면 주석해제
                    Console.ForegroundColor = Tool.color1;
                }
                else
                { monsters.monsterList[i].ShortInfo(); }//배열에 넣은 몬스터 출력
            }
        }
        void ShowChar(Character character)//플레이어 정보출력 함수. 가독성을 위해 일단 뺐다.
        {
            Console.Write($" LV.");
            Tool.ColorTxt(character.level.ToString(), Tool.color4);
            Console.WriteLine($" {character.name} ({character.job})");
            Console.Write($" Hp ");
            Tool.ColorTxt(character.hp.ToString(), Tool.color4);
            Console.Write(" / ");
            Tool.ColorTxt(character.hp.ToString(), Tool.color4);
            Console.WriteLine();
            Console.WriteLine();
        }
        void BattleMsg(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.WriteLine();
            Console.ForegroundColor = Tool.color1;
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
                { WrongMsg(); }
                else
                {
                    Console.Clear();
                    BattleMsg("Battle!!", Tool.color2);
                    float beforehit = monsters.monsterList[num].hp;
                    int error = (int)Math.Ceiling((float)character.damage * 0.1f); //오차범위 처리.
                    int charDamage = random.Next(character.damage - error, character.damage + error + 1);
                    Console.WriteLine($"{character.name}의 공격!");
                    Console.WriteLine();
                    Console.Write($"Lv.");
                    Tool.ColorTxt(monsters.monsterList[num].level.ToString(), Tool.color4);
                    Console.Write($" {Tool.Josa(monsters.monsterList[num].name, "을", "를")} 맞췄습니다. (데미지 : ");
                    Tool.ColorTxt(charDamage.ToString(), Tool.color2);
                    Console.Write(")");
                    Console.WriteLine();
                    Console.WriteLine();
                    monsters.monsterList[num].Damaged(charDamage);
                    Console.WriteLine();
                    Console.Write($"Lv.");
                    Tool.ColorTxt(monsters.monsterList[num].level.ToString(), Tool.color4); 
                    Console.Write($" {monsters.monsterList[num].name}");
                    Console.WriteLine();
                    Console.Write($"HP ");
                    Tool.ColorTxt(beforehit.ToString(), Tool.color4); 
                    Console.Write(" -> ");
                    if (monsters.monsterList[num].hp <= 0)
                    { Tool.ColorTxt("Dead", Tool.color2); }
                    else
                    {
                        Console.Write($"Hp ");
                        Tool.ColorTxt(monsters.monsterList[num].hp.ToString(), Tool.color2);
                    }
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
            Tool.ColorTxt("0", Tool.color5);
            Console.Write(". ");
            Console.WriteLine(message);
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
            Tool.ColorTxt("0", Tool.color5);
            Console.Write(". ");
            Console.WriteLine(message);
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
                { MonAtkMsg(character, monsters.monsterList[i]); }
            }
        }
        void MonAtkMsg(Character character, Monster monster)//몬스터 공격시 나오는 메세지
        {
            Console.Clear();
            BattleMsg("Battle!!", ConsoleColor.DarkRed);
            Console.WriteLine($"{monster.name}의 공격!");
            //monster.RandomAttack();
            Console.Write($"Lv.{character.level} {Tool.Josa(character.name, "을", "를")} 맞췄습니다. (데미지 : ");
            Tool.ColorTxt(monster.damage.ToString(), Tool.color2);
            Console.Write(")");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"Lv.{character.level} {character.name}");
            Console.Write($"HP ");
            Tool.ColorTxt(character.hp.ToString(), Tool.color4);
            Console.Write(" -> ");
            character.hp -= (int)monster.damage;
            if (character.hp <= 0)
            { Tool.ColorTxt("Dead", Tool.color2); }
            else
            {
                Console.Write($"Hp ");
                Tool.ColorTxt(character.hp.ToString(), Tool.color2);
            }
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
            Console.Write($"HP {startHp} -> ");
            if (startHp == character.hp)
            { Tool.ColorTxt(character.hp.ToString(), Tool.color3); }
            else
            { Tool.ColorTxt(character.hp.ToString(), Tool.color2); }
            Console.WriteLine();
            NextButton("다음", false);
        }
    }
}