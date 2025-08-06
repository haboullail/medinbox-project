using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medinbox.Application.Constants
{
    public class Roles
    {
        public const string Reader = "Reader";
        public const string Writer = "Writer";
        public const string ReaderWriter = "ReaderWriter";

        public static readonly Dictionary<string, string> RoleColors = new()
        {
            { Reader, "#007bff" },        
            { Writer, "#28a745" },        
            { ReaderWriter, "#ff9800" }   
        };
    }
}
