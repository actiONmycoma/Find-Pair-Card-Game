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

        static void Main(string[] args)
        {
            InitWindow(910, 800, "Найди пару");

            SetFont("Nord-Star-Deco.ttf");

            string backgroundImage = LoadTexture("background.png");
            string blurImage = LoadTexture("blur.png");

            string[] levelsImage =
            {
                LoadTexture("Level_1.png"),
                LoadTexture("Level_2.png"),
                LoadTexture("Level_3.png")
            };

            string[] buttonsImage =
            {
                LoadTexture("noButton.png"),
                LoadTexture("yesButton.png")
            };

            string flipSound = LoadSound("flipcard.wav");
            string clickCardSound = LoadSound("clickCard.wav");
            string clickSound = LoadSound("click.wav");

            bool isGame = true;

            while (isGame)
            {
                int openCards = 0;
                int firstOpenIndex = -1;
                int secondOpenIndex = -1;

                int cardsLeft;
                int clickCount = 0;
                int timeCount = 0;

                int level = 0;

                bool isNewGame = true;
                bool isEndGame = false;
                bool isLose = false;

                //цикл стартового экрана
                while (true)
                {
                    DispatchEvents();

                    if (GetMouseButtonDown(0) == true)
                    {
                        if (MouseX >= 120 && MouseX <= 320 && MouseY >= 350 && MouseY <= 550) level = 1;
                        if (MouseX >= 350 && MouseX <= 550 && MouseY >= 350 && MouseY <= 550) level = 2;
                        if (MouseX >= 580 && MouseX <= 780 && MouseY >= 350 && MouseY <= 550) level = 3;
                    }

                    if (level != 0)
                    {
                        PlaySound(clickSound);

                        if (level == 1) amountCards = 12;
                        if (level == 2) amountCards = 24;
                        if (level == 3) amountCards = 30;

                        cardsLeft = amountCards;

                        break;
                    }

                    DrawSprite(backgroundImage, 0, 0);
                    DrawSprite(blurImage, 0, 0);

                    SetFillColor(255, 255, 255);
                    DrawText(120, 150, "Игра \"НАЙДИ ПАРУ\"", 40);
                    DrawText(120, 250, "Выбирите сложность", 40);

                    DrawSprite(levelsImage[0], 120, 350);
                    DrawSprite(levelsImage[1], 350, 350);
                    DrawSprite(levelsImage[2], 580, 350);

                    DisplayWindow();

                    Delay(1);
                }

                InitCardsArr();
                LoadCardsImage();

                ChangeCardsState(1);                

                //основной игровой цикл
                while (true)
                {
                    DispatchEvents();

                    if (timeCount >= 910)
                    {
                        isLose = true;
                        break;
                    }

                    if (cardsLeft == 0)
                    {
                        isEndGame = true;
                        break;
                    }

                    if (openCards == 2)
                    {
                        bool playSound = false;

                        if (cards[firstOpenIndex, (int)CardData.ImageId] ==
                            cards[secondOpenIndex, (int)CardData.ImageId])
                        {
                            cards[firstOpenIndex, (int)CardData.State] = -1;
                            cards[secondOpenIndex, (int)CardData.State] = -1;

                            cardsLeft -= 2;

                            if (level == 2) timeCount -= 200;
                            if (level == 3) timeCount -= 150;
                        }
                        else
                        {
                            playSound = true;

                            cards[firstOpenIndex, (int)CardData.State] = 0;
                            cards[secondOpenIndex, (int)CardData.State] = 0;
                        }

                        openCards = 0;
                        firstOpenIndex = -1;
                        secondOpenIndex = -1;

                        Delay(900);

                        if (playSound) PlaySound(flipSound);

                        Delay(600);

                    }

                    if (GetMouseButtonDown(0) == true)
                    {
                        int cardIndex = GetCardIndexByMousePosition();

                        if (cardIndex != -1 && cardIndex != firstOpenIndex)
                        {
                            PlaySound(clickCardSound);

                            cards[cardIndex, (int)CardData.State] = 1;

                            openCards++;
                            clickCount++;

                            if (openCards == 1) firstOpenIndex = cardIndex;
                            if (openCards == 2) secondOpenIndex = cardIndex;
                        }
                    }

                    ClearWindow();

                    DrawSprite(backgroundImage, 0, 0);

                    SetFillColor(247, 240, 6);
                    FillRectangle(0, 0, 910 - (timeCount += level / 2 + 1), 30);

                    DrawCards();

                    DisplayWindow();

                    if (isNewGame)
                    {
                        isNewGame = false;
                        ChangeCardsState(0);
                        Delay(5000);
                    }

                    Delay(1);
                }

                //цикл конца игры
                while (true)
                {
                    DispatchEvents();

                    bool inButton = false;

                    if (GetMouseButtonDown(0) == true)
                    {
                        if (MouseX >= 120 && MouseX <= 320 && MouseY >= 550 && MouseY <= 679) inButton = true;

                        if (MouseX >= 450 && MouseX <= 750 && MouseY >= 550 && MouseY <= 679)
                        {
                            isGame = false;
                            inButton = true;
                        }
                    }

                    if (inButton)
                    {
                        PlaySound(clickSound);
                        break;
                    }

                    ClearWindow();

                    DrawSprite(backgroundImage, 0, 0);
                    DrawSprite(blurImage, 0, 0);

                    if (isEndGame)
                    {
                        SetFillColor(255, 255, 255);
                        DrawText(120, 300, "Все карты открыты!", 40);
                        DrawText(120, 350, $"Количество ходов {clickCount / 2}", 40);
                    }

                    if (isLose)
                    {
                        SetFillColor(255, 255, 255);
                        DrawText(120, 300, "Время вышло! Вы проиграли!", 40);
                    }

                    DrawText(120, 450, "Начать игру заново?", 40);

                    DrawSprite(buttonsImage[1], 120, 550);
                    DrawSprite(buttonsImage[0], 450, 550);

                    DisplayWindow();

                    Delay(1);
                }

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
