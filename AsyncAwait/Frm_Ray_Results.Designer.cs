
namespace AsyncAwait
{
    partial class Frm_Ray_Results
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgv_Items = new System.Windows.Forms.DataGridView();
            this.lblTime = new System.Windows.Forms.Label();
            this.clm_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clm_Delta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btStart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Items)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_Items
            // 
            this.dgv_Items.AllowUserToAddRows = false;
            this.dgv_Items.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_Items.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Items.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clm_Id,
            this.clm_Delta});
            this.dgv_Items.Location = new System.Drawing.Point(12, 12);
            this.dgv_Items.Name = "dgv_Items";
            this.dgv_Items.RowHeadersWidth = 51;
            this.dgv_Items.RowTemplate.Height = 24;
            this.dgv_Items.Size = new System.Drawing.Size(494, 426);
            this.dgv_Items.TabIndex = 1;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(13, 445);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(46, 17);
            this.lblTime.TabIndex = 2;
            this.lblTime.Text = "label1";
            // 
            // clm_Id
            // 
            this.clm_Id.FillWeight = 35F;
            this.clm_Id.HeaderText = "Ids";
            this.clm_Id.MinimumWidth = 6;
            this.clm_Id.Name = "clm_Id";
            this.clm_Id.ReadOnly = true;
            // 
            // clm_Delta
            // 
            this.clm_Delta.HeaderText = "Delta";
            this.clm_Delta.MinimumWidth = 6;
            this.clm_Delta.Name = "clm_Delta";
            this.clm_Delta.ReadOnly = true;
            // 
            // btStart
            // 
            this.btStart.Location = new System.Drawing.Point(381, 445);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(124, 72);
            this.btStart.TabIndex = 3;
            this.btStart.Text = "Analisar";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // Frm_Results
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 529);
            this.Controls.Add(this.btStart);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.dgv_Items);
            this.Name = "Frm_Results";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Items)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.DataGridView dgv_Items;
        public System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn clm_Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn clm_Delta;
        private System.Windows.Forms.Button btStart;
    }
}