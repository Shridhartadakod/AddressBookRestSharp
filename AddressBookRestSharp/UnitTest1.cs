using Microsoft.VisualStudio.TestTools.UnitTesting;
using AddressBookRestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;

namespace AddressBookRestSharp
{
    [TestClass]
    public class UnitTest1
    {
        RestClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient(" http://localhost:4000");
        }

        private IRestResponse getContacts()
        {
            RestRequest request = new RestRequest("/Contacts", Method.GET);
            IRestResponse response = client.Execute(request);
            return response;
        }

        //Creating method to get contacts
        [TestMethod]
        public void onCallingGETApi_ReturnContactList()
        {
            IRestResponse response = getContacts();

            //assert
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            List<Contact> dataResponse = JsonConvert.DeserializeObject<List<Contact>>(response.Content);
            Assert.AreEqual(15, dataResponse.Count);
            foreach (var item in dataResponse)
            {
                System.Console.WriteLine("id: " + item.Id + "FirstName: " + item.FirstName + "LastName: " + item.LastName + "Address: " + item.Address + "City: " + item.City + "State: " + item.State + "Zip: " + item.Zip + "PhoneNumber: " + item.PhoneNumber);
            }
        }

        //Creating method to add contacts
        [TestMethod]
        public void givenContact_OnPost_ShouldReturnAddedContact()
        {
            RestRequest request = new RestRequest("/Contacts", Method.POST);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("FirstName", "Harish");
            jObjectbody.Add("LastName", "Motekar");
            jObjectbody.Add("Address", "Ghandhinagar  road");
            jObjectbody.Add("City", "Dharwad");
            jObjectbody.Add("State", "KA");
            jObjectbody.Add("Zip", 580001);
            jObjectbody.Add("PhoneNumber", 1234567890);
            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

            //act
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
            Contact dataResponse = JsonConvert.DeserializeObject<Contact>(response.Content);
            Assert.AreEqual("Harish", dataResponse.FirstName);
            Assert.AreEqual("Motekar", dataResponse.LastName);
            Assert.AreEqual("Ghandhinagar road", dataResponse.Address);
            Assert.AreEqual("KA", dataResponse.State);
            Assert.AreEqual(280001, dataResponse.Zip);
            Assert.AreEqual("Dharwad", dataResponse.City);
            Assert.AreEqual(1234567890, dataResponse.PhoneNumber);
        }

        //Creating method to update contacts
        [TestMethod]
        public void givenContact_OnPUT_ShouldReturnUpdatedContact()
        {
            RestRequest request = new RestRequest("/Contacts/3", Method.PUT);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("FirstName", "Shridhar");
            jObjectbody.Add("LastName", "Tadakod");
            jObjectbody.Add("Address", "KHB Colony");
            jObjectbody.Add("City", "Dharwad");
            jObjectbody.Add("State", "KA");
            jObjectbody.Add("Zip", 580009);
            jObjectbody.Add("PhoneNumber", 9902845705);
            request.AddOrUpdateParameter("application/json", jObjectbody, ParameterType.RequestBody);

            //act
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            Contact dataResponse = JsonConvert.DeserializeObject<Contact>(response.Content);
            Assert.AreEqual("Shraddha", dataResponse.FirstName);
            Assert.AreEqual("Khot", dataResponse.LastName);

        }

        //Creating method to delete contact
        [TestMethod]
        public void givenContact_OnDELETE_ShouldReturnContact()
        {
            RestRequest request = new RestRequest("/Contacts/6", Method.DELETE);
            JObject jObjectbody = new JObject();
            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

            //act
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.NotFound);
            Contact dataResponse = JsonConvert.DeserializeObject<Contact>(response.Content);
            Assert.AreEqual(null, dataResponse.FirstName);
            Assert.AreEqual(null, dataResponse.LastName);

        }
    }
}