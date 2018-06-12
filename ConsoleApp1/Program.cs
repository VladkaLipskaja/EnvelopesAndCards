using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        public static void DeclareSize(int number, ref double[,] entities, string entityName)
        {
            string length;
            string width;

            for (int i = 0; i < number; i++)
            {
                Console.WriteLine("{0} #{1}", entityName, i + 1);
                Console.WriteLine("Length: ");
                length = Console.ReadLine();

                if (!Double.TryParse(length, out double l))
                {
                    Console.WriteLine("Sorry, invalid data\nPress any key, please, to exit...");
                    Console.ReadKey();
                    return;
                };

                Console.WriteLine("Width: ");
                width = Console.ReadLine();

                if (!Double.TryParse(width, out double w))
                {
                    Console.WriteLine("Sorry, invalid data\nPress any key, please, to exit...");
                    Console.ReadKey();
                    return;
                };

                if (l < w)
                {
                    Console.WriteLine("Length and width will be swapped due to definition\n");
                    entities[i, 1] = l;
                    entities[i, 0] = w;
                }
                else
                {
                    entities[i, 0] = l;
                    entities[i, 1] = w;
                }

                Console.WriteLine("{0} #{1}: {2}:{3}\n", entityName, i + 1, entities[i, 0], entities[i, 1]);
            }
        }

        public static bool CheckPair(double envelopeLength, double envelopeWidth, double cardLength, double cardWidth)
        {
            if (envelopeLength > cardLength && envelopeWidth > cardWidth)
            {
                return true;
            }

            var d = Math.Sqrt(4 * (Math.Pow(cardLength, 2) + Math.Pow((cardLength / cardWidth), 2) * (Math.Pow(cardLength, 2) - Math.Pow(envelopeLength, 2))));

            if (!d.Equals(Double.NaN) && !d.Equals(Double.PositiveInfinity))
            {
                var x1 = (2 * envelopeWidth + d) / (2 * (Math.Pow((cardLength / cardWidth), 2) + 1));
                var x2 = (2 * envelopeWidth - d) / (2 * (Math.Pow((cardLength / cardWidth), 2) + 1));

                var y1 = x1 * (cardLength / cardWidth);
                var y2 = x2 * (cardLength / cardWidth);

                if ((x1 < envelopeLength && y1 < envelopeWidth && x1 >= 0 && y1 >= 0) || (x2 < envelopeLength && y2 < envelopeWidth && x2 >= 0 && y2 >= 0))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public static bool CheckMatrix(int[,] possibleWays, int numb, out List<int> sequence)
        {
            sequence = new List<int>();
            var usedEnvelopes = new int[numb];
            var failedJoins = new int[numb, numb];


            var currentCard = 0;

            bool foundEnvelope;
            int i;
            int last;

            while (currentCard < numb && currentCard >= 0)
            {
                i = 0;
                foundEnvelope = false;

                while (i < numb && !foundEnvelope)
                {
                    if (possibleWays[currentCard, i] == 1 && usedEnvelopes[i] != 1 && failedJoins[currentCard, i] != -1)
                    {
                        sequence.Add(i);
                        usedEnvelopes[i] = 1;
                        currentCard++;
                        foundEnvelope = true;
                        i = numb;
                    }
                    else
                    {
                        i++;
                    }
                }

                if (!foundEnvelope)
                {
                    if (sequence.Count == 0)
                    {
                        return false;
                    }

                    last = sequence.Last();

                    currentCard--;
                    sequence.Remove(last);

                    failedJoins[currentCard, last] = -1;
                    usedEnvelopes[last] = 0;

                }
            }

            if (sequence.Count == numb)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void Print(int[,] matrix, int numb)
        {
            for (int i = 0; i < numb; i++)
            {
                for (int j = 0; j < numb; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }

                Console.WriteLine();
            }
        }

        public static void PrintResult(double[,] envelopes, double[,] cards, int numb, List<int> sequence)
        {
            Console.WriteLine("Envelope - Card");

            for (int i = 0; i < numb; i++)
            {
                Console.WriteLine("#{0}: {1}:{2} - {3}:{4}", i + 1, envelopes[sequence[i], 0], envelopes[sequence[i], 1], cards[i, 0], cards[i, 1]);
            }
        }

        static void Main(string[] args)
        {
            string number;

            Console.WriteLine("Task: There are n cards and n envelopes (defining like (5:4 (length : width))),\n we should identify is it possible to put all cards per envelopes");

            Console.WriteLine("How much cards and envelopes would you like?");
            number = Console.ReadLine();

            if (Int32.TryParse(number, out int numb))
            {
                Console.WriteLine("Number of cards: {0}\nNumber of envelopes: {0}", numb);
            }
            else
            {
                Console.WriteLine("Sorry, invalid data\nPress any key, please, to exit...");
                Console.ReadKey();
                return;
            }


            var envelopes = new double[numb, 2];
            var cards = new double[numb, 2];

            DeclareSize(numb, ref envelopes, "Envelope");
            DeclareSize(numb, ref cards, "Card");

            var possibleWays = new int[numb, numb];

            for (int i = 0; i < numb; i++) //envelopes
            {
                for (int j = 0; j < numb; j++) //cards
                {
                    if (CheckPair(envelopes[i, 0], envelopes[i, 1], cards[j, 0], cards[j, 1]))
                    {
                        possibleWays[j, i] = 1;
                    }
                }
            }

            Print(possibleWays, numb);

            if (CheckMatrix(possibleWays, numb, out List<int> sequence))
            {
                Console.WriteLine("It is possible:");
                PrintResult(envelopes, cards, numb, sequence);
            }
            else
            {
                Console.WriteLine("Sorry, it is impossible");
            }
        }
    }
}

