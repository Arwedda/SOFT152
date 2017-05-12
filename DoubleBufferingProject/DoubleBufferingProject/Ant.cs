using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleBufferingProject
{
    class Ant
    {
        public int X { set; get; }
        public int Y { set; get; }

        private Random randomObject;

        public Ant(Random r)
        {
            randomObject = r;

            //  assume Ants X and Y postion is between 200...400

            X = randomObject.Next(200, 400);
            Y = randomObject.Next(200, 400);

            Console.WriteLine("ant created at, x = {0}, y = {1} ", X, Y);
        }
    }
}
