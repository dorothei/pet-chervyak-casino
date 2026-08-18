namespace testcasino 
	{
	class Program 
		{
        const float Version = 1.5f;
		public static double balance = 100;
		public static byte streak = 0;
		public const string BetQuestion = "Ваша ставка?";		
        public const string HeadsOrTailsQuestion = "Крупье:\nОрел или Решка?";
        public const string ErrorMessage = "Выбрано некорректное действие. Пейте поменьше пива и попробуйте еще раз.";
		static void Main()
			{
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
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
            Console.WriteLine($"Version: {Version}");
			Console.ReadKey();
			Console.WriteLine("Дворецкий:\nДобро пожаловать в казино 'Червячок!'");
			Console.WriteLine("В нашем казино - выигрывают!");
			Console.WriteLine($"Ваш баланс на сегодня: {balance}");
			Console.WriteLine("------------------------------------");
			
			while (true) 
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
					};
				
				if (streak > 2) 
					{
					Console.WriteLine($"Вы в ударе! Выигрыш увеличен на 15. | Выиграно подряд: {streak}");	
					}
					
				Console.WriteLine("1 - Сыграть в Орла и Решку");
				Console.WriteLine("2 - Сыграть в Блекджек");
				Console.WriteLine("3 - Сыграть в Больше/Меньше");
				Console.WriteLine("0 - Выйти из казино");
                if (byte.TryParse(Console.ReadLine(), out byte act))
                {
                    switch (act)
                    {
                    case 1: 
						Casino.Coinflip();
						break;
					case 2: 
						Casino.Blackjack();
						break;
					case 3:
						Casino.HighLow();
						break;
					case 0:
						Console.WriteLine("Женщина на ресепшене:\nДо скорых встреч!");
						return;
					default:
						Console.WriteLine(ErrorMessage);
						break; 
                    } 
                } else
                {
                    Console.WriteLine(ErrorMessage);
                };
				}
			}
		}


    class Messages
    {
        public static string BetOverrun() => $"Крупье:\nУказанная сумма больше чем ваш баланс. В кредит залезть нельзя. Попробуйте еще раз | {Program.balance}"; 

        public static string YourBalance() => $"Ваш баланс: {Program.balance}";

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

            Console.WriteLine($"Крупье:\n{status}\n{YourBalance()} | {change}");       
        }
    }

	class Casino 
		{	
		public static void Coinflip() 
			{
			Console.WriteLine(Program.HeadsOrTailsQuestion);
			string ? input = Console.ReadLine()?.Trim().ToLower();
			while ((input != "орел") && (input != "решка")) 
				{
				Console.WriteLine(Program.HeadsOrTailsQuestion);
				input = Console.ReadLine()?.Trim().ToLower();
				}
			Console.WriteLine(Program.BetQuestion);
			double bet = Convert.ToDouble(Console.ReadLine());
			while (bet > Program.balance) 
				{
				Console.WriteLine(Messages.BetOverrun());
				bet = Convert.ToInt32(Console.ReadLine());
				}
				
			int seed = Random.Shared.Next(2);
			double win_dif = Math.Round(bet + bet * 0.3, 2);
				
			string textOutcome = (seed == 1) ? "решка" : "орел";	
			if (input == textOutcome) 
				{
				if (Program.streak > 2) 
					{
					Program.balance += win_dif + 15;
					} else 
						{
						Program.balance += win_dif;
						};
				Program.streak += 1;
				Messages.GameResult("coinflip", 1, win_dif);
				} else 
					{
					Program.streak = 0;
					Program.balance -= bet;
					Messages.GameResult("coinflip", 0, bet, textOutcome);
					}
			}
			
		public static void Blackjack() 
			{
			throw new NotImplementedException("Пока нереализовано");
			}
		
		public static void HighLow()
			{
			int your_number = Random.Shared.Next(1, 11);
			int random_number = Random.Shared.Next(1, 11);
			Console.WriteLine("Крупье:\nВаша задача угадать, будет следующее число больше или меньше вашего текущего.");
			Console.WriteLine("Если выпадает число, равное вашему, вы проигрываете.");
			Console.WriteLine(Program.BetQuestion);
			double bet = Convert.ToDouble(Console.ReadLine());
			
			while (bet > Program.balance) 
				{
				Console.WriteLine(Messages.BetOverrun());
				bet = Convert.ToDouble(Console.ReadLine());
				}
				
			Console.WriteLine($"Ваше число: {your_number}");
			Console.WriteLine("Больше или меньше?");
			double win_dif = 0;
			string ? choice = Console.ReadLine()?.Trim().ToLower();
			switch (choice) 
			{
				case "больше":
					win_dif = bet * (10 / (11 - your_number) * 0.95);
					if (your_number < random_number) 
						{
						if (Program.streak > 2) 
							{
							Program.balance += win_dif + 15;	
							} else 
								{
								Program.balance += win_dif;
								}
						Program.streak += 1;
						Messages.GameResult("highlow", 1, win_dif, Convert.ToString(random_number));
						} else 
							{
							Program.streak = 0;
							Program.balance -= bet;
							Messages.GameResult("highlow", 0, bet, Convert.ToString(random_number));
							}
					break;
						
				case "меньше":
					if (your_number > random_number) 
					{
					win_dif = Math.Round(bet * (10 / (your_number - 1 + 0.1) * 0.95),2);
					if (Program.streak > 2) 
						{
						Program.balance += win_dif + 15;
						} else 
							{
							Program.balance += win_dif;
							};
					Program.streak += 1;
                    Messages.GameResult("highlow", 1, win_dif, Convert.ToString(random_number));
					} else 
						{
						Program.streak = 0;
						Program.balance -= bet;
                        Messages.GameResult("highlow", 0, bet, Convert.ToString(random_number));
						}
					break;
					
				default:
					Console.WriteLine(Program.ErrorMessage);
					break;
			}
			}
		}
	}

