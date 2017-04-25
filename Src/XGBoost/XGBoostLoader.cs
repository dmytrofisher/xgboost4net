using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace XGBoost
{
	public class XGBoostLoader
	{
		public static void LoadXGBoostDll() {
			var resource = "XGBoost.libxgboost.dll";
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
			using (var reader = new BinaryReader(stream)) {
				var bytes = reader.ReadBytes((int) stream.Length);
				var path = ExtractEmbeddedDlls("libxgboost.dll", bytes);
				LoadLibrary(path);
			}
		}

		private static string ExtractEmbeddedDlls(string dllName, byte[] resourceBytes) {
			var assem = Assembly.GetExecutingAssembly();
			var an = assem.GetName();

			// The temporary folder holds one or more of the temporary DLLs
			// It is made "unique" to avoid different versions of the DLL or architectures.
			var tempFolder = String.Format("{0}.{1}.{2}", an.Name, an.ProcessorArchitecture, an.Version);

			var dirName = Path.Combine(Path.GetTempPath(), tempFolder);
			if (!Directory.Exists(dirName)) Directory.CreateDirectory(dirName);


			// See if the file exists, avoid rewriting it if not necessary
			var dllPath = Path.Combine(dirName, dllName);
			var rewrite = true;
			if (File.Exists(dllPath)) {
				var existing = File.ReadAllBytes(dllPath);
				if (resourceBytes.SequenceEqual(existing))
					rewrite = false;
			}
			if (rewrite) File.WriteAllBytes(dllPath, resourceBytes);
			return dllPath;
		}

		[DllImport("kernel32",
			SetLastError = true,
			CharSet = CharSet.Unicode)]
		private static extern IntPtr LoadLibrary(string lpFileName);
	}
}
