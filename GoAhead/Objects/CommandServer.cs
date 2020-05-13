using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using GoAhead.Commands;

namespace GoAhead.Objects
{
    public class CommandServer
    {
        public void Run(int portNumber)
        {
            TcpListener serverSocket = new TcpListener(portNumber);
            TcpClient clientSocket = default(TcpClient);
            serverSocket.Start();
            Console.WriteLine(">> GoAhead Command Server Started");
            clientSocket = serverSocket.AcceptTcpClient();

            CommandStringParser parser = new CommandStringParser("");

            while ((true))
            {
                try
                {
                    NetworkStream networkStream = clientSocket.GetStream();
                    byte[] bytesFrom = new byte[10025];
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);

                    string dataFromClient = Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf('$'));
                    string[] atoms = dataFromClient.Split(';');

                    foreach (string commandString in atoms.Where(s => !string.IsNullOrEmpty(s)))
                    {
                        string errorDescription = "";
                        Command cmd = null;

                        bool valid = parser.ParseCommand(commandString, true, out cmd, out errorDescription);
                        if (valid)
                        {
                            CommandExecuter.Instance.Execute(cmd);
                            byte[] sendBytes = Encoding.ASCII.GetBytes("result=" + cmd.OutputManager.GetOutput() + "$");
                            networkStream.Write(sendBytes, 0, sendBytes.Length);
                            networkStream.Flush();
                        }
                        else
                        {
                            byte[] sendBytes = Encoding.ASCII.GetBytes(errorDescription);
                            networkStream.Write(sendBytes, 0, sendBytes.Length);
                            networkStream.Flush();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    break;
                }
            }

            clientSocket.Close();
            serverSocket.Stop();
            Console.ReadLine();
        }
    }
}