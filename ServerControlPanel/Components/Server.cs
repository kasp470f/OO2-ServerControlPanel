using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Windows.Media;


namespace ServerControlPanel.Components
{
    public class Server
    {
        public static List<Server> serverStats = new List<Server>();

        public string ServerId { get; set; }
        public Brush StatusColor { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }

        
        public string Uptime { get; set; }
        public string Disk { get; set; }
        public string RAM { get; set; }
        public string CPU { get; set; }


        public static void AddServer(string serverid, bool statuscolor, string ip, string port, SshClient client)
        {
            serverStats.Add(new Server() 
            { 
                ServerId = serverid,
                StatusColor = Status(statuscolor), 
                IP = ip, 
                Port = port,
                Uptime = "NONE",
                Disk = DiskSpaceAvailable(client),
                RAM = string.Format(CheckUsage(client.RunCommand("ps -eo %mem").Result) + "%"),
                CPU = string.Format(CheckUsage(client.RunCommand("ps -eo %cpu").Result) + "%"),
            });;
        }

        private static Brush Status(bool connected)
        {
            return (connected) ? Brushes.Green : Brushes.Gray;
        }

        private static string DiskSpaceAvailable(SshClient client)
        {
            return client.RunCommand("df /").Result.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[10];
        } 

        private static double CheckUsage(string processes)
        {
            string[] processUsage = processes.Split('\n');
            double Usage = 0;

            for (int i = 1; i < processUsage.Length - 1; i++)
            {
                Usage += double.Parse(processUsage[i]);
            }
            return Usage;
        }
    }

    
}
