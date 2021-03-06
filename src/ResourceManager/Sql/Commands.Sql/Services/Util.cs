﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using Microsoft.Azure.Commands.Sql.Security.Services;
using Microsoft.Azure.Commands.Sql.Properties;

namespace Microsoft.Azure.Commands.Sql.Services
{
    public class Util
    {
        /// <summary>
        /// Generates a client side tracing Id of the format:
        /// [Guid]
        /// </summary>
        /// <returns>A string representation of the client side tracing Id.</returns>
        public static string GenerateTracingId()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0}",
                Guid.NewGuid().ToString()
            );
        }

        /// <summary>
        /// In cases where the user decided to use one of the shortcuts (ALL or NONE), this method sets the value of the EventType property to reflect the correct values.
        /// In addition the is a deprecated audit events validity check.
        /// </summary>
        internal static string[] ProcessAuditEvents(string[] eventTypes)
        {
            if (eventTypes == null || eventTypes.Length == 0)
            {
                return eventTypes;
            }


            string[] auditEvents =
            {
                SecurityConstants.PlainSQL_Success,
                SecurityConstants.PlainSQL_Failure,
                SecurityConstants.ParameterizedSQL_Success,
                SecurityConstants.ParameterizedSQL_Failure,
                SecurityConstants.StoredProcedure_Success,
                SecurityConstants.StoredProcedure_Failure,
                SecurityConstants.Login_Success,
                SecurityConstants.Login_Failure,
                SecurityConstants.TransactionManagement_Success,
                SecurityConstants.TransactionManagement_Failure
            };


            if (eventTypes.Length == 1)
            {
                if (eventTypes[0] == SecurityConstants.None)
                {
                    return new string[] { };
                }
                if (eventTypes[0] == SecurityConstants.All)
                {
                    return auditEvents;
                }
            }
            else
            {
                if (eventTypes.Contains(SecurityConstants.All))
                {
                    throw new Exception(string.Format(Resources.InvalidEventTypeSet, SecurityConstants.All));
                }
                if (eventTypes.Contains(SecurityConstants.None))
                {
                    throw new Exception(string.Format(Resources.InvalidEventTypeSet, SecurityConstants.None));
                }
            }
            return eventTypes;
        }        
    }
}
