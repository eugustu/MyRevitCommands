using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.IFC;

namespace AsyncAwait
{
    public static class Utils
    {
        public static CurveLoop CurveLoopFromWall(Document doc, Wall wall, FamilyInstance familyInstance)
        {
            XYZ dir = familyInstance.FacingOrientation;
            return ExporterIFCUtils.GetInstanceCutoutFromWall(doc, wall, familyInstance, out dir);
        }
    }
}
