using Newtonsoft.Json;
using NUnit.Framework;
using RA;
using RestAssuredAPIDemo.Helper;
using RestAssuredAPIDemo.Model;

namespace RestAssuredAPIDemo.Test
{
    [TestFixture]
    public class CountryTest
    {
        private const string ApiService = "http://services.groupkt.com";
        private readonly CountryHelper _countryHelper = new CountryHelper();
        private Country _newCountry = null;

        [SetUp]
        public void SetUp()
        {
            _newCountry = new Country
            {
                Name = "New Country",
                AlphaTwoCode = "NC",
                AlphaThreeCode = "NCO"
            };
        }

        [Test]
        public void VerifyCountriesAreRetrieved()
        {
            var jsonData = JsonConvert.SerializeObject(new RestAssured()
                .Given()
                    .Name("Validate US, DE and GB countries are retrieved correctly")
                    .Host(ApiService)
                    .Uri("/country/get/all")
                .When()
                    .Get()
                .Then()
                    .TestStatus("Verify Responde Code is 200", code => code == 200)
                    .Assert("Verify Response Code is 200")
                    .Retrieve(x => x.RestResponse.result));

            var countries = CountryHelper.GetCountriesFromJson(jsonData);
            var usaCountryExist = CountryHelper.VerifyCountryAlphaTwoCodeExist(countries, "US");
            var germanyCountryExist = CountryHelper.VerifyCountryAlphaTwoCodeExist(countries, "DE");
            var greatBritainCountryExist = CountryHelper.VerifyCountryAlphaTwoCodeExist(countries, "GB");

            Assert.IsTrue(usaCountryExist);
            Assert.IsTrue(germanyCountryExist);
            Assert.IsTrue(greatBritainCountryExist);
        }

        [Test]
        public void VerifyUnitedStatesCountryExists()
        {
            new RestAssured()
                .Given()
                    .Name("Validate USA Country Data is retrieved correctly")
                    .Host(ApiService)
                    .Uri("/country/get/iso2code/US")
                .When()
                    .Get()
                .Then()
                    .TestStatus("Verify Responde Code is 200", code => code == 200)
                    .TestBody("Verify Name must be United States of America in response", x => x.RestResponse.result.name == "United States of America")
                    .TestBody("Verify Alpha 2 code must be US in response", x => x.RestResponse.result.alpha2_code == "US")
                    .TestBody("Verify Alpha 3 code must be USA in response", x => x.RestResponse.result.alpha3_code == "USA")
                    .Debug()
                    .AssertAll();
        }

        [Test]
        public void VerifyGermanyCountryExists()
        {
            new RestAssured()
                .Given()
                    .Name("Validate Germany Country Data is retrieved correctly")
                    .Host(ApiService)
                    .Uri("/country/get/iso2code/DE")
                .When()
                    .Get()
                .Then()
                    .TestBody("Verify Name must be Germany in response", x => x.RestResponse.result.name == "Germany")
                    .TestBody("Verify Alpha 2 code must be DE in response", x => x.RestResponse.result.alpha2_code == "DE")
                    .TestBody("Verify Alpha 3 code must be DEU in response", x => x.RestResponse.result.alpha3_code == "DEU")
                    .AssertAll();
        }

        [Test]
        public void VerifyGreatBritainCountryExists()
        {
            new RestAssured()
                .Given()
                    .Name("Validate Great Britain Country Data is retrieved correctly")
                    .Host(ApiService)
                    .Uri("/country/get/iso2code/GB")
                .When()
                    .Get()
                .Then()
                    .TestStatus("Verify Responde Code is 200", code => code == 200)
                    .TestBody("Verify Name must be United Kingdom of Great Britain and Northern Ireland", x => x.RestResponse.result.name == "United Kingdom of Great Britain and Northern Ireland")
                    .TestBody("Verify Alpha 2 code must be GB in response", x => x.RestResponse.result.alpha2_code == "GB")
                    .TestBody("Verify Alpha 3 code must be GBR in response", x => x.RestResponse.result.alpha3_code == "GBR")
                    .AssertAll();
        }

        [Test]
        public void VerifyFakeCountryNotExists()
        {
            var message = new RestAssured()
                .Given()
                    .Name("Validate Fake Country does not exist")
                    .Host(ApiService)
                    .Uri("/country/get/iso2code/FAKE")
                .When()
                    .Get()
                .Then()
                    .TestStatus("Verify Responde Code is 200", code => code == 200)
                    .Assert("Verify Responde Code is 200")
                    .Retrieve(msg => msg.RestResponse.messages);

            Assert.IsTrue(message.ToString().Contains("No matching country found for requested code [FAKE]."));
        }

        [Test]
        public void CreateNewCountry()
        {
            new RestAssured()
                .Given()
                    .Name("Create a new country")
                    .Header("Content-Type", "application/json")
                    .Host(ApiService)
                    .Uri("/createCountry")
                    .Body(JsonConvert.SerializeObject(_newCountry))
                    .Debug()
                .When()
                    .Post()
                .Then()
                    .TestStatus("Verify Responde code is 201", x => x == 201)
                    .Assert("Verify Responde code is 201");
        }
    }
}