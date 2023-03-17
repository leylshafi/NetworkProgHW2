using Server;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

var listener = new TcpListener(IPAddress.Parse("127.0.0.1"),45678);
listener.Start();
while (true)
{
    var client = await listener.AcceptTcpClientAsync();
    new Task(() =>
    {
        var stream = client.GetStream();
        var bw = new BinaryWriter(stream);
        var br = new BinaryReader(stream);
        while (true)
        {
            var jsonStr = br.ReadString();

            var command = JsonSerializer.Deserialize<Command>(jsonStr);

            if (command is null)
                return;

            switch (command.Text.ToLower())
            {
                case Command.HELP:
                    {
                        var helpText = HelpText();
                        bw.Write(helpText);
                        stream.Flush();
                        break;
                    }
                case Command.PROCLIST:
                    {
                        var jsonList = GetProcesses();
                        bw.Write(jsonList);
                        stream.Flush();
                        break;
                    }
                case Command.KILL:
                    {
                        var canKill = KillProcess(command.Param);
                        bw.Write(canKill);
                        break;
                    }
                case Command.RUN:
                    {
                        var canRun = RunProcess(command.Param);
                        bw.Write(canRun);
                        break;
                    }
                default:
                    break;
            }

        }




    }).Start();

    string HelpText()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("\nproclist ------ see all processes");
        builder.Append("\nkill <process name> ------ end process");
        builder.Append("\nrun <process name> ------ run process");

        return builder.ToString();
    }
    bool KillProcess(string? processName)
    {
        if (processName is not null)
        {
            
            var processes = Process.GetProcessesByName(processName);

            if (processes.Length > 0)
            {
                try
                {
                    foreach (var p in processes)
                        p.Kill();

                    return true;
                }
                catch (Exception) { }
            }
            return false;

        }
        else return false;
    }

    bool RunProcess(string? processName)
    {
        if (processName is not null)
        {
            //CalculatorApp

            try
            {
                Process.Start(processName);
                return true;
            }
            catch (Exception) { return false; }

        }
        else return false;
    }

    string GetProcesses()
    {
        var list = Process.GetProcesses();
        var names=list.Select(p => p.ProcessName);
        var jsonList = JsonSerializer.Serialize(names);

        return jsonList;
    }
}