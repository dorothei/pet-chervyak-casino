namespace ChervyakCasino
	{
		class Program 
			{
				public const float Version = 2.2f;
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


		class Messages
			{
				public const string HeadsOrTailsQuestion = "Крупье:\nОрел или Решка?";
				public const string BetQuestion = "Ваша ставка?";	
				public const string RspChoiceQuestion = "Мысли:\nКамень, ножницы, бумага?";
				public static void ErrorMessage() =>
				Console.WriteLine("Выбрано некорректное действие. Попробуйте еще раз.");
				public const string ByeMessage = "Женщина на ресепшене:\nДо скорых встреч!";
				public const string HighlowStartMessage = "Крупье:\nВаша задача угадать, будет следующее число больше или меньше вашего текущего.\nЕсли выпадает число, равное вашему, вы проигрываете.";
				public const string RspStartMessage = "Крупье:\nДобро пожаловать за стол для игры в Камень-Ножницы-Бумага!\nПравила просты:\nК бьет Н\nН бьет Б\nБ бьет К.";
				public const string RspGameMessage = "Камень, ножницы, бумага! Цу-е-фа";
				public const string RspDrawMessage = "Что-ж. Ничья.";
				public static string BetOverrun() => 
				$"Крупье:\nУказанная сумма больше чем ваш баланс или равна нулю. Казино кредитов, к сожалению, не даёт. Попробуйте еще раз | {Casino.ShowBalance()}"; 
				public static string YourBalanceMessage() => 
				$"Ваш баланс: {Math.Round(Casino.ShowBalance(), 2)}";

				public static void Menu()
				{
					Console.WriteLine("1 - Сыграть в Орла и Решку");
					Console.WriteLine("2 - Сыграть в Камень-Ножницы-Бумага [NEW]");
					Console.WriteLine("3 - Сыграть в Больше/Меньше");
					Console.WriteLine("0 - Выйти из казино");
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
							case 0:
								Console.WriteLine(ByeMessage);
								Environment.Exit(0);
								break;

							default:
								ErrorMessage();
								break;
						};
					}
				}

				public static void GameResult(string game, byte result, double prize, object ? details = null)
				{   
					string status = "", change = "";
					switch (result)
					{   
						case 1:
							change =  $"+ {prize}";
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
							};
							break;
						case 2:
							switch (game)
							{
								case "rsp":
									status = RspDrawMessage;
									break;
							}; 
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
							};
							break;
					}
            		Console.WriteLine($"Крупье:\n{status}\n{YourBalanceMessage()} | {change}");       
        		}

				public static void ShowHeader()
				{
					Console.WriteLine("###################################################################");
					Console.WriteLine(" ███  █   █ █████ ████  █   █ █   █  ███   ███  █   █  ███  █   █ ");
					Console.WriteLine("█     █   █ █     █   █ █   █  █ █  █   █ █     █   █ █   █ █  █");  
					Console.WriteLine("█     █████ ████  ████  █   █   █   █████ █     █████ █   █ ███");   
					Console.WriteLine("█     █   █ █     █  █   █ █    █   █   █ █     █   █ █   █ █  █");
					Console.WriteLine(" ███  █   █ █████ █   █   █     █   █   █  ███  █   █  ███  █   █ ");
					Console.WriteLine("");
					Console.WriteLine("     ███   ███   ████ ███ █   █  ███  ");
					Console.WriteLine("   █     █   █ █      █  ██  █ █   █  ");
					Console.WriteLine("  █     █████  ███   █  █ █ █ █   █   ");
					Console.WriteLine(" █     █   █     █  █  █  ██ █   █    ");
					Console.WriteLine(" ███  █   █ ████  ███ █   █  ███      ");
					Console.WriteLine("###################################################################");
					Console.WriteLine("Author: 68 79 82 79 84 79 82 79");
					Console.WriteLine($"Version: {Program.Version}");
					Console.WriteLine("Дворецкий:\nДобро пожаловать в казино 'Червячок!'");
					Console.WriteLine("В нашем казино - выигрывают!");
					Console.WriteLine($"Ваш баланс на сегодня: {Casino.ShowBalance()}");
					Console.WriteLine("------------------------------------");
				}

				public static void CheckEndOfGame(double balance)
				{
					if (balance <= 0) 
					{
						Console.WriteLine("ПЛОХАЯ КОНЦОВКА:");
						Console.WriteLine("Вы проиграли все свои деньги в казино! Вас накажет жена.");
						Console.WriteLine("По возвращению домой из казино ваша жена выгнала Вас из дома.");
						Console.ReadKey();
						Environment.Exit(0);
					}

					if (balance > 1000)
					{
						Console.WriteLine("ХОРОШАЯ КОНЦОВКА:");
						Console.WriteLine("Вы обыграли казино! Не приходите сюда больше.");
						Console.WriteLine("Вы ничего не рассказали жене про свой выигрыш.");
						Console.ReadKey();
						Environment.Exit(0);
					}
				}
   			}

		public class Casino 
			{	
				// Стартовый баланс
				public static double balance = 100;

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

				public class Coinflip
					{
						public static (string choice, double bet) UserCoinflipConfig()
						{
							var bet = UserBet();
							string ? choice = "";
							while ((choice != "орел") && (choice != "решка"))
							{
								Console.WriteLine(Messages.HeadsOrTailsQuestion);
								choice = Console.ReadLine()?.Trim().ToLower();
							}
							return (choice, bet);
						} 

						// Генерируем исход игры 
						public static int GenerateCoinflipResult() =>
						Random.Shared.Next(0, 2);

						public static void PlayCoinflipRound()
						{
							var userConfig = UserCoinflipConfig();
							double winReward = Math.Round(userConfig.bet + userConfig.bet * 0.3, 2);
							string textOutcome = (GenerateCoinflipResult() == 1) ? "решка" : "орел";
							if (userConfig.choice == textOutcome) 
							{
								BalanceChange(+winReward);
								Messages.GameResult("coinflip", 1, winReward);
							} else
								{
									BalanceChange(-userConfig.bet);
									Messages.GameResult("coinflip", 0, userConfig.bet, textOutcome);
								}
						}
					}

				public class RockScissorsPaper
					{
						private static bool alreadyPlayed = false;
						public enum Rsp
						{
							Rock = 0,
							Paper = 1,
							Scissors = 2
						}
						public static (Rsp randomRsp, string ruTranslate) GenerateComputerChoice()
						{
							Rsp randomRsp = (Rsp)Random.Shared.Next(0,3);
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
							if (!alreadyPlayed)
							{
								Console.WriteLine(Messages.RspStartMessage);
							} 
							Console.WriteLine(Messages.RspGameMessage);
						}

						public static (Rsp userChoice, double bet) UserRSPConfig()
						{
							var bet = UserBet();
							Console.WriteLine(Messages.RspChoiceQuestion);
							string? userInput = Console.ReadLine()?.Trim().ToLower();
							while ((userInput != "камень") && (userInput != "ножницы") && (userInput != "бумага"))
							{
								Console.WriteLine(Messages.RspChoiceQuestion);
								userInput = Console.ReadLine()?.Trim().ToLower();
							}
							Rsp choice = userInput switch
							{
								"камень" => Rsp.Rock,
								"бумага" => Rsp.Paper,
								"ножницы" => Rsp.Scissors,
								_ => 0
							};
							return (choice, bet);
						}

						public static double WinReward(double bet) =>
						bet * 1.95;

						public static void PlayRSP()
						{
							OutputMessages();
							alreadyPlayed = true;
							var computerChoice = GenerateComputerChoice();
							var input = UserRSPConfig();
							double winReward = WinReward(input.bet);
							int gameResult = (3 + (int)input.userChoice - (int)computerChoice.randomRsp) % 3;
							switch (gameResult)
							{
								case 0:
									Messages.GameResult("rsp", 2, 0, computerChoice.ruTranslate);
									return;
								case 1:
									BalanceChange(winReward); 
									Messages.GameResult("rsp", 1, winReward, computerChoice.ruTranslate);
									return;
								case 2:
									BalanceChange(-input.bet);
									Messages.GameResult("rsp", 0, input.bet, computerChoice.ruTranslate);
									return;
								default:
									return;
							}
						}
					}

				public class Highlow
					{
						public static bool alreadyPlayed = false;
						public static (int yourNumber, int randomNumber) GenerateHighLowNumbers() =>
						(Random.Shared.Next(1, 11), Random.Shared.Next(1, 11));

						public static void OutMessage()
						{
							if (!alreadyPlayed)
							{
								Console.WriteLine(Messages.HighlowStartMessage);
							} 
						}

						public static (double bet, string choice, int yourNumber, int randomNumber) UserHighlowConfig()
						{	
							var random = GenerateHighLowNumbers();
							string ? choice = "";
							double bet = UserBet();
							Console.WriteLine($"Ваше число: {random.yourNumber}");
							Console.WriteLine("Больше или меньше?");
							while ((choice != "больше") && (choice != "меньше"))
							{
								choice = Console.ReadLine()?.Trim().ToLower();
							}
							return (bet, choice, random.yourNumber, random.randomNumber);
						}

						public static double WinReward(double bet, string choice, int yourNumber)
						{
							double winReward = choice switch
							{
								"больше" => bet * (10 / (11 - yourNumber) * 0.95),
								"меньше" => Math.Round(bet * (10 / (yourNumber - 1 + 0.1) * 0.95),2),
								_ => 0
							};
							return winReward;	
						}

						public static void PlayHighlow()
						{
							OutMessage();
							alreadyPlayed = true;
							var userConfig = UserHighlowConfig();
							var winReward = WinReward(userConfig.bet, userConfig.choice, userConfig.yourNumber);
							if ((userConfig.choice == "больше" && userConfig.yourNumber < userConfig.randomNumber) || (userConfig.choice == "меньше" && userConfig.yourNumber > userConfig.randomNumber))
							{
								BalanceChange(+winReward);
								Messages.GameResult("highlow", 1, winReward, Convert.ToString(userConfig.randomNumber));
							} else
								{
									Messages.GameResult("highlow", 0, userConfig.bet, Convert.ToString(userConfig.randomNumber));
									BalanceChange(-userConfig.bet);
								}
						}
					}	
			}
		}
	

