# P90s Logger

A simple, modular and lightweight C# logger.

## Usage

This tool contains two ready-to-go implementations, which are thread safe by default. `ConsoleLogger` prints the logs directly to the console. `FileLogger` prints the logs into the specified file when program exits or when the number of logs exceeds the specified buffer limit.

```c#
using P90Ez.Logger;

ILogger Logger = new ConsoleLogger(); //new FileLogger("Logs.txt");

Logger.Log("This is just a normal message.", ILogger.Severity.Message);
Logger.Log("Just a warning.", ILogger.Severity.Warning);
Logger.Log("An error occurred!", ILogger.Severity.Critical);
```

### Example

```c#
using P90Ez.Logger;

namespace Tests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ILogger Logger = new ConsoleLogger(); //new FileLogger("Logs.txt");

            Logger.Log("This is just a normal message.", ILogger.Severity.Message);

            Run(Logger);
        }

        static void Run(ILogger Logger)
        {
            Thread.Sleep(3000);

            Logger.Log("Just a warning.", ILogger.Severity.Warning);

            Thread.Sleep(5000);
            ErrorTest.Foo.Bar(Logger);
        }
    }
}
namespace Tests.ErrorTest
{
    public class Foo
    {
        public static void Bar(ILogger Logger)
        {
            Logger.Log("An error occurred!", ILogger.Severity.Critical);
        }
    }
}
```

Output:

```
[16:38:31][Message] Tests.Program.Main:11 - This is just a normal message.
[16:38:34][Warning] Tests.Program.Run:21 - Just a warning.
[16:38:39][Critical] An error occurred!
        Tests.ErrorTest.Foo.Bar:35
        Tests.Program.Run:25
        Tests.Program.Main:13
```

## Customization

### Custom Format

To create a custom format, while keeping the logic of the existing loggers (`ConsoleLogger` and `FileLogger`), it is possible to inherit from either of those loggers. To change the format, overload the function `FormatMessage` as shown below.

```c#
public class CustomFormatLogger : ConsoleLogger //FileLogger
{
    public override string FormatMessage(string Message, ILogger.Severity Severity)
    {
        //implement body for custom formatting
    }
}
```

For a concrete example, have a look at `BaseLogger`.\
The function `BuildFunctionInformation` can be used for easier stacktrace formatting. To change the formatting of the stacktrace, it is possible to overload this function as well.

### Custom Logic

To create a logger, which does something else than outputting to the console or into a file, it is possible to inherit from `BaseLogger`. This class already implements `FormatMessage` and `BuildFunctionInformation`. To implement your custom logic, implement the function `Log` as shown below.

```c#
public class CustomLogicLogger : BaseLogger
{
    public override void Log(string Message, ILogger.Severity Severity)
    {
        //implement body for custom logic
    }
}
```

For a concrete example, have a look at either `ConsoleLogger` (simple) or `FileLogger` (more in-depth).

### From Scratch

Of course, it as also possible to implement every aspect yourself and just use the provided interface. Similar to _Custom Logic_, implement the function `Log` as shown below.

```c#
public class FromScratchLogger : ILogger
{
    public void Log(string Message, ILogger.Severity Severity)
    {
        //implement logging
    }
}
```

## Credits

Created and maintained by P90Ez aka. Manu. Feel free to use this Logger in your software, as it is provided with the MIT license.
