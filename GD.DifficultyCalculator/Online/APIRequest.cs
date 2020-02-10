// Copyright (c) Alten. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Net.Http;
using osu.Framework.IO.Network;

namespace GD.Calculator.Online
{
    public abstract class APIRequest<T> : APIRequest
        where T : new()
    {
        protected APIRequest()
        {
            base.Success += onSuccess;
        }

        protected virtual char CharToSplitBy => ':';
        public T Result => ((RobtopWebRequest<T>)WebRequest).ResponseObject;
        protected override WebRequest CreateWebRequest() => new RobtopWebRequest<T>(Uri, CharToSplitBy);
        private void onSuccess() => Success?.Invoke(Result);

        /// <summary>Invoked on successful completion of an API request. This will be scheduled to the API's internal scheduler (run on update thread automatically).</summary>
        public new event APISuccessHandler<T> Success;
    }

    public abstract class APIRequestList<T> : APIRequest
        where T : new()
    {
        protected APIRequestList()
        {
            base.Success += onSuccess;
        }

        protected virtual char CharToSplitBy => ':';
        protected virtual char CharToAddToList => '|';
        public List<T> Result => ((RobtopWebRequestList<T>)WebRequest).ResponseObject;
        protected override WebRequest CreateWebRequest() => new RobtopWebRequestList<T>(Uri, CharToSplitBy, CharToAddToList);
        private void onSuccess() => Success?.Invoke(Result);

        /// <summary>Invoked on successful completion of an API request. This will be scheduled to the API's internal scheduler (run on update thread automatically).</summary>
        public new event APISuccessHandler<List<T>> Success;
    }

    public abstract class APIRequest
    {
        private bool cancelled;
        protected WebRequest WebRequest;
        protected abstract string Target { get; }
        protected virtual string Uri => $@"http://boomlings.com/database/{Target}.php";
        protected virtual WebRequest CreateWebRequest() => new WebRequest(Uri);

        /// <summary>
        /// Invoked on successful completion of an API request. This will be scheduled to the API's internal scheduler (run on update thread automatically).
        /// </summary>
        public event APISuccessHandler Success;

        public void Perform()
        {
            WebRequest                       =  CreateWebRequest();
            WebRequest.Failed                += fail;
            WebRequest.AllowRetryOnTimeout   =  false;
            WebRequest.Method                =  HttpMethod.Post;
            WebRequest.AllowInsecureRequests =  true;

            WebRequest.AddParameter("secret",
                "Wmfd2893gb7"); // Since every single request to the server has this value, we might as well add this.

            if (!WebRequest.Aborted) //could have been aborted by a Cancel() call
            {
                Console.WriteLine($@"Performing request {this}");
                WebRequest.Perform();
            }

            Success?.Invoke();
        }

        public void Cancel() => fail(new OperationCanceledException(@"Request cancelled"));

        private void fail(Exception e)
        {
            if (WebRequest?.Completed == true)
                return;

            if (cancelled)
                return;

            cancelled = true;
            WebRequest?.Abort();

            Console.WriteLine($@"Failing request {this} ({e})");
        }
    }

    public delegate void APIFailureHandler(Exception e);

    public delegate void APISuccessHandler();

    public delegate void APIProgressHandler(long current, long total);

    public delegate void APISuccessHandler<in T>(T content);
}
