using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using PoojaStores.Models.Repositories.Repositories.Interfaces;
using PoojaStores.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Services.Services
{
    public class ImageMgmtService : IImageMgmtService
    {
        private readonly IImageMgmtRepo iRepo;
        public ImageMgmtService(IImageMgmtRepo _iRepo)
        {
            iRepo = _iRepo;
        }
        public ProcessResponse SaveHomeImage(HomePageImages request)
        {
            ProcessResponse response = new ProcessResponse();
            if(request.ImageId>0)
            {
                response = iRepo.UpdateImages(request);
                
            }
            else
            {
                response = iRepo.SaveHomeImage(request);
            }
            return response;
        }
        public HomePageImages GetImageById(int id)
        {
            return iRepo.GetImageById(id);
        }
        public List<ImageDisplayModel> GetAllHomeImages()
        {
            return iRepo.GetAllCatImages();
        }

        public List<HomePageImages> GetAllBanners()
        {
            return iRepo.GetAllBannerImages();

        }
        public List<ImageDisplayModel> GetHomeCatImages()
        {
            return iRepo.GetHomeCatImages();
        }
        public List<ImageDisplayModel> GetAllHomeBanners()
        {
            return iRepo.GetAllHomePageImages();
        }
        public List<ImageDisplayModel> GetProductPageBanners()
        {
            return iRepo.GetAllProductPageImages();
        }
    }
}
