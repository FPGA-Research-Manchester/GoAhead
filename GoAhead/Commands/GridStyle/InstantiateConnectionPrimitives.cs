using GoAhead.Commands.BlockingShared;
using GoAhead.Commands.LibraryElementInstantiation;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.Commands.XDLManipulation;
using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GoAhead.Commands.GridStyle
{
    class InstantiateConnectionPrimitives : Command
    {
        protected override void DoCommandAction()
        {
            CheckParameters();

            for(int i = 0; i < NumberOfPrimitives; i++)
            {
                LibElemInst instance = new LibElemInst();
                instance.InstanceName = GetNextInstanceName();
                instance.LibraryElementName = LibraryElementName;
                LibraryElementInstanceManager.Instance.Add(instance);                
            }
        }

        private string GetNextInstanceName()
        {
            return $"{InstanceName}_{m_instanceIndex++}";
        }

        private void CheckParameters()
        {
            bool libraryElementNameIsCorrect = !string.IsNullOrEmpty(LibraryElementName);
            bool instanceNameIsCorrect = !string.IsNullOrEmpty(InstanceName);
            bool numberOfPrimitivesIsCorrect = NumberOfPrimitives >= 0;

            if(!libraryElementNameIsCorrect || !instanceNameIsCorrect || !numberOfPrimitivesIsCorrect)
            {
                throw new ArgumentException("Unexpected format in one of the parameters.");
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        private static int m_instanceIndex = 0;

        [Parameter(Comment = "The name of the library element")]
        public string LibraryElementName = "DoubleSliceConnectionPrimitive";

        [Parameter(Comment = "The name of the instantiated connection primitive")]
        public string InstanceName = "inst";

        [Parameter(Comment = "The number of primitives to instantiate")]
        public int NumberOfPrimitives = 4;
    }
}
