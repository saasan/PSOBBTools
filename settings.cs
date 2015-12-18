using System;
using System.ComponentModel;
using System.Design;
using System.Runtime.Serialization;

namespace PSOBBTools
{
	/// <summary>
	/// �A�v���P�[�V�����̐ݒ�
	/// </summary>
	[TypeConverter(typeof(PropertyDisplayConverter))]
	public class Settings
	{
        /// <summary>�ݒ�t�@�C����</summary>
        public const string settingsFile = "settings.xml";
        /// <summary>�ݒ�t�@�C����ۑ�����t�H���_��</summary>
        public static readonly string settingsFolder;
        /// <summary>���ݒ�t�@�C����</summary>
        public const string oldSettingsFile = "PSOBBTools.xml";
        /// <summary>���O�t�H���_��</summary>
		public const string logFolder = "log";
        /// <summary>SS�t�H���_��</summary>
		public const string ssFolder = "bmp";
        /// <summary>PSOBB�̃E�B���h�E�N���X��</summary>
        public const string windowClassName = "PHANTASY STAR ONLINE Blue Burst";

        /// <summary>�}�O�^�C�}�[�̎��Ԃ̍ŏ��l</summary>
        public const decimal magTimerTimeMin = 1;
        /// <summary>�}�O�^�C�}�[�̎��Ԃ̍ő�l</summary>
        public const decimal magTimerTimeMax = 600;

        /// <summary>SS�t�@�C���̌`��</summary>
		public enum SSFileFormats
		{
			png,
			jpg
		}

        /// <summary>PSOBB�̃t�H���_</summary>
		private string psobbFolder = @"C:\Program Files\SEGA\PHANTASY STAR ONLINE Blue Burst";
        // �`�[���`���b�g�̃��O�t�@�C����</summary>
		private string teamChatFileFilter = "GuildChat*.txt";

        /// <summary>�`�[���`���b�g�̃`���C��ON/OFF</summary>
		private bool teamChimeEnabled = false;
        /// <summary>SS�̎������kON/OFF</summary>
		private bool ssCompressionEnabled = false;
        /// <summary>�V�X�e���{�^��ON/OFF</summary>
		private bool systemButtonsEnabled = false;

        /// <summary>�`���C�����̃t�@�C��</summary>
		private string chimeFile = "pon.wav";

        /// <summary>SS�̃t�@�C����</summary>
		private string ssFileFilter = "pso*.bmp";
        /// <summary>SS�̈��k�`��</summary>
		private SSFileFormats ssFileFormat = SSFileFormats.jpg;

        /// <summary>�}�O�^�C�}�[���̃t�@�C��</summary>
		private string magTimerFile = "popin.wav";
        /// <summary>�}�O�^�C�}�[�̎���</summary>
		private long magTimerTime = 210;
        /// <summary>�}�O�^�C�}�[�̌J��Ԃ�</summary>
		private bool magTimerReload = false;
        /// <summary>�}�O�^�C�}�[����Ɏ�O�ɕ\��</summary>
		private bool magTimerTopMost = false;

        /// <summary>�}�O�^�C�}�[�E�B���h�E�̈ʒu</summary>
		private System.Drawing.Point magTimerLocation = new System.Drawing.Point(50, 50);

        /// <summary>�ύX�C�x���g</summary>
        public event EventHandler Changed;

		static Settings()
		{
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            settingsFolder = appData + @"\" + System.Windows.Forms.Application.CompanyName + @"\" + System.Windows.Forms.Application.ProductName;
		}

        public Settings()
        {
        }

        /// <summary>
        /// �ύX�C�x���g�𔭐�������
        /// </summary>
        /// <param name="e">�C�x���g�f�[�^</param>
        void OnChanged(EventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
            }
        }
        
        /// <summary>
        /// PSOBB�̃t�H���_
        /// </summary>
		[Category("�S��"),
			PropertyDisplayName("PSOBB�̃t�H���_"),
			Description("PSOBB�̃t�H���_(PsoBB.exe��������t�H���_)��ݒ肵�܂��B"),
			Editor(typeof(System.Windows.Forms.Design.FolderNameEditor),
			    typeof(System.Drawing.Design.UITypeEditor))]
		public string PSOBBFolder
		{
			get { return psobbFolder; }
			set
            {
                psobbFolder = value;
                OnChanged(EventArgs.Empty);
            }
		}

        /// <summary>
        /// �`�[���`���b�g�̃��O�t�@�C����
        /// </summary>
		[Category("�S��"),
			PropertyDisplayName("���O�t�@�C����(�`�[��)"),
			Description("�Ď��ΏۂƂ���`�[���`���b�g�̃��O�t�@�C������ݒ肵�܂��B")]
		public string TeamChatFileFilter
		{
			get { return teamChatFileFilter; }
			set
			{
				teamChatFileFilter = value;
                OnChanged(EventArgs.Empty);
            }
		}

        /// <summary>
        /// �`�[���`���b�g�̃`���C��ON/OFF
        /// </summary>
		[Category("�S��"),
			PropertyDisplayName("�`�[���`���b�g�̃`���C��"),
            Description("�`�[���`���b�g�̃`���C�����g�p���邩�ǂ�����ݒ肵�܂��B")]
		public bool TeamChimeEnabled
		{
			get { return teamChimeEnabled; }
			set
			{
				teamChimeEnabled = value;
                OnChanged(EventArgs.Empty);
            }
		}

