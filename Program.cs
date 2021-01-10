using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Data;
using System.Drawing;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace MuAutoReset
{

    class Program
    {

        public static int MyHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                // You will need to define the struct
                //var message = (CWPSTRUCT)Marshal.PtrToStructure(lParam, typeof(CWPSTRUCT));
                // Do something with the data
            }

            Console.WriteLine(nCode);

            return 0; // Return value is ignored unless you set callNext to false
        }

        static void Main(string[] args)
        {

            var mainProcesses = MuClient.StartAll(Utils.LoadConfig());

            foreach(var item in mainProcesses)
            {
                var main = item.Value;

                var hookProc = new NativeMethods.HookProc(MyHookCallback);

                var hook = NativeMethods.SetWindowsHookEx(NativeMethods.HookType.WH_GETMESSAGE, hookProc, (IntPtr)(0), 0);

                //Hook MyHook = new Hook(HookType.WH_CALLWNDPROC, false, false);
                //MyHook.Callback += MyHookCallback;
                //MyHook.StartHook();
                
            }


            var x = 0;
            var y = 0;

            Thread.Sleep(20000);

            while (true)
            {
                Thread.Sleep(100);

                var config = Utils.LoadConfig();

                foreach (var character in config.Characters)
                {
                    var main = mainProcesses[character.Name];

                    NativeMethods.MakeClick(main, 450, 330);

                    var childs = NativeMethods.GetAllChildrenWindowHandles(main.MainWindowHandle, 10);

                    foreach (var child in childs)
                    {
                        if (child != null)
                        {
                            //NativeMethods.MakeClick(child, 450, 330);
                        }
                    }

                    //var level = MuClient.GetLevel(main);

                    //if (character.ResetLevel <= level)
                    //{
                    //    Console.WriteLine("Reset: {0}, on level: {1}", character.Name, level);
                    //    Thread.Sleep(character.CommandsDelay);
                    //    MuClient.MakeReset(main, character);
                    //}



                    //Console.WriteLine(title);

                    //foreach (Match match in matches)
                    //{
                    //    Console.WriteLine(match.Groups[1].Value);
                    //}

                    //Thread.Sleep(10000);

                    //NativeMethods.MakeClick(main, (int)(600 / 1.25), (int)(480 / 1.25));
                }
            }
            

            /*
            var mainProcesses = new Dictionary<string, Process>();

            var startInfo = new ProcessStartInfo();
            startInfo.WorkingDirectory = "E:\\Games\\MuOnlineEU S15v10\\";
            startInfo.FileName = "main.exe";

            var tmain = Process.Start(startInfo);
            mainProcesses.Add("JWL3", tmain);


            while (true)
            {
                var config = Utils.LoadConfig();

                Console.Write("Character: ");
                string characterName = Console.ReadLine();

                foreach(var character in config.Characters)
                {
                    if (character.Name.ToLower() == characterName.ToLower())
                    {
                        var main = mainProcesses[character.Name];

                        Console.WriteLine(Utils.GetProcessWindowTitle(main));

                        Thread.Sleep(5000);

                        foreach (var command in character.Commands)
                        {
                            try
                            {
                                Console.WriteLine(command);
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }
                        }
                    }
                }

                
            }
            
    */


            //startInfo.WorkingDirectory = "E:\\Games\\MuOnlineEU S15v10\\";

            //startInfo.FileName = "main.exe";

            //var main = Process.Start(startInfo);

            //Thread.Sleep(25000);

            //NativeMethods.SetForegroundWindow(main.MainWindowHandle);
            //NativeMethods.ShowWindow(main.MainWindowHandle, NativeMethods.ShowWindowEnum.Restore);

            //var sim = new InputSimulator();

            //sim.Keyboard.TextEntry("blablablab");
        }
    }
}
