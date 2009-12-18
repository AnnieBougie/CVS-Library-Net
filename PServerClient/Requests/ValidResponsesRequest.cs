﻿using System;
using System.Collections.Generic;
using System.Linq;
using PServerClient.Responses;

namespace PServerClient.Requests
{
   /// <summary>
   /// Valid-responses request-list \n
   /// Response expected: no. Tell the server what responses the client will accept.
   /// request-list is a space separated list of tokens. The Root request need not have
   /// been previously sent.
   /// </summary>
   public class ValidResponsesRequest : RequestBase
   {
      private readonly ResponseType[] _validResponses;

      public ValidResponsesRequest(ResponseType[] validResponses)
      {
         _validResponses = validResponses;   
      }

      public override bool ResponseExpected { get { return false; } }
      public override RequestType RequestType { get { return RequestType.ValidResponses; } }

      public override string GetRequestString()
      {
         return string.Format("{2} {0}{1}", ResponseHelper.GetValidResponsesString(_validResponses), LineEnd, RequestName);
      }
   }
}