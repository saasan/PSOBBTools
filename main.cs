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
		// アプリケーション固定名
		private static string mutexName = Settings.applicationName;
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
			catch (ApplicationException e)
			{
				// グローバル・ミューテックスによる多重起動禁止
				MessageBox.Show("すでに起動しています。", Settings.applicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			// ミューテックスを取得する
			if (mutexObject.WaitOne(0, false))
			{
				// アプリケーションを実行
				using (Main main = new Main()) 
				{
					Application.Run();
				}

				// ミューテックスを解放する
				mutexObject.ReleaseMutex();
			}
			else
			{
				// 警告を表示して終了
				MessageBox.Show("すでに起動しています。", Settings.applicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			// ミューテックスを破棄する
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

		// PSOBBのウィンドウクラス名
		private const string PSOBB_CLASS = "PHANTASY STAR ONLINE Blue Burst";

		// 設定
		private Settings settings = new Settings();

		// メニュー
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
		// タスクトレイのアイコン
		private NotifyIcon notifyIcon = new NotifyIcon();
		// ログ監視
		private FileSystemWatcher logWatcher = new FileSystemWatcher();
		// SS監視
		private FileSystemWatcher bmpWatcher = new FileSystemWatcher();
		// システムボタン用タイマ
		private Timer systemButtonTimer = new Timer();

		// 設定ウィンドウ
		private Form formSettings = null;
		// マグタイマーウィンドウ
		private Form formMagTimer = null;

		// 変換するSSリスト
		private Queue convertFileList = Queue.Synchronized(new Queue());
		// ThreadingImageConverterスレッド
		private System.Threading.Thread ImageConverterThread;
		// ThreadingImageConverter
		private ThreadingImageConverter imageConverter;
		
		public Main()
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
				MessageBox.Show("初期化処理に失敗しました。\n詳細：" + e.Message, Settings.applicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		// コンテキストメニュー作成
		private void CreateMenu() 
		{
			menuMagTimer.Text = "マグタイマー(&M)";
			menuMagTimer.DefaultItem = true;
			menuMagTimer.Click += new EventHandler(this.menuMagTimer_Click);
			contextMenu.MenuItems.Add(menuMagTimer);

			menuLine1.Text = "-";
			contextMenu.MenuItems.Add(menuLine1);

			menuTeamChime.Text = "チームチャットのチャイム(&T)";
			menuTeamChime.Click += new EventHandler(this.menuTeamChime_Click);
			contextMenu.MenuItems.Add(menuTeamChime);

			menuCommand.Text = "コマンド(&C)";
			menuCommand.Click += new EventHandler(this.menuCommand_Click);
			contextMenu.MenuItems.Add(menuCommand);

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
			menuCommand.Checked = settings.CommandEnabled;
			menuSSCompression.Checked = settings.SSCompressionEnabled;
			menuSystemButtons.Checked = settings.SystemButtonsEnabled;
		}

		private void menuTeamChime_Click(object sender, System.EventArgs e)
		{
			// 変更結果の反映
			settings.TeamChimeEnabled = !settings.TeamChimeEnabled;

			// 監視の有効化/無効化
			logWatcher.EnableRaisingEvents = (settings.TeamChimeEnabled || settings.CommandEnabled);
		}

		private void menuCommand_Click(object sender, System.EventArgs e)
		{
			// 変更結果の反映
			settings.CommandEnabled = !settings.CommandEnabled;

			// 監視の有効化/無効化
			logWatcher.EnableRaisingEvents = (settings.TeamChimeEnabled || settings.CommandEnabled);
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
			// 設定ウィンドウ表示
			if (formSettings == null)
			{
				formSettings = new FormSettings(settings);
				formSettings.ShowDialog();
				formSettings.Dispose();
				formSettings = null;

				// 設定の反映
				ValidateSettings();
			}
			else
			{
				formSettings.Activate();
			}
		}
	
		private void ShowMagTimer()
		{
			// マグタイマーウィンドウ表示
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

			// コマンド
			if (settings.CommandEnabled && File.Exists(e.FullPath))
			{
				// コマンド処理
				cmd(e.FullPath);
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

		// コマンド処理
		private void cmd(string file)
		{
			string line = "";
			string lastLine = "";

			try 
			{
				// 最終行を読み取る
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

			// 最終行から名前とメッセージを抽出
			string name = "";
			string message = "";
			string [] split = null;

			split = lastLine.Split('\t');
			switch (split.Length)
			{
				case 3:
					// チームチャットの場合
					name = split[1];
					message = split[2];
					break;

				case 4:
					// 通常チャットの場合(一応対策)
					name = split[2];
					message = split[3];
					break;

				default:
					break;
			}

			// 名前チェックOKなら実行
			if (name == settings.ExecName)
			{
				CompileInvoke.Exec(Application.StartupPath + @"\command\" + message + ".cs");
			}
		}

		/// <summary>
		/// 設定の読み込み
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
					MessageBox.Show("設定の読み込み中にエラーが発生しました。\n設定ファイルとプログラムのバージョンがあっているか確認してください。\n詳細：" + e.Message, Settings.applicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		/// <summary>
		/// 設定の保存
		/// </summary>
		private void Save()
		{
			System.IO.Stream stream = System.IO.File.Create(@Settings.settingFile);
			SoapFormatter soapFormatter = new SoapFormatter();
			soapFormatter.Serialize(stream, settings);
			stream.Close(); 
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
				logWatcher.EnableRaisingEvents = (settings.TeamChimeEnabled || settings.CommandEnabled);
				bmpWatcher.EnableRaisingEvents = settings.SSCompressionEnabled;
			}
			else
			{
				MessageBox.Show("PSOBBのフォルダが見つかりません。設定を確認してください。", Settings.applicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				settings.TeamChimeEnabled = false;
				settings.CommandEnabled = false;
				settings.SSCompressionEnabled = false;
				logWatcher.EnableRaisingEvents = false;
				bmpWatcher.EnableRaisingEvents = false;
			}
		}
	}
}
