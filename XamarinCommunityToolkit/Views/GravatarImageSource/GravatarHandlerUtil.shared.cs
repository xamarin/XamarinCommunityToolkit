#if !NETSTANDARD
using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	static class GravatarHandlerUtil
	{
		const string requestUriFormat = "https://www.gravatar.com/avatar/{0}?s={1}&d={2}";
		static readonly Lazy<HttpClient> lazyHttp = new Lazy<HttpClient>(() => new HttpClient());

		public static async Task<FileInfo> Load(ImageSource imageSource, float scale, string cacheDirectory)
		{
			if (imageSource is GravatarImageSource gis)
			{
				var cacheFilePath = Path.Combine(cacheDirectory, nameof(GravatarImageSource), CacheFileName(gis, scale));
				var cacheFileInfo = new FileInfo(cacheFilePath);

				if (!UseCacheFile(gis.CachingEnabled, gis.CacheValidity, cacheFileInfo))
				{
					var imageBytes = await GetGravatarAsync(gis.Email, gis.Size, scale, gis.Default).ConfigureAwait(false);
					SaveImage(cacheFileInfo, imageBytes ?? Array.Empty<byte>());
				}

				return cacheFileInfo;
			}

			return null;
		}

		static void SaveImage(FileInfo cacheFileInfo, byte[] imageBytes)
		{
			if (imageBytes.Length < 1)
				return;

			// Delete Cached File
			if (cacheFileInfo.Exists)
				cacheFileInfo.Delete();

			File.WriteAllBytes(cacheFileInfo.FullName, imageBytes);
		}

		static bool UseCacheFile(bool cachingEnabled, TimeSpan cacheValidity, FileInfo file)
		{
			if (!file.Directory.Exists)
				file.Directory.Create();

			return cachingEnabled && file.Exists && file.CreationTime.Add(cacheValidity) > DateTime.Now;
		}

		static string CacheFileName(GravatarImageSource gis, float scale)
			=> $"{GetMd5Hash(gis.Email)}-{gis.Size}@{scale}x.png";

		static async Task<byte[]> GetGravatarAsync(string email, int size, float scale, DefaultGravatar defaultGravatar)
		{
			var requestUri = GetGravatarUri(email, size, scale, defaultGravatar);
			using var response = await lazyHttp.Value.GetAsync(requestUri).ConfigureAwait(false);

			if (!response.IsSuccessStatusCode)
				return Array.Empty<byte>();

			return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
		}

		static string GetGravatarUri(string email, int size, float scale, DefaultGravatar defaultGravatar)
			=> string.Format(requestUriFormat, GetMd5Hash(email), size * scale, DefaultGravatarName(defaultGravatar));

		static string DefaultGravatarName(DefaultGravatar defaultGravatar)
		{
			return defaultGravatar switch
			{
				DefaultGravatar.FileNotFound => "404",
				DefaultGravatar.MysteryPerson => "mp",
				_ => $"{defaultGravatar}".ToLower(),
			};
		}

		static string GetMd5Hash(string str)
		{
			using var md5 = MD5.Create();
			var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

			var sBuilder = new StringBuilder();

			if (hash != null)
			{
				for (var i = 0; i < hash.Length; i++)
					sBuilder.Append(hash[i].ToString("x2"));
			}

			return sBuilder.ToString();
		}
	}
}
#endif
