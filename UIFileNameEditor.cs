// http://www.codeproject.com/vb/net/UIFilenameEditor.asp
using System;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms;

namespace PSOBBTools
{
	/// <summary>
	/// フィルタが指定できるFileNameEditor
	/// </summary>
	public class UIFileNameEditor : System.Drawing.Design.UITypeEditor
	{
		public UIFileNameEditor()
		{
		}
	
		public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
		{
			if (context != null && context.Instance != null) 
			{
				return UITypeEditorEditStyle.Modal;
			}
			return UITypeEditorEditStyle.None;
		}
	
		[RefreshProperties(RefreshProperties.All)]
		public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (context == null || provider == null || context.Instance == null) 
			{
				return base.EditValue(provider, value);
			}

			using (FileDialog fileDlg = new OpenFileDialog())
			{
				fileDlg.FileName = (string)value;
				fileDlg.CheckFileExists	= true;
				fileDlg.CheckPathExists = true;

				FileDialogFilterAttribute filterAtt = (FileDialogFilterAttribute)context.PropertyDescriptor.Attributes[typeof(FileDialogFilterAttribute)];

				if (filterAtt != null) 
				{
					fileDlg.Filter = filterAtt.Filter;
				}

				if (fileDlg.ShowDialog() == DialogResult.OK)
				{
					value = fileDlg.FileName;
				}
			}

			return value;
		}
	}

	/// <summary>
	/// フィルタ属性
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class FileDialogFilterAttribute : Attribute
	{
		private string filter;

		public FileDialogFilterAttribute(string filter) : base()
		{
			this.filter = filter;
		}

		public string Filter
		{
			get
			{
				return filter;
			}
		}
	}
}
