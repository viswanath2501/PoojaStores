using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Repositories.Interfaces
{
    public interface IProductMainMgmtRepo
    {
        ProcessResponse UpdateProducts(ProductMain request);
        ProductMain GetProductById(int id);
        ProductHomeDisplayModel GetProDisplMdl(int id);
        List<string> ProductOtherImgsUploaded(int id);
        List<ProductListModel> GetProductList(int pageNumber = 1, int pageSize = 10, int catId = 0, int subCateId = 0, string search = "");
        int GetProductsCount(int catId = 0, int subCatId = 0, string search = "");
        List<ProductHomeDisplayModel> GetProductsForCustomer(int catId = 0, int subcatId = 0, string type = "", int pagenumber = 1, int pageSize = 12, string orderBy = "default", int logedId = 0);
        ProductMasterModel GetProductDetail(int id);
        ProcessResponse UpdateProductImages(ProductImages request);

        List<ProductHomeDisplayModel> GetProductsByCatId(int id);
        List<ProductHomeDisplayModel> GetProductsBySubCatId(int id);
        int GetTotalProductsCount();
        int GetCategoryProductCount(int catId);
        int GetSubCategoryProductsCount(int subCatId);
        List<ProductDisplayModel> GetFeaturedProductList(int id = 0);
        ProductHomeDisplayModel GetProductDisplayById(int id,int userId);

    }
}
