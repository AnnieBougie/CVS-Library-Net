using System.Collections.Generic;

namespace PServerClient.Requests
{
   /// <summary>
   /// watch-add \n
   /// Response expected: yes. Actually do the cvs watch on, cvs watch off, cvs
   /// watch add, and cvs watch remove commands, respectively. This uses any pre-
   /// vious Argument, Directory, Entry, or Modified requests, if they have been
   /// sent. The last Directory sent specifies the working directory at the time of the
   /// operation.
   /// </summary>
   public class WatchAddRequest : NoArgRequestBase
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="WatchAddRequest"/> class.
      /// </summary>
      public WatchAddRequest()
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="WatchAddRequest"/> class.
      /// </summary>
      /// <param name="lines">The lines.</param>
      public WatchAddRequest(IList<string> lines)
         : base(lines)
      {
      }

      /// <summary>
      /// Gets a value indicating whether a response is expected from CVS after sending the request.
      /// </summary>
      /// <value><c>true</c> if [response expected]; otherwise, <c>false</c>.</value>
      public override bool ResponseExpected
      {
         get
         {
            return true;
         }
      }

      /// <summary>
      /// Gets the RequestType of the request
      /// </summary>
      /// <value>The RequestType value</value>
      public override RequestType Type
      {
         get
         {
            return RequestType.WatchAdd;
         }
      }
   }
}