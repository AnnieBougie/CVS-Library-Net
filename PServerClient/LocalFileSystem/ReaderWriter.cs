﻿using System;
using System.Collections.Generic;
using System.IO;

namespace PServerClient.LocalFileSystem
{
   public class ReaderWriter : IReaderWriter
   {
      private static IReaderWriter _readerWriter;

      public static IReaderWriter Current
      {
         get
         {
            if (_readerWriter == null)
               _readerWriter = new ReaderWriter();
            return _readerWriter;
         }
         set
         {
            _readerWriter = value;
         }
      }

      //public byte[] ReadFile(string filePath)
      //{
      //   FileInfo file = new FileInfo(filePath);
      //   return ReadFile(file);
      //}

      public byte[] ReadFile(FileInfo file)
      {
         if (!file.Exists)
            throw new IOException(string.Format("The specified file does not exist: {0}", file.FullName));
         byte[] buffer;
         using (FileStream stream = file.Open(FileMode.Open))
         {
            buffer = new byte[file.Length];
            stream.Read(buffer, 0, (int)file.Length);
            stream.Close();
         }
         return buffer;
      }

      public IList<string> ReadFileLines(FileInfo file)
      {
         if (!file.Exists)
            throw new IOException(string.Format("The specified file does not exist: {0}", file.FullName));
         TextReader reader = file.OpenText();
         IList<string> lines = new List<string>();
         string line;
         while ((line = reader.ReadLine()) != null)
         {
            lines.Add(line);
         }
         reader.Close();
         return lines;
      }

      public void WriteFileLines(FileInfo file, IList<string> lines)
      {
         using (TextWriter tw = file.CreateText())
         {
            foreach (string s in lines)
            {
               tw.WriteLine(s);
            }
            tw.Flush();
            tw.Close();
         }
      }

      //public void WriteFile(string filePath, byte[] buffer)
      //{
      //   FileInfo file = new FileInfo(filePath);
      //   WriteFile(file, buffer);

      //}

      public void WriteFile(FileInfo file, byte[] buffer)
      {
         using (FileStream stream = file.Open(FileMode.OpenOrCreate, FileAccess.Write))
         {
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
            stream.Close();
         }
         file.Refresh();
      }


      public bool Exists(FileSystemInfo info)
      {
         return info.Exists;
      }

      //public void CreateDirectory(string dirPath)
      //{
      //   DirectoryInfo dir = new DirectoryInfo(dirPath);
      //   CreateDirectory(dir);
      //}

      public void CreateDirectory(DirectoryInfo dir)
      {
         if (!dir.Exists)
            dir.Create();
          dir.Refresh();
     }

      //public void DeleteDirectory(string dirPath)
      //{
      //   DirectoryInfo dir = new DirectoryInfo(dirPath);
      //   DeleteDirectory(dir);
      //}

     // public void DeleteDirectory(DirectoryInfo dir)
     // {
     //    if (dir.Exists)
     //       dir.Delete(true);
     //     dir.Refresh();
     //}

      //public void DeleteFile(string filePath)
      //{
      //   FileInfo file = new FileInfo(filePath);
      //   DeleteFile(file);
      //}

      //public void DeleteFile(FileInfo file)
      //{
      //   if (file.Exists)
      //      file.Delete();
      //}

      public void Delete(FileSystemInfo info)
      {
         if (info.Exists)
            info.Delete();
         info.Refresh();
      }
   }
}
