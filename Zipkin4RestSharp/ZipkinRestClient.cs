﻿using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using System.Net;
using System.Net.Cache;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Zipkin;

namespace Zipkin4RestSharp
{
    public class ZipkinRestClient: IRestClient
    {
        private IRestClient _innerClient;

		private const string traceIdB3Header = "X-B3-TraceId";
		private const string spanIdB3Header = "X-B3-SpanId";
		private const string parentSpanIdB3Header = "X-B3-ParentSpanId";

        public ZipkinRestClient(IRestClient innerClient): base()
        {
            _innerClient = innerClient;
        }

        public IAuthenticator Authenticator
        {
            get
            {
                return _innerClient.Authenticator;
            }

            set
            {
                _innerClient.Authenticator = value;
            }
        }

        public Uri BaseUrl
        {
            get
            {
                return _innerClient.BaseUrl;
            }

            set
            {
                _innerClient.BaseUrl = value;
            }
        }

        public RequestCachePolicy CachePolicy
        {
            get
            {
                return _innerClient.CachePolicy;
            }

            set
            {
                _innerClient.CachePolicy = value;
            }
        }

        public X509CertificateCollection ClientCertificates
        {
            get
            {
                return _innerClient.ClientCertificates;
            }

            set
            {
                _innerClient.ClientCertificates = value;
            }
        }

        public CookieContainer CookieContainer
        {
            get
            {
                return _innerClient.CookieContainer;
            }

            set
            {
                _innerClient.CookieContainer = value;
            }
        }

        public IList<Parameter> DefaultParameters
        {
            get
            {
                return _innerClient.DefaultParameters;
            }
        }

        public Encoding Encoding
        {
            get
            {
                return _innerClient.Encoding;
            }

            set
            {
                _innerClient.Encoding = value;
            }
        }

        public bool FollowRedirects
        {
            get
            {
                return _innerClient.FollowRedirects;
            }

            set
            {
                _innerClient.FollowRedirects = value;
            }
        }

        public int? MaxRedirects
        {
            get
            {
                return _innerClient.MaxRedirects;
            }

            set
            {
                _innerClient.MaxRedirects = value;
            }
        }

        public bool PreAuthenticate
        {
            get
            {
                return _innerClient.PreAuthenticate;
            }

            set
            {
                _innerClient.PreAuthenticate = value;
            }
        }

        public IWebProxy Proxy
        {
            get
            {
                return _innerClient.Proxy;
            }

            set
            {
                _innerClient.Proxy = value;
            }
        }

        public int ReadWriteTimeout
        {
            get
            {
                return _innerClient.ReadWriteTimeout;
            }

            set
            {
                _innerClient.ReadWriteTimeout = value;
            }
        }

        public int Timeout
        {
            get
            {
                return _innerClient.Timeout;
            }

            set
            {
                _innerClient.Timeout = value;
            }
        }

        public string UserAgent
        {
            get
            {
                return _innerClient.UserAgent;
            }

            set
            {
                _innerClient.UserAgent = value;
            }
        }

        public bool UseSynchronizationContext
        {
            get
            {
                return _innerClient.UseSynchronizationContext;
            }

            set
            {
                _innerClient.UseSynchronizationContext = value;
            }
        }

        public void AddHandler(string contentType, IDeserializer deserializer)
        {
            _innerClient.AddHandler(contentType, deserializer);
        }

        public Uri BuildUri(IRestRequest request)
        {
            return _innerClient.BuildUri(request);
        }

        public void ClearHandlers()
        {
            _innerClient.ClearHandlers();
        }

        public byte[] DownloadData(IRestRequest request)
        {
            return _innerClient.DownloadData(request);
        }

        public virtual IRestResponse Execute(IRestRequest request)
        {
            InjectZipkinTraces(request);
			using (GetClientSendTrace(request))
			{
	            return _innerClient.Execute(request);
			}
        }

        public IRestResponse<T> Execute<T>(IRestRequest request) where T : new()
        {
            InjectZipkinTraces(request);
			using (GetClientSendTrace(request))
			{
				return _innerClient.Execute<T>(request);
			}
		}

        public IRestResponse ExecuteAsGet(IRestRequest request, string httpMethod)
        {
            InjectZipkinTraces(request);
			using (GetClientSendTrace(request))
			{
				return _innerClient.ExecuteAsGet(request, httpMethod);
			}
		}

        public IRestResponse<T> ExecuteAsGet<T>(IRestRequest request, string httpMethod) where T : new()
        {
            InjectZipkinTraces(request);
			using (GetClientSendTrace(request))
			{
				return _innerClient.ExecuteAsGet<T>(request, httpMethod);
			}
		}

        public IRestResponse ExecuteAsPost(IRestRequest request, string httpMethod)
        {
            InjectZipkinTraces(request);
			using (GetClientSendTrace(request))
			{
				InjectZipkinTraces(request);
				return _innerClient.ExecuteAsPost(request, httpMethod);
			}
		}

