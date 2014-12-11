using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;

namespace Hvz.Api.Response
{
    public class ApiResponse
    {
        public static T BuildResponse<T>(IRestResponse restResponse) where T : ApiResponse, new()
        {
            var apiResponse = new T();
            apiResponse.StatusCode = restResponse.StatusCode;
            if (apiResponse.StatusCode == HttpStatusCode.OK)
            {
                apiResponse.Status = ResponseStatus.Ok;
            }
            else
            {
                apiResponse.Status = ResponseStatus.Error;

                if(apiResponse.StatusCode != HttpStatusCode.BadRequest)
                    apiResponse.Errors.Add(restResponse.StatusDescription);
            }

            return apiResponse;
        }

        public enum ResponseStatus
        {
            Error,
            Ok
        }

        public ApiResponse()
        {
            this.Errors = new List<string>();
        }

        public ResponseStatus Status { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public List<string> Errors { get; set; }
    }
}

