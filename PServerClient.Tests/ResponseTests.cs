using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using NUnit.Framework;
using PServerClient.Responses;
using PServerClient.Tests.TestSetup;

namespace PServerClient.Tests
{
   /// <summary>
   /// Tests for Response classes
   /// </summary>
   [TestFixture]
   public class ResponseTests
   {
      /// <summary>
      /// Test for AuthResponse when authenticate string is received
      /// </summary>
      [Test]
      public void AuthResponseAuthenticatedTest()
      {
         AuthResponse response = new AuthResponse();
         IList<string> lines = new List<string> { "blah\n\r blahI LOVE YOUblah" };
         response.Initialize(lines);
         response.Process();
         Assert.AreEqual(AuthStatus.Authenticated, response.Status);
         Assert.AreEqual(lines.Count, response.LineCount);
      }

      /// <summary>
      /// Test for AuthResponse when not authenticate string is received
      /// </summary>
      [Test]
      public void AuthResponseNotAuthenticatedTest()
      {
         AuthResponse response = new AuthResponse();
         IList<string> lines = new List<string> { "blah\n\r blahI HATE YOUblah" };
         response.Initialize(lines);
         response.Process();
         Assert.AreEqual(AuthStatus.NotAuthenticated, response.Status);
         Assert.AreEqual(lines.Count, response.LineCount);
      }

      /// <summary>
      /// Test for AuthResponse 
      /// </summary>
      [Test]
      public void AuthResponseTest()
      {
         IResponse response = new AuthResponse();
         ResponseTest(response, ResponseType.Auth, 1, "I LOVE YOU", new List<string> { "I LOVE YOU" });
      }

      /// <summary>
      /// Tests the checked in response.
      /// </summary>
      [Test]
      public void TestCheckedInResponse()
      {
         CheckedInResponse response = new CheckedInResponse();
         string contents = "/1 :pserver:abougie@gb-aix-q:2401/usr/local/cvsroot/sandbox AB4%o=wSobI4w\n";
         ResponseTest(response, ResponseType.CheckedIn, 5, "/usr/local/cvsroot/sandbox/mod1/file1.cs", new List<string> { "mod1/", "/usr/local/cvsroot/sandbox/mod1/file1.cs", "/file1.cs/1.2.3.4///", "u=rw,g=rw,o=rw", "74" }, contents);
      }

      /// <summary>
      /// Tests the checksum response.
      /// </summary>
      [Test]
      public void TestChecksumResponse()
      {
         ChecksumResponse response = new ChecksumResponse();
         ResponseTest(response, ResponseType.Checksum, 1, "123", new List<string> { "123" });
         Assert.AreEqual("123", response.CheckSum);
      }

      /// <summary>
      /// Tests the clear static directory response.
      /// </summary>
      [Test]
      public void TestClearStaticDirectoryResponse()
      {
         ClearStaticDirectoryResponse response = new ClearStaticDirectoryResponse();
         IList<string> lines = new List<string> { "mod1/", "/usr/local/cvsroot/sandbox/mod1/" };
         ResponseTest(response, ResponseType.ClearStaticDirectory, 2, "/usr/local/cvsroot/sandbox/mod1/", lines);
         Assert.AreEqual("mod1/", response.ModuleName);
         Assert.AreEqual("/usr/local/cvsroot/sandbox/mod1/", response.RepositoryPath);
      }

      /// <summary>
      /// Tests the clear sticky response.
      /// </summary>
      [Test]
      public void TestClearStickyResponse()
      {
         ClearStickyResponse response = new ClearStickyResponse();
         IList<string> lines = new List<string> { "mod1/", "/usr/local/cvsroot/sandbox/mod1/" };
         ResponseTest(response, ResponseType.ClearSticky, 2, "/usr/local/cvsroot/sandbox/mod1/", lines);
         Assert.AreEqual("mod1/", response.ModuleName);
         Assert.AreEqual("/usr/local/cvsroot/sandbox/mod1/", response.RepositoryPath);
      }

