﻿using System;
using System.Collections.Generic;

namespace PServerClient.Responses
{
   /// <summary>
   /// Mbinary \n
   //Additional data: file transmission (note: compressed file transmissions are not
   //supported). This is like ‘M’, except the contents of the file transmission are
   //binary and should be copied to standard output without translation to local
   //text file conventions. To transmit a text file to standard output, servers should
   //use a series of ‘M’ requests.
   /// </summary>
   public class MbinaryResponse : ResponseBase, IFileResponse
   {
      public long FileLength { get; set; }
      public override void ProcessResponse(IList<string> lines)
      {
         throw new NotImplementedException();
      }

      public override string DisplayResponse()
      {
         throw new NotImplementedException();
      }

      public override ResponseType ResponseType { get { return ResponseType.Mbinary; } }
      public ReceiveFile File { get; set; }

   }
}