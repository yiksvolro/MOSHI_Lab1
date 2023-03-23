using System;

namespace Lab1
{
    class Lab1_Mod
    {
        static int N = 12;
        static void Main(string[] args)
        {

            //Ініціалізація дошки
            int[,] board = InitializeBoard();
            Console.WriteLine("Initialized solution:");
            PrintBoard(board);

            
            double T = 100;
            double coolingRate = 0.0001;

            //Встановлення найкращим розв. ініціалізоване розвязок та обрахунок його правильності
            int[,] bestSolution = new int[N, N];
            CopyBoard(board, bestSolution);
            int bestScore = CalculateScore(bestSolution);

            //Алгоритм відпалу
            while (T > 0.1)
            {
                //Генерація сусіднього розв
                int[,] neighbor = GenerateNeighbor(board);

                //Обрахунок правильності сусіднього розв
                int neighborScore = CalculateScore(neighbor);

                //Різниця між правильністю сусіднього та найкращого розв
                int delta = neighborScore - bestScore;

                //Якщо сусідній розв кращий, то приймаємо його
                if (delta < 0)
                {
                    CopyBoard(neighbor, board);
                    bestScore = neighborScore;

                    //Якщо новий розв краще, ніж найкращий розв, оновлюємо найкращий розв
                    if (bestScore < CalculateScore(bestSolution))
                    {
                        CopyBoard(board, bestSolution);
                    }
                }
                // Якщо сусідній розв гірше, приймаємо його з ймовірністю по температурі
                else if (Math.Exp(-delta / T) > new Random().NextDouble())
                {
                    CopyBoard(neighbor, board);
                }

                // Знижуємо температуру
                T *= 1 - coolingRate;
            }

            // Вивід найращого розвязку
            Console.WriteLine("Best solution:");
            PrintBoard(bestSolution);
        }

        private static int[,] InitializeBoard() //функція ініціалізації дошки з ферзями на головній діагоналі
        {
            var board = new int[N, N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    board[i, j] = i == j ? 1 : 0;
                }
            }
            return board;
        }
        private static int CalculateScore(int[,] board)//функція обрахунку правильності рішення
        {
            int N = board.GetLength(0);
            int[] rowCounts = new int[N];
            int[] colCounts = new int[N];
            int[] diag1Counts = new int[2 * N - 1];
            int[] diag2Counts = new int[2 * N - 1];
            int score = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (board[i, j] == 1)
                    {
                        rowCounts[i]++;
                        colCounts[j]++;
                        diag1Counts[i + j]++;
                        diag2Counts[N - 1 - i + j]++;
                    }
                }
            }
            for (int i = 0; i < N; i++)
            {
                score += (rowCounts[i] * (rowCounts[i] - 1)) / 2;
                score += (colCounts[i] * (colCounts[i] - 1)) / 2;
            }
            for (int i = 0; i < 2 * N - 1; i++)
            {
                score += (diag1Counts[i] * (diag1Counts[i] - 1)) / 2;
                score += (diag2Counts[i] * (diag2Counts[i] - 1)) / 2;
            }
            return score;
        }
        private static int[,] GenerateNeighbor(int[,] board)//функція генерації сусіднього рішення
        {
            int N = board.GetLength(0);
            int[,] neighbor = new int[N, N];
            Array.Copy(board, neighbor, N * N);
            Random rnd = new Random();
            int i = rnd.Next(N);
            int j = rnd.Next(N);
            neighbor[i, j] = 1; //встановлення ферзя в рандомну точку
            for (int k = 0; k < N; k++)
            {
                if (k != j)
                {
                    neighbor[i, k] = 0;//заміна всіх точок на 0 в рядку рандомного ферзя
                }
            }
            return neighbor;
        }
        private static void PrintBoard(int[,] board)//функція виводу дошки на екран
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Console.Write(board[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
        private static void CopyBoard(int[,] source, int[,] destination)//функція копіювання одного розв'язку в інший
        {
            Array.Copy(source, destination, N * N);
        }

    }
}
