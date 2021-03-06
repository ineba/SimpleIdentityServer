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

using Microsoft.EntityFrameworkCore;
using SimpleIdentityServer.EF.Models;

namespace SimpleIdentityServer.EF.Mappings
{
    public static class ScopeMapping
    {
        public static void AddScopeMapping(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Scope>()
                .ToTable("scopes")
                .HasKey(p => p.Name);
            modelBuilder.Entity<Scope>()
                .Property(s => s.Description)
                .HasMaxLength(255);
            modelBuilder.Entity<Scope>()
                .HasMany(s => s.ScopeClaims)
                .WithOne(c => c.Scope)
                .HasForeignKey(c => c.ScopeName)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Scope>()
                .HasMany(s => s.ClientScopes)
                .WithOne(c => c.Scope)
                .HasForeignKey(c => c.ScopeName)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Scope>()
                .HasMany(s => s.ConsentScopes)
                .WithOne(c => c.Scope)
                .HasForeignKey(c => c.ScopeName)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
