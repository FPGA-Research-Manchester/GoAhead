using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoAhead.Code
{
    internal class Tree<T>
    {
        public T Value { get; set; }
        public List<Tree<T>> Children { get; }

        public Tree() 
        {
            Value = default;
            Children = new List<Tree<T>>();
        }
        public Tree (T value)
        {
            Value = value;
            Children = new List<Tree<T>>();
        }

        /// <summary>
        /// Add a tree to this node's list of children.
        /// </summary>
        /// <param name="child">The child tree to add.</param>
        public void AddChild(Tree<T> child) 
        {
            Children.Add(child);
        }

        /// <summary>
        /// Override of base ToString() method making it recursive to give the tree string proper formatting.
        /// </summary>
        /// <returns>A string representation of this tree with children leading to sink nodes in curly braces.</returns>
        public override string ToString() 
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{Value} "); // The spaces are important!
            int index = 1; // Used to properly format nodes with more than 2 children
            bool done = false; // Used later in the foreach loop
            if(Children.Count > 1)
            {
                sb.Append("{ ");
            }
            foreach(Tree<T> child in Children) // Go through every child and add curly braces where necessary
            {
                sb.Append(child.ToString()); // Recursive call 
                index++;
                if (Children.Count > 1 && index >= Children.Count && !done)
                {
                    sb.Append("} ");
                    done = true; // Without this there are redundant curly braces at the end of the string
                }
                else if (Children.Count > 1 && index < Children.Count)
                    sb.Append("} { ");
            }
            return sb.ToString();
        }


    }
}
