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

using Microsoft.AspNetCore.Http;
using SimpleIdentityServer.Configuration.Core.Errors;
using SimpleIdentityServer.Configuration.Core.Exceptions;
using SimpleIdentityServer.Configuration.Startup.DTOs.Responses;
using SimpleIdentityServer.Core.Common.Extensions;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Configuration.Startup.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        #region Constructor

        public ExceptionHandlerMiddleware(
            RequestDelegate next)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            _next = next;
        }

        #endregion

        #region Public methods

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                var identityServerManagerException = exception as IdentityConfigurationException;
                if (identityServerManagerException == null)
                {
                    identityServerManagerException = new IdentityConfigurationException(ErrorCodes.UnhandledExceptionCode, exception.Message);
                }

                var errorResponse = new ErrorResponse
                {
                    Code = identityServerManagerException.Code,
                    Message = identityServerManagerException.Message
                };

                // context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                var serializedErrorResponse = errorResponse.SerializeWithDataContract();
                await context.Response.WriteAsync(serializedErrorResponse);
            }
        }

        #endregion
    }
}
