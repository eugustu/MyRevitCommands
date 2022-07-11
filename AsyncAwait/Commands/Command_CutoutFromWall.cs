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
    class Command_CutoutFromWall : IExternalCommand
    {
        public Document doc;
        public Frm_CutoutFromWall frm;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var start = DateTime.Now;

            UIApplication app = commandData.Application;
            UIDocument uidoc = app.ActiveUIDocument;
            doc = uidoc.Document;

            Wall wall = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfType<Wall>()
                .First();

            List<FamilyInstance> doors = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.OST_Doors)
                .OfType<FamilyInstance>()
                .ToList();

            frm = new Frm_CutoutFromWall(doc, wall, doors);
            frm.ShowDialog();

            frm.lblTime.Text = (DateTime.Now - start).TotalSeconds.ToString();

            return Result.Failed;
        }
    }
}
