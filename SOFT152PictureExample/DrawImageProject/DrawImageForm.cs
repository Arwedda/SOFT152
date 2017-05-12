using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawImageProject
{
    public partial class DrawImageForm : Form
    {
        private Image pictureOne;



       private string fileName;

        public DrawImageForm()
        {
            InitializeComponent();
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            fileName = @"images\Rabbit.jpg";
            pictureOne = new Bitmap(fileName);


            // etiher call a method to  do the drawing
            DrawTheImage();


            // alternatively can also call refresh 
            // which will call the Paint() method

            // Refresh();
        }

        private void DrawTheImage()
        {
            Rectangle pictureOnePosAndSize = new Rectangle (20, 30, 250, 200);

            using (Graphics panelGraphics = ImagePanel.CreateGraphics())
            {
                panelGraphics.Clear(Color.White);

                panelGraphics.DrawImage(pictureOne, pictureOnePosAndSize);
            }
        }  // end DrawTheImage()

        private void ImagePanel_Paint(object sender, PaintEventArgs e)
        {
            // the Paint() method is called automatically, and it may be called 
            // before a picture is loaded. So test for a null image and if null
            // then return from this method before the picture is used
            if (pictureOne == null)
            {
                // just a debug meesage to show if the picture is null when Paint() is called
                Console.WriteLine(" paint called pictureOne is null ");
                return;
            }

            Rectangle pictureOnePosAndSize = new Rectangle(20, 30, 250, 200);

            using (Graphics panelGraphics = ImagePanel.CreateGraphics())
            {
               panelGraphics.DrawImage(pictureOne, pictureOnePosAndSize);

            }
        }

    }
}
