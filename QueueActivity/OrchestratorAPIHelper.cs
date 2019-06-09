using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etisalat.CIT.OPS.Robotics.QueueActivity
{
    public static class OrchestratorAPIHelper
    {
        static string baseUrl = "https://platform.uipath.com";

        public static string GetToken(object loginModel)
        {
            string token = string.Empty;
            try
            {
                var client = new RestClient(baseUrl + "/api/account/authenticate");
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/json");

                string jsonLoginModel = JsonConvert.SerializeObject(loginModel, Formatting.Indented);
                request.AddParameter("loginModel", jsonLoginModel, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                string isSuccess = GetValue(response.Content, "success");
                if ((int)response.StatusCode == 200 && isSuccess == "True")
                {
                    //Console.WriteLine("Authenticate - Success : " + response.StatusCode);
                    token = GetValue(response.Content, "result");
                }
                else
                {
                    Console.WriteLine("Authenticate - Failed : " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION: " + ex.Message);
            }

            return token;
        }

        public static QueueDefinitionDto[] GetQueueDefinitions(string token)
        {
            QueueDefinitionDto[] queues = null;
            string url = baseUrl + "/odata/QueueDefinitions?$top=50";
            //if(!string.IsNullOrEmpty(queueName))
            //{
            //    url += "?$filter=Name%20eq%20" + queueName;
            //}

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + token);

            IRestResponse response = client.Execute(request);
            if ((int)response.StatusCode == 200)
            {
                //Console.WriteLine("GetQueueDefinitions - Success : " + response.StatusCode);

                JObject jContent = JObject.Parse(response.Content);
                int qCount = int.Parse(jContent["@odata.count"].ToString());
                queues = JsonConvert.DeserializeObject<QueueDefinitionDto[]>(jContent["value"].ToString());
            }
            else
            {
                Console.WriteLine("GetQueueDefinitions - Failed : " + response.StatusCode);
            }

            return queues;
        }

        public static QueueProcessingStatus[] GetQueueDefinitionByName(string token, string queueName)
        {
            QueueProcessingStatus[] queues = null;
            string url = baseUrl + "/odata/QueueProcessingRecords/UiPathODataSvc.RetrieveQueuesProcessingStatus?$filter=((contains(QueueDefinitionName,'" + queueName + "')%20or%20contains(QueueDefinitionDescription,'" + queueName + "')))&$orderby=QueueDefinitionName&$top=50";

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + token);

            IRestResponse response = client.Execute(request);
            if ((int)response.StatusCode == 200)
            {
                //Console.WriteLine("GetQueueDefinitions - Success : " + response.StatusCode);

                JObject jContent = JObject.Parse(response.Content);
                int qCount = int.Parse(jContent["@odata.count"].ToString());
                queues = JsonConvert.DeserializeObject<QueueProcessingStatus[]>(jContent["value"].ToString());
            }
            else
            {
                Console.WriteLine("GetQueueDefinitionByName - Failed : " + response.StatusCode);
            }

            return queues;
        }

        public static bool AddQueue(string token, string queueName, object specificContent)
        {
            bool result = false;
            var client = new RestClient(baseUrl + "/odata/Queues/UiPathODataSvc.AddQueueItem");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);

            object someNull = null;

            var data = new
            {
                itemData = new
                {
                    Name = queueName,
                    Priority = "Normal",
                    SpecificContent = specificContent,
                    DeferDate = someNull,
                    DueDate = someNull
                }
            };

            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            request.AddParameter("data", jsonData, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            if ((int)response.StatusCode == 201)
            {
                //Console.WriteLine("AddQueue - Success : " + response.StatusCode);
                result = true;
            }
            else
            {
                Console.WriteLine("AddQueue - Failed : " + response.StatusCode);
                result = false;
            }

            return result;
        }

        private static string GetValue(string json, string key)
        {
            JObject jContent = JObject.Parse(json);
            string value = jContent[key].ToString();
            return value;
        }

        public static QueueItemDto[] GetQueueItems(string token, int queueId)
        {
            QueueItemDto[] queueItems = null;
            string url = baseUrl + "/odata/QueueItems?$filter=(QueueDefinitionId%20eq%20" + queueId + ")&$top=100";

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + token);

            IRestResponse response = client.Execute(request);
            if ((int)response.StatusCode == 200)
            {
                JObject jContent = JObject.Parse(response.Content);
                int qCount = int.Parse(jContent["@odata.count"].ToString());
                queueItems = JsonConvert.DeserializeObject<QueueItemDto[]>(jContent["value"].ToString());
            }
            else
            {
                Console.WriteLine("GetQueueItems - Failed : " + response.StatusCode);
            }

            return queueItems;
        }

        public static QueueItemDto[] GetAllFailedQueueItems(string token, int queueId)
        {
            QueueItemDto[] queueItems = null;
            string url = baseUrl + "/odata/QueueItems?$filter=(Status%20%20eq%20%20'2'%20and%20QueueDefinitionId%20eq%20" + queueId + ")&$top=100";

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + token);

            IRestResponse response = client.Execute(request);
            if ((int)response.StatusCode == 200)
            {
                JObject jContent = JObject.Parse(response.Content);
                int qCount = int.Parse(jContent["@odata.count"].ToString());
                queueItems = JsonConvert.DeserializeObject<QueueItemDto[]>(jContent["value"].ToString());
            }
            else
            {
                Console.WriteLine("GetAllFailedQueueItems - Failed : " + response.StatusCode);
            }

            return queueItems;
        }

        public static void SetItemReviewStatus(string token, int queueId, string rowVersion)
        {
            string url = baseUrl + "/odata/QueueItems/UiPathODataSvc.SetItemReviewStatus()";

            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);

            string jsonData = "{\"queueItems\":[{\"RowVersion\": \"" + rowVersion + "\",\"Id\":" + queueId + "}],\"status\":\"Retried\"}";
            request.AddParameter("data", jsonData, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            if ((int)response.StatusCode == 200)
            {
                JObject jContent = JObject.Parse(response.Content);
            }
            else
            {
                Console.WriteLine("GetAllFailedQueueItems - Failed : " + response.StatusCode);
            }
        }
    }
}
