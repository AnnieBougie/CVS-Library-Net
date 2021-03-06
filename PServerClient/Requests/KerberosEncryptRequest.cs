using System.Collections.Generic;

namespace PServerClient.Requests
{
   /// <summary>
   /// Kerberos-encrypt \n
   /// Response expected: no. Use Kerberos encryption to encrypt all further commu-
   /// nication between the client and the server. This will only work if the connection
   /// was made over Kerberos in the first place. If both the Gzip-stream and the
   /// Kerberos-encrypt requests are used, the Kerberos-encrypt request should be
   /// used first. This will make the client and server encrypt the compressed data,
   /// as opposed to compressing the encrypted data. Encrypted data is generally
   /// incompressible.
   /// Note that this request does not fully prevent an attacker from hijacking the con-
   /// nection, in the sense that it does not prevent hijacking the connection between
   /// the initial authentication and the Kerberos-encrypt request.
   /// </summary>
   public class KerberosEncryptRequest : NoArgRequestBase
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="KerberosEncryptRequest"/> class.
      /// </summary>
      public KerberosEncryptRequest()
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="KerberosEncryptRequest"/> class.
      /// </summary>
      /// <param name="lines">The lines.</param>
      public KerberosEncryptRequest(IList<string> lines)
         : base(lines)
      {
      }

      /// <summary>
      /// Gets the RequestType of the request
      /// </summary>
      /// <value>The RequestType value</value>
      public override RequestType Type
      {
         get
         {
            return RequestType.KerberosEncrypt;
         }
      }
   }
}