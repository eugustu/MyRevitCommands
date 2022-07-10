using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Form = System.Windows.Forms.Form;

namespace AsyncAwait
{
    public partial class Frm_Ray_Results : Form
    {
        private Document _doc;
        private List<Floor> _floors;
        public Frm_Ray_Results(Document doc, List<Floor> floors)
        {
            InitializeComponent();
            _doc = doc;
            _floors = floors;
        }

        private async void btStart_Click(object sender, EventArgs e)
        {
            var values = await ObterDistancias(_floors);
            this.dgv_Items.Rows.AddRange(values);
        }

        private async Task<DataGridViewRow[]> ObterDistancias(IEnumerable<Floor> floors)
        {
            Options opt = new Options();
            XYZ rayd = new XYZ(0, 0, 1);
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
            ReferenceIntersector refI = new ReferenceIntersector(filter, FindReferenceTarget.Face, (View3D)_doc.ActiveView);

            var tasks = floors.Select(floor =>
                Task.Factory.StartNew(() =>
                {
                    BoundingBoxXYZ bb = floor.get_BoundingBox(_doc.ActiveView);
                    XYZ center = (bb.Max + bb.Min) / 2;
                    ReferenceWithContext refC = refI.FindNearest(center, rayd);
                    Reference reference = refC.GetReference();
                    DataGridViewRow row = (DataGridViewRow)dgv_Items.RowTemplate.Clone();
                    row.CreateCells(dgv_Items, floor.Id.IntegerValue.ToString(), reference.GlobalPoint.ToString());
                    return row;
                }));
            return await Task.WhenAll(tasks);
        }
    }
}
