using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Repositories.Interfaces
{
    public interface IImageMgmtRepo
    {
        ProcessResponse SaveHomeImage(HomePageImages request);
        List<ImageDisplayModel> GetAllCatImages();
        List<HomePageImages> GetAllBannerImages();
        List<ImageDisplayModel> GetAllHomePageImages();
        List<ImageDisplayModel> GetAllProductPageImages();
        HomePageImages GetImageById(int id);
        ProcessResponse UpdateImages(HomePageImages request);
        List<ImageDisplayModel> GetHomeCatImages();
        void LogError(Exception ex);

    }
}
