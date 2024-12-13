using System;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
namespace NetAutoGUI.Windows
{
    public static class HotkeyHook
    {
        public static string fileName;
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int WM_HOTKEY = 0x0312;

        public static void Init(string[] args, Action<int> handlerKey)
        {
            // 注册全局快捷键
            RegisterHotkey(Keys.F4, KeyModifiers.None, 1);
            RegisterHotkey(Keys.D1, KeyModifiers.Alt, 2);
            RegisterHotkey(Keys.D2, KeyModifiers.Alt, 3);
            RegisterHotkey(Keys.D3, KeyModifiers.Alt, 4);
            RegisterHotkey(Keys.D4, KeyModifiers.Alt, 5);
            Console.WriteLine("Press 'Q' to quit.");

            // 消息循环
            while (true)
            {
                // 使用 PeekMessage 模拟消息循环
                if (PeekMessage(out var msg, IntPtr.Zero, 0, 0, 1))
                {
                    if (msg.message == WM_HOTKEY)
                    {
                        int id = msg.wParam.ToInt32();
                        handlerKey(id);
                    }
                    else if (msg.message == 0x0012) // WM_QUIT
                    {
                        break;
                    }

                    // 将消息传递给系统
                    TranslateMessage(ref msg);
                    DispatchMessage(ref msg);
                }

                // 在这里可以执行其他任务，或者睡眠一段时间以减少 CPU 使用率
                  Thread.Sleep(500);
            }

            // 注销全局快捷键
            UnregisterHotkey(1);
            UnregisterHotkey(2);
        }

        private static void RegisterHotkey(Keys key, KeyModifiers modifiers, int id)
        {
            uint modifierCode = (uint)modifiers;
            uint keyCode = (uint)key;
            RegisterHotKey(IntPtr.Zero, id, modifierCode, keyCode);
        }

        private static void UnregisterHotkey(int id)
        {
            UnregisterHotKey(IntPtr.Zero, id);
        }




        [DllImport("user32.dll")]
        private static extern bool PeekMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);

        [DllImport("user32.dll")]
        private static extern bool TranslateMessage([In] ref MSG lpMsg);

        [DllImport("user32.dll")]
        private static extern IntPtr DispatchMessage([In] ref MSG lpmsg);

        [StructLayout(LayoutKind.Sequential)]
        public struct MSG
        {
            public IntPtr hwnd;
            public uint message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public System.Drawing.Point pt;
        }

        [Flags]
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            Windows = 8
        }

        public static async Task<string> GetUrlMarkDown(string url)
        {
            // string url = "https://example.com"; // 你要获取标题的网址

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(url);
            string html = await response.Content.ReadAsStringAsync();

            string title = GetTitleFromHtml(html);
            return $"[{title}]({url})";
        }

        static string GetTitleFromHtml(string html)
        {
            // 使用正则表达式提取<title>标签中的内容作为标题
            Match match = Regex.Match(html, @"<title[^>]\s*(.+?)\s*</title>");
            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }
            else
            {
                return "Title not found";
            }
        }
        public static string GetClipboardData()
        {
            // 检查剪贴板是否包含文本数据
            if (Clipboard.ContainsText(TextDataFormat.Text))
            {
                return Clipboard.GetText(TextDataFormat.Text);
            }
            else
            {
                return "剪贴板中没有文本数据.";
            }
        }

        public static void SetClipboardData(string text)
        {
            // 设置剪贴板的文本数据
            Clipboard.SetText(text);
        }

    }
}