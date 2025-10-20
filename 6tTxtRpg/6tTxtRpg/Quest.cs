using _6TxtRpg;
using System.Reflection.Metadata.Ecma335;

namespace _6tTxtRpg
{
    public static class OpenQuest
    {
        static bool isQuestMenu = false;
        static bool isOn = false;
        internal static List<Quest> QuestList = new List<Quest>()
        {
           new Quest(
        "마을을 위협하는 고블린 처치",
        @"이봐! 마을 근처에 고블린들이 너무 많아졌다고 생각하지 않나?
마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!
모험가인 자네가 좀 처치해주게!",
"고블린 5마리 처치",
"쓸만한 방패",
"5 G",
        5, // 목표 개수 (고블린 5마리)
        ItemPreset.testItemList[0], // 보상 아이템 (임시)
        1, // 보상 아이템 개수
        5 //골드
    ),

    // 2. 장비를 장착해보자
    new Quest(
        "장비를 장착해보자",
        @"모험을 떠나기 전에 장비를 갖추는 건 기본 중의 기본이네! 
자네가 가진 장비 중 하나를 골라 착용해 보게. 
장비를 착용해야 비로소 모험가다운 모습을 갖출 수 있지 않겠나!",
"장비 1개 착용",
"소형 체력 포션",
"10 G",
        1, // 목표 개수 (장비 1개 착용)
        ItemPreset.testItemList[1], // 보상 아이템 (임시)
        3, // 보상 아이템 개수
        10//골드
    ),

    // 3. 더욱 더 강해지기!
    new Quest(
        "더욱 더 강해지기!",
        @"지금의 레벨에 만족하면 더 이상 성장할 수 없네. 
더 깊은 던전과 더 강한 몬스터를 상대하기 위해서는 스스로 강해져야 해! 
끊임없이 수련하여 목표 레벨을 달성하게!",
"레벨 5 달성",
"소형 마나 포션",
"50 G",
        5, // 목표 개수 (레벨 5 달성)
        ItemPreset.testItemList[2], // 보상 아이템 (임시)
        1, // 보상 아이템 개수
        50
    )
            };
        public static void ShowQuest()
        {
            isQuestMenu = true;
            while (isQuestMenu)
            { QuestMenu(); }
        }
        static void QuestMenu()
        {
            Console.Clear();
            int index = 0;
            foreach (var quest in QuestList)
            {
                if (index > 10)
                { return; }
                Tool.ColorTxt($"{index + 1} ", Tool.cyan);
                Console.Write(QuestList[index].QuestName);
                if (QuestList[index].IsStart && QuestList[index].CurrentRequire >= QuestList[index].Require)
                { Tool.ColorTxt(" - 완료 가능", Tool.red); }
                else if (QuestList[index].IsStart)
                { Tool.ColorTxt(" - 진행 중", Tool.yellow); }
                Console.WriteLine();
                ++index;
            }
            Console.WriteLine();
            Tool.ColorTxt($"0 ", Tool.cyan);
            Console.WriteLine("뒤로 가기");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            char inputChar = keyInfo.KeyChar;

            if (int.TryParse(inputChar.ToString(), out int questNum))
            {
                if (questNum == 0)
                {
                    isQuestMenu = false;
                    return;
                }
                else if (questNum > QuestList.Count)
                {
                    Console.WriteLine($">> {questNum}");
                    Tool.WrongMsg();
                    Console.ReadKey(true);
                }
                else
                {
                    isOn = true;
                    while (isOn)
                    {
                        Console.Clear();
                        Tool.ColorTxt($" - {QuestList[questNum - 1].QuestName}", Tool.yellow);
                        if (QuestList[questNum - 1].IsStart && QuestList[questNum - 1].CurrentRequire >= QuestList[questNum - 1].Require)
                        { Tool.ColorTxt(" - 완료 가능", Tool.red); }
                        else if (QuestList[questNum - 1].IsStart)
                        { Tool.ColorTxt(" - 진행 중", Tool.red); }
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine(QuestList[questNum - 1].QuestInfo);
                        Console.WriteLine();
                        Tool.ColorTxt($"목표 : {QuestList[questNum - 1].QuestRequire} ({QuestList[questNum - 1].CurrentRequire}/{QuestList[questNum - 1].Require})", Tool.yellow);
                        Console.WriteLine();
                        Console.WriteLine();
                        Tool.ColorTxt($"보상 : \n\n    {QuestList[questNum - 1].QuestReward} X {QuestList[questNum - 1].RewardAmount}\n    {QuestList[questNum - 1].QuestReward2}", Tool.cyan);
                        Console.WriteLine();
                        Console.WriteLine();
                        if (!QuestList[questNum - 1].IsStart)
                        {
                            Tool.ColorTxt($"1 ", Tool.cyan);
                            Console.WriteLine("수락");
                        }
                        else if (QuestList[questNum - 1].IsStart)
                        {
                            Tool.ColorTxt($"1 ", Tool.cyan);
                            Console.WriteLine("완료");
                        }
                        Tool.ColorTxt($"0 ", Tool.cyan);
                        Console.WriteLine("뒤로 가기");
                        string questMenu = Console.ReadKey(true).KeyChar.ToString();
                        switch (questMenu)
                        {
                            case "1":
                                if (!QuestList[questNum - 1].IsStart)
                                {
                                    QuestList[questNum - 1].IsStart = true;
                                }
                                else if (QuestList[questNum - 1].IsFinish)
                                {
                                    //TODO:보상템 맞춰서 인벤에 넣어야 되는데 함수를 모름.
                                    //QuestList[questNum - 1].RewardItem;
                                    for (int i = 0; i < QuestList[questNum - 1].RewardAmount; ++i)
                                    { Inventory.GetItem(QuestList[questNum - 1].RewardItem); }
                                    TxtR.player.gold += QuestList[questNum - 1].RewardGold;
                                    Console.WriteLine();
                                    Tool.ColorTxt($"{Tool.Josa(QuestList[questNum - 1].QuestName, "을", "를")} 완료했습니다.\n{QuestList[questNum - 1].QuestReward} {Tool.Josa($"{QuestList[questNum - 1].RewardAmount}개", "과", "와")} {Tool.Josa(QuestList[questNum - 1].QuestReward2, "을", "를")} 얻었습니다.", Tool.yellow);
                                    Console.ReadKey(true);
                                    isOn = false;
                                    QuestList.RemoveAt(questNum - 1);
                                }
                                else
                                {
                                    Console.WriteLine();
                                    Tool.ColorTxt($"아직 조건이 완료되지 않았습니다. ", Tool.red);
                                    Console.WriteLine();
                                    Console.ReadKey(true);
                                }
                                break;
                            case "0":
                                isOn = false;
                                break;
                            default:
                                Console.WriteLine($">> {questMenu}");
                                Tool.WrongMsg();
                                break;
                        }
                    }
                }
            }
        }
    }
    class Quest
    {
        string questName = "퀘스트 명";
        public string QuestName { get { return questName; } set { questName = value; } }
        string questInfo = @"퀘스트 내용";
        public string QuestInfo { get { return questInfo; } set { questInfo = value; } }
        string questRequire = @"완료 조건";
        public string QuestRequire { get { return questRequire; } set { questRequire = value; } }
        string questReward = @"보상조건";
        public string QuestReward { get { return questReward; } set { questReward = value; } }
        string questReward2 = @"보상조건2";
        public string QuestReward2 { get { return questReward2; } set { questReward2 = value; } }
        int require = 0;
        public int Require { get { return require; } }
        int currentRequire = 0;
        public int CurrentRequire { get { return currentRequire; } set { currentRequire = value; } }
        bool isStart = false;
        public bool IsStart { get { return isStart; } set { isStart = value; } }
        bool isFinish = false;
        public bool IsFinish { get { return isFinish; } set { isFinish = value; } }
        Item rewardItem;
        public Item RewardItem { get { return rewardItem; } }
        int rewardAmount = 0;
        public int RewardAmount { get { return rewardAmount; } }
        int rewardGold = 0;
        public int RewardGold { get { return rewardGold; } }

        public Quest(string questName, string questInfo, string questRequire, string questReward, string questReward2, int require, Item rewardItem, int rewardAmount, int rewardGold)
        {
            this.questName = questName;
            this.questInfo = questInfo;
            this.questRequire = questRequire;
            this.require = require;
            this.questReward = questReward;
            this.questReward2 = questReward2;
            this.rewardItem = rewardItem;
            this.rewardAmount = rewardAmount;
            this.rewardGold = rewardGold;
        }

        public void Trigger()
        {
            ++currentRequire;
            Console.WriteLine();
            Console.WriteLine($"{QuestName} ({currentRequire}/{Require})");
            if (CurrentRequire >= Require)
            {
                Console.WriteLine($"{Tool.Josa(QuestName, "을", "를")} 완료했습니다.");
                IsFinish = true;
            }
            Console.WriteLine();
        }
    }
}
