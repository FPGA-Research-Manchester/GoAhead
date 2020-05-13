using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using GoAhead.FPGA;

namespace GoAhead.Commands.Data
{
    [CommandDescription(Description = "Read a textual FPGA description (Altera)", Wrapper = true)]
    class ReadAlteraFPGA : Command
    {
        protected override void DoCommandAction()
        {
            // reset PRIOR to reading to reset high lighter 
            CommandExecuter.Instance.Execute(new Reset());
            FPGA.FPGA.Instance.Reset();
            FPGA.FPGA.Instance.BackendType = FPGATypes.BackendType.Altera;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(FileName);

            XmlElement xmlEl = xmlDoc.DocumentElement;
            Dictionary<string, XmlNode> blockTypes = new Dictionary<string, XmlNode>();

            foreach (XmlAttribute attr in xmlEl.Attributes)
            {
                if (attr.Name.Equals("name"))
                {
                    if (attr.Value.Equals("stratixv"))
                    {
                        FPGA.FPGA.Instance.Family = FPGATypes.FPGAFamily.StratixV;
                    }
                    else if (attr.Value.Equals("CycloneIVE"))
                    {
                        FPGA.FPGA.Instance.Family = FPGATypes.FPGAFamily.CycloneIVE;
                    }
                    else
                    {
                        throw new ArgumentException("Unknonwn Altera FPGA family " + attr.Value);
                    }
                }
            }

            foreach (XmlNode xmlNode in xmlEl.ChildNodes)
            {
                // get block types
                if (xmlNode.Name.Equals("BLOCK"))
                {
                    foreach (XmlAttribute attr in xmlNode.Attributes)
                    {
                        if (attr.Name.EndsWith("type"))
                        {
                            string type = attr.Value;                            
                            blockTypes.Add(type, xmlNode);                        
                        }
                        else if (attr.Name.EndsWith("IS_CONTAINED"))
                        {
                            
                        }
                        else
                        {

                        }                       
                    }
                }

                // get block instances
                if (xmlNode.Name.Equals("DEVICE"))
                {
                    foreach (XmlAttribute attr in xmlNode.Attributes)
                    {
                        if (attr.Name.Equals("name"))
                        {
                            FPGA.FPGA.Instance.DeviceName = attr.Value;
                        }
                    }

                    foreach (XmlNode instanceNode in xmlNode.ChildNodes)
                    {
                        if (instanceNode.Name.Equals("BLOCK_INSTANCE"))
                        {
                            string type = "";
                            foreach (XmlAttribute attr in instanceNode.Attributes)
                            {
                                if (attr.Name.Equals("type"))
                                {
                                    type = attr.Value;
                                }
                            }

                            // get locations
                            foreach (XmlNode locationNode in instanceNode.ChildNodes)
                            {
                                int x = -1;
                                int y = -1;
                                int z = -1;
                                string name = "";
                                foreach (XmlAttribute attr in locationNode.Attributes)
                                {
                                    if (attr.Name.Equals("x"))
                                    {
                                        x = int.Parse(attr.Value);
                                    }
                                    if (attr.Name.Equals("y"))
                                    {
                                        y = int.Parse(attr.Value);
                                    }
                                    if (attr.Name.Equals("z"))
                                    {
                                        z = int.Parse(attr.Value);
                                    }
                                    if (attr.Name.Equals("name"))
                                    {
                                        name = attr.Value;                                       
                                    }
                                }

                                TileKey key = new TileKey(x, y);
                                Tile t = new Tile(key, name);
                                if (!blockTypes.ContainsKey(type))
                                {
                                }
                                else
                                {
                                    XmlNode blockTypeNode = blockTypes[type];
                                }
                                //Console.WriteLine(key.ToString() + " " + name);
                                if (FPGA.FPGA.Instance.Contains(key))
                                {
                                    Tile containingTile = FPGA.FPGA.Instance.GetTile(key);
                                    containingTile.Add(t);
                                }
                                else
                                {
                                    FPGA.FPGA.Instance.Add(t);
                                }
                            }
                        }
                    }                
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The file to read")]
        public string FileName = "cycloneive.arch";
    }
}
