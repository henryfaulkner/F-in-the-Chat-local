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

namespace Client
{
    public class Program
    {
        private int pin = 10;
        private ClientWebSocket client = new ClientWebSocket();

        private void SendClientRequestOn(int cum, PinValueChangedEventArgs e) {
            // toggle the state of the LED every time the button is pressed
            if (e.ChangeType == PinEventTypes.Rising)
            {
                return;
            }
        }

        private void SendClientRequestOff(int cum, PinValueChangedEventArgs e) {
            // toggle the state of the LED every time the button is pressed
            if (e.ChangeType == PinEventTypes.Falling)
            {
                return;
            }
        }

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

        // https://docs.microsoft.com/en-us/samples/microsoft/windows-iotcore-samples/push-button/
        // ^ Good reference for button
        public static void Main(string[] args)
        {
            //server implementation
            TcpListener server = new TcpListener(IPAddress.Parse(""), 80);
            server.Start();
            Console.WriteLine("Server has started on :80. Waiting for a connection…\n");
            //triggers when a client connects
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("A client connected.");
            NetworkStream stream = client.GetStream();
            
            bool on = false;
            using (var controller = new GpioController()) {
                //enter to an infinite cycle to be able to handle every change in stream
                while(true) {
                    GpioDriver driver = new GpioDriver();
                    
                        controller.OpenPin(pin, PinMode.Output);

                        // Check if input pull-up resistors are supported
                        if (driver.IsPinModeSupported(pin, GpioPinDriveMode.InputPullUp))
                            driver.SetPinMode(pin, GpioPinDriveMode.InputPullUp);
                        else
                            driver.SetPinMode(pin, GpioPinDriveMode.Input);
                        // Set a debounce timeout to filter out switch bounce noise from a button press
                        var Debounce = TimeSpan.FromMilliseconds(50);

                        // Register for the ValueChanged event so our buttonPin_ValueChanged 
                        // function is called when the button is pressed
                        ValueChanged += SendClientRequestOn;
                        
                
                    //traps here until some bytes of data have been sent
                    while(!stream.DataAvailable);
                    //TCPHandshake(client, ref stream);
                    Console.WriteLine(ReadTransmission(client, stream));
                    if(on) {
                        // absolute path of file
                        //RunPython("/Users/henryfaulkner/Desktop/Projects/F-in-the-Chat/server/python/off.py");
                        //Console.WriteLine("Turn Off.");
                        on = false;
                    } else {
                        // absolute path of file
                        //RunPython("/Users/henryfaulkner/Desktop/Projects/F-in-the-Chat/server/python/rainbow.py");
                        //Console.WriteLine("Turn on.");
                        on = true;
                    }
                }
            }
        }
    }
}
