using _6tTxtRpg;
using _6TxtRpg;

namespace _6TxtRpg
{
    class Battle // 전투기능 작업
    {
        List<Monster> battleMon = new List<Monster>();
        Random random = new Random();//공용으로 쓸 랜덤
        byte monNum = 0;//몬스터수. 다른 메서드에서 쓸거 같아서 뻄.
        bool isBattle = false; //전투상태인지 체크.
        Phase currentPhase = Phase.Unknown;//페이즈 체크용 변수
        int startHp;//데미지 깎기 전 Hp를 저장하기 위한 변수.
        enum Phase//bool로 처리하다가 너무 많아질거 같아서 enum으로 바꿨음.
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
        { RunBattle(character, monsters); }//RunBattle 메서드 작동.
        //처음에는 생성자에 로직을 냅다 다 넣었다가 전투가 필요한 부분에 넣기 쉽게 메서드로 뺐음.
        public void RunBattle(Character character, MonsterList monsters)//외부에서 막 쓰라고 public으로 처리했다.
        {
            Console.ForegroundColor = Tool.color1;//기본텍스트 색상 처리. Tool 클래스의 변수를 활용해서 변수만 바꿔도 관련된 부분의 색상이 전부 바뀌게 처리했다. 
            //monsters.monsterList.Clear();//그냥 쓰면 몬스터 리스트에 몬스터가 계속 쌓일 수 있으니까 한번 전부 지운다. 
            monNum = (byte)random.Next(1, 5);//등장 몬스터 수 지정. 작은 수니까 byte로 처리했다. 0~3까지 계산.
            for (int i = 0; i < monNum; ++i)//몬스터 수만큼 반복.
            { monsters.AddRandom(); }//몬스터의 메서드를 써서 랜덤으로 뽑힌 수 만큼 몬스터를 추가한다.
            battleMon = monsters.GetMonsters();
            battleMon = battleMon.OrderBy(Mon => random.Next()).ToList();//리스트를 한번 섞어준다 쉐킷쉐킷
            startHp = character.hp;//결과 화면 출력을 위해 현재 플레이어의 Hp를 변수에 저장했다.
            isBattle = true;//반복구문을 위한 bool값 재생.
            currentPhase = Phase.Waiting;//페이즈를 대기 페이즈로 세팅.
            Console.Clear();//맨 처음만 여기에서 한번 콘솔을 지워줌.
            Tool.ColorTxt($" {monNum}마리의 몬스터가 당신에게 덤벼든다!!", Tool.color2);//반복구문안에 들어가면 계속 뜰테니 밖으로 뺌.
            Console.WriteLine();
            Console.WriteLine();
            while (isBattle) //여기서부터 반복구문
            {
                if (currentPhase == Phase.CharATK)//플레이어 공격페이즈일때
                { BattleMsg("Battle!!", Tool.color2); }//글자 표시됨.
                if (currentPhase != Phase.MonsterATK)//몬스터 공격페이즈가 아닐때
                {
                    if (currentPhase != Phase.CharATK && currentPhase != Phase.CharATKFin && currentPhase != Phase.Waiting)//플레이어 공격페이즈가 아닐때
                    { Console.WriteLine(); }//한줄 띄움(Battle!! 글자에 맞춰서 보기좋게 출력되도록 처리한거임.)
                    ShowMon(battleMon);//몬스터 상태 출력
                    Console.WriteLine();
                    ShowChar(character);//플레이어 상태 출력
                }
                if (currentPhase == Phase.Waiting)//선택페이즈
                {
                    Tool.ColorTxt("1", Tool.color5);
                    Console.WriteLine(". 관찰");
                    Tool.ColorTxt("2", Tool.color5);
                    Console.WriteLine(". 공격");
                    Console.WriteLine();
                    BattleMenuKey();
                }
                else if (currentPhase == Phase.CharATK)//공격페이즈
                {
                    Tool.ColorTxt("0", Tool.color5);
                    Console.WriteLine(". 취소");
                    Console.WriteLine();
                    AtkMenu(character, battleMon);
                }
                else if (currentPhase == Phase.CharATKFin)
                { NextButton("다음", "", Phase.MonsterATK); }
                else if (currentPhase == Phase.MonsterATK)
                {
                    MonATK(character, battleMon);
                    currentPhase = Phase.Waiting;
                }
                MonDead(battleMon, monNum);//몬스터가 죽었는지 확인
                if (character.hp <= 0)//플레이어가 죽었는지 확인
                {
                    currentPhase = Phase.MonWin;
                    isBattle = false;
                }
                Console.Clear();//반복구문 끝날때마다 삭제
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
            for (int i = 0; i < battleMon.Count(); ++i)//전투가 끝나면 리스트를 한번 순회한다.
            {
                if (battleMon[i].isDead)
                { monsters.RemoveMonter(battleMon[i]); }//hp가 0마만인건 싹 지운다.
            }
            battleMon.Clear();//사용완료했으니 지우기.
        }
        void ShowMon(List<Monster> monsters)//몬스터 정보 출력 함수 //TODO:띄어쓰기 처리 안해놨음. 나중에 디버깅하면서 수정해야 됨.
        {
            for (int i = 0; i < monNum; ++i)
            {

                if (currentPhase == Phase.CharATK)
                { Tool.ColorTxt((i + 1).ToString(), Tool.color5); }
                Console.Write(" ");
                if (monsters[i].isDead)//사망시 Dead표시
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"LV.{monsters[i].level} {monsters[i].name} Hp Dead");//TODO:몬스터 레벨변수 생기면 주석해제
                    Console.ForegroundColor = Tool.color1;
                }
                else
                { monsters[i].ShortInfo(); }//배열에 넣은 몬스터 출력
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
                case ConsoleKey.D1://관찰키
                    Console.Clear();
                    for (int i = 0; i < monNum; ++i)
                    {
                        battleMon[i].ShowInfo();
                        Console.WriteLine();
                    }
                    Console.WriteLine("아무키나 눌러주세요.");
                    Console.Write(">> ");
                    Console.ReadKey(true);
                    break;
                case ConsoleKey.D2://공격키
                    currentPhase = Phase.CharATK;//몬스터 이름앞에 숫자가 나옴.
                    break;
                default:
                    WrongMsg();
                    break;
            }
        }
        void AtkMenu(Character character, List<Monster> monsters)//몬스터 공격 선택 함수
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
        void CharAtk(Character character, List<Monster> monsters, byte num)//몬스터 공격 판정용 함수
        {
            if (monNum >= num + 1) //몬스터가 있음.
            {
                if (monsters[num].isDead) //만약 몬스터가 죽었을 때
                { WrongMsg(); }
                else
                {
                    Console.Clear();
                    BattleMsg("Battle!!", Tool.color2);
                    float beforehit = monsters[num].hp;
                    int error = (int)Math.Ceiling((float)character.damage * 0.1f); //오차범위 처리.
                    int charDamage = random.Next(character.damage - error, character.damage + error + 1);
                    Console.WriteLine($"{character.name}의 공격!");
                    Console.WriteLine();
                    Console.Write($"Lv.");
                    Tool.ColorTxt(monsters[num].level.ToString(), Tool.color4);
                    Console.Write($" {Tool.Josa(monsters[num].name, "을", "를")} 맞췄습니다. (데미지 : ");
                    Tool.ColorTxt(charDamage.ToString(), Tool.color2);
                    Console.Write(")");
                    Console.WriteLine();
                    Console.WriteLine();
                    monsters[num].Damaged(charDamage);
                    Console.WriteLine();
                    Console.Write($"Lv.");
                    Tool.ColorTxt(monsters[num].level.ToString(), Tool.color4);
                    Console.Write($" {monsters[num].name}");
                    Console.WriteLine();
                    Console.Write($"HP ");
                    Tool.ColorTxt(beforehit.ToString(), Tool.color4);
                    Console.Write(" -> ");
                    if (monsters[num].isDead)
                    {
                        Tool.ColorTxt("Dead", Tool.color2);
                        monsters[num].DropItem();//죽을때 아이템 드롭
                    }
                    else
                    {
                        Console.Write($"Hp ");
                        Tool.ColorTxt(monsters[num].hp.ToString(), Tool.color2);
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
        void MonATK(Character character, List<Monster> monsters)//몬스터 순서대로 공격 메서드
        {
            for (int i = 0; i < monNum; i++)
            {
                if (!monsters[i].isDead)//안 죽었을때만 플레이어 공격.
                { MonAtkMsg(character, monsters[i]); }
            }
        }
        void MonAtkMsg(Character character, Monster monster)//몬스터 공격시 나오는 메세지
        {
            int beforehit = character.hp;
            Console.Clear();
            BattleMsg("Battle!!", ConsoleColor.DarkRed);
            Console.WriteLine($"{monster.name}의 공격!");
            Console.WriteLine();
            monster.RandomAttack(character);
            //ㄴConsole.WriteLine();
            //Console.Write($"Lv.{character.level} {Tool.Josa(character.name, "을", "를")} 맞췄습니다. (데미지 : ");
            //Tool.ColorTxt(monster.damage.ToString(), Tool.color2);
            Console.Write(")");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"Lv.{character.level} {character.name}");
            Console.Write($"HP ");
            Tool.ColorTxt(beforehit.ToString(), Tool.color4);
            Console.Write(" -> ");
            //character.hp -= (int)monster.damage;
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
        void MonDead(List<Monster> monsters, int num)
        {
            if (num <= 1)
            {
                if (monsters[0].hp <= 0)
                {
                    WinEnd();
                }
            }
            else if (num <= 2)
            {
                if (monsters[0].hp <= 0 && monsters[1].hp <= 0)
                {
                    WinEnd();
                }
            }
            else if (num <= 3)
            {
                if (monsters[0].hp <= 0 && monsters[1].hp <= 0 && monsters[2].hp <= 0)
                {
                    WinEnd();
                }
            }
            else if (num <= 4)
            {
                if (monsters[0].hp <= 0 && monsters[1].hp <= 0 && monsters[2].hp <= 0 && monsters[3].hp <= 0)
                {
                    WinEnd();
                }
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
            Console.WriteLine();
            NextButton("다음", false);
        }
    }
}