        public IRestResponse<T> ExecuteAsPost<T>(IRestRequest request, string httpMethod) where T : new()
        {
            InjectZipkinTraces(request);
			return _innerClient.ExecuteAsPost<T>(request, httpMethod);
        }

        public RestRequestAsyncHandle ExecuteAsync(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            InjectZipkinTraces(request);
			return _innerClient.ExecuteAsync(request, callback);
        }

        public RestRequestAsyncHandle ExecuteAsync<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback)
        {
            InjectZipkinTraces(request);
			return _innerClient.ExecuteAsync<T>(request, callback);
        }

        public RestRequestAsyncHandle ExecuteAsyncGet(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, string httpMethod)
        {
            InjectZipkinTraces(request);
			return _innerClient.ExecuteAsyncGet(request, callback, httpMethod);
        }

        public RestRequestAsyncHandle ExecuteAsyncGet<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback, string httpMethod)
        {
            InjectZipkinTraces(request);
			return _innerClient.ExecuteAsyncGet<T>(request, callback, httpMethod);
        }

        public RestRequestAsyncHandle ExecuteAsyncPost(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, string httpMethod)
        {
            InjectZipkinTraces(request);
			return _innerClient.ExecuteAsyncPost(request, callback, httpMethod);
        }

        public RestRequestAsyncHandle ExecuteAsyncPost<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback, string httpMethod)
        {
            InjectZipkinTraces(request);
			return _innerClient.ExecuteAsyncPost<T>(request, callback, httpMethod);
        }

        public Task<IRestResponse> ExecuteGetTaskAsync(IRestRequest request)
        {
            InjectZipkinTraces(request);
			return _innerClient.ExecuteGetTaskAsync(request);
        }

        public Task<IRestResponse> ExecuteGetTaskAsync(IRestRequest request, CancellationToken token)
        {
            InjectZipkinTraces(request);
			return _innerClient.ExecuteGetTaskAsync(request, token);
        }

        public Task<IRestResponse<T>> ExecuteGetTaskAsync<T>(IRestRequest request)
        {
            InjectZipkinTraces(request);
			return _innerClient.ExecuteGetTaskAsync<T>(request);
        }

        public Task<IRestResponse<T>> ExecuteGetTaskAsync<T>(IRestRequest request, CancellationToken token)
        {
            InjectZipkinTraces(request);
			return _innerClient.ExecuteGetTaskAsync<T>(request, token);
        }

        public Task<IRestResponse> ExecutePostTaskAsync(IRestRequest request)
        {
            InjectZipkinTraces(request);
			return _innerClient.ExecutePostTaskAsync(request);
        }

        public Task<IRestResponse> ExecutePostTaskAsync(IRestRequest request, CancellationToken token)
        {
            InjectZipkinTraces(request);
            return _innerClient.ExecutePostTaskAsync(request, token);
        }

        public Task<IRestResponse<T>> ExecutePostTaskAsync<T>(IRestRequest request)
        {
            InjectZipkinTraces(request);
            return _innerClient.ExecutePostTaskAsync<T>(request);
        }

        public Task<IRestResponse<T>> ExecutePostTaskAsync<T>(IRestRequest request, CancellationToken token)
        {
            InjectZipkinTraces(request);
            return _innerClient.ExecutePostTaskAsync<T>(request, token);
        }

        public Task<IRestResponse> ExecuteTaskAsync(IRestRequest request)
        {
            InjectZipkinTraces(request);
            return _innerClient.ExecuteTaskAsync(request);
        }

        public Task<IRestResponse> ExecuteTaskAsync(IRestRequest request, CancellationToken token)
        {
            InjectZipkinTraces(request);
            return _innerClient.ExecuteTaskAsync(request, token);
        }

        public Task<IRestResponse<T>> ExecuteTaskAsync<T>(IRestRequest request)
        {
            InjectZipkinTraces(request);
            return _innerClient.ExecuteTaskAsync<T>(request);
        }

        public Task<IRestResponse<T>> ExecuteTaskAsync<T>(IRestRequest request, CancellationToken token)
        {
            InjectZipkinTraces(request);
            return _innerClient.ExecuteTaskAsync<T>(request, token);
        }

        public void RemoveHandler(string contentType)
        {
			_innerClient.RemoveHandler(contentType);
        }

		private ITrace GetClientSendTrace(IRestRequest request)
		{
			var trace = new StartClientOrContinueTrace("http-request");
			trace.TimeAnnotateWith(PredefinedTag.ClientSend);
			trace.AnnotateWith(PredefinedTag.HttpMethod, request.Method.ToString());
			trace.AnnotateWith(PredefinedTag.HttpHost, this.BaseUrl.ToString());
			trace.AnnotateWith(PredefinedTag.HttpPath, request.Resource);
			return trace;
		}

		private void InjectZipkinTraces(IRestRequest request)
		{
			var span = TraceContextPropagation.CurrentSpan;
			if (span != null)
			{
				request.AddHeader(traceIdB3Header, span.TraceId.ToString());
				request.AddHeader(spanIdB3Header, span.Id.ToString());
				request.AddHeader(parentSpanIdB3Header, span.ParentId.ToString());
			}
		}
    }
}
