using ICSharpCode.SharpZipLib.GZip;

using Newtonsoft.Json.Linq;

using SQLite;

using XycloneDesigns.Apis.General.Tables;

namespace Database.General
{
	internal partial class Program
	{
		//static readonly string DirectoryCurrent = Directory.GetCurrentDirectory();
		static readonly string DirectoryCurrent = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName!;

		static readonly string DirectoryOutputs = Path.Combine(DirectoryCurrent, ".outputs");
		static readonly string DirectoryInputs = Path.Combine(DirectoryCurrent, ".inputs");

		static void Main(string[] args)
		{
			_CleaningPre();

			JArray apifiles = [];
			StreamWriters streamwriters = [];

			FileInfo fileinfo_countries = new (Path.Combine(DirectoryOutputs, "countries.db"));
			FileInfo fileinfo_languages = new (Path.Combine(DirectoryOutputs, "languages.db"));
			FileInfo fileinfo_municipalities = new (Path.Combine(DirectoryOutputs, "municipalities.db"));
			FileInfo fileinfo_provinces = new (Path.Combine(DirectoryOutputs, "provinces.db"));

			SQLiteConnection sqliteconnection_countries = new (fileinfo_countries.FullName); 
			SQLiteConnection sqliteconnection_languages = new (fileinfo_languages.FullName); 
			SQLiteConnection sqliteconnection_municipalities = new (fileinfo_municipalities.FullName); 
			SQLiteConnection sqliteconnection_provinces = new (fileinfo_provinces.FullName);

			sqliteconnection_countries.CreateTable<Country>();
			sqliteconnection_languages.CreateTable<Language>();
			sqliteconnection_municipalities.CreateTable<Municipality>();
			sqliteconnection_provinces.CreateTable<Province>();

			Process<Country>(sqliteconnection_countries, "countries", streamwriters);
			Process<Language>(sqliteconnection_languages, "languages", streamwriters);
			Process<Municipality>(sqliteconnection_municipalities, "municipalities", streamwriters);
			Process<Province>(sqliteconnection_provinces, "provinces", streamwriters);

			sqliteconnection_countries.CommitAndClose();
			sqliteconnection_languages.CommitAndClose();
			sqliteconnection_municipalities.CommitAndClose();
			sqliteconnection_provinces.CommitAndClose();

			string countrieszipfile = fileinfo_countries.ZipFile();
			string countriesgzipfile = fileinfo_countries.GZipFile();
			string languageszipfile = fileinfo_languages.ZipFile();
			string languagesgzipfile = fileinfo_languages.GZipFile();
			string municipalitieszipfile = fileinfo_municipalities.ZipFile();
			string municipalitiesgzipfile = fileinfo_municipalities.GZipFile();
			string provinceszipfile = fileinfo_provinces.ZipFile();
			string provincesgzipfile = fileinfo_provinces.GZipFile();

			apifiles.Add(countrieszipfile, "countries");
			apifiles.Add(countriesgzipfile, "countries");
			apifiles.Add(languageszipfile, "languages");
			apifiles.Add(languagesgzipfile, "languages");
			apifiles.Add(municipalitieszipfile, "municipalities");
			apifiles.Add(municipalitiesgzipfile, "municipalities");
			apifiles.Add(provinceszipfile, "provinces");
			apifiles.Add(provincesgzipfile, "provinces");

			fileinfo_countries.Delete();
			fileinfo_languages.Delete();
			fileinfo_municipalities.Delete();
			fileinfo_provinces.Delete();

			string apifilesjson = apifiles.ToString();
			string apifilespath = Path.Combine(DirectoryOutputs, "index.json");

			using FileStream apifilesfilestream = File.OpenWrite(apifilespath);
			using StreamWriter apifilesstreamwriter = new (apifilesfilestream);

			apifilesstreamwriter.Write(apifilesjson);
			apifilesstreamwriter.Close();
			apifilesfilestream.Close();

			_CleaningPost();
		}

		static void _CleaningPre()
		{
			Console.WriteLine("Pre Cleaning...");

			if (Directory.Exists(DirectoryOutputs)) Directory.Delete(DirectoryOutputs, true);

			Console.WriteLine("Creating Directories...");

			Directory.CreateDirectory(DirectoryOutputs);
		}
		static void _CleaningPost()
		{
			Console.WriteLine("Cleaning Up...");
		}
		static void Process<TTable>(SQLiteConnection sqliteconnection, string name, StreamWriters streamwriters) where TTable : _Table
		{
			string inputpath = Path.Combine(DirectoryInputs, name);
			string outputpath = Path.Combine(DirectoryOutputs, name);

			Directory.CreateDirectory(streamwriters.PathBase = outputpath);

			Func<JObject, StreamWriter, object> table = (jobject, streamwriter) => true switch
			{
				true when typeof(TTable) == typeof(Country) => jobject.ToCountry(streamwriter),
				true when typeof(TTable) == typeof(Language) => jobject.ToLanguage(streamwriter),
				true when typeof(TTable) == typeof(Municipality) => jobject.ToMunicipality(streamwriter),
				true when typeof(TTable) == typeof(Province) => jobject.ToProvince(streamwriter),
				
				_ => throw new ArgumentException(),
			};

			List<object> objects = Directory.EnumerateFiles(inputpath)
				.Select(file =>
				{
					streamwriters.Add(file, string.Format("{0}.txt", file.Split('\\')[^1]), true);

					using FileStream filestream = File.OpenRead(file);
					using StreamReader streamreader = new(filestream);

					string json = streamreader.ReadToEnd();
					object insert = table.Invoke(JObject.Parse(json), streamwriters[file]);

					streamwriters.Dispose(true, file);

					return insert;

				}).ToList();

			sqliteconnection.InsertAll(objects);
		}
	}

	public static class Extensions
	{
		public static void Add(this JArray jarray, string filename, string? type)
		{
			string description = filename.Split('.').Last() switch
			{
				"zip" => "zipped ",
				"gz" => "g-zipped ",

				_ => string.Empty
			};

			jarray.Add(new JObject
			{
				{ "DateCreated", DateTime.Now.ToString("dd-MM-yyyy") },
				{ "DateEdited", DateTime.Now.ToString("dd-MM-yyyy") },
				{ "Name", filename.Split('/').Last() },
				{ "Url", string.Format("https://raw.githubusercontent.com/xyclone-designs/database.general/refs/heads/main/.output/{0}", filename) },
				{ "Description", string.Format("individual {0}database for {1}", description, type) }
			});
		}
	}
}