using System;
namespace Slight.Core
{
    public sealed class Cookie
    {
        public DateTime TimeStamp { get; set; }
        public bool Secure { get; set; }
        public string Port { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public DateTime Expires { get; set; }
        public bool Expired { get; set; }
        public bool Discard { get; set; }
        public string Value { get; set; }
        public bool HttpOnly { get; set; }
        public Uri CommentUri { get; set; }
        public string Comment { get; set; }
        public string Domain { get; set; }
        public int Version { get; set; }


        public Cookie()
        {

        }


        internal Cookie(System.Net.Cookie cookie)
        {
            Value = cookie.Value;
            Name = cookie.Name;
            Domain = cookie.Domain;
            Comment = cookie.Comment;
            CommentUri = cookie.CommentUri;
            Discard = cookie.Discard;
            Expired = cookie.Expired;
            Expires = cookie.Expires;
            HttpOnly = cookie.HttpOnly;
            Path = cookie.Path;
            Port = cookie.Port;
            Secure = cookie.Secure;
            TimeStamp = cookie.TimeStamp;
            Version = cookie.Version;
        }
    }
}