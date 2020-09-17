using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_of_Nim
{
    class Program
    {
        // Variables
        private static readonly Random rand = new Random();


        static void WriteMenuUI()
        {
            Console.WriteLine("");
            Console.WriteLine("Welcome to the game of Nim!");
            Console.WriteLine("Please select one of the following options: ");
            Console.WriteLine("1. Play the game");
            Console.WriteLine("2. Quit game");
            Console.WriteLine("");
        }

        static int ReadMenuInt()
        {
            int input;

            try
            {
                Console.WriteLine("");
                Console.Write("Please enter your input: ");
                input = Convert.ToInt32(Console.ReadLine());
                return input;
            }
            
            catch (Exception exception)
            {
                Console.WriteLine("");
                Console.WriteLine("Invalid input detected!" + exception);
            }

            return 2;
        }

        static int ChooseVariation()
        {
            int input;

            Console.WriteLine("");
            Console.WriteLine("Choose the game variation: ");
            Console.WriteLine("1. Normal - Player that takes last win");
            Console.WriteLine("2. Misère - Player that takes last lose");
            input = ReadMenuInt();

            while ( (input < 1) || (input > 2))
            {
                Console.WriteLine("");
                Console.WriteLine("Wrong input detected! Please enter the correct input" + "\n");
                Console.WriteLine("Choose the game variation: ");
                Console.WriteLine("1. Normal - Player that takes last win");
                Console.WriteLine("2. Misère - Player that takes last lose");
                input = ReadMenuInt();
            }

            return input;
        }

        static void PlayGame(int variation)
        {
            // Variables
            int gameStatus = 1;
            int player = 1;
            int totalPieces = 0;
            int pileNumber;

            List<Piles> piles = new List<Piles>();


            // Setting up the game
            pileNumber = GenerateRandom(1); // Generating random number between 2-5

            CreatePiles(piles ,pileNumber);   // Generate the piles


            while (gameStatus != 2) // Start looping the game
            {

                // Show the current state
                PrintState(piles);  // Prints the current state

                // Get the valid moves and Update the state
                GetMove(piles, player);

                totalPieces = GetTotalPieces(piles);

                // Check the win status
                if (CheckWin(totalPieces, player, variation) == true) gameStatus = 2;
                
             
                // Switch the players
                player = SwapTurn(player);
            }
        }

        static int GenerateRandom(int input)
        {
            int total = 0;

            if (input == 1)
            {
                total = rand.Next(2, 6);    // Generates number from 2 to 5
            }
        
            else if (input == 2)
            {
                int number = rand.Next(1, 4); // Generate number from 1 to 3
                total = (2 * number) + 1;
            }

            return total;
        }
  
        static void CreatePiles(List<Piles> piles, int total)
        {
            Console.WriteLine("");
            Console.WriteLine("Generating the piles...");
            Console.WriteLine("");
            try
            {
                if ((total < 2) || (total > 5)) throw new InvalidOperationException();

                for (int i = 0; i < total; i++)
                {
                    Piles pile = new Piles(GenerateRandom(2));
                    piles.Add(pile);
                }
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Error! wrong random number generated");
            }
        }

        static void PrintState(List<Piles> piles)
        {
            int num = 1;
            int n = piles.Count;

            Console.WriteLine("");
            Console.WriteLine("Current state: " + "\n");

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"Pile {num}: " + piles[i].GetPieces());
                num++;
            }

        }

        static void GetMove(List<Piles> piles, int player)
        {
            int pileInput;
            int pieceInput;


            Console.WriteLine("");
            Console.Write($"Current turn: Player {player}");
            pileInput = GetPileInput(piles);

            Console.WriteLine("");
            pieceInput = GetPieceInput(piles, pileInput);

            UpdateState(piles, pileInput, pieceInput);  // Update the state of the piles

        }

        static int GetPileInput(List<Piles> piles)
        {
            int input;
            int number;
            int n = piles.Count();

            Console.WriteLine("");
            Console.Write("Please choose one of the piles: ");
            input = Convert.ToInt32(Console.ReadLine());

            number = input;

            while ( (number < 1) || (number > n) )
            {
                Console.WriteLine("");
                Console.WriteLine("Invalid input detected, please make sure the pile you chose is available");
                Console.WriteLine("");
                Console.Write("Please choose one of the piles: ");
                input = Convert.ToInt32(Console.ReadLine());
                number = input;              
            }

            input -= 1; // Transformed into index

            while (piles[input].GetPieces() == 0)
            {
                Console.WriteLine("");
                Console.WriteLine("Invalid input detected, please make sure the pile you chose is has more than 0 piece");
                Console.WriteLine("");
                Console.Write("Please choose one of the piles: ");
                input = Convert.ToInt32(Console.ReadLine());
                input -= 1;
            }

            return input;

        }

        static int GetPieceInput(List<Piles> piles, int pileInput)
        {
            int input;
            int num = piles[pileInput].GetPieces();
            int chosenPile = pileInput + 1;

            Console.WriteLine("");
            Console.WriteLine($"You chose pile {chosenPile}");

            Console.WriteLine($"Available piece: {num} ");
            Console.Write("How many pieces do you want to take: ");
            input = Convert.ToInt32(Console.ReadLine());

            while ( (input < 1) || (input > num))
            {
                Console.WriteLine("");
                Console.WriteLine("Invalid input, you must pick at least 1 piece and no more than the total of pieces available" + "\n");
                Console.WriteLine($"Available piece: {num} ");
                Console.Write("How many pieces do you want to take: ");
                input = Convert.ToInt32(Console.ReadLine());
            }
         
            return input;
        }

        static void UpdateState(List<Piles> piles, int pileInput, int pieceInput)
        {
            Console.WriteLine("");
            Console.WriteLine("Move verified, piles has been updated" + "\n");

            piles[pileInput].UpdatePieces(pieceInput);
        }

        static bool CheckWin(int totalPieces, int player, int variation)
        {
            if (totalPieces == 0)
            {
                if (variation == 1) // Normal play - Player takes last win
                {
                    Console.WriteLine("");
                    Console.WriteLine("Variation: Normal Play - player that takes last wins");
                    Console.WriteLine($" Player {player} wins the game! " + "\n");                  
                    return true;
                }
                else if (variation == 2) // Misere Play - Player takes last loses
                {
                    if (player == 1) player = 2;
                    else player = 1;

                    Console.WriteLine("");
                    Console.WriteLine("Variation: Misère - player that takes last loses");
                    Console.WriteLine($" Player {player} wins the game! " + "\n");
                    return true;
                }
            }
            return false;
        }

        static int SwapTurn(int player)
        {
            if (player == 1) return 2;

            else return 1;

        }

        static int GetTotalPieces(List<Piles> piles)
        {
            int total = 0;

            for (int i = 0; i < piles.Count; i++)
            {
                total += piles[i].GetPieces(); 
            }

            return total;
        }

        static void Main(string[] args)
        {
            int menuState = 0;

            while (menuState != 2)
            {
                // Variables
                int variation = 0;

                WriteMenuUI();
                int menuInput = ReadMenuInt();
                Console.WriteLine("");

                switch (menuInput)
                {
                    case 1:
                        Console.WriteLine("");
                        variation = ChooseVariation();
                        PlayGame(variation);
                        break;

                    case 2:                   
                        Console.WriteLine("Quitting game.... Thanks for playing!");
                        Console.WriteLine("");
                        menuState = 2;
                        break;

                    default:
                        Console.WriteLine("Unknown input detected, please enter the correct input" + "\n");
                        break;
                }
            }
        }
    }
}
