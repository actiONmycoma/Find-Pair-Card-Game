using System;
using SFML.System;
using SFML.Learning;
using SFML.Window;


namespace Find_Pair_Card_Game
{
    internal class Program : Game
    {
        static void Main(string[] args)
        {
            InitWindow(800, 600, "Найди пару");

            while (true) 
            {
                DispatchEvents();

                DisplayWindow();

                Delay(1);
            }

        }
    }
}
