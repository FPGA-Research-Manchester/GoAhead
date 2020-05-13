using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using GoAhead.Commands.InterfaceManager;
using GoAhead.Commands.Selection;

namespace GoAhead.Commands.Wizards
{
    [CommandDescription(Description = "Reads in the system specification from the provied XML files and ...", Wrapper = true, Publish = true)]
    public class BuildIslandStyleSystem : Command
    {
        protected override void DoCommandAction()
        {
            IslandStyleSystemParameter systemParameter = new IslandStyleSystemParameter();
            systemParameter.Read(XMLSpecification);

            string staticProjectDir = systemParameter.StaticParameter[IslandStyleSystemParameter.ISEProjectDir].InnerText;
            string partialProjectDir = systemParameter.PartialParameter[IslandStyleSystemParameter.ISEProjectDir].InnerText;
            List<string> partialAreaPlaceholderVHDLFiles = new List<string>();
            List<string> partialAreas = new List<string>();

            // the blokcer around the partial areas
            List<string> staticPlaceHolder = new List<string>();
            List<string> modulesPerArea = new List<string>();

            // run the selection commands as the BuildIslandStyleStaticSystem works on them
            foreach (KeyValuePair<string, PartialAreaSetting> tupel in systemParameter.PartialAreas)
            {
                string selCommand = tupel.Value.Settings[IslandStyleSystemParameter.Geometry];
                string prName = tupel.Key;

                partialAreas.Add(tupel.Key);
                partialAreaPlaceholderVHDLFiles.Add(staticProjectDir + "partial_area_placeholder_" + tupel.Key + ".vhd");

                CommandExecuter.Instance.Execute(new ClearSelection());
                CommandExecuter.Instance.Execute(selCommand);
                CommandExecuter.Instance.Execute(new ExpandSelection());
                StoreCurrentSelectionAs storeAsCmd = new StoreCurrentSelectionAs();
                storeAsCmd.UserSelectionType = prName;
                CommandExecuter.Instance.Execute(storeAsCmd);

                // assign the interfaces
                LoadInterfaceAsCSV loadInterfaceCmd = new LoadInterfaceAsCSV();
                loadInterfaceCmd.FileName = tupel.Value.Settings[IslandStyleSystemParameter.Interface];
                loadInterfaceCmd.PartialArea = prName;
                CommandExecuter.Instance.Execute(loadInterfaceCmd);

                staticPlaceHolder.Add(partialProjectDir + "static_placeholder_" + prName + ".vhd");

                foreach (ModuleSetting moduleSetting in tupel.Value.Modules)
                {
                    modulesPerArea.Add(prName + ":" + moduleSetting.Settings["path"]);
                }
            }

            List<string> connectionPrimitives = new List<string>();
            foreach (KeyValuePair<string, PartialAreaSetting> tupel in systemParameter.PartialAreas)
            {
                connectionPrimitives.Add(tupel.Value.Settings[IslandStyleSystemParameter.ConnectionPrimitive]);
            }

            // read in the macro
            // CommandExecuter.Instance.Execute(systemParameter.SystemParameter[IslandStyleSystemParameter.ConnectionPrimitive].InnerText);

            // build static

            BuildIslandStyleStaticSystem buildStaticCmd = new BuildIslandStyleStaticSystem();
            buildStaticCmd.ProjectDirectory = staticProjectDir;
            buildStaticCmd.ConnectionPrimitives = connectionPrimitives;
            buildStaticCmd.PartialAreas = partialAreas;
            buildStaticCmd.PartialAreaPlaceholderVHDLFiles = partialAreaPlaceholderVHDLFiles;
            CommandExecuter.Instance.Execute(buildStaticCmd);

            BuildIslandStyleModule buildModuleCmd = new BuildIslandStyleModule();
            buildModuleCmd.ProjectDirectory = partialProjectDir;
            buildModuleCmd.ConnectionPrimitives = connectionPrimitives;
            buildModuleCmd.ModulesPerArea = modulesPerArea;
            buildModuleCmd.PartialAreas = partialAreas;
            buildModuleCmd.TopVHDLFile = partialProjectDir + systemParameter.PartialParameter[IslandStyleSystemParameter.VHDL].InnerText;
            buildModuleCmd.VHDLWrapper = staticPlaceHolder;
            CommandExecuter.Instance.Execute(buildModuleCmd);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The XML system specification")]
        public string XMLSpecification = "";
    }

