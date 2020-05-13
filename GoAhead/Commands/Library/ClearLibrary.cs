using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;

namespace GoAhead.Commands.Library
{
    [CommandDescription(Description = "Clear the library", Wrapper = true, Publish = true)]
    class ClearLibrary : Command
    {
        protected override void DoCommandAction()
        {
            // capture current library
            foreach(LibraryElement el in Objects.Library.Instance.GetAllElements())
            {
                m_clearedElements.Add(el);
            }

            Objects.Library.Instance.Clear();
        }

        public override void Undo()
        {
            Objects.Library.Instance.Clear();
            foreach (LibraryElement el in m_clearedElements)
            {
                Objects.Library.Instance.Add(el);
            }
        }

        /// <summary>
        /// populated before undo
        /// </summary>
        private List<LibraryElement> m_clearedElements = new List<LibraryElement>();
    }
}
