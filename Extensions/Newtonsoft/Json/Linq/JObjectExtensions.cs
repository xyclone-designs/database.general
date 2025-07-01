using SQLite;

using System;
using System.Linq;

using XycloneDesigns.Apis.General.Tables;

namespace Newtonsoft.Json.Linq
{
	public static class JObjectExtensions 
	{
		public class Params
		{
			public SQLiteConnection? ConnectionCountries { get; set; }
			public SQLiteConnection? ConnectionDistricts { get; set; }
			public SQLiteConnection? ConnectionLanguages { get; set; }
			public SQLiteConnection? ConnectionMunicipalities { get; set; }
			public SQLiteConnection? ConnectionProvinces { get; set; }
		}

		public static Country CountryFromDefault(this JObject jobject, Params? _params)
		{
			return new Country
			{
				Capital = jobject.GetValue(Country.SQL.Column_Capital, StringComparison.OrdinalIgnoreCase)?.ToObject<string?>(),
				Code = jobject.GetValue(Country.SQL.Column_Code, StringComparison.OrdinalIgnoreCase)?.ToObject<string?>(),
				Name = jobject.GetValue(Country.SQL.Column_Name, StringComparison.OrdinalIgnoreCase)?.ToObject<string?>(),
				Population = jobject.GetValue(Country.SQL.Column_Population, StringComparison.OrdinalIgnoreCase)?.ToObject<int?>(),
				SquareKms = jobject.GetValue(Country.SQL.Column_SquareKms, StringComparison.OrdinalIgnoreCase)?.ToObject<decimal?>(),
				UrlCoatOfArms = jobject.GetValue(Country.SQL.Column_UrlCoatOfArms, StringComparison.OrdinalIgnoreCase)?.ToObject<string?>(),
				UrlWebsite = jobject.GetValue(Country.SQL.Column_UrlWebsite, StringComparison.OrdinalIgnoreCase)?.ToObject<string?>(),
			};
		}
		public static District? DistrictFromGeography(this JObject jobject, Params? _params)
		{
			if (jobject.GetValue("parent_level")?.ToObject<string?>() is not string parentlevel || parentlevel != "district")
				return null;

			Country? country = _params?.ConnectionCountries?
				.Table<Country>()
				.FirstOrDefault(_ => _.Code == Country.Codes.SouthAfrica);

			return new District
			{
				Code = jobject.GetValue("parent_code")?.ToObject<string?>(),
				Name = jobject.GetValue("name")?.ToObject<string?>(),
				PkCountry = country?.Pk,
			};
		}
		public static Language LanguageFromDefault(this JObject jobject, Params? _params)
		{
			return new Language
			{
				Code = jobject.GetValue(Language.SQL.Column_Code, StringComparison.OrdinalIgnoreCase)?.ToObject<string?>(),
				Name = jobject.GetValue(Language.SQL.Column_Name, StringComparison.OrdinalIgnoreCase)?.ToObject<string?>(),
			};
		}
		public static Municipality MuniciplityFromGeography(this JObject jobject, Params? _params)
		{
			Country? country = _params?.ConnectionCountries?
				.Table<Country>()
				.FirstOrDefault(_ => _.Code == Country.Codes.SouthAfrica);

			District? district = jobject.GetValue("parent_level")?.ToObject<string?>() is not string parentlevel || parentlevel != "district" 
				? null
				: _params?.ConnectionDistricts?
					.TableEnumerable<District>()
					.FirstOrDefault(_ =>
					{
						return string.Equals(_.Code, jobject.GetValue("parent_code")?.ToObject<string?>(), StringComparison.OrdinalIgnoreCase);
					});

			Province? province = _params?.ConnectionProvinces?
				.TableEnumerable<Province>()
				.FirstOrDefault(_ =>
				{
					return
						string.Equals(_.Code, jobject.GetValue("province_code")?.ToObject<string?>(), StringComparison.OrdinalIgnoreCase) ||
						string.Equals(_.Name, jobject.GetValue("province_name")?.ToObject<string?>(), StringComparison.OrdinalIgnoreCase);
				});

			return new Municipality
			{
				AddressPostal = string.Format("{0}\n{1}\n{2}",
					jobject.GetValue("postal_address_1")?.ToObject<string?>(),
					jobject.GetValue("postal_address_2")?.ToObject<string?>(),
					jobject.GetValue("postal_address_3")?.ToObject<string?>()),
				AddressStreet = string.Format("{0}\n{1}\n{2}\n{3}",
					jobject.GetValue("street_address_1")?.ToObject<string?>(),
					jobject.GetValue("street_address_2")?.ToObject<string?>(),
					jobject.GetValue("street_address_3")?.ToObject<string?>(),
					jobject.GetValue("street_address_4")?.ToObject<string?>()),
				Category = jobject.GetValue("category")?.ToObject<string?>(),
				GeoCode = jobject.GetValue("geo_code")?.ToObject<string?>(),
				GeoLevel = jobject.GetValue("geo_level")?.ToObject<string?>(),
				IsDisestablished = jobject.GetValue("is_disestablished")?.ToObject<bool?>(),
				MiifCategory = jobject.GetValue("miif_category")?.ToObject<string?>(),
				Name = jobject.GetValue("name")?.ToObject<string?>(),
				NameLong = jobject.GetValue("long_name")?.ToObject<string?>(),
				NumberFax = jobject.GetValue("fax_number")?.ToObject<string?>(),
				NumberPhone = jobject.GetValue("phone_number")?.ToObject<string?>(),
				Population = jobject.GetValue("population")?.ToObject<int?>(),
				SquareKms = jobject.GetValue("square_kms")?.ToObject<decimal?>(),
				PkCountry = country?.Pk,
				PkDistrict = district?.Pk,
				PkProvince = province?.Pk,
				UrlWebsite = jobject.GetValue("url")?.ToObject<string?>(),
			};
		}
		public static Municipality MuniciplityFromMembers(this JObject jobject, Params? _params)
		{
			Province? province = _params?.ConnectionProvinces?
				.TableEnumerable<Province>()
				.FirstOrDefault(_ =>
				{
					return
						string.Equals(_.Code, jobject.GetValue("municipality.province_code")?.ToObject<string?>(), StringComparison.OrdinalIgnoreCase) ||
						string.Equals(_.Name, jobject.GetValue("municipality.province_name")?.ToObject<string?>(), StringComparison.OrdinalIgnoreCase);
				});

			return new Municipality
			{
				AddressPostal = string.Format("{0}\n{1}\n{2}",
					jobject.GetValue("municipality.postal_address_1")?.ToObject<string?>(),
					jobject.GetValue("municipality.postal_address_2")?.ToObject<string?>(),
					jobject.GetValue("municipality.postal_address_3")?.ToObject<string?>()),
				AddressStreet = string.Format("{0}\n{1}\n{2}\n{3}",
					jobject.GetValue("municipality.street_address_1")?.ToObject<string?>(),
					jobject.GetValue("municipality.street_address_2")?.ToObject<string?>(),
					jobject.GetValue("municipality.street_address_3")?.ToObject<string?>(),
					jobject.GetValue("municipality.street_address_4")?.ToObject<string?>()),
				Category = jobject.GetValue("municipality.category")?.ToObject<string?>(),
				GeoCode = jobject.GetValue("municipality.demarcation_code")?.ToObject<string?>(),
				MiifCategory = jobject.GetValue("municipality.miif_category")?.ToObject<string?>(),
				Name = jobject.GetValue("municipality.name")?.ToObject<string?>(),
				NameLong = jobject.GetValue("municipality.long_name")?.ToObject<string?>(),
				NumberFax = jobject.GetValue("municipality.fax_number")?.ToObject<string?>(),
				NumberPhone = jobject.GetValue("municipality.phone_number")?.ToObject<string?>(),
				PkProvince = province?.Pk,
				UrlWebsite = jobject.GetValue("municipality.url")?.ToObject<string?>(),
			};
		}
		public static Province ProvinceFromDefault(this JObject jobject, Params? _params)
		{
			Country? country = _params?.ConnectionCountries?
				.Table<Country>()
				.FirstOrDefault(_ => _.Code == Country.Codes.SouthAfrica);

			return new Province
			{
				Capital = jobject.GetValue(Province.SQL.Column_Capital, StringComparison.OrdinalIgnoreCase)?.ToObject<string?>(),
				Code = jobject.GetValue(Province.SQL.Column_Code, StringComparison.OrdinalIgnoreCase)?.ToObject<string?>(),
				Name = jobject.GetValue(Province.SQL.Column_Name, StringComparison.OrdinalIgnoreCase)?.ToObject<string?>(),
				Population = jobject.GetValue(Province.SQL.Column_Population, StringComparison.OrdinalIgnoreCase)?.ToObject<int?>(),
				SquareKms = jobject.GetValue(Province.SQL.Column_SquareKms, StringComparison.OrdinalIgnoreCase)?.ToObject<decimal?>(),
				UrlCoatOfArms = jobject.GetValue(Province.SQL.Column_UrlCoatOfArms, StringComparison.OrdinalIgnoreCase)?.ToObject<string?>(),
				UrlWebsite = jobject.GetValue(Province.SQL.Column_UrlWebsite, StringComparison.OrdinalIgnoreCase)?.ToObject<string?>(),
			};
		}
	}
}