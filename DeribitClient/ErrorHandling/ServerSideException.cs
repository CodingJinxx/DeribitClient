﻿using System;

namespace DeribitClient.Validator
{
    public class ServerSideException : Exception
    {
        public ServerSideException(int errorCode, string shortDescription, string description) : base($"Server Side Error - {DateTime.Now}: {errorCode} - {shortDescription} \"{description}\"")
        {
            
        }
    }
}