      /// <summary>
      /// Tests the copy file response.
      /// </summary>
      [Test]
      public void TestCopyFileResponse()
      {
         CopyFileResponse response = new CopyFileResponse();
         IList<string> lines = new List<string> { "/usr/local/cvsroot/sandbox/mod1/file1.cs", "/usr/local/cvsroot/sandbox/mod1/newfile1.cs" };
         ResponseTest(response, ResponseType.CopyFile, 2, "/usr/local/cvsroot/sandbox/mod1/file1.cs\n/usr/local/cvsroot/sandbox/mod1/newfile1.cs", lines);
         Assert.AreEqual("/usr/local/cvsroot/sandbox/mod1/file1.cs", response.OriginalFileName);
         Assert.AreEqual("/usr/local/cvsroot/sandbox/mod1/newfile1.cs", response.NewFileName);
      }

      /// <summary>
      /// Tests the created response.
      /// </summary>
      [Test]
      public void TestCreatedResponse()
      {
         CreatedResponse response = new CreatedResponse();
         IList<string> lines = new List<string> { "mod1/", "/usr/local/cvsroot/sandbox/mod1/file1.cs", "/file1.cs/1.2.3.4///", "u=rw,g=rw,o=rw", "74" };
         string contents = "/1 :pserver:abougie@gb-aix-q:2401/usr/local/cvsroot/sandbox AB4%o=wSobI4w\n";
         ResponseTest(response, ResponseType.Created, 5, "/usr/local/cvsroot/sandbox/mod1/file1.cs", lines, contents);
      }

      /// <summary>
      /// Tests the error response.
      /// </summary>
      [Test]
      public void TestErrorResponse()
      {
         ErrorResponse response = new ErrorResponse();
         IList<string> lines = new List<string> { "My error message" };
         ResponseTest(response, ResponseType.Error, 1, "My error message", lines);
         Assert.AreEqual("My error message", response.Message);
     }

      /// <summary>
      /// Tests the file response base process multiple directories.
      /// </summary>
      [Test]
      public void TestFileResponseBaseProcessMultipleDirectories()
      {
         UpdatedResponse response = new UpdatedResponse();
         IList<string> lines = new List<string> { "mod1/mod2/mod3/", "/usr/local/cvsroot/sandbox/mod1/mod2/mod3/file1.cs", "/file1.cs/1.2.3.4///", "u=rw,g=rw,o=rw", "74" };
         response.Initialize(lines);
         response.Process();
         Assert.AreEqual("mod1/mod2/mod3/", response.Module);
         Assert.AreEqual("/file1.cs/1.2.3.4///", response.EntryLine);
         Assert.AreEqual("/usr/local/cvsroot/sandbox/mod1/mod2/mod3/file1.cs", response.RepositoryPath);
         Assert.AreEqual("file1.cs", response.Name);
         Assert.AreEqual("1.2.3.4", response.Revision);
         Assert.AreEqual("u=rw,g=rw,o=rw", response.Properties);
         Assert.AreEqual(74, response.Length);
         Assert.AreEqual(5, response.LineCount);
      }

      /// <summary>
      /// Tests the file response base process.
      /// </summary>
      [Test]
      public void TestFileResponseBaseProcess()
      {
         UpdatedResponse response = new UpdatedResponse();
         IList<string> lines = new List<string> { "mod1/", "/usr/local/cvsroot/sandbox/mod1/file1.cs", "/file1.cs/1.2.3.4///", "u=rw,g=rw,o=rw", "74" };
         response.Initialize(lines);
         response.Process();
         Assert.AreEqual("mod1/", response.Module);
         Assert.AreEqual("/file1.cs/1.2.3.4///", response.EntryLine);
         Assert.AreEqual("/usr/local/cvsroot/sandbox/mod1/file1.cs", response.RepositoryPath);
         Assert.AreEqual("file1.cs", response.Name);
         Assert.AreEqual("1.2.3.4", response.Revision);
         Assert.AreEqual("u=rw,g=rw,o=rw", response.Properties);
         Assert.AreEqual(74, response.Length);
         Assert.AreEqual(5, response.LineCount);
      }

