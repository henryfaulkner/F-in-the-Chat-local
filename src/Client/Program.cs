using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Device.Gpio;
using System.Threading;
using System.Net.WebSockets;
using Iot.Device.RotaryEncoder;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using System.Device.Gpio;

namespace Client
{
    public class Program
    {
        private static int buttonPin = 10;
        private ClientWebSocket client = new ClientWebSocket();

        private static void RunPython(string pathToPythonFile) {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "/usr/bin/python3";
            start.Arguments = pathToPythonFile;
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using(Process process = Process.Start(start)) {
                using(StreamReader reader = process.StandardOutput) {
                    Console.Write(reader.ReadToEnd());
                }
            } 
        }

        private static void RunPython(string pathToPythonFile, string args) {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "/usr/bin/python3";
            start.Arguments = string.Format("{0} {1}", pathToPythonFile, args);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using(Process process = Process.Start(start)) {
                using(StreamReader reader = process.StandardOutput) {
                    Console.Write(reader.ReadToEnd());
                }
            } 
        }

        // We are using System.Device.Gpio
        public static void Main(string[] args)
        {
            ButtonState bs = new ButtonState();
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            bool on = false;

            using (var controller = new GpioController()) {
                controller.OpenPin(buttonPin, PinMode.Input);
                //enter to an infinite cycle to be able to handle every change in stream
                while(true) {
                    // Set a debounce timeout to filter out switch bounce noise from a button press
                    var Debounce = TimeSpan.FromMilliseconds(50);

                    if (on) {
                        controller.WaitForEvent(buttonPin, PinEventTypes.Rising, token);
                        RunPython("/Users/henryfaulkner/Desktop/Projects/F-in-the-Chat/src/python/off.py");
                        on = false;
                    } else {
                        controller.WaitForEvent(buttonPin, PinEventTypes.Falling, token);
                        RunPython("/Users/henryfaulkner/Desktop/Projects/F-in-the-Chat/src/python/rainbow.py");
                        on = true;
                    }   
                }
                controller.ClosePin(buttonPin);
            }
        }
    }
}
