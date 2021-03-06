namespace PServerClient.Responses
{
   /// <summary>
   /// F \n
   /// Flush stderr. That is, make it possible for the user to see what has been written
   /// to stderr (it is up to the implementation to decide exactly how far it should go
   /// to ensure this).
   /// </summary>
   public class FlushResponse : ResponseBase
   {
      /// <summary>
      /// Gets the ResponseType.
      /// </summary>
      /// <value>The response type.</value>
      public override ResponseType Type
      {
         get
         {
            return ResponseType.Flush;
         }
      }
   }
}