﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PServerClient.Responses
{
   public class ErrorResponse : ResponseBase
   {
      public override void ProcessResponse(IList<string> lines)
      {
         throw new NotImplementedException();
      }

 
   }
}