using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace AntSim
{
    public partial class AntSimForm : Form
    {
        //List representing all of the ants
        private List<Ant> antList;
        //List representing all of the nests
        private List<Nest> nestList;
        //List representing all of the food
        private List<Food> foodList;

        //Point to place nests & food
        private Point placePoint;

        //Bitmap Image for use with double buffering
        private Bitmap worldImage;

        //Random number generator used for various tasks
        private Random randomNumberGenerator;

        //Food number
        private int foodPileNumber = 1;

        /// <summary>
        /// Sets most global properties for the simulation - ant numbers, distances, panel size.
        /// Creates ants, starts the timer, creates the lists.
        /// </summary>
        public AntSimForm()
        {
            int totAnts = 150;
            int maxSight = 50;
            int maxTalk = maxSight / 2;

            int worldWidth = 660;
            int worldHeight = 637;

            InitializeComponent();

            antList = new List<Ant> { };
            nestList = new List<Nest> { };
            foodList = new List<Food> { };

            worldImage = new Bitmap( worldWidth, worldHeight );

            CreateAnts( totAnts, maxSight, maxTalk );
            moveTimer.Start();
        }

        /// <summary>
        /// Creates each individual ant.
        /// Calls method to draw the initial simulation set up.
        /// </summary>
        /// <param name="locAnts"></param>Passes in the total number of ants.
        /// <param name="tempSight"></param>Passes in the ant's sight distance.
        /// <param name="tempTalk"></param>Passes in the ant's talk distance.
        private void CreateAnts( int locAnts, int tempSight, int tempTalk )
        {
            Ant tempAnt = new Ant();
            randomNumberGenerator = new Random();

            for ( int i = 1; i <= locAnts; i++ )
            {
                tempAnt = new Ant( randomNumberGenerator, groundPanel.Width, groundPanel.Height, tempSight, tempTalk );
                antList.Add( tempAnt );
            }

            DrawWorld();
        }

        /// <summary>
        /// Ticks every 0.1 second
        /// Cycles through each ant's decision making and any consequential actions each tick.
        /// Calls draw method every tick to visually update simulation each tick.
        /// </summary>
        private void moveTimer_Tick( object sender, EventArgs e )
        {
            int maxChoices = 3;

            foreach ( Ant a in antList )
            {
                a.AntAction( randomNumberGenerator, maxChoices, groundPanel.Width, groundPanel.Height, nestList, foodList, antList );
                CheckFood();
            }

            DrawWorld();
        }

        /// <summary>
        /// Checks if any of the food piles in food list are depleted them and removes them if they are.
        /// </summary>
        private void CheckFood()
        {
            int i = 0;

            try
            {
                foreach ( Food f in foodList )
                {
                    if ( f.Rations == 0 )
                    {
                        foodList.RemoveAt( i );
                    }

                    i++;
                }
            }
            catch ( InvalidOperationException )
            {
                //Triggers when emptying foodList
            }
        }

        /// <summary>
        /// Draws the world. Draws all the ants, nests and food piles that are currently in the lists.
        /// </summary>
        private void DrawWorld()
        {
            int antSize = 4;
            int foodSize = 8;
            int nestSize = 16;

            Point localPoint = new Point();

            using ( Graphics worldImageGraphics = Graphics.FromImage( worldImage ))
            {
                worldImageGraphics.Clear( Color.BurlyWood );

                using ( Brush foodBrush = new SolidBrush( Color.LawnGreen ))
                {
                    foreach ( Food f in foodList )
                    {
                        localPoint.X = f.MyX;
                        localPoint.Y = f.MyY;
                        worldImageGraphics.FillRectangle( foodBrush, f.MyX - ( foodSize / 2 ), f.MyY - ( foodSize / 2 ), foodSize, foodSize );
                    }
                }

                using ( Brush nestBrush = new SolidBrush( Color.Chocolate ))
                {
                    foreach ( Nest n in nestList )
                    {
                        localPoint.X = n.MyX;
                        localPoint.Y = n.MyY;
                        worldImageGraphics.FillRectangle( nestBrush, n.MyX - ( nestSize / 2 ), n.MyY - ( nestSize / 2), nestSize, nestSize );
                    }
                }

                using ( Brush antBrush = new SolidBrush( Color.Black ))
                {
                    foreach ( Ant a in antList )
                    {
                        localPoint.X = a.MyX;
                        localPoint.Y = a.MyY;
                        worldImageGraphics.FillRectangle( antBrush, a.MyX - ( antSize / 2 ), a.MyY - ( antSize / 2 ), antSize, antSize );
                    }
                }

                using ( Graphics panelGraphics = groundPanel.CreateGraphics() )
                {
                    panelGraphics.DrawImage( worldImage, 0, 0, groundPanel.Width, groundPanel.Height );
                }
            }
        }

        /// <summary>
        /// Handles placing food piles and nests at the mouse click location.
        /// Adds the generated nests and food piles to the relevant list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Information on the mouseclick such as co-ordinates.</param>
        private void groundPanel_MouseUp( object sender, MouseEventArgs e )
        {
            int tempX = 0;
            int tempY = 0;
            int tempRations = 8;

            placePoint = e.Location;

            tempX = placePoint.X;
            tempY = placePoint.Y;

            if ( e.Button == MouseButtons.Left )
            {
                Food tempFood;
                tempFood = new Food( tempX, tempY, foodPileNumber, tempRations );
                foodList.Add( tempFood );
                foodPileNumber++;
            }
            else if ( e.Button == MouseButtons.Right )
            {
                Nest tempNest;
                tempNest = new Nest( tempX, tempY, nestList.Count + 1 );
                nestList.Add( tempNest );
            }
        }
    }
}