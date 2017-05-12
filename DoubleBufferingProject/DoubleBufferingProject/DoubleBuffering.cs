using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoubleBufferingProject
{
    public partial class DoubleBuffering : Form
    {
        // declare a random object
        // this is the object we are going to pass to each Ant
        private Random randObjectForAnt;

        private Ant ant1;
        private Ant ant2;

        //----------------------------
        // Declare a Bitmap on which to draw on
        private Bitmap worldImage;

        public DoubleBuffering()
        {
            InitializeComponent();

            // create a Random object to be given to an Ant
            randObjectForAnt = new Random();

            // declare two variables to hold the size 
            // of the background image
            int worldWidth;
            int worldHeight;

            // define an arbitary world size
            worldWidth = 800;
            worldHeight = 600;

            // now create the world image of size specified
            worldImage = new Bitmap(worldWidth, worldHeight);

            MakeAnts();

            DrawAnts();
            timer.Start();
        }


        private void MakeAnts()
        {
            // create two ant objects and pass 
            // each Ant the same random number object
            ant1 = new Ant(randObjectForAnt);

            ant2 = new Ant(randObjectForAnt);
        }



        private void DrawAnts()
        {
            int posX, posY;
            int antSize;

            // some arbitary size to draw the Ant
            antSize = 10;

            // get the graphics context of the worldImage. As this is the image 
            // on which all the drawing will be taking place
            using (Graphics worldImageGraphics = Graphics.FromImage(worldImage))
            using (Brush antBrush = new SolidBrush(Color.Red))
            {

                // once we have the graphics context, all drawing 
                // techniques are the same
                worldImageGraphics.Clear(Color.White);

                posX = ant1.X;
                posY = ant1.Y;

                worldImageGraphics.FillRectangle(antBrush, posX, posY, antSize, antSize);

                posX = ant2.X;
                posY = ant2.Y;

                worldImageGraphics.FillRectangle(antBrush, posX, posY, antSize, antSize);
            }

            // having finsihed drawing on the image, now draw the whole image 
            // onto the panel
            using (Graphics panelGraphics = drawingPanel.CreateGraphics())
            {
                panelGraphics.DrawImage(worldImage, 0, 0, drawingPanel.Width, drawingPanel.Height);
            }
        
        }

        private void MoveAnts()
        {

            // for moving the ants simply
            // allow ant1 to move west and
            // ant2 to move south
            ant1.X++;

            ant2.Y++;

        }

        private void timer_Tick(object sender, EventArgs e)
        {

            MoveAnts();
            DrawAnts(); 
        }

        private void drawingPanel_Paint(object sender, PaintEventArgs e)
        {
            // when the paint method is called, just call the DrawAnts() method
            DrawAnts();
        }
    }
}
