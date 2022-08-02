using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoAhead.Code;

namespace GoAhead.Commands.Misc
{
    class GenerateRoutingConstraint : CommandWithFileOutput
    {
        [Parameter(Comment = "The name of the file that contains the paths to use")]
        public string InputFile = "";

        [Parameter(Comment = "The tile to set the port cost for")]
        public string NetName = "";

        protected override void DoCommandAction()
        {
            char[] separator = { '.' }; // For use in splitting a location into tile and port
            Tree<string> tree = new Tree<string>();
            List<string> nodes = new List<string>();
            using (StreamReader sr = new StreamReader(InputFile))
            {
                while (sr.ReadLine() is string nextLine) // Read line by line until null is read (end of file)
                {
                    StringBuilder sb = new StringBuilder();
                    string prevTile = "";
                    string prevPort = "";
                    string[] splitLine = nextLine.Replace(" ", "").Split("->".ToCharArray(), StringSplitOptions.RemoveEmptyEntries); // Split locations in path and remove random redundant empty entries which show up
                    foreach(string loc in splitLine)
                    {
                        string tile = loc.Split(separator)[0];
                        string port = loc.Split(separator)[1];
                        if (prevTile == "") // If this is the first in the path
                        {
                            tree.Value = port;
                            nodes.Add(loc); // Add the node to the list so it is not re-added to the tree
                            prevPort = port; 
                        }
                        else if(tile == prevTile)
                        {
                            if (!nodes.Contains(loc))
                            {
                                AddChildToSpecificNode(tree, new Tree<string>(port), prevPort); // Add to the tree
                                nodes.Add(loc);
                            }
                            prevPort = port;
                        }

                        prevTile = tile;
                    }
                }
            }
            
            //Console.WriteLine(tree.ToString());
            if(FileName != "")
            {
                try
                {
                    using(StreamWriter sw = new StreamWriter(FileName))
                    {
                        StringBuilder sb = new StringBuilder("set_property FIXED_ROUTE { ");
                        sb.Append(tree.ToString());
                        sb.Append($" }} [get_nets {NetName}]");
                        sw.WriteLine(sb.ToString());
                    }
                }
                catch (Exception ex) 
                {
                    OutputManager.WriteWarning("An exception occurred during writing to file: " + ex.Message);
                }
            }
            else
            {
                OutputManager.WriteWarning("No file name supplied!");
            }
        }
        public bool AddChildToSpecificNode(Tree<string> parent, Tree<string> childToAdd, string val)
        {
            bool success = false;
            if (parent.Value.Equals(val))
            {
                parent.AddChild(childToAdd);
                return true;
            }
            else
            {
                foreach (Tree<string> child2 in parent.Children)
                {
                    success = AddChildToSpecificNode(child2, childToAdd, val);
                    if (success)
                        break;
                }
                return success;
            }
        }
        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