      /// <summary>
      /// Tests the flush response.
      /// </summary>
      [Test]
      public void TestFlushResponse()
      {
         FlushResponse response = new FlushResponse();
         ResponseTest(response, ResponseType.Flush, 1, string.Empty, new List<string> { string.Empty });
      }

      /// <summary>
      /// Tests the mbinary response.
      /// </summary>
      [Test]
      public void TestMbinaryResponse()
      {
         MbinaryResponse response = new MbinaryResponse();
         string contents = "/1 :pserver:abougie@gb-aix-q:2401/usr/local/cvsroot/sandbox AB4%o=wSobI4w\n";
         IList<string> lines = new List<string> { "mod1/", "/usr/local/cvsroot/sandbox/mod1/file1.cs", "/file1.cs/1.2.3.4///", "u=rw,g=rw,o=rw", "74" };
         ResponseTest(response, ResponseType.Mbinary, 5, "/usr/local/cvsroot/sandbox/mod1/file1.cs", lines, contents);
      }

      /// <summary>
      /// Tests the merged response.
      /// </summary>
      [Test]
      public void TestMergedResponse()
      {
         MergedResponse response = new MergedResponse();
         string contents = "/1 :pserver:abougie@gb-aix-q:2401/usr/local/cvsroot/sandbox AB4%o=wSobI4w\n";
         IList<string> lines = new List<string> { "mod1/", "/usr/local/cvsroot/sandbox/mod1/file1.cs", "/file1.cs/1.2.3.4///", "u=rw,g=rw,o=rw", "74" };
         ResponseTest(response, ResponseType.Merged, 5, "/usr/local/cvsroot/sandbox/mod1/file1.cs", lines, contents);
      }

      /// <summary>
      /// Tests the message tag response.
      /// </summary>
      [Test]
      public void TestMessageTagResponse()
      {
         MTMessageResponse response = new MTMessageResponse();
         ResponseTest(response, ResponseType.MTMessage, 1, "My message", new List<string> { "My message" });
         Assert.AreEqual("My message", response.Message);
      }

      /// <summary>
      /// Tests the message response.
      /// </summary>
      [Test]
      public void TestMessageResponse()
      {
         MessageResponse response = new MessageResponse();
         ResponseTest(response, ResponseType.Message, 1, "My message", new List<string> { "My message" });
         Assert.AreEqual("My message", response.Message);
      }

      /// <summary>
      /// Tests the mode response.
      /// </summary>
      [Test]
      public void TestModeResponse()
      {
         ModeResponse response = new ModeResponse();
         ResponseTest(response, ResponseType.Mode, 1, "modemode", new List<string> { "modemode" });
         Assert.AreEqual("modemode", response.Mode);
      }

      /// <summary>
      /// Tests the mod time response.
      /// </summary>
      [Test]
      public void TestModTimeResponse()
      {
         ModTimeResponse response = new ModTimeResponse();
         ResponseTest(response, ResponseType.ModTime, 1, "11/27/2009 2:21:06 PM", new List<string> { "27 Nov 2009 14:21:06 -0000" });
         DateTime expected = new DateTime(2009, 11, 27, 14, 21, 6);
         Assert.AreEqual(expected, response.ModTime);
      }

      /// <summary>
      /// Tests the module expansion response.
      /// </summary>
      [Test]
      public void TestModuleExpansionResponse()
      {
         ModuleExpansionResponse response = new ModuleExpansionResponse();
         IList<string> lines = new List<string> { "mod1" };
         ResponseTest(response, ResponseType.ModuleExpansion, 1, "mod1", lines);
         Assert.AreEqual("mod1", response.ModuleName);
      }

