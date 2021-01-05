using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using WindowsInput;
using WindowsInput.Native;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MuAutoReset
{
    class MuClient
    {
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int VK_RETURN = 0x0D;
        const int WM_MOUSEMOVE = 0x0200;

        private static Keys ConvertCharToVirtualKey(char ch)
        {
            short vkey = NativeMethods.VkKeyScan(ch);
            Keys retval = (Keys)(vkey & 0xff);
            int modifiers = vkey >> 8;

            if ((modifiers & 1) != 0)
                retval |= Keys.Shift;
            if ((modifiers & 2) != 0)
                retval |= Keys.Control;
            if ((modifiers & 4) != 0)
                retval |= Keys.Alt;

            return retval;
        }

        public static void RunCommand(Process main, string command) {
            ToggleConfirm(main);
            Thread.Sleep(200);

            foreach(var ch in command)
            {
                NativeMethods.PostMessage(main.MainWindowHandle, WM_KEYDOWN, (int)ConvertCharToVirtualKey(ch), 0);
            }

            Thread.Sleep(200);

            ToggleConfirm(main);
        }

        public static void AddStats(Process main, MuCharacter character)
        {
            foreach(var command in character.Commands)
            {
                RunCommand(main, command);
                Thread.Sleep(character.CommandsDelay);
            }
        }

        public static void MakeReset(Process main, MuCharacter character)
        {
            RunCommand(main, "/reset");
            Thread.Sleep(character.CommandsDelay);
            AddStats(main, character);
        }

        public static void ToggleConfirm(Process main)
        {
            NativeMethods.PostMessage(main.MainWindowHandle, WM_KEYDOWN, VK_RETURN, 0);
            NativeMethods.PostMessage(main.MainWindowHandle, WM_KEYUP, VK_RETURN, 0);
        }

        public static void Login(Process main, MuCharacter character) { }

        public static void SelectCharacter(Process main, MuCharacter character) { }

        public static Process StartClient(Config config)
        {
            var startInfo = new ProcessStartInfo();
            startInfo.WorkingDirectory = config.Client.WorkingDirectory;
            startInfo.FileName = config.Client.FileName;

            return Process.Start(startInfo);
        }

        public static Dictionary<string, Process> StartAll(Config config)
        {
            var processes = new Dictionary<string, Process>();

            foreach (var character in config.Characters)
            {
                processes.Add(character.Name, StartClient(config));
            }

            return processes;
        }

        public static int GetLevel(Process main)
        {
            var title = Utils.GetProcessWindowTitle(main);

            var regex = new Regex(@"Level:\s\[(\d+)\]");

            var matches = regex.Matches(title);

            if (matches.Count >= 1)
            {
                var match = matches[0];

                if (match.Groups.Count >= 2)
                {
                    return int.Parse(match.Groups[1].Value);
                }
            }

            return -1;
        }
    }
}
