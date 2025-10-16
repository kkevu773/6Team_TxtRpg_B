using _6tTxtRpg;
using _6TxtRpg;

namespace _6tTxtRpg
{
    internal class OpenQuest
    {
        bool isQuestMenu = false;
        string[] QuestName = new string[]
{
            "마을을 위협하는 고블린 처치",
            "장비를 장착해보자",
            "더욱 더 강해지기!",
};
        List<Quest> QuestList = new List<Quest>()
        {
            new Quest("마을을 위협하는 고블린 처치",@"이봐! 마을 근처에 고블린들이 너무 많아졌다고 생각하지 않나?
마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!
모험가인 자네가 좀 처치해주게!", QuestType.Hunting,10, ItemPreset.itemList[0],1),
            new Quest("장비를 장착해보자",@"모험을 떠나기 전에 장비를 갖추는 건 기본 중의 기본이네! 
자네가 가진 장비 중 하나를 골라 착용해 보게. 
장비를 착용해야 비로소 모험가다운 모습을 갖출 수 있지 않겠나!",QuestType.Equip,10, ItemPreset.itemList[0],1),
            new Quest("더욱 더 강해지기!",@"지금의 레벨에 만족하면 더 이상 성장할 수 없네. 
더 깊은 던전과 더 강한 몬스터를 상대하기 위해서는 스스로 강해져야 해! 
끊임없이 수련하여 목표 레벨을 달성하게!",QuestType.LevelUp,10, ItemPreset.itemList[0],1)
            };
        public void ShowQuest()
        {
            isQuestMenu = true;
            while (isQuestMenu)
            { QuestMenu(); }
        }
        void QuestMenu()
        {
            Console.Clear();
            int i = 0;
            foreach (var quest in QuestList)
            {
                if (i > QuestList.Count)
                { return; }
                Tool.ColorTxt($"{i+1} ", Tool.color5);
                Console.WriteLine(QuestList[i].QuestName);
                ++i;
            }
            Console.WriteLine();
            Tool.ColorTxt($"0 ", Tool.color5);
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
                { return; }
                else
                {
                    Console.Clear();
                    Console.WriteLine(QuestList[questNum - 1].QuestName);
                    Console.WriteLine();
                    Console.WriteLine(QuestList[questNum - 1].QuestInfo);
                    Console.WriteLine();
                    Tool.ColorTxt($"1 ", Tool.color5);
                    Console.WriteLine("수락");
                    Tool.ColorTxt($"0 ", Tool.color5);
                    Console.WriteLine("뒤로 가기");
                    Console.ReadKey(true);
                }
            }
        }
    }
    public class Quest
    {
        string questName = "퀘스트 명";
        public string QuestName {get{return questName;} set{questName = value;} }
        string questInfo = @"퀘스트 내용";
        public string QuestInfo {get{return questInfo;} set{questInfo = value;} }
        QuestType questType = QuestType.Unknown;
        int require = 0;
        bool isFinish = false;
        Item rewardItem;
        int rewardAmount = 0;

        public Quest(string questName, string questInfo, QuestType questType, int require, Item rewardItem, int rewardAmount)
        {
            this.questName = questName;
            this.questInfo = questInfo;
            this.questType = questType;
            this.rewardItem = rewardItem;
            this.rewardAmount = rewardAmount;
        }
    }
    public enum QuestType
    {
        Hunting,
        Collecting,
        LevelUp,
        Equip,
        Unknown
    }
}
