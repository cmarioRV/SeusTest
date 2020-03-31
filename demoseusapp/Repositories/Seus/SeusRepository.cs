﻿using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using demoseusapp.Models;
using demoseusapp.Repositories;
using demoseusapp.Services;

namespace demoseusapp
{
    public class SeusRepository : ISeusRepository
    {
        private const string State = "appseguros";
        private const string UrlRedirect = "sura://appsegurossura";
        public const string baseSeusURL = "https://seuslab.sura.com/";
        public const string seusClientId = "da03d411c50b475fd88888eb0e5e53c36ddf0c73b74bebefd7f378a8ee7f9212";

        private const string ResponseType = "code";
        private const string ContentTypeJson = "application/json";
        private const string ContentTypeEncoded = "application/x-www-form-urlencoded";
        private const string ContentTypeText = "text/plain; charset=UTF-8";

        private const string accountRootEndpoint = "idp/oauth/";
        private const string AuthorizeEndPoint = "authorize?client_id={0}&code_challenge={1}&state={2}&redirect_uri={3}&response_type={4}";
        private const string AutenticateEndPoint = "login";
        private const string TokenEndPoint = "token";
        private const string revokeToken = "revoke";
        private const string userInfoEndpoint = "userinfo";

        protected string urlBase;
        protected string fullUrl;

        protected readonly NSUrlSessionHandler httpClientHandler;
        protected IStorage storage;

        public SeusRepository()
        {
            urlBase = string.Format("{0}{1}", baseSeusURL, accountRootEndpoint);

            httpClientHandler = new NSUrlSessionHandler();
            httpClientHandler.AllowAutoRedirect = false;
        }

        public void SetStorage(IStorage storage)
        {
            this.storage = storage;
        }

        public AuthorizeResponse Authorize(string codeChallenge)
        {
            string url = string.Format(AuthorizeEndPoint, seusClientId, codeChallenge, State, UrlRedirect, ResponseType);

            fullUrl = string.Format("{0}{1}", urlBase, url);

            var headers = new Dictionary<string, string>
            {
                { "Accept", ContentTypeJson }
            };

            var repositoryBase = new RestConsumerJson<object, AuthorizeResponse>(httpClientHandler, storage);
            return repositoryBase.ConsumeRestService(null, fullUrl, HttpMethod.Get, headers);
        }

        public string Autenticate(AutenticateRequest request)
        {
            fullUrl = string.Format("{0}{1}", urlBase, AutenticateEndPoint);

            string body = $"username={request.Username}&password={request.Password}&tag={request.Tag}&session_id={request.SessionId}";

            var headers = new Dictionary<string, string>
            {
                { "Accept", ContentTypeText }
            };

            var repositoryBase = new ClientFormUrlEncodeResponse(httpClientHandler, storage);
            return repositoryBase.ConsumeRestService(body, fullUrl, HttpMethod.Post, ContentTypeEncoded, headers);
        }

        public TokenResponse Token(TokenRequest tokenRequest)
        {
            fullUrl = string.Format("{0}{1}", urlBase, TokenEndPoint);

            var headers = new Dictionary<string, string>
            {
                { "Accept", ContentTypeJson }
            };

            string body = $"grant_type=authorization_code&code={tokenRequest.AutenticateCode}&redirect_uri={WebUtility.UrlEncode(UrlRedirect)}&client_id={seusClientId}&code_verifier={tokenRequest.CodeChallenge}";

            var repositoryBase = new ClientJsonResponse<TokenResponse>(httpClientHandler, storage);
            return repositoryBase.ConsumeRestService(body, fullUrl, HttpMethod.Post, ContentTypeEncoded, headers);
        }

        public TokenResponse RefreshToken(string refreshToken)
        {
            fullUrl = string.Format("{0}{1}", urlBase, TokenEndPoint);

            var headers = new Dictionary<string, string>
            {
                { "Accept", ContentTypeJson }
            };

            string body = $"grant_type=refresh_token&refresh_token={refreshToken}";

            var repositoryBase = new ClientJsonResponse<TokenResponse>(httpClientHandler, storage);
            return repositoryBase.ConsumeRestService(body, fullUrl, HttpMethod.Post, ContentTypeEncoded, headers, false);
        }

        public UserInfoResponse GetUserInfo(string accessToken)
        {
            fullUrl = string.Format("{0}{1}", urlBase, userInfoEndpoint);

            var headers = new Dictionary<string, string>
            {
                { "Accept", ContentTypeJson }
            };

            var repositoryBase = new RestConsumerJson<object, UserInfoResponse>(httpClientHandler, storage);
            return repositoryBase.ConsumeRestService(null, fullUrl, HttpMethod.Get, headers);
        }

        public string InvalidateToken(InvalidTokenRequest token)
        {
            fullUrl = string.Format("{0}{1}", urlBase, revokeToken);

            var repositoryBase = new ClientFormUrlEncodeResponse(httpClientHandler, storage);
            string body = $"token={token.Token}&token_type_hint={token.TokenTypeHint}";
            return repositoryBase.ConsumeRestService(body, fullUrl, HttpMethod.Post, ContentTypeEncoded);
        }
    }
}
