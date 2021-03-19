using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;


namespace ServerControlPanel.Components
{
    public class Server
    {
        public static List<Server> serverStats = new List<Server>();
        
        private string serverid;
        private Brush statuscolor;
        private string ip;
        private string port;
        private string username;
        private string password;

        public string ServerId { get => serverid; set => serverid = value; }
        public Brush StatusColor { get => statuscolor; set => statuscolor = value; }
        public string IP { get => ip; set => ip = value; }
        public string Port { get => port; set => port = value; }
        public string Username { set => username = value; }
        public string Password { set => password = value; }

        public string Uptime { get; set; }
        public string Disk { get; set; }
        public string RAM { get; set; }
        public string CPU { get; set; }

        public static void AddServer(string serverid, bool statuscolor, string ip, string port, string username, string password, SshClient client)
        {
            serverStats.Add(new Server() 
            { 
                ServerId = serverid,
                StatusColor = Status(statuscolor), 
                IP = ip, 
                Port = port,
                Username = username,
                Password = password,
                Uptime = UptimeTime(client),
                Disk = DiskSpaceAvailable(client),
                RAM = string.Format(CheckUsage(client.RunCommand("ps -eo %mem").Result) + "%"),
                CPU = string.Format(CheckUsage(client.RunCommand("ps -eo %cpu").Result) + "%"),
            });
        }

        private static Brush Status(bool connected)
        {
            return (connected) ? Brushes.Green : Brushes.Gray;
        }

        private static string UptimeTime(SshClient client)
        {
            double secs = double.Parse(client.RunCommand("cat /proc/uptime").Result.Split(' ')[1]);
            TimeSpan t = TimeSpan.FromSeconds(secs);

            string answer = string.Format("{0:D2} hours, {1:D2} minutes, {2:D2} seconds",
                            t.Hours,
                            t.Minutes,
                            t.Seconds
                            );
            return answer;
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


        public static void ReloadServers()
        {

            foreach (Server server in serverStats)
            {
                try
                {
                    using (var client = new SshClient(server.IP, int.Parse(server.Port), server.username, server.password))
                    {
                        client.Connect();

                        server.StatusColor = Status(client.IsConnected);
                        server.Uptime = UptimeTime(client);
                        server.Disk = DiskSpaceAvailable(client);
                        server.RAM = string.Format(CheckUsage(client.RunCommand("ps -eo %mem").Result) + "%");
                        server.CPU = string.Format(CheckUsage(client.RunCommand("ps -eo %cpu").Result) + "%");

                        client.Disconnect();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show($"The {server.serverid} could not be found!");
                    server.StatusColor = Brushes.Gray;
                }
            }
        }
    }

    
}
