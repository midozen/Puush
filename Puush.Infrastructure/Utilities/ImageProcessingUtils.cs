using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Puush.Infrastructure.Utilities;

public static class ImageProcessingUtils
{
    public static async Task<MemoryStream> CreateThumbnailAsync(Stream file)
    {
        // await using var stream = file.OpenReadStream();
        using var image = await Image.LoadAsync(file);

        image.Mutate(x => x
            .AutoOrient()
            .Resize(new ResizeOptions
            {
                Size = new Size(170, 170),
                Mode = ResizeMode.Max
            }));

        var thumbnailStream = new MemoryStream();
        await image.SaveAsync(thumbnailStream, new JpegEncoder { Quality = 82 });
        thumbnailStream.Position = 0;

        return thumbnailStream;
    }
}