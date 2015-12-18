using System;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.InteropServices;
using System.Collections;

namespace PSOBBTools
{
	public class StartUp 
	{
		// http://www.atmarkit.co.jp/fdotnet/dotnettips/145winmutex/winmutex.html
		// �A�v���P�[�V�����Œ薼
		private static string mutexName = Settings.applicationName;
		// ���d�N����h�~����~���[�e�b�N�X
		private static System.Threading.Mutex mutexObject;

		[STAThread]
		public static void Main()
		{
			// Windows 2000�iNT 5.0�j�ȍ~�̂݃O���[�o���E�~���[�e�b�N�X���p��
			OperatingSystem os = Environment.OSVersion;

			if ((os.Platform == PlatformID.Win32NT) && (os.Version.Major >= 5))
			{
				mutexName = @"Global\" + mutexName;
			}

			try
			{
				// �~���[�e�b�N�X�𐶐�����
				mutexObject = new System.Threading.Mutex(false, mutexName);
			}
			catch (ApplicationException e)
			{
				// �O���[�o���E�~���[�e�b�N�X�ɂ�鑽�d�N���֎~
				MessageBox.Show("���łɋN�����Ă��܂��B", Settings.applicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			// �~���[�e�b�N�X���擾����
			if (mutexObject.WaitOne(0, false))
			{
				// �A�v���P�[�V���������s
				using (Main main = new Main()) 
				{
					Application.Run();
				}

				// �~���[�e�b�N�X���������
				mutexObject.ReleaseMutex();
			}
			else
			{
				// �x����\�����ďI��
				MessageBox.Show("���łɋN�����Ă��܂��B", Settings.applicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			// �~���[�e�b�N�X��j������
			mutexObject.Close();
		}
	}

	public class Main : IDisposable
	{
		// API
		private const int GWL_STYLE = -16;

		private const int WS_SYSMENU = 0x00080000;
		private const int WS_MINIMIZEBOX = 0x00020000;
		private const int WS_MAXIMIZEBOX = 0x00010000;

		[DllImport("user32.dll")] 
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
		[DllImport("user32.dll")] 
		private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
		[DllImport("user32.dll")]
		private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		// PSOBB�̃E�B���h�E�N���X��
		private const string PSOBB_CLASS = "PHANTASY STAR ONLINE Blue Burst";

		// �ݒ�
		private Settings settings = new Settings();

		// ���j���[
		private MenuItem menuTeamChime = new MenuItem();
		private MenuItem menuCommand = new MenuItem();
		private MenuItem menuSSCompression = new MenuItem();
		private MenuItem menuSystemButtons = new MenuItem();
		private MenuItem menuLine1 = new MenuItem();
		private MenuItem menuSetting = new MenuItem();
		private MenuItem menuLine2 = new MenuItem();
		private MenuItem menuMagTimer = new MenuItem();
		private MenuItem menuLine3 = new MenuItem();
		private MenuItem menuExit = new MenuItem();
		private ContextMenu contextMenu = new ContextMenu();
		// �^�X�N�g���C�̃A�C�R��
		private NotifyIcon notifyIcon = new NotifyIcon();
		// ���O�Ď�
		private FileSystemWatcher logWatcher = new FileSystemWatcher();
		// SS�Ď�
		private FileSystemWatcher bmpWatcher = new FileSystemWatcher();
		// �V�X�e���{�^���p�^�C�}
		private Timer systemButtonTimer = new Timer();

		// �ݒ�E�B���h�E
		private Form formSettings = null;
		// �}�O�^�C�}�[�E�B���h�E
		private Form formMagTimer = null;

		// �ϊ�����SS���X�g
		private Queue convertFileList = Queue.Synchronized(new Queue());
		// ThreadingImageConverter�X���b�h
		private System.Threading.Thread ImageConverterThread;
		// ThreadingImageConverter
		private ThreadingImageConverter imageConverter;
		
		public Main()
		{
			// ����������
			InitializeComponent();

			// �ݒ�ǂݍ���
			Load();

			// �ݒ�̔��f
			ValidateSettings();

			// ThreadingImageConverter
			imageConverter = new ThreadingImageConverter(convertFileList);
		}

		// ����������
		private void InitializeComponent()
		{
			try 
			{
				// �R���e�L�X�g���j���[�쐬
				CreateMenu();

				// �^�X�N�g���C�̃A�C�R���쐬
				CreateNotifyIcon();

				// �Ď��ݒ�
				CreateWatcher();

				// �^�C�}�[�ݒ�
				systemButtonTimer.Interval = 1000;
				systemButtonTimer.Tick += new EventHandler(systemButtonTimer_Tick);
			}
			catch (Exception e)
			{
				MessageBox.Show("�����������Ɏ��s���܂����B\n�ڍׁF" + e.Message, Settings.applicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		// �R���e�L�X�g���j���[�쐬
		private void CreateMenu() 
		{
			menuMagTimer.Text = "�}�O�^�C�}�[(&M)";
			menuMagTimer.DefaultItem = true;
			menuMagTimer.Click += new EventHandler(this.menuMagTimer_Click);
			contextMenu.MenuItems.Add(menuMagTimer);

			menuLine1.Text = "-";
			contextMenu.MenuItems.Add(menuLine1);

			menuTeamChime.Text = "�`�[���`���b�g�̃`���C��(&T)";
			menuTeamChime.Click += new EventHandler(this.menuTeamChime_Click);
			contextMenu.MenuItems.Add(menuTeamChime);

			menuCommand.Text = "�R�}���h(&C)";
			menuCommand.Click += new EventHandler(this.menuCommand_Click);
			contextMenu.MenuItems.Add(menuCommand);

			menuSSCompression.Text = "SS�̎������k(&C)";
			menuSSCompression.Click += new EventHandler(this.menuSSCompression_Click);
			contextMenu.MenuItems.Add(menuSSCompression);

			menuSystemButtons.Text = "�V�X�e���{�^���̕\��(&B)";
			menuSystemButtons.Click += new EventHandler(this.menuSystemButtons_Click);
			contextMenu.MenuItems.Add(menuSystemButtons);

			menuLine2.Text = "-";
			contextMenu.MenuItems.Add(menuLine2);

			menuSetting.Text = "�ݒ�(&S)";
			menuSetting.Click += new EventHandler(this.menuSetting_Click);
			contextMenu.MenuItems.Add(menuSetting);

			menuLine3.Text = "-";
			contextMenu.MenuItems.Add(menuLine3);

			menuExit.Text = "�I��(&X)";
			menuExit.Click += new EventHandler(this.menuExit_Click);
			contextMenu.MenuItems.Add(menuExit);

			contextMenu.Popup += new EventHandler(contextMenu_Popup);
		}

		// �^�X�N�g���C�̃A�C�R���쐬
		private void CreateNotifyIcon() 
		{
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
			notifyIcon.Icon = new System.Drawing.Icon(assembly.GetManifestResourceStream("PSOBBTools.tray.ico"), 16, 16);
			notifyIcon.Text = "PSOBBTools";
			notifyIcon.ContextMenu = contextMenu;
			notifyIcon.DoubleClick += new EventHandler(notifyIcon_DoubleClick);
			notifyIcon.Visible = true;
		}

		// �Ď��ݒ�
		private void CreateWatcher() 
		{
			// ���O�Ď�
			logWatcher.Filter = settings.TeamChatFileFilter;
			logWatcher.NotifyFilter = NotifyFilters.LastWrite;
			logWatcher.Changed += new FileSystemEventHandler(LogFile_Changed);

			// SS�Ď�
			bmpWatcher.Filter = settings.SSFileFilter;
			bmpWatcher.Created += new FileSystemEventHandler(BmpFile_Created);
		}

		private void notifyIcon_DoubleClick(object sender, System.EventArgs e)
		{
			ShowMagTimer();
		}

		private void contextMenu_Popup(object sender, System.EventArgs e)
		{
			menuTeamChime.Checked = settings.TeamChimeEnabled;
			menuCommand.Checked = settings.CommandEnabled;
			menuSSCompression.Checked = settings.SSCompressionEnabled;
			menuSystemButtons.Checked = settings.SystemButtonsEnabled;
		}

		private void menuTeamChime_Click(object sender, System.EventArgs e)
		{
			// �ύX���ʂ̔��f
			settings.TeamChimeEnabled = !settings.TeamChimeEnabled;

			// �Ď��̗L����/������
			logWatcher.EnableRaisingEvents = (settings.TeamChimeEnabled || settings.CommandEnabled);
		}

		private void menuCommand_Click(object sender, System.EventArgs e)
		{
			// �ύX���ʂ̔��f
			settings.CommandEnabled = !settings.CommandEnabled;

			// �Ď��̗L����/������
			logWatcher.EnableRaisingEvents = (settings.TeamChimeEnabled || settings.CommandEnabled);
		}

		private void menuSSCompression_Click(object sender, System.EventArgs e)
		{
			// �ύX���ʂ̔��f
			settings.SSCompressionEnabled = !settings.SSCompressionEnabled;

			// �Ď��̗L����/������
			bmpWatcher.EnableRaisingEvents = settings.SSCompressionEnabled;
		}

		private void menuSystemButtons_Click(object sender, System.EventArgs e)
		{
			// �ύX���ʂ̔��f
			settings.SystemButtonsEnabled = !settings.SystemButtonsEnabled;

			// �^�C�}�[�̗L����/������
			systemButtonTimer.Enabled = settings.SystemButtonsEnabled;
		}

		private void menuSetting_Click(object sender, System.EventArgs e)
		{
			ShowSettings();
		}

		private void menuMagTimer_Click(object sender, System.EventArgs e)
		{
			ShowMagTimer();
		}

		private void menuExit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void systemButtonTimer_Tick(object sender, EventArgs e)
		{
			IntPtr hwnd = FindWindow(PSOBB_CLASS, null);

			if (hwnd != IntPtr.Zero)
			{
				int style = GetWindowLong(hwnd, GWL_STYLE);
				style |= (WS_SYSMENU | WS_MINIMIZEBOX | WS_MAXIMIZEBOX);
				SetWindowLong(hwnd, GWL_STYLE, style);
			}
		}

		private void ShowSettings()
		{
			// �ݒ�E�B���h�E�\��
			if (formSettings == null)
			{
				formSettings = new FormSettings(settings);
				formSettings.ShowDialog();
				formSettings.Dispose();
				formSettings = null;

				// �ݒ�̔��f
				ValidateSettings();
			}
			else
			{
				formSettings.Activate();
			}
		}
	
		private void ShowMagTimer()
		{
			// �}�O�^�C�}�[�E�B���h�E�\��
			if (formMagTimer == null)
			{
				formMagTimer = new FormMagTimer(settings);
				formMagTimer.Location = new System.Drawing.Point(settings.MagTimerLocation.X, settings.MagTimerLocation.Y);
				formMagTimer.ShowDialog();
				settings.MagTimerLocation = formMagTimer.Location;
				formMagTimer.Dispose();
				formMagTimer = null;
			}
			else
			{
				formMagTimer.Activate();
			}
		}
	
		public void Dispose()
		{
			// �ݒ�ۑ�
			Save();

			// ��n��
			logWatcher.EnableRaisingEvents = false;
			bmpWatcher.EnableRaisingEvents = false;
			notifyIcon.Visible = false;

			// �X���b�h�̏I����҂�
			if (ImageConverterThread != null)
			{
				ImageConverterThread.Join();
			}
		}

		/// <summary>
		/// ���O�t�@�C���ύX�C�x���g
		/// </summary>
		private void LogFile_Changed(object source, FileSystemEventArgs e)
		{
			// �`�[���`���b�g�̃`���C��
			if (settings.TeamChimeEnabled && File.Exists(settings.ChimeFile))
			{
				Sound.Play(settings.ChimeFile);
			}

			// �R�}���h
			if (settings.CommandEnabled && File.Exists(e.FullPath))
			{
				// �R�}���h����
				cmd(e.FullPath);
			}
		}

		private void BmpFile_Created(object source, FileSystemEventArgs e)
		{
			if (File.Exists(e.FullPath))
			{
				// ���k
				ThreadingImageConverter.convertFile cf = new ThreadingImageConverter.convertFile();

				cf.FileName = e.FullPath;

				if (settings.SSFileFormat == Settings.SSFileFormats.png)
				{
					cf.FileFormat = "png";
				}
				else
				{
					cf.FileFormat = "jpg";
				}


				lock(convertFileList.SyncRoot)
				{
					convertFileList.Enqueue(cf);
				}

				if (ImageConverterThread == null || !ImageConverterThread.IsAlive)
				{
					ImageConverterThread = imageConverter.createThread();
					ImageConverterThread.Start();
				}
			}
		}

		// �R�}���h����
		private void cmd(string file)
		{
			string line = "";
			string lastLine = "";

			try 
			{
				// �ŏI�s��ǂݎ��
				using (StreamReader reader = new StreamReader(file, System.Text.Encoding.Unicode)) 
				{
					while ((line = reader.ReadLine()) != null)
					{
						lastLine = line;
					}
				}
			}
			catch (Exception e) 
			{
				return;
			}

			// �ŏI�s���疼�O�ƃ��b�Z�[�W�𒊏o
			string name = "";
			string message = "";
			string [] split = null;

			split = lastLine.Split('\t');
			switch (split.Length)
			{
				case 3:
					// �`�[���`���b�g�̏ꍇ
					name = split[1];
					message = split[2];
					break;

				case 4:
					// �ʏ�`���b�g�̏ꍇ(�ꉞ�΍�)
					name = split[2];
					message = split[3];
					break;

				default:
					break;
			}

			// ���O�`�F�b�NOK�Ȃ���s
			if (name == settings.ExecName)
			{
				CompileInvoke.Exec(Application.StartupPath + @"\command\" + message + ".cs");
			}
		}

		/// <summary>
		/// �ݒ�̓ǂݍ���
		/// </summary>
		private void Load()
		{
			if (File.Exists(Settings.settingFile)) 
			{
				try
				{
					System.IO.Stream stream = System.IO.File.OpenRead(@Settings.settingFile);
					SoapFormatter soapFormatter = new SoapFormatter();
					settings = (Settings)soapFormatter.Deserialize(stream);
					stream.Close();
				}
				catch (Exception e)
				{
					MessageBox.Show("�ݒ�̓ǂݍ��ݒ��ɃG���[���������܂����B\n�ݒ�t�@�C���ƃv���O�����̃o�[�W�����������Ă��邩�m�F���Ă��������B\n�ڍׁF" + e.Message, Settings.applicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		/// <summary>
		/// �ݒ�̕ۑ�
		/// </summary>
		private void Save()
		{
			System.IO.Stream stream = System.IO.File.Create(@Settings.settingFile);
			SoapFormatter soapFormatter = new SoapFormatter();
			soapFormatter.Serialize(stream, settings);
			stream.Close(); 
		}

		// �ݒ�̃`�F�b�N�Ɣ��f
		private void ValidateSettings()
		{
			logWatcher.Filter = settings.TeamChatFileFilter;
			bmpWatcher.Filter = settings.SSFileFilter;
			systemButtonTimer.Enabled = settings.SystemButtonsEnabled;

			if (settings.PSOBBFolder.Length > 0 && Directory.Exists(settings.PSOBBFolder))
			{
				logWatcher.Path = settings.PSOBBFolder + @"\" + Settings.logFolder;
				bmpWatcher.Path = settings.PSOBBFolder + @"\" + Settings.ssFolder;
				logWatcher.EnableRaisingEvents = (settings.TeamChimeEnabled || settings.CommandEnabled);
				bmpWatcher.EnableRaisingEvents = settings.SSCompressionEnabled;
			}
			else
			{
				MessageBox.Show("PSOBB�̃t�H���_��������܂���B�ݒ���m�F���Ă��������B", Settings.applicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				settings.TeamChimeEnabled = false;
				settings.CommandEnabled = false;
				settings.SSCompressionEnabled = false;
				logWatcher.EnableRaisingEvents = false;
				bmpWatcher.EnableRaisingEvents = false;
			}
		}
	}
}
