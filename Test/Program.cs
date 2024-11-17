using System;
using System.IO;

class Program
{
    static async Task Main(string[] args)
    {
        System.Diagnostics.Process.Start(@"C:\Users\admin\AppData\Local\JianyingPro\Apps\JianyingPro.exe");
        await Task.Delay(5000);
        string directoryPath = @"D:\Downloads\尚硅谷";

        if (Directory.Exists(directoryPath))
        {
            string[] files = Directory.GetFiles(directoryPath);

            foreach (string file in files)
            {
                Console.WriteLine(file);
            }
        }
        else
        {
            Console.WriteLine("Directory does not exist.");
        }
    }
}