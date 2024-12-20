﻿// See https://aka.ms/new-console-template for more information
using Microsoft.VisualBasic.ApplicationServices;
using NetAutoGUI;
using NetAutoGUI.Windows;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Vanara.PInvoke;

using static Vanara.PInvoke.User32;

public class Program
{
    [DllImport("user32.dll")]
    static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    [DllImport("user32.dll")]
    static extern bool IsWindowVisible(IntPtr hWnd);
    private static bool isFile = false;
    const int WM_HOTKEY = 0x0312;
    static string filePath = "1.txt";
    static void Main(string[] args) {
        
        HotkeyHook.fileName = File.ReadAllText(filePath);
        var ct = new CancellationTokenSource().Token;
        //*入库动态选择商品 *Code*
        Task.Run(() => {
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = System.AppDomain.CurrentDomain.BaseDirectory;
                watcher.Filter = Path.GetFileName(filePath);

                // 监听更改事件
                watcher.Changed += OnChanged;

                // 启用事件触发
                watcher.EnableRaisingEvents = true;

                // 输出当前文件内容
                Console.WriteLine("Initial file content:");
              

                Console.WriteLine("Press 'q' to quit.");
                ct.WaitHandle.WaitOne();
            }
        });
      
        HotkeyHook.Init(null, HandleHotkey);
    }

    private static void OnChanged(object sender, FileSystemEventArgs e)
    {
        HotkeyHook.fileName = File.ReadAllText(filePath).Trim();   //*入库动态选择商品 *Code*
    }

    static void HandleHotkey(int id)
    {
        switch (id)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                GUI.Keyboard.HotKey(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);
                Task.Delay(200).Wait();
                GUI.Keyboard.HotKey(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);
                Task.Delay(500).Wait();
                var content = HotkeyHook.GetClipboardData();
                if (content != null && content.Trim().StartsWith("http"))
                {
                    content = HotkeyHook.GetUrlMarkDown(content).Result;
                }
                else
                {
                    switch (id)
                    {
                        case 0:
                            break;
                        case 1:
                            content = $"# {content}";
                            break;

                        case 2:
                            content = $"## {content}";
                            break;

                        case 3:
                            content = $"### {content}";
                            break;

                        case 4:
                            var contentBuilder = new StringBuilder();
                            contentBuilder.AppendLine("``` c#");
                            contentBuilder.AppendLine(content);
                            contentBuilder.AppendLine("```");
                            content = contentBuilder.ToString();
                            break;
                    }
                }
                if (isFile)
                {
                    if (File.Exists(HotkeyHook.fileName))
                    {
                        File.AppendAllText(HotkeyHook.fileName, content);
                    }
                    break;
                }
                else
                {
                    try
                    {
                        PastContent(content);
                    }
                    catch (Exception ex)
                    {
 
                    }
                   
                    break;
                }



            default:

                // GUI.Keyboard.HotKey(VirtualKeyCode.MENU, VirtualKeyCode.TAB);
                break;
        }
      
        //Console.WriteLine("Hello, World!");
    }

    private static void PastContent(string? content)
    {
        HotkeyHook.SetClipboardData(content);
        var aimWindow = GUI.Application.ActivateWindowByTitle(HotkeyHook.fileName);

        Task.Delay(500).Wait();
        //new IntPtr(value)
        GUI.Keyboard.HotKey(VirtualKeyCode.CONTROL, VirtualKeyCode.END);

        GUI.Keyboard.Press(VirtualKeyCode.RETURN);
        Task.Delay(200).Wait();
        GUI.Keyboard.HotKey(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V);
        Task.Delay(200).Wait();
        GUI.Keyboard.HotKey(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_S);
        using (GUI.Keyboard.Hold(VirtualKeyCode.MENU))
        {
            GUI.Keyboard.Press(VirtualKeyCode.TAB);
            GUI.Keyboard.Press(VirtualKeyCode.TAB);
        }
    }

    //var winChrome = NetAutoGUI.GUI.Application.WaitForWindowLikeTitle("*Token*", timeoutSeconds: 5);

    //var uiE = new UIElement(winChrome.Id);

    //var window = GUI.Application.GetAllWindows(hwnd);
    //uiE.Focus();
    //GUI.Application.WaitForWindowByTitle("*Token*");

    //Console.ReadKey();

    //GUI.Keyboard.Write("你好，我是杨中科");
    //GUI.Keyboard.Press(VirtualKeyCode.RETURN);
    int n = 0;
    //while (n < 10000)
    //{
    // Thread.Sleep(2000);
    // GUI.Keyboard.Press(new[] { VirtualKeyCode.VK_1 });
    // Console.WriteLine(n);
    // n++;
    //}
    //GUI.Application.LaunchApplication("notepad.exe");

    //Console.ReadKey();\
}