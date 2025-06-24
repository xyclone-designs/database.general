using XycloneDesigns.Apis.General.Tables;

namespace Newtonsoft.Json.Linq
{
	public static class JObjectExtensions 
	{
		public static Country ToCountry(this JObject jobject, StreamWriter logger)
		{
			Country country = new();

			if (jobject.TryGetValue(Country.SQL.Column_Capital, StringComparison.OrdinalIgnoreCase, out JToken? capital) && capital?.ToObject<string?>() is string _capital)
				country.Capital = _capital;
			else logger.WriteLine("Capital");

			if (jobject.TryGetValue(Country.SQL.Column_Code, StringComparison.OrdinalIgnoreCase, out JToken? code) && code?.ToObject<string?>() is string _code)
				country.Code = _code;
			else logger.WriteLine("Code");

			if (jobject.TryGetValue(Country.SQL.Column_Name, StringComparison.OrdinalIgnoreCase, out JToken? name) && name?.ToObject<string?>() is string _name)
				country.Name = _name;
			else logger.WriteLine("Name");

			if (jobject.TryGetValue(Country.SQL.Column_Population, StringComparison.OrdinalIgnoreCase, out JToken? population) && population?.ToObject<int?>() is int _population)
				country.Population = _population;
			else logger.WriteLine("Population");

			if (jobject.TryGetValue(Country.SQL.Column_SquareKms, StringComparison.OrdinalIgnoreCase, out JToken? squarekms) && squarekms?.ToObject<decimal?>() is decimal _squarekms)
				country.SquareKms = _squarekms;
			else logger.WriteLine("SquareKms");

			if (jobject.TryGetValue(Country.SQL.Column_UrlCoatOfArms, StringComparison.OrdinalIgnoreCase, out JToken? urlcoatofarms) && urlcoatofarms?.ToObject<string?>() is string _urlcoatofarms)
				country.UrlCoatOfArms = _urlcoatofarms;
			else logger.WriteLine("UrlCoatOfArms");

			if (jobject.TryGetValue(Country.SQL.Column_UrlWebsite, StringComparison.OrdinalIgnoreCase, out JToken? urlwebsite) && urlwebsite?.ToObject<string?>() is string _urlwebsite)
				country.UrlWebsite = _urlwebsite;
			else logger.WriteLine("UrlWebsite");

			return country;
		}
		public static Language ToLanguage(this JObject jobject, StreamWriter logger)
		{
			Language language = new ();

			if (jobject.TryGetValue(Country.SQL.Column_Code, StringComparison.OrdinalIgnoreCase, out JToken? code) && code?.ToObject<string?>() is string _code)
				language.Code = _code;
			else logger.WriteLine("Code");

			if (jobject.TryGetValue(Country.SQL.Column_Name, StringComparison.OrdinalIgnoreCase, out JToken? name) && name?.ToObject<string?>() is string _name)
				language.Name = _name;
			else logger.WriteLine("Name");

			return language;
		}
		public static Municipality ToMunicipality(this JObject jobject, StreamWriter logger)
		{
			Municipality municipality = new ();

			if (jobject.TryGetValue(Municipality.SQL.Column_AddressEmail, StringComparison.OrdinalIgnoreCase, out JToken? addressemail) && addressemail?.ToObject<string?>() is string _addressemail)
				municipality.AddressEmail = _addressemail;
			else logger.WriteLine("AddressEmail");

			if (jobject.TryGetValue(Municipality.SQL.Column_AddressPostal, StringComparison.OrdinalIgnoreCase, out JToken? addresspostal) && addresspostal?.ToObject<string?>() is string _addresspostal)
				municipality.AddressPostal = _addresspostal;
			else logger.WriteLine("AddressPostal");

			if (jobject.TryGetValue(Municipality.SQL.Column_AddressStreet, StringComparison.OrdinalIgnoreCase, out JToken? addressstreet) && addressstreet?.ToObject<string?>() is string _addressstreet)
				municipality.AddressStreet = _addressstreet;
			else logger.WriteLine("AddressStreet");

			if (jobject.TryGetValue(Municipality.SQL.Column_Category, StringComparison.OrdinalIgnoreCase, out JToken? category) && category?.ToObject<string?>() is string _category)
				municipality.Category = _category;
			else logger.WriteLine("Category");

			if (jobject.TryGetValue(Municipality.SQL.Column_GeoCode, StringComparison.OrdinalIgnoreCase, out JToken? geocode) && geocode?.ToObject<string?>() is string _geocode)
				municipality.GeoCode = _geocode;
			else logger.WriteLine("GeoCode");

			if (jobject.TryGetValue(Municipality.SQL.Column_GeoLevel, StringComparison.OrdinalIgnoreCase, out JToken? geolevel) && geolevel?.ToObject<string?>() is string _geolevel)
				municipality.GeoLevel = _geolevel;
			else logger.WriteLine("GeoLevel");

			if (jobject.TryGetValue(Municipality.SQL.Column_IsDisestablished, StringComparison.OrdinalIgnoreCase, out JToken? isdisestablished) && isdisestablished?.ToObject<bool?>() is bool _isdisestablished)
				municipality.IsDisestablished = _isdisestablished;
			else logger.WriteLine("IsDisestablished");

			if (jobject.TryGetValue(Municipality.SQL.Column_MiifCategory, StringComparison.OrdinalIgnoreCase, out JToken? miifcategory) && miifcategory?.ToObject<string?>() is string _miifcategory)
				municipality.MiifCategory = _miifcategory;
			else logger.WriteLine("MiifCategory");

			if (jobject.TryGetValue(Municipality.SQL.Column_Name, StringComparison.OrdinalIgnoreCase, out JToken? name) && name?.ToObject<string?>() is string _name)
				municipality.Name = _name;
			else logger.WriteLine("Name");

			if (jobject.TryGetValue(Municipality.SQL.Column_NameLong, StringComparison.OrdinalIgnoreCase, out JToken? namelong) && namelong?.ToObject<string?>() is string _namelong)
				municipality.NameLong = _namelong;
			else logger.WriteLine("NameLong");

			if (jobject.TryGetValue(Municipality.SQL.Column_NumberFax, StringComparison.OrdinalIgnoreCase, out JToken? numberfax) && numberfax?.ToObject<string?>() is string _numberfax)
				municipality.NumberFax = _numberfax;
			else logger.WriteLine("NumberFax");

			if (jobject.TryGetValue(Municipality.SQL.Column_NumberPhone, StringComparison.OrdinalIgnoreCase, out JToken? numberphone) && numberphone?.ToObject<string?>() is string _numberphone)
				municipality.NumberPhone = _numberphone;
			else logger.WriteLine("NumberPhone");

			if (jobject.TryGetValue(Municipality.SQL.Column_Population, StringComparison.OrdinalIgnoreCase, out JToken? population) && population?.ToObject<int?>() is int _population)
				municipality.Population = _population;
			else logger.WriteLine("Population");

			if (jobject.TryGetValue(Municipality.SQL.Column_SquareKms, StringComparison.OrdinalIgnoreCase, out JToken? squarekms) && squarekms?.ToObject<decimal?>() is decimal _squarekms)
				municipality.SquareKms = _squarekms;
			else logger.WriteLine("SquareKms");

			if (jobject.TryGetValue(Municipality.SQL.Column_UrlLogo, StringComparison.OrdinalIgnoreCase, out JToken? urllogo) && urllogo?.ToObject<string?>() is string _urllogo)
				municipality.UrlLogo = _urllogo;
			else logger.WriteLine("UrlLogo");

			if (jobject.TryGetValue(Municipality.SQL.Column_UrlWebsite, StringComparison.OrdinalIgnoreCase, out JToken? urlwebsite) && urlwebsite?.ToObject<string?>() is string _urlwebsite)
				municipality.UrlWebsite = _urlwebsite;
			else logger.WriteLine("UrlWebsite");


			return municipality;
		}
		public static Province ToProvince(this JObject jobject, StreamWriter logger)
		{
			Province province = new ();

			if (jobject.TryGetValue(Province.SQL.Column_Capital, StringComparison.OrdinalIgnoreCase, out JToken? capital) && capital?.ToObject<string?>() is string _capital)
				province.Capital = _capital;
			else logger.WriteLine("Capital");

			if (jobject.TryGetValue(Province.SQL.Column_Id, StringComparison.OrdinalIgnoreCase, out JToken? id) && id?.ToObject<string?>() is string _id)
				province.Id = _id;
			else logger.WriteLine("Id");

			if (jobject.TryGetValue(Province.SQL.Column_Name, StringComparison.OrdinalIgnoreCase, out JToken? name) && name?.ToObject<string?>() is string _name)
				province.Name = _name;
			else logger.WriteLine("Name");

			if (jobject.TryGetValue(Province.SQL.Column_Population, StringComparison.OrdinalIgnoreCase, out JToken? population) && population?.ToObject<int?>() is int _population)
				province.Population = _population;
			else logger.WriteLine("Population");

			if (jobject.TryGetValue(Province.SQL.Column_SquareKms, StringComparison.OrdinalIgnoreCase, out JToken? squarekms) && squarekms?.ToObject<decimal?>() is decimal _squarekms)
				province.SquareKms = _squarekms;
			else logger.WriteLine("SquareKms");

			if (jobject.TryGetValue(Province.SQL.Column_UrlCoatOfArms, StringComparison.OrdinalIgnoreCase, out JToken? urlcoatofarms) && urlcoatofarms?.ToObject<string?>() is string _urlcoatofarms)
				province.UrlCoatOfArms = _urlcoatofarms;
			else logger.WriteLine("UrlCoatOfArms");

			if (jobject.TryGetValue(Province.SQL.Column_UrlWebsite, StringComparison.OrdinalIgnoreCase, out JToken? urlwebsite) && urlwebsite?.ToObject<string?>() is string _urlwebsite)
				province.UrlWebsite = _urlwebsite;
			else logger.WriteLine("UrlWebsite");

			return province;
		}
	}
}