﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Okta.Idx.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace okta_social_login_example.Okta
{
    public static class SocialLoginExtensions
    {
        public static void AddOktaIdentityEngine<I, T>(this IServiceCollection services, bool addSession = false)
            where I : class, IIdxClient
            where T : class, I
        {
            if (addSession)
            {
                services.AddSession();
            }

            services.AddSingleton<I, T>();
        }

        public static void AddSession(this IServiceCollection services, int idleTimeout = 15)
        {
            services.AddDistributedMemoryCache();
            
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(idleTimeout);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }

        public static void UseOktaIdentityEngine(this IApplicationBuilder applicationBuilder, bool useSession = false)
        {
            if(useSession)
            {
                applicationBuilder.UseSession();
            }

            // placeholder for additional configuration if necessary
        }

        public static IdxContext GetIdxContext(this HttpContext httpContext)
        {
            return GetIdxContext(httpContext.Session);
        }

        public static IdxContext GetIdxContext(this ISession session)
        {
            string stateHandle = session.GetString(SocialLoginIdxClient.IdxStateKey);
            return GetIdxContext(session, stateHandle);
        }

        public static IdxContext GetIdxContext(this ISession session, string stateHandle)
        {
            string idxContextJson = session.GetString(stateHandle);
            IdxContext idxContext = JsonConvert.DeserializeObject<IdxContext>(idxContextJson);
            return idxContext;
        }
    }
}
