using _6tTxtRpg;
namespace _6tTxtRpg
{
    public class Buff
    {
        public string Name { get; set; }
        public Status EffectStatus { get; set; }
        public int EffectNum { get; set; }
        public int RemainTurn { get; set; }

        public Buff(string name, Status effectStatus, int effectNum, int remainTurn)
        {
            Name = name;
            EffectStatus = effectStatus;
            EffectNum = effectNum;
            RemainTurn = remainTurn;
        }
        
        public void StatUp() // 실제 수치를 조정하면 버그가 날 확률 높아짐. (피드백)
        {
            switch(EffectStatus)
            {
                case Status.Hp:
                    TxtR.player.hp += EffectNum;
                    if (TxtR.player.hp > TxtR.player.maxHp) TxtR.player.hp = TxtR.player.maxHp;
                    break;
                case Status.Mp:
                    TxtR.player.mp += EffectNum;
                    if (TxtR.player.mp > TxtR.player.maxMp) TxtR.player.mp = TxtR.player.maxMp;
                    break; 
                case Status.Exp:
                    // 여기에 케릭터 경험치 획득 메소드 연결
                    break;
                case Status.Level:
                    // 사용 미정
                    break;
                case Status.Atk:
                    TxtR.player.damage += EffectNum;
                    break;
                case Status.Def:
                    TxtR.player.defense += EffectNum;
                    break;
                case Status.BonusMp:
                    TxtR.player.maxMp += EffectNum;
                    break;
                case Status.BonusHp:
                    TxtR.player.maxHp += EffectNum;
                    break;
                case Status.BonusAtk:
                    // 미구현
                    break;
                case Status.BonusDef:
                    //
                    // 미구현
                    break;
            }
        }

        public void StatDown()
        {
            switch (EffectStatus)
            {
                case Status.Hp:
                    TxtR.player.hp -= EffectNum;
                    break;
                case Status.Mp:
                    TxtR.player.mp -= EffectNum;
                    break;
                case Status.Exp:
                    // 여기에 캐릭터 경험치 획득 메소드 연결
                    break;
                case Status.Level:
                    // 사용 미정
                    break;
                case Status.Atk:
                    TxtR.player.damage -= EffectNum;
                    break;
                case Status.Def:
                    TxtR.player.defense -= EffectNum;
                    break;
                case Status.BonusMp:
                    TxtR.player.maxMp -= EffectNum;
                    break;
                case Status.BonusHp:
                    TxtR.player.maxHp -= EffectNum;
                    break;
                case Status.BonusAtk:
                    // 미구현
                    break;
                case Status.BonusDef:
                    //
                    // 미구현
                    break;
            }
        }
        public void IncreaseTurn()
        {
            RemainTurn++;
            if(RemainTurn > 99) RemainTurn = 99;
        }

        public void DecreaseTurn()
        {
            RemainTurn--;
            if(RemainTurn <= 0) RemoveBuff();
        }

        public void RemoveBuff()
        {
            StatDown();
            BuffList.buffList.Remove(this);
        }
    }

    public class BuffList
    {
        public static List<Buff> buffList = new List<Buff>();

        public static void GetBuff(Item item)
        {
            Buff newBuff = new Buff(item.Name, item.EffectStatus, item.EffectNum, item.Enchant);
            bool sameBuff = false;
            
            for(int i = 0; i < BuffList.buffList.Count; i++)
            {
                if (BuffList.buffList[i].Name ==item.Name)
                {
                    BuffList.buffList[i].RemainTurn += item.Enchant;
                    sameBuff = true;
                    break;
                }
            }
            if (!sameBuff)
            {
                BuffList.buffList.Add(newBuff);
                newBuff.StatUp();
            }
        }

        public static void RemoveAllBuff()
        {
            for (int i = BuffList.buffList.Count - 1; i >= 0; i--)
            {
                BuffList.buffList[i].RemoveBuff();
            }
        }

        public static void UpdateBuff()
        {
            for (int i = BuffList.buffList.Count - 1; i >= 0; i--)
            {
                BuffList.buffList[i].DecreaseTurn();
            }
        }

        public static void PrintBuff()
        {
            Console.WriteLine("=======================버프=======================");
            foreach(Buff buff in BuffList.buffList)
            {
                string outLine = ($"[{buff.Name}/{buff.EffectStatus} +{buff.EffectNum} ({buff.RemainTurn})]");
                Console.WriteLine(outLine);
            }
            Console.WriteLine("==================================================");
        }
    }
}