        /// <summary>
        /// SS�̎������kON/OFF
        /// </summary>
		[Category("�S��"),
			PropertyDisplayName("SS�̎������k"),
            Description("�X�N���[���V���b�g���������k���邩�ǂ�����ݒ肵�܂��B")]
		public bool SSCompressionEnabled
		{
			get { return ssCompressionEnabled; }
			set
			{
				ssCompressionEnabled = value;
                OnChanged(EventArgs.Empty);
            }
		}

        /// <summary>
        /// �V�X�e���{�^��ON/OFF
        /// </summary>
		[Category("�S��"),
			PropertyDisplayName("�V�X�e���{�^���̕\��"),
            Description("�V�X�e���{�^����\�����邩�ǂ�����ݒ肵�܂��B")]
		public bool SystemButtonsEnabled
		{
			get { return systemButtonsEnabled; }
			set
			{
				systemButtonsEnabled = value;
                OnChanged(EventArgs.Empty);
            }
		}

        /// <summary>
        /// �`���C�����̃t�@�C��
        /// </summary>
		[Category("�`�[���`���b�g�̃`���C��"),
			PropertyDisplayName("�`���C���pWAV�t�@�C��"),
			Description("�`���C���Ƃ��Ďg�p����WAV�t�@�C����ݒ肵�܂��B"),
			Editor(typeof(UIFileNameEditor),
			    typeof(System.Drawing.Design.UITypeEditor)),
			FileDialogFilter("WAV�t�@�C�� (*.wav)|*.wav")]
		public string ChimeFile
		{
			get { return chimeFile; }
			set
			{
				chimeFile = value;
                OnChanged(EventArgs.Empty);
            }
		}

        /// <summary>
        /// SS�̃t�@�C����
        /// </summary>
		[Category("SS�̎������k"),
			PropertyDisplayName("SS�̃t�@�C����"),
			Description("�Ď��ΏۂƂ���X�N���[���V���b�g�̃t�@�C������ݒ肵�܂��B")]
		public string SSFileFilter
		{
			get { return ssFileFilter; }
			set
			{
				ssFileFilter = value;
                OnChanged(EventArgs.Empty);
            }
		}

        /// <summary>
        /// SS�̈��k�`��
        /// </summary>
		[Category("SS�̎������k"),
			PropertyDisplayName("SS�̈��k�`��"),
			Description("�X�N���[���V���b�g�̈��k�`����ݒ肵�܂��B")]
		public SSFileFormats SSFileFormat
		{
			get { return ssFileFormat; }
			set
			{
				ssFileFormat = value;
                OnChanged(EventArgs.Empty);
            }
		}

        /// <summary>
        /// �}�O�^�C�}�[���̃t�@�C��
        /// </summary>
		[Category("�}�O�^�C�}�["),
		PropertyDisplayName("�}�O�^�C�}�[�pWAV�t�@�C��"),
			Description("�}�O�^�C�}�[�̉��Ƃ��Ďg�p����WAV�t�@�C����ݒ肵�܂��B"),
			Editor(typeof(UIFileNameEditor),
			    typeof(System.Drawing.Design.UITypeEditor)),
			FileDialogFilter("WAV�t�@�C�� (*.wav)|*.wav")]
		public string MagTimerFile
		{
			get { return magTimerFile; }
			set
			{
				magTimerFile = value;
                OnChanged(EventArgs.Empty);
            }
		}

        /// <summary>
        /// �}�O�^�C�}�[�̎���
        /// </summary>
		[Category("�}�O�^�C�}�["),
			PropertyDisplayName("�}�O�^�C�}�[�̎���(�b)"),
			Description("�}�O�^�C�}�[�̎���(�b)��ݒ肵�܂��B")]
		public long MagTimerTime
		{
			get { return magTimerTime; }
			set
			{
                if (magTimerTimeMin <= value && value <= magTimerTimeMax)
				{
					magTimerTime = value;
                    OnChanged(EventArgs.Empty);
                }
				else
				{
                    System.Windows.Forms.MessageBox.Show("�}�O�^�C�}�[�̎��Ԃɂ�" + magTimerTimeMin + "����" + magTimerTimeMax + "�̊Ԃ̐������w�肵�Ă��������B", System.Windows.Forms.Application.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				}
			}
		}

        /// <summary>
        /// �}�O�^�C�}�[�̌J��Ԃ�
        /// </summary>
		[Category("�}�O�^�C�}�["),
			PropertyDisplayName("�}�O�^�C�}�[�̌J��Ԃ�"),
			Description("�}�O�^�C�}�[�̌J��Ԃ���ݒ肵�܂��B")]
		public bool MagTimerReload
		{
			get { return magTimerReload; }
			set
			{
				magTimerReload = value;
                OnChanged(EventArgs.Empty);
            }
		}

        /// <summary>
        /// �}�O�^�C�}�[����Ɏ�O�ɕ\��
        /// </summary>
        [Category("�}�O�^�C�}�["),
            PropertyDisplayName("�}�O�^�C�}�[����Ɏ�O�ɕ\��"),
            Description("�}�O�^�C�}�[����Ɏ�O�ɕ\�����邩�ǂ�����ݒ肵�܂��B")]
        public bool MagTimerTopMost
        {
            get { return magTimerTopMost; }
            set
			{
				magTimerTopMost = value;
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// �}�O�^�C�}�[�E�B���h�E�̈ʒu
        /// </summary>
		[Browsable(false)]
		public System.Drawing.Point MagTimerLocation
		{
			get { return magTimerLocation; }
			set
			{
				magTimerLocation = value;
                OnChanged(EventArgs.Empty);
            }
		}
	}
}