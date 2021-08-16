using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using RestSharp;
using TechTalk.SpecFlow;
using WebApiHolidays.Data.Models;

namespace WebApiHolidays.Specs.Features.Steps
{
    [Binding]
    public class ApiSteps
    {
        private RestClient _http;
        private RestRequest _request;
        private IRestResponse _response;
        private HolidayItems holidayItems;
        
        [Given(@"User sends GET request to (.*) service with Year (.*) and Country (.*)")]
        public void UserSendsRequest(string service, string year, string country)
        {
            _http = new RestClient("https://date.nager.at/api/v3/publicholidays/");
            _request = new RestRequest("{year}/{country}", Method.GET);

            _request.AddUrlSegment("country", country);
            _request.AddUrlSegment("year", year);
            
            _response = _http.Execute(_request);
            
            string holidaysResponse = "{\"Holidays\": " + _response.Content + " }";
            holidayItems = JsonConvert.DeserializeObject<HolidayItems>(holidaysResponse);
        }

        [Then(@"Status Code is (.*)")]
        public void VerifyStatusCode(int status)
        {
            HttpStatusCode statusCode = _response.StatusCode;
            int actualStatusCode = (int)statusCode;
            Assert.AreEqual(status, actualStatusCode, $"Incorrect status code. Actual: {actualStatusCode} - Expected: {status}");
        }

        [Then(@"Holiday objects count more than (.*)")]
        public void VerifyHolidaysCount(int count)
        {
            int actualHolidaysCount = holidayItems.Holidays.Count;
            Assert.True(actualHolidaysCount > count, $"Incorrect holidays count. Actual: {actualHolidaysCount} - Expected: {count}");
        }

        [Then(@"User verifies that holiday with localName (.*) on (.*)")]
        public void VerifyHolidayDay(string holidayName, string dayOfTheWeek)
        {
            Holiday holiday = holidayItems.Holidays.Find(holiday => holiday.LocalName == holidayName);

            if (holiday != null)
            {
                DateTime holidayDate = DateTime.Parse(holiday.Date);
                string actualDayOfTheWeek = holidayDate.DayOfWeek.ToString();
                
                StringAssert.AreEqualIgnoringCase(dayOfTheWeek, actualDayOfTheWeek, $"Incorrect holiday day. Actual: {actualDayOfTheWeek} - Expected: {dayOfTheWeek}");
            }
            else
            {
                throw new Exception($"Unable to find holiday with localName: \"{holidayName}\"");
            }
        }

        [Then(@"User verifies that holiday with name (.*) exists")]
        public void VerifyHolidayExists(string holidayName)
        {
            Holiday holiday = holidayItems.Holidays.Find(holiday => holiday.Name == holidayName);

            Assert.True(holiday != null, $"Holiday with name: \"{holidayName}\" is not exist");
        }

        [Then(@"(.*) holiday object has valid schema")]
        public void ThenObjectHasValidSchema(int index)
        {
            JSchema jsonSchema = JSchema.Parse(@"{
            '$schema': 'http://json-schema.org/draft-04/schema#',
            'type': 'object',
            'properties': {
                'date': {
                    'type': 'string'
                },
                'localName': {
                    'type': 'string'
                },
                'name': {
                    'type': 'string'
                },
                'countryCode': {
                    'type': 'string'
                },
                'fixed': {
                    'type': 'string'
                },
                'global': {
                    'type': 'string'
                },
                'counties': {
                    'type':'null'
                },
                'launchYear': {
                    'type': 'string'
                },
                'types': {
                    'type': 'array',
                    'items': [
                    {
                        'type': 'string'
                    }
                    ]
                }
            },
            'required': [
            'date',
            'localName',
            'name',
            'countryCode',
            'fixed',
            'global',
            'launchYear',
            'types'
                ]
        }");

            string holidayObj = JsonConvert.SerializeObject(holidayItems.Holidays[index - 1]);
            
            JObject holiday = JObject.Parse(@holidayObj);

            Assert.True(holiday.IsValid(jsonSchema), $"Incorrect Json Schema, Actual: {holidayObj}");

        }
    }
}