using NZWalks3.API.Model;

namespace NZWalks3.API.Repository
{
    public interface IImageRepository
    {
         Task<Image>UploadImage(Image image);
    }
}
