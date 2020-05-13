using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;
using GoAhead.Commands;

namespace GoAhead.Commands
{
    class WriteToComPort : Command
    {    
        protected override void DoCommandAction()
        {
            SerialPort serialPort = new SerialPort();
            serialPort.PortName = ComPort;
            serialPort.BaudRate = 115200;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.Two;
            serialPort.Handshake = Handshake.None;
            serialPort.ReadTimeout = 500;
            serialPort.WriteTimeout = 500;

            serialPort.Open();

            byte[] fileBytes = File.ReadAllBytes(Filename);

            int index = 0;
            while (true)
            {
                if (index + 1024 < fileBytes.Length)
                {
                    Console.WriteLine("Sending 1024 bytes");
                    serialPort.Write(fileBytes, index, 1024);
                }
                else
                {                    
                    int remainingLength = fileBytes.Length - index;
                    Console.WriteLine("Sending remaining " + remainingLength + " bytes");
                    serialPort.Write(fileBytes, index, remainingLength);
                    break;
                }
                index += 1024;
                Thread.Sleep(Delay);
            }

            serialPort.Close();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The delay between two chunks in milli seconds")]
        public int Delay = 1000;    

        [Parameter(Comment = "The com port to use")]
        public string ComPort = "COM1";    

        [Parameter(Comment = "The binary file to wrtie to the com port")]
        public string Filename = "bin.data";      
    }
}
