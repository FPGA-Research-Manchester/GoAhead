using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.LibraryElementInstantiationManager
{
    [CommandDescription(Description = "Print all tiles in the current selection in which the given libary element can be placed")]
    class PrintPossibleLibraryElementPlacementsInSelection : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            LibraryElement libEl = Objects.Library.Instance.GetElement(LibraryElementName);
            List<Tile> anchors = new List<Tile>();

            foreach (Tile placePos in TileSelectionManager.Instance.GetSelectedTiles().Where(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB)))
            {
                StringBuilder errorList = null;
                bool placementOk = DesignRuleChecker.CheckLibraryElementPlacement(placePos, libEl, out errorList);
                if (placementOk)
                {
                    anchors.Add(placePos);
                }
            }

            foreach (Tile t in anchors)
            {
                OutputManager.WriteOutput("Libary element " + libEl.Name + " can be placed at " + t.Location);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the library element, e.g. BM_S6_L4_R4_double")]
        public string LibraryElementName = "BM_S6_L4_R4_double";
    }
}
