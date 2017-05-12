using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntSim
{
    class Nest
    {
        //The nest's position
        public int MyX { get; private set; }
        public int MyY { get; private set; }

        //The colony number
        public int Colony { get; private set; }

        //Amount of food delivered to the colony by it's ants
        public int Rations { private get; set; }

        /// <summary>
        /// Nest prepared for creation.
        /// </summary>
        public Nest()
        {
            MyX = 0;
            MyY = 0;
            Colony = 0;
            Rations = 0;
        }

        /// <summary>
        /// Created a new next where using information about the mouseclick
        /// </summary>
        /// <param name="locX">Passes in the X co-ordinate of the mouseclick</param>
        /// <param name="locY">Passes in the Y co-ordinate of the mouseclick</param>
        /// <param name="col">Passes in the colony number</param>
        
        public Nest( int locX, int locY, int col )
        {
            MyX = locX;
            MyY = locY;
            Colony = col;
            Rations = 0;
        }

        /// <summary>
        /// Adds the amount of food an ant is holding to the ration amount held by the nest.
        /// </summary>
        /// <param name="amountToDeposit">The amount of food the ant is holding.</param>
        public void DepositFood( int amountToDeposit )
        {
            Rations += amountToDeposit;
        }
    }
}