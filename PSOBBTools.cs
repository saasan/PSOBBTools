using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;         // XmlSerializer
using System.Collections;               // Queue

namespace PSOBBTools
{
	public class StartUp
	{
		// http://www.atmarkit.co.jp/fdotnet/dotnettips/145winmutex/winmutex.html
		// �A�v���P�[�V�����Œ薼
		private static string mutexName = Application.ProductName;
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
			catch (ApplicationException)
			{
				return;
			}

			// �~���[�e�b�N�X���擾����
			if (mutexObject.WaitOne(0, false))
			{
				// �A�v���P�[�V���������s
				using (PSOBBTools main = new PSOBBTools())
				{
					Application.Run();
				}

				// �~���[�e�b�N�X���������
				mutexObject.ReleaseMutex();
			}

			// �~���[�e�b�N�X��j������
			mutexObject.Close();
		}
	}

	public class PSOBBTools : IDisposable
	{
		// �ݒ�
		private Settings settings = new Settings();

		// ���j���[
		private MenuItem menuMagTimer = new MenuItem();
        private MenuItem menuWindowResize = new MenuItem();
        private MenuItem menuLine1 = new MenuItem();
		private MenuItem menuTeamChime = new MenuItem();
		private MenuItem menuSSCompression = new MenuItem();
		private MenuItem menuSystemButtons = new MenuItem();
		private MenuItem menuLine2 = new MenuItem();
		private MenuItem menuSetting = new MenuItem();
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
		private Form formSettings;
		// �}�O�^�C�}�[�E�B���h�E
		private Form formMagTimer;
        // �E�B���h�E�T�C�Y�̕ύX�E�B���h�E
        private Form formWindowResize;

		// �ϊ�����SS���X�g
		private Queue convertFileList = Queue.Synchronized(new Queue());
		// ThreadingImageConverter�X���b�h
		private System.Threading.Thread ImageConverterThread;
		// ThreadingImageConverter
		private ThreadingImageConverter imageConverter;

		public PSOBBTools()
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
				MessageBox.Show("�����������Ɏ��s���܂����B\n�ڍׁF" + e.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		// �R���e�L�X�g���j���[�쐬
		private void CreateMenu()
		{
			menuMagTimer.Text = "�}�O�^�C�}�[(&M)";
			menuMagTimer.DefaultItem = true;
			menuMagTimer.Click += new EventHandler(this.menuMagTimer_Click);
			contextMenu.MenuItems.Add(menuMagTimer);

			menuWindowResize.Text = "�E�B���h�E�T�C�Y�̕ύX(&R)";
			menuWindowResize.Click += new EventHandler(this.menuWindowResize_Click);
			contextMenu.MenuItems.Add(menuWindowResize);

			menuLine1.Text = "-";
			contextMenu.MenuItems.Add(menuLine1);

			menuTeamChime.Text = "�`�[���`���b�g�̃`���C��(&T)";
			menuTeamChime.Click += new EventHandler(this.menuTeamChime_Click);
			contextMenu.MenuItems.Add(menuTeamChime);

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
			menuSSCompression.Checked = settings.SSCompressionEnabled;
			menuSystemButtons.Checked = settings.SystemButtonsEnabled;
		}

		private void menuTeamChime_Click(object sender, System.EventArgs e)
		{
			// �ύX���ʂ̔��f
			settings.TeamChimeEnabled = !settings.TeamChimeEnabled;

			// �Ď��̗L����/������
			logWatcher.EnableRaisingEvents = settings.TeamChimeEnabled;
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

		private void menuWindowResize_Click(object sender, System.EventArgs e)
		{
            ShowWindowResize();
		}

		private void menuExit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void systemButtonTimer_Tick(object sender, EventArgs e)
		{
			IntPtr hwnd = Window.FindWindow(Settings.windowClassName, null);

			if (hwnd != IntPtr.Zero)
			{
                Window.WindowStyleFlags style = (Window.WindowStyleFlags)Window.GetWindowLong(hwnd, Window.GetWindowLongFlags.GWL_STYLE);

                style |= (Window.WindowStyleFlags.WS_SYSMENU |
                    Window.WindowStyleFlags.WS_MINIMIZEBOX |
                    Window.WindowStyleFlags.WS_MAXIMIZEBOX);

                Window.SetWindowLong(hwnd, Window.GetWindowLongFlags.GWL_STYLE, (int)style);

                // �E�B���h�E���X�V
                Window.SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0,
                        Window.SetWindowPosFlags.SWP_NOMOVE |
                        Window.SetWindowPosFlags.SWP_NOSIZE |
                        Window.SetWindowPosFlags.SWP_NOZORDER |
                        Window.SetWindowPosFlags.SWP_FRAMECHANGED |
                        Window.SetWindowPosFlags.SWP_NOACTIVATE);
			}
		}

        /// <summary>
        /// �ݒ�E�B���h�E�\��
        /// </summary>
        private void ShowSettings()
		{
            // ���łɕ\������Ă��邩�H
            if (formSettings == null || formSettings.IsDisposed)
			{
                // ���j���[�̖�����
                foreach (MenuItem menu in contextMenu.MenuItems)
                {
                    if (!menu.Equals(menuExit))
                    {
                        menu.Enabled = false;
                    }
                }

			    // �ݒ�E�B���h�E�\��
                using (formSettings = new FormSettings(settings))
                {
				    formSettings.ShowDialog();
                }

				// �ݒ�̔��f
				ValidateSettings();

                // ���j���[�̗L����
                foreach (MenuItem menu in contextMenu.MenuItems)
                {
                    if (!menu.Equals(menuExit))
                    {
                        menu.Enabled = true;
                    }
                }
            }
			else
			{
                // �A�N�e�B�u�ɂ���
				formSettings.Activate();
			}
		}

        /// <summary>
        /// �}�O�^�C�}�[�E�B���h�E�\��
        /// </summary>
		private void ShowMagTimer()
		{
            // ���łɕ\������Ă��邩�H
            if (formMagTimer == null || formMagTimer.IsDisposed)
			{
			    // �}�O�^�C�}�[�E�B���h�E�\��
                using (formMagTimer = new FormMagTimer(settings))
                {
				    formMagTimer.Location = settings.MagTimerLocation;
				    formMagTimer.ShowDialog();
				    settings.MagTimerLocation = formMagTimer.Location;
                }
			}
			else
			{
                // �A�N�e�B�u�ɂ���
                formMagTimer.Activate();
			}
		}

        /// <summary>
        /// �E�B���h�E�T�C�Y�̕ύX�E�B���h�E�\��
        /// </summary>
        private void ShowWindowResize()
        {
            // ���łɕ\������Ă��邩�H
            if (formWindowResize == null || formWindowResize.IsDisposed)
            {
                // �E�B���h�E�T�C�Y�̕ύX�E�B���h�E�\��
                using (formWindowResize = new FormWindowResize())
                {
                    formWindowResize.ShowDialog();
                }
            }
            else
            {
                // �A�N�e�B�u�ɂ���
                formWindowResize.Activate();
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

		/// <summary>
		/// �ݒ�̓ǂݍ���
		/// </summary>
		private void Load()
		{
            string filePath;

            // ���ݒ�t�@�C��������΂�����g��
            if (File.Exists(Settings.oldSettingsFile))
            {
                filePath = Settings.oldSettingsFile;
            }
            else
            {
                filePath = Settings.settingsFolder + @"\" + Settings.settingsFile;
            }

			if (File.Exists(filePath))
			{
                try
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                        settings = (Settings)serializer.Deserialize(fs);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("�ݒ�̓ǂݍ��ݒ��ɃG���[���������܂����B\n�ڍׁF" + e.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
		}

		/// <summary>
		/// �ݒ�̕ۑ�
		/// </summary>
		private void Save()
		{
            string filePath;

            // ���ݒ�t�@�C��������΂�����g��
            if (File.Exists(Settings.oldSettingsFile))
            {
                filePath = Settings.oldSettingsFile;
            }
            else
            {
                filePath = Settings.settingsFolder + @"\" + Settings.settingsFile;

                // �ۑ��t�H���_�쐬
                if (!Directory.Exists(Settings.settingsFolder))
                {
                    Directory.CreateDirectory(Settings.settingsFolder);
                }
            }

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                    serializer.Serialize(fs, settings);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("�ݒ�̕ۑ����ɃG���[���������܂����B\n�ڍׁF" + e.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
				logWatcher.EnableRaisingEvents = settings.TeamChimeEnabled;
				bmpWatcher.EnableRaisingEvents = settings.SSCompressionEnabled;
			}
			else
			{
				MessageBox.Show("PSOBB�̃t�H���_��������܂���B�ݒ���m�F���Ă��������B", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				settings.TeamChimeEnabled = false;
				settings.SSCompressionEnabled = false;
				logWatcher.EnableRaisingEvents = false;
				bmpWatcher.EnableRaisingEvents = false;
			}
		}
	}
}