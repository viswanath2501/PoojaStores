using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using PoojaStores.Models.Repositories.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Repositories.Repos
{
    public class ImageMgmtRepo : IImageMgmtRepo
    {
        private readonly MyDbContext context;

        public ImageMgmtRepo(MyDbContext _context)
        {
            this.context = _context;
        }

        public ProcessResponse SaveHomeImage(HomePageImages request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.homePageImages.Add(request);
                context.SaveChanges();
                response.statusMessage = "Success";
                response.currentId = request.ImageId;
                response.statusCode = 1;
            }
            catch(Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Failed";
                LogError(ex);
            }
            return response;
        }
        public List<ImageDisplayModel> GetAllCatImages()
        {
            return (from hi in context.homePageImages
                    join ct in context.categoryMasters on hi.RelatedCategoryId equals ct.CategoryId
                    where hi.IsDeleted == false && (hi.ImageNumber < 10)
                    select new ImageDisplayModel
                    {
                        CategoryName=ct.CategoryName,
                        ImageNumber=hi.ImageNumber,
                        Image=hi.Image,
                        ImageSize=hi.ImageSize,
                        ImageId=hi.ImageId
                        

                    }).OrderBy(b => b.ImageNumber).ToList();
        }
        public List<ImageDisplayModel> GetHomeCatImages()
        {
            return (from hi in context.homePageImages
                    join ct in context.categoryMasters on hi.RelatedCategoryId equals ct.CategoryId
                    where hi.IsDeleted == false && (hi.ImageNumber < 10)
                    select new ImageDisplayModel
                    {
                        CategoryName = ct.CategoryName,
                        ImageNumber = hi.ImageNumber,
                        Image = hi.Image,
                        ImageSize = hi.ImageSize,
                        DiscountPercent=hi.DiscountPercent,
                        TextOnButton=hi.TextOnButton,
                        ImageTitle1=hi.ImageTitle1,
                        ImageTitle2=hi.ImageTitle2,
                        ImageDescription=hi.ImageDescription,
                        OldCost=hi.OldCost,
                        NewCost=hi.NewCost,
                        StartingAt=hi.StartingAt,
                        RelatedCategoryId=hi.RelatedCategoryId



                    }).OrderBy(b => b.ImageNumber).ToList();
        }
        public List<HomePageImages> GetAllBannerImages()
        {
            return context.homePageImages.Where(a => a.IsDeleted == false && (a.ImageNumber == 10 ||a.ImageNumber==11)).ToList();
        }
        public List<ImageDisplayModel> GetAllHomePageImages()
        {
            return (from hi in context.homePageImages
                    join ct in context.categoryMasters on hi.RelatedCategoryId equals ct.CategoryId
                    where hi.IsDeleted == false && (hi.ImageNumber == 10)
                    select new ImageDisplayModel
                    {
                        CategoryName = ct.CategoryName,
                        ImageNumber = hi.ImageNumber,
                        Image = hi.Image,
                        ImageSize = hi.ImageSize,
                        ImageId = hi.ImageId,
                        ImageTitle1=hi.ImageTitle1,
                        ImageTitle2=hi.ImageTitle2,
                        DiscountPercent =hi.DiscountPercent,
                        StartingAt=hi.StartingAt,
                        TextOnButton=hi.TextOnButton,
                        OldCost=hi.OldCost,
                        RelatedCategoryId=hi.RelatedCategoryId
                    }).OrderBy(b => b.ImageNumber).ToList();
        }
        public List<ImageDisplayModel> GetAllProductPageImages()
        {
            return (from hi in context.homePageImages
                    join ct in context.categoryMasters on hi.RelatedCategoryId equals ct.CategoryId
                    where hi.IsDeleted == false && (hi.ImageNumber == 11)
                    select new ImageDisplayModel
                    {
                        CategoryName = ct.CategoryName,
                        ImageNumber = hi.ImageNumber,
                        Image = hi.Image,
                        ImageSize = hi.ImageSize,
                        ImageId = hi.ImageId


                    }).OrderBy(b => b.ImageNumber).ToList();
        }
        public HomePageImages GetImageById(int id)
        {
            return context.homePageImages.Where(a => a.IsDeleted == false && a.ImageId == id).FirstOrDefault();
        }
        public ProcessResponse UpdateImages(HomePageImages request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.ImageId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;
        }
        public void LogError(Exception ex)
        {
            ApplicationErrorLog obj = new ApplicationErrorLog();
            obj.Error = ex.Message != null ? ex.Message : "";
            obj.ExceptionDateTime = DateTime.Now;
            obj.InnerException = ex.InnerException != null ? ex.InnerException.ToString() : "";
            obj.Source = ex.Source;
            obj.Stacktrace = ex.StackTrace != null ? ex.StackTrace : "";
            context.applicationErrorLogs.Add(obj);
            context.SaveChanges();
        }

    }
}
