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
    class Command_PointPlaneDelta : IExternalCommand
    {
        public Document doc;
        public Frm_PointPlaneDelta frm;
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
            Solid geomElem = bulkheadFloor.get_Geometry(new Options()).OfType<Solid>().First();
            Plane plane = Plane.CreateByNormalAndOrigin(XYZ.BasisZ, geomElem.ComputeCentroid());

            rayFloors = rayFloors.Except(new Floor[] { bulkheadFloor }).ToList();

            frm = new Frm_PointPlaneDelta(doc, rayFloors, plane);
            frm.ShowDialog();

            return Result.Failed;
        }
    }
}
