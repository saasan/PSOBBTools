namespace PSOBBTools
{
    partial class FormWindowResize
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWindowResize));
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.radioSize640 = new System.Windows.Forms.RadioButton();
            this.radioSize1024 = new System.Windows.Forms.RadioButton();
            this.radioSize = new System.Windows.Forms.RadioButton();
            this.labelWidth = new System.Windows.Forms.Label();
            this.labelHeight = new System.Windows.Forms.Label();
            this.upDownWidth = new System.Windows.Forms.NumericUpDown();
            this.upDownHeight = new System.Windows.Forms.NumericUpDown();
            this.radioSize800 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.upDownWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDownHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(130, 131);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(211, 131);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "キャンセル";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // radioSize640
            // 
            this.radioSize640.AutoSize = true;
            this.radioSize640.Location = new System.Drawing.Point(12, 12);
            this.radioSize640.Name = "radioSize640";
            this.radioSize640.Size = new System.Drawing.Size(71, 16);
            this.radioSize640.TabIndex = 0;
            this.radioSize640.Text = "&640×480";
            this.radioSize640.UseVisualStyleBackColor = true;
            this.radioSize640.CheckedChanged += new System.EventHandler(this.radioSize_CheckedChanged);
            // 
            // radioSize1024
            // 
            this.radioSize1024.AutoSize = true;
            this.radioSize1024.Location = new System.Drawing.Point(12, 56);
            this.radioSize1024.Name = "radioSize1024";
            this.radioSize1024.Size = new System.Drawing.Size(77, 16);
            this.radioSize1024.TabIndex = 1;
            this.radioSize1024.Text = "&1024×768";
            this.radioSize1024.UseVisualStyleBackColor = true;
            this.radioSize1024.CheckedChanged += new System.EventHandler(this.radioSize_CheckedChanged);
            // 
            // radioSize
            // 
            this.radioSize.AutoSize = true;
            this.radioSize.Checked = true;
            this.radioSize.Location = new System.Drawing.Point(12, 78);
            this.radioSize.Name = "radioSize";
            this.radioSize.Size = new System.Drawing.Size(91, 16);
            this.radioSize.TabIndex = 2;
            this.radioSize.TabStop = true;
            this.radioSize.Text = "指定サイズ(&S)";
            this.radioSize.UseVisualStyleBackColor = true;
            this.radioSize.CheckedChanged += new System.EventHandler(this.radioSize_CheckedChanged);
            // 
            // labelWidth
            // 
            this.labelWidth.AutoSize = true;
            this.labelWidth.Location = new System.Drawing.Point(30, 106);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(40, 12);
            this.labelWidth.TabIndex = 3;
            this.labelWidth.Text = "幅(&W) :";
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.Location = new System.Drawing.Point(161, 106);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(47, 12);
            this.labelHeight.TabIndex = 5;
            this.labelHeight.Text = "高さ(&H) :";
            // 
            // upDownWidth
            // 
            this.upDownWidth.Location = new System.Drawing.Point(83, 103);
            this.upDownWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.upDownWidth.Minimum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.upDownWidth.Name = "upDownWidth";
            this.upDownWidth.Size = new System.Drawing.Size(65, 19);
            this.upDownWidth.TabIndex = 4;
            this.upDownWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.upDownWidth.Value = new decimal(new int[] {
            640,
            0,
            0,
            0});
            this.upDownWidth.Enter += new System.EventHandler(this.upDown_Enter);
            // 
            // upDownHeight
            // 
            this.upDownHeight.Location = new System.Drawing.Point(221, 103);
            this.upDownHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.upDownHeight.Minimum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.upDownHeight.Name = "upDownHeight";
            this.upDownHeight.Size = new System.Drawing.Size(65, 19);
            this.upDownHeight.TabIndex = 6;
            this.upDownHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.upDownHeight.Value = new decimal(new int[] {
            480,
            0,
            0,
            0});
            this.upDownHeight.Enter += new System.EventHandler(this.upDown_Enter);
            // 
            // radioSize800
            // 
            this.radioSize800.AutoSize = true;
            this.radioSize800.Location = new System.Drawing.Point(12, 34);
            this.radioSize800.Name = "radioSize800";
            this.radioSize800.Size = new System.Drawing.Size(71, 16);
            this.radioSize800.TabIndex = 1;
            this.radioSize800.Text = "&800×600";
            this.radioSize800.UseVisualStyleBackColor = true;
            this.radioSize800.CheckedChanged += new System.EventHandler(this.radioSize_CheckedChanged);
            // 
            // FormWindowResize
            // 
            this.AcceptButton = this.buttonOK;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(302, 168);
            this.Controls.Add(this.radioSize);
            this.Controls.Add(this.radioSize800);
            this.Controls.Add(this.radioSize1024);
            this.Controls.Add(this.radioSize640);
            this.Controls.Add(this.upDownHeight);
            this.Controls.Add(this.upDownWidth);
            this.Controls.Add(this.labelHeight);
            this.Controls.Add(this.labelWidth);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormWindowResize";
            this.Text = "ウィンドウサイズの変更";
            ((System.ComponentModel.ISupportInitialize)(this.upDownWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDownHeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.RadioButton radioSize640;
        private System.Windows.Forms.RadioButton radioSize1024;
        private System.Windows.Forms.RadioButton radioSize;
        private System.Windows.Forms.Label labelWidth;
        private System.Windows.Forms.Label labelHeight;
        private System.Windows.Forms.NumericUpDown upDownWidth;
        private System.Windows.Forms.NumericUpDown upDownHeight;
        private System.Windows.Forms.RadioButton radioSize800;
    }
}