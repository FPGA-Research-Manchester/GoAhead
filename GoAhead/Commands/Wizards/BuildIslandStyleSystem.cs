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
            systemParameter.Read(this.XMLSpecification);

            String staticProjectDir = systemParameter.StaticParameter[IslandStyleSystemParameter.ISEProjectDir].InnerText;
            String partialProjectDir = systemParameter.PartialParameter[IslandStyleSystemParameter.ISEProjectDir].InnerText;
            List<String> partialAreaPlaceholderVHDLFiles = new List<String>();
            List<String> partialAreas = new List<String>();

            // the blokcer around the partial areas
            List<String> staticPlaceHolder = new List<String>();
            List<String> modulesPerArea = new List<String>();

            // run the selection commands as the BuildIslandStyleStaticSystem works on them
            foreach (KeyValuePair<String, PartialAreaSetting> tupel in systemParameter.PartialAreas)
            {
                String selCommand = tupel.Value.Settings[IslandStyleSystemParameter.Geometry];
                String prName = tupel.Key;

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

            List<String> connectionPrimitives = new List<String>();
            foreach (KeyValuePair<String, PartialAreaSetting> tupel in systemParameter.PartialAreas)
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
        public String XMLSpecification = "";
    }

    public class IslandStyleSystemParameter
    {
        public void Read(String XMLSpecification)
        {
            m_xmlDoc.Load(XMLSpecification);

            XmlElement xmlEl = m_xmlDoc.DocumentElement;

            foreach (XmlNode xmlNode in xmlEl.ChildNodes)
            {
                if (Regex.IsMatch(xmlNode.Name, "(" + IslandStyleSystemParameter.Device + ")|(" + IslandStyleSystemParameter.ConnectionPrimitive + ")"))
                {
                    //this.SystemParameter[xmlNode.Name] = xmlNode.InnerText;
                    this.SystemParameter[xmlNode.Name] = xmlNode;
                }
                else if (Regex.IsMatch(xmlNode.Name, IslandStyleSystemParameter.StaticInfo))
                {
                    foreach (XmlNode innerNode in xmlNode.ChildNodes)
                    {
                        this.StaticParameter[innerNode.Name] = innerNode;
                    }
                }
                else if (Regex.IsMatch(xmlNode.Name, IslandStyleSystemParameter.PartialInfo))
                {
                    foreach (XmlNode innerNode in xmlNode.ChildNodes)
                    {
                        this.PartialParameter[innerNode.Name] = innerNode;
                    }
                }
                else if (Regex.IsMatch(xmlNode.Name, IslandStyleSystemParameter.PartialArea))
                {
                    String name = xmlNode.Attributes[0].Value;
                    if (this.PartialAreas.ContainsKey(name))
                    {
                        throw new ArgumentException("A partial area named " + name + " already exists");
                    }
                    this.PartialAreas.Add(name, new PartialAreaSetting());

                    foreach (XmlNode innerNode in xmlNode.ChildNodes)
                    {
                        if (innerNode.Name.Equals(IslandStyleSystemParameter.Module))
                        {
                            ModuleSetting moduleSettings = new ModuleSetting();
                            this.PartialAreas[name].Modules.Add(moduleSettings);
                            for (int i = 0; i < innerNode.Attributes.Count; i++)
                            {
                                moduleSettings.Settings[innerNode.Attributes[i].Name] = innerNode.Attributes[i].Value;
                            }
                        }
                        this.PartialAreas[name].Settings[innerNode.Name] = innerNode.InnerText;
                        this.PartialAreas[name].Nodes[innerNode.Name] = innerNode;
                    }
                }
            }
        }

        public XmlDocument XmlDoc
        {
            get { return this.m_xmlDoc; }
            set { this.m_xmlDoc = value; }
        }

        public const String StaticInfo = "static_info";
        public const String PartialInfo = "partial_info";
        public const String PartialArea = "partial_area";
        public const String ISEProjectDir = "ise_project_dir";
        public const String ConnectionPrimitive = "connection_primitive";
        public const String Device = "device";
        public const String Geometry = "geometry";
        public const String Interface = "interface";
        public const String VHDL = "vhdl";
        public const String Module = "module";
        public const String Netlist = "netlist";

        public Dictionary<String, PartialAreaSetting> PartialAreas = new Dictionary<String, PartialAreaSetting>();

        //public Dictionary<String, String> SystemParameter = new Dictionary<String, String>();
        public Dictionary<String, XmlNode> SystemParameter = new Dictionary<String, XmlNode>();

        public Dictionary<String, XmlNode> PartialParameter = new Dictionary<String, XmlNode>();
        public Dictionary<String, XmlNode> StaticParameter = new Dictionary<String, XmlNode>();

        public Dictionary<String, XmlNode> PartialAreaNodes = new Dictionary<String, XmlNode>();

        private XmlDocument m_xmlDoc = new XmlDocument();
    }

    public class ModuleSetting
    {
        public Dictionary<String, String> Settings = new Dictionary<String, String>();
    }

    public class PartialAreaSetting
    {
        public Dictionary<String, String> Settings = new Dictionary<String, String>();
        public Dictionary<String, XmlNode> Nodes = new Dictionary<String, XmlNode>();
        public List<ModuleSetting> Modules = new List<ModuleSetting>();
        public XmlNode Node = null;
    }
}