﻿namespace PServerClient.Requests
{
   /// <summary>
   /// version \n
   //Response expected: yes. Request that the server transmit its version message.
   //The Root request need not have been previously sent.
   /// </summary>
   public class VersionRequest : NoArgRequestBase
   {
      public override bool ResponseExpected { get { return true; } }
      public override RequestType RequestType { get { return RequestType.Version; } }
   }
}