using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntSim
{
    class Ant
    {
        //The ant's position
        public int MyX { get; private set; }
        public int MyY { get; private set; }

        //The direction that the ant faces
        private int Direction { get; set; }

        //Ant's memory quality
        private int Memory { get; set; }

        //Ant's strength quality
        private int Strength { get; set; }

        //Whether this ant knows where a nest or food are located
        private int Colony { get; set; }
        private int FoodSource { get; set; }

        //Whether this ant currently holds food
        private int HasFood { get; set; }

        //Whether the food should be shared
        private bool FoodLeft { get; set; }

        //The position of ant's chosen home
        private int NestX { get; set; }
        private int NestY { get; set; }

        //The position of the food the ant knows about
        private int FoodX { get; set; }
        private int FoodY { get; set; }
        
        //Distance the ant can see
        private int SightDistance { get; set; }

        //Distance the ant can communicate with other ants
        private int CommunicationDistance { get; set; }

        /// <summary>
        /// Ant prepared for creation
        /// </summary>
        public Ant()
        {
            MyX = 0;
            MyY = 0;
            Direction = 0;
            Memory = 0;
            Strength = 0;
            Colony = 0;
            FoodSource = 0;
            HasFood = 0;
            FoodLeft = false;
            NestX = 0;
            NestY = 0;
            FoodX = 0;
            FoodY = 0;
            SightDistance = 0;
            CommunicationDistance = 0;
        }

        /// <summary>
        /// Spawns a real Ant
        /// </summary>
        /// <param name="locRand">Randomly generated number to calculate position</param>
        /// <param name="locWidth">The maximum X coordinate that the Ant can spawn at</param>
        /// <param name="locHeight">The maximum Y coordinate that the Ant can spawn at</param>
        /// <param name="locSight" >The maximum distance that the Ant can see</param>
        /// <param name="locTalk" >The maximum distance that the Ant can communicate</param>
        public Ant( Random locRand, int locWidth, int locHeight, int locSight, int locTalk )
        {
            //Selects random X and Y coordinates to start at
            MyX = locRand.Next( 0, locWidth + 1 );
            MyY = locRand.Next( 0, locHeight + 1 );

            //Selects random starting direction
            Direction = locRand.Next( 0, 8 );

            //Selects random memory quality (can have 0.03% to 0.003% chance to forget each or both items each 0.1 second)
            Memory = locRand.Next( 10000, 100001 );

            //Selects random strength quality
            Strength = locRand.Next( 1, 4 );

            //Starts without a Colony, therefore starts with insignificant home coordinates
            Colony = 0;
            NestX = 0;
            NestY = 0;

            //Starts without food or knowledge of food location
            FoodSource = 0;
            FoodX = 0;
            FoodY = 0;
            HasFood = 0;
            FoodLeft = false;

            //Sets distance that the ant can see
            SightDistance = locSight;

            //Sets distance that the ant can communicate between ants
            CommunicationDistance = locTalk;
        }

        /// <summary>
        /// The action the ant takes every tenth of a second. Checks for ants, food, and nests.
        /// Checks if he forgets anything and moves. Calls the methods the ant uses to be ant like.
        /// </summary>
        /// <param name="tempRand">Temporary random number</param>
        /// <param name="tempChoices">Determines which direction the ant will turn</param>
        /// <param name="tempWidth">Width of the form</param>
        /// <param name="tempHeight">Height of the form</param>
        /// <param name="tempNestList">Temp list of all the nests.</param>
        /// <param name="tempFoodList">Temp list of all the foods.</param>
        /// <param name="tempAntList">Temp list of all the ants.</param>
        public void AntAction( Random tempRand, int tempChoices, int tempWidth, int tempHeight, List<Nest> tempNestList, List<Food> tempFoodList, List<Ant> tempAntList )
        {
            TestMemory( tempRand );
            CheckForFood( tempFoodList );
            CheckForNest( tempNestList );
            CheckForAnts( tempAntList );
            Move( tempRand, tempChoices, tempWidth, tempHeight );
        }

        /// <summary>
        /// Generates a random number and the ant forgets some information if the number is 0, 1 or 2.
        /// </summary>
        /// <param name="locRand">generates a random number</param>
        private void TestMemory( Random locRand )
        {
            int memoryTest = 0;

            memoryTest = locRand.Next( 0, Memory + 1 );

            if ( memoryTest == 0 )
            {
                Colony = 0;
                FoodSource = 0;
                FoodLeft = false;
            }
            else if ( memoryTest == 1 )
            {
                Colony = 0;
            }
            else if ( memoryTest == 2 )
            {
                FoodSource = 0;
                FoodLeft = false;
            }
        }
        
        /// <summary>
        /// Checks if the ant can see food and memorises it.
        /// Also checks if the ant's current food is further away than new food
        /// And remembers the closest one.
        /// </summary>
        /// <param name="locFoodList">Passes in the food list</param>
        private void CheckForFood( List<Food> locFoodList )
        {
            int visionDistance = 0;
            int stepDifference = 0;
            int compDifference = 0;

            foreach ( Food f in locFoodList )
            {
                //Calculate vision distance of food
                visionDistance = CalculateSightDistance( f.MyX, MyX, f.MyY, MyY );

                //If able to see food
                if ( visionDistance > -SightDistance && visionDistance < SightDistance )
                {
                    if ( FoodSource != 0 && FoodSource != f.FoodSource )
                    {
                        stepDifference = CalculateStepDistance( FoodX, NestX, FoodY, NestY );
                        compDifference = CalculateStepDistance( f.MyX, NestX, f.MyY, NestY );

                        if ( compDifference < stepDifference )
                        {
                            RememberFood( f );
                        }
                    }
                    else
                    {
                        RememberFood( f );
                    }

                    //If on top of food take it
                    if ( visionDistance == 0 && HasFood == 0 )
                    {
                        TakeFood( f );
                    }
                }
            }
        }

        /// <summary>
        /// Saves the food to the ants memory.
        /// </summary>
        /// <param name="locF">Passes in the food object.</param>
        private void RememberFood( Food locF )
        {
            FoodSource = locF.FoodSource;
            FoodX = locF.MyX;
            FoodY = locF.MyY;
            FoodLeft = true;
        }

        /// <summary>
        /// The ant takes food from the foodpile eqaul to his strength
        /// or if there's not enough there he takes what is left.
        /// </summary>
        /// <param name="locF">Passes in the food object.</param>
        private void TakeFood( Food locF )
        {
            if (Strength > locF.Rations)
            {
                HasFood = locF.Rations;
                FoodLeft = locF.TakeFood(locF.Rations);
            }
            else
            {
                HasFood = Strength;
                FoodLeft = locF.TakeFood(Strength);
            }
        }

        /// <summary>
        /// The ant checks to see if he can detect a nest and if he doesnt have a colony remembers it as his colony.
        /// He also deposits food at the nest.
        /// </summary>
        /// <param name="locNestList">Passes in the Nest object</param>
        private void CheckForNest( List<Nest> locNestList )
        {
            int visionDistance = 0;

            foreach ( Nest n in locNestList )
            {
                //Calculate vision distance of nest
                visionDistance = CalculateSightDistance( n.MyX, MyX, n.MyY, MyY );

                //If able to see nest
                if ( visionDistance > -SightDistance && visionDistance < SightDistance )
                {
                    if ( Colony == 0 )
                    {
                        RememberColony( n );
                    }

                    //If on top of own nest drop food
                    if ( visionDistance == 0 && Colony == n.Colony )
                    {
                        GiveFood( n );
                    }
                }
            }
        }

        /// <summary>
        /// The ant memorises the colony location and sets the colony as his own.
        /// </summary>
        /// <param name="locN">Passes in the next object.</param>
        private void RememberColony( Nest locN )
        {
            Colony = locN.Colony;
            NestX = locN.MyX;
            NestY = locN.MyY;
        }

        /// <summary>
        /// The ant deposits the food into his nest.
        /// </summary>
        /// <param name="locN">Passes in the Nest object.</param>
        private void GiveFood( Nest locN )
        {
            locN.DepositFood(HasFood);
            HasFood = 0;
        }

        /// <summary>
        /// Checks to see if there are any ants within it's detection radius and
        /// communicates with them if there is.
        /// </summary>
        /// <param name="locAntList">Passes in the list of ants.</param>
        private void CheckForAnts( List<Ant> locAntList )
        {
            foreach ( Ant a in locAntList )
            {
                int talkDistance = 0;

                //Calculate vision distance between ants
                talkDistance = CalculateSightDistance( a.MyX, MyX, a.MyY, MyY );

                //If able to see other ant
                if ( talkDistance > -CommunicationDistance && talkDistance < CommunicationDistance )
                {   //If this ant has a colony and the other ant does not then convert it to this ant's colony
                    if ( Colony != 0 && a.Colony == 0 )
                    {
                        TeachColony( a );
                    }
                    else if ( Colony == 0 && a.Colony != 0 )
                    {
                        LearnColony( a );
                    }
                    else if ( Colony == a.Colony )
                    {
                        ShareFood( a );
                    }
                }
            }
        }

        /// <summary>
        /// Teaches the ant's colony information to another ant.
        /// Also shares food information.
        /// </summary>
        /// <param name="locA">Passes in the ant object.</param>
        private void TeachColony( Ant locA )
        {
            locA.Colony = Colony;
            locA.NestX = NestX;
            locA.NestY = NestY;

            ShareFood( locA );
        }

        /// <summary>
        /// Learns colony information from another ant.
        /// Also shares food information.
        /// </summary>
        /// <param name="locA">Passes in the ant object</param>
        private void LearnColony( Ant locA )
        {
            Colony = locA.Colony;
            NestX = locA.NestX;
            NestY = locA.NestY;

            ShareFood( locA );
        }

        /// <summary>
        /// Calculates the sight distance by taking the coordinate value of the ant and the value of a nest/food
        /// and calculating how many pixels different they are.
        /// </summary>
        /// <param name="x1">Ant's X co-ordinate.</param>
        /// <param name="x2">Object's X co-ordinate.</param>
        /// <param name="y1">Ant's Y co-ordinate.</param>
        /// <param name="y2">Object's Y co-ordinate.</param>
        /// <returns></returns>
        private int CalculateSightDistance( int x1, int x2, int y1, int y2 )
        {
            int xDiff = 0;
            int yDiff = 0;

            if ( x1 > x2 )
            {
                xDiff = x1 - x2;
            }
            else
            {
                xDiff = x2 - x1;
            }

            if ( y1 > y2 )
            {
                yDiff = y1 - y2;
            }
            else
            {
                yDiff = y2 - y1;
            }

            if ( xDiff > 0 && yDiff > 0 || xDiff < 0 && yDiff < 0 )
            {
                return ( xDiff + yDiff );
            }
            else if ( xDiff > 0 && yDiff < 0 )
            {
                return ( yDiff - xDiff );
            }
            else //( xDiff < 0 && yDiff > 0 )
            {
                return ( xDiff - yDiff );
            }
        }

        /// <summary>
        /// Calculates the number of steps an ant needs to make between food and the nest.
        /// </summary>
        /// <param name="x1">Food x-coordinate</param>
        /// <param name="x2">Nest x-coordinate</param>
        /// <param name="y1">Food y-coordinate</param>
        /// <param name="y2">Nest y-coordinate</param>
        /// <returns></returns>
        private int CalculateStepDistance(int x1, int x2, int y1, int y2)
        {
            int xDiff = 0;
            int yDiff = 0;

            //Calculate the number of steps on each axis
            xDiff = Math.Abs( x1 - x2 );
            yDiff = Math.Abs( y1 - y2 );

            //Since ants intelligently step diagonally when it is efficient to do so, the number of steps required is the larger of x and y
            if ( xDiff < yDiff )
            {
                return xDiff;
            }
            else
            {
                return yDiff;
            }
        }

        /// <summary>
        /// Shares food between ants.
        /// Also calculates the food closest to their own colonies if both ants have knowledge of different food.
        /// </summary>
        /// <param name="locA">Passes in the ant object</param>
        private void ShareFood( Ant locA )
        {
            int totDiff = 0;
            int compDiff = 0;

            //If this ant doesn't know where food is and the other ant does then ask where to get food
            if ( FoodLeft == false && locA.FoodLeft )
            {
                if ( FoodSource != locA.FoodSource )
                {
                    LearnFood( locA );
                }
                else
                {
                    locA.FoodLeft = false;
                }
            } //If this ant knows where food is and the other ant does not then tell it where to get food
            else if ( FoodLeft && locA.FoodLeft == false )
            {
                if ( FoodSource != locA.FoodSource )
                {
                    TeachFood( locA );
                }
                else
                {
                    FoodLeft = false;
                }
            } //Both ants know where food is and they are different calculate closest to their nest
            else if ( FoodLeft && locA.FoodLeft && FoodSource != locA.FoodSource )
            {
                totDiff = CalculateStepDistance( FoodX, NestX, FoodY, NestY );
                compDiff = CalculateStepDistance( locA.FoodX, NestX, locA.FoodY, NestY );
                if ( totDiff > compDiff )
                {
                    LearnFood( locA );
                }
                else
                {
                    TeachFood( locA );
                }
            }
        }

        /// <summary>
        /// Handles an ant learning food from another ant
        /// </summary>
        /// <param name="tempA">Passes in the ant object</param>
        private void LearnFood( Ant tempA )
        {
            FoodSource = tempA.FoodSource;
            FoodX = tempA.FoodX;
            FoodY = tempA.FoodY;
            FoodLeft = true;
        }

        /// <summary>
        /// Handles the ant teaching good to another ant.
        /// </summary>
        /// <param name="tempA">Passes in the ant object</param>
        private void TeachFood( Ant tempA )
        {
            tempA.FoodSource = FoodSource;
            tempA.FoodX = FoodX;
            tempA.FoodY = FoodY;
            tempA.FoodLeft = true;
        }
        
        /// <summary>
        /// Handles the ant's movement, including moving towards nest, food and 
        /// wandering aimlessly.
        /// </summary>
        /// <param name="locRand">Passes in a random number.</param>
        /// <param name="locChoices">Passes in the number that determines it's facing direction.</param>
        /// <param name="locWidth">Passes in the width of the form.</param>
        /// <param name="locHeight">Passes in the height of the form.</param>
        private void Move( Random locRand, int locChoices, int locWidth, int locHeight )
        {
            //The ant has food but no colony or doesn't know where food is
            if (( HasFood > 0 && Colony == 0 ) || ( HasFood == 0 && FoodLeft == false ))
            {
                //Randomly wander
                Wander( locRand, locChoices );
            }//Ant knows where food is but currently doesn't have any
            else if ( FoodLeft && HasFood == 0 )
            {
                //Collect food
                Direction = ObjectiveDirection( FoodX, FoodY );
            }//Ant has food and has a colony
            else if ( HasFood > 0 && Colony != 0 )
            {
                //Take food to colony
                Direction = ObjectiveDirection( NestX, NestY );
            }
            UpdatePosition();

            //Ensure selected direction is on the map
            MyX = CheckOnScreen( MyX, locWidth );
            MyY = CheckOnScreen( MyY, locHeight );
        }

        /// <summary>
        /// Handles the ant wandering aimlessly if it has no food to take back to a nest
        /// or food to take back but no nest.
        /// </summary>
        /// <param name="tempRand">Passes in a random number</param>
        /// <param name="tempChoices">Passes in the number that determines it's facing direction.</param>
        private void Wander( Random tempRand, int tempChoices )
        {
            int chosen = 0;

            chosen = tempRand.Next( 0, tempChoices );
            if ( chosen == 0 )
            {
                Direction--;
                if ( Direction < 0 )
                {
                    Direction = 7;
                }
            }
            else if ( chosen == 2 )
            {
                Direction++;
                if ( Direction > 7 )
                {
                    Direction = 0;
                }
            }
        }

        /// <summary>
        /// Determines what direction the ant needs to take to reach it's current objective.
        /// </summary>
        /// <param name="locX">Objective's X co-ordinate.</param>
        /// <param name="locY">Objective's Y co-ordinate.</param>
        /// <returns>The direction that the and should travel in to reach its destination</returns>
        private int ObjectiveDirection( int locX, int locY )
        {
            if ( MyX > locX )
            {
                //Needs to move up/left
                if ( MyY > locY )
                {
                    return 0;
                }//Needs to move down/left
                else if ( MyY < locY )
                {
                    return 6;
                }//Needs to move left
                else
                {
                    return 7;
                }
            }
            else if ( MyX < locX )
            {
                //Needs to move up/right
                if ( MyY > locY )
                {
                    return 2;
                }//Needs to move down/right
                else if ( MyY < locY )
                {
                    return 4;
                }//Needs to move right
                else
                {
                    return 3;
                }
            }
            else
            {
                //Needs to move up
                if ( MyY > locY )
                {
                    return 1;
                }
                    //Needs to move down
                else
                {
                    return 5;
                }
            }
        }

        /// <summary>
        /// Alters the X and/or Y coordinate value to simulate taking a single step in any given direction.
        /// </summary>
        private void UpdatePosition()
        {
            switch ( Direction )
            {
                //Up/Left
                case 0:
                    MyX--;
                    MyY--;
                    break;
                //Up
                case 1:
                    MyY--;
                    break;
                //Up/Right
                case 2:
                    MyX++;
                    MyY--;
                    break;
                //Right
                case 3:
                    MyX++;
                    break;
                //Down/Right
                case 4:
                    MyX++;
                    MyY++;
                    break;
                //Down
                case 5:
                    MyY++;
                    break;
                //Down/Left
                case 6:
                    MyX--;
                    MyY++;
                    break;
                //Left
                case 7:
                    MyX--;
                    break;
            }
        }

        /// <summary>
        /// Checks that the step that the ant has taken is still within the confines of the panel.
        /// If it is not then it moves the ant to the other side of the panel.
        /// </summary>
        /// <param name="locAxis">The current X or Y coordinate of the ant.</param>
        /// <param name="screenMax">The maximum X or Y coordinate within the panel</param>
        /// <returns>The coordinate that the ant is on after the check</returns>
        private int CheckOnScreen( int locAxis, int screenMax )
        {
            if ( locAxis < 1 )
            {
                return screenMax - 4;
            }
            else if ( locAxis > screenMax - 4 )
            {
                return 1;
            }
            else
            {
                return locAxis;
            }
        }
    }
}