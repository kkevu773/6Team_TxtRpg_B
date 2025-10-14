using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;
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
        enum Phase
        {
            Waiting,
            CharATK,
            CharATKFin,
            MonsterATK,
            Unknown,
        }
        public Battle(Character character, List<Monster> monsters) //반드시 받아야 되는 인자값을 위해 생성자로 만듬.
        { //몬스터 1~4마리의 정보가 필요할 것 같다.
            monNum = (byte)random.Next(0, 4);//등장 몬스터 수. 작은 수니까 byte로 처리했다. 0~3까지 계산.
            for (int i = 0; i >= monNum; ++i)//몬스터 수만큼 반복.
            {
                monsters.OrderBy(monsters => random.Next());//몬스터 리스트 한번 섞어줌.
                fightMonsters.Add(monsters[0]);//0번 몬스터만 리스트에 추가한다.
            }
            isBattle = true;
            currentPhase = Phase.Waiting;
            while (isBattle) //여기서부터 반복구문
            {
                if (currentPhase == Phase.CharATKFin || currentPhase ==  Phase.MonsterATK)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Battle!!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                ShowMon();
                ShowChar(character);
                if (currentPhase == Phase.Waiting)//선택페이즈
                {
                    Console.WriteLine("1. 공격");
                    BattleMenuKey();
                }
                else if (currentPhase == Phase.CharATK)//공격페이즈
                {
                    AtkMenu(character);
                    currentPhase = Phase.CharATKFin;
                }
                else if (currentPhase == Phase.CharATKFin)
                {
                    Console.WriteLine("0.취소");
                    Console.ReadKey(true); // 그냥 아무키나 눌러도 되면 되는거 아냐?
                    currentPhase = Phase.MonsterATK;
                }
            }
        }
        void WrongMsg()//잘못된 키 입력시 나오는 함수
        { Console.WriteLine("잘못된 입력입니다"); }//키 입력이 잘못될 시 나오는 메세지

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
                    //Console.WriteLine($"LV.{fightMonsters[i].level} {fightMonsters[i].name} Hp Dead");//TODO:몬스터 레벨변수 생기면 주석해제
                    Console.ForegroundColor = ConsoleColor.White;
                }
                //Console.WriteLine($"LV.{fightMonsters[i].level} {fightMonsters[i].name} Hp {fightMonsters[i].hp}");//배열에 넣은 몬스터 출력
            }
        }
        void ShowChar(Character character)//플레이어 정보출력 함수. 가독성을 위해 일단 뺐다.
        {
            Console.WriteLine($"LV.{character.level} {character.name} ({character.job}");
            Console.WriteLine($"Hp {character.hp} / {character.hp}");
            Console.WriteLine();
        }
        void BattleMenuKey()//배틀 매뉴 함수. 역시 가독성을 위해 뺐다.
        {
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
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    CharAtk(character,0);
                    break;
                case ConsoleKey.D2:
                    CharAtk(character,1);
                    break;
                case ConsoleKey.D3:
                    CharAtk(character,2);
                    break;
                case ConsoleKey.D4:
                    CharAtk(character,3);
                    break;
                default:
                    WrongMsg();
                    break;
            }
        }

        void CharAtk(Character character, int num)//몬스터 공격 판정용 함수
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
                    fightMonsters[num].hp -= random.Next(character.damage - error, character.damage + error + 1);
                }
            }
        }
        void MonsterATK(Character character)
        {
            for(int i = 0; i < monNum ; i++)
            {
                character.hp -= (int)fightMonsters[i].damage;
            }
        }
    }
}