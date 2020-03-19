using Aquatrols.Models;
using ModernHttpClient;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace Aquatrols
{
    class ServiceHelper
    {
        /// <summary>
        /// Post request function
        /// </summary>
        /// <returns>The request.</returns>
        /// <param name = "strSerializedData" > String serialized data.</param>
        /// <param name = "strMethod" > String method.</param>
        /// <param name = "isHeader" > If set to <c>true</c> is header.</param>
        public async Task<string> PostRequest(string strSerializedData, string strMethod, bool isHeader, string token)
        {
            string result = "";
            try
            {
                var httpClient = new HttpClient(new NativeMessageHandler());
                if (isHeader)
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }
                string serviceUrl = ConfigEntity.baseURL + strMethod;
                httpClient.Timeout = TimeSpan.FromSeconds(300);
                var uri = new Uri(serviceUrl);
                var content = new StringContent(strSerializedData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await httpClient.PostAsync(uri, content);
                result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return result;
            }
        }
        /// <summary>
        /// Get the request.
        /// </summary>
        /// <returns>The request.</returns>
        /// <param name="strSerializedData">String serialized data.</param>
        /// <param name="strMethod">String method.</param>
        /// <param name="isHeader">If set to <c>true</c> is header.</param>
        /// <param name="token">Token.</param>
        public async Task<ServiceResponse> GetRequest(String strSerializedData, String strMethod, bool isHeader, String token)
        {
            ServiceResponse serviceResponse = null;
            try
            {
                var httpClient = new HttpClient(new NativeMessageHandler());
                if (isHeader)
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }
                string serviceUrl = ConfigEntity.baseURL + strMethod;
                httpClient.Timeout = TimeSpan.FromSeconds(300);

                var uri = new Uri(serviceUrl);
                string contentType = string.Empty;
                if (strSerializedData != null)
                {
                    var content = new StringContent(strSerializedData, Encoding.UTF8, "application/json");
                }
                HttpResponseMessage response = null;
                response = await httpClient.GetAsync(uri);
                serviceResponse = new ServiceResponse();
                serviceResponse.response = response.Content.ReadAsStringAsync().Result;
                serviceResponse.status = response.StatusCode.ToString();

                return serviceResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return serviceResponse;
            }
        }

        /// <summary>
        /// Post File request function
        /// </summary>
        /// <returns>The request.</returns>
        /// <param name = "File" > File to be uploaded to blob.</param>
        /// <param name="fileName">Nae of the File uploaded</param>
        /// <param name = "strMethod" > String method.</param>
        /// <param name = "isHeader" > If set to <c>true</c> is header.</param>
        /// <param name="token">Token.</param>
        public async Task<string> PostFileRequest(Stream File, String fileName, String strMethod, bool isHeader, String token, string type)
        {
            string result = "";
            try
            {
                var httpClient = new HttpClient(new NativeMessageHandler());
                if (isHeader)
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }
                string serviceUrl = ConfigEntity.baseURL + strMethod;
                httpClient.Timeout = TimeSpan.FromSeconds(300);
                var uri = new Uri(serviceUrl);

                HttpContent fileStreamContent = new StreamContent(File);
                fileStreamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { Name = "file", FileName = fileName };
                fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                var formData = new MultipartFormDataContent();

                formData.Add(fileStreamContent);

                HttpResponseMessage response = null;
                response = await httpClient.PostAsync(uri, formData);
                result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return result;
            }
        }

        /// <summary>
        /// Service response.
        /// </summary>
        public class ServiceResponse
        {
            public string response { get; set; }
            public string status { get; set; }
        }
    }
}
