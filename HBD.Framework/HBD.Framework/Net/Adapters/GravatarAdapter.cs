#region using

using System;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;

#endregion

namespace HBD.Framework.Net.Adapters
{
    public class GravatarAdapter
    {
        private const string Url = "http://www.gravatar.com/avatar/{0}.jpg?s={1}&r={2}&d={3}";

        /// <summary>
        ///     Generate Gravatar Url or Image from Email, Size and Rating.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="size"></param>
        /// <param name="rating"></param>
        /// <param name="defaultIcon"></param>
        public GravatarAdapter(string email, short size, GravatarRating rating = GravatarRating.G,
            GravatarDefaultIcon defaultIcon = GravatarDefaultIcon.Identicon)
        {
            Email = email;
            Size = GetLegalSize(size);
            Rating = rating;
            DefaultIcon = defaultIcon;
        }

        public string Email { get; set; }
        public short Size { get; set; }
        public GravatarRating Rating { get; set; }
        public GravatarDefaultIcon DefaultIcon { get; set; }

        private static short GetLegalSize(short size)
            => (short) (size > 512 ? 512 : size < 1 ? 1 : size);

        public Uri GetUrl()
        {
            return new Uri(string.Format(Url,
                Email.GetMd5HashCode(),
                GetLegalSize(Size),
                Rating,
                DefaultIcon == GravatarDefaultIcon.NotFound ? "404" : DefaultIcon.ToString()));
        }

        protected virtual WebRequest CreateRequest() => WebRequest.Create(GetUrl());

        protected virtual Image GetImage(WebResponse response)
        {
            using (var stream = response.GetResponseStream())
            {
                if (stream != null) return Image.FromStream(stream);
            }
            return null;
        }

        public Image GetImage()
        {
            using (var response = CreateRequest().GetResponse())
            {
                return GetImage(response);
            }
        }

        public async Task<Image> GetImageAsync()
        {
            using (var response = await CreateRequest().GetResponseAsync())
            {
                return GetImage(response);
            }
        }
    }

    public enum GravatarRating
    {
        G = 0,
        Pg = 1,
        R = 2,
        X = 4
    }

    public enum GravatarDefaultIcon
    {
        Identicon = 0,
        Monsterid = 1,
        Wavatar = 2,
        NotFound = 4
    }
}