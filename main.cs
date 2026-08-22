using System.Reflection;
namespace Casino
{
    class Program
    {
        public static readonly string Version = Assembly.GetExecutingAssembly()
                                                        .GetName()
                                                        .Version?
                                                        .ToString(2) ?? throw new Exception();

        static void EncodingFix()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
        }

        static void Main()
        {
            EncodingFix();
            Messages.ShowHeader();

            while (true)
            {   
                Console.WriteLine("Нажмите ENTER, чтобы продолжить.");
                Console.ReadKey();
                Console.Clear();
                Messages.CheckEndOfGame(Games.ShowBalance());
                Messages.Menu();
            }
        }
    }
}
