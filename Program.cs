using System.Data;
using System.Globalization;
using System.IO;

namespace Matrix
{
    class Test
    {
        public const int XCOORD = 5;
        public const int YCOORD = 5;

        static void Main(string[] args)
        {
            int[,] matrix = new int[,] { { 1, 5, 2, 3, 2 },
                { 4, 6, 0, 1, 2 }, { 2, 1, 1, 3, 5 },
                { 6, 0, 8, 3, 2 }, { 5, 7, 1, 1, 2} };

            ShowMatrix(matrix);

            Coordinate cstart = new Coordinate(0, 0);
            Coordinate cend = new Coordinate(4, 4);
            Console.WriteLine("FROM " + matrix[cstart.X, cstart.Y] + " TO " + matrix[cend.X, cend.Y]);

            List<Coordinate> path = ShortestPath(matrix, cstart, cend);

            foreach (Coordinate c in path) Console.WriteLine(c);

            ShowMatrixHighlighted(matrix, path, cstart, cend);
        }

        static List<Coordinate> ShortestPath(int[,] matrix, Coordinate cstart, Coordinate cend)
        {
            Coordinate current = new Coordinate(cstart.X, cstart.Y);
            List<Coordinate> path = new List<Coordinate>();

            //Console.WriteLine(current.X + " != " + cend.X + " && " + current.Y + " != " + cend.Y);
            while ((current.X != cend.X) && (current.Y != cend.Y))
            {
                Coordinate choose1 = new Coordinate(current.X, current.Y);  
                Coordinate choose2 = new Coordinate(current.X, current.Y);

                if ((cend.X - current.X) > 0) choose1.X += 1;           //va a destra
                else if ((cend.X - current.X) < 0) choose1.X -= 1;      //va a sinistra

                if ((cend.Y - current.Y) > 0) choose2.Y += 1;           //va in basso
                else if ((cend.Y - current.Y) < 0) choose2.Y -= 1;      //va in alto


                if (choose1.X >= 0 && choose1.Y >= 0 && choose2.X >= 0 && choose2.Y >= 0)
                {
                    //Console.WriteLine("(" + choose1.X + ", " + choose1.Y + ") " + matrix[choose1.X, choose1.Y] +
                    //    " > " + matrix[choose2.X, choose2.Y] + " (" + choose2.X + ", " + choose2.Y + ")");
                    if (matrix[choose1.X, choose1.Y] >= matrix[choose2.X, choose2.Y])
                    {
                        //Console.WriteLine("va in basso/alto");
                        current.X = choose1.X;
                        current.Y = choose1.Y;
                    }
                    else if (matrix[choose2.X, choose2.Y] >= matrix[choose1.X, choose1.Y])
                    {
                        //Console.WriteLine("va di lato");
                        current.X = choose2.X;
                        current.Y = choose2.Y;
                    }
                }
                else if (choose1.X >= 0 && choose1.Y >= 0)
                {
                    current.X = choose1.X;
                    current.Y = choose1.Y;
                }
                else if (choose2.X >= 0 && choose2.Y >= 0)
                {
                    current.X = choose2.X;
                    current.Y = choose2.Y;
                }

                path.Add(new Coordinate(current.X, current.Y));
            }
            if(current.X != cend.X)
            {
                while(current.X != cend.X)
                {
                    /*
                    if ((cend.X - current.X) > 0) {
                        current.X += 1;      //va a destra
                        if (matrix[current.X, current.Y] == 0) current.X -= 2;
                    }
                    else if ((cend.X - current.X) < 0) 
                    {
                        current.X -= 1;      //va a sinistra
                        if (matrix[current.X, current.Y] == 0) current.X += 2;
                    }*/
                    if ((cend.X - current.X) > 0) current.X += 1;           //va a destra
                    else if ((cend.X - current.X) < 0) current.X -= 1;      //va a sinistra

                    path.Add(new Coordinate(current.X, current.Y));
                }
            }
            else if (current.Y != cend.Y)
            {
                while (current.Y != cend.Y)
                {
                    /*
                    if (cend.Y - current.Y > 0)
                    {
                        current.Y += 1;      //va in alto
                        if (matrix[current.X, current.Y] == 0) current.Y -= 2;
                    }
                    else if ((cend.Y - current.Y) < 0)
                    {
                        current.Y -= 1;      //va in basso
                        if (matrix[current.X, current.Y] == 0) current.Y += 2;
                    }*/
                    if ((cend.Y - current.Y) > 0) current.Y += 1;           //va in basso
                    else if ((cend.Y - current.Y) < 0) current.Y -= 1;      //va in alto

                    path.Add(new Coordinate(current.X, current.Y));
                }
            }

            return path;
        }

        static void ShowMatrix(int[,] matrix) 
        {
            for (int i = 0; i < XCOORD; i++)
            {
                if (i != 0) Console.Write("|");
                Console.WriteLine("\n---------------------");
                for (int j = 0; j < YCOORD; j++)
                    Console.Write("| " + matrix[i, j] + " ");
            }
            Console.WriteLine("|\n---------------------");
        }

        static void ShowMatrixHighlighted(int[,] matrix, List<Coordinate> list, Coordinate cstart, Coordinate cend)
        {
            for (int i = 0; i < XCOORD; i++)
            {
                if (i != 0) Console.Write("|");
                Console.WriteLine("\n---------------------");

                for (int j = 0; j < YCOORD; j++)
                {
                    int flag = 0;
                    if (i == cstart.X && j == cstart.Y)
                    {
                        Console.Write("|S" + matrix[i, j] + "S");
                        flag = 1;
                    }
                    if (i == cend.X && j == cend.Y)
                    {
                        Console.Write("|E" + matrix[i, j] + "E");
                        flag = 1;
                    }

                    if (flag == 0)
                    {
                        foreach (Coordinate c in list)
                        {

                            if (i == c.X && j == c.Y)
                            {
                                Console.Write("|#" + matrix[i, j] + "#");
                                flag = 1;
                            }
                        }
                    }

                    if (flag == 0) Console.Write("| " + matrix[i, j] + " ");
                }
                    
            }
            Console.WriteLine("|\n---------------------");
        }
    }

    class Coordinate
    {
        private int x;
        private int y;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public Coordinate(int x, int y)
        {
            this.X = x;
            this.Y = y; 
        }

        public override string? ToString()
        {
            return "[" + this.X + "," + this.Y + "]";
        }
    }
}
