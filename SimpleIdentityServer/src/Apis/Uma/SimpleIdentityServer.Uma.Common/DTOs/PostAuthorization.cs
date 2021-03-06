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

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SimpleIdentityServer.Uma.Common.DTOs
{
    [DataContract]
    public class PostClaimToken
    {
        [DataMember(Name = PostClaimTokenNames.Format)]
        public string Format { get; set; }
        [DataMember(Name = PostClaimTokenNames.Token)]
        public string Token { get; set; }
    }

    [DataContract]
    public class PostAuthorization
    {
        [DataMember(Name = PostAuthorizationNames.TicketId)]
        public string TicketId { get; set; }
        [DataMember(Name = PostAuthorizationNames.Rpt)]
        public string Rpt { get; set; }
        [DataMember(Name = PostAuthorizationNames.ClaimTokens)]
        public List<PostClaimToken> ClaimTokens { get; set; }
    }
}
