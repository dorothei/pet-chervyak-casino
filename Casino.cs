namespace testcasino 
	{
		class Program 
			{
				public const float Version = 2.0f;
				public static byte streak = 0;	
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

				static void EncodingFix()
				{
					Console.OutputEncoding = System.Text.Encoding.UTF8;
            		Console.InputEncoding = System.Text.Encoding.UTF8;
				}
			}


		class Messages
			{
				public static readonly string HeadsOrTailsQuestion = "Крупье:\nОрел или Решка?";
				public static readonly string BetQuestion = "Ваша ставка?";	
				public static readonly string ErrorMessage = "Выбрано некорректное действие. Пейте поменьше пива и попробуйте еще раз.";
				public static string BetOverrun() => 
				$"Крупье:\nУказанная сумма больше чем ваш баланс. В кредит залезть нельзя. Попробуйте еще раз | {Casino.ShowBalance()}"; 

				public static readonly string ByeMessage = "Женщина на ресепшене:\nДо скорых встреч!";
				public static string YourBalanceMessage() => 
				$"Ваш баланс: {Casino.ShowBalance()}";

				public static void Menu()
				{
					Console.WriteLine("1 - Сыграть в Орла и Решку");
					Console.WriteLine("2 - Сыграть в Блекджек");
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
								Casino.Blackjack.Pass();
								break;
							case 3: 
								Casino.Highlow.PlayHighlow();
								break;
							case 0:
								Console.WriteLine(ByeMessage);
								return;
							default:
								Console.WriteLine(ErrorMessage);
								break;
						};
					}
				}

				public static void GameResult(string game, byte result, double prize, string details = "")
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
						return;
					}

					if (balance > 10000)
					{
						Console.WriteLine("ХОРОШАЯ КОНЦОВКА:");
						Console.WriteLine("Вы обыграли казино! Не приходите сюда больше.");
						Console.WriteLine("Вы ничего не рассказали жене про свой выигрыш.");
						Console.ReadKey();
						return;
					}
				}
   			}

		class Casino 
			{	
				// Стартовый баланс
				public static double balance = 100;

				public static double ShowBalance() =>
				balance;

				public static void BalanceChange(double difference) =>
				balance += difference;

				public class Coinflip
					{
						public static (string choice, double bet) UserCoinflipConfig()
						{
							// Спрашиваем у пользователя исход раунда
							string ? choice = "";
							while ((choice != "орел") && (choice != "решка"))
							{
								Console.WriteLine(Messages.HeadsOrTailsQuestion);
								choice = Console.ReadLine()?.Trim().ToLower();
							}

							// Спрашиваем у пользователя ставку
							Console.WriteLine(Messages.BetQuestion);
							double.TryParse(Console.ReadLine(), out double bet);
							while (bet > ShowBalance() || bet <= 0)
							{
								Console.WriteLine(Messages.BetOverrun());
								double.TryParse(Console.ReadLine(), out bet);
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

				public class Blackjack
					{
						public static void Pass() 
						{
							throw new NotImplementedException("Пока нереализовано");
						}
					}

				public class Highlow
					{
						public static (int yourNumber, int randomNumber) GenerateHighLowNumbers() =>
						(Random.Shared.Next(1, 11), Random.Shared.Next(1, 11));

						public static (double bet, string choice, int yourNumber, int randomNumber) UserHighlowConfig()
						{	
							var random = GenerateHighLowNumbers();
							var choice = "";
							Console.WriteLine("Крупье:\nВаша задача угадать, будет следующее число больше или меньше вашего текущего.");
							Console.WriteLine("Если выпадает число, равное вашему, вы проигрываете.");
							Console.WriteLine(Messages.BetQuestion);
							Console.WriteLine(Messages.BetQuestion);
							double.TryParse(Console.ReadLine(), out double bet);
							while (bet > ShowBalance() || bet <= 0)
							{
								Console.WriteLine(Messages.BetOverrun());
								double.TryParse(Console.ReadLine(), out bet);
							}

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
	

