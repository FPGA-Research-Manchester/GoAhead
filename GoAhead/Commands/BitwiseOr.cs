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
    class BitwiseOr : Command
    {
        protected override void DoCommandAction()
        {
            byte[] result = null;
            int lastLength = 0;
            String lastFile = "";
            foreach (String f in this.Inputs)
            {
                byte[] bytes = File.ReadAllBytes(f);
                if (result == null)
                {
                    result = new byte[bytes.Length];
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        result[i] = 0;
                    }
                }
                if (lastLength == 0)
                {
                    lastLength = bytes.Length;
                    lastFile = f;
                }
                else if (lastLength != bytes.Length)
                {
                    throw new ArgumentException("File " + f + " has a different byte size than " + lastFile);                    
                }
                lastLength = bytes.Length;
                lastFile = f;

                for (int i = 0; i < bytes.Length; i++)
                {
                    result[i] |= bytes[i];
                }
            }
            File.WriteAllBytes(this.Output, result);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }


        [Parameter(Comment = "The result (overwritten if it already exists)")]
        public String Output = "out.bin";

        [Parameter(Comment = "A list of files")]
        public List<String> Inputs = new List<String>();
    }
}
