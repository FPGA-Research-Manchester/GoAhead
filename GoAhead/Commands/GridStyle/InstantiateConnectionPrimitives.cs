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
            this.CheckParameters();

            for(int i = 0; i < this.NumberOfPrimitives; i++)
            {
                LibElemInst instance = new LibElemInst();
                instance.InstanceName = this.GetNextInstanceName();
                instance.LibraryElementName = this.LibraryElementName;
                Objects.LibraryElementInstanceManager.Instance.Add(instance);                
            }
        }

        private String GetNextInstanceName()
        {
            return $"{this.InstanceName}_{m_instanceIndex++}";
        }

        private void CheckParameters()
        {
            bool libraryElementNameIsCorrect = !String.IsNullOrEmpty(this.LibraryElementName);
            bool instanceNameIsCorrect = !String.IsNullOrEmpty(this.InstanceName);
            bool numberOfPrimitivesIsCorrect = this.NumberOfPrimitives >= 0;

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
        public String LibraryElementName = "DoubleSliceConnectionPrimitive";

        [Parameter(Comment = "The name of the instantiated connection primitive")]
        public String InstanceName = "inst";

        [Parameter(Comment = "The number of primitives to instantiate")]
        public int NumberOfPrimitives = 4;
    }
}
