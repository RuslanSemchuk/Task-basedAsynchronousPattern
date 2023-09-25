using System;
using System.Diagnostics;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        int numThreads = 1; 

        if (args.Length == 1 && int.TryParse(args[0], out int parsedThreads) && parsedThreads > 0)
        {
            numThreads = parsedThreads; 
        }
        else
        {
            Console.WriteLine("Enter the number of threads:");
            while (!int.TryParse(Console.ReadLine(), out numThreads) || numThreads <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a positive integer:");
            }
        }

        try
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Task<int> task = CalculateAsync(10, numThreads);

            Console.WriteLine("Task started");

            int result = await task;

            stopwatch.Stop();

            Console.WriteLine("Result: " + result);
            Console.WriteLine("Elapsed Time: " + stopwatch.ElapsedMilliseconds + "ms");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    static async Task<int> CalculateAsync(int num, int numThreads)
    {
        if (numThreads < 1)
            throw new ArgumentOutOfRangeException(nameof(numThreads), "Number of threads must be at least 1.");

        
        var tasks = new Task<int>[numThreads];
        for (int i = 0; i < numThreads; i++)
        {
            tasks[i] = CalculateSquareAsync(num);
        }

      
        Task<int> firstCompletedTask = await Task.WhenAny(tasks);
        return await firstCompletedTask;
    }

    static async Task<int> CalculateSquareAsync(int num)
    {
        await Task.Delay(1000); 
        return num * num;
    }
}