      /// <summary>
      /// Tests the new entry response.
      /// </summary>
      [Test]
      public void TestNewEntryResponse()
      {
         NewEntryResponse response = new NewEntryResponse();
         IList<string> lines = new List<string> { "mod1", "/file1.cs/1.1.1.1///" };
         ResponseTest(response, ResponseType.NewEntry, 2, "mod1\n/file1.cs/1.1.1.1///", lines);
         Assert.AreEqual("file1.cs", response.FileName);
         Assert.AreEqual("1.1.1.1", response.Revision);
      }

      /// <summary>
      /// Tests the notified response.
      /// </summary>
      [Test]
      public void TestNotifiedResponse()
      {
         NotifiedResponse response = new NotifiedResponse();
         IList<string> lines = new List<string> { "/usr/local/cvsroot/sandbox/mod1/file1.cs" };
         ResponseTest(response, ResponseType.Notified, 1, "/usr/local/cvsroot/sandbox/mod1/file1.cs", lines);
      }

      /// <summary>
      /// Tests the unknown response.
      /// </summary>
      [Test]
      public void TestUnknownResponse()
      {
         UnknownResponse response = new UnknownResponse();
         IList<string> lines = new List<string> { "D2009.12.31.13.46.32" };
         ResponseTest(response, ResponseType.Unknown, 1, "D2009.12.31.13.46.32", lines);
      }

      /// <summary>
      /// Tests the ok response.
      /// </summary>
      [Test]
      public void TestOkResponse()
      {
         OkResponse response = new OkResponse();
         ResponseTest(response, ResponseType.Ok, 1, "ok", new List<string> { string.Empty });
      }

      /// <summary>
      /// Tests the patched response.
      /// </summary>
      [Test]
      public void TestPatchedResponse()
      {
         PatchedResponse response = new PatchedResponse();
         IList<string> lines = new List<string> { "mod1/", "/usr/local/cvsroot/sandbox/mod1/file1.cs", "/file1.cs/1.2.3.4///", "u=rw,g=rw,o=rw", "74" };
         string contents = "/1 :pserver:abougie@gb-aix-q:2401/usr/local/cvsroot/sandbox AB4%o=wSobI4w\n";
         ResponseTest(response, ResponseType.Patched, 5, "/usr/local/cvsroot/sandbox/mod1/file1.cs", lines, contents);
      }

      /// <summary>
      /// Tests the RCS diff response.
      /// </summary>
      [Test]
      public void TestRcsDiffResponse()
      {
         RcsDiffResponse response = new RcsDiffResponse();
         IList<string> lines = new List<string> { "mod1/", "/usr/local/cvsroot/sandbox/mod1/file1.cs", "/file1.cs/1.2.3.4///", "u=rw,g=rw,o=rw", "74" };
         string contents = "/1 :pserver:abougie@gb-aix-q:2401/usr/local/cvsroot/sandbox AB4%o=wSobI4w\n";
         ResponseTest(response, ResponseType.RcsDiff, 5, "/usr/local/cvsroot/sandbox/mod1/file1.cs", lines, contents);
     }

      /// <summary>
      /// Tests the removed response.
      /// </summary>
      [Test]
      public void TestRemovedResponse()
      {
         RemovedResponse response = new RemovedResponse();
         IList<string> lines = new List<string> { "/usr/local/cvsroot/sandbox/mod1/file1.cs" };
         ResponseTest(response, ResponseType.Removed, 1, "/usr/local/cvsroot/sandbox/mod1/file1.cs", lines);
         Assert.AreEqual("/usr/local/cvsroot/sandbox/mod1/file1.cs", response.RepositoryPath);
      }

