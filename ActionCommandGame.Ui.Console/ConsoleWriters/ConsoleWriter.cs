using ActionCommandGame.Services.Model.Core;

namespace ActionCommandGame.Ui.ConsoleApp.ConsoleWriters
{
    public static class ConsoleWriter
    {
        public static void WriteTitle(string text, ConsoleColor? color = null)
        {
            WriteText(text, color);
            WriteText(string.Empty.PadRight(text.Length, '-'), color);
        }

        public static Task WriteText(string text, ConsoleColor? color = null, bool writeNewLine = true)
        {
            if (color != null)
            {
                Console.ForegroundColor = color.Value;
            }

            if (writeNewLine)
            {
                Console.WriteLine(text);
            }
            else
            {
                Console.Write(text);
            }
            Console.ResetColor();
            return Task.CompletedTask;
        }

        public static Task WriteLine()
        {
            Console.WriteLine();
            Console.ResetColor();
            return Task.CompletedTask;
        }

        public static Task Clear()
        {
            Console.Clear();
            Console.ResetColor();
            return Task.CompletedTask;
        }

        public static Task WriteMessages(IList<ServiceMessage> messages)
        {
            if (messages.Any(m => m.MessagePriority == MessagePriority.Info))
            {
                foreach (var message in messages)
                {
                    if (message.Code.ToLower() == "levelup")
                    {
                        ConsoleBlockWriter.Write(message.Message, 2);
                    }
                    else
                    {
                        WriteText(message.Message, ConsoleColor.Cyan);
                    }
                }
            }

            if (messages.Any(m => m.MessagePriority == MessagePriority.Warning))
            {
                foreach (var message in messages)
                {
                    WriteText(message.Message, ConsoleColor.DarkGreen);
                }
            }

            if (messages.Any(m => m.MessagePriority == MessagePriority.Error))
            {
                foreach (var message in messages)
                {
                    WriteText(message.Message, ConsoleColor.Red);
                }
            }

            return Task.CompletedTask;
        }

    }
}
