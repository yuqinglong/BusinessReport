namespace BusinessReport
{
    partial class frmHuatai
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHuatai));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnSee = new System.Windows.Forms.Button();
            this.dtEndDate = new System.Windows.Forms.DateTimePicker();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.dtStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.dgvShow = new System.Windows.Forms.DataGridView();
            this.pgbShow = new System.Windows.Forms.ProgressBar();
            this.日期 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AIA000705 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AIA000904 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AIA001101 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AIA001401 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AIA001001 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AIA001002 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AIA001003 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AIA001004 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AIA001005 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AIA000603 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AIA000709 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AIA000902 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AIA010901 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AIA000905 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AIA001006 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AIA001102 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BKA001003 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BKA001004 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BKA001101 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BKA001102 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BKA001105 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BKA051101 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BKA051401 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BKA001401 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BKA001005 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AIA001201 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AIA001301 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AIA001307 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AIA001204 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AIA000706 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AIA000604 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.总计 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvShow)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnExport);
            this.groupBox1.Controls.Add(this.btnSee);
            this.groupBox1.Controls.Add(this.dtEndDate);
            this.groupBox1.Controls.Add(this.lblEndDate);
            this.groupBox1.Controls.Add(this.dtStartDate);
            this.groupBox1.Controls.Add(this.lblStartDate);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1185, 49);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择日期";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(483, 17);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(77, 23);
            this.btnExport.TabIndex = 6;
            this.btnExport.Text = "导出Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnSee
            // 
            this.btnSee.Location = new System.Drawing.Point(400, 17);
            this.btnSee.Name = "btnSee";
            this.btnSee.Size = new System.Drawing.Size(77, 23);
            this.btnSee.TabIndex = 5;
            this.btnSee.Text = "查询";
            this.btnSee.UseVisualStyleBackColor = true;
            this.btnSee.Click += new System.EventHandler(this.btnSee_Click);
            // 
            // dtEndDate
            // 
            this.dtEndDate.Location = new System.Drawing.Point(279, 18);
            this.dtEndDate.Name = "dtEndDate";
            this.dtEndDate.Size = new System.Drawing.Size(105, 21);
            this.dtEndDate.TabIndex = 3;
            // 
            // lblEndDate
            // 
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Location = new System.Drawing.Point(207, 22);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(65, 12);
            this.lblEndDate.TabIndex = 2;
            this.lblEndDate.Text = "结束日期：";
            // 
            // dtStartDate
            // 
            this.dtStartDate.Checked = false;
            this.dtStartDate.Location = new System.Drawing.Point(79, 18);
            this.dtStartDate.Name = "dtStartDate";
            this.dtStartDate.Size = new System.Drawing.Size(115, 21);
            this.dtStartDate.TabIndex = 0;
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Location = new System.Drawing.Point(8, 22);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(65, 12);
            this.lblStartDate.TabIndex = 1;
            this.lblStartDate.Text = "开始日期：";
            // 
            // dgvShow
            // 
            this.dgvShow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvShow.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvShow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvShow.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.日期,
            this.AIA000705,
            this.AIA000904,
            this.AIA001101,
            this.AIA001401,
            this.AIA001001,
            this.AIA001002,
            this.AIA001003,
            this.AIA001004,
            this.AIA001005,
            this.AIA000603,
            this.AIA000709,
            this.AIA000902,
            this.AIA010901,
            this.AIA000905,
            this.AIA001006,
            this.AIA001102,
            this.BKA001003,
            this.BKA001004,
            this.BKA001101,
            this.BKA001102,
            this.BKA001105,
            this.BKA051101,
            this.BKA051401,
            this.BKA001401,
            this.BKA001005,
            this.AIA001201,
            this.AIA001301,
            this.AIA001307,
            this.AIA001204,
            this.AIA000706,
            this.AIA000604,
            this.总计});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvShow.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvShow.Location = new System.Drawing.Point(3, 53);
            this.dgvShow.Name = "dgvShow";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvShow.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvShow.RowHeadersWidth = 50;
            this.dgvShow.RowTemplate.Height = 23;
            this.dgvShow.Size = new System.Drawing.Size(1179, 325);
            this.dgvShow.TabIndex = 5;
            // 
            // pgbShow
            // 
            this.pgbShow.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pgbShow.Location = new System.Drawing.Point(0, 360);
            this.pgbShow.Name = "pgbShow";
            this.pgbShow.Size = new System.Drawing.Size(1185, 23);
            this.pgbShow.TabIndex = 6;
            // 
            // 日期
            // 
            this.日期.DataPropertyName = "DownLoadDay";
            this.日期.HeaderText = "日期";
            this.日期.Name = "日期";
            // 
            // AIA000705
            // 
            this.AIA000705.DataPropertyName = "mainType705Count";
            this.AIA000705.HeaderText = "AIA000705";
            this.AIA000705.Name = "AIA000705";
            // 
            // AIA000904
            // 
            this.AIA000904.DataPropertyName = "mainType904Count";
            this.AIA000904.HeaderText = "AIA000904";
            this.AIA000904.Name = "AIA000904";
            // 
            // AIA001101
            // 
            this.AIA001101.DataPropertyName = "mainType1101Count";
            this.AIA001101.HeaderText = "AIA001101";
            this.AIA001101.Name = "AIA001101";
            // 
            // AIA001401
            // 
            this.AIA001401.DataPropertyName = "mainType1401Count";
            this.AIA001401.HeaderText = "AIA001401";
            this.AIA001401.Name = "AIA001401";
            // 
            // AIA001001
            // 
            this.AIA001001.DataPropertyName = "aipType1001Count";
            this.AIA001001.HeaderText = "AIA001001";
            this.AIA001001.Name = "AIA001001";
            // 
            // AIA001002
            // 
            this.AIA001002.DataPropertyName = "aipType1002Count";
            this.AIA001002.HeaderText = "AIA001002";
            this.AIA001002.Name = "AIA001002";
            // 
            // AIA001003
            // 
            this.AIA001003.DataPropertyName = "aipType1003Count";
            this.AIA001003.HeaderText = "AIA001003";
            this.AIA001003.Name = "AIA001003";
            // 
            // AIA001004
            // 
            this.AIA001004.DataPropertyName = "aipType1004Count";
            this.AIA001004.HeaderText = "AIA001004";
            this.AIA001004.Name = "AIA001004";
            // 
            // AIA001005
            // 
            this.AIA001005.DataPropertyName = "aipType1005Count";
            this.AIA001005.HeaderText = "AIA001005";
            this.AIA001005.Name = "AIA001005";
            // 
            // AIA000603
            // 
            this.AIA000603.DataPropertyName = "chargeType603Count";
            this.AIA000603.HeaderText = "811";
            this.AIA000603.Name = "AIA000603";
            // 
            // AIA000709
            // 
            this.AIA000709.DataPropertyName = "applyType709Count";
            this.AIA000709.HeaderText = "818";
            this.AIA000709.Name = "AIA000709";
            // 
            // AIA000902
            // 
            this.AIA000902.DataPropertyName = "applyType902Count";
            this.AIA000902.HeaderText = "822";
            this.AIA000902.Name = "AIA000902";
            // 
            // AIA010901
            // 
            this.AIA010901.DataPropertyName = "riskType901Count";
            this.AIA010901.HeaderText = "813";
            this.AIA010901.Name = "AIA010901";
            // 
            // AIA000905
            // 
            this.AIA000905.DataPropertyName = "tbqrsType905Count";
            this.AIA000905.HeaderText = "823";
            this.AIA000905.Name = "AIA000905";
            // 
            // AIA001006
            // 
            this.AIA001006.DataPropertyName = "familyType1006Count";
            this.AIA001006.HeaderText = "AIA001006";
            this.AIA001006.Name = "AIA001006";
            // 
            // AIA001102
            // 
            this.AIA001102.DataPropertyName = "familyType1102Count";
            this.AIA001102.HeaderText = "AIA001102";
            this.AIA001102.Name = "AIA001102";
            // 
            // BKA001003
            // 
            this.BKA001003.DataPropertyName = "bkaType1003Count";
            this.BKA001003.HeaderText = "BKA001003";
            this.BKA001003.Name = "BKA001003";
            // 
            // BKA001004
            // 
            this.BKA001004.DataPropertyName = "bkaType1004Count";
            this.BKA001004.HeaderText = "BKA001004";
            this.BKA001004.Name = "BKA001004";
            // 
            // BKA001101
            // 
            this.BKA001101.DataPropertyName = "bkaType1101Count";
            this.BKA001101.HeaderText = "BKA001101";
            this.BKA001101.Name = "BKA001101";
            // 
            // BKA001102
            // 
            this.BKA001102.DataPropertyName = "bkaType1102Count";
            this.BKA001102.HeaderText = "BKA001102";
            this.BKA001102.Name = "BKA001102";
            // 
            // BKA001105
            // 
            this.BKA001105.DataPropertyName = "bkaType1105Count";
            this.BKA001105.HeaderText = "BKA001105";
            this.BKA001105.Name = "BKA001105";
            // 
            // BKA051101
            // 
            this.BKA051101.DataPropertyName = "bkaType51101Count";
            this.BKA051101.HeaderText = "BKA051101";
            this.BKA051101.Name = "BKA051101";
            // 
            // BKA051401
            // 
            this.BKA051401.DataPropertyName = "bkaType51401Count";
            this.BKA051401.HeaderText = "BKA051401";
            this.BKA051401.Name = "BKA051401";
            // 
            // BKA001401
            // 
            this.BKA001401.DataPropertyName = "bkaType1401Count";
            this.BKA001401.HeaderText = "BKA001401";
            this.BKA001401.Name = "BKA001401";
            // 
            // BKA001005
            // 
            this.BKA001005.DataPropertyName = "bkaType1005Count";
            this.BKA001005.HeaderText = "BKA001005";
            this.BKA001005.Name = "BKA001005";
            // 
            // AIA001201
            // 
            this.AIA001201.DataPropertyName = "familyType1201Count";
            this.AIA001201.HeaderText = "AIA001201";
            this.AIA001201.Name = "AIA001201";
            // 
            // AIA001301
            // 
            this.AIA001301.DataPropertyName = "familyType1301Count";
            this.AIA001301.HeaderText = "AIA001301";
            this.AIA001301.Name = "AIA001301";
            // 
            // AIA001307
            // 
            this.AIA001307.DataPropertyName = "familyType1307Count";
            this.AIA001307.HeaderText = "AIA001307";
            this.AIA001307.Name = "AIA001307";
            // 
            // AIA001204
            // 
            this.AIA001204.DataPropertyName = "applyType204Count";
            this.AIA001204.HeaderText = "838";
            this.AIA001204.Name = "AIA001204";
            // 
            // AIA000706
            // 
            this.AIA000706.DataPropertyName = "familyType0706Count";
            this.AIA000706.HeaderText = "819";
            this.AIA000706.Name = "AIA000706";
            // 
            // AIA000604
            // 
            this.AIA000604.DataPropertyName = "familyType0604Count";
            this.AIA000604.HeaderText = "810";
            this.AIA000604.Name = "AIA000604";
            // 
            // 总计
            // 
            this.总计.DataPropertyName = "Total";
            this.总计.HeaderText = "总计";
            this.总计.Name = "总计";
            // 
            // frmHuatai
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1185, 383);
            this.Controls.Add(this.pgbShow);
            this.Controls.Add(this.dgvShow);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmHuatai";
            this.Text = "华泰";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvShow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnSee;
        private System.Windows.Forms.DateTimePicker dtEndDate;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.DateTimePicker dtStartDate;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.DataGridView dgvShow;
        private System.Windows.Forms.ProgressBar pgbShow;
        private System.Windows.Forms.DataGridViewTextBoxColumn 日期;
        private System.Windows.Forms.DataGridViewTextBoxColumn AIA000705;
        private System.Windows.Forms.DataGridViewTextBoxColumn AIA000904;
        private System.Windows.Forms.DataGridViewTextBoxColumn AIA001101;
        private System.Windows.Forms.DataGridViewTextBoxColumn AIA001401;
        private System.Windows.Forms.DataGridViewTextBoxColumn AIA001001;
        private System.Windows.Forms.DataGridViewTextBoxColumn AIA001002;
        private System.Windows.Forms.DataGridViewTextBoxColumn AIA001003;
        private System.Windows.Forms.DataGridViewTextBoxColumn AIA001004;
        private System.Windows.Forms.DataGridViewTextBoxColumn AIA001005;
        private System.Windows.Forms.DataGridViewTextBoxColumn AIA000603;
        private System.Windows.Forms.DataGridViewTextBoxColumn AIA000709;
        private System.Windows.Forms.DataGridViewTextBoxColumn AIA000902;
        private System.Windows.Forms.DataGridViewTextBoxColumn AIA010901;
        private System.Windows.Forms.DataGridViewTextBoxColumn AIA000905;
        private System.Windows.Forms.DataGridViewTextBoxColumn AIA001006;
        private System.Windows.Forms.DataGridViewTextBoxColumn AIA001102;
        private System.Windows.Forms.DataGridViewTextBoxColumn BKA001003;
        private System.Windows.Forms.DataGridViewTextBoxColumn BKA001004;
        private System.Windows.Forms.DataGridViewTextBoxColumn BKA001101;
        private System.Windows.Forms.DataGridViewTextBoxColumn BKA001102;
        private System.Windows.Forms.DataGridViewTextBoxColumn BKA001105;
        private System.Windows.Forms.DataGridViewTextBoxColumn BKA051101;
        private System.Windows.Forms.DataGridViewTextBoxColumn BKA051401;
        private System.Windows.Forms.DataGridViewTextBoxColumn BKA001401;
        private System.Windows.Forms.DataGridViewTextBoxColumn BKA001005;
        private System.Windows.Forms.DataGridViewTextBoxColumn AIA001201;
        private System.Windows.Forms.DataGridViewTextBoxColumn AIA001301;
        private System.Windows.Forms.DataGridViewTextBoxColumn AIA001307;
        private System.Windows.Forms.DataGridViewTextBoxColumn AIA001204;
        private System.Windows.Forms.DataGridViewTextBoxColumn AIA000706;
        private System.Windows.Forms.DataGridViewTextBoxColumn AIA000604;
        private System.Windows.Forms.DataGridViewTextBoxColumn 总计;
    }
}