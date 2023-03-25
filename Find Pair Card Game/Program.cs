using System;
using SFML.System;
using SFML.Learning;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;


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

        static int[,] cards;
        static string[] imageNames;

        static int amountCards = 30;
        static int cardWidth = 120;
        static int cardHeigth = 120;

        static int topOffset = 60;
        static int bottomOffset = 20;
        static int space = 30;
        static int cardsInLine = 6;

        static string backgroundImage = LoadTexture("background.png");

        static void Main(string[] args)
        {
            InitWindow(910, 800, "Найди пару");

            SetFont("arialmt.ttf");

            int openCards = 0;
            int firstOpenIndex = -1;
            int secondOpenIndex = -1;

            int cardsLeft = amountCards;
            int clickCount = 0;

            bool isNewGame = true;
            bool isEndGame = false;

            InitCardsArr();
            LoadCardsImage();

            while (true)
            {
                DispatchEvents();

                if (isNewGame)
                {
                    ChangeCardsState(1);
                }

                if (cardsLeft == 0) isEndGame = true;

                if (openCards == 2)
                {
                    if (cards[firstOpenIndex, (int)CardData.ImageId] ==
                        cards[secondOpenIndex, (int)CardData.ImageId])
                    {
                        cards[firstOpenIndex, (int)CardData.State] = -1;
                        cards[secondOpenIndex, (int)CardData.State] = -1;

                        cardsLeft -= 2;
                    }
                    else
                    {
                        cards[firstOpenIndex, (int)CardData.State] = 0;
                        cards[secondOpenIndex, (int)CardData.State] = 0;
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
                        cards[cardIndex, (int)CardData.State] = 1;

                        openCards++;
                        clickCount++;

                        if (openCards == 1) firstOpenIndex = cardIndex;
                        if (openCards == 2) secondOpenIndex = cardIndex;
                    }
                }

                ClearWindow();

                DrawSprite(backgroundImage, 0, 0);

                DrawCards();

                if (isEndGame)
                {
                    SetFillColor(255, 255, 255);
                    DrawText(300, 300, "Все карты открыты!", 40);
                    DrawText(300, 350, $"Количество ходов {clickCount / 2}", 40);
                }

                DisplayWindow();

                if (isNewGame)
                {
                    isNewGame = false;
                    ChangeCardsState(0);
                    Delay(5000);
                }

                Delay(1);
            }

        }

        static void DrawCards()
        {
            for (int i = 0; i < cards.GetLength(0); i++)
            {
                if (cards[i, (int)CardData.State] == 1)//лицом вверх
                {
                    DrawSprite(imageNames[cards[i, (int)CardData.ImageId]],
                        cards[i, (int)CardData.PosX], cards[i, (int)CardData.PosY]);
                }

                if (cards[i, (int)CardData.State] == 0)//рубашкой вверх
                {
                    DrawSprite(imageNames[0], cards[i, (int)CardData.PosX], cards[i, (int)CardData.PosY]);
                }
            }
        }

        static void InitCardsArr()
        {
            cards = new int[amountCards, Enum.GetValues(typeof(CardData)).Length];

            int[] cardsId = _getCardsId();

            for (int i = 0; i < cards.GetLength(0); i++)
            {
                cards[i, (int)CardData.State] = 0;
                cards[i, (int)CardData.PosX] = (i % cardsInLine) * (cardWidth + space) + bottomOffset;
                cards[i, (int)CardData.PosY] = (i / cardsInLine) * (cardHeigth + space) + topOffset;
                cards[i, (int)CardData.Width] = cardWidth;
                cards[i, (int)CardData.Height] = cardHeigth;
                cards[i, (int)CardData.ImageId] = cardsId[i];
            }
        }

        static void LoadCardsImage()
        {
            imageNames = new string[16];

            imageNames[0] = LoadTexture("Icon_close.png");

            for (int i = 1; i < imageNames.Length; i++)
            {
                imageNames[i] = LoadTexture($"Icon_{i}.png");
            }
        }

        static void ChangeCardsState(int state)
        {
            for (int i = 0; i < cards.GetLength(0); i++)
            {
                cards[i, (int)CardData.State] = state;
            }
        }

        static int GetCardIndexByMousePosition()
        {
            for (int i = 0; i < cards.GetLength(0); i++)
            {
                bool isInCard = MouseX >= cards[i, (int)CardData.PosX] && MouseY >= cards[i, (int)CardData.PosY] &&
                    MouseX <= cards[i, (int)CardData.PosX] + cards[i, (int)CardData.Width] &&
                    MouseY <= cards[i, (int)CardData.PosY] + cards[i, (int)CardData.Height];

                if (isInCard) return i;
            }

            return -1;
        }

        static int[] _getCardsId()
        {
            int[] idArr = new int[amountCards];

            int id = 1;

            for (int i = 0; i < idArr.Length; i += 2)
            {
                idArr[i] = id++;
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