    public class IslandStyleSystemParameter
    {
        public void Read(string XMLSpecification)
        {
            m_xmlDoc.Load(XMLSpecification);

            XmlElement xmlEl = m_xmlDoc.DocumentElement;

            foreach (XmlNode xmlNode in xmlEl.ChildNodes)
            {
                if (Regex.IsMatch(xmlNode.Name, "(" + Device + ")|(" + ConnectionPrimitive + ")"))
                {
                    //this.SystemParameter[xmlNode.Name] = xmlNode.InnerText;
                    SystemParameter[xmlNode.Name] = xmlNode;
                }
                else if (Regex.IsMatch(xmlNode.Name, StaticInfo))
                {
                    foreach (XmlNode innerNode in xmlNode.ChildNodes)
                    {
                        StaticParameter[innerNode.Name] = innerNode;
                    }
                }
                else if (Regex.IsMatch(xmlNode.Name, PartialInfo))
                {
                    foreach (XmlNode innerNode in xmlNode.ChildNodes)
                    {
                        PartialParameter[innerNode.Name] = innerNode;
                    }
                }
                else if (Regex.IsMatch(xmlNode.Name, PartialArea))
                {
                    string name = xmlNode.Attributes[0].Value;
                    if (PartialAreas.ContainsKey(name))
                    {
                        throw new ArgumentException("A partial area named " + name + " already exists");
                    }
                    PartialAreas.Add(name, new PartialAreaSetting());

                    foreach (XmlNode innerNode in xmlNode.ChildNodes)
                    {
                        if (innerNode.Name.Equals(Module))
                        {
                            ModuleSetting moduleSettings = new ModuleSetting();
                            PartialAreas[name].Modules.Add(moduleSettings);
                            for (int i = 0; i < innerNode.Attributes.Count; i++)
                            {
                                moduleSettings.Settings[innerNode.Attributes[i].Name] = innerNode.Attributes[i].Value;
                            }
                        }
                        PartialAreas[name].Settings[innerNode.Name] = innerNode.InnerText;
                        PartialAreas[name].Nodes[innerNode.Name] = innerNode;
                    }
                }
            }
        }

        public XmlDocument XmlDoc
        {
            get { return m_xmlDoc; }
            set { m_xmlDoc = value; }
        }

        public const string StaticInfo = "static_info";
        public const string PartialInfo = "partial_info";
        public const string PartialArea = "partial_area";
        public const string ISEProjectDir = "ise_project_dir";
        public const string ConnectionPrimitive = "connection_primitive";
        public const string Device = "device";
        public const string Geometry = "geometry";
        public const string Interface = "interface";
        public const string VHDL = "vhdl";
        public const string Module = "module";
        public const string Netlist = "netlist";

        public Dictionary<string, PartialAreaSetting> PartialAreas = new Dictionary<string, PartialAreaSetting>();

        //public Dictionary<String, String> SystemParameter = new Dictionary<String, String>();
        public Dictionary<string, XmlNode> SystemParameter = new Dictionary<string, XmlNode>();

        public Dictionary<string, XmlNode> PartialParameter = new Dictionary<string, XmlNode>();
        public Dictionary<string, XmlNode> StaticParameter = new Dictionary<string, XmlNode>();

        public Dictionary<string, XmlNode> PartialAreaNodes = new Dictionary<string, XmlNode>();

        private XmlDocument m_xmlDoc = new XmlDocument();
    }

    public class ModuleSetting
    {
        public Dictionary<string, string> Settings = new Dictionary<string, string>();
    }

    public class PartialAreaSetting
    {
        public Dictionary<string, string> Settings = new Dictionary<string, string>();
        public Dictionary<string, XmlNode> Nodes = new Dictionary<string, XmlNode>();
        public List<ModuleSetting> Modules = new List<ModuleSetting>();
        public XmlNode Node = null;
    }
}