using System.Collections.Generic;

namespace PServerClient.Requests
{
   /// <summary>
   /// update-patches \n
   /// Response expected: yes. This request does not actually do anything. It is used
   /// as a signal that the server is able to generate patches when given an update
   /// request. The client must issue the -u argument to update in order to receive
   /// patches.
   /// </summary>
   public class UpdatePatchesRequest : NoArgRequestBase
   {
      public UpdatePatchesRequest()
      {
      }

      public UpdatePatchesRequest(IList<string> lines)
         : base(lines)
      {
      }

      public override bool ResponseExpected
      {
         get
         {
            return true;
         }
      }

      public override RequestType Type
      {
         get
         {
            return RequestType.UpdatePatches;
         }
      }
   }
}