      /// <summary>
      /// Tests the remove entry response.
      /// </summary>
      [Test]
      public void TestRemoveEntryResponse()
      {
         RemoveEntryResponse response = new RemoveEntryResponse();
         IList<string> lines = new List<string> { "/usr/local/cvsroot/sandbox/mod1/file1.cs" };
         ResponseTest(response, ResponseType.RemoveEntry, 1, "/usr/local/cvsroot/sandbox/mod1/file1.cs", lines);
         Assert.AreEqual("/usr/local/cvsroot/sandbox/mod1/file1.cs", response.RepositoryPath);
      }

      /// <summary>
      /// Tests the set static directory response.
      /// </summary>
      [Test]
      public void TestSetStaticDirectoryResponse()
      {
         SetStaticDirectoryResponse response = new SetStaticDirectoryResponse();
         IList<string> lines = new List<string> { "mod1/", "/usr/local/cvsroot/sandbox/mod1/" };
         ResponseTest(response, ResponseType.SetStaticDirectory, 2, "/usr/local/cvsroot/sandbox/mod1/", lines);
         Assert.AreEqual("mod1/", response.ModuleName);
         Assert.AreEqual("/usr/local/cvsroot/sandbox/mod1/", response.RepositoryPath);
     }

      /// <summary>
      /// Tests the set sticky response.
      /// </summary>
      [Test]
      public void TestSetStickyResponse()
      {
         SetStickyResponse response = new SetStickyResponse();
         IList<string> lines = new List<string> { "mod1/", "/usr/local/cvsroot/sandbox/mod1/" };
         ResponseTest(response, ResponseType.SetSticky, 2, "/usr/local/cvsroot/sandbox/mod1/", lines);
         Assert.AreEqual("mod1/", response.ModuleName);
         Assert.AreEqual("/usr/local/cvsroot/sandbox/mod1/", response.RepositoryPath);
      }

      /// <summary>
      /// Tests the template response.
      /// </summary>
      [Test]
      public void TestTemplateResponse()
      {
         TemplateResponse response = new TemplateResponse();
         IList<string> lines = new List<string> { "mod1/", "/usr/local/cvsroot/sandbox/mod1/file1.cs", "/file1.cs/1.2.3.4///", "u=rw,g=rw,o=rw", "74" };
         string contents = "/1 :pserver:abougie@gb-aix-q:2401/usr/local/cvsroot/sandbox AB4%o=wSobI4w\n";
         ResponseTest(response, ResponseType.Template, 5, "/usr/local/cvsroot/sandbox/mod1/file1.cs", lines, contents);
      }

      /// <summary>
      /// Tests the updated response.
      /// </summary>
      [Test]
      public void TestUpdatedResponse()
      {
         UpdatedResponse response = new UpdatedResponse();
         string contents = "/1 :pserver:abougie@gb-aix-q:2401/usr/local/cvsroot/sandbox AB4%o=wSobI4w\n";
         IList<string> lines = new List<string> { "mod1/", "/usr/local/cvsroot/sandbox/mod1/file1.cs", "/file1.cs/1.2.3.4///", "u=rw,g=rw,o=rw", "74" };
         ResponseTest(response, ResponseType.Updated, 5, "/usr/local/cvsroot/sandbox/mod1/file1.cs", lines, contents);

      }

      /// <summary>
      /// Tests the update existing.
      /// </summary>
      [Test]
      public void TestUpdateExisting()
      {
         UpdateExistingResponse response = new UpdateExistingResponse();
         string contents = "/1 :pserver:abougie@gb-aix-q:2401/usr/local/cvsroot/sandbox AB4%o=wSobI4w\n";
         IList<string> lines = new List<string> { "mod1/", "/usr/local/cvsroot/sandbox/mod1/file1.cs", "/file1.cs/1.2.3.4///", "u=rw,g=rw,o=rw", "74" };
         ResponseTest(response, ResponseType.UpdateExisting, 5, "/usr/local/cvsroot/sandbox/mod1/file1.cs", lines, contents);
      }

