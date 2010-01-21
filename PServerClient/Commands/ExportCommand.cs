using System;
using System.Collections.Generic;
using PServerClient.Connection;
using PServerClient.CVS;
using PServerClient.Requests;
using PServerClient.Responses;

namespace PServerClient.Commands
{
   /// <summary>
   /// The CVS export command gets the files from a CVS module
   /// and saves them to the file system without the CVS information.
   /// </summary>
   public class ExportCommand : CommandBase
   {
      private Folder _currentFolder;

      /// <summary>
      /// Initializes a new instance of the <see cref="ExportCommand"/> class.
      /// Uses a date to select the version in CVS to get.
      /// This date can be in the future to ensure getting the latest revision
      /// </summary>
      /// <param name="root">The CVS root instance.</param>
      /// <param name="connection">The connection.</param>
      /// <param name="exportDate">The as of date to retrieve.</param>
      public ExportCommand(IRoot root, IConnection connection, DateTime exportDate)
         : base(root, connection)
      {
         string dateArg = GetExportDate(exportDate);
         IRequest exportTypeRequest = new ArgumentRequest(dateArg);
         StartUp(root, exportTypeRequest);
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ExportCommand"/> class.
      /// </summary>
      /// <param name="root">The CVS root instance.</param>
      /// <param name="connection">The connection.</param>
      /// <param name="tag">The revision tag to retrieve.</param>
      public ExportCommand(IRoot root, IConnection connection, string tag)
         : base(root, connection)
      {
         string tagArg = "-r " + tag;
         IRequest exportTypeRequest = new ArgumentRequest(tagArg);
         StartUp(root, exportTypeRequest);
      }

      /// <summary>
      /// Gets the command type.
      /// </summary>
      /// <value></value>
      public override CommandType Type
      {
         get
         {
            return CommandType.Export;
         }
      }

      internal string GetExportDate(DateTime exportDate)
      {
         string mydate = exportDate.ToRfc822();
         return string.Format("-D {0}", mydate);
      }

      /// <summary>
      /// Processes the responses of each request. When all the requests 
      /// needed to save a file have been retrieved from the CVS server,
      /// the file is saved and the information contained in the command
      /// is deleted for performance.
      /// </summary>
      /// <param name="request">The request.</param>
      protected internal override void AfterRequest(IRequest request)
      {
         if (request is ExportRequest)
         {
            IResponse response;
            IFileResponseGroup file = null;
            IList<IResponse> messages = null;
            bool gettingFile = false;
            ResponseProcessor processor = new ResponseProcessor();

            response = Connection.GetResponse();
            ProcessResponse(response);
            do
            {
               if (gettingFile)
               {
                  if (response is MTMessageResponse)
                     messages.Add(response);
                  if (response is UpdatedResponse)
                  {
                     messages = ResponseHelper.CollapseMessagesInResponses(messages);
                     file.MT = (IMessageResponse)messages[0];
                     file.FileResponse = (IFileResponse)response;

                     // process each file
                     Entry entry = processor.AddFile(_currentFolder, file);
                     entry.Save(true);
                     _currentFolder = entry.Parent;
                     file = null;
                     RemoveProcessedResponses();
                     gettingFile = false; // all done getting file
                  }
               }
               else
               {
                  if (response is ModTimeResponse)
                  {
                     file = new FileResponseGroup();
                     messages = new List<IResponse>();
                     file.ModTime = (ModTimeResponse)response;
                     gettingFile = true;
                  }
               }

               response = Connection.GetResponse();
               ProcessResponse(response);
            }
            while (response != null);
         }
         else
         {
            base.AfterRequest(request);
         }
      }
   
      private void StartUp(IRoot root, IRequest exportTypeRequest)
      {
         _currentFolder = root.RootFolder;

         Requests.Add(new RootRequest(root.Repository));
         Requests.Add(new GlobalOptionRequest("-q")); // somewhat quiet
         Requests.Add(exportTypeRequest);
         Requests.Add(new ArgumentRequest("-R"));
         Requests.Add(new ArgumentRequest(root.Module));
         Requests.Add(new DirectoryRequest(".", root.Repository + "/" + root.Module));
         Requests.Add(new ExportRequest());
      }
   }
}