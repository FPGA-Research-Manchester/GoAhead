using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Net;
using GoAhead.Commands;
using System.Collections.Generic;

namespace GoAhead.Objects
{
    public class CommandServer
    {
        public void Run(int portNumber)
        {
            TcpListener serverSocket = new TcpListener(portNumber);
            TcpClient clientSocket = default; 
            serverSocket.Start();
            Console.WriteLine(">> GoAhead Command Server Started");
            clientSocket = serverSocket.AcceptTcpClient();
            Console.WriteLine(">> Connection established");
            bool connOpen = true;
            CommandStringParser parser = new CommandStringParser("");

            while (connOpen)
            {
                try
                {
                    NetworkStream networkStream = clientSocket.GetStream();
                    byte[] bytesFrom = new byte[65536];
                    networkStream.Read(bytesFrom, 0, clientSocket.ReceiveBufferSize);

                    string dataFromClient = Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf('$'));
                    string[] atoms = dataFromClient.Split(';');

                    List<byte> sendBytesList = new List<byte>();
                    bool allCommandsValid = true;

                    foreach (string commandString in atoms.Where(s => !string.IsNullOrEmpty(s)))
                    {
                        string errorDescription = "";
                        Command cmd = null;

                        bool valid = parser.ParseCommand(commandString, true, out cmd, out errorDescription);
                        if (commandString.ToLower().Trim() == "q")
                        {
                            connOpen = false;
                            Console.WriteLine(">> Closing server - press ENTER to exit");
                            break;
                        }
                        else if (valid)
                        {
                            CommandExecuter.Instance.Execute(cmd);
                            sendBytesList.AddRange(Encoding.ASCII.GetBytes("result=" + cmd.OutputManager.GetOutput() + "$"));
                        }
                        else
                        {
                            byte[] sendBytes = Encoding.ASCII.GetBytes(errorDescription);
                            networkStream.Write(sendBytes, 0, sendBytes.Length);
                            networkStream.Flush();
                            allCommandsValid = false;
                        }
                    }

                    if(allCommandsValid)
                    {
                        byte[] sendBytes = sendBytesList.ToArray();
                        //byte[] sendBytes = Encoding.ASCII.GetBytes("result=" + cmd.OutputManager.GetOutput() + "$");
                        networkStream.Write(sendBytes, 0, sendBytes.Length);
                        networkStream.Flush();
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