      /// <summary>
      /// Tests the valid requests response.
      /// </summary>
      [Test]
      public void TestValidRequestsResponse()
      {
         ValidRequestsResponse response = new ValidRequestsResponse();
         string process = "Root Valid-responses valid-requests Repository Directory";
         IList<string> lines = new List<string> { process };
         string display = "Root\r\nValid-responses\r\nvalid-requests\r\nRepository\r\nDirectory\r\n";
         ResponseTest(response, ResponseType.ValidRequests, 1, display, lines);
         Assert.AreEqual(5, response.ValidRequestTypes.Count);
         Assert.AreEqual(RequestType.Root, response.ValidRequestTypes[0]);
         Assert.AreEqual(RequestType.Directory, response.ValidRequestTypes[4]);
      }

      /// <summary>
      /// Tests the wrapper RSC option response.
      /// </summary>
      [Test]
      public void TestWrapperRscOptionResponse()
      {
         WrapperRscOptionResponse response = new WrapperRscOptionResponse();
         string process = "*.cs -k 'b'";
         IList<string> lines = new List<string> { process };
         ResponseTest(response, ResponseType.WrapperRscOption, 1, "*.cs -k 'b'", lines);
      }

      /// <summary>
      /// Tests the response helper pattern.
      /// </summary>
      [Test]
      public void TestResponseHelperPattern()
      {
         // test auth pattern
         string test = "I LOVE YOU blah";
         string pattern = ResponseHelper.ResponsePatterns[0];
         Match m = Regex.Match(test, pattern);
         Assert.IsTrue(m.Success);
         string data = m.Groups["data"].Value;
         Assert.AreEqual("I LOVE YOU", data);

         // test other patterns
         for (int i = 1; i < 32; i++)
         {
            string responseName = ResponseHelper.ResponseNames[i];
            ////Console.WriteLine(responseName);
            test = string.Format("{0} blah", responseName);
            Console.WriteLine(test);
            pattern = ResponseHelper.ResponsePatterns[i];
            m = Regex.Match(test, pattern);
            Assert.IsTrue(m.Success);
            data = m.Groups["data"].Value;
            Assert.AreEqual("blah", data.TrimStart());
         }
      }

      /// <summary>
      /// Test the response
      /// </summary>
      /// <param name="response">The response.</param>
      /// <param name="expectedType">The expected type.</param>
      /// <param name="lineCount">The line count.</param>
      /// <param name="expectedDisplay">The expected display.</param>
      /// <param name="lines">The lines.</param>
      private void ResponseTest(IResponse response, ResponseType expectedType, int lineCount, string expectedDisplay, IList<string> lines)
      {
         ResponseTest(response, expectedType, lineCount, expectedDisplay, lines, string.Empty);
      }

      /// <summary>
      /// Test the response
      /// </summary>
      /// <param name="response">The response.</param>
      /// <param name="expectedType">The expected type.</param>
      /// <param name="lineCount">The line count.</param>
      /// <param name="expectedDisplay">The expected display.</param>
      /// <param name="lines">The response lines.</param>
      /// <param name="fileContents">The file contents.</param>
      private void ResponseTest(IResponse response, ResponseType expectedType, int lineCount, string expectedDisplay, IList<string> lines, string fileContents)
      {
         Assert.AreEqual(lineCount, response.LineCount);
         Assert.AreEqual(expectedType, response.Type);
         response.Initialize(lines);
         Assert.IsFalse(response.Processed);
         response.Process();
         Assert.IsTrue(response.Processed);
         if (response is IFileResponse)
            ((IFileResponse)response).Contents = fileContents.Encode();
         string display = response.Display();
         Console.WriteLine(display);
         Assert.AreEqual(expectedDisplay, display);
         XElement el = response.GetXElement(); 
         bool result = TestHelper.ValidateResponseXML(el);
         Assert.IsTrue(result);
         Console.WriteLine(el.ToString());
         Console.WriteLine("Lines:");
         foreach (string s in response.Lines)
         {
            Console.WriteLine(s);
         }
      }
   }
}