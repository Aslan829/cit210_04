using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unit04.Game.Casting;
using Unit04.Game.Directing;
using Unit04.Game.Services;


namespace Unit04
{
    /// <summary>
    /// The program's entry point.
    /// </summary>
    class Program
    {
        private static int FRAME_RATE = 12;
        private static int MAX_X = 900;
        private static int MAX_Y = 600;
        private static int CELL_SIZE = 15;
        private static int FONT_SIZE = 15;
        private static int COLS = 60;
        private static int ROWS = 40;
        private static string CAPTION = "Robot Finds Kitten";
        private static string DATA_PATH = "Data/messages.txt";
        private static Color WHITE = new Color(255, 255, 255);
        private static int DEFAULT_ARTIFACTS = 40;


        /// <summary>
        /// Starts the program using the given arguments.
        /// </summary>
        /// <param name="args">The given arguments.</param>
        static void Main(string[] args)
        {
            // create the cast
            Cast cast = new Cast();

            // create the banner
            Actor banner = new Actor();
            banner.SetText("");
            banner.SetFontSize(FONT_SIZE);
            banner.SetColor(WHITE);
            banner.SetPosition(new Point(CELL_SIZE, 0));
            cast.AddActor("banner", banner);

            // create the robot
            Actor robot = new Actor();
            robot.SetText("#");
            robot.SetFontSize(FONT_SIZE);
            robot.SetColor(WHITE);
            robot.SetPosition(new Point(MAX_X / 2, MAX_Y - CELL_SIZE)); //set Y to be bottom of screen - cell_size so it can fit on screen
            cast.AddActor("robot", robot);

            // load the messages
            List<string> messages = File.ReadAllLines(DATA_PATH).ToList<string>();

            // create the artifacts
            Random random = new Random();
            //makes # of default artifacts (maybe change to be random)
            for (int i = 0; i < DEFAULT_ARTIFACTS; i++)
            {

                //Makes random variables
                /*string text = ((char)random.Next(33, 126)).ToString();
                string message = messages[i];*/

                //sets positions
                int x = random.Next(1, COLS);
                int y = 0; //set position at the bottom of screen
                Point position = new Point(x, y);
                position = position.Scale(CELL_SIZE);

                //sets colors
                int r = random.Next(0, 256);
                int g = random.Next(0, 256);
                int b = random.Next(0, 256);
                Color color = new Color(r, g, b);


                //Make a random variable. If 1 --> gem, if 2 --> rock
                int choice = random.Next(0,2);

                //Make a gem
                if (choice == 0)
                {
                    Artifact Rock = new Artifact();
                    Rock.SetText("O");
                    Rock.SetFontSize(FONT_SIZE);
                    Rock.SetColor(color);
                    Rock.SetPosition(position);
                    //rock.SetMessage(message);
                    cast.AddActor("rocks", Rock);
                } 
                //Makes the gem
                else if (choice == 1)
                {
                    Artifact gem = new Artifact();
                    gem.SetText("*");
                    gem.SetFontSize(FONT_SIZE);
                    gem.SetColor(color);
                    gem.SetPosition(position);
                    //rock.SetMessage(message);
                    cast.AddActor("gems", gem); 
                }
            }

            // start the game
            KeyboardService keyboardService = new KeyboardService(CELL_SIZE); //start keyboard reading
            VideoService videoService 
                = new VideoService(CAPTION, MAX_X, MAX_Y, CELL_SIZE, FRAME_RATE, false); //makes video
            Director director = new Director(keyboardService, videoService);
            director.StartGame(cast);
        }
    }
}