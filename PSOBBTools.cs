using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Xml.Serialization;     // XmlSerializer
using System.Collections;           // ICollection
using System.Collections.Generic;   // Queue<>

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
        private MenuItem menuChatLog = new MenuItem();
		private MenuItem menuMagTimer = new MenuItem();
        private MenuItem menuLine1 = new MenuItem();
		private MenuItem menuTeamChime = new MenuItem();
		private MenuItem menuSSCompression = new MenuItem();
		private MenuItem menuSystemButtons = new MenuItem();
        private MenuItem menuWindowAutoRestore = new MenuItem();
        private MenuItem menuLine2 = new MenuItem();
        private MenuItem menuWindowSave = new MenuItem();
        private MenuItem menuWindowRestore = new MenuItem();
		private MenuItem menuLine3 = new MenuItem();
		private MenuItem menuOpenFolderLog = new MenuItem();
		private MenuItem menuOpenFolderBmp = new MenuItem();
		private MenuItem menuLine4 = new MenuItem();
		private MenuItem menuSetting = new MenuItem();
		private MenuItem menuLine5 = new MenuItem();
		private MenuItem menuExit = new MenuItem();
		private ContextMenu contextMenu = new ContextMenu();
		// タスクトレイのアイコン
		private NotifyIcon notifyIcon = new NotifyIcon();
		// チームチャットログ監視
		private FileSystemWatcher logWatcher = new FileSystemWatcher();
		// SS監視
		private FileSystemWatcher bmpWatcher = new FileSystemWatcher();
		// ウィンドウ関連(システムボタン、位置とサイズ)用タイマ
		private Timer windowTimer = new Timer();

		// チャットログウィンドウ
		private Form formChatLog;
		// マグタイマーウィンドウ
		private Form formMagTimer;
		// 設定ウィンドウ
		private Form formSettings;

		// 変換するSSリスト
        private Queue<ThreadingImageConverter.convertFile> convertFileList = new Queue<ThreadingImageConverter.convertFile>();
		// ThreadingImageConverterスレッド
		private System.Threading.Thread ImageConverterThread;
		// ThreadingImageConverter
		private ThreadingImageConverter imageConverter;

        // PSOBBのウィンドウ検出が初回
        private bool firstTime = true;

        public PSOBBTools()
		{
			// 初期化処理
			InitializeComponent();

			// 設定読み込み
			Load();

			// 設定の適用
			ApplySettings();

			// ThreadingImageConverter
			imageConverter = new ThreadingImageConverter(convertFileList);

            // イベントハンドラを追加
            settings.Changed += new EventHandler(settings_Changed);

            // チャットログを表示
            if (settings.ChatLogVisible)
            {
                ShowChatLog();
            }

            // マグタイマーを表示
            if (settings.MagTimerVisible)
            {
                ShowMagTimer();
            }

            // タスクトレイのアイコンを表示
            notifyIcon.Visible = true;
        }

        public void Dispose()
        {
            // タスクトレイのアイコンを非表示
            notifyIcon.Visible = false;

            // イベントハンドラを削除
            settings.Changed -= new EventHandler(settings_Changed);

            // 監視を停止
            logWatcher.EnableRaisingEvents = bmpWatcher.EnableRaisingEvents = false;
            
            // 設定保存
            Save();

            // スレッドの終了を待つ
            if (ImageConverterThread != null)
            {
                ImageConverterThread.Join();
            }
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
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
				windowTimer.Interval = 1000;
				windowTimer.Tick += new EventHandler(windowTimer_Tick);
			}
			catch (Exception e)
			{
				MessageBox.Show("初期化処理に失敗しました。\n詳細：" + e.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

        /// <summary>
        /// コンテキストメニュー作成
        /// </summary>
        private void CreateMenu()
		{
            menuChatLog.Text = "チャットログ(&C)";
            menuChatLog.DefaultItem = true;
            menuChatLog.Click += new EventHandler(this.menuChatLog_Click);
            contextMenu.MenuItems.Add(menuChatLog);

			menuMagTimer.Text = "マグタイマー(&M)";
			menuMagTimer.Click += new EventHandler(this.menuMagTimer_Click);
			contextMenu.MenuItems.Add(menuMagTimer);

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

            menuWindowAutoRestore.Text = "自動的にウィンドウの位置とサイズを復元(&A)";
			menuWindowAutoRestore.Click += new EventHandler(this.menuWindowAutoRestore_Click);
			contextMenu.MenuItems.Add(menuWindowAutoRestore);

			menuLine2.Text = "-";
			contextMenu.MenuItems.Add(menuLine2);

            menuWindowSave.Text = "ウィンドウの位置とサイズを保存(&V)";
			menuWindowSave.Click += new EventHandler(this.menuWindowSave_Click);
			contextMenu.MenuItems.Add(menuWindowSave);

            menuWindowRestore.Text = "ウィンドウの位置とサイズを復元(&R)";
            menuWindowRestore.Click += new EventHandler(this.menuWindowRestore_Click);
            contextMenu.MenuItems.Add(menuWindowRestore);

			menuLine3.Text = "-";
			contextMenu.MenuItems.Add(menuLine3);

            menuOpenFolderLog.Text = "logフォルダを開く(&L)";
			menuOpenFolderLog.Click += new EventHandler(this.menuOpenFolderLog_Click);
			contextMenu.MenuItems.Add(menuOpenFolderLog);

            menuOpenFolderBmp.Text = "bmpフォルダを開く(&P)";
			menuOpenFolderBmp.Click += new EventHandler(this.menuOpenFolderBmp_Click);
			contextMenu.MenuItems.Add(menuOpenFolderBmp);

			menuLine4.Text = "-";
			contextMenu.MenuItems.Add(menuLine4);

			menuSetting.Text = "設定(&S)";
			menuSetting.Click += new EventHandler(this.menuSetting_Click);
			contextMenu.MenuItems.Add(menuSetting);

			menuLine5.Text = "-";
			contextMenu.MenuItems.Add(menuLine5);

			menuExit.Text = "終了(&X)";
			menuExit.Click += new EventHandler(this.menuExit_Click);
			contextMenu.MenuItems.Add(menuExit);

			contextMenu.Popup += new EventHandler(contextMenu_Popup);
		}

        /// <summary>
        /// タスクトレイのアイコン作成
        /// </summary>
        private void CreateNotifyIcon()
		{
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
			notifyIcon.Icon = new System.Drawing.Icon(assembly.GetManifestResourceStream("PSOBBTools.tray.ico"), 16, 16);
			notifyIcon.Text = "PSOBBTools";
			notifyIcon.ContextMenu = contextMenu;
			notifyIcon.DoubleClick += new EventHandler(notifyIcon_DoubleClick);
		}

        /// <summary>
        /// 監視設定
        /// </summary>
        private void CreateWatcher()
		{
            // チームチャットログ監視
            logWatcher.Filter = Settings.teamChatFilePrefix + "*" + Settings.teamChatFileExtension;
			logWatcher.NotifyFilter = NotifyFilters.LastWrite;
			logWatcher.Changed += new FileSystemEventHandler(LogFile_Changed);

			// SS監視
            bmpWatcher.Filter = Settings.ssFileFilter;
			bmpWatcher.Created += new FileSystemEventHandler(BmpFile_Created);
		}

        /// <summary>
        /// アイコンのダブルクリック
        /// </summary>
        private void notifyIcon_DoubleClick(object sender, System.EventArgs e)
		{
            ShowChatLog();
		}

        /// <summary>
        /// コンテキストメニューのポップアップ
        /// </summary>
        private void contextMenu_Popup(object sender, System.EventArgs e)
		{
			menuTeamChime.Checked = settings.TeamChimeEnabled;
			menuSSCompression.Checked = settings.SSCompressionEnabled;
			menuSystemButtons.Checked = settings.SystemButtonsEnabled;
            menuWindowAutoRestore.Checked = settings.WindowAutoRestoreEnabled;

            menuWindowSave.Enabled = menuWindowRestore.Enabled = WindowExists();
        }

        /// <summary>
        /// チャットログ
        /// </summary>
        private void menuChatLog_Click(object sender, System.EventArgs e)
        {
            ShowChatLog();
        }

        /// <summary>
        /// マグタイマー
        /// </summary>
        private void menuMagTimer_Click(object sender, System.EventArgs e)
        {
            ShowMagTimer();
        }

        /// <summary>
        /// ウィンドウの位置とサイズを保存
        /// </summary>
        private void menuWindowSave_Click(object sender, System.EventArgs e)
        {
            SaveWindow();
        }

        /// <summary>
        /// ウィンドウの位置とサイズを復元
        /// </summary>
        private void menuWindowRestore_Click(object sender, System.EventArgs e)
        {
            RestoreWindow(settings.WindowPosition, settings.WindowSize);
        }

        /// <summary>
        /// チームチャットのチャイム
        /// </summary>
        private void menuTeamChime_Click(object sender, System.EventArgs e)
		{
			settings.TeamChimeEnabled = !settings.TeamChimeEnabled;
		}

        /// <summary>
        /// SSの自動圧縮
        /// </summary>
        private void menuSSCompression_Click(object sender, System.EventArgs e)
		{
			settings.SSCompressionEnabled = !settings.SSCompressionEnabled;
		}

        /// <summary>
        /// システムボタンの表示
        /// </summary>
        private void menuSystemButtons_Click(object sender, System.EventArgs e)
		{
			settings.SystemButtonsEnabled = !settings.SystemButtonsEnabled;
		}

        /// <summary>
        /// 自動的にウィンドウの位置とサイズを復元
        /// </summary>
        private void menuWindowAutoRestore_Click(object sender, System.EventArgs e)
		{
            settings.WindowAutoRestoreEnabled = !settings.WindowAutoRestoreEnabled;
		}

        /// <summary>
        /// logフォルダを開く
        /// </summary>
        private void menuOpenFolderLog_Click(object sender, System.EventArgs e)
		{
            OpenFolder(settings.PSOBBFolder + Path.DirectorySeparatorChar + Settings.logFolder);
		}

        /// <summary>
        /// bmpフォルダを開く
        /// </summary>
        private void menuOpenFolderBmp_Click(object sender, System.EventArgs e)
		{
            OpenFolder(settings.PSOBBFolder + Path.DirectorySeparatorChar + Settings.bmpFolder);
        }

        /// <summary>
        /// 設定
        /// </summary>
		private void menuSetting_Click(object sender, System.EventArgs e)
		{
			ShowSettings();
		}

        /// <summary>
        /// 終了
        /// </summary>
        private void menuExit_Click(object sender, System.EventArgs e)
		{
            // チャットログの表示状態を保存
            if (formChatLog != null && !formChatLog.IsDisposed)
            {
                settings.ChatLogVisible = formChatLog.Visible;
            }
            else
            {
                settings.ChatLogVisible = false;
            }

            // マグタイマーの表示状態を保存
            if (formMagTimer != null && !formMagTimer.IsDisposed)
            {
                settings.MagTimerVisible = formMagTimer.Visible;
            }
            else
            {
                settings.MagTimerVisible = false;
            }
            
            Application.Exit();
		}

		private void windowTimer_Tick(object sender, EventArgs e)
		{
            if (!WindowExists())
            {
                // ウィンドウが無くなっていたら、次回起動時のためにtrueに戻す
                firstTime = true;

                return;
            }

            if (settings.SystemButtonsEnabled)
            {
                AddSystemButton();
            }

            if (settings.WindowAutoRestoreEnabled && firstTime)
            {
                RestoreWindow(settings.WindowPosition, settings.WindowSize);
            }

            firstTime = false;
		}

        /// <summary>
        /// システムボタンを追加
        /// </summary>
        private void AddSystemButton()
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
        /// ウィンドウの位置とサイズを保存
        /// </summary>
        private void SaveWindow()
        {
            IntPtr hwnd = Window.FindWindow(Settings.windowClassName, null);

            if (hwnd == IntPtr.Zero)
            {
                MessageBox.Show("PSOBBのウィンドウが見つかりません。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Window.RECT rect;

                if (Window.GetWindowRect(hwnd, out rect))
                {
                    settings.WindowPosition = new Point(rect.left, rect.top);
                }
                else
                {
                    MessageBox.Show("ウィンドウ位置の取得に失敗しました。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (Window.GetClientRect(hwnd, out rect))
                {
                    settings.WindowSize = new Size(rect.right, rect.bottom);
                }
                else
                {
                    MessageBox.Show("ウィンドウサイズの取得に失敗しました。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// ウィンドウの位置とサイズを復元
        /// </summary>
        private void RestoreWindow(Point position, Size size)
        {
            IntPtr hwnd = Window.FindWindow(Settings.windowClassName, null);

            if (hwnd == IntPtr.Zero)
            {
                MessageBox.Show("PSOBBのウィンドウが見つかりません。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Window.WindowStyleFlags style = (Window.WindowStyleFlags)Window.GetWindowLong(hwnd, Window.GetWindowLongFlags.GWL_STYLE);
                int exStyle = Window.GetWindowLong(hwnd, Window.GetWindowLongFlags.GWL_EXSTYLE);
                Window.RECT rect;

                rect.left = rect.top = 0;
                rect.right = size.Width;
                rect.bottom = size.Height;

                if (Window.AdjustWindowRectEx(ref rect, style, false, (uint)exStyle))
                {
                    Window.SetWindowPos(hwnd, IntPtr.Zero, position.X, position.Y, rect.right - rect.left, rect.bottom - rect.top,
                        Window.SetWindowPosFlags.SWP_NOACTIVATE |
                        Window.SetWindowPosFlags.SWP_NOOWNERZORDER |
                        Window.SetWindowPosFlags.SWP_NOZORDER);
                }
            }
        }

        /// <summary>
        /// PSOBBのウィンドウが存在するか調べる
        /// </summary>
        private bool WindowExists()
		{
			IntPtr hwnd = Window.FindWindow(Settings.windowClassName, null);

            return (hwnd != IntPtr.Zero);
        }

        /// <summary>
        /// チャットログウィンドウ表示
        /// </summary>
        private void ShowChatLog()
		{
            // すでに表示されているか？
            if (formChatLog == null || formChatLog.IsDisposed)
			{
                // チャットログウィンドウ表示
                formChatLog = new FormChatLog(settings);
			    formChatLog.Show();
			}
			else
			{
                // アクティブにする
                formChatLog.Activate();
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
                formMagTimer = new FormMagTimer(settings);
			    formMagTimer.Show();
			}
			else
			{
                // アクティブにする
                formMagTimer.Activate();
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

				if (settings.SSFileFormat == Settings.CompressionFormats.png)
				{
					cf.FileFormat = "png";
				}
				else
				{
					cf.FileFormat = "jpg";
				}

                lock (((ICollection)convertFileList).SyncRoot)
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
		/// フォルダを開く
		/// </summary>
        private void OpenFolder(string path)
        {
            System.Diagnostics.Process.Start("explorer.exe", "/n," + path);
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
                filePath = Settings.settingsFolder + Path.DirectorySeparatorChar + Settings.settingsFile;
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
                filePath = Settings.settingsFolder + Path.DirectorySeparatorChar + Settings.settingsFile;

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

        /// <summary>
        /// 設定が変更されたイベント
        /// </summary>
        private void settings_Changed(object sender, EventArgs e)
        {
            ApplySettings();
        }

        /// <summary>
        /// 設定の適用
        /// </summary>
        private void ApplySettings()
		{
			windowTimer.Enabled = (settings.SystemButtonsEnabled || settings.WindowAutoRestoreEnabled);

            // 一旦監視を停止
            logWatcher.EnableRaisingEvents = bmpWatcher.EnableRaisingEvents = false;

			if (!String.IsNullOrEmpty(settings.PSOBBFolder) && Directory.Exists(settings.PSOBBFolder))
			{
				logWatcher.Path = settings.PSOBBFolder + Path.DirectorySeparatorChar + Settings.logFolder;
				bmpWatcher.Path = settings.PSOBBFolder + Path.DirectorySeparatorChar + Settings.bmpFolder;
				logWatcher.EnableRaisingEvents = settings.TeamChimeEnabled;
				bmpWatcher.EnableRaisingEvents = settings.SSCompressionEnabled;
			}
		}
	}
}
