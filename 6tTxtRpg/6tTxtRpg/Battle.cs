using _6tTxtRpg;
namespace _6TxtRpg
{
    class Battle // 전투기능 작업
    {
        MonsterList battleMon = new MonsterList();
        Random random = new Random();//공용으로 쓸 랜덤
        byte monNum = 0;//몬스터수. 다른 메서드에서 쓸거 같아서 뻄.
        int stage = 1;
        int copyStage;
        public int Stage
        {
            get { return stage; }
            set
            {
                if (value < 0)
                { value = 0; }
                stage = value;
            }
        }
        private bool isPractice_;
        bool isBattle = false; //전투상태인지 체크.
        bool isBoss = false;
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
            CharRun,
            Unknown,
        }
        private readonly Character character_;
        private MonsterList monsterList_;
        public Battle(Character character, MonsterList monsters) //반드시 받아야 되는 인자값을 위해 생성자로 만듬.
        {
            character_ = character;
            monsterList_ = monsters;
        }//RunBattle 메서드 작동.
        //처음에는 생성자에 로직을 냅다 다 넣었다가 전투가 필요한 부분에 넣기 쉽게 메서드로 뺐음.
        public void RunBattle(bool isPractice)//외부에서 막 쓰라고 public으로 처리했다.
        {
            isPractice_ = isPractice;
            MonSetting();
            startHp = character_.hp;//결과 화면 출력을 위해 현재 플레이어의 Hp를 변수에 저장했다.
            isBattle = true;//반복구문을 위한 bool값 재생.
            currentPhase = Phase.Waiting;//페이즈를 대기 페이즈로 세팅.
            Console.Clear();//맨 처음만 여기에서 한번 콘솔을 지워줌.
            Tool.ColorTxt($" {monNum}마리의 몬스터가 당신에게 덤벼든다!!", Tool.color2);//반복구문안에 들어가면 계속 뜰테니 밖으로 뺌.
            Console.WriteLine();
            Console.WriteLine();
            while (isBattle) //여기서부터 반복구문
            {
                if (currentPhase == Phase.CharATK)//플레이어 공격페이즈일때
                {
                    BattleMsg("Battle!!", Tool.color2); //글자 표시됨.
                    BuffList.UpdateBuff();
                }
                if (currentPhase != Phase.MonsterATK)//몬스터 공격페이즈가 아닐때
                {
                    if (currentPhase != Phase.CharATK && currentPhase != Phase.CharATKFin && currentPhase != Phase.Waiting)//플레이어 공격페이즈가 아닐때
                    { Console.WriteLine(); }//한줄 띄움(Battle!! 글자에 맞춰서 보기좋게 출력되도록 처리한거임.)
                    ShowMon();//몬스터 상태 출력
                    Console.WriteLine();
                    ShowChar();//플레이어 상태 출력
                }
                if (currentPhase == Phase.Waiting)//선택페이즈
                {
                    Tool.ColorTxt("1", Tool.color5);
                    Console.WriteLine(".공격");
                    Tool.ColorTxt("2", Tool.color5);
                    Console.WriteLine(".관찰");
                    Tool.ColorTxt("3", Tool.color5);
                    Console.WriteLine(".도망");
                    Console.WriteLine();
                    BattleMenuKey();
                }
                else if (currentPhase == Phase.CharATK)//공격페이즈
                {
                    Tool.ColorTxt("0", Tool.color5);
                    Console.WriteLine(".취소");
                    Console.WriteLine();
                    AtkMenu();
                }
                else if (currentPhase == Phase.CharATKFin)
                { NextButton("다음", "", Phase.MonsterATK); }
                else if (currentPhase == Phase.MonsterATK)
                {
                    MonATK();
                    currentPhase = Phase.Waiting;
                }
                MonDead(monNum);//몬스터가 죽었는지 확인
                if (character_.hp <= 0)//플레이어가 죽었는지 확인
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
                if (!isPractice)
                { ++Stage; }
                BattleResult();
            }
            else if (currentPhase == Phase.MonWin)
            {
                BattleMsg("You Lose", Tool.color2);
                BattleResult();
            }
            else if (currentPhase == Phase.CharRun)
            {
                Console.WriteLine($"{Tool.Josa(character_.name, "은", "는")} 열심히 도망갔다!");
                Console.WriteLine();
                BattleResult();
            }
            for (byte i = 0; i < battleMon.monsterList.Count(); ++i)//전투가 끝나면 리스트를 한번 순회한다.
            {
                if (battleMon.monsterList[i].isDead)
                { monsterList_.RemoveMonter(battleMon.monsterList[i]); }//hp가 0마만인건 싹 지운다.
            }
            battleMon.monsterList.Clear();//사용완료했으니 지우기.
        }
        void ShowMon()//몬스터 정보 출력 함수 //TODO:띄어쓰기 처리 안해놨음. 나중에 디버깅하면서 수정해야 됨.
        {
            for (int i = 0; i < monNum; ++i)
            {

                if (currentPhase == Phase.CharATK)
                { Tool.ColorTxt((i + 1).ToString(), Tool.color5); }
                Console.Write(" ");
                if (battleMon.monsterList[i].isDead)//사망시 Dead표시
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"LV.{battleMon.monsterList[i].level} {battleMon.monsterList[i].name} Hp Dead");//TODO:몬스터 레벨변수 생기면 주석해제
                    Console.ForegroundColor = Tool.color1;
                }
                else
                { battleMon.monsterList[i].ShortInfo(); }//배열에 넣은 몬스터 출력
            }
        }
        void MonSetting()
        {
            if (!isPractice_ || Stage == 1)
            { copyStage = Stage; }
            else if (isPractice_)
            { copyStage = Stage - 1; }
            if (copyStage <= 1)
            { monNum = (byte)random.Next(1, 2); }
            else if (copyStage <= 3 && Stage > 1)
            { monNum = (byte)random.Next(1, 3); }
            else if (copyStage <= 5 && Stage > 3)
            { monNum = (byte)random.Next(1, 4); }
            else if (copyStage <= 10 && Stage > 5)
            { monNum = (byte)random.Next(1, 5); }
            else if (copyStage <= 15 && Stage > 10)
            { monNum = (byte)random.Next(1, 6); }
            else if (copyStage <= 25 && Stage > 15)
            { monNum = (byte)random.Next(1, 7); }
            else if (copyStage <= 50 && Stage > 25)
            { monNum = (byte)random.Next(1, 8); }
            else if (copyStage > 50)
            { monNum = (byte)random.Next(1, 9); }

            //등장 몬스터 수 지정. 작은 수니까 byte로 처리했다. 0~3까지 계산.
            for (int i = 0; i < monNum; ++i)//몬스터 수만큼 반복.
            {
                if (!isPractice_ || monsterList_.monsterList.Count <= monNum * 2)
                { monsterList_.AddRandom(Stage); }
            }//몬스터의 메서드를 써서 랜덤으로 뽑힌 수 만큼 몬스터를 추가한다}
             //이부분 인수에 원하시는 level최소치를 넣어주세요 !!! level = 넣은인수 +0~2
            battleMon.monsterList = monsterList_.GetMonsters().ToList();//몬스터 리스트 복제.
            if (Stage % 5 == 0 && !isPractice_)
            {
                if (battleMon.monsterList.FirstOrDefault(mon => mon.name == "울부짖는 늑대왕") == null)
                { battleMon.AddMonster(new Monster.WolfKing()); }
                battleMon.monsterList = battleMon.monsterList.OrderByDescending(mon => mon.name == "울부짖는 늑대왕").ThenBy(mon => random.Next()).Take(monNum).ToList();
            }
            else
            { battleMon.monsterList = battleMon.monsterList.OrderBy(mon => mon.name == "울부짖는 늑대왕").ThenBy(mon => random.Next()).Take(monNum).ToList(); }
        }
        void ShowChar()//플레이어 정보출력 함수. 가독성을 위해 일단 뺐다.
        {
            character_.ShortInfo();
            if (currentPhase != Phase.CharATKFin)
            { Console.WriteLine(); }
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
            switch (Console.ReadKey().KeyChar) //숫자만 눌러도 작동하게 ReadKey로 처리했습니다.
            {
                case '1'://관찰키
                    currentPhase = Phase.CharATK;//몬스터 이름앞에 숫자가 나옴.
                    break;
                case '2'://공격키
                    Console.Clear();
                    for (int i = 0; i < monNum; ++i)
                    {
                        battleMon.monsterList[i].ShowInfo();
                        Console.WriteLine();
                    }
                    Console.WriteLine("아무키나 눌러주세요.");
                    Console.Write(">> ");
                    Console.ReadKey(true);
                    break;
                case '3'://도망
                    currentPhase = Phase.CharRun;
                    isBattle = false;
                    break;
                default:
                    WrongMsg();
                    break;
            }
        }
        void AtkMenu()//몬스터 공격 선택 함수
        {

            TypeMsg("대상을 선택해주세요.");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            char inputChar = keyInfo.KeyChar;
            if (int.TryParse(inputChar.ToString(), out int monIndex))
            {
                if (monIndex == 0)
                { currentPhase = Phase.Waiting; }
                else if (monIndex >= 1 && monIndex <= battleMon.monsterList.Count)
                { CharAtk(monIndex - 1); }
                else
                { WrongMsg(); }
            }
        }
        void CharAtk(int num)//몬스터 공격 판정용 함수
        {
            if (monNum >= num + 1) //몬스터가 있음.
            {
                if (battleMon.monsterList[num].isDead) //만약 몬스터가 죽었을 때
                { WrongMsg(); }
                else
                {
                    Console.Clear();
                    BattleMsg("Battle!!", Tool.color2);
                    float beforehit = battleMon.monsterList[num].hp;
                    int error = (int)Math.Ceiling((float)character_.damage * 0.1f); //오차범위 처리.
                    int charDamage = random.Next(character_.damage - error, character_.damage + error + 1);
                    Console.WriteLine($"{character_.name}의 공격!");
                    character_.PlayerCri();
                    Console.WriteLine();
                    Console.Write($"Lv.");
                    Tool.ColorTxt(battleMon.monsterList[num].level.ToString(), Tool.color4);
                    Console.Write($" {Tool.Josa(battleMon.monsterList[num].name, "을", "를")} 맞췄습니다. (데미지 : ");
                    Tool.ColorTxt(charDamage.ToString(), Tool.color2);
                    Console.Write(")");
                    Console.WriteLine();
                    Console.WriteLine();
                    battleMon.monsterList[num].Damaged(charDamage);
                    Console.WriteLine();
                    if (battleMon.monsterList[num].isDead)
                    { Console.WriteLine(); }
                    Console.Write($"Lv.");
                    Tool.ColorTxt(battleMon.monsterList[num].level.ToString(), Tool.color4);
                    Console.Write($" {battleMon.monsterList[num].name}");
                    Console.WriteLine();
                    Console.Write($"HP ");
                    Tool.ColorTxt(beforehit.ToString(), Tool.color4);
                    Console.Write(" -> ");
                    if (battleMon.monsterList[num].isDead)
                    {
                        Tool.ColorTxt("Dead", Tool.color2);
                        character_.exp += battleMon.monsterList[num].level;
                        Console.WriteLine();
                        character_.levelUp();
                    }
                    else
                    {
                        Console.Write($"Hp ");
                        Tool.ColorTxt(battleMon.monsterList[num].hp.ToString(), Tool.color2);
                        Console.WriteLine();
                    }
                    NextButton("다음", "", Phase.CharATKFin);
                }
            }
            else
            { WrongMsg(); }
        }
        void NextButton(string message, string typeMsg, Phase nextPhase)
        {
            Console.ReadKey(true);
            currentPhase = nextPhase;
        }
        void NextButton(string message, bool isMsgOn)
        {
            if (isMsgOn)
            {
                Console.WriteLine();
                TypeMsg("대상을 선택해주세요.");
            }
            else
            { Console.Write(">> "); }
            Console.ReadKey(true);
        }
        void MonATK()//몬스터 순서대로 공격 메서드
        {
            for (byte i = 0; i < monNum; i++)
            {
                if (!battleMon.monsterList[i].isDead)//안 죽었을때만 플레이어 공격.
                { MonAtkMsg(i); }
            }
        }
        void MonAtkMsg(byte num)//몬스터 공격시 나오는 메세지
        {
            int beforehit = character_.hp;
            Console.Clear();
            BattleMsg("Battle!!", ConsoleColor.DarkRed);
            Console.WriteLine($"{battleMon.monsterList[num].name}의 공격!");
            Console.WriteLine();
            battleMon.monsterList[num].RandomAttack(character_);
            Console.WriteLine();
            Console.Write($"Lv.");
            Tool.ColorTxt(character_.level.ToString(), Tool.color4);
            Console.WriteLine($" {character_.name}");
            Console.Write($"HP ");
            Tool.ColorTxt(beforehit.ToString(), Tool.color4);
            Console.Write(" -> ");
            //character.hp -= (int)monster.damage;
            if (character_.hp <= 0)
            { Tool.ColorTxt("Dead", Tool.color2); }
            else
            {
                Console.Write($"Hp ");
                Tool.ColorTxt(character_.hp.ToString(), Tool.color2);
            }
            Console.WriteLine();
            NextButton("다음", false);
        }
        void MonDead(byte num)
        {
            if (battleMon.monsterList.All(mon => mon.isDead))
            { WinEnd(); }
        }
        void WinEnd()
        {
            currentPhase = Phase.CharWin;
            isBattle = false;
        }
        void BattleResult()
        {
            BuffList.RemoveAllBuff();//버프 지우는 위치
            Console.Write($"LV. ");
            Tool.ColorTxt(character_.level.ToString(), Tool.color4);
            Console.WriteLine($" {character_.name}");
            Console.Write($"HP {startHp} -> ");
            if (startHp == character_.hp)
            { Tool.ColorTxt(character_.hp.ToString(), Tool.color3); }
            else
            { Tool.ColorTxt(character_.hp.ToString(), Tool.color2); }
            Console.WriteLine();
            if (currentPhase == Phase.MonWin)//임의로 졌을때 풀회복시켜서 내보냄.
            {
                character_.hp = character_.maxHp;
                character_.mp = character_.maxMp;
            }
            Console.WriteLine();
            NextButton("다음", false);
        }
    }
}