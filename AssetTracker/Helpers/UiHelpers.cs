namespace AssetTracker.Helpers;
public class UiHelpers
{
    public static void PrintErrorMessage(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(msg);
        Console.ResetColor();
    }

    public static void PrintSuccessMessage(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(msg);
        Console.ResetColor();
    }

    public static void PauseDisplay()
    {
        Console.WriteLine("Press any key to continue...");
        _ = Console.ReadKey();
    }

    public static int GetValidInt(string prompt, int min = int.MinValue, int max = int.MaxValue)
    {
        while (true)
        {
            try
            {
                Console.Write(prompt);
                var choiceStr = Console.ReadLine();
                if (!int.TryParse(choiceStr, out int choice) || choice < min || choice > max)
                    throw new Exception("Invalid Choice");
                return choice;
            }
            catch (Exception ex) { PrintErrorMessage(ex.Message); }
        }
    }
}
