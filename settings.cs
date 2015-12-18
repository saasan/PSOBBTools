using System;
using System.ComponentModel;
using System.Design;
using System.Runtime.Serialization;

namespace PSOBBTools
{
	/// <summary>
	/// アプリケーションの設定
	/// </summary>
	[Serializable, TypeConverter(typeof(PropertyDisplayConverter))]
	public class Settings
	{
		// アプリケーション名
		public const string applicationName = "PSOBBTools";
		// 設定ファイル名
		public const string settingFile = "PSOBBTools.xml";
		// ログフォルダ名
		public const string logFolder = "log";
		// SSフォルダ名
		public const string ssFolder = "bmp";

		// SSファイルの形式
		public enum SSFileFormats
		{
			png,
			jpg
		}

		// PSOBBのフォルダ
		private string psobbFolder = @"C:\Program Files\SEGA\PHANTASY STAR ONLINE Blue Burst";
		// チームチャットのログファイル名
		private string teamChatFileFilter = "GuildChat*.txt";

		// チームチャットのチャイムON/OFF
		private bool teamChimeEnabled = false;
		// コマンドON/OFF
		private bool commandEnabled = false;
		// SSの自動圧縮ON/OFF
		private bool ssCompressionEnabled = false;
		// システムボタンON/OFF
		private bool systemButtonsEnabled = false;

		// チャイム音のファイル
		private string chimeFile = "pon.wav";

		// コマンドの実行を許可するキャラ名
		private string execName = "";

		// SSのファイル名
		private string ssFileFilter = "pso*.bmp";
		// SSの圧縮形式
		private SSFileFormats ssFileFormat = SSFileFormats.jpg;

		// マグタイマー音のファイル
		private string magTimerFile = "popin.wav";
		// マグタイマーの時間
		private long magTimerTime = 210;
		// マグタイマーの繰り返し
		private bool magTimerReload = false;

		// マグタイマーウィンドウの位置
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

		[Category("全般"),
			PropertyDisplayName("PSOBBのフォルダ"),
			Description("PSOBBのフォルダ(PsoBB.exe等があるフォルダ)を指定します。"),
			Editor(typeof(System.Windows.Forms.Design.FolderNameEditor),
			typeof(System.Drawing.Design.UITypeEditor))]
		public string PSOBBFolder
		{
			get { return psobbFolder; }
			set { psobbFolder = value; }
		}

		[Category("全般"),
			PropertyDisplayName("ログファイル名(チーム)"),
			Description("監視対象とするチームチャットのログファイル名を指定します。")]
		public string TeamChatFileFilter
		{
			get { return teamChatFileFilter; }
			set { teamChatFileFilter = value; }
		}

		[Category("全般"),
			PropertyDisplayName("チームチャットのチャイム"),
			Description("チームチャットのチャイムの使用を切り替えます。")]
		public bool TeamChimeEnabled
		{
			get { return teamChimeEnabled; }
			set { teamChimeEnabled = value; }
		}

		[Category("全般"),
			PropertyDisplayName("コマンドの使用"),
			Description("コマンドの使用を切り替えます。")]
		public bool CommandEnabled
		{
			get { return commandEnabled; }
			set { commandEnabled = value; }
		}

		[Category("全般"),
			PropertyDisplayName("SSの自動圧縮"),
			Description("スクリーンショットの自動圧縮の使用を切り替えます。")]
		public bool SSCompressionEnabled
		{
			get { return ssCompressionEnabled; }
			set { ssCompressionEnabled = value; }
		}

		[Category("全般"),
			PropertyDisplayName("システムボタンの表示"),
			Description("システムボタンの表示を切り替えます。")]
		public bool SystemButtonsEnabled
		{
			get { return systemButtonsEnabled; }
			set { systemButtonsEnabled = value; }
		}

		[Category("チームチャットのチャイム"),
			PropertyDisplayName("チャイム用WAVファイル"),
			Description("チャイムとして使用するWAVファイルを指定します。"),
			Editor(typeof(UIFileNameEditor),
			typeof(System.Drawing.Design.UITypeEditor)),
			FileDialogFilter("WAVファイル (*.wav)|*.wav")]
		public string ChimeFile
		{
			get { return chimeFile; }
			set { chimeFile = value; }
		}

		[Category("コマンド"),
			PropertyDisplayName("コマンド許可キャラ名"),
			Description("コマンドの実行を許可するキャラクター名を指定します。")]
		public string ExecName
		{
			get { return execName; }
			set { execName = value; }
		}

		[Category("SSの自動圧縮"),
			PropertyDisplayName("SSのファイル名"),
			Description("監視対象とするスクリーンショットのファイル名を指定します。")]
		public string SSFileFilter
		{
			get { return ssFileFilter; }
			set { ssFileFilter = value; }
		}

		[Category("SSの自動圧縮"),
			PropertyDisplayName("SSの圧縮形式"),
			Description("スクリーンショットの圧縮形式を指定します。")]
		public SSFileFormats SSFileFormat
		{
			get { return ssFileFormat; }
			set { ssFileFormat = value; }
		}

		[Category("マグタイマー"),
		PropertyDisplayName("マグタイマー用WAVファイル"),
			Description("マグタイマーの音として使用するWAVファイルを指定します。"),
			Editor(typeof(UIFileNameEditor),
			typeof(System.Drawing.Design.UITypeEditor)),
			FileDialogFilter("WAVファイル (*.wav)|*.wav")]
		public string MagTimerFile
		{
			get { return magTimerFile; }
			set { magTimerFile = value; }
		}

		[Category("マグタイマー"),
			PropertyDisplayName("マグタイマーの時間(秒)"),
			Description("マグタイマーの時間(秒)を指定します。")]
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
					System.Windows.Forms.MessageBox.Show("マグタイマーの時間には整数を指定してください。", applicationName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				}
			}
		}

		[Category("マグタイマー"),
			PropertyDisplayName("マグタイマーの繰り返し"),
			Description("マグタイマーの繰り返しを指定します。")]
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