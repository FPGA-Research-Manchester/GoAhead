using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GoAhead.Code.VHDL
{
    public class VHDLParserEntity
    {
        public string WholeEntityAsString = "";
        public string EntityName = "";
        public List<HDLEntitySignal> InterfaceSignals = new List<HDLEntitySignal>();
    }

    public class HDLEntitySignalMetaData
    {
        /// <summary>
        /// Where to place the signal in an island
        /// </summary>
        public FPGA.FPGATypes.Direction Direction = FPGA.FPGATypes.Direction.East;

        public int Column = 0;
        public string WholeMetaDataStrig = "";
    }

    public class HDLEntitySignal
    {
        public string SignalName = "";
        public string SigalType = "";
        public string Range = "";

        /// <summary>
        /// in our out
        /// </summary>
        public string SignalDirection = "";

        public int MSB = 0;
        public int LSB = 0;
        public string WholeSignalDeclaration = "";
        public HDLEntitySignalMetaData MetaData = null;
        public bool IsClock = false;
    }

    public class VHDLParser
    {
        public VHDLParser(string fileName)
        {
            m_fileName = fileName;
        }

        private void Parse()
        {
            StreamReader sr = new StreamReader(m_fileName);
            string wholeFile = sr.ReadToEnd();
            sr.Close();

            Regex regExpEntities = new Regex(@"(entity[\s\S]*?end[\s\S]*?;)", RegexOptions.IgnorePatternWhitespace & RegexOptions.CultureInvariant);
            MatchCollection entityMatches = regExpEntities.Matches(wholeFile);
            foreach (Match entityMatch in entityMatches)
            {
                VHDLParserEntity entity = new VHDLParserEntity();

                string entityCode = entityMatch.Groups[1].Value;
                string[] atoms = entityCode.Split(' ');

                entity.WholeEntityAsString = entityCode;
                entity.EntityName = atoms[1];
                entity.InterfaceSignals.AddRange(GetSignals(entityCode));

                m_entities.Add(entity);
            }
        }

        private IEnumerable<HDLEntitySignal> GetSignals(string entityCode)
        {
            int openBR = 0;
            int pos = entityCode.IndexOf('(') + 1;
            string signalString = "";
            while (pos < entityCode.Length)
            {
                if (entityCode[pos] == '(')
                {
                    openBR++;
                }
                if (entityCode[pos] == ')')
                {
                    if (openBR == 0)
                    {
                        yield return GetHDLEntitySignal(signalString, "");
                        break;
                    }
                    else
                    {
                        openBR--;
                    }
                }
                if (entityCode[pos] == ';')
                {
                    // read until end of line
                    string metaData = "";
                    pos++; // but skip the semicolon
                    while (pos < entityCode.Length)
                    {
                        char c = entityCode[pos];
                        metaData += entityCode[pos];

                        pos++;
                        if (metaData.Contains('\n'))
                        {
                            break;
                        }
                    }
                    metaData = Regex.Replace(metaData, Environment.NewLine, "");
                    yield return GetHDLEntitySignal(signalString, metaData);
                    signalString = "";
                }
                else
                {
                    signalString += entityCode[pos];
                }
                pos++;
            }
        }

        private HDLEntitySignal GetHDLEntitySignal(string signalString, string metaData)
        {
            HDLEntitySignal signal = new HDLEntitySignal();

            signal.WholeSignalDeclaration = Regex.Replace(signalString, @"^\s*", "");
            string[] atoms = signal.WholeSignalDeclaration.Split(':');
            signal.SignalName = Regex.Replace(atoms[0], @"(^\s*)|(\s*$)", "");
            atoms = Regex.Split(signal.WholeSignalDeclaration, @"(:\s+in)|(:\s+out)");
            signal.SigalType = Regex.Replace(atoms[2], @"(^\s*)|(\s*$)", "");

            signal.SignalDirection = Regex.IsMatch(signalString, @":\s*in\s*") ? "in" : Regex.IsMatch(signalString, @":\s*out\s*") ? "out" : "inout";

            if (signal.WholeSignalDeclaration.Contains('('))
            {
                // vector
                int from = signal.WholeSignalDeclaration.IndexOf('(');
                int length = signal.WholeSignalDeclaration.Length - from;
                signal.Range = signal.WholeSignalDeclaration.Substring(from, length);
                // extract digits from range
                string digtis = Regex.Replace(signal.Range, @"(downto)|(to)|(\()|(\))", "");
                string[] digitAtoms = digtis.Split(' ');
                signal.MSB = int.Parse(digitAtoms[0]);
                signal.LSB = int.Parse(digitAtoms[2]);
                //signal.Width
            }
            else
            {
                // std_logic
                signal.LSB = 0;
                signal.MSB = 0;
            }

            if (!string.IsNullOrEmpty(metaData))
            {
                HDLEntitySignalMetaData hdlMetaData = new HDLEntitySignalMetaData();
                hdlMetaData.WholeMetaDataStrig = metaData;
                if (metaData.ToLower().Contains("east")) { hdlMetaData.Direction = FPGA.FPGATypes.Direction.East; }
                if (metaData.ToLower().Contains("west")) { hdlMetaData.Direction = FPGA.FPGATypes.Direction.West; }
                if (metaData.ToLower().Contains("south")) { hdlMetaData.Direction = FPGA.FPGATypes.Direction.South; }
                if (metaData.ToLower().Contains("north")) { hdlMetaData.Direction = FPGA.FPGATypes.Direction.North; }

                Regex regExpModules = new Regex(@"column=[0-9]+(\s|$)");
                MatchCollection modulesMatches = regExpModules.Matches(metaData.ToLower());
                foreach (Match match in modulesMatches)
                {
                    string columnCode = match.Groups[0].Value;
                    columnCode = Regex.Replace(columnCode, "column=", "");
                    hdlMetaData.Column = int.Parse(columnCode);
                }

                signal.MetaData = hdlMetaData;
            }

            return signal;
        }

        public VHDLParserEntity GetEntity(int i)
        {
            if (m_entities.Count == 0)
            {
                Parse();
            }
            return m_entities[i];
        }

        public IEnumerable<VHDLParserEntity> GetEntities()
        {
            if (m_entities.Count == 0)
            {
                Parse();
            }

            return m_entities;
        }

        private List<VHDLParserEntity> m_entities = new List<VHDLParserEntity>();
        private readonly string m_fileName = "";
    }
}