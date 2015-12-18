// http://www.atmarkit.co.jp/fdotnet/dotnettips/101compileinvoke/compileinvoke.html
using System;
using System.IO;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

namespace PSOBBTools
{
	/// <summary>
	/// CompileInvoke の概要の説明です。
	/// </summary>
	public class CompileInvoke
	{
		// 指定されたC#ファイルをコンパイル、実行
		public static void Exec(string file)
		{
			try
			{
				if (!File.Exists(file))
				{
					return;
				}

				using (StreamReader reader = new StreamReader(file, System.Text.Encoding.GetEncoding("Shift_JIS"))) 
				{
					string text = reader.ReadToEnd();
					reader.Close();

					CSharpCodeProvider cscp = new CSharpCodeProvider();
					ICodeCompiler cc = cscp.CreateCompiler();

					CompilerParameters param = new CompilerParameters();
					param.GenerateInMemory = true;
					param.ReferencedAssemblies.Add("System.dll");
					param.ReferencedAssemblies.Add("System.Windows.Forms.dll");

					CompilerResults cr = cc.CompileAssemblyFromSource(param, text);
					if (cr.Errors.Count == 0)
					{
						Assembly asm = cr.CompiledAssembly;

						Type type = asm.GetType("PSOBBToolsCommand");

						MethodInfo mi = type.GetMethod("Main");
						mi.Invoke(null, null);
					}
					else
					{
						// ログファイル名
						string log = System.IO.Path.ChangeExtension(file, ".log");

						using (StreamWriter writer = new StreamWriter(log)) 
						{
							foreach (System.CodeDom.Compiler.CompilerError compErr in cr.Errors)
							{
								writer.Write(compErr.Line.ToString() + ",");
								writer.Write(compErr.Column.ToString() + ",");
								writer.Write(compErr.ErrorNumber + ",");
								writer.WriteLine(compErr.ErrorText);
							}
						}
					}
				}
			}
			catch (Exception e)
			{
			}
		}
	}
}
