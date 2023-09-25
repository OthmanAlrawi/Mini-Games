using System;
using System.Timers;
using System.Media;

namespace Game_1
{
    class Space_Invaders
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(31, 11);
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            Console.Title = "SpaceShip";
            Console.ForegroundColor = ConsoleColor.Red;

            var Timer_1 = new Timer(100);
            Timer_1.Enabled = true;
            // Shot movement
            var Timer_2 = new Timer(150);
            Timer_2.Enabled = true;
            // Refresh rate
            var Timer_3 = new Timer();
            Timer_3.Interval = 2000;
            Timer_3.Enabled = true;
            // Enemy spawn
            var Timer_4 = new Timer();
            Timer_4.Interval = 800;
            Timer_4.Enabled = true;
            // Enemy Movement

            string[,] grid = new string[Console.WindowHeight, Console.WindowWidth];

            string SpaceShip = "H";
            string Shot = "o";
            int[,] Enemy_Positions = new int[2, 10];
            int Max_Enemies = 10;
            int Score = 0;
            int x_SpaceShip = Console.WindowWidth / 2;
            int y_SpaceShip = Console.WindowHeight - 2;

            for (int i = 0; i <= Console.WindowHeight - 1; i++)
            {
                for (int j = 0; j <= Console.WindowWidth - 1; j++)
                {
                    grid[i, j] = " ";
                }
            }

            int t = 0;
            Timer_3.Elapsed += Spawn_Enemy;
            Timer_4.Elapsed += Move_Enemy;

            void Move_Enemy(Object source, ElapsedEventArgs e)
            {
                Timer_4.Interval = Timer_4.Interval - 1.5;
                for (int t = 0; t <= Max_Enemies - 1; t++)
                {
                    if (grid[Enemy_Positions[1, t], Enemy_Positions[0, t]] == "T")
                    {
                        Enemy_Positions[1, t]++;
                        grid[Enemy_Positions[1, t] - 1, Enemy_Positions[0, t]] = " ";
                        grid[Enemy_Positions[1, t], Enemy_Positions[0, t]] = "T";
                    }
                }
            }

            void Spawn_Enemy(Object source, ElapsedEventArgs e)
            {
                Timer_3.Interval = Timer_3.Interval - 15;
                Random random = new Random();
                int x_Enemy = random.Next(Console.WindowWidth);
                int y_Enemy = random.Next(Console.WindowHeight / 4);

                if (t <= Max_Enemies - 1)
                {
                    Enemy_Positions[0, t] = x_Enemy;
                    Enemy_Positions[1, t] = y_Enemy;
                    grid[y_Enemy, x_Enemy] = "T";
                    t++;
                }
                else
                {
                    for (int t = 0; t <= Max_Enemies - 1; t++)
                    {
                        if (grid[Enemy_Positions[1, t], Enemy_Positions[0, t]] == " ")
                        {
                            Enemy_Positions[0, t] = x_Enemy;
                            Enemy_Positions[1, t] = y_Enemy;
                            grid[y_Enemy, x_Enemy] = "T";
                            t++;
                            break;
                        }
                    }
                }
            }
            Timer_2.Elapsed += frame;

            void frame(Object source, ElapsedEventArgs e)
            {
                Console.Clear();

                grid[y_SpaceShip, x_SpaceShip] = SpaceShip;

                for (int i = 0; i <= Console.WindowHeight - 1; i++)
                {
                    for (int j = 0; j <= Console.WindowWidth - 1; j++)
                    {
                        Console.Write(grid[i, j]);
                    }
                }
                Console.SetCursorPosition(0, Console.WindowHeight - 1);
                Console.Write("Score: " + Score);

                for (int t = 0; t <= Console.WindowWidth - 1; t++)
                {
                    if (grid[y_SpaceShip + 1, t] == "T")
                    {
                        Console.Clear();
                        Console.Write("You Lost!");
                        Console.WriteLine("\nYour Score was: " + Score);
                        Timer_2.Elapsed -= frame;
                        Timer_3.Elapsed -= Spawn_Enemy;
                        Timer_4.Elapsed -= Move_Enemy;

                    }
                }
            }
            for (int k = 1; k > 0; k++)
            {

                if (x_SpaceShip == Console.WindowWidth)
                {
                    x_SpaceShip = 0;
                }

                else if (x_SpaceShip == -1)
                {
                    x_SpaceShip = Console.WindowWidth - 1;
                }

                var player_input = Console.ReadKey(true);

                grid[y_SpaceShip, x_SpaceShip] = " ";
                if (player_input.Key == ConsoleKey.D)
                {
                    x_SpaceShip++;
                }

                else if (player_input.Key == ConsoleKey.A)
                {
                    x_SpaceShip--;
                }

                else if (player_input.Key == ConsoleKey.Spacebar)
                {
                    Console.Beep();
                    int x_Shot = x_SpaceShip;
                    int y_Shot = y_SpaceShip - 1;

                    Timer_1.Elapsed += MovingShot;
                    void MovingShot(Object source, ElapsedEventArgs e)
                    {

                        if (y_Shot > -1 && grid[y_Shot, x_Shot] != "T")
                        {
                            grid[y_Shot + 1, x_Shot] = " ";
                            grid[y_Shot, x_Shot] = Shot;
                            y_Shot--;

                        }

                        else if (y_Shot > -1 && grid[y_Shot, x_Shot] == "T")
                        {
                            Score++;
                            grid[y_Shot + 1, x_Shot] = " ";
                            grid[y_Shot, x_Shot] = " ";


                        }

                        else
                        {
                            grid[y_Shot + 1, x_Shot] = " ";
                        }
                    }
                }
            }
        }
    }
}
