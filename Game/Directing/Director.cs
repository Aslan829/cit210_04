using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unit04.Game.Casting;
using Unit04.Game.Directing;
using Unit04.Game.Services;

namespace Unit04.Game.Directing
{
    /// <summary>
    /// <para>A person who directs the game.</para>
    /// <para>
    /// The responsibility of a Director is to control the sequence of play.
    /// </para>
    /// </summary>
    public class Director
    {
        private KeyboardService keyboardService = null;
        private VideoService videoService = null;

        private int score = 0; //banner score, easy to update here

        /// <summary>
        /// Constructs a new instance of Director using the given KeyboardService and VideoService.
        /// </summary>
        /// <param name="keyboardService">The given KeyboardService.</param>
        /// <param name="videoService">The given VideoService.</param>
        public Director(KeyboardService keyboardService, VideoService videoService)
        {
            this.keyboardService = keyboardService;
            this.videoService = videoService;
        }

        /// <summary>
        /// Starts the game by running the main game loop for the given cast.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        public void StartGame(Cast cast)
        {
            

            videoService.OpenWindow();
            while (videoService.IsWindowOpen())
            {
                GetInputs(cast);
                DoUpdates(cast); //find mroe effecient solution to this, this isnt good
                DoOutputs(cast);
            }
            videoService.CloseWindow();
        }

        /// <summary>
        /// Gets directional input from the keyboard and applies it to the robot.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        private void GetInputs(Cast cast)
        {
            Actor robot = cast.GetFirstActor("robot");
            Point velocity = keyboardService.GetDirection();
            robot.SetVelocity(velocity);     
        }

        /// <summary>
        /// Updates the robot's position and resolves any collisions with artifacts.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        private void DoUpdates(Cast cast)
        {
            
            //Make 2 new gems every update
            Actor banner = cast.GetFirstActor("banner");
            banner.SetText("SCORE:" + this.score);
            Actor robot = cast.GetFirstActor("robot");
            List<Actor> rocks = cast.GetActors("rocks"); //Rocks list / bring gem list too
            List<Actor> gems = cast.GetActors("gems");

       
            
            int maxX = videoService.GetWidth();
            int maxY = videoService.GetHeight();
            robot.MoveNext(maxX, maxY);

            Random random = new Random();
            
            //makes # of default artifacts (maybe change to be random)
            for (int i = 0; i < 2; i++)
            {

                //Makes random variables
                /*string text = ((char)random.Next(33, 126)).ToString();
                string message = messages[i];*/

                //sets positions
                int x = random.Next(1, 60);
                int y = 0; //set position at the bottom of screen
                Point position = new Point(x, y);
                position = position.Scale(15);

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
                    Rock.SetFontSize(15);
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
                    gem.SetFontSize(15);
                    gem.SetColor(color);
                    gem.SetPosition(position);
                    //rock.SetMessage(message);
                    cast.AddActor("gems", gem); 
                }
            }


            
            //foreach for rocks / interaction method
            foreach (Actor actor in rocks) 
            {
                Point actor_position = actor.GetPosition();//Edited here
                actor_position.Down(5);//edited here (go down cell size)
                //made go off screen?

                if (robot.GetPosition().Equals(actor.GetPosition()))//what happens when merge artifacts
                {
                    Artifact artifact = (Artifact) actor;
                    this.score -= 1;
                    banner.SetText("SCORE:" + score);
                    
                }
            } 

            //gem going down/interact function
            foreach (Actor actor in gems) 
            {
                Point actor_position = actor.GetPosition();//Edited here
                actor_position.Down(5);//edited here (go down cell size)
                //made go off screen?

                if (robot.GetPosition().Equals(actor.GetPosition()))//what happens when merge artifacts
                {
                    Artifact artifact = (Artifact) actor;
                    this.score += 1;
                    banner.SetText("SCORE:" + score);
                    
                }
            } 
        }

        /// <summary>
        /// Draws the actors on the screen.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        public void DoOutputs(Cast cast)
        {
            List<Actor> actors = cast.GetAllActors();
            videoService.ClearBuffer();
            videoService.DrawActors(actors);
            videoService.FlushBuffer();
        }

    }
}