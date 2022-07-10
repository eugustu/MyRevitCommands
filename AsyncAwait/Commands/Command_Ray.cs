using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace AsyncAwait
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    class Command_Ray : IExternalCommand
    {
        public Document doc;
        public Frm_Ray_Results frm;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var start = DateTime.Now;

            UIApplication app = commandData.Application;
            UIDocument uidoc = app.ActiveUIDocument;
            doc = uidoc.Document;

            List<Floor> rayFloors = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfType<Floor>()
                .ToList();

            Floor bulkheadFloor = rayFloors
                .First(floor => floor.Name == "Concreto moldado em loco 225 mm");

            rayFloors = rayFloors
                .Except(new Floor[] { bulkheadFloor }).ToList();

            frm = new Frm_Ray_Results(doc, rayFloors);
            frm.ShowDialog();

            frm.lblTime.Text = (DateTime.Now - start).TotalSeconds.ToString();

            return Result.Failed;
        }
    }
}
