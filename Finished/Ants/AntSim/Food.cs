using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntSim
{
    class Food
    {
        //The food's position
        public int MyX { get; private set; }
        public int MyY { get; private set; }

        //The food number
        public int FoodSource { get; private set; }

        //The quantity of food
        public int Rations { get; set; }

        /// <summary>
        /// Food prepared for creation
        /// </summary>
        public Food()
        {
            MyX = 0;
            MyY = 0;
            Rations = 0;
        }

        /// <summary>
        /// Spawns food on mouseclick
        /// </summary>
        /// <param name="locX">Passes in the X value of mouseclick.</param>
        /// <param name="locY">Passes in the Y value of mouseclick.</param>
        /// <param name="foodNo">Passes in the number of the food pile.</param>
        /// <param name="rat">Passes in the number of food the pile holds.</param>
        public Food( int locX, int locY, int foodNo, int rat )
        {
            MyX = locX;
            MyY = locY;
            FoodSource = foodNo;
            Rations = rat;
        }

        /// <summary>
        /// Removes food based on the strength value of the mouse interacting with it.
        /// It also determines if it has food left and tells the ant when if it's empty.
        /// </summary>
        /// <param name="amountToTake">Passes in the strength of the ant and removes that much food.</param>
        /// <returns>Returns whether there is food left or not.</returns>
        public bool TakeFood( int amountToTake )
        {
            Rations -= amountToTake;

            if ( Rations == 0 )
            {
                return false;
            }
            return true;
        }
    }
}