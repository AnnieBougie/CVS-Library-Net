﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PServerClient.Requests
{
   public class RDiffRequest : RequestBase
   {
      public override bool ResponseExpected { get { return true; } }

      public override string GetRequestString()
      {
         return string.Format("rdiff{0}", lineEnd);
      }
   }
}
