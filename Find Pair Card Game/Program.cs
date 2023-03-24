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
            ImageId
        }

        static int[,] cardsArr;
        static int amountCards = 30;
        static int cardWidth = 50;
        static int cardHeigth = 50;

        static int topOffset = 20;
        static int bottomOffset = 20;
        static int space = 30;
        static int cardsInLine = 5;


        static void Main(string[] args)
        {
            InitWindow(800, 600, "Найди пару");

            int openCards = 0;
            int firstOpenIndex = -1;
            int secondOpenIndex = -1;

            bool isNewGame = true;

            InitCardsArr();

            while (true)
            {
                DispatchEvents();

                if (isNewGame)
                {
                    ChangeCardsState();
                }


                if (openCards == 2)
                {
                    if (cardsArr[firstOpenIndex, (int)CardData.ImageId] ==
                        cardsArr[secondOpenIndex, (int)CardData.ImageId])
                    {
                        cardsArr[firstOpenIndex, (int)CardData.State] = -1;
                        cardsArr[secondOpenIndex, (int)CardData.State] = -1;
                    }
                    else
                    {
                        cardsArr[firstOpenIndex, (int)CardData.State] = 0;
                        cardsArr[secondOpenIndex, (int)CardData.State] = 0;
                    }

                    openCards = 0;
                    firstOpenIndex = -1;
                    secondOpenIndex = -1;

                    Delay(1500);
                }

                if (GetMouseButtonDown(0) == true)
                {
                    int cardIndex = GetCardIndexByMousePosition();

                    if (cardIndex != -1 && cardIndex != firstOpenIndex)
                    {
                        cardsArr[cardIndex, (int)CardData.State] = 1;

                        openCards++;

                        if (openCards == 1) firstOpenIndex = cardIndex;
                        if (openCards == 2) secondOpenIndex = cardIndex;
                    }
                }

                ClearWindow();

                DrawCards();

                DisplayWindow();

                if (isNewGame)
                {
                    isNewGame = false;
                    ChangeCardsState();
                    Delay(5000);
                }

                Delay(1);
            }

        }

        static void DrawCards()
        {
            for (int i = 0; i < cardsArr.GetLength(0); i++)
            {
                if (cardsArr[i, (int)CardData.State] == 1)//лицом вверх
                {
                    if (cardsArr[i, (int)CardData.ImageId] == 0) SetFillColor(0, 100, 25);
                    if (cardsArr[i, (int)CardData.ImageId] == 1) SetFillColor(60, 10, 115);
                    if (cardsArr[i, (int)CardData.ImageId] == 2) SetFillColor(30, 150, 0);
                    if (cardsArr[i, (int)CardData.ImageId] == 3) SetFillColor(100, 200, 100);
                    if (cardsArr[i, (int)CardData.ImageId] == 4) SetFillColor(150, 50, 25);
                    if (cardsArr[i, (int)CardData.ImageId] == 5) SetFillColor(200, 80, 150);
                }

                if (cardsArr[i, (int)CardData.State] == 0)//рубашкой вверх
                {
                    if (cardsArr[i, (int)CardData.ImageId] == 5) SetFillColor(255, 255, 255);
                }

                if (cardsArr[i, (int)CardData.State] != -1)
                {
                    FillRectangle(cardsArr[i, (int)CardData.PosX], cardsArr[i, (int)CardData.PosY],
                                cardsArr[i, (int)CardData.Width], cardsArr[i, (int)CardData.Height]);
                }

                SetFillColor(255, 255, 255);
            }
        }

        static void InitCardsArr()
        {
            cardsArr = new int[amountCards, Enum.GetValues(typeof(CardData)).Length];

            int[] cardsId = _getCardsId();

            for (int i = 0; i < cardsArr.GetLength(0); i++)
            {
                cardsArr[i, (int)CardData.State] = 0;
                cardsArr[i, (int)CardData.PosX] = (i % cardsInLine) * (cardWidth + space) + bottomOffset;
                cardsArr[i, (int)CardData.PosY] = (i / cardsInLine) * (cardHeigth + space) + topOffset;
                cardsArr[i, (int)CardData.Width] = cardWidth;
                cardsArr[i, (int)CardData.Height] = cardHeigth;
                cardsArr[i, (int)CardData.ImageId] = cardsId[i];
            }
        }

        static void ChangeCardsState()
        {
            for (int i = 0; i < cardsArr.GetLength(0); i++)
            {
                if (cardsArr[i, (int)CardData.State] == 0) 
                    cardsArr[i, (int)CardData.State] = 1;
                else if (cardsArr[i, (int)CardData.State] == 1)
                    cardsArr[i, (int)CardData.State] = 0;
            }
        }

        static int GetCardIndexByMousePosition()
        {
            for (int i = 0; i < cardsArr.GetLength(0); i++)
            {
                bool isInCard = MouseX >= cardsArr[i, (int)CardData.PosX] && MouseY >= cardsArr[i, (int)CardData.PosY] &&
                    MouseX <= cardsArr[i, (int)CardData.PosX] + cardsArr[i, (int)CardData.Width] &&
                    MouseY <= cardsArr[i, (int)CardData.PosY] + cardsArr[i, (int)CardData.Height];

                if (isInCard) return i;
            }

            return -1;
        }

        static int[] _getCardsId()
        {
            Random rnd = new Random();

            int[] idArr = new int[amountCards];

            for (int i = 0; i < idArr.Length; i += 2)
            {
                idArr[i] = rnd.Next(6);
                idArr[i + 1] = idArr[i];
            }

            _shuffleCardsId(idArr);

            return idArr;
        }

        static void _shuffleCardsId(int[] arr)
        {
            Random rnd = new Random();

            int shuffleCount = 100;

            for (int i = 0; i < shuffleCount; i++)
            {
                for (int j = 0; j < arr.Length; j++)
                {
                    int randomIndex = rnd.Next(arr.Length);
                    int tmp = arr[j];
                    arr[j] = arr[randomIndex];
                    arr[randomIndex] = tmp;
                }
            }
        }
    }
}
