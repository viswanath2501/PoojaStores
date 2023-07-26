using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Services.Interfaces
{
    public interface IProductMgmtService
    {
        ProcessResponse UpdateProduct(ProductMain request);
        ProductMain GetProductById(int id);
        ProductHomeDisplayModel GetProDisplMdl(int id);
        List<ProductListModel> GetProductList(int pageNumber = 1, int pageSize = 10, int catId = 0, int subCateId = 0, string search = "");
        int GetProductsCount(int catId = 0, int subCatId = 0, string ser = "");
        List<ProductHomeDisplayModel> GetCustomerProducts(int pageNumber = 1, int pageSize = 10, int catId = 0, int subCateId = 0, string search = "", string orderBy = "default", int logedId = 0);
        List<string> ProductOtherImgsUploaded(int id);
        ProcessResponse UpdateProductImages(ProductImages request);
        ProductMasterModel GetProductDetail(int id);
        List<ProductHomeDisplayModel> GetProductsByCatId(int id);
        List<ProductHomeDisplayModel> GetProductsBySubCatId(int id);
        ProductHomeDisplayModel GetProductDisplayById(int id,int userId);
        int GetNoOfProducts();
        int GetNoOfProductsInCategory(int catId);
        int GetNoOfProductsInSubCategory(int subId);
        List<ProductDisplayModel> GetFeaturedProductList(int id = 0);

    }
}
