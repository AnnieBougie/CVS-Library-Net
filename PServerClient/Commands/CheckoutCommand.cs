﻿using System;
using System.Collections.Generic;
using System.Linq;
using PServerClient.CVS;
using PServerClient.Requests;
using PServerClient.Responses;

namespace PServerClient.Commands
{
   public class CheckoutCommand : CommandBase
   {
      public CheckoutCommand(Root root)
         : base(root)
      {
         Requests.Add(new RootRequest(root));
         Requests.Add(new GlobalOptionRequest("-q")); // somewhat quiet
         Requests.Add(new ArgumentRequest(root.Module));
         Requests.Add(new DirectoryRequest(root));
         Requests.Add(new CheckOutRequest());
      }

      public override CommandType CommandType { get { return CommandType.CheckOut; } }

      public override void PostExecute()
      {
         IList<IResponse> checkOutResponses = Requests.Where(r => r.RequestType == RequestType.CheckOut)
            .First()
            .Responses;
         foreach (IResponse response in checkOutResponses)
         {
            Console.WriteLine(response.ResponseType + ": ");
            Console.WriteLine(response.DisplayResponse());
         }
         ServerFileReceiver fileReceiver = new ServerFileReceiver(Root);
         fileReceiver.ProcessCheckoutResponses(checkOutResponses);
      }
   }
}