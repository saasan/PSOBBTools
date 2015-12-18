using System;
using System.ComponentModel;
using System.Design;
using System.Runtime.Serialization;

namespace PSOBBTools
{
	/// <summary>
	/// アプリケーションの設定
	/// </summary>
	[TypeConverter(typeof(PropertyDisplayConverter))]
	public class Settings
	{
        /// <summary>設定ファイル名</summary>
        public const string settingsFile = "settings.xml";
        /// <summary>設定ファイルを保存するフォルダ名</summary>
        public static readonly string settingsFolder;
        /// <summary>旧設定ファイル名</summary>
        public const string oldSettingsFile = "PSOBBTools.xml";
        /// <summary>ログフォルダ名</summary>
		public const string logFolder = "log";
        /// <summary>SSフォルダ名</summary>
		public const string ssFolder = "bmp";
        /// <summary>PSOBBのウィンドウクラス名</summary>
        public const string windowClassName = "PHANTASY STAR ONLINE Blue Burst";

        /// <summary>マグタイマーの時間の最小値</summary>
        public const decimal magTimerTimeMin = 1;
        /// <summary>マグタイマーの時間の最大値</summary>
        public const decimal magTimerTimeMax = 600;

        /// <summary>SSファイルの形式</summary>
		public enum SSFileFormats
		{
			png,
			jpg
		}

        /// <summary>PSOBBのフォルダ</summary>
		private string psobbFolder = @"C:\Program Files\SEGA\PHANTASY STAR ONLINE Blue Burst";
        // チームチャットのログファイル名</summary>
		private string teamChatFileFilter = "GuildChat*.txt";

        /// <summary>チームチャットのチャイムON/OFF</summary>
		private bool teamChimeEnabled = false;
        /// <summary>SSの自動圧縮ON/OFF</summary>
		private bool ssCompressionEnabled = false;
        /// <summary>システムボタンON/OFF</summary>
		private bool systemButtonsEnabled = false;

        /// <summary>チャイム音のファイル</summary>
		private string chimeFile = "pon.wav";

        /// <summary>SSのファイル名</summary>
		private string ssFileFilter = "pso*.bmp";
        /// <summary>SSの圧縮形式</summary>
		private SSFileFormats ssFileFormat = SSFileFormats.jpg;

        /// <summary>マグタイマー音のファイル</summary>
		private string magTimerFile = "popin.wav";
        /// <summary>マグタイマーの時間</summary>
		private long magTimerTime = 210;
        /// <summary>マグタイマーの繰り返し</summary>
		private bool magTimerReload = false;
        /// <summary>マグタイマーを常に手前に表示</summary>
		private bool magTimerTopMost = false;

        /// <summary>マグタイマーウィンドウの位置</summary>
		private System.Drawing.Point magTimerLocation = new System.Drawing.Point(50, 50);

        /// <summary>変更イベント</summary>
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
        /// 変更イベントを発生させる
        /// </summary>
        /// <param name="e">イベントデータ</param>
        void OnChanged(EventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
            }
        }
        
        /// <summary>
        /// PSOBBのフォルダ
        /// </summary>
		[Category("全般"),
			PropertyDisplayName("PSOBBのフォルダ"),
			Description("PSOBBのフォルダ(PsoBB.exe等があるフォルダ)を設定します。"),
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
        /// チームチャットのログファイル名
        /// </summary>
		[Category("全般"),
			PropertyDisplayName("ログファイル名(チーム)"),
			Description("監視対象とするチームチャットのログファイル名を設定します。")]
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
        /// チームチャットのチャイムON/OFF
        /// </summary>
		[Category("全般"),
			PropertyDisplayName("チームチャットのチャイム"),
            Description("チームチャットのチャイムを使用するかどうかを設定します。")]
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
        /// SSの自動圧縮ON/OFF
        /// </summary>
		[Category("全般"),
			PropertyDisplayName("SSの自動圧縮"),
            Description("スクリーンショットを自動圧縮するかどうかを設定します。")]
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
        /// システムボタンON/OFF
        /// </summary>
		[Category("全般"),
			PropertyDisplayName("システムボタンの表示"),
            Description("システムボタンを表示するかどうかを設定します。")]
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
        /// チャイム音のファイル
        /// </summary>
		[Category("チームチャットのチャイム"),
			PropertyDisplayName("チャイム用WAVファイル"),
			Description("チャイムとして使用するWAVファイルを設定します。"),
			Editor(typeof(UIFileNameEditor),
			    typeof(System.Drawing.Design.UITypeEditor)),
			FileDialogFilter("WAVファイル (*.wav)|*.wav")]
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
        /// SSのファイル名
        /// </summary>
		[Category("SSの自動圧縮"),
			PropertyDisplayName("SSのファイル名"),
			Description("監視対象とするスクリーンショットのファイル名を設定します。")]
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
        /// SSの圧縮形式
        /// </summary>
		[Category("SSの自動圧縮"),
			PropertyDisplayName("SSの圧縮形式"),
			Description("スクリーンショットの圧縮形式を設定します。")]
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
        /// マグタイマー音のファイル
        /// </summary>
		[Category("マグタイマー"),
		PropertyDisplayName("マグタイマー用WAVファイル"),
			Description("マグタイマーの音として使用するWAVファイルを設定します。"),
			Editor(typeof(UIFileNameEditor),
			    typeof(System.Drawing.Design.UITypeEditor)),
			FileDialogFilter("WAVファイル (*.wav)|*.wav")]
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
        /// マグタイマーの時間
        /// </summary>
		[Category("マグタイマー"),
			PropertyDisplayName("マグタイマーの時間(秒)"),
			Description("マグタイマーの時間(秒)を設定します。")]
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
                    System.Windows.Forms.MessageBox.Show("マグタイマーの時間には" + magTimerTimeMin + "から" + magTimerTimeMax + "の間の整数を指定してください。", System.Windows.Forms.Application.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				}
			}
		}

        /// <summary>
        /// マグタイマーの繰り返し
        /// </summary>
		[Category("マグタイマー"),
			PropertyDisplayName("マグタイマーの繰り返し"),
			Description("マグタイマーの繰り返しを設定します。")]
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
        /// マグタイマーを常に手前に表示
        /// </summary>
        [Category("マグタイマー"),
            PropertyDisplayName("マグタイマーを常に手前に表示"),
            Description("マグタイマーを常に手前に表示するかどうかを設定します。")]
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
        /// マグタイマーウィンドウの位置
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