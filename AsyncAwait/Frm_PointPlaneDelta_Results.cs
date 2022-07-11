using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Form = System.Windows.Forms.Form;

namespace AsyncAwait
{
    public partial class Frm_PointPlaneDelta_Results : Form
    {
        private Document _doc;
        private List<Floor> _floors;
        private Plane _plane;
        private DateTime _start;
        public Frm_PointPlaneDelta_Results(Document doc, List<Floor> floors, Plane plane)
        {
            InitializeComponent();
            _doc = doc;
            _floors = floors;
            _plane = plane;
        }

        private async void btParallel_Click(object sender, EventArgs e)
        {
            _start = DateTime.Now;
            var values = await ObterDistanciasAsync();
            this.dgv_Items.Rows.AddRange(values);
            this.lblTime.Text = (DateTime.Now - _start).TotalSeconds.ToString();
        }

        private async Task<DataGridViewRow[]> ObterDistanciasAsync()
        {
            XYZ rayd = new XYZ(0, 0, 1);
            Options opt = new Options();

            var tasks = _floors.Select(floor =>
                Task.Factory.StartNew(() =>
                {
                    GeometryElement geoEle = floor.get_Geometry(opt);
                    BoundingBoxXYZ bb = floor.get_BoundingBox(_doc.ActiveView);
                    XYZ center = (bb.Max + bb.Min) / 2;
                    double dist = _plane.SignedDistanceTo(center);
                    DataGridViewRow row = (DataGridViewRow)dgv_Items.RowTemplate.Clone();
                    row.CreateCells(dgv_Items, floor.Id.IntegerValue.ToString(), dist);
                    Thread.Sleep(10);
                    return row;
                }));
            return await Task.WhenAll(tasks);
        }

        private void btn_Normal_Click(object sender, EventArgs e)
        {
            _start = DateTime.Now;
            var values = ObterDistanciasNormal();
            this.dgv_Items.Rows.AddRange(values);
            this.lblTime.Text = (DateTime.Now - _start).TotalSeconds.ToString();
        }

        private DataGridViewRow[] ObterDistanciasNormal()
        {
            XYZ rayd = new XYZ(0, 0, 1);
            Options opt = new Options();

            var rows = _floors.Select(floor =>
                {
                    GeometryElement geoEle = floor.get_Geometry(opt);
                    BoundingBoxXYZ bb = floor.get_BoundingBox(_doc.ActiveView);
                    XYZ center = (bb.Max + bb.Min) / 2;
                    double dist = _plane.SignedDistanceTo(center);
                    DataGridViewRow row = (DataGridViewRow)dgv_Items.RowTemplate.Clone();
                    row.CreateCells(dgv_Items, floor.Id.IntegerValue.ToString(), dist);
                    Thread.Sleep(10);
                    return row;
                }).ToArray();

            return rows;
        }
    }
}
