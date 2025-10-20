using _6tTxtRpg;
namespace _6tTxtRpg
{
    public static class Tool//유틸용 함수. Tool.메서드();나 Tool.변수명; 으로 쓰면 됨.
    {
        //색상 테마용
        public static ConsoleColor white = ConsoleColor.White;//기본색상
        public static ConsoleColor red = ConsoleColor.Red;//치명타 색상
        public static ConsoleColor green = ConsoleColor.Green;//승리할 때 색상
        public static ConsoleColor yellow = ConsoleColor.Yellow;//꾸미기용 색상
        public static ConsoleColor cyan = ConsoleColor.Cyan;//선택숫자 색상
        public static void ColorTxt(string inputText, ConsoleColor inputColor) //단문을 특정색상으로 바꿔줌
        {
            Console.ForegroundColor = inputColor;
            Console.Write(inputText);
            Console.ForegroundColor = white;
        }
        public static void WrongMsg()//잘못된 키 입력시 나오는 함수
        {
            Console.WriteLine("잘못된 입력입니다");
            Console.ReadKey(true);
        }//키 입력이 잘못될 시 나오는 메세지
        public static string Josa(string word, string particleWith, string particleWithout)
        {//조사 처리용 메서드입니다.Tool.Josa(변수.Tostring(),"을","를"); 이렇게 쓰시면 됩니다.
;            if (string.IsNullOrEmpty(word))
            {
                return "";
            }
            // 마지막 글자(단어의 종성)를 확인
            char lastChar = word[word.Length - 1];

            // 한글의 유니코드 범위 확인
            if (lastChar < 0xAC00 || lastChar > 0xD7A3)
            {
                // 한글이 아닌 경우 (특수문자, 영어 등)는 받침이 없는 것으로 간주하고 처리
                return word + particleWithout;
            }

            // 1. 유니코드 한글 값에서 "가"의 코드값 (AC00)을 빼서 상대적인 위치를 구함
            int uniVal = lastChar - 0xAC00;

            // 2. 종성(받침)을 결정하는 공식: uniVal % 28
            // 0이면 받침이 없음
            int jongSung = uniVal % 28;

            if (jongSung == 0)
            {
                // 종성이 없음 (받침 없음) -> particleWithout 사용
                return word + particleWithout;
            }
            else
            {
                // 종성이 있음 (받침 있음) -> particleWith 사용
                return word + particleWith;
            }
        }
    }
}
