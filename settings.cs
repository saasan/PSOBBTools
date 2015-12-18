using System;
using System.ComponentModel;
using System.Design;
using System.Runtime.Serialization;

namespace PSOBBTools
{
	/// <summary>
	/// �A�v���P�[�V�����̐ݒ�
	/// </summary>
	[Serializable, TypeConverter(typeof(PropertyDisplayConverter))]
	public class Settings
	{
		// �A�v���P�[�V������
		public const string applicationName = "PSOBBTools";
		// �ݒ�t�@�C����
		public const string settingFile = "PSOBBTools.xml";
		// ���O�t�H���_��
		public const string logFolder = "log";
		// SS�t�H���_��
		public const string ssFolder = "bmp";

		// SS�t�@�C���̌`��
		public enum SSFileFormats
		{
			png,
			jpg
		}

		// PSOBB�̃t�H���_
		private string psobbFolder = @"C:\Program Files\SEGA\PHANTASY STAR ONLINE Blue Burst";
		// �`�[���`���b�g�̃��O�t�@�C����
		private string teamChatFileFilter = "GuildChat*.txt";

		// �`�[���`���b�g�̃`���C��ON/OFF
		private bool teamChimeEnabled = false;
		// �R�}���hON/OFF
		private bool commandEnabled = false;
		// SS�̎������kON/OFF
		private bool ssCompressionEnabled = false;
		// �V�X�e���{�^��ON/OFF
		private bool systemButtonsEnabled = false;

		// �`���C�����̃t�@�C��
		private string chimeFile = "pon.wav";

		// �R�}���h�̎��s��������L������
		private string execName = "";

		// SS�̃t�@�C����
		private string ssFileFilter = "pso*.bmp";
		// SS�̈��k�`��
		private SSFileFormats ssFileFormat = SSFileFormats.jpg;

		// �}�O�^�C�}�[���̃t�@�C��
		private string magTimerFile = "popin.wav";
		// �}�O�^�C�}�[�̎���
		private long magTimerTime = 210;
		// �}�O�^�C�}�[�̌J��Ԃ�
		private bool magTimerReload = false;

		// �}�O�^�C�}�[�E�B���h�E�̈ʒu
		private System.Drawing.Point magTimerLocation = new System.Drawing.Point(10, 10);

		public Settings()
		{
		}

		public Settings(SerializationInfo info, StreamingContext ctxt)
		{
			psobbFolder = (string)info.GetValue("psobbFolder", typeof(string));

			teamChatFileFilter = (string)info.GetValue("teamChatFileFilter", typeof(string));

			teamChimeEnabled = (bool)info.GetValue("teamChimeEnabled", typeof(bool));
			commandEnabled = (bool)info.GetValue("commandEnabled", typeof(bool));
			ssCompressionEnabled = (bool)info.GetValue("ssCompressionEnabled", typeof(bool));
			systemButtonsEnabled = (bool)info.GetValue("systemButtonsEnabled", typeof(bool));

			chimeFile = (string)info.GetValue("chimeFile", typeof(string));

			execName = (string)info.GetValue("execName", typeof(string));

			ssFileFilter = (string)info.GetValue("ssFileFilter", typeof(string));
			ssFileFormat = (SSFileFormats)info.GetValue("ssFileFormat", typeof(SSFileFormats));

			magTimerFile = (string)info.GetValue("magTimerFile", typeof(string));
			magTimerTime = (long)info.GetValue("magTimerTime", typeof(long));
			magTimerReload = (bool)info.GetValue("magTimerReload", typeof(bool));

			magTimerLocation = (System.Drawing.Point)info.GetValue("magTimerLocation", typeof(System.Drawing.Point));
		}

		public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
		{
			info.AddValue("psobbFolder", psobbFolder);

			info.AddValue("teamChatFileFilter", teamChatFileFilter);

			info.AddValue("teamChimeEnabled", teamChimeEnabled);
			info.AddValue("commandEnabled", commandEnabled);
			info.AddValue("ssCompressionEnabled", ssCompressionEnabled);
			info.AddValue("systemButtonsEnabled", systemButtonsEnabled);

			info.AddValue("chimeFile", chimeFile);

			info.AddValue("execName", execName);

			info.AddValue("ssFileFilter", ssFileFilter);
			info.AddValue("ssFileFormat", ssFileFormat);
		
			info.AddValue("magTimerFile", magTimerFile);
			info.AddValue("magTimerTime", magTimerTime);
			info.AddValue("magTimerReload", magTimerReload);
		
			info.AddValue("magTimerLocation", magTimerLocation);
		}

		[Category("�S��"),
			PropertyDisplayName("PSOBB�̃t�H���_"),
			Description("PSOBB�̃t�H���_(PsoBB.exe��������t�H���_)���w�肵�܂��B"),
			Editor(typeof(System.Windows.Forms.Design.FolderNameEditor),
			typeof(System.Drawing.Design.UITypeEditor))]
		public string PSOBBFolder
		{
			get { return psobbFolder; }
			set { psobbFolder = value; }
		}

