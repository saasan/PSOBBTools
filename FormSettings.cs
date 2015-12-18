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

			PropertyGridSettings.Size = this.ClientSize;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FormSettings));
			this.PropertyGridSettings = new System.Windows.Forms.PropertyGrid();
			this.SuspendLayout();
			// 
			// PropertyGridSettings
			// 
			this.PropertyGridSettings.CommandsVisibleIfAvailable = true;
			this.PropertyGridSettings.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PropertyGridSettings.LargeButtons = false;
			this.PropertyGridSettings.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.PropertyGridSettings.Location = new System.Drawing.Point(0, 0);
			this.PropertyGridSettings.Name = "PropertyGridSettings";
			this.PropertyGridSettings.Size = new System.Drawing.Size(352, 374);
			this.PropertyGridSettings.TabIndex = 0;
			this.PropertyGridSettings.Text = "PropertyGrid";
			this.PropertyGridSettings.ViewBackColor = System.Drawing.SystemColors.Window;
			this.PropertyGridSettings.ViewForeColor = System.Drawing.SystemColors.WindowText;
			// 
			// FormSettings
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(352, 374);
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
