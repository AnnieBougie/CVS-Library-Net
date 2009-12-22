﻿using PServerClient.CVS;

namespace PServerClient.Requests
{
   /// <summary>
   /// Directory local-directory \n
   //Additional data: repository \n. Response expected: no. Tell the server what
   //directory to use. The repository should be a directory name from a previous
   //server response. Note that this both gives a default for Entry and Modified
   //and also for ci and the other commands; normal usage is to send Directory
   //for each directory in which there will be an Entry or Modified, and then a final
   //Directory for the original directory, then the command. The local-directory
   //is relative to the top level at which the command is occurring (i.e. the last
   //Directory which is sent before the command); to indicate that top level, ‘.’
   //should be sent for local-directory.
   /// </summary>
   public class DirectoryRequest : RequestBase
   {
      public DirectoryRequest(Root root)
      {
         RequestLines = new string[2];
         RequestLines[0] = string.Format("{0} .", RequestName);
         RequestLines[1] = string.Format("{0}/{1}", root.RepositoryPath, root.Module);
      }

      public override bool ResponseExpected { get { return false; } }
      public override RequestType RequestType { get { return RequestType.Directory; } }
   }
}