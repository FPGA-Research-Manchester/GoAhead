using System;
using System.IO;
using GoAhead.Code.TCL;
using GoAhead.Code.XDL;
using GoAhead.Commands;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Code
{
    public abstract class DesignParser
    {
        public static DesignParser CreateDesignParser(string fileName)
        {
            if (FPGA.FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.Undefined))
            {
                throw new ArgumentException("Can not load design " + fileName + " as no FPGA is loaded. Use OpenBinFPGA to open a device description first.");
            }

            if (!File.Exists(fileName))
            {
                throw new ArgumentException("File " + fileName + " not found");
            }

            switch (Path.GetExtension(fileName).ToLower())
            {
                case ".viv_nl":
                    FPGATypes.AssertBackendType(FPGATypes.BackendType.Vivado);
                    return new TCLDesignParser(fileName);
                case ".xdl":
                    FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE);
                    return new XDLDesignParser(fileName);
                default:
                    throw new ArgumentException("The extension of the argument FileName must be either xdl or viv_nl (case insensitive), but found " + Path.GetExtension(fileName));
            }
        }

        public abstract void ParseDesign(NetlistContainer nlc, Command caller);

        protected string m_fileName = "";
    }
}