		[Category("�S��"),
			PropertyDisplayName("���O�t�@�C����(�`�[��)"),
			Description("�Ď��ΏۂƂ���`�[���`���b�g�̃��O�t�@�C�������w�肵�܂��B")]
		public string TeamChatFileFilter
		{
			get { return teamChatFileFilter; }
			set { teamChatFileFilter = value; }
		}

		[Category("�S��"),
			PropertyDisplayName("�`�[���`���b�g�̃`���C��"),
			Description("�`�[���`���b�g�̃`���C���̎g�p��؂�ւ��܂��B")]
		public bool TeamChimeEnabled
		{
			get { return teamChimeEnabled; }
			set { teamChimeEnabled = value; }
		}

		[Category("�S��"),
			PropertyDisplayName("�R�}���h�̎g�p"),
			Description("�R�}���h�̎g�p��؂�ւ��܂��B")]
		public bool CommandEnabled
		{
			get { return commandEnabled; }
			set { commandEnabled = value; }
		}

		[Category("�S��"),
			PropertyDisplayName("SS�̎������k"),
			Description("�X�N���[���V���b�g�̎������k�̎g�p��؂�ւ��܂��B")]
		public bool SSCompressionEnabled
		{
			get { return ssCompressionEnabled; }
			set { ssCompressionEnabled = value; }
		}

		[Category("�S��"),
			PropertyDisplayName("�V�X�e���{�^���̕\��"),
			Description("�V�X�e���{�^���̕\����؂�ւ��܂��B")]
		public bool SystemButtonsEnabled
		{
			get { return systemButtonsEnabled; }
			set { systemButtonsEnabled = value; }
		}

		[Category("�`�[���`���b�g�̃`���C��"),
			PropertyDisplayName("�`���C���pWAV�t�@�C��"),
			Description("�`���C���Ƃ��Ďg�p����WAV�t�@�C�����w�肵�܂��B"),
			Editor(typeof(UIFileNameEditor),
			typeof(System.Drawing.Design.UITypeEditor)),
			FileDialogFilter("WAV�t�@�C�� (*.wav)|*.wav")]
		public string ChimeFile
		{
			get { return chimeFile; }
			set { chimeFile = value; }
		}

		[Category("�R�}���h"),
			PropertyDisplayName("�R�}���h���L������"),
			Description("�R�}���h�̎��s��������L�����N�^�[�����w�肵�܂��B")]
		public string ExecName
		{
			get { return execName; }
			set { execName = value; }
		}

		[Category("SS�̎������k"),
			PropertyDisplayName("SS�̃t�@�C����"),
			Description("�Ď��ΏۂƂ���X�N���[���V���b�g�̃t�@�C�������w�肵�܂��B")]
		public string SSFileFilter
		{
			get { return ssFileFilter; }
			set { ssFileFilter = value; }
		}

		[Category("SS�̎������k"),
			PropertyDisplayName("SS�̈��k�`��"),
			Description("�X�N���[���V���b�g�̈��k�`�����w�肵�܂��B")]
		public SSFileFormats SSFileFormat
		{
			get { return ssFileFormat; }
			set { ssFileFormat = value; }
		}

		[Category("�}�O�^�C�}�["),
		PropertyDisplayName("�}�O�^�C�}�[�pWAV�t�@�C��"),
			Description("�}�O�^�C�}�[�̉��Ƃ��Ďg�p����WAV�t�@�C�����w�肵�܂��B"),
			Editor(typeof(UIFileNameEditor),
			typeof(System.Drawing.Design.UITypeEditor)),
			FileDialogFilter("WAV�t�@�C�� (*.wav)|*.wav")]
		public string MagTimerFile
		{
			get { return magTimerFile; }
			set { magTimerFile = value; }
		}

		[Category("�}�O�^�C�}�["),
			PropertyDisplayName("�}�O�^�C�}�[�̎���(�b)"),
			Description("�}�O�^�C�}�[�̎���(�b)���w�肵�܂��B")]
		public long MagTimerTime
		{
			get { return magTimerTime; }
			set
			{
				if (value > 0)
				{
					magTimerTime = value;
				}
				else
				{
					System.Windows.Forms.MessageBox.Show("�}�O�^�C�}�[�̎��Ԃɂ͐������w�肵�Ă��������B", applicationName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				}
			}
		}

		[Category("�}�O�^�C�}�["),
			PropertyDisplayName("�}�O�^�C�}�[�̌J��Ԃ�"),
			Description("�}�O�^�C�}�[�̌J��Ԃ����w�肵�܂��B")]
		public bool MagTimerReload
		{
			get { return magTimerReload; }
			set { magTimerReload = value; }
		}

		[Browsable(false)]
		public System.Drawing.Point MagTimerLocation
		{
			get { return magTimerLocation; }
			set { magTimerLocation = value; }
		}
	}
}