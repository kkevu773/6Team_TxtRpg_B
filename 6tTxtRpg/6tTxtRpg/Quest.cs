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
            @"이봐! 마을 근처에 고블린들이 너무 많아졌다고 생각하지 않나?
마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!
모험가인 자네가 좀 처치해주게!",
            @"모험을 떠나기 전에 장비를 갖추는 건 기본 중의 기본이네! 
자네가 가진 장비 중 하나를 골라 착용해 보게. 
장비를 착용해야 비로소 모험가다운 모습을 갖출 수 있지 않겠나!",
            @"지금의 레벨에 만족하면 더 이상 성장할 수 없네. 
더 깊은 던전과 더 강한 몬스터를 상대하기 위해서는 스스로 강해져야 해! 
끊임없이 수련하여 목표 레벨을 달성하게!",
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
