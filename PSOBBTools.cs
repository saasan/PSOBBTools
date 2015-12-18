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
		// アプリケーション固定名
		private static string mutexName = Application.ProductName;
		// 多重起動を防止するミューテックス
		private static System.Threading.Mutex mutexObject;

		[STAThread]
		public static void Main()
		{
			// Windows 2000（NT 5.0）以降のみグローバル・ミューテックス利用可
			OperatingSystem os = Environment.OSVersion;

			if ((os.Platform == PlatformID.Win32NT) && (os.Version.Major >= 5))
			{
				mutexName = @"Global\" + mutexName;
			}

			try
			{
				// ミューテックスを生成する
				mutexObject = new System.Threading.Mutex(false, mutexName);
			}
			catch (ApplicationException)
			{
				return;
			}

			// ミューテックスを取得する
			if (mutexObject.WaitOne(0, false))
			{
				// アプリケーションを実行
				using (PSOBBTools main = new PSOBBTools())
				{
					Application.Run();
				}

				// ミューテックスを解放する
				mutexObject.ReleaseMutex();
			}

			// ミューテックスを破棄する
			mutexObject.Close();
		}
	}

	public class PSOBBTools : IDisposable
	{
		// 設定
		private Settings settings = new Settings();

		// メニュー
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
		// タスクトレイのアイコン
		private NotifyIcon notifyIcon = new NotifyIcon();
		// ログ監視
		private FileSystemWatcher logWatcher = new FileSystemWatcher();
		// SS監視
		private FileSystemWatcher bmpWatcher = new FileSystemWatcher();
		// システムボタン用タイマ
		private Timer systemButtonTimer = new Timer();

		// 設定ウィンドウ
		private Form formSettings;
		// マグタイマーウィンドウ
		private Form formMagTimer;
        // ウィンドウサイズの変更ウィンドウ
        private Form formWindowResize;

		// 変換するSSリスト
		private Queue convertFileList = Queue.Synchronized(new Queue());
		// ThreadingImageConverterスレッド
		private System.Threading.Thread ImageConverterThread;
		// ThreadingImageConverter
		private ThreadingImageConverter imageConverter;

		public PSOBBTools()
		{
			// 初期化処理
			InitializeComponent();

			// 設定読み込み
			Load();

			// 設定の反映
			ValidateSettings();

			// ThreadingImageConverter
			imageConverter = new ThreadingImageConverter(convertFileList);
		}

		// 初期化処理
		private void InitializeComponent()
		{
			try
			{
				// コンテキストメニュー作成
				CreateMenu();

				// タスクトレイのアイコン作成
				CreateNotifyIcon();

				// 監視設定
				CreateWatcher();

				// タイマー設定
				systemButtonTimer.Interval = 1000;
				systemButtonTimer.Tick += new EventHandler(systemButtonTimer_Tick);
			}
			catch (Exception e)
			{
				MessageBox.Show("初期化処理に失敗しました。\n詳細：" + e.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		// コンテキストメニュー作成
		private void CreateMenu()
		{
			menuMagTimer.Text = "マグタイマー(&M)";
			menuMagTimer.DefaultItem = true;
			menuMagTimer.Click += new EventHandler(this.menuMagTimer_Click);
			contextMenu.MenuItems.Add(menuMagTimer);

			menuWindowResize.Text = "ウィンドウサイズの変更(&R)";
			menuWindowResize.Click += new EventHandler(this.menuWindowResize_Click);
			contextMenu.MenuItems.Add(menuWindowResize);

			menuLine1.Text = "-";
			contextMenu.MenuItems.Add(menuLine1);

			menuTeamChime.Text = "チームチャットのチャイム(&T)";
			menuTeamChime.Click += new EventHandler(this.menuTeamChime_Click);
			contextMenu.MenuItems.Add(menuTeamChime);

			menuSSCompression.Text = "SSの自動圧縮(&C)";
			menuSSCompression.Click += new EventHandler(this.menuSSCompression_Click);
			contextMenu.MenuItems.Add(menuSSCompression);

			menuSystemButtons.Text = "システムボタンの表示(&B)";
			menuSystemButtons.Click += new EventHandler(this.menuSystemButtons_Click);
			contextMenu.MenuItems.Add(menuSystemButtons);

			menuLine2.Text = "-";
			contextMenu.MenuItems.Add(menuLine2);

			menuSetting.Text = "設定(&S)";
			menuSetting.Click += new EventHandler(this.menuSetting_Click);
			contextMenu.MenuItems.Add(menuSetting);

			menuLine3.Text = "-";
			contextMenu.MenuItems.Add(menuLine3);

			menuExit.Text = "終了(&X)";
			menuExit.Click += new EventHandler(this.menuExit_Click);
			contextMenu.MenuItems.Add(menuExit);

			contextMenu.Popup += new EventHandler(contextMenu_Popup);
		}

		// タスクトレイのアイコン作成
		private void CreateNotifyIcon()
		{
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
			notifyIcon.Icon = new System.Drawing.Icon(assembly.GetManifestResourceStream("PSOBBTools.tray.ico"), 16, 16);
			notifyIcon.Text = "PSOBBTools";
			notifyIcon.ContextMenu = contextMenu;
			notifyIcon.DoubleClick += new EventHandler(notifyIcon_DoubleClick);
			notifyIcon.Visible = true;
		}

		// 監視設定
		private void CreateWatcher()
		{
			// ログ監視
			logWatcher.Filter = settings.TeamChatFileFilter;
			logWatcher.NotifyFilter = NotifyFilters.LastWrite;
			logWatcher.Changed += new FileSystemEventHandler(LogFile_Changed);

			// SS監視
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
			// 変更結果の反映
			settings.TeamChimeEnabled = !settings.TeamChimeEnabled;

			// 監視の有効化/無効化
			logWatcher.EnableRaisingEvents = settings.TeamChimeEnabled;
		}

		private void menuSSCompression_Click(object sender, System.EventArgs e)
		{
			// 変更結果の反映
			settings.SSCompressionEnabled = !settings.SSCompressionEnabled;

			// 監視の有効化/無効化
			bmpWatcher.EnableRaisingEvents = settings.SSCompressionEnabled;
		}

		private void menuSystemButtons_Click(object sender, System.EventArgs e)
		{
			// 変更結果の反映
			settings.SystemButtonsEnabled = !settings.SystemButtonsEnabled;

			// タイマーの有効化/無効化
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

                // ウィンドウを更新
                Window.SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0,
                        Window.SetWindowPosFlags.SWP_NOMOVE |
                        Window.SetWindowPosFlags.SWP_NOSIZE |
                        Window.SetWindowPosFlags.SWP_NOZORDER |
                        Window.SetWindowPosFlags.SWP_FRAMECHANGED |
                        Window.SetWindowPosFlags.SWP_NOACTIVATE);
			}
		}

        /// <summary>
        /// 設定ウィンドウ表示
        /// </summary>
        private void ShowSettings()
		{
            // すでに表示されているか？
            if (formSettings == null || formSettings.IsDisposed)
			{
                // メニューの無効化
                foreach (MenuItem menu in contextMenu.MenuItems)
                {
                    if (!menu.Equals(menuExit))
                    {
                        menu.Enabled = false;
                    }
                }

			    // 設定ウィンドウ表示
                using (formSettings = new FormSettings(settings))
                {
				    formSettings.ShowDialog();
                }

				// 設定の反映
				ValidateSettings();

                // メニューの有効化
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
                // アクティブにする
				formSettings.Activate();
			}
		}

        /// <summary>
        /// マグタイマーウィンドウ表示
        /// </summary>
		private void ShowMagTimer()
		{
            // すでに表示されているか？
            if (formMagTimer == null || formMagTimer.IsDisposed)
			{
			    // マグタイマーウィンドウ表示
                using (formMagTimer = new FormMagTimer(settings))
                {
				    formMagTimer.Location = settings.MagTimerLocation;
				    formMagTimer.ShowDialog();
				    settings.MagTimerLocation = formMagTimer.Location;
                }
			}
			else
			{
                // アクティブにする
                formMagTimer.Activate();
			}
		}

        /// <summary>
        /// ウィンドウサイズの変更ウィンドウ表示
        /// </summary>
        private void ShowWindowResize()
        {
            // すでに表示されているか？
            if (formWindowResize == null || formWindowResize.IsDisposed)
            {
                // ウィンドウサイズの変更ウィンドウ表示
                using (formWindowResize = new FormWindowResize())
                {
                    formWindowResize.ShowDialog();
                }
            }
            else
            {
                // アクティブにする
                formWindowResize.Activate();
            }
        }

		public void Dispose()
		{
			// 設定保存
			Save();

			// 後始末
			logWatcher.EnableRaisingEvents = false;
			bmpWatcher.EnableRaisingEvents = false;
			notifyIcon.Visible = false;

			// スレッドの終了を待つ
			if (ImageConverterThread != null)
			{
				ImageConverterThread.Join();
			}
		}

		/// <summary>
		/// ログファイル変更イベント
		/// </summary>
		private void LogFile_Changed(object source, FileSystemEventArgs e)
		{
			// チームチャットのチャイム
			if (settings.TeamChimeEnabled && File.Exists(settings.ChimeFile))
			{
				Sound.Play(settings.ChimeFile);
			}
		}

		private void BmpFile_Created(object source, FileSystemEventArgs e)
		{
			if (File.Exists(e.FullPath))
			{
				// 圧縮
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
		/// 設定の読み込み
		/// </summary>
		private void Load()
		{
            string filePath;

            // 旧設定ファイルがあればそれを使う
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
                    MessageBox.Show("設定の読み込み中にエラーが発生しました。\n詳細：" + e.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
		}

		/// <summary>
		/// 設定の保存
		/// </summary>
		private void Save()
		{
            string filePath;

            // 旧設定ファイルがあればそれを使う
            if (File.Exists(Settings.oldSettingsFile))
            {
                filePath = Settings.oldSettingsFile;
            }
            else
            {
                filePath = Settings.settingsFolder + @"\" + Settings.settingsFile;

                // 保存フォルダ作成
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
                MessageBox.Show("設定の保存中にエラーが発生しました。\n詳細：" + e.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

		// 設定のチェックと反映
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
				MessageBox.Show("PSOBBのフォルダが見つかりません。設定を確認してください。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				settings.TeamChimeEnabled = false;
				settings.SSCompressionEnabled = false;
				logWatcher.EnableRaisingEvents = false;
				bmpWatcher.EnableRaisingEvents = false;
			}
		}
	}
}
