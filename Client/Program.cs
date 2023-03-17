using Client;
using System.Net.Sockets;
using System.Text.Json;

var client = new TcpClient("127.0.0.1", 45678);

var stream = client.GetStream();

var bw = new BinaryWriter(stream);
var br = new BinaryReader(stream);

var userCommand = string.Empty;

while(true)
{
    Console.WriteLine("Enter commandText, then Param: ");
    userCommand = Console.ReadLine();
    if(userCommand is not null)
    {
        var temp = userCommand.Split(' ');
        var command = new Command()
        {
            Text = temp[0],
            Param = temp[1]
        };

        switch (command.Text.ToLower())
        {
            case Command.HELP:
                if (!string.IsNullOrWhiteSpace(command.Param))
                {
                    Console.WriteLine(" Wrong Syntax ");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                var jsonStr = JsonSerializer.Serialize(command);
                bw.Write(jsonStr);
                await Task.Delay(50);
                var response = br.ReadString();
                Console.WriteLine(response);
                Console.ReadKey();
                Console.Clear();
                break;
            case Command.PROCLIST:
                if (!string.IsNullOrWhiteSpace(command.Param))
                {
                    Console.WriteLine(" Wrong Syntax ");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }

                jsonStr = JsonSerializer.Serialize(command);
                bw.Write(jsonStr);
                await Task.Delay(50);
                response = br.ReadString();
                Console.WriteLine(response);
                Console.ReadKey();
                Console.Clear();
                break;
            case Command.KILL:
                //CalculatorApp
                if (string.IsNullOrWhiteSpace(command.Param))
                {
                    Console.WriteLine(" Wrong Syntax ");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                jsonStr = JsonSerializer.Serialize(command);

                bw.Write(jsonStr);

                await Task.Delay(50);

                var responseBool = br.ReadBoolean();
                if (responseBool is true)
                {
                    Console.WriteLine("Process succesfully ended");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Process couldn't ended");
                    Console.ReadKey();
                    Console.Clear();
                }
                break;
            case Command.RUN:
                if (string.IsNullOrWhiteSpace(command.Param))
                {
                    Console.WriteLine(" Wrong Syntax ");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                jsonStr = JsonSerializer.Serialize(command);

                bw.Write(jsonStr);

                await Task.Delay(50);

                responseBool = br.ReadBoolean();
                if (responseBool is true)
                {
                    Console.WriteLine("Process succesfully run");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Process couldn't run");
                    Console.ReadKey();
                    Console.Clear();
                }
                break;
            default:
                break;
        }
    }

    

}