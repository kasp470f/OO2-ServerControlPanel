using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public string Processes { get; set; }
        public bool Status { get; set; }

        public static void AddServer(string serverid, bool statuscolor, string ip, string port, string username, string password, SshClient client)
        {
            serverStats.Add(new Server()
            {
                ServerId = serverid,
                StatusColor = StatusC(statuscolor),
                IP = ip,
                Port = port,
                Username = username,
                Password = password,
                Uptime = UptimeTime(client),
                Disk = DiskSpaceAvailable(client),
                RAM = string.Format(CheckUsage(client.RunCommand("ps -eo %mem").Result) + "%"),
                CPU = string.Format(CheckUsage(client.RunCommand("ps -eo %cpu").Result) + "%"),
                Processes = client.RunCommand("ps -eo user,pid,%cpu,%mem,command").Result,
                Status = true,
            });
        }

        private static Brush StatusC(bool connected)
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
            string[] array = client.RunCommand("df /").Result.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string used = array[8];
            string available = array[9];
            string procent = array[10];

            return string.Format($"{FormatBytes(long.Parse(used) * 1000)} used of {FormatBytes(long.Parse(available) * 1000)} ({procent})");
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

        public static string FormatBytes(long bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB", "PB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            return string.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }

        public static void ReloadServers()
        {
            for (int i = 0; i < serverStats.Count; i++)
            {
                Server server = serverStats[i];
                try
                {
                    new TaskFactory().StartNew(() =>
                    {
                        try
                        {
                            using (var client = new SshClient(server.IP, int.Parse(server.Port), server.username, server.password))
                            {
                                client.ConnectionInfo.Timeout = TimeSpan.FromSeconds(5);
                                client.Connect();

                                server.StatusColor = StatusC(client.IsConnected);
                                server.Uptime = UptimeTime(client);
                                server.Disk = DiskSpaceAvailable(client);
                                server.RAM = string.Format(CheckUsage(client.RunCommand("ps -eo %mem").Result) + "%");
                                server.CPU = string.Format(CheckUsage(client.RunCommand("ps -eo %cpu").Result) + "%");
                                server.Processes = client.RunCommand("ps -eo user,pid,%cpu,%mem,command").Result;
                                server.Status = true;
                                
                                client.Disconnect();
                            }
                        }
                        catch (Exception)
                        {
                            server.StatusColor = Brushes.Gray;
                            server.Status = false;
                        }
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public static void Reboot(Server Source)
		{
            try
            {
                using (var client = new SshClient(Source.IP, int.Parse(Source.Port), Source.username, Source.password))
                {
                    client.Connect();
                    client.RunCommand("reboot");
                    client.Disconnect();
                }
            }
            catch (Exception)
            {
            }
		}
    }
}
