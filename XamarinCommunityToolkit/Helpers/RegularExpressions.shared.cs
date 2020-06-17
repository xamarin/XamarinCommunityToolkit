using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinCommunityToolkit.Helpers
{
    public static class RegularExpressions
    {
        // http://regexlib.com/RETester.aspx?regexp_id=167
        public static string Email => @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";
        public static string PhoneNumber_US => @"/^\b\d{3}[-.]?\d{3}[-.]?\d{4}\b$/";
    }
}
