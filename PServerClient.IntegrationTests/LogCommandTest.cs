﻿using NUnit.Framework;
using PServerClient.Commands;
using PServerClient.Connection;
using PServerClient.CVS;
using PServerClient.Tests.TestSetup;

namespace PServerClient.IntegrationTests
{
   /// <summary>
   /// Test of the LogCommand class
   /// </summary>
   [TestFixture]
   public class LogCommandTest
   {
      private IRoot _root;
      private IConnection _connection;

      /// <summary>
      /// Sets up test data
      /// </summary>
      [SetUp]
      public void SetUp()
      {
         _root = new Root(TestConfig.RepositoryPath, TestConfig.ModuleName, TestConfig.CVSHost, TestConfig.CVSPort, TestConfig.Username, TestConfig.Password);
         _connection = new PServerConnection();
      }

      /// <summary>
      /// Tests the log execute.
      /// </summary>
      [Test][Ignore]
      public void TestLogExecute()
      {
         LogCommand command = new LogCommand(_root, _connection);
         command.LocalOnly = true;
         command.Execute();
      }
   }
}
