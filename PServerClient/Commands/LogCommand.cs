﻿using PServerClient.CVS;
using PServerClient.Requests;

namespace PServerClient.Commands
{
   public class LogCommand : CommandBase
   {
      public LogCommand(Root root) : base(root)
      {
         Requests.Add(new AuthRequest(root));
         Requests.Add(new RootRequest(root));
         if (LocalOnly)
            Requests.Add(new ArgumentRequest("-l"));
         Requests.Add(new LogRequest());
      }

      public bool LocalOnly { get; set; }
      public bool DefaultBranch { get; set; }
      public bool Dates { get; set; }

      public override CommandType CommandType { get { return CommandType.Log; } }
   }
}