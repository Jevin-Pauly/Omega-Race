using System;
using System.Diagnostics;

namespace C_SHARP_demo
{

    class Program
    {
        static void Main(string[] args)
        {
            //-------------------------------------------------------------
            // Printing
            //-------------------------------------------------------------

            // explicit printing
            System.Console.WriteLine("---- Testing Console print ----");
            System.Diagnostics.Debug.WriteLine("Debug window printing...");

            // simplied using the using directive
            int x = 5;
            int y = 10;
            Console.WriteLine("  console x: {0} ", x);
            Debug.WriteLine("  y:{1} x: {0} ", x, y);

            // Othre writes...
            Debug.WriteLineIf(2 * x == y, "\tOnly prints if condition is true.");

            //-------------------------------------------------------------
            // Using asserts
            //-------------------------------------------------------------
            Debug.Assert( 2*x == y, "Ya done goofed!");  // Debug-mode only tests: stop execution if condition is fails

            //*-------------------------------------------------------------
            // Structs (value type) on stack no new()
            //-------------------------------------------------------------

            Position pos;

            pos.x = 5;
            pos.y = 7;

            Debug.WriteLine("\nPos struct");
            Debug.WriteLine("  Pos x:{0} y:{1} ", pos.x, pos.y);
            //*/

            //*-------------------------------------------------------------
            // Structs by reference on heap with new()
            //-------------------------------------------------------------

            Position pPos = new Position(1,2);

            Debug.WriteLine("\nPos struct by pointer");
            Debug.WriteLine("  Pos x:{0} y:{1} ", pPos.x, pPos.y);
            //*/

            //*-------------------------------------------------------------
            // Class (reference type) on heap need new()
            //-------------------------------------------------------------

            Car pJetta = new Car();

            pJetta.d = 99;
            pJetta.pos.x = 88;

            Debug.WriteLine("\nJetta");
            Debug.WriteLine("  Pos x:{0} y:{1} ", pJetta.pos.x, pJetta.pos.y);
            Debug.WriteLine("  d:{0}", pJetta.d);
            //*/

            //*-------------------------------------------------------------
            // Boxing / UnBoxing
            //-------------------------------------------------------------

            // pos is value type: box it, make it reference type
            object pObj = pos;

            // unbox it, bring it back to value type
            Position pos2 = (Position)pObj;
            //*/

            //*-------------------------------------------------------------
            // Pass by reference
            //-------------------------------------------------------------

            Car pGolf = new Car();

            pGolf.d = 55;
            pGolf.pos.x = 66;
            pGolf.pos.y = 77;

            Debug.WriteLine("\nGolf before");
            pGolf.PrintMe();

            Car.AddHundred(pGolf);

            Debug.WriteLine("\nGolf after");
            pGolf.PrintMe();
            //*/

            //*-------------------------------------------------------------
            // Pass by using out
            //-------------------------------------------------------------

            Car pPassat = new Car(33, 22, 11);
            Position pos3;

            Debug.WriteLine("\nPassat");
            pPassat.PrintMe();

            Debug.WriteLine("\nPassat: Get Position");
            pPassat.GetPos(out pos3);

            pos3.PrintMe();
            //*/

            //*-------------------------------------------------------------
            // Pass by using ref
            //-------------------------------------------------------------

            Car pTiguan = new Car(99, 88, 77);

            Debug.WriteLine("\nTiguan");
            pTiguan.PrintMe();

            Car.Presto(ref pTiguan);
            Debug.WriteLine("\nTiguan: Presto Chango");
            pTiguan.PrintMe();
            //*/

            // See also Ref vs Out: https://www.c-sharpcorner.com/UploadFile/ff2f08/ref-vs-out-keywords-in-C-Sharp/

            // Cheap stall of the output window
            Console.ReadKey();

        }
    }

    struct Position
    {
        public int x;
        public int y;

        /* Default c'tor not allowed
        public Position()
        {
            this.x = 0;
            this.y = 0;
        }
        //*/

        public Position(int _x, int _y)
        {
            this.x = _x;
            this.y = _y;
        }

        public void PrintMe()
        {
            Debug.WriteLine("  Pos x:{0} y:{1} ", this.x, this.y);
        }
    }

    class Car
    {
        public Position pos;
        public int d;

        public Car()
        {
            this.d = 0;
            this.pos.x = 0;
            this.pos.y = 0;
        }

        public Car(int valD, int vX, int vY)
        {
            this.d = valD;
            this.pos.x = vX;
            this.pos.y = vY;
        }

        static public void AddHundred(Car pCar)
        {
            pCar.d += 100;
            pCar.pos.x += 100;
            pCar.pos.y += 100;
        }

        public void PrintMe()
        {
            Debug.WriteLine("  Pos x:{0} y:{1} ", this.pos.x, this.pos.y);
            Debug.WriteLine("  d:{0}", this.d);
        }

        public void GetPos(out Position position)
        {
            position.x = this.pos.x;
            position.y = this.pos.y;
        }

        static public void Presto(ref Car pCar)
        {
            pCar = new Car(111, 222, 333);
        }
    }
}


// --- End of File ---
