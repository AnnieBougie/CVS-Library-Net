﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PServerClient.LocalFileSystem;

namespace PServerClient.Responses
{
   public class PatchedResponse : ResponseBase, IFileResponse
   {
      public override ResponseType ResponseType { get { return ResponseType.Patched; } }
      public long FileLength { get; set; }
      public ReceiveFile File { get; set; }

      public override void ProcessResponse(IList<string> lines)
      {
         base.ProcessResponse(lines);
      }
   }
}
