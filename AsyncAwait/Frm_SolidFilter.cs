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
    public partial class Frm_SolidFilter : Form
    {
        private Document _doc;
        private List<Floor> _floors;
        private Floor _bulkhead;
        private DateTime _start;
        public Frm_SolidFilter(Document doc, List<Floor> floors, Floor floor)
        {
            InitializeComponent();
            _doc = doc;
            _floors = floors;
            _bulkhead = floor;
        }

        private async void btn_Parallel_Click(object sender, EventArgs e)
        {
            dgv_Items.Rows.Clear();
            _start = DateTime.Now;
            var values = await ObterCurveArraysAsync();
            this.dgv_Items.Rows.AddRange(values);
            this.lblTime.Text = (DateTime.Now - _start).TotalSeconds.ToString();
        }

        private void btn_Normal_Click(object sender, EventArgs e)
        {
            dgv_Items.Rows.Clear();
            _start = DateTime.Now;
            var values = ObterCurveArraysNormal();
            this.dgv_Items.Rows.AddRange(values);
            this.lblTime.Text = (DateTime.Now - _start).TotalSeconds.ToString();
        }

        private async Task<DataGridViewRow[]> ObterCurveArraysAsync()
        {
            Options opt = new Options();
            FilteredElementCollector collector = new FilteredElementCollector(_doc);

            var tasks = _floors.Select(floor =>
                Task.Factory.StartNew(() =>
                {
                    Solid solid = _bulkhead.get_Geometry(opt).OfType<Solid>().First();
                    ElementIntersectsSolidFilter filter = new ElementIntersectsSolidFilter(solid);
                    ElementId intersect = collector.OfCategory(BuiltInCategory.OST_Floors).WherePasses(filter).ToElementIds().First();
                    DataGridViewRow row = (DataGridViewRow)dgv_Items.RowTemplate.Clone();
                    row.CreateCells(dgv_Items, floor.Id.IntegerValue, intersect.IntegerValue);
                    Thread.Sleep(10);
                    return row;
                }));
            return await Task.WhenAll(tasks);
        }

        private DataGridViewRow[] ObterCurveArraysNormal()
        {
            Options opt = new Options();
            FilteredElementCollector collector = new FilteredElementCollector(_doc);

            var tasks = _floors.Select(floor =>
                {
                    Solid solid = _bulkhead.get_Geometry(opt).OfType<Solid>().First();
                    ElementIntersectsSolidFilter filter = new ElementIntersectsSolidFilter(solid);
                    ElementId intersect = collector.OfCategory(BuiltInCategory.OST_Floors).WherePasses(filter).ToElementIds().First();
                    DataGridViewRow row = (DataGridViewRow)dgv_Items.RowTemplate.Clone();
                    row.CreateCells(dgv_Items, floor.Id.IntegerValue, intersect.IntegerValue);
                    Thread.Sleep(10);
                    return row;
                });
            return tasks.ToArray();
        }
    }
}
