using System;
using System.IO;
using NetAutoGUI;

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



                try
                {
                    ClickImage(1);
                    ClickImage(2);
                    // GUI.Keyboard.Press(GUI.Keyboard.Keys.Control, GUI.Keyboard.Keys.C);
                    GUI.Keyboard.HotKey(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V);
                        GUI.Keyboard.Press(VirtualKeyCode.RETURN);
                         
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

                Console.WriteLine(file);
            }
        }
        else
        {
            Console.WriteLine("Directory does not exist.");
        }
    }

    private static void ClickImage(int i)
    {
        var rect = GUI.Screenshot.LocateAllOnScreen($"test{i}.png").First();
        GUI.Mouse.Click(rect.X, rect.Y);
        Thread.Sleep(2000);
    }
}