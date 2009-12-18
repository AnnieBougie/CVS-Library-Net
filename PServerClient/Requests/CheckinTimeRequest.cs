﻿using System;

namespace PServerClient.Requests
{
   /// <summary>
   /// Checkin-time time \n
   //For the file specified by the next Modified request, use time as the time of
   //the checkin. The time is in the format specified by RFC822 as modified by
   //RFC1123. The client may specify any timezone it chooses; servers will want to
   //convert that to their own timezone as appropriate. An example of this format
   //is:
   //26 May 1997 13:01:40 -0400
   //________________________________________
   //Page 14
   //There is no requirement that the client and server clocks be synchronized. The
   //client just sends its recommendation for a timestamp (based on file timestamps
   //or whatever), and the server should just believe it (this means that the time
   //might be in the future, for example).
   //Note that this is not a general-purpose way to tell the server about the time-
   //stamp of a file; that would be a separate request (if there are servers which can
   //maintain timestamp and time of checkin separately).
   //This request should affect the import request, and may optionally affect the ci
   //request or other relevant requests if any.
   /// </summary>
   public class CheckinTimeRequest : RequestBase
   {
      private DateTime _checkin;

      public CheckinTimeRequest(DateTime time)
      {
         _checkin = time;
      }

      public override bool ResponseExpected { get { return false; } }
      public override RequestType RequestType { get { return RequestType.CheckinTime; } }

      public override string GetRequestString()
      {
         string time = _checkin.ToRfc822();
         return string.Format("{2} {0}{1}", time, LineEnd, RequestName);
      }
   }
}