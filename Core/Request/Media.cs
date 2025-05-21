using System.Net;
using LILO_WebEngine.Core.Contracts;

namespace LILO_WebEngine.Core.Request
{
    public class MediaToStreamHandler : IMediaProvider
    {
        private readonly Dictionary<string, MediaFile> _mediaFiles = new Dictionary<string, MediaFile>();

        public async Task<bool> StreamMediaAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            string path = request.Url.LocalPath;

            if (!Path.IsPathRooted(path) || Path.GetInvalidFileNameChars().Any(c => path.Contains(c)))
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.StatusDescription = "File not found";
                return false;
            }

            MediaFile mediaFile = GetMediaFile(path);

            if (mediaFile == null)
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.StatusDescription = "File not found";
                return false;
            }

            response.ContentType = mediaFile.ContentType;
            response.ContentLength64 = mediaFile.Length;

            try
            {
                using (Stream stream = await mediaFile.OpenReadAsync())
                {
                    await stream.CopyToAsync(response.OutputStream, (int)mediaFile.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error streaming media file: {ex.Message}");
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.StatusDescription = "Internal server error";
                return false;
            }

            return true;
        }

        private MediaFile GetMediaFile(string path)
        {
            if (!_mediaFiles.ContainsKey(path))
            {
                MediaFile mediaFile = new MediaFile(path, "audio", new FileInfo(path).Length);
                _mediaFiles.Add(path, mediaFile);
            }

            return _mediaFiles[path];
        }
    }

}
