using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PSOBBTools
{
	/// <summary>
	/// FormMagTimer �̊T�v�̐����ł��B
	/// </summary>
	public class FormMagTimer : System.Windows.Forms.Form
	{
		private const string FORM_TEXT = "MagTimer";
		private const string BUTTON_TEXT_START = "�X�^�[�g(&S)";
		private const string BUTTON_TEXT_STOP = "�X�g�b�v(&S)";

		private long timerStart;
		private long timerEnd;
		private Settings settings;
		
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ProgressBar progressMagTimer;
		private System.Windows.Forms.Button buttonStart;
		private System.Windows.Forms.Timer timerMagTimer;
		private System.Windows.Forms.Label labelTime;
		private System.Windows.Forms.NumericUpDown upDownTime;
		private System.Windows.Forms.Label labelSeconds;
		private System.Windows.Forms.CheckBox checkReload;

		public FormMagTimer(Settings settings)
		{
			InitializeComponent();

			this.settings = settings;
			upDownTime.Value = (decimal)settings.MagTimerTime;
			checkReload.Checked = settings.MagTimerReload;

			this.Text = FORM_TEXT;
			buttonStart.Text = BUTTON_TEXT_START;
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FormMagTimer));
			this.progressMagTimer = new System.Windows.Forms.ProgressBar();
			this.buttonStart = new System.Windows.Forms.Button();
			this.timerMagTimer = new System.Windows.Forms.Timer(this.components);
			this.checkReload = new System.Windows.Forms.CheckBox();
			this.upDownTime = new System.Windows.Forms.NumericUpDown();
			this.labelTime = new System.Windows.Forms.Label();
			this.labelSeconds = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.upDownTime)).BeginInit();
			this.SuspendLayout();
			// 
			// progressMagTimer
			// 
			this.progressMagTimer.Location = new System.Drawing.Point(8, 8);
			this.progressMagTimer.Name = "progressMagTimer";
			this.progressMagTimer.Size = new System.Drawing.Size(240, 24);
			this.progressMagTimer.TabIndex = 0;
			// 
			// buttonStart
			// 
			this.buttonStart.Location = new System.Drawing.Point(256, 8);
			this.buttonStart.Name = "buttonStart";
			this.buttonStart.Size = new System.Drawing.Size(88, 24);
			this.buttonStart.TabIndex = 1;
			this.buttonStart.Text = "�X�^�[�g(&S)";
			this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
			// 
			// timerMagTimer
			// 
			this.timerMagTimer.Tick += new System.EventHandler(this.timerMagTimer_Tick);
			// 
			// checkReload
			// 
			this.checkReload.Location = new System.Drawing.Point(136, 40);
			this.checkReload.Name = "checkReload";
			this.checkReload.Size = new System.Drawing.Size(72, 16);
			this.checkReload.TabIndex = 5;
			this.checkReload.Text = "�J��Ԃ�";
			this.checkReload.CheckedChanged += new System.EventHandler(this.checkReload_CheckedChanged);
			// 
			// upDownTime
			// 
			this.upDownTime.Location = new System.Drawing.Point(48, 40);
			this.upDownTime.Maximum = new System.Decimal(new int[] {
																	   500,
																	   0,
																	   0,
																	   0});
			this.upDownTime.Minimum = new System.Decimal(new int[] {
																	   1,
																	   0,
																	   0,
																	   0});
			this.upDownTime.Name = "upDownTime";
			this.upDownTime.ReadOnly = true;
			this.upDownTime.Size = new System.Drawing.Size(48, 19);
			this.upDownTime.TabIndex = 3;
			this.upDownTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.upDownTime.Value = new System.Decimal(new int[] {
																	 1,
																	 0,
																	 0,
																	 0});
			this.upDownTime.ValueChanged += new System.EventHandler(this.upDownTime_ValueChanged);
			// 
			// labelTime
			// 
			this.labelTime.Location = new System.Drawing.Point(8, 40);
			this.labelTime.Name = "labelTime";
			this.labelTime.Size = new System.Drawing.Size(40, 16);
			this.labelTime.TabIndex = 2;
			this.labelTime.Text = "���ԁF";
			this.labelTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelSeconds
			// 
			this.labelSeconds.Location = new System.Drawing.Point(104, 40);
			this.labelSeconds.Name = "labelSeconds";
			this.labelSeconds.Size = new System.Drawing.Size(24, 16);
			this.labelSeconds.TabIndex = 4;
			this.labelSeconds.Text = "�b";
			this.labelSeconds.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FormMagTimer
			// 
			this.AcceptButton = this.buttonStart;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(354, 66);
			this.Controls.Add(this.labelSeconds);
			this.Controls.Add(this.labelTime);
			this.Controls.Add(this.upDownTime);
			this.Controls.Add(this.checkReload);
			this.Controls.Add(this.buttonStart);
			this.Controls.Add(this.progressMagTimer);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "FormMagTimer";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "MagTimer";
			((System.ComponentModel.ISupportInitialize)(this.upDownTime)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonStart_Click(object sender, System.EventArgs e)
		{
			if (timerMagTimer.Enabled)
			{
				MagTimerStop();
			}
			else
			{
				MagTimerStart();
			}
		}

		private void timerMagTimer_Tick(object sender, System.EventArgs e)
		{
			long now = DateTime.Now.Ticks;

			now -= timerStart;

			if (now > timerEnd)
			{
				MagTimerStop();

				// �`���C��
				if (System.IO.File.Exists(settings.MagTimerFile))
				{
					Sound.Play(settings.MagTimerFile);
				}

				if (settings.MagTimerReload)
				{
					MagTimerStart();
				}
			}
			else
			{
				progressMagTimer.Value = (int)(now * 100 / timerEnd);

				long value = (timerEnd - now) / 10000000 + 1;
				long end = timerEnd / 10000000;
				this.Text = FORM_TEXT + " (" + value.ToString() + "/" + end.ToString() + ")";
			}
		}

		private void MagTimerStart()
		{
			timerStart = DateTime.Now.Ticks;
			timerEnd = settings.MagTimerTime * 1000 * 1000 * 10;
			timerMagTimer.Enabled = true;
			buttonStart.Text = BUTTON_TEXT_STOP;
		}

		private void MagTimerStop()
		{
			timerMagTimer.Enabled = false;
			progressMagTimer.Value = 0;
			this.Text = FORM_TEXT;
			buttonStart.Text = BUTTON_TEXT_START;
		}

		private void upDownTime_ValueChanged(object sender, System.EventArgs e)
		{
			settings.MagTimerTime = (long)upDownTime.Value;
		}

		private void checkReload_CheckedChanged(object sender, System.EventArgs e)
		{
			settings.MagTimerReload = checkReload.Checked;
		}
	}
}
