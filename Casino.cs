namespace Casino
{
	public static class Games
	{
		public const double StartBalance = 100; // Стартовый баланс
		static double balance = StartBalance;
		public const double BasicCoefficient = 1.95;
		public const double BadEndBalance = 0;
		public const double GoodEndBalance = 1000;
		public static double ShowBalance() =>
		balance;
		public static void BalanceChange(double difference) =>
		balance += difference;

		public static double UserBet()
		{
			Console.WriteLine(Messages.BetQuestion);
			double.TryParse(Console.ReadLine(), out double bet);
			while (bet > ShowBalance() || bet <= 0)
			{
				Messages.BetOverrun();
				double.TryParse(Console.ReadLine(), out bet);
			}
			return bet;
		}

		public static string UserChoice(params string[] options)
		{
			string input;
			while (true)
			{
				input = Console.ReadLine()?.Trim().ToLower() ?? throw new ArgumentOutOfRangeException();
				if (options.Contains(input))
				{
					return input;
				}
				Messages.ErrorMessage();
			}
		}

		public class Coinflip
		{

			public static (string choice, double bet) UserCoinflipConfig()
			{
				double bet = UserBet();
				Console.WriteLine(Messages.HeadsOrTailsQuestion);
				string choice = UserChoice("орел", "решка");
				return (choice, bet);
			}

			// Генерируем исход игры 
			public static string GenerateComputerCoinflip() =>
			(Random.Shared.Next(0, 2) == 1) ? "решка" : "орел";

			public static double WinReward(double bet) =>
			bet * BasicCoefficient;

			public static void OutputMessages()
			{
				if (Settings.IsShowRulesEnabled())
				{
					Console.WriteLine(Messages.HeadsOrTailsStartMessage);
				}
			}

			public static int GameResult(string userChoice, string computerOutcome)
			{
				if (userChoice == computerOutcome)
				{
					return 1;
				}
				else
				{
					return 0;
				}
			}

			public static void CoinflipResultMessage(int gameResult, string computerOutcome, double bet, double winReward)
			{
				switch (gameResult)
				{
					case 0:
						BalanceChange(-bet);
						Messages.GameResult("coinflip", 0, bet, computerOutcome);
						break;
					case 1:
						BalanceChange(+winReward);
						Messages.GameResult("coinflip", 1, winReward);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			public static void PlayCoinflipRound()
			{
				OutputMessages();
				var computerOutcome = GenerateComputerCoinflip();
				(string userChoice, double bet) = UserCoinflipConfig();
				CoinflipResultMessage(GameResult(userChoice, computerOutcome), computerOutcome, bet, WinReward(bet));
			}
		}

		public class RockScissorsPaper
		{
			public enum Rsp
			{
				Rock = 0,
				Paper = 1,
				Scissors = 2
			}
			public static (Rsp randomRsp, string ruTranslate) GenerateComputerChoice()
			{
				Rsp randomRsp = (Rsp)Random.Shared.Next(0, 3);
				string ruTranslate = randomRsp switch
				{
					Rsp.Rock => "камень",
					Rsp.Paper => "бумага",
					Rsp.Scissors => "ножницы",
					_ => ""
				};
				return (randomRsp, ruTranslate);
			}

			public static void OutputMessages()
			{
				if (Settings.IsShowRulesEnabled())
				{
					Console.WriteLine(Messages.RspStartMessage);
				}
				Console.WriteLine(Messages.RspGameMessage);
			}

			public static Rsp RspOutput(string choice) => choice switch
			{
				"камень" => Rsp.Rock,
				"бумага" => Rsp.Paper,
				"ножницы" => Rsp.Scissors,
				_ => throw new ArgumentOutOfRangeException()
			};


			public static (Rsp userChoice, double bet) UserRSPConfig()
			{
				Console.WriteLine(Messages.RspChoiceQuestion);
				string choice = UserChoice("камень", "ножницы", "бумага");
				return (RspOutput(choice), UserBet());
			}

			public static double WinReward(double bet) =>
			bet * BasicCoefficient;

			public static void RspResultMessage(int gameResult, string ruTranslate, double bet, double winReward)
			{
				switch (gameResult)
				{
					case 0:
						Messages.GameResult("rsp", 2, 0, ruTranslate);
						return;
					case 1:
						BalanceChange(winReward);
						Messages.GameResult("rsp", 1, winReward, ruTranslate);
						return;
					case 2:
						BalanceChange(-bet);
						Messages.GameResult("rsp", 0, bet, ruTranslate);
						return;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			public static void PlayRSP()
			{
				OutputMessages();
				(Rsp computerChoice, string ruTranslate) = GenerateComputerChoice();
				(Rsp userChoice, double bet) = UserRSPConfig();
				int gameResult = (3 + (int)userChoice - (int)computerChoice) % 3;
				RspResultMessage(gameResult, ruTranslate, bet, WinReward(bet));
			}
		}

		public class Highlow
		{
			public static (int yourNumber, int randomNumber) GenerateHighLowNumbers() =>
			(Random.Shared.Next(1, 11), Random.Shared.Next(1, 11));

			public static void OutMessage()
			{
				if (Settings.IsShowRulesEnabled())
				{
					Console.WriteLine(Messages.HighlowStartMessage);
				}
			}

			public static (double bet, string choice, int yourNumber, int randomNumber) UserHighlowConfig()
			{
				(int yourNumber, int randomNumber) = GenerateHighLowNumbers();
				double bet = UserBet();
				Console.WriteLine($"Ваше число: {yourNumber}");
				Console.WriteLine("Больше или меньше?");
				string choice = UserChoice("больше", "меньше");
				return (bet, choice, yourNumber, randomNumber);
			}

			public static double WinReward(double bet, string choice, int yourNumber)
			{
				int outcome = choice switch
				{
					"больше" => 10 - yourNumber,
					"меньше" => yourNumber - 1,
					_ => 0
				};

				if (outcome == 0)
				{
					return 0;
				}
				return Math.Round(bet * (10.0 / outcome * (BasicCoefficient / 2.0)), 2);
			}

			public static byte HighlowResult(string userChoice, int yourNumber, int randomNumber)
			{
				if ((userChoice == "больше" && yourNumber < randomNumber) || (userChoice == "меньше" && yourNumber > randomNumber))
				{
					return 1;
				}
				else
				{
					return 0;
				}
			}

			public static void HighlowResultMessage(byte gameResult, int randomNumber, double bet, double winReward)
			{
				string randomNumberOut = Convert.ToString(randomNumber);
				switch (gameResult)
				{
					case 0:
						BalanceChange(-bet);
						Messages.GameResult("highlow", gameResult, bet, randomNumberOut);
						break;

					case 1:
						BalanceChange(winReward);
						Messages.GameResult("highlow", gameResult, winReward, randomNumberOut);
						break;

					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			public static void PlayHighlow()
			{
				OutMessage();
				(double bet, string userChoice, int yourNumber, int randomNumber) = UserHighlowConfig();
				var winReward = WinReward(bet, userChoice, yourNumber);
				var gameResult = HighlowResult(userChoice, yourNumber, randomNumber);
				HighlowResultMessage(gameResult, randomNumber, bet, winReward);
			}
		}
	}
}


