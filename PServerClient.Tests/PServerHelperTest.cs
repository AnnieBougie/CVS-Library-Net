﻿using System;
using System.IO;
using NUnit.Framework;
using PServerClient.Tests.TestSetup;

namespace PServerClient.Tests
{
   /// <summary>
   /// Tests for PServerHelper static class
   /// </summary>
   [TestFixture]
   public class PServerHelperTest
   {
      /// <summary>
      /// Test for date time ToEntryString extension method
      /// </summary>
      [Test]
      public void TestConvertDateTimeToEntryFormat()
      {
         DateTime dt = DateTime.Parse("12/11/2009 19:09:26");
         string date = dt.ToEntryString();
         Assert.AreEqual("Fri Dec 11 19:09:26 2009", date);

         dt = dt.AddDays(-5);
         date = dt.ToEntryString();
         Assert.AreEqual("Sun Dec  6 19:09:26 2009", date);
      }

      /// <summary>
      /// Test for date time ToRfc822 method
      /// </summary>
      [Test]
      public void TestConvertDateTimeToRfc822String()
      {
         DateTime dt = DateTime.Parse("1/1/2011 1:52:22 PM");
         string result = dt.ToRfc822();
         Assert.AreEqual("01 Jan 2011 13:52:22 -0000", result);
      }

      /// <summary>
      /// Test for EntryToDateTime method
      /// </summary>
      [Test]
      public void TestConvertEntryFileDateStringToDateTime()
      {
         string test = "Mon Dec  7 23:15:36 2009";
         DateTime dt = test.EntryToDateTime();
         Assert.AreEqual(DateTime.Parse("12/7/2009 11:15:36 PM"), dt);
      }

      /// <summary>
      /// Test for EntryToDateTime exception
      /// </summary>
      [Test]
      [ExpectedException(typeof(ArgumentException))]
      public void TestEntryToDateTimeException()
      {
         string test = "Monday December 7, 2009";
#pragma warning disable 168
         DateTime dt = test.EntryToDateTime();
      }

      /// <summary>
      /// Test for Rfc822ToDateTime string to date time method
      /// </summary>
      [Test]
      public void TestConvertRfc822ToDateTime()
      {
         string dt = "01 Jan 2011 13:52:22 -0000";
         DateTime expected = DateTime.Parse("1/1/2011 1:52:22 PM");
         DateTime result = dt.Rfc822ToDateTime();
         Assert.AreEqual(result, expected);
      }

      /// <summary>
      /// Tests the convert RFC822 to date time bad string format throws exception.
      /// </summary>
      [Test][ExpectedException(typeof(ArgumentException))]
      public void TestConvertRfc822ToDateTimeBadStringFormatThrowsException()
      {
         string dt = "my bad date";
         DateTime result = dt.Rfc822ToDateTime();
      }

      /// <summary>
      /// Test for converting byte array to string
      /// </summary>
      [Test]
      public void TestDecode()
      {
         byte[] decode = new byte[] { 97, 98, 99, 10 };
         string expected = "abc\n";
         string result = decode.Decode();
         Assert.AreEqual(expected, result);
      }

      /// <summary>
      /// Test for converting byte array to string with trailing nulls
      /// </summary>
      [Test]
      public void TestDecodeWithTrailingNulls()
      {
         byte[] decode = new byte[] { 97, 98, 99, 10, 0, 0, 0 };
         string expected = "abc\n";
         string result = decode.Decode();
         Assert.AreEqual(expected, result);
      }

      /// <summary>
      /// Test for converting a string into a byte array
      /// </summary>
      [Test]
      public void TestEncode()
      {
         string encode = "abc";
         byte[] expected = new byte[] { 97, 98, 99 };
         byte[] result = encode.Encode();
         Assert.AreEqual(expected, result);
      }

      /// <summary>
      /// Test of ScramblePassword
      /// </summary>
      [Test]
      public void TestScramble()
      {
         string password = "password";
         Console.WriteLine(password.ScramblePassword());
      }

      /// <summary>
      /// Test for enum string to integer value of matching enum
      /// </summary>
      [Test]
      public void TestStringToEnum()
      {
         string test = "Dec";
         int result = PServerHelper.StringToEnum(typeof(MonthName), test);
         MonthName mon = MonthName.Dec;
         Assert.AreEqual(mon, (MonthName)result);
      }

      /// <summary>
      /// Tests StringToEnum with an invalid string.
      /// </summary>
      [Test][ExpectedException(typeof(FormatException))]
      public void TestStringToEnumInvalidString()
      {
         string test = "Bad";
         int result = PServerHelper.StringToEnum(typeof(MonthName), test);
      }

      /// <summary>
      /// Test for ScramblePassword
      /// </summary>
      [Test]
      public void TestPasswordScramble()
      {
         string password = @"!""%&'()*+z";
         string scrambled = password.ScramblePassword();
         string expected = "Ax5mHlF@LC>";
         Assert.AreEqual(expected, scrambled, "Password was not scrambled correctly");
      }

      /// <summary>
      /// Test for UnscramblePassword
      /// </summary>
      [Test]
      public void TestUnscramblePassword()
      {
         string scrambled = "Ax5mHlF@LC>";
         string expected = @"!""%&'()*+z";
         string unscrambled = scrambled.UnscramblePassword();
         Assert.AreEqual(expected, unscrambled, "Password was not unscrambled correctly");
      }

      /// <summary>
      /// Test for GetRootModuleFolderPath
      /// </summary>
      [Test]
      public void TestGetRootModuleFolderPath()
      {
         DirectoryInfo working = TestConfig.WorkingDirectory;
         string module = "mymod";
         DirectoryInfo di = PServerHelper.GetRootModuleFolderPath(working, module);
         Assert.AreEqual(@"c:\_temp\mymod", di.FullName);

         module = "mymod/project";
         di = PServerHelper.GetRootModuleFolderPath(working, module);
         Assert.AreEqual(@"c:\_temp\mymod\project", di.FullName);
      }

      /// <summary>
      /// Test for GetUpdatedFnamePathFile from fname string.
      /// </summary>
      [Test]
      public void TestGetUpdatedFnamePathFile()
      {
         string test = "fname abougie/cvstest/AssemblyInfo.cs";
         var result = PServerHelper.GetUpdatedFnamePathFile(test);
         string filename = result[1];
         Assert.AreEqual("AssemblyInfo.cs", filename);
         string path = result[0];
         Assert.AreEqual(path, "abougie/cvstest");
      }

      /// <summary>
      /// Tests CommandOption ToRequestString extension method.
      /// </summary>
      [Test]
      public void TestCommandOptionToRequestString()
      {
         CommandOption option = CommandOption.DateSpec;
         string expected = "-D";
         string result = option.ToRequestString();
         Assert.AreEqual(expected, result);
      }

      /// <summary>
      /// Tests the GlobalOption ToRequestString extension method.
      /// </summary>
      [Test]
      public void TestGlobalOptionToRequestString()
      {
         GlobalOption option = GlobalOption.ReallyQuite;
         string expected = "-Q";
         string result = option.ToRequestString();
         Assert.AreEqual(expected, result);
      }

      /// <summary>
      /// Tests the unix line end get.
      /// </summary>
      [Test]
      public void TestUnixLineEndGet()
      {
         string result = PServerHelper.UnixLineEnd;
         Assert.AreEqual("\n", result);
      }

      /// <summary>
      /// Tests the windows line end get.
      /// </summary>
      [Test]
      public void TestWindowsLineEndGet()
      {
         string result = PServerHelper.WindowsLineEnd;
         Assert.AreEqual("\r\n", result);
      }
   }
}