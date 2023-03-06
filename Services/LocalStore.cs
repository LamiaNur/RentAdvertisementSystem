using Microsoft.JSInterop;
using System.IO.Compression;
using System.Text;

namespace RentAdvertisementSystem.Services
{
    public class LocalStorage
    {
        private readonly IJSRuntime jsruntime;
		public LocalStorage(IJSRuntime jSRuntime)
		{
			jsruntime = jSRuntime;
		}
        public const string PathName = "LocalStore";
        public async Task SetItemAsync(string key, string value)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "bin", "Debug" , PathName);
            var file = Path.Combine(path, key + ".txt");
            if (Directory.Exists(file))
            {
                Directory.Delete(file);
            }
            await File.WriteAllTextAsync(file, value);
        }

        public async Task<string> GetItemAsync(string key)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "bin", "Debug", PathName);
            var file = Path.Combine(path, key + ".txt");
            if (Directory.Exists(file))
            {
                return await File.ReadAllTextAsync(file);
            }
            return string.Empty;
        }
        public async Task RemoveAsync(string key)
		{
			await jsruntime.InvokeVoidAsync("localStorage.removeItem", key).ConfigureAwait(false);
		}
        public async Task<string> GetStringAsync(string key)
		{
			var str = await jsruntime.InvokeAsync<string>("localStorage.getItem", key).ConfigureAwait(false);
			if (str == null)
				return null;
			var bytes = await Compressor.DecompressBytesAsync(Convert.FromBase64String(str));
			return Encoding.UTF8.GetString(bytes);
		}
        public async Task SaveStringAsync(string key, string value)
		{
			var compressedBytes = await Compressor.CompressBytesAsync(Encoding.UTF8.GetBytes(value));
			await jsruntime.InvokeVoidAsync("localStorage.setItem", key, Convert.ToBase64String(compressedBytes)).ConfigureAwait(false);
		}
    }
    internal class Compressor
	{
		public static async Task<byte[]> CompressBytesAsync(byte[] bytes)
		{
			using (var outputStream = new MemoryStream())
			{
				using (var compressionStream = new GZipStream(outputStream, CompressionLevel.Optimal))
				{
					await compressionStream.WriteAsync(bytes, 0, bytes.Length);
				}
				return outputStream.ToArray();
			}
		}

		public static async Task<byte[]> DecompressBytesAsync(byte[] bytes)
		{
			using (var inputStream = new MemoryStream(bytes))
			{
				using (var outputStream = new MemoryStream())
				{
					using (var compressionStream = new GZipStream(inputStream, CompressionMode.Decompress))
					{
						await compressionStream.CopyToAsync(outputStream);
					}
					return outputStream.ToArray();
				}
			}
		}
	}
}