namespace ChervyakCasino
{
	class Program
	{
		public const float Version = 2.48f;
		private static void EncodingFix()
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;
			Console.InputEncoding = System.Text.Encoding.UTF8;
		}
		static void Main()
		{
			EncodingFix();
			Messages.ShowHeader();
			Console.ReadKey();

			while (true)
			{
				Messages.CheckEndOfGame(Casino.ShowBalance());
				Messages.Menu();
			}
		}
	}

	class Settings
	{
		static bool showRules = true;

		public static bool IsShowRulesEnabled() =>
		showRules;

		public static void ToggleRules() =>
		showRules = !showRules;
	}

	class Messages
	{
		public const string HeadsOrTailsQuestion = """
					Крупье:
					Орел или Решка?
					""";
		public const string RspChoiceQuestion = """
					Мысли:
					Камень, ножницы, бумага?
					""";
		public const string ByeMessage = """
					Женщина на ресепшене:
					До скорых встреч!
					""";
		public const string HighlowStartMessage = """
					Крупье:
					Ваша задача угадать, будет следующее число больше или меньше вашего текущего.
					Если выпадает число, равное вашему, вы проигрываете.
					""";
		public const string RspStartMessage = """
					Крупье:
					Добро пожаловать за стол для игры в Камень-Ножницы-Бумага!
					Правила просты:
					К бьет Н
					Н бьет Б
					Б бьет К.
					""";
		public const string RspGameMessage = "Камень, ножницы, бумага! Цу-е-фа";
		public const string RspDrawMessage = "Что-ж. Ничья.";
		public const string BetQuestion = "Ваша ставка?";
		public static string BetOverrun() =>
			$"""
					Крупье:
					Указанная сумма больше чем ваш баланс или равна нулю. Казино кредитов, к сожалению, не даёт. 
					Попробуйте еще раз | {Casino.ShowBalance()}
					""";

		public static void ErrorMessage() =>
		Console.WriteLine("Выбрано некорректное действие. Попробуйте еще раз.");

		public static string YourBalanceMessage() =>
		$"Ваш баланс: {Math.Round(Casino.ShowBalance(), 2)}";

		public static void Menu()
		{
			Console.WriteLine($"""
						1 - Сыграть в Орла и Решку
						2 - Сыграть в Камень-Ножницы-Бумага [NEW]
						3 - Сыграть в Больше/Меньше
						4 - Показывать правила игр - {Settings.IsShowRulesEnabled()}
						0 - Выйти из казино
						""");

			if (byte.TryParse(Console.ReadLine(), out byte act))
			{
				switch (act)
				{
					case 1:
						Casino.Coinflip.PlayCoinflipRound();
						break;
					case 2:
						Casino.RockScissorsPaper.PlayRSP();
						break;
					case 3:
						Casino.Highlow.PlayHighlow();
						break;
					case 4:
						Settings.ToggleRules();
						break;
					case 0:
						Console.WriteLine(ByeMessage);
						Environment.Exit(0);
						break;

					default:
						ErrorMessage();
						break;
				}
				;
			}
		}

		public static void GameResult(string game, byte result, double prize, object? details = null)
		{
			string status = "", change = "";
			switch (result)
			{
				case 1:
					change = $"+ {prize}";
					switch (game)
					{
						case "coinflip":
							status = "Вы угадали!";
							break;

						case "highlow":
							status = $"Вы выиграли! Загаданное число: {details}";
							break;

						case "rsp":
							status = $"Вы выиграли! Я загадал: {details}";
							break;
					}
					;
					break;
				case 2:
					switch (game)
					{
						case "rsp":
							status = RspDrawMessage;
							break;
					}
					;
					break;
				case 0:
					change = $"- {prize}";
					switch (game)
					{
						case "coinflip":
							status = $"Вы не угадали. Выпало: {details}";
							break;

						case "highlow":
							status = $"Вы проиграли. Загаданное число: {details}";
							break;

						case "rsp":
							status = $"Вы проиграли. Загадано: {details}";
							break;
					}
					;
					break;
			}
			Console.WriteLine($"Крупье:\n{status}\n{YourBalanceMessage()} | {change}");
		}

		public static void ShowHeader()
		{
			Console.WriteLine($"""
						###################################################################
						███  █   █ █████ ████  █   █ █   █  ███   ███  █   █  ███  █   █ 
						█    █   █ █     █   █ █   █  █ █  █   █ █     █   █ █   █ █  █  
						█    █████ ████  ████  █   █   █   █████ █     █████ █   █ ███
						█    █   █ █     █  █   █ █    █   █   █ █     █   █ █   █ █  █
						███  █   █ █████ █   █   █     █   █   █  ███  █   █  ███  █   █

								 ███   ███   ████ ███ █   █  ███  
								█     █   █ █      █  ██  █ █   █  
								█     █████  ███   █  █ █ █ █   █   
								█     █   █     █  █  █  ██ █   █    
								 ███  █   █ ████  ███ █   █  ███      
						###################################################################
						Author: 68 79 82 79 84 79 82 79"
						Version: {Program.Version}
						Дворецкий:
						Добро пожаловать в казино "Червячок"!
						В нашем казино - выигрывают!
						Ваш баланс на сегодня: {Casino.ShowBalance()}
						------------------------------------
						Приступить к лудомании... (ENTER)
						""");
		}

		public static void CheckEndOfGame(double balance)
		{
			if (balance <= Casino.BadEndBalance)
			{
				Console.WriteLine("""
							ПЛОХАЯ КОНЦОВКА:
							Вы проиграли все свои деньги в казино! Вас накажет жена.
							По возвращению домой из казино ваша жена выгнала Вас из дома.
							""");
				Console.ReadKey();
				Environment.Exit(0);
			}

			if (balance > Casino.GoodEndBalance)
			{
				Console.WriteLine("""
							ХОРОШАЯ КОНЦОВКА:
							Вы обыграли казино! Не приходите сюда больше.
							Вы ничего не рассказали жене про свой выигрыш.
							""");
				Console.ReadKey();
				Environment.Exit(0);
			}
		}
	}

	public class Casino
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
				Console.WriteLine(Messages.BetOverrun());
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


