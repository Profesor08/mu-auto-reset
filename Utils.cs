using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using Newtonsoft.Json;

namespace MuAutoReset
{
    class MuCharacter
    {
        public string Name;
        public int ResetLevel;
        public string Login;
        public string Password;
        public int CharacterId;
        public int PartyMemberId;
        public int CommandsDelay;
        public string[] Commands;
    }

    class MuClientConfig
    {
        public string WorkingDirectory;
        public string FileName;
    }

    class Config
    {
        public MuClientConfig Client;
        public MuCharacter[] Characters;
    }

    class Utils
    {
        public static string GetProcessWindowTitle(Process process) {
            var titleLenght = NativeMethods.GetWindowTextLength(process.MainWindowHandle);
            var sb = new StringBuilder(titleLenght + 1);

            NativeMethods.GetWindowText(process.MainWindowHandle, sb, sb.Capacity);


            return sb.ToString();
        }

        public static string LoadConfigFile() {
            return @"
                {
                    ""Client"": {
                        ""WorkingDirectory"": ""E:\\Games\\MuOnlineEU S15v10\\"",
                        ""FileName"": ""main.exe""
                    },
                    ""Characters"": [
                        {
                            ""Name"": ""JWL3"",
                            ""ResetLevel"": 25,
                            ""Login"": ""twink2"",
                            ""Password"": ""twink2"",
                            ""CharacterId"": 2,
                            ""PartyMemberId"": 1,
                            ""CommandsDelay"": 3000,
                            ""Commands"": [
                                ""/addstr 1000"",
                                ""/addagi 7000"",
                                ""/addcmd 31985""
                            ]
                        }
                    ]
                }
            ";
        }

        public static Config LoadConfig() {
            return JsonConvert.DeserializeObject<Config>(LoadConfigFile());
        }
    }
}
