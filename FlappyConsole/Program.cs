using System;
using System.Threading;
using System.Collections.Generic;

namespace FlappyConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread keyClick = new Thread(new ThreadStart(Push));
            List<Block> blocks = new List<Block>();
            Random rand = new Random();

            int width = 70;
            int height = 30;

            Bird Jacob = new Bird(5, 5, "Jacob");
            int gravity = 3;
            int acc = 1;
            int score = 0;
            blocks.Add(new Block(width - 1, rand.Next(height / 2)));

            Console.ReadKey();
            keyClick.Start();
            NextCycle();

            void NextCycle()
            {
                acc = Math.Clamp(acc+1, -3, 1);
                gravity = Math.Clamp(gravity+acc, -3, 3);
                Jacob.y += gravity;

                try
                {
                    foreach (Block b in blocks)
                    {
                        b.x--;
                        if (b.x < 1)
                        {
                            score++;
                            blocks.Remove(b);
                        }
                    }
                    if (blocks[0].x == 3)
                    {
                        blocks.Add(new Block(width - 1, rand.Next(height / 2)));
                    }
                }
                catch
                {
                    //nevim
                }

                if (Jacob.x == blocks[0].x && (Jacob.y < blocks[0].upperY || Jacob.y >= blocks[0].lowerY)) End();

                if(Jacob.y < 0)
                {
                    Jacob.y = 0;
                    acc = 1;
                    gravity = 0;
                }
                else if(Jacob.y > height)
                {
                    End();
                }

                Draw();
            }

            void Draw()
            {
                Console.Clear();
                Console.Write($"SCORE: {score}");

                Char(Jacob.x, Jacob.y, '>');

                foreach(Block b in blocks)
                {
                    for(int i = 0; i < b.upperY; i++)
                    {
                        Char(b.x, i, '|');
                    }
                    for(int i = b.lowerY; i < height; i++)
                    {
                        Char(b.x, i, '|');
                    }
                }
                
                Console.SetCursorPosition(0, height);
                for (int i = 0; i < width; i++) Console.Write('-');

                Thread.Sleep(100);
                NextCycle();
            }
            void Char(int x, int y, char who)
            {
                Console.SetCursorPosition(x, y);
                Console.Write(who);
            }
            void Push()
            {
                Console.ReadKey();
                acc = -3;
                Push();
            }
            void End()
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("YOU LOST");
                Console.WriteLine($"YOUR SCORE: {score}");
                Environment.Exit(0);
            }
        }
    }
    class Bird
    {
        public int x, y;
        public string birdName;

        public Bird(int x, int y, string birdName)
        {
            this.x = x;
            this.y = y;
            this.birdName = birdName;
        }
    }
    class Block
    {
        public int x, upperY, lowerY;
        Random rand = new Random();

        public Block(int x, int upperY)
        {
            this.x = x;
            this.upperY = upperY;
            lowerY = upperY + rand.Next(4)+8;
        }
    }
}