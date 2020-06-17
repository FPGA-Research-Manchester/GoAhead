using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
namespace GoAhead.Commands.Misc
{
    public class PermuteUtils
    {
        // Returns an enumeration of enumerators, one for each permutation
        // of the input.
        public static IEnumerable<IEnumerable<T>> Permute<T>(IEnumerable<T> list, int count)
        {
            if (count == 0)
            {
                yield return new T[0];
            }
            else
            {
                int startingElementIndex = 0;
                foreach (T startingElement in list)
                {
                    IEnumerable<T> remainingItems = AllExcept(list, startingElementIndex);

                    foreach (IEnumerable<T> permutationOfRemainder in Permute(remainingItems, count - 1))
                    {
                        yield return Concat<T>(
                            new T[] { startingElement },
                            permutationOfRemainder);
                    }
                    startingElementIndex += 1;
                }
            }
        }

        // Enumerates over contents of both lists.
        public static IEnumerable<T> Concat<T>(IEnumerable<T> a, IEnumerable<T> b)
        {
            foreach (T item in a) { yield return item; }
            foreach (T item in b) { yield return item; }
        }

        // Enumerates over all items in the input, skipping over the item
        // with the specified offset.
        public static IEnumerable<T> AllExcept<T>(IEnumerable<T> input, int indexToSkip)
        {
            int index = 0;
            foreach (T item in input)
            {
                if (index != indexToSkip) yield return item;
                index += 1;
            }
        }
    }

    public class SubSetUtils
    {
        public static IEnumerable<List<T>> SubSets<T>(List<T> source)
        {
            for (int i = 0; i < Math.Pow(2, source.Count); i++)
            {
                List<T> combination = new List<T>();

                for (int j = 0; j < source.Count; j++)
                {
                    if ((i & (1 << (source.Count - j - 1))) != 0)
                    {
                        combination.Add(source[j]);
                    }
                }
                yield return combination;
            }
        }
    }

    class FindPortMapping : Command
    {       
        protected override void DoCommandAction()
        {
            foreach (List<Port> subset in SubSetUtils.SubSets<Port>(this.m_from))
            {
                if (subset.Count == this.m_to.Count)
                {
                    ShowPermutations<Port>(subset, subset.Count, this.m_to, subset.Count);
                }
            }
        }

        private void ShowPermutations<T>(IEnumerable<T> from, int fromCount, IEnumerable<T> to, int toCount)
        {
            int tries = 0;
            foreach (IEnumerable<T> fromPermutation in PermuteUtils.Permute<T>(from, fromCount))
            {
                foreach (IEnumerable<T> toPermutation in PermuteUtils.Permute<T>(to, toCount))
                {
                    tries++;
                    List<T> currentFromPermuation = new List<T>();
                    List<T> currentToPermuation = new List<T>();
                    foreach (T i in fromPermutation)
                    {
                        currentFromPermuation.Add(i);
                    }

                    foreach (T i in toPermutation)
                    {
                        currentToPermuation.Add(i);
                    }

                    bool hit = true;
                    StringBuilder buffer = new StringBuilder();
                    for (int i = 0; i < currentFromPermuation.Count; i++)
                    {
                        String fromStr = currentFromPermuation[i].ToString();
                        String toStr = currentToPermuation[i].ToString();
                        buffer.AppendLine(fromStr + "->" + toStr);
                    }
                    if (hit)
                    {
                        this.OutputManager.WriteOutput(buffer.ToString());
                        this.OutputManager.WriteOutput("----------------");
                        return;
                    }
                }
            }
        }
        
        public override void Undo()
        {
            throw new NotImplementedException();
        }
        private readonly List<Port> m_from = new List<Port>();
        private readonly List<Port> m_to = new List<Port>();
    }
}
