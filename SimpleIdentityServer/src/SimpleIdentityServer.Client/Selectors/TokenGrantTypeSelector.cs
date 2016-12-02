﻿#region copyright
// Copyright 2015 Habart Thierry
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using SimpleIdentityServer.Client.Builders;
using SimpleIdentityServer.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleIdentityServer.Client.Selectors
{
    public interface ITokenGrantTypeSelector
    {
        ITokenClient UseClientCredentials(params string[] scopes);
        ITokenClient UseClientCredentials(List<string> scopes);
        ITokenClient UsePassword(string userName, string password, params string[] scopes);
        ITokenClient UsePassword(string userName, string password, List<string> scopes);
        ITokenClient UseRefreshToken(string refreshToken);
        IIntrospectClient Introspect(string token, TokenType tokenType);
    }

    internal class TokenGrantTypeSelector : ITokenGrantTypeSelector
    {
        private readonly RequestBuilder _requestBuilder;
        private readonly ITokenClient _tokenClient;
        private readonly IIntrospectClient _introspectClient;
        
        public TokenGrantTypeSelector(RequestBuilder requestBuilder, ITokenClient tokenClient, IIntrospectClient introspectClient)
        {
            _requestBuilder = requestBuilder;
            _tokenClient = tokenClient;
            _introspectClient = introspectClient;
        }
        
        public ITokenClient UseClientCredentials(params string[] scopes)
        {
            if (scopes == null || !scopes.Any())
            {
                throw new ArgumentNullException(nameof(scopes));
            }

            return UseClientCredentials(scopes.ToList());
        }

        public ITokenClient UseClientCredentials(List<string> scopes)
        {
            if (scopes == null || !scopes.Any())
            {
                throw new ArgumentNullException(nameof(scopes));
            }

            _requestBuilder.Content.Add(RequestTokenNames.Scope, ConcatScopes(scopes));
            _requestBuilder.Content.Add(RequestTokenNames.GrantType, GrantTypes.ClientCredentials);
            return _tokenClient;
        }

        public ITokenClient UsePassword(string userName, string password, params string[] scopes)
        {
            if (scopes == null)
            {
                throw new ArgumentNullException(nameof(scopes));
            }

            return UsePassword(userName, password, scopes.ToList());
        }

        public ITokenClient UsePassword(string userName, string password, List<string> scopes)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (scopes == null || !scopes.Any())
            {
                throw new ArgumentNullException(nameof(scopes));
            }

            _requestBuilder.Content.Add(RequestTokenNames.Username, userName);
            _requestBuilder.Content.Add(RequestTokenNames.Password, password);
            _requestBuilder.Content.Add(RequestTokenNames.Scope, ConcatScopes(scopes));
            _requestBuilder.Content.Add(RequestTokenNames.GrantType, GrantTypes.Password);
            return _tokenClient;
        }

        public ITokenClient UseRefreshToken(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            _requestBuilder.Content.Add(RequestTokenNames.GrantType, GrantTypes.RefreshToken);
            _requestBuilder.Content.Add(RequestTokenNames.RefreshToken, refreshToken);
            return _tokenClient;
        }

        public IIntrospectClient Introspect(string token, TokenType tokenType)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            _requestBuilder.Content.Add(IntrospectionRequestNames.Token, token);
            _requestBuilder.Content.Add(IntrospectionRequestNames.TokenTypeHint, tokenType == TokenType.RefreshToken ? TokenTypes.RefreshToken : TokenTypes.AccessToken);
            return _introspectClient;
        }

        private static string ConcatScopes(List<string> scopes)
        {
            return string.Join(" ", scopes);
        }
    }
}
