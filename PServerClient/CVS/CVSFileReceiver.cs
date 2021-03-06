using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PServerClient.Responses;

namespace PServerClient.CVS
{
   public class CVSFileReceiver
   {
      private readonly IRoot _root;

      public CVSFileReceiver(IRoot root)
      {
         _root = root;
      }

      public void ProcessCheckoutResponses(IList<IResponse> checkOutResponses)
      {
         if (checkOutResponses.Where(r => r.Type == ResponseType.MessageTag).Count() > 1)
         {
            ReceiveMTUpdatedResponses(checkOutResponses);
         }
         else
         {
            ReceiveMUStyleResponses(checkOutResponses);
         }
         //WriteToDisk(_root.ModuleFolder);
      }

      public void WriteToDisk(Folder module)
      {
         module.Write();
         foreach (ICVSItem item in module)
         {
            if (item is Folder)
               WriteToDisk((Folder) item);
            else
               item.Write();
         }
      }

      internal void ReceiveMTUpdatedResponses(IList<IResponse> responses)
      {
         int i = 0;
         IResponse response = responses[i++];
         while (response.Type != ResponseType.Ok)
         {
            IList<IResponse> entryResponses;
            if (response.Type == ResponseType.ModTime)
            {
               entryResponses = new List<IResponse> {response};
               response = responses[i++];
               while (response.Type == ResponseType.MessageTag)
               {
                  MessageTagResponse r = (MessageTagResponse) response;
                  if (r.Message.StartsWith("fname"))
                     entryResponses.Add(response);
                  response = responses[i++];
               }
               entryResponses.Add(response);
               AddNewEntry(entryResponses);
            }
            response = responses[i++];
         }
      }

      internal void ReceiveMUStyleResponses(IList<IResponse> responses)
      {
         //int i = 0;
         //IResponse response = responses[i++];
         //while (response.ResponseType != ResponseType.Ok)
         //{
         //   if (response.ResponseType == ResponseType.Message)
         //   {

         //   }
         //}
      }

      public void AddNewEntry(IList<IResponse> entryResponses)
      {
         IResponse res = entryResponses.Where(r => r.Type == ResponseType.MessageTag).First();
         string[] names = PServerHelper.GetUpdatedFnamePathFile(res.Display());
         string[] folders = names[0].Split(new[] {@"/"}, StringSplitOptions.RemoveEmptyEntries);
         Folder current = CreateFolderStructure(folders);

         string filename = names[1];
         FileInfo fi = new FileInfo(Path.Combine(current.Info.FullName, filename));
         Entry entry = new Entry(fi, current);
         res = entryResponses.Where(r => r.Type == ResponseType.ModTime).First();
         entry.ModTime = ((ModTimeResponse) res).ModTime;
         UpdatedResponse ur = (UpdatedResponse) entryResponses.Where(r => r.Type == ResponseType.Updated).First();
         if (entry.Name == ur.File.Name)
         {
            entry.Revision = ur.File.Revision;
            entry.StickyOption = "";
            entry.Length = ur.File.Length;
            entry.Properties = ur.File.Properties;
            entry.FileContents = ur.File.Contents;
         }
         //current.AddItem(entry);
      }

      public Folder CreateFolderStructure(string[] folders)
      {
         Folder current = _root.RootFolder;
         string repository = _root.Module;
         for (int i = 1; i < folders.Length; i++)
         {
            string folderName = folders[i];
            Folder folder = null;
            foreach (ICVSItem item in current)
            {
               if ((item is Folder) && item.Name == folderName)
                  folder = (Folder) item;
            }
            if (folder == null)
            {
               repository += "/" + folderName;
               DirectoryInfo di = new DirectoryInfo(Path.Combine(current.Info.FullName, folderName));
               folder = new Folder(di, current);
               //current.AddItem(folder);
            }
            current = folder;
         }
         return current;
      }
   }
}