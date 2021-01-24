using System;
using System.Drawing;
using System.Timers;

namespace Game_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetWindowSize(31, 13);
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            

            var Timer_Frame = new Timer(100);
            Timer_Frame.Enabled = true;

            var Timer_Movement = new Timer(100);
            Timer_Movement.Enabled = true;

            string[,] Grid = new string[Console.WindowHeight, Console.WindowWidth];
            int[,] Apple_Positions = new int[2, 100000];
            string Apple = "O";
            string Player_Level = "Noob";
            int g = 0;
            int Score = 0;
            int x_Snake = 1;
            int y_Snake = Console.WindowHeight - 2;
            int x_Apple = 0;
            int y_Apple = 0;
            

            for (int i = 0; i <= Console.WindowHeight - 1; i++)
            {
                for (int j = 0; j <= Console.WindowWidth - 1; j++)
                {
                    if (i == 0 || i == Console.WindowHeight - 1)
                    {
                        Grid[i, j] = "=";
                    }
                    else if (j == 0 || j == Console.WindowWidth - 1)
                    {
                        Grid[i, j] = "|";
                    }
                    else
                    {
                        Grid[i, j] = " ";
                    }
                }
            }

            Timer_Frame.Elapsed += Update_Frame;

            void Update_Frame(Object source, ElapsedEventArgs e)
            {

                for (int i = 0; i <= Console.WindowHeight - 1; i++)
                {
                    for (int j = 0; j <= Console.WindowWidth - 1; j++)
                    {
                        if (i == 0 || i == Console.WindowHeight - 1)
                        {
                            Grid[i, j] = "=";
                        }
                        else if (j == 0 || j == Console.WindowWidth - 1)
                        {
                            Grid[i, j] = "|";
                        }
                
                    } 
                }
                if (Grid[y_Snake, x_Snake] == "=" || Grid[y_Snake, x_Snake] == "|")
                {

                    Timer_Frame.Elapsed -= Update_Frame;
                    Timer_Movement.Elapsed -= Right;
                    Timer_Movement.Elapsed -= Left;
                    Timer_Movement.Elapsed -= Up;
                    Timer_Movement.Elapsed -= Down;

                    if (Score >= 10)
                    {
                        Player_Level = "Peasant";
                    }
                    else if (Score >= 20)
                    {
                        Player_Level = "Not bad";
                    }
                    else if (Score >= 30)
                    {
                        Player_Level = "Ok.";
                    }
                    else if (Score >= 40)
                    {
                        Player_Level = "Pro-Gamer";
                    }
                    Console.Title = Player_Level + ": " + Score;
                    for(int q = 0; q < 1;)
                    {
                        var w = Console.ReadKey(true);
                    }
                    
                }
                for (int p = g - Score + 1; p < g - 1; p++)
                {
                    if(Grid[y_Snake, x_Snake] == Grid[Apple_Positions[1, p], Apple_Positions[0, p]])
                    {
                        Timer_Frame.Elapsed -= Update_Frame;
                        Timer_Movement.Elapsed -= Right;
                        Timer_Movement.Elapsed -= Left;
                        Timer_Movement.Elapsed -= Up;
                        Timer_Movement.Elapsed -= Down;
                        if (Score >= 10)
                        {
                            Player_Level = "Peasant";
                        }
                        else if (Score >= 20)
                        {
                            Player_Level = "Not bad";
                        }
                        else if (Score >= 30)
                        {
                            Player_Level = "Ok.";
                        }
                        else if (Score >= 40)
                        {
                            Player_Level = "Pro-Gamer";
                        }
                        Console.Title = Player_Level + ": " + Score;
                        for (int q = 0; q < 1;)
                        {
                            var r = Console.ReadKey(true);
                        }
                    }
                }

                if (Grid[y_Snake, x_Snake] == Apple)
                {
                    Score++;
                }

                Random x = new Random();

                if (Grid[y_Apple, x_Apple] != Apple)
                {
                    x_Apple = x.Next(1, Console.WindowWidth - 1);
                    y_Apple = x.Next(1, Console.WindowHeight - 1);
                    Grid[y_Apple, x_Apple] = Apple;
                }

                Console.Clear();

                Grid[y_Snake, x_Snake] = ".";

                for (int i = 0; i <= Console.WindowHeight - 1; i++)
                {
                    for (int j = 0; j <= Console.WindowWidth - 1; j++)
                    {
                        Console.Write(Grid[i, j]);
                    }
                }
                Console.Title = "Snake " + Score;
                Apple_Positions[0, g] = x_Snake;
                Apple_Positions[1, g] = y_Snake;

                for(int l = 0; l <= g - Score; l++)
                {
                    Grid[Apple_Positions[1, l], Apple_Positions[0, l]] = " ";
                }

                for (int p = g - Score + 1; p <= g; p++)
                {
                    Grid[Apple_Positions[1, p], Apple_Positions[0, p]] = ".";
                }
                g++;
            }

            void Right(Object source, ElapsedEventArgs e)
            {
                x_Snake++;
            }

            void Left(Object source, ElapsedEventArgs e)
            {
                x_Snake--;
            }

            void Up(Object source, ElapsedEventArgs e)
            {
                y_Snake--;
            }

            void Down(Object source, ElapsedEventArgs e)
            {
                if (y_Snake != 0 && y_Snake != Console.WindowHeight)
                {
                    y_Snake++;
                }
            }

            for (int k = 0; k < 1; )
            {

                var Player_Input = Console.ReadKey(true);

                if (Player_Input.Key == ConsoleKey.D && Grid[y_Snake, x_Snake + 1] != ".")
                {
                    Timer_Movement.Elapsed -= Right;
                    Timer_Movement.Elapsed -= Left;
                    Timer_Movement.Elapsed -= Up;
                    Timer_Movement.Elapsed -= Down;
                    Timer_Movement.Elapsed += Right;
                }

                else if (Player_Input.Key == ConsoleKey.A && Grid[y_Snake, x_Snake - 1] != ".")
                {
                    Timer_Movement.Elapsed -= Right;
                    Timer_Movement.Elapsed -= Left;
                    Timer_Movement.Elapsed -= Up;
                    Timer_Movement.Elapsed -= Down;
                    Timer_Movement.Elapsed += Left;
                }

                else if (Player_Input.Key == ConsoleKey.W && Grid[y_Snake - 1, x_Snake] != ".")
                {
                    Timer_Movement.Elapsed -= Right;
                    Timer_Movement.Elapsed -= Left;
                    Timer_Movement.Elapsed -= Up;
                    Timer_Movement.Elapsed -= Down;
                    Timer_Movement.Elapsed += Up;
                }

                else if (Player_Input.Key == ConsoleKey.S && Grid[y_Snake + 1, x_Snake] != ".")
                {
                    Timer_Movement.Elapsed -= Right;
                    Timer_Movement.Elapsed -= Left;
                    Timer_Movement.Elapsed -= Up;
                    Timer_Movement.Elapsed -= Down;
                    Timer_Movement.Elapsed += Down;
                }
                else if (Player_Input.Key == ConsoleKey.Spacebar)
                {
                    Timer_Frame.Enabled = !Timer_Frame.Enabled;
                    Timer_Movement.Enabled = !Timer_Movement.Enabled;
                }
            }
        }
    }
}
