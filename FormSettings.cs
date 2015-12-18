using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PSOBBTools
{
	/// <summary>
	/// FormSettings �̊T�v�̐����ł��B
	/// </summary>
	public class FormSettings : System.Windows.Forms.Form
	{
		internal System.Windows.Forms.PropertyGrid PropertyGridSettings;
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormSettings(Settings settings)
		{
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			PropertyGridSettings.SelectedObject = settings;
		}

		/// <summary>
		/// �g�p����Ă��郊�\�[�X�Ɍ㏈�������s���܂��B
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

		#region Windows �t�H�[�� �f�U�C�i�Ő������ꂽ�R�[�h 
		/// <summary>
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
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
            this.Text = "PSOBBTools�ݒ�";
            this.ResumeLayout(false);

		}
		#endregion
	}
}
