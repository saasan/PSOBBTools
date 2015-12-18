using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PSOBBTools
{
	/// <summary>
	/// FormSettings の概要の説明です。
	/// </summary>
	public class FormSettings : System.Windows.Forms.Form
	{
		internal System.Windows.Forms.PropertyGrid PropertyGridSettings;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormSettings(Settings settings)
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			PropertyGridSettings.SelectedObject = settings;
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows フォーム デザイナで生成されたコード 
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.PropertyGridSettings = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // PropertyGridSettings
            // 
            this.PropertyGridSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertyGridSettings.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.PropertyGridSettings.Location = new System.Drawing.Point(0, 0);
            this.PropertyGridSettings.Name = "PropertyGridSettings";
            this.PropertyGridSettings.Size = new System.Drawing.Size(472, 454);
            this.PropertyGridSettings.TabIndex = 0;
            // 
            // FormSettings
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
            this.ClientSize = new System.Drawing.Size(472, 454);
            this.Controls.Add(this.PropertyGridSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSettings";
            this.Text = "PSOBBTools設定";
            this.ResumeLayout(false);

		}
		#endregion
	}
}
