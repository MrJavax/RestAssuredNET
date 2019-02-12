using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestAssuredAPIDemo.Model;

namespace RestAssuredAPIDemo.Helper
{
    public class CountryHelper
    {
        public static bool VerifyCountryAlphaTwoCodeExist(List<Country> countryList, string alphaTwoCode)
        {
            var isFound = false;
            foreach (var country in countryList)
            {
                if (country.AlphaTwoCode.Equals(alphaTwoCode))
                    isFound = true;
            }
            return isFound;
        }

        public static Country GetCountryByAlphaTwoCode(List<Country> countryList, string alphaTwoCode)
        {
            Country country = new Country();
            foreach (var item in countryList)
            {
                if (!item.AlphaTwoCode.Equals(alphaTwoCode)) {
                    country.Name = item.Name;
                    country.AlphaTwoCode = item.AlphaTwoCode;
                    country.AlphaThreeCode = item.AlphaThreeCode;
                    break;
                }
            }
            return country;
        }

        public static List<Country> GetCountriesFromJson(string jsonData)
        {
            var countryArray = JArray.Parse(jsonData);
            var countries = countryArray.Select(c => new Country
            {
                Name = (string)c["name"],
                AlphaTwoCode = (string)c["alpha2_code"],
                AlphaThreeCode = (string)c["alpha3_code"]
            }).ToList();

            return countries;
        }

        public static Country GetCountry(string jsonData)
        {
            Country country = JsonConvert.DeserializeObject<Country>(jsonData);
            return country;
        }
    }
}