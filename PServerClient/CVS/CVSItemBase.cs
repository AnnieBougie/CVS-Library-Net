﻿using System;
using System.Collections;
using System.IO;

namespace PServerClient.CVS
{
   /// <summary>
   /// Abstract base class for both repository folders and repository files
   /// </summary>
   public abstract class CVSItemBase : ICVSItem
   {
      protected CVSItemBase(Folder parent)
      {
         Parent = parent;
         Parent.AddItem(this);
      }

      protected CVSItemBase(FileSystemInfo info)
      {
         Info = info;
         Parent = null;
      }

      public abstract void Write();
      public abstract CVSFolder CVSFolder { get; }
      public abstract void Save(bool recursive);
      
      public virtual void Read() { throw new NotSupportedException(); }

      public FileSystemInfo Info { get; protected set; }
      public Folder Parent { get; protected set; }
   }
}