using _6tTxtRpg;

namespace _6tTxtRpg
{
    internal class Quest
    {
        string[] questName = new string[]
{
            "마을을 위협하는 고블린 처치",
            "장비를 장착해보자",
            "더욱 더 강해지기!",
};
        string[] questInfo = new string[]
        {
            @"",
            @"",
            @"",
        };
        Dictionary<string, (string, int)> QuestDiction;
        public Quest()
        {
            QuestDiction = new Dictionary<string, (string, int)>()
            {
                { questName[0],(questInfo[0],10)},
                { questName[1],(questInfo[1],10)},
                { questName[2],(questInfo[2],10)},
            };
        }
        public void ShowQuest()
        {
            Console.Clear();
            int i = 1;
            foreach (var quest in QuestDiction)
            {
                Tool.ColorTxt($"{i} ",Tool.color5);
                Console.WriteLine(quest.Key);
                ++i;
            }
            Console.ReadKey();
        }
    }
}
