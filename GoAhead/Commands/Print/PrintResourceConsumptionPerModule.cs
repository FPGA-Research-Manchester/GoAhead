using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.Objects;
using GoAhead.Code.XDL;
using System.Windows.Forms;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "Print the hierarchichal (per module) resource consumption", Wrapper = false, Publish = true)]
    class PrintResourceConsumptionPerModule : NetlistContainerCommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            NetlistContainer nlc = GetNetlistContainer();

            if (m_treeView == null)
            {
                m_treeView = new TreeView();
            }

            TreeNode root = new TreeNode(NetlistContainerName);
            root.Tag = new TreeNodeTag();
            TreeNode topLevel = new TreeNode("Top Level Logic");
            topLevel.Tag = new TreeNodeTag();
            m_treeView.Nodes.Add(root);
            root.Nodes.Add(topLevel);

            foreach (XDLInstance inst in nlc.Instances)
            {
                string instanceName = inst.Name;
                string[] atoms = instanceName.Split('/');                            

                if (atoms.Length == 1)
                {
                    TreeNode newNode = new TreeNode(atoms[0]);
                    TreeNodeTag tag = new TreeNodeTag();
                    tag.Instance = inst;
                    newNode.Tag = tag;
                    topLevel.Nodes.Add(newNode);
                }
                else if (atoms.Length > 1)
                {
                    TreeNode currentNode = root;
                    for(int i=0;i<atoms.Length;i++)
                    {
                        string nodeName = atoms[i];
                        TreeNode parent = currentNode.Find(nodeName);
                        if (parent == null)
                        {
                            TreeNode newNode = new TreeNode(nodeName);
                            currentNode.Nodes.Add(newNode);
                            currentNode = newNode;
                            if (i == atoms.Length - 1)
                            {
                                TreeNodeTag tag = new TreeNodeTag();
                                tag.Instance = inst;
                                newNode.Tag = tag;
                            }
                            else
                            {
                                newNode.Tag = new TreeNodeTag();
                            }
                        }
                        else
                        {
                            // descend further down
                            currentNode = parent;
                        }
                    }
                }
            }

            int hierarchy = 0;
            foreach (TreeNode n in root.Nodes)
            {
                int count = n.GetNodeCount(true);
                OutputManager.WriteOutput(n.Text + ": " + count);
                hierarchy++;
            }
        }
        
        public TreeView TreeView
        {
            get { return m_treeView; }
            set { m_treeView = value; }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        private TreeView m_treeView = null;
    }

    public class TreeNodeTag
    {
        public XDLInstance Instance { get; set; }
    }

    public static class TreeNodeExtension
    {
        public static TreeNode Find(this TreeNode motherNode, string findNodeText)
        {
            TreeNode result = null;
            foreach (TreeNode childNode in motherNode.Nodes)
            {
                if (childNode.Text.Equals(findNodeText, StringComparison.CurrentCulture))
                {
                    result = childNode;
                }
            }
            return result;
        }

        public static IEnumerable<TreeNode> GetChildNodes(this TreeNode root)
        {
            foreach (TreeNode childNode in root.Nodes)
            {
                if (childNode.Nodes.Count == 0)
                {
                    yield return childNode;
                }
                else
                {
                    foreach (TreeNode n in GetChildNodes(childNode))
                    {
                        yield return n; 
                    }
                }
            }

        }
    }
}
