using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6tTxtRpg
{
    public static class Tool//유틸용 함수. Tool.메서드(); 로 쓰면 됨.
    {

        //색상 테마용
        internal static ConsoleColor color1 = ConsoleColor.White;//기본색상
        internal static ConsoleColor color2 = ConsoleColor.Red;//치명타 색상
        internal static ConsoleColor color3 = ConsoleColor.Green;//승리할 때 색상
        internal static ConsoleColor color4 = ConsoleColor.Yellow;//꾸미기용 색상
        internal static ConsoleColor color5 = ConsoleColor.Cyan;//선택숫자 색상
        public static void ColorTxt(string inputText, ConsoleColor inputColor) //단문을 특정색상으로 바꿔줌
        {
            Console.ForegroundColor = inputColor;
            Console.Write(inputText);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
