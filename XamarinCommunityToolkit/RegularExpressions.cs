using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinCommunityToolkit
{
    public static class RegularExpressions
    {
        internal static string Email => @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
        internal static string PhoneNumber_US => @"/^\b\d{3}[-.]?\d{3}[-.]?\d{4}\b$/";
    }
}
