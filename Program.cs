using ICSharpCode.SharpZipLib.GZip;

using Newtonsoft.Json.Linq;

using SQLite;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
			FileInfo fileinfo_district = new (Path.Combine(DirectoryOutputs, "districts.db"));
			FileInfo fileinfo_languages = new (Path.Combine(DirectoryOutputs, "languages.db"));
			FileInfo fileinfo_municipalities = new (Path.Combine(DirectoryOutputs, "municipalities.db"));
			FileInfo fileinfo_provinces = new (Path.Combine(DirectoryOutputs, "provinces.db"));

			SQLiteConnection sqliteconnection_countries = new (fileinfo_countries.FullName); 
			SQLiteConnection sqliteconnection_districts = new (fileinfo_district.FullName); 
			SQLiteConnection sqliteconnection_languages = new (fileinfo_languages.FullName); 
			SQLiteConnection sqliteconnection_municipalities = new (fileinfo_municipalities.FullName); 
			SQLiteConnection sqliteconnection_provinces = new (fileinfo_provinces.FullName);

			sqliteconnection_countries.CreateTable<Country>();
			sqliteconnection_districts.CreateTable<District>();
			sqliteconnection_languages.CreateTable<Language>();
			sqliteconnection_municipalities.CreateTable<Municipality>();
			sqliteconnection_provinces.CreateTable<Province>();

			JObjectExtensions.Params _params = new()
			{
				ConnectionCountries = sqliteconnection_countries,
				ConnectionDistricts = sqliteconnection_districts,
				ConnectionLanguages = sqliteconnection_languages,
				ConnectionMunicipalities = sqliteconnection_municipalities,
				ConnectionProvinces = sqliteconnection_provinces,
			};

			sqliteconnection_languages
				.InsertAll(Process_LanguagesDefault(_params), out int _)
				.Commit();

			sqliteconnection_countries
				.InsertAll(Process_CountriesDefault(_params), out int _)
				.Commit();

			sqliteconnection_provinces
				.InsertAll(Process_ProvincesDefault(_params), out int _)
				.Commit();

			sqliteconnection_districts
				.InsertAll(Process_DistrictsMunicipalMoneyGeography(_params).DistinctBy(_ => _.Code), out int _)
				.Commit();

			sqliteconnection_municipalities
				.InsertAll(Process_MunicipalitesMunicipalMoneyGeography(_params), out int _)
				//.InsertAll(Process_MunicipalitesMunicipalMoneyMembers(_params), out int _)
				.Commit();

			sqliteconnection_countries.CommitAndClose();
			sqliteconnection_districts.CommitAndClose();
			sqliteconnection_languages.CommitAndClose();
			sqliteconnection_municipalities.CommitAndClose();
			sqliteconnection_provinces.CommitAndClose();

			apifiles.Add(fileinfo_countries.ZipFile(), "countries");
			apifiles.Add(fileinfo_countries.GZipFile(), "countries");
			apifiles.Add(fileinfo_district.ZipFile(), "district");
			apifiles.Add(fileinfo_district.GZipFile(), "district");
			apifiles.Add(fileinfo_languages.ZipFile(), "languages");
			apifiles.Add(fileinfo_languages.GZipFile(), "languages");
			apifiles.Add(fileinfo_municipalities.ZipFile(), "municipalities");
			apifiles.Add(fileinfo_municipalities.GZipFile(), "municipalities");
			apifiles.Add(fileinfo_provinces.ZipFile(), "provinces");
			apifiles.Add(fileinfo_provinces.GZipFile(), "provinces");

			fileinfo_countries.Delete();
			fileinfo_district.Delete();
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

		static IEnumerable<Country> Process_CountriesDefault(JObjectExtensions.Params _params)
		{
			string directory = Path.Combine(DirectoryInputs, "countries");

			foreach (string filepath in Directory.EnumerateFiles(directory))
			{
				using FileStream filestream = File.OpenRead(filepath);
				using StreamReader streamreader = new(filestream);

				string json = streamreader.ReadToEnd();

				yield return JObject
					.Parse(json)
					.CountryFromDefault(_params);
			}
		}
		static IEnumerable<District> Process_DistrictsMunicipalMoneyGeography(JObjectExtensions.Params _params)
		{
			string path = Path.Combine(DirectoryInputs, "municipalities", "municipalmoney.municipalities.geography.json");
			using FileStream filestream = File.OpenRead(path);
			using StreamReader streamreader = new(filestream);

			string json = streamreader.ReadToEnd();

			foreach (JToken jtoken in JArray.Parse(json))
				if (((JObject)jtoken).DistrictFromGeography(_params) is District district)
					yield return district;
		}
		static IEnumerable<Language> Process_LanguagesDefault(JObjectExtensions.Params _params)
		{
			string directory = Path.Combine(DirectoryInputs, "languages");

			foreach (string filepath in Directory.EnumerateFiles(directory))
			{
				using FileStream filestream = File.OpenRead(filepath);
				using StreamReader streamreader = new(filestream);

				string json = streamreader.ReadToEnd();

				yield return JObject
					.Parse(json)
					.LanguageFromDefault(_params);
			}
		}
		static IEnumerable<Municipality> Process_MunicipalitesMunicipalMoneyGeography(JObjectExtensions.Params _params)
		{
			string path = Path.Combine(DirectoryInputs, "municipalities", "municipalmoney.municipalities.geography.json");
			using FileStream filestream = File.OpenRead(path);
			using StreamReader streamreader = new(filestream);

			string json = streamreader.ReadToEnd();

			foreach (JToken jtoken in JArray.Parse(json))
				yield return ((JObject)jtoken).MuniciplityFromGeography(_params);
		}
		static IEnumerable<Municipality> Process_MunicipalitesMunicipalMoneyMembers(JObjectExtensions.Params _params)
		{
			string path = Path.Combine(DirectoryInputs, "municipalities", "municipalmoney.municipalities.members.json");
			using FileStream filestream = File.OpenRead(path);
			using StreamReader streamreader = new(filestream);

			string json = streamreader.ReadToEnd();

			foreach (JToken jtoken in (JArray)JObject.Parse(json).GetValue("data")!)
				yield return ((JObject)jtoken).MuniciplityFromMembers(_params);
		}
		static IEnumerable<Province> Process_ProvincesDefault(JObjectExtensions.Params _params)
		{
			string directory = Path.Combine(DirectoryInputs, "provinces");

			foreach (string filepath in Directory.EnumerateFiles(directory))
			{
				using FileStream filestream = File.OpenRead(filepath);
				using StreamReader streamreader = new(filestream);

				string json = streamreader.ReadToEnd();

				yield return JObject
					.Parse(json)
					.ProvinceFromDefault(_params);
			}
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
				{ "Description", string.Format("indivcodeual {0}database for {1}", description, type) }
			});
		}
	}
}