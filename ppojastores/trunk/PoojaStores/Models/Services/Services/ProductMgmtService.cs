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
    public class ProductMgmtService : IProductMgmtService
    {
        private readonly IProductMainMgmtRepo pRepo;
        public ProductMgmtService(IProductMainMgmtRepo repo)
        {
            pRepo = repo;
        }

        public ProcessResponse UpdateProduct(ProductMain request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                response = pRepo.UpdateProducts(request);

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Failed " + ex.Message;
            }

            return response;
        }

        public ProductMain GetProductById(int id)
        {
            return pRepo.GetProductById(id);
        }
        public ProductHomeDisplayModel GetProDisplMdl(int id)
        {
            return pRepo.GetProDisplMdl(id);
        }
        public List<ProductListModel> GetProductList(int pageNumber = 1, int pageSize = 10, int catId = 0, int subCateId = 0, string search = "")
        {
            return pRepo.GetProductList(pageNumber, pageSize, catId, subCateId, search);
        }        
        public int GetProductsCount(int catId=0, int subCatId=0, string ser = "")
        {
            return pRepo.GetProductsCount(catId, subCatId, ser);
        }
        public List<ProductHomeDisplayModel> GetCustomerProducts(int pageNumber = 1, int pageSize = 12, int catId = 0, int subCateId = 0, string search = "", string orderBy = "default", int logedId = 0)
        {
            return pRepo.GetProductsForCustomer(catId, subCateId, search, pageNumber, pageSize, orderBy,logedId);
        }
        public List<string> ProductOtherImgsUploaded(int id)
        {
            return pRepo.ProductOtherImgsUploaded(id);
        }
        public ProcessResponse UpdateProductImages(ProductImages request)
        {
            return pRepo.UpdateProductImages(request);
        }

        public ProductMasterModel GetProductDetail(int id)
        {
            return pRepo.GetProductDetail(id);
        }
        public ProductHomeDisplayModel GetProductDisplayById(int id,int userId)
        {
            return pRepo.GetProductDisplayById(id,userId);
        }
        public List<ProductHomeDisplayModel> GetProductsByCatId(int id)
        {
            return pRepo.GetProductsByCatId(id);
        }
        public List<ProductHomeDisplayModel> GetProductsBySubCatId(int id)
        {
            return pRepo.GetProductsBySubCatId(id);
        }
        public int GetNoOfProducts()
        {
            return pRepo.GetTotalProductsCount();
        }
        public int GetNoOfProductsInCategory(int catId)
        {
            return pRepo.GetCategoryProductCount(catId);
        }
        public int GetNoOfProductsInSubCategory(int subId)
        {
            return pRepo.GetSubCategoryProductsCount(subId);
        }
        public List<ProductDisplayModel> GetFeaturedProductList(int id=0)
        {
            return pRepo.GetFeaturedProductList(id);
        }
    }
}
