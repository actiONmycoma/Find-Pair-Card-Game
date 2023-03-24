using System;
using SFML.System;
using SFML.Learning;
using SFML.Window;


namespace Find_Pair_Card_Game
{
    internal class Program : Game
    {
        public enum CardData
        {
            State,
            PosX,
            PosY,
            Width,
            Height,
            ImageId,
            AmountData
        }

        static int[,] cardsArr;
        static int amountCards = 10;
        static int cardWidth = 50;
        static int cardHeigth = 50;

        static int topOffset = 20;
        static int bottomOffset = 20;
        static int space = 30;
        static int cardsInLine = 5;

        static void InitCardsArr()
        {
            cardsArr = new int[amountCards, (int)CardData.AmountData];

            for (int i = 0; i < cardsArr.GetLength(0); i++)
            {
                cardsArr[i, (int)CardData.State] = 1;
                cardsArr[i, (int)CardData.PosX] = (i % cardsInLine) * (cardWidth + space) + bottomOffset;
                cardsArr[i, (int)CardData.PosY] = (i / cardsInLine) * (cardHeigth + space) + topOffset;
                cardsArr[i, (int)CardData.Width] = cardWidth;
                cardsArr[i, (int)CardData.Height] = cardHeigth;
                cardsArr[i, (int)CardData.ImageId] = 0;

            }
        }

        static void Main(string[] args)
        {
            InitWindow(800, 600, "Найди пару");

            InitCardsArr();

            while (true)
            {
                DispatchEvents();

                //for (int i = 0; i < cardsArr.GetLength(0); i++)
                //{
                //    FillRectangle(cardsArr[i, (int)CardData.PosX], cardsArr[i,(int)CardData.PosY],
                //        cardsArr[i,(int)CardData.Width], cardsArr[i,(int)CardData.Height]);
                //}

                DisplayWindow();

                Delay(1);
            }

        }
    }
}
