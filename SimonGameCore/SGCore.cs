using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SimonGame
{
    public static class SGCore
    {
        private static int btnNum = -1;


        private static int level = 1;              //Game Level Status indicator. starts at zero and increments as game progresses
        private static int state = 0;              //actual game stage indicator, selects which slot to test against within the list
        private static int highScore = 0;//TODO Make Property  //Local save state high score to the game

        private static List<int> answers = new List<int>();        //List of correct button sequence
        //private static DispatcherTimer LightTimer = new DispatcherTimer();  //Timer for Button Lighting sequence to show level
        private static int lightSlot = 0;              //State storage for lighting indicator
        private static bool lightOn = false;            //Boolean value to enable the Lighting tester
        private static bool cursorTracking = true; //TODO: Make property

        //Boolean value to determine if the cursor lighting of the buttons should fire
        public static void ColorSelected(string input)
        {
            switch (input)
            {
                case "Green":
                    btnNum = 0;
                    break;
                case "Blue":
                    btnNum = 1;
                    break;
                case "Red":
                    btnNum = 2;
                    break;
                case "Yellow":
                    btnNum = 3;
                    break;
                default:
                    btnNum = -1;
                    break;
            }
        }
        public static string InitializeGame()
        {
            string debugTextAnswer = String.Empty;
            Random randint = new Random();
            for (int i = 0; i < 10; i++)
            {
                answers.Add(randint.Next(0, 4));
                debugTextAnswer += answers[i];
            }
                     
            cursorTracking = false;
            return debugTextAnswer;
        }
        public static void ResetGame()
        {
            if (highScore < level)
            {
                highScore = level;
                
            }
            
            cursorTracking = true;
            list_Clearer();
            state = 0;
            level = 0;
        }
        private static void list_Clearer()
        {
            answers.Clear();
            answers.TrimExcess();
        }
    }
}
