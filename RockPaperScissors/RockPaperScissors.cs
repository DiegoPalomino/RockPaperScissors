using System;
using System.Collections.Generic;

namespace RockPaperScissors
{
    /// <summary>
    /// Class for the game Rock, Paper, Scissors
    /// Diego Palomino   2021/05/17  V1 Creation
    /// </summary>
    class RockPaperScissors
    {
        public enum Action { Rock, Paper, Flamethrower, Scissors };
        public enum Result { Tie, Player1Won, Player2Won }

        const int MAX_PLAYERS = 2;
        const int POINTS_TO_WIN = 3;

        static string _player1 = string.Empty;
        static string _player2 = string.Empty;

        static Random rand = new Random();

        static int opponopponent1Points = 0;
        static int opponopponent2Points = 0;

        static bool isRandomSelectionActivate = false;

        /// <summary>
        /// Main for the game Rock, Paper, Scissors
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu();
            }
        }

        /// <summary>
        /// Fonction menu for console execution
        /// </summary>
        /// <returns>Resultat boolean of the chosen option</returns>
        private static bool MainMenu()
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Play");
            Console.WriteLine("2) Options");
            Console.WriteLine("3) Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Play();
                    return true;
                case "2":
                    Options();
                    return true;
                case "3":
                    return false;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Main method of game execution
        /// Can be played between 2 players or one player and the computer
        /// </summary>
        private static void Play()
        {
            Console.WriteLine();
            ResetValues();
            Console.WriteLine("Hello and welcome to Rock, Paper, Scissors game");
            Console.Write("Please enter the number of players: ");
            var players = ValidatePlayersNumber();

            Console.Write("Please enter the players name number 1: ");
            _player1 = Console.ReadLine();

            if (players > 1)
            {
                Console.Write("Please enter the players name number {0}: ", players);
                _player2 = Console.ReadLine();
            }

            List<Action> computerActions = new List<Action>();
            var round = 1;

            while (opponopponent1Points < POINTS_TO_WIN && opponopponent2Points < POINTS_TO_WIN)
            {
                if((opponopponent1Points + 1) == POINTS_TO_WIN)
                    Console.WriteLine("One point to win {0}", _player1);
                else if ((opponopponent2Points + 1) == POINTS_TO_WIN)
                    Console.WriteLine("One point to win {0}", !string.IsNullOrEmpty(_player2) ? _player2 : "Computer");

                Console.WriteLine("Round {0} Begins", round);
                Console.WriteLine("{0}, which hand do you want to choose(Rock, Paper, Scissors, Flamethrower) + Enter key? ", _player1);
                var playerAction = GetPlayerAction();
                Action player2Action = default(Action);
                Action computerAction = default(Action);

                if (!string.IsNullOrEmpty(_player2))
                {
                    Console.WriteLine("{0}, which hand do you want to choose(Rock, Paper, Scissors, Flamethrower)+ Enter key? ", _player2);
                    player2Action = GetPlayerAction();
                    TryPlayOpponenent(playerAction, player2Action, false);
                }
                else
                {
                    var lastComputerAction = computerActions.Count > 0 ? computerActions[computerActions.Count - 1] :
                        Action.Scissors;
                    computerAction = GetComputerAction(lastComputerAction);
                    computerActions.Add(computerAction);
                    TryPlayOpponenent(playerAction, computerAction, true);
                }
                round++;

                Console.WriteLine("{0}, you picked: {1}", _player1, playerAction.ToString());
                if (!string.IsNullOrEmpty(_player2))
                    Console.WriteLine("{0}, you picked: {1}", _player2, player2Action.ToString());
                else
                    Console.WriteLine("The computer picked: {0}", computerAction.ToString());
            }

            Console.WriteLine("Results: {0} {1} points, {2} {3} points", _player1, opponopponent1Points, 
                !string.IsNullOrEmpty(_player2) ? _player2 : "Computer", opponopponent2Points);
            bool isPlayerWinner = opponopponent1Points > opponopponent2Points;
            Console.WriteLine("{0} won the game!", isPlayerWinner ? _player1 : 
                !string.IsNullOrEmpty(_player2) ? _player2 : "Computer");
        }

        /// <summary>
        /// Main method to reset value of global properties
        /// </summary>
        private static void ResetValues() 
        { 
            opponopponent1Points = 0;
            opponopponent2Points = 0;
            _player1 = string.Empty;
            _player2 = string.Empty;
        }

        /// <summary>
        /// Method to analyze the result of each play and determine who wins the point
        /// </summary>
        /// <param name="playerAction"></param>
        /// <param name="player2Action"></param>
        private static void TryPlayOpponenent(Action playerAction, Action player2Action, bool isComputer)
        {
            switch(CalculateResult(playerAction, player2Action))
            {
				case Result.Player1Won:
					Console.WriteLine("{0}, you won the round! You gained a point.", _player1);
					opponopponent1Points++;
					break;
				case Result.Player2Won:
                    if(isComputer)
					    Console.WriteLine("Computer won the round! Computer gained a point.");
                    else
                        Console.WriteLine("{0}, you won the round! You gained a point.", _player2);

                    opponopponent2Points++;
					break;
				case Result.Tie:
					Console.WriteLine("Round tied. The round is restarted.");
					break;
			}
        }

        /// <summary>
        /// Function that calculates the result, according to the rules established in the game
        /// </summary>
        /// <param name="opp1"></param>
        /// <param name="opp2"></param>
        /// <returns>Result: Tie, Player1Won, Player2Won</returns>
        public static Result CalculateResult(Action opp1, Action opp2)
        {
            switch (opp1)
            {
                case Action.Rock:
                    switch (opp2)
                    {
                        case Action.Rock: return Result.Tie;
                        case Action.Paper: return Result.Player2Won;
                        case Action.Scissors: return Result.Player1Won;
                        case Action.Flamethrower: return Result.Player1Won;
                    }
                    break;
                case Action.Paper:
                    switch (opp2)
                    {
                        case Action.Rock: return Result.Player1Won;
                        case Action.Paper: return Result.Tie;
                        case Action.Scissors: return Result.Player2Won;
                        case Action.Flamethrower: return Result.Player2Won;
                    }
                    break;
                case Action.Scissors:
                    switch (opp2)
                    {
                        case Action.Rock: return Result.Player2Won;
                        case Action.Paper: return Result.Player1Won;
                        case Action.Scissors: return Result.Tie;
                        case Action.Flamethrower: return Result.Player1Won;
                    }
                    break;
                case Action.Flamethrower:
                    switch (opp2)
                    {
                        case Action.Rock: return Result.Player2Won;
                        case Action.Paper: return Result.Player1Won;
                        case Action.Scissors: return Result.Player2Won;
                        case Action.Flamethrower: return Result.Tie;
                    }
                    break;
            }

            throw new Exception(string.Format("Unhandled action pair occured: {0}, {1}", opp1, opp2));
        }

        /// <summary>
        /// Function that determines the behavior of the computer game
        /// </summary>
        /// <param name="action"></param>
        /// <returns>Action: Rock, Paper, Flamethrower, Scissors</returns>
        private static Action GetComputerAction(Action action)
        {
            if (isRandomSelectionActivate)
            { 
                return (Action)rand.Next((int)Action.Rock, (int)Action.Scissors);
            }
            else
            {
                var computerAction = action switch
                {
                    Action.Rock => Action.Paper,
                    Action.Paper => Action.Flamethrower,
                    Action.Flamethrower => Action.Scissors,
                    Action.Scissors => Action.Rock,
                    _ => throw new ArgumentOutOfRangeException(nameof(action))
                };

                return computerAction;
            }
        }

        /// <summary>
        /// Function that validates the selection made by the player
        /// </summary>
        /// <returns>Action: Rock, Paper, Flamethrower, Scissors</returns>
        private static Action GetPlayerAction()
        {
            Action result;
            do
            {
                var input = ReadLine();
                if (Action.TryParse(input, true, out result))
                {
                    return result;
                }
                else
                {
                    Console.WriteLine("Invalid action {0}. Please input Rock, Paper, Scissors or Flamethrower + Enter key?", 
                        input);
                }
            } while (true);
        }

        /// <summary>
        /// Function to hide the selection of the players and not make it visible to the other player
        /// </summary>
        /// <returns>Player selection</returns>
        private static string ReadLine()
        {
            string selection = string.Empty;
            while (true)
            {
                var key = System.Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                    break;
                selection += key.KeyChar;
            }

            return selection;
        }

        /// <summary>
        /// Submenu for Options choice of Main menu
        /// </summary>
        /// <returns>Resultat boolean of the chosen option</returns>
        private static bool Options()
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Change mode random to computer opponent");
            Console.WriteLine("2) Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    if (isRandomSelectionActivate)
                        isRandomSelectionActivate = false;
                    else
                        isRandomSelectionActivate = true;

                    return true;
                case "2":
                    return true;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Function to validate the number of players entered to start the game
        /// </summary>
        /// <returns>Number of players</returns>
        private static int ValidatePlayersNumber()
        {
            int result;
            do
            {
                var input = Console.ReadLine();
                if (Int32.TryParse(input, out result) && (result > 0 && result <= MAX_PLAYERS))
                {
                    return result;
                }
                else
                {
                    Console.WriteLine("Invalid number of players. Please enter a valid number!");
                }
            } while (true);
        }
    }
}
