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
using static AsyncAwait.Utils;
using System.Threading;

namespace AsyncAwait
{
    public partial class Frm_CutoutFromWall : Form
    {
        private Document _doc;
        private Wall _wall;
        private List<FamilyInstance> _fis;
        private DateTime _start;

        public Frm_CutoutFromWall(Document doc, Wall wall, List<FamilyInstance> fis)
        {
            InitializeComponent();
            _doc = doc;
            _wall = wall;
            _fis = fis;
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
            var tasks = _fis.Select(fi =>
                Task.Factory.StartNew(() =>
                {
                    CurveLoop cl = CurveLoopFromWall(_doc, _wall, fi);
                    DataGridViewRow row = (DataGridViewRow)dgv_Items.RowTemplate.Clone();
                    row.CreateCells(dgv_Items, fi.Id.IntegerValue, cl.GetExactLength().ToString());
                    Thread.Sleep(10);
                    return row;
                }));
            return await Task.WhenAll(tasks);
        }

        private DataGridViewRow[] ObterCurveArraysNormal()
        {
            var tasks = _fis.Select(fi =>
                {
                    CurveLoop cl = CurveLoopFromWall(_doc, _wall, fi);
                    DataGridViewRow row = (DataGridViewRow)dgv_Items.RowTemplate.Clone();
                    row.CreateCells(dgv_Items, fi.Id.IntegerValue, cl.GetExactLength().ToString());
                    Thread.Sleep(10);
                    return row;
                });
            return tasks.ToArray();
        }
    }
}
