using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace MyRevitCommands
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    class Tests : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIApplication app = commandData.Application;
            UIDocument uidoc = app.ActiveUIDocument;
            Document doc = uidoc.Document;

            RevitLinkInstance rli = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.OST_RvtLinks)
                .OfType<RevitLinkInstance>()
                .Cast<RevitLinkInstance>()
                .First();

            Document docLink = rli.GetLinkDocument();

            List<Wall> walls = new FilteredElementCollector(docLink)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.OST_Walls)
                .Cast<Wall>()
                .ToList();

            List<GeometryElement> wallGeos = walls.Select(wall => wall.get_Geometry(new Options())).ToList();
            List<Solid> wallSolids = wallGeos.Select(solid => solid.OfType<Solid>().Cast<Solid>().Single()).ToList();
            Solid uniqueSolid = null;
            foreach(Solid solid in wallSolids)
            {
                if (uniqueSolid is null) uniqueSolid = solid;
                uniqueSolid = BooleanOperationsUtils
                  .ExecuteBooleanOperation(uniqueSolid, solid,
                    BooleanOperationsType.Union);
            }

            Transform Trli = rli.GetTotalTransform();
            Solid TSolid = SolidUtils.CreateTransformed(uniqueSolid, Trli);
            //CRIAR OS SOLIDOS TRANSFORMADOS E JUNTAR, DEPOIS CONTINUAR O CÓDIGO... \/
            PlanarFace inferiorFace = TSolid.Faces.Cast<PlanarFace>().Single(face => Math.Abs(face.FaceNormal.Z + 1) < 0.01);
            CurveLoop externalCurve = inferiorFace.GetEdgesAsCurveLoops().OrderByDescending(curveLoop => curveLoop.GetExactLength()).First();
            CurveLoop middleCurve = CurveLoop.CreateViaOffset(externalCurve, UnitUtils.ConvertToInternalUnits(10, UnitTypeId.Centimeters), XYZ.BasisZ);

            Level level = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.OST_Levels)
                .Cast<Level>()
                .OrderByDescending(height => height.Elevation)
                .First();

            using (Transaction createWalls = new Transaction(doc, "New Walls"))
            {
                createWalls.Start();
                middleCurve.ToList().ForEach(curve => Wall.Create(doc, curve, level.Id, false));
                createWalls.Commit();
            }
            return Result.Succeeded;
        }
    }
}
