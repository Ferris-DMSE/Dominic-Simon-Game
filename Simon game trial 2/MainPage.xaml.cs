using System;
using System.Collections.Generic;
using System.IO;
using Windows.Media;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace simon_game_trial_2
{
    /// <summary>
    /// main code repository for base Simon Game
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region main_variables
        //______________________________________Variable Definitions__________________________________________________

        private int Level = 1;              //Game Level Status indicator. starts at zero and increments as game progresses
        private int State = 0;              //actual game stage indicator, selects which slot to test against within the list
        private int highScore = 0;              //Local save state high score to the game
        private int btnNum = -1;             //Test holding variable to define button pressed
        private List<int> answers = new List<int>();        //List of correct button sequence
        private DispatcherTimer LightTimer = new DispatcherTimer();  //Timer for Button Lighting sequence to show level
        private int LightSlot = 0;              //State storage for lighting indicator
        private bool LightOn = false;            //Boolean value to enable the Lighting tester
        private bool cursorTracking = true;     //Boolean value to determine if the cursor lighting of the buttons should fire
        private int tick = 0;                  //i is timer tick counter, to prevent the timer from firing too slowly or too quickly
        #endregion

        #region button_Presses
        private void Color_Click(object sender, TappedRoutedEventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Content.ToString())
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
            Lighting_off();
            bool status = answercheck();
            levelAdjuster(status);
        }

        private void Start_button(object sender, RoutedEventArgs e)
        {
            Random randint = new Random();
            for (int i = 0; i < 10; i++)
            {
                answers.Add(randint.Next(0, 4));
                textBox.Text += answers[i];
            }
            LightTimer.Start();
            cursorTracking = false;
        }

        private void stop_button_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }
        #endregion

        #region modifier_classes
        private void Reset()
        {
            if (highScore < Level)
            {
                highScore = Level;
                Highscore.Text = "High Score: " + highScore;
            }
            textBox.Text = "";
            cursorTracking = true;
            list_Clearer();
            State = 0;
            Level = 0;
        }

        public MainPage()
        {
            this.InitializeComponent();
            initializeTimer(); // Do this somewhere else. Maybe on start?
        }

        private void System_Reset()
        {
            //establishes base content settings that will be used whenever it is time to reset content
            textBox.Text = "";
            Lighting_off();
            State = 0;
            Level = 1;
            list_Clearer();
            Lose.Play();
        }

        private bool answercheck()
        {
            bool isEmpty = !answers.Any();
            if (isEmpty)
            {
                return false;
            }
            else
            {
                if (btnNum == answers[State])
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        private void levelAdjuster(bool Status)
        {
            Random randint = new Random();

            if (Status)
            {
                if (State < Level)
                {
                    State++;
                }
                else if (State == Level)
                {
                    answers.Add(randint.Next(0, 4));
                    State = 0;
                    Level++;
                    textBox.Text += answers[Level];
                    Lighting_off();
                    cursorTracking = false;
                }
                else
                {
                    Reset();
                }
            }
            else
            {
                Reset();
            }
        }

        private void list_Clearer()
        {
            answers.Clear();
            answers.TrimExcess();
        }

        private void Lighting_off()
        {
            Green.Background = new SolidColorBrush(Colors.Gray);
            Blue.Background = new SolidColorBrush(Colors.Gray);
            Red.Background = new SolidColorBrush(Colors.Gray);
            Yellow.Background = new SolidColorBrush(Colors.Gray);
        }
        #endregion

        #region button_visual_modifiers
        private void Button_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            string btn = Convert.ToString(((Button)sender).Content);
            if (cursorTracking)
            {
                switch (btn)
                {
                    case "Green":
                        Green.Background = new SolidColorBrush(Colors.Green);
                        break;
                    case "Blue":
                        Blue.Background = new SolidColorBrush(Colors.Blue);
                        break;
                    case "Red":
                        Red.Background = new SolidColorBrush(Colors.Red);
                        break;
                    case "Yellow":
                        Yellow.Background = new SolidColorBrush(Colors.Yellow);
                        break;
                    default:
                        break;
                }
            }

        }

        private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            string btn = Convert.ToString(((Button)sender).Content);
            if (cursorTracking)
            {
                switch (btn)
                {
                    case "Green":
                        Green.Background = new SolidColorBrush(Colors.Gray);
                        break;
                    case "Blue":
                        Blue.Background = new SolidColorBrush(Colors.Gray);
                        break;
                    case "Red":
                        Red.Background = new SolidColorBrush(Colors.Gray);
                        break;
                    case "Yellow":
                        Yellow.Background = new SolidColorBrush(Colors.Gray);
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        #region Timer_Methods

        private void initializeTimer()
        {
            LightTimer.Interval = TimeSpan.FromMilliseconds(10);
            LightTimer.Tick += LightTimer_Tick;
        }

        private void LightTimer_Tick(object sender, object e)
        {
            tick++;
            if (tick > 25)
            {
                if (!cursorTracking)
                {
                    LightOn = !LightOn;
                    LightTesting();
                }
                tick = 0;
            }
        }

        private void LightTesting()
        {
            //LightTimer.Start();
            if (LightOn && LightSlot < Level)
            {
                switch (answers[LightSlot])
                {
                    case 0:
                        Green.Background = new SolidColorBrush(Colors.Green);
                        break;
                    case 1:
                        Blue.Background = new SolidColorBrush(Colors.Blue);
                        break;
                    case 2:
                        Red.Background = new SolidColorBrush(Colors.Red);
                        break;
                    case 3:
                        Yellow.Background = new SolidColorBrush(Colors.Yellow);
                        break;
                    default:
                        break;
                }
            }
            if (!LightOn)
            {
                Lighting_off();
                if (LightSlot == Level)
                {
                    //LightTimer.Stop();
                    LightSlot = 0;
                    cursorTracking = true;
                }
                LightSlot++;
            }
        }


        #endregion
    }
}
