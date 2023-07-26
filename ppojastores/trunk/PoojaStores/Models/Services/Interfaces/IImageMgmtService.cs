using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Services.Interfaces
{
    public interface IImageMgmtService
    {
        
        ProcessResponse SaveHomeImage(HomePageImages request);
        HomePageImages GetImageById(int id);
        List<ImageDisplayModel> GetAllHomeImages();
        List<HomePageImages> GetAllBanners();
        List<ImageDisplayModel> GetAllHomeBanners();
        List<ImageDisplayModel> GetHomeCatImages();
        List<ImageDisplayModel> GetProductPageBanners();

    }
}
