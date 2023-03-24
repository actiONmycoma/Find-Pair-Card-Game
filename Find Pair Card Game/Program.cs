﻿using System;
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

            InitCardsArr();

            while (true)
            {
                DispatchEvents();

                ClearWindow();

                DrawCards();

                DisplayWindow();

                Delay(1);
            }

        }

        static void DrawCards()
        {
            for (int i = 0; i < cardsArr.GetLength(0); i++)
            {
                if (cardsArr[i, (int)CardData.ImageId] == 0) SetFillColor(0, 100, 25);
                if (cardsArr[i, (int)CardData.ImageId] == 1) SetFillColor(60, 10, 115);
                if (cardsArr[i, (int)CardData.ImageId] == 2) SetFillColor(30, 150, 0);
                if (cardsArr[i, (int)CardData.ImageId] == 3) SetFillColor(100, 200, 100);
                if (cardsArr[i, (int)CardData.ImageId] == 4) SetFillColor(150, 50, 25);
                if (cardsArr[i, (int)CardData.ImageId] == 5) SetFillColor(200, 80, 150);

                FillRectangle(cardsArr[i, (int)CardData.PosX], cardsArr[i, (int)CardData.PosY],
                    cardsArr[i, (int)CardData.Width], cardsArr[i, (int)CardData.Height]);
            }
        }

        static void InitCardsArr()
        {
            cardsArr = new int[amountCards, Enum.GetValues(typeof(CardData)).Length];

            int[] cardsId = _getCardsId();

            for (int i = 0; i < cardsArr.GetLength(0); i++)
            {
                cardsArr[i, (int)CardData.State] = 1;
                cardsArr[i, (int)CardData.PosX] = (i % cardsInLine) * (cardWidth + space) + bottomOffset;
                cardsArr[i, (int)CardData.PosY] = (i / cardsInLine) * (cardHeigth + space) + topOffset;
                cardsArr[i, (int)CardData.Width] = cardWidth;
                cardsArr[i, (int)CardData.Height] = cardHeigth;
                cardsArr[i, (int)CardData.ImageId] = cardsId[i];
            }
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

            _shuffleId(idArr);

            return idArr;
        }

        static void _shuffleId(int[] arr)
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
