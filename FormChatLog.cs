using System;
using System.IO;
using System.Collections;           // ICollection
using System.Collections.Generic;   // Queue<>
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace PSOBBTools
{
    public partial class FormChatLog : Form
    {
        private Settings settings;

        // Tail
        private Tail logTail = new Tail();
        private Tail teamLogTail = new Tail();

        // リストビューに追加するアイテム
        private Queue<ListViewItem> logItems = new Queue<ListViewItem>();
        private Queue<ListViewItem> teamLogItems = new Queue<ListViewItem>();

        public FormChatLog(Settings settings)
        {
            InitializeComponent();

            this.settings = settings;

            // フォームの状態を復元
            this.Size = settings.ChatLogSize;
            this.Location = settings.ChatLogLocation;
            splitContainer.SplitterDistance = settings.ChatLogSplitterDistance;
            listViewChatLog.Sorting = settings.ChatLogSortOrder;
            listViewTeamChatLog.Sorting = settings.ChatLogTeamSortOrder;

            // Tail
            logTail.Encoding = Encoding.Unicode;
            logTail.Filter = Settings.chatFilePrefix + "*" + Settings.chatFileExtension;
            logTail.Changed += new TailEventArgsHandler(LogFile_Changed);
            teamLogTail.Encoding = Encoding.Unicode;
            teamLogTail.Filter = Settings.teamChatFilePrefix + "*" + Settings.teamChatFileExtension;
            teamLogTail.Changed += new TailEventArgsHandler(TeamLogFile_Changed);

            // 設定の適用
            ApplySettings();

            // ファイルを読み込む
            ReadFile();

            // タイマーを開始
            timerUpdateListView.Enabled = true;

            // リストビューをソート
            listViewChatLog.ListViewItemSorter = new ListViewItemComparer(0, listViewChatLog.Sorting);
            listViewTeamChatLog.ListViewItemSorter = new ListViewItemComparer(0, listViewTeamChatLog.Sorting);

            // 監視を開始
            logTail.EnableRaisingEvents = teamLogTail.EnableRaisingEvents = true;

            // イベントハンドラを追加
            settings.Changed += new EventHandler(settings_Changed);
        }

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            // イベントハンドラを削除
            settings.Changed -= new EventHandler(settings_Changed);

            // 監視を停止
            logTail.EnableRaisingEvents = teamLogTail.EnableRaisingEvents = false;

            // フォームの状態を保存
            settings.ChatLogLocation = this.Location;
            settings.ChatLogSize = this.Size;
            settings.ChatLogSplitterDistance = splitContainer.SplitterDistance;
            settings.ChatLogSortOrder = listViewChatLog.Sorting;
            settings.ChatLogTeamSortOrder = listViewTeamChatLog.Sorting;

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// logフォルダを開く
        /// </summary>
        private void menuFileOpenFolderLog_Click(object sender, EventArgs e)
        {
            OpenFolder(settings.PSOBBFolder + '\\' + Settings.logFolder);
        }

        /// <summary>
        /// 閉じる
        /// </summary>
        private void menuFileClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 編集メニューのドロップダウンイベント
        /// </summary>
        private void menuEdit_DropDownOpened(object sender, EventArgs e)
        {
            menuEditCopyGuildCardId.Enabled = splitContainer.ActiveControl.Equals(listViewChatLog);
        }

        /// <summary>
        /// すべての項目をコピー
        /// </summary>
        private void menuEditCopyAll_Click(object sender, EventArgs e)
        {
            if (splitContainer.ActiveControl is ListView)
            {
                ListView listView = (ListView)splitContainer.ActiveControl;

                string text = "";

                for (int i = 0; i < listView.SelectedItems.Count; i++)
                {
                    if (i != 0)
                    {
                        text += '\n';
                    }

                    for (int j = 0; j < listView.SelectedItems[i].SubItems.Count; j++)
                    {
                        if (j != 0)
                        {
                            text += '\t';
                        }

                        text += listView.SelectedItems[i].SubItems[j].Text;
                    }
                }

                if (!String.IsNullOrEmpty(text))
                {
                    Clipboard.SetText(text);
                }
            }
        }

        /// <summary>
        /// ギルドカードIDをコピー
        /// </summary>
        private void menuEditCopyGuildCardId_Click(object sender, EventArgs e)
        {
            if (splitContainer.ActiveControl.Equals(listViewChatLog))
            {
                ListView listView = (ListView)splitContainer.ActiveControl;

                CopyColumn(listView, 1);
            }
        }

        /// <summary>
        /// 名前をコピー
        /// </summary>
        private void menuEditCopyName_Click(object sender, EventArgs e)
        {
            if (splitContainer.ActiveControl is ListView)
            {
                ListView listView = (ListView)splitContainer.ActiveControl;

                int index;

                if (listView.Equals(listViewChatLog))
                {
                    index = 2;
                }
                else
                {
                    index = 1;
                }

                CopyColumn(listView, index);
            }
        }

        /// <summary>
        /// 発言をコピー
        /// </summary>
        private void menuEditCopyWord_Click(object sender, EventArgs e)
        {
            if (splitContainer.ActiveControl is ListView)
            {
                ListView listView = (ListView)splitContainer.ActiveControl;

                int index;

                if (listView.Equals(listViewChatLog))
                {
                    index = 3;
                }
                else
                {
                    index = 2;
                }

                CopyColumn(listView, index);
            }
        }

        /// <summary>
        /// カラムをコピー
        /// </summary>
        private static void CopyColumn(ListView listView, int index)
        {
            string text = "";

            for (int i = 0; i < listView.SelectedItems.Count; i++)
            {
                if (i != 0)
                {
                    text += '\n';
                }

                text += listView.SelectedItems[i].SubItems[index].Text;
            }

            if (!String.IsNullOrEmpty(text))
            {
                Clipboard.SetText(text);
            }
        }

        /// <summary>
        /// 選択の切り替え
        /// </summary>
        private void menuEditInvertSelection_Click(object sender, EventArgs e)
        {
            if (splitContainer.ActiveControl is ListView)
            {
                ListView listView = (ListView)splitContainer.ActiveControl;

                listView.BeginUpdate();

                foreach (ListViewItem listViewItem in listView.Items)
                {
                    listViewItem.Selected = !listViewItem.Selected;
                }

                listView.EndUpdate();
            }
        }

        /// <summary>
        /// すべて選択
        /// </summary>
        private void menuEditSelectAll_Click(object sender, EventArgs e)
        {
            if (splitContainer.ActiveControl is ListView)
            {
                ListView listView = (ListView)splitContainer.ActiveControl;

                listView.BeginUpdate();

                foreach (ListViewItem listViewItem in listView.Items)
                {
                    listViewItem.Selected = true;
                }

                listView.EndUpdate();
            }
        }

        /// <summary>
        /// クリア
        /// </summary>
        private void menuEditClear_Click(object sender, EventArgs e)
        {
            if (splitContainer.ActiveControl is ListView)
            {
                ListView listView = (ListView)splitContainer.ActiveControl;

                listView.BeginUpdate();

                listView.Items.Clear();

                listView.EndUpdate();
            }
        }

        /// <summary>
        /// 表示メニューのドロップダウンイベント
        /// </summary>
        private void menuView_DropDownOpened(object sender, EventArgs e)
        {
            menuViewTeamChat.Checked = settings.ChatLogTeamVisible;
            menuViewAutoScroll.Checked = settings.ChatLogAutoScroll;
        }

        /// <summary>
        /// チームチャットを表示
        /// </summary>
        private void menuViewTeamChat_Click(object sender, EventArgs e)
        {
            settings.ChatLogTeamVisible = !settings.ChatLogTeamVisible;
        }

        /// <summary>
        /// 自動スクロール
        /// </summary>
        private void menuViewAutoScroll_Click(object sender, EventArgs e)
        {
            settings.ChatLogAutoScroll = !settings.ChatLogAutoScroll;
        }

        /// <summary>
        /// ファイルを読み込む
        /// </summary>
        private void ReadFile()
        {
            DateTime dt = DateTime.Today;

            // 日付
            string date = dt.Year.ToString("d4") + '/' + dt.Month.ToString("d2") + '/' + dt.Day.ToString("d2");
            // ファイル名の日付部分
            string dateString = dt.Year.ToString("d4") + dt.Month.ToString("d2") + dt.Day.ToString("d2");

            // ログフォルダ
            string logFolder = settings.PSOBBFolder + @"\" + Settings.logFolder + @"\";

            // チャットログ
            {
                logTail.FullPath = logFolder + Settings.chatFilePrefix + dateString + Settings.chatFileExtension;

                string content = "";

                try
                {
                    content = logTail.GetDifference();
                }
                catch (Exception) {}

                AddLog(false, date, content);
            }

            // チームチャットログ
            {
                teamLogTail.FullPath = logFolder + Settings.teamChatFilePrefix + dateString + Settings.teamChatFileExtension;

                string content = "";

                try
                {
                    content = teamLogTail.GetDifference();
                }
                catch (Exception) {}

                AddLog(true, date, content);
            }
        }
        
        /// <summary>
        /// ログをキューに追加する
        /// </summary>
        private void AddLog(bool isTeamLog, string date, string content)
        {
            string[] lines = content.Split(new char[] { '\r', '\n' });

            foreach (string line in lines)
            {
                string[] parts = line.Split('\t');

                if (isTeamLog)
                {
                    // チームチャットのログ

                    if (parts.Length == 3)
                    {
                        parts[0] = date + ' ' + parts[0];

                        lock (((ICollection)teamLogItems).SyncRoot)
                        {
                            teamLogItems.Enqueue(new ListViewItem(parts));
                        }
                    }
                }
                else
                {
                    // チャットのログ

                    if (parts.Length == 4)
                    {
                        // 4ならギルドカードID有り
                        parts[0] = date + ' ' + parts[0];

                        lock (((ICollection)logItems).SyncRoot)
                        {
                            logItems.Enqueue(new ListViewItem(parts));
                        }
                    }
                    else if (parts.Length == 3)
                    {
                        // 3ならギルドカードID無し
                        parts[0] = date + ' ' + parts[0];

                        lock (((ICollection)logItems).SyncRoot)
                        {
                            logItems.Enqueue(new ListViewItem(new string[] { parts[0], "", parts[1], parts[2] }));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// リストビューにアイテムを追加する
        /// </summary>
        private void AddListViewItems(ListView listView, Queue<ListViewItem> items)
        {
            lock (((ICollection)items).SyncRoot)
            {
                if (items.Count == 0)
                {
                    return;
                }
            }

            listView.BeginUpdate();

            lock (((ICollection)items).SyncRoot)
            {
                // アイテムを追加
                while (items.Count > 0)
                {
                    ListViewItem item = items.Dequeue();

                    listView.Items.Add(item);
                }
            }

            // カラムの幅を自動調整
            foreach (ColumnHeader ch in listView.Columns)
            {
                ch.Width = -2;
            }

            // ログをスクロール
            if (settings.ChatLogAutoScroll)
            {
                if (listView.Items.Count > 0)
                {
                    if (listView.Sorting != SortOrder.Descending)
                    {
                        listView.TopItem = listView.Items[listView.Items.Count - 1];
                    }
                    else
                    {
                        listView.TopItem = listView.Items[0];
                    }
                }
            }

            listView.EndUpdate();
        }

        /// <summary>
        /// チャットログファイル変更イベント
        /// </summary>
        private void LogFile_Changed(object sender, TailEventArgs e)
        {
            Regex regex = new Regex(Settings.chatFilePrefix + @"(?<1>\d{4})(?<2>\d{2})(?<3>\d{2})\" + Settings.chatFileExtension);
            Match match = regex.Match(e.Name);
            string date = match.Groups[1].Value + '/' + match.Groups[2].Value + '/' + match.Groups[3].Value;

            this.AddLog(false, date, e.Difference);
        }

        /// <summary>
        /// チームチャットログファイル変更イベント
        /// </summary>
        private void TeamLogFile_Changed(object sender, TailEventArgs e)
        {
            Regex regex = new Regex(Settings.teamChatFilePrefix + @"(?<1>\d{4})(?<2>\d{2})(?<3>\d{2})\" + Settings.teamChatFileExtension);
            Match match = regex.Match(e.Name);
            string date = match.Groups[1].Value + '/' + match.Groups[2].Value + '/' + match.Groups[3].Value;

            this.AddLog(true, date, e.Difference);
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
            splitContainer.Panel2Collapsed = !settings.ChatLogTeamVisible;

            // 一旦監視を停止
            bool logTailEnable = logTail.EnableRaisingEvents;
            bool teamLogTailEnable = logTail.EnableRaisingEvents;
            logTail.EnableRaisingEvents = teamLogTail.EnableRaisingEvents = false;

            if (!String.IsNullOrEmpty(settings.PSOBBFolder) && Directory.Exists(settings.PSOBBFolder))
            {
                logTail.Path = teamLogTail.Path = settings.PSOBBFolder + @"\" + Settings.logFolder;

                // 監視を元に戻す
                logTail.EnableRaisingEvents = logTailEnable;
                teamLogTail.EnableRaisingEvents = teamLogTailEnable;
            }
        }

        private void listViewChatLog_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            SortListView(listViewChatLog, e.Column);
        }

        private void listViewTeamChatLog_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            SortListView(listViewTeamChatLog, e.Column);
        }

        /// <summary>
        /// リストビューをソートする
        /// </summary>
        private void SortListView(ListView listView, int column)
        {
            listView.Sorting = (listView.Sorting == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending);

            listView.BeginUpdate();
            listView.ListViewItemSorter = new ListViewItemComparer(column, listView.Sorting);
            listView.EndUpdate();
        }

        /// <summary>
        /// リストビュー更新用タイマーイベント
        /// </summary>
        private void timerUpdateListView_Tick(object sender, EventArgs e)
        {
            AddListViewItems(listViewChatLog, logItems);
            AddListViewItems(listViewTeamChatLog, teamLogItems);
        }

        /// <summary>
        /// フォルダを開く
        /// </summary>
        private void OpenFolder(string path)
        {
            System.Diagnostics.Process.Start("explorer.exe", "/n," + path);
        }
    }
}