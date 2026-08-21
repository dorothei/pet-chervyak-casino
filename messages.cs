namespace Casino
{
    public static class Messages
    {
        /// GAME MESSAGES
        // H.O.T messages
        public const string HeadsOrTailsStartMessage = """
                    Крупье:
                    Добро пожаловать за стол для игры в Орел и Решку!
                    Правила просты:
                    Загадываете сторону монеты, а я её подкидываю.
                    Если вы угадываете выпавшую сторону, выигрываете.
                    """;
        public const string HeadsOrTailsQuestion = """
					Крупье:
					Орел или Решка?
					""";

        // R.S.P messages
        public const string RspStartMessage = """
					Крупье:
					Добро пожаловать за стол для игры в Камень-Ножницы-Бумага!
					Правила просты:
					К бьет Н.
					Н бьет Б.
					Б бьет К.
					""";
        public const string RspGameMessage = "Камень, ножницы, бумага! Цу-е-фа";
        public const string RspDrawMessage = "Что-ж. Ничья.";
        public const string RspChoiceQuestion = """
					Мысли:
					Камень, ножницы, бумага?
					""";

        // highlow messages
        public const string HighlowStartMessage = """
					Крупье:
					Ваша задача угадать, будет следующее число больше или меньше вашего текущего.
					Если выпадает число, равное вашему, вы проигрываете.
					""";

        // bets messages
        public const string BetQuestion = "Ваша ставка?";
        public static void BetOverrun() =>
        Console.WriteLine($"""
					Крупье:
					Указанная сумма больше чем ваш баланс или равна нулю. Казино кредитов, к сожалению, не даёт. 
					Попробуйте еще раз | {Games.ShowBalance()}
					""");

        // other
        private static readonly string debugCheatCode = $"Введен чит-код разработчика | +{Games.ShowBalance()}";
        public static void ErrorMessage() =>
        Console.WriteLine("Выбрано некорректное действие. Попробуйте еще раз.");
        public const string ByeMessage = """
					Женщина на ресепшене:
					До скорых встреч!
					""";
        public static string YourBalanceMessage() =>
        $"Ваш баланс: {Math.Round(Games.ShowBalance(), 2)}";

        public static void Menu()
        {
            Console.WriteLine($"""
						Что будем делать?
						1 - Сыграть в Орла и Решку
						2 - Сыграть в Камень-Ножницы-Бумага [NEW]
						3 - Сыграть в Больше/Меньше
						4 - Показывать правила игр - {Settings.IsShowRulesEnabled()}
						0 - Выйти из казино
						""");

            string? act = Console.ReadLine();
            switch (act)
            {
                case "1":
                    Games.Coinflip.PlayCoinflipRound();
                    break;
                case "2":
                    Games.RockScissorsPaper.PlayRSP();
                    break;
                case "3":
                    Games.Highlow.PlayHighlow();
                    break;
                case "4":
                    Settings.ToggleRules();
                    break;
                case "0":
                    Console.WriteLine(ByeMessage);
                    Environment.Exit(0);
                    break;

                case "HESoYAM":
                    Games.BalanceChange(100);
                    Console.WriteLine(debugCheatCode);
                    break;

                default:
                    ErrorMessage();
                    break;
            }
            ;
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
						Ваш баланс на сегодня: {Games.ShowBalance()}
						------------------------------------
						Приступить к лудоприключению... (Нажмите ENTER)
						""");
            Console.ReadKey();
            Console.Clear();
            Console.ReadKey();
        }

        public static void CheckEndOfGame(double balance)
        {
            if (balance <= Games.BadEndBalance)
            {
                Console.WriteLine("""
							ПЛОХАЯ КОНЦОВКА:
							Вы проиграли все свои деньги в казино! Вас накажет жена.
							По возвращению домой из казино ваша жена выгнала Вас из дома.
							""");
                Console.ReadKey();
                Environment.Exit(0);
            }

            if (balance > Games.GoodEndBalance)
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
}