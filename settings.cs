using System;
using System.ComponentModel;
using System.Design;
using System.Runtime.Serialization;
using System.Drawing;
using System.Windows.Forms;

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
        /// <summary>logフォルダ名</summary>
		public const string logFolder = "log";
        /// <summary>bmpフォルダ名</summary>
		public const string bmpFolder = "bmp";
        /// <summary> チャットのログファイルの接頭辞</summary>
        public const string chatFilePrefix = "chat";
        /// <summary> チャットのログファイルの拡張子</summary>
        public const string chatFileExtension = ".txt";
        /// <summary> チームチャットのログファイルの接頭辞</summary>
        public const string teamChatFilePrefix = "GuildChat";
        /// <summary> チームチャットのログファイルの拡張子</summary>
        public const string teamChatFileExtension = ".txt";
        /// <summary>SSのファイル名</summary>
        public const string ssFileFilter = "pso*.bmp";

        /// <summary>PSOBBのウィンドウクラス名</summary>
        public const string windowClassName = "PHANTASY STAR ONLINE Blue Burst";

        /// <summary>マグタイマーの時間の最小値</summary>
        public const decimal magTimerTimeMin = 1;
        /// <summary>マグタイマーの時間の最大値</summary>
        public const decimal magTimerTimeMax = 600;

        /// <summary>圧縮形式</summary>
		public enum CompressionFormats
		{
			png,
			jpg
		}

        /// <summary>ウィンドウサイズのタイプ</summary>
        public enum WindowSizeTypes
        {
            w640h480,
            w800h600,
            w1024h768,
            custom
        }

        /// <summary>PSOBBのフォルダ</summary>
		private string psobbFolder = @"C:\Program Files\SEGA\PHANTASY STAR ONLINE Blue Burst";

        /// <summary>チームチャットのチャイムON/OFF</summary>
		private bool teamChimeEnabled = false;
        /// <summary>SSの自動圧縮ON/OFF</summary>
		private bool ssCompressionEnabled = false;
        /// <summary>システムボタンON/OFF</summary>
		private bool systemButtonsEnabled = false;

        /// <summary>チャイム音のファイル</summary>
		private string chimeFile = "pon.wav";

        /// <summary>SSの圧縮形式</summary>
		private CompressionFormats ssFileFormat = CompressionFormats.jpg;

        /// <summary>マグタイマー音のファイル</summary>
		private string magTimerFile = "popin.wav";
        /// <summary>マグタイマーの時間</summary>
		private long magTimerTime = 210;
        /// <summary>マグタイマーの繰り返し</summary>
		private bool magTimerReload = false;
        /// <summary>マグタイマーを常に手前に表示</summary>
		private bool magTimerTopMost = false;

        /// <summary>マグタイマーウィンドウを表示</summary>
        private bool magTimerVisible = false;
        /// <summary>マグタイマーウィンドウの位置</summary>
		private Point magTimerLocation = new Point(50, 50);

        /// <summary>チャットログウィンドウにチームチャットログを表示</summary>
        private bool chatLogTeamVisible = true;

        /// <summary>チャットログウィンドウを表示</summary>
        private bool chatLogVisible = false;
        /// <summary>チャットログウィンドウの位置</summary>
        private Point chatLogLocation = new Point(50, 50);
        /// <summary>チャットログウィンドウのサイズ</summary>
        private Size chatLogSize = new Size(640, 480);
        /// <summary>チャットログウィンドウの分割線までの距離</summary>
        private int chatLogSplitterDistance = 200;
        /// <summary>チャットログのソート順</summary>
        private SortOrder chatLogSortOrder = SortOrder.Ascending;
        /// <summary>チームチャットログのソート順</summary>
        private SortOrder chatLogTeamSortOrder = SortOrder.Ascending;
        /// <summary>チャットログの自動スクロール</summary>
        private bool chatLogAutoScroll = true;

        /// <summary>ウィンドウサイズのタイプ</summary>
        private WindowSizeTypes windowResizeType = WindowSizeTypes.custom;
        /// <summary>ウィンドウサイズの変更の幅</summary>
        private decimal windowResizeWidth = 640;
        /// <summary>ウィンドウサイズの変更の高さ</summary>
        private decimal windowResizeHeight = 480;

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
        protected virtual void OnChanged(EventArgs e)
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
			Description("PSOBBのフォルダ(PsoBB.exe等があるフォルダ)を設定します。PSOBBのインストール先を変更している場合はこの値を変更してください。"),
			Editor(typeof(System.Windows.Forms.Design.FolderNameEditor),
                typeof(System.Drawing.Design.UITypeEditor))]
		public string PSOBBFolder
		{
			get { return psobbFolder; }
			set
            {
                if (!String.IsNullOrEmpty(value) && System.IO.Directory.Exists(value))
                {
                    psobbFolder = value;
                    OnChanged(EventArgs.Empty);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("PSOBBのフォルダが存在しません。", System.Windows.Forms.Application.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
		}

        /// <summary>
        /// チームチャットのチャイムON/OFF
        /// </summary>
		[Category("全般"),
			PropertyDisplayName("チームチャットのチャイム"),
            Description("チームチャットのチャイムを鳴らすかどうかを設定します。(True=鳴らす、False=鳴らさない)")]
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
            Description("スクリーンショットを自動圧縮するかどうかを設定します。(True=する、False=しない)")]
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
            Description("システムボタンを表示するかどうかを設定します。(True=する、False=しない)")]
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
        /// SSの圧縮形式
        /// </summary>
		[Category("SSの自動圧縮"),
			PropertyDisplayName("SSの圧縮形式"),
			Description("スクリーンショットの圧縮形式を設定します。")]
		public CompressionFormats SSFileFormat
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
            Description("マグタイマーの繰り返しを設定します。(True=繰り返す、False=繰り返さない)")]
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
            Description("マグタイマーを常に手前に表示するかどうかを設定します。(True=する、False=しない)")]
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
        /// マグタイマーウィンドウを表示
        /// </summary>
		[Browsable(false)]
        public bool MagTimerVisible
		{
            get { return magTimerVisible; }
            set { magTimerVisible = value; }
		}

        /// <summary>
        /// マグタイマーウィンドウの位置
        /// </summary>
		[Browsable(false)]
		public Point MagTimerLocation
		{
			get { return magTimerLocation; }
			set { magTimerLocation = value; }
		}

        /// <summary>
        /// チャットログウィンドウにチームチャットログを表示
        /// </summary>
        [Category("チャットログ"),
            PropertyDisplayName("チームチャットログを表示"),
            Description("チャットログウィンドウにチームチャットログを表示するかどうかを設定します。(True=する、False=しない)")]
        public bool ChatLogTeamVisible
        {
            get { return chatLogTeamVisible; }
            set
            {
                chatLogTeamVisible = value;
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// チャットログの自動スクロール
        /// </summary>
        [Category("チャットログ"),
            PropertyDisplayName("チャットログの自動スクロール"),
            Description("チャットログを自動的にスクロールするかどうかを設定します。(True=する、False=しない)")]
        public bool ChatLogAutoScroll
        {
            get { return chatLogAutoScroll; }
            set
            {
                chatLogAutoScroll = value;
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// チャットログウィンドウを表示
        /// </summary>
        [Browsable(false)]
        public bool ChatLogVisible
        {
            get { return chatLogVisible; }
            set { chatLogVisible = value; }
        }

        /// <summary>
        /// チャットログウィンドウの位置
        /// </summary>
		[Browsable(false)]
        public Point ChatLogLocation
		{
			get { return chatLogLocation; }
			set { chatLogLocation = value; }
		}

        /// <summary>
        /// チャットログウィンドウのサイズ
        /// </summary>
        [Browsable(false)]
        public Size ChatLogSize
        {
            get { return chatLogSize; }
            set { chatLogSize = value; }
        }

        /// <summary>
        /// チャットログウィンドウの分割線までの距離
        /// </summary>
        [Browsable(false)]
        public int ChatLogSplitterDistance
        {
            get { return chatLogSplitterDistance; }
            set { chatLogSplitterDistance = value; }
        }

        /// <summary>
        /// チャットログのソート順
        /// </summary>
        [Browsable(false)]
        public SortOrder ChatLogSortOrder
        {
            get { return chatLogSortOrder; }
            set { chatLogSortOrder = value; }
        }

        /// <summary>
        /// チームチャットログのソート順
        /// </summary>
        [Browsable(false)]
        public SortOrder ChatLogTeamSortOrder
        {
            get { return chatLogTeamSortOrder; }
            set { chatLogTeamSortOrder = value; }
        }

        /// <summary>
        /// ウィンドウサイズのタイプ
        /// </summary>
        [Browsable(false)]
        public WindowSizeTypes WindowResizeType
        {
            get { return windowResizeType; }
            set { windowResizeType = value; }
        }

        /// <summary>
        /// ウィンドウサイズの変更の幅
        /// </summary>
        [Browsable(false)]
        public decimal WindowResizeWidth
        {
            get { return windowResizeWidth; }
            set { windowResizeWidth = value; }
        }

        /// <summary>
        /// ウィンドウサイズの変更の高さ
        /// </summary>
        [Browsable(false)]
        public decimal WindowResizeHeight
        {
            get { return windowResizeHeight; }
            set { windowResizeHeight = value; }
        }
    }
}