using System;
using System.Collections.Generic;
using System.Text;

namespace Deribit.Core.Messages
{
    public class ExcessiveParameterException : Exception
    {
        public ExcessiveParameterException(string objectName, string parameter) : base(
            $"{parameter} is should not be included on {objectName}")
        {

        }
    }
}
