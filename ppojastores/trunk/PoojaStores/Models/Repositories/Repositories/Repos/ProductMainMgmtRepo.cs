using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using PoojaStores.Models.Repositories.Repositories.Interfaces;
using PoojaStores.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Repositories.Repos
{
    public class ProductMainMgmtRepo : IProductMainMgmtRepo
    {
        private readonly MyDbContext context;

        public ProductMainMgmtRepo(MyDbContext _db)
        {
            context = _db;
        }

        public ProcessResponse UpdateProducts(ProductMain request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.ProductId > 0)
                {
                    ProductMain pm = context.productMains.Where(a => a.IsDeleted == false && a.ProductId == request.ProductId).FirstOrDefault();
                    context.Entry(pm).CurrentValues.SetValues(request);
                    context.SaveChanges();
                }
                else
                {
                    request.IsDeleted = false;
                    context.productMains.Add(request);
                    context.SaveChanges();
                   
                    response.currentId = request.ProductId;
                }

                response.statusCode = 1;
                response.statusMessage = "Success ";

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
            return context.productMains.Where(a => a.ProductId == id).FirstOrDefault();
        }
        public ProductHomeDisplayModel GetProDisplMdl(int id)
        {
            ProductHomeDisplayModel result = new ProductHomeDisplayModel();
            try
            {
                result = (from p in context.productMains
                          join d in context.discountMasters on p.DiscountMasterId equals d.DiscountId
                          join g in context.gSTMasters on p.GSTMasterId equals g.MasterId
                          where p.IsDeleted==false && p.ProductId==id
                          select new ProductHomeDisplayModel
                          {
                              ProductId = p.ProductId,
                              Title = p.ProductTitle,
                              MinimumOrderQty = (int)p.MinimumOrderQty,
                              QtyAvailable = (int)p.QtyAvailable,
                              Image1 = p.ProductMainImage1,
                              SellingPrice = p.SellingPrice,
                              GSTPrice=g.GSTTaxValue,
                              Discount = d.DiscountPercentage
                          }).FirstOrDefault();
                result.DiscountedPrice = Math.Round((decimal)result.SellingPrice * (1 - ((decimal)result.Discount / 100)), 2);
                result.GSTPrice = (((result.GSTPrice) / 100) * result.DiscountedPrice);
            }
            catch (Exception e) { }
            return result;
        }

        public ProductHomeDisplayModel GetProductDisplayById(int id,int userId)
        {
            return (from p in context.productMains
                    join ct in context.categoryMasters on p.CategoryId equals ct.CategoryId
                    join d in context.discountMasters on p.DiscountMasterId equals d.DiscountId
                    where p.IsDeleted == false && p.ProductId == id
                    select new ProductHomeDisplayModel
                    {
                        Image1=p.ProductMainImage1,
                        Image2=p.ProductMainImage2,
                        Title=p.ProductTitle,
                        QtyAvailable=(int)p.QtyAvailable,
                        CategoryName=ct.CategoryName,
                        SellingPrice=p.SellingPrice,
                        DiscountedPrice= Math.Round((decimal)p.SellingPrice * (1 - ((decimal)d.DiscountPercentage / 100)), 2),
                       MinimumOrderQty = (int)p.MinimumOrderQty,

                    }).FirstOrDefault();

            ProductHomeDisplayModel ph = new ProductHomeDisplayModel();

            ph.IsCart = false;

            if (userId > 0)
            {
                Cart c = context.carts.Where(a => a.IsDeleted == false && a.CurrentStatus == "Open" && a.CartUserId == userId).FirstOrDefault();
                CartDetails cd = context.cartDetails.Where(a => a.IsDeleted == false && a.CartId == c.CartId && a.ProductId == id).FirstOrDefault();
                if (cd != null)
                {
                    ph.IsCart = true;
                }
            }

           
        }

        public List<string> ProductOtherImgsUploaded(int id)
        {
            return context.productImages.Where(a => a.IsDeleted == false && a.ProductId == id).Select(b => b.ImageUrl).ToList();
        }

        public List<ProductListModel> GetProductList(int pageNumber = 1, int pageSize = 10, int catId = 0, int subCateId = 0, string search = "")
        {
            List<ProductListModel> response = new List<ProductListModel>();
            SqlParameter[] sParams =
           {
                new SqlParameter("pageNumber",pageNumber),
                new SqlParameter("pageSize",pageSize),
                new SqlParameter("search", search ?? ""),
                new SqlParameter("categoryId", catId),
                new SqlParameter("subcategory", subCateId ),
            };
            string sp = StoredProcedures.GetAllProducts + " @pageNumber, @pageSize, @categoryId, @subcategory, @search";
            response = context.Set<ProductListModel>().FromSqlRaw(sp, sParams).ToList();
            return response;

        }
        public int GetProductsCount(int catId=0,int subCatId=0,string search = "")
        {
            if (search == null)
            {
                search = "";
            }
            int total = 0;
            //SqlParameter[] sParms =
            //{
            //    new SqlParameter("categoryId",catId),
            //    new SqlParameter("subcategory",subCatId),
            //    new SqlParameter("search",search)
            //};
            //string sp = StoredProcedures.GetProductsCount + "@categoryId,@subcategory,@search";
            //total = context.Set<ProductsCountFromSQL>().FromSqlRaw(sp, sParms).AsEnumerable().Select(r=>r.cnt).FirstOrDefault();

            if (catId > 0)
            {
                if (subCatId > 0)
                {
                    total = (from p in context.productMains
                             join c in context.categoryMasters on p.CategoryId equals c.CategoryId
                             join s in context.subCategoryMasters on p.SubcategoryId equals s.SubCategoryId
                             where p.IsDeleted == false && c.IsDeleted == false && s.IsDeleted == false && c.CategoryId == catId && s.SubCategoryId == subCatId
                                    && (p.ProductTitle.Contains(search) || p.ProductCode.Contains(search))
                             select new ProductMain { }).Count();
                }
                else
                {
                    total = (from p in context.productMains
                             join c in context.categoryMasters on p.CategoryId equals c.CategoryId
                             join s in context.subCategoryMasters on p.SubcategoryId equals s.SubCategoryId
                             where p.IsDeleted == false && c.IsDeleted == false && s.IsDeleted == false && c.CategoryId == catId
                                    && (p.ProductTitle.Contains(search) || p.ProductCode.Contains(search))
                             select new ProductMain { }).Count();
                }
            }
            else
            {
                total = (from p in context.productMains
                         join c in context.categoryMasters on p.CategoryId equals c.CategoryId
                         join s in context.subCategoryMasters on p.SubcategoryId equals s.SubCategoryId
                         where p.IsDeleted == false && c.IsDeleted == false && s.IsDeleted == false
                                && (p.ProductTitle.Contains(search) || p.ProductCode.Contains(search))
                         select new ProductMain { }).Count();
            }
            return total;
        }
        public List<ProductHomeDisplayModel> GetProductsForCustomer(int catId = 0, int subcatId = 0, string type = "", int pageNumber = 1, int pageSize = 12,string orderBy="default",int logedId=0)
        {
            List<ProductHomeDisplayModel> response = new List<ProductHomeDisplayModel>();
            List<ProductGetModel> dbProductsList = new List<ProductGetModel>();
                       
            SqlParameter[] sParams =
           {
                new SqlParameter("pageNumber",pageNumber),
                new SqlParameter("pageSize",pageSize),
                new SqlParameter("search", type ?? ""),
                new SqlParameter("categoryId", catId),
                new SqlParameter("subcategoryId", subcatId ),
                new SqlParameter("OBy", orderBy ),
            };
            string sp = StoredProcedures.GetProductsForCustomer + " @pageNumber, @pageSize, @search, @categoryId, @subcategoryId, @OBy";
            dbProductsList = context.Set<ProductGetModel>().FromSqlRaw(sp, sParams).ToList();
            foreach (ProductGetModel d in dbProductsList)
            {
                ProductHomeDisplayModel gm = new ProductHomeDisplayModel();
                CloneObjects.CopyPropertiesTo(d, gm);
                gm.IsCart = false;
                gm.IsWishlist = false;
                if (logedId > 0)
                {
                    Cart cr = context.carts.Where(a => a.IsDeleted == false && a.CartUserId == logedId && a.CurrentStatus == "Open").FirstOrDefault();
                    if (cr != null)
                    {
                        CartDetails c = context.cartDetails.Where(a => a.IsDeleted == false && a.ProductId == d.ProductId && a.CurrentStatus == "Open" && a.CartId == cr.CartId).FirstOrDefault();
                        if (c != null)
                        {
                            gm.IsCart = true;
                        }
                    }
                    WishList w = context.wishLists.Where(a => a.IsDeleted == false && a.ProductId == d.ProductId && a.UserId == logedId).FirstOrDefault();
                    if (w != null)
                    {
                        gm.IsWishlist = true;
                    }
                }
                gm.DiscountedPrice = Math.Round((decimal)gm.DiscountedPrice,2);
                response.Add(gm);
            }
            return response;           
        }        
        public ProductMasterModel GetProductDetail(int id)
        {
            ProductMasterModel response = new ProductMasterModel();

            response = (from pm in context.productMains
                        join ct in context.categoryMasters on pm.CategoryId equals ct.CategoryId

                        join mm in context.measurementMasters on pm.MeasurementMasterId equals mm.MeasurementId
                        join pi in context.pojaItemMasters on pm.PoojaItemMasterId equals pi.PrId
                        join ps in context.pojaServiceMasters on pm.PoojaServiceMasterId equals ps.PrId
                        join sm in context.specialMasters on pm.SpecialMasterId equals sm.PrId
                        join dl in context.deliveryMasters on pm.DeliveryMasterId equals dl.DeliveryId
                        join ds in context.discountMasters on pm.DiscountMasterId equals ds.DiscountId
                        join gs in context.gSTMasters on pm.GSTMasterId equals gs.MasterId
                        where pm.IsDeleted == false  && pm.ProductId==id
                        select new ProductMasterModel
                        {
                            ProductId=pm.ProductId,
                            CategoryId=pm.CategoryId,
                            CategoryName = ct.CategoryName,
                            MeasurementName = mm.MeasurementName,
                            PoojaItemName = pi.ItemName,
                            PoojaServiceName=ps.ServiceName,
                            SpecialityName=sm.SpecialityName,
                            DeliveryName=dl.DeliveryType,
                            DiscountPercentage=(decimal)ds.DiscountPercentage,
                            GSTName=gs.GSTName,
                            GSTValue=(decimal)gs.GSTTaxValue,
                            PackHeight=pm.PackHeight,
                            ActualWeight=pm.ActualWeight,
                            PackWidth=pm.PackWidth,
                            ProductTitle=pm.ProductTitle,
                            ProductDescription=pm.ProductDescription,
                            //ActualPrice=pm.ActualPrice,
                            SellingPrice=pm.SellingPrice,
                            ProductCode=pm.ProductCode,
                            SQUID=pm.SQUID,
                            BrandName=pm.BrandName,
                            MinimumOrderQty=pm.MinimumOrderQty,
                            QtyAvailable=pm.QtyAvailable,
                            PackLength=pm.PackLength,
                            VolumetricWeight=pm.VolumetricWeight,
                            ProductSize=pm.ProductSize,
                            ProductMainImage1=pm.ProductMainImage1,
                            ProductMainImage2=pm.ProductMainImage2


                        }).FirstOrDefault();

            return response;
        }
        public ProcessResponse UpdateProductImages(ProductImages request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.ImageId > 0)
                {
                    context.Entry(request).CurrentValues.SetValues(request);
                    context.SaveChanges();
                }
                else
                {
                    context.productImages.Add(request);
                    context.SaveChanges();
                }

                response.statusCode = 1;
                response.statusMessage = "Success ";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Failed " + ex.Message;
            }

            return response;
        }

        public List<ProductHomeDisplayModel> GetProductsByCatId(int id)
        {
            List<ProductHomeDisplayModel> pd = new List<ProductHomeDisplayModel>();
             pd= (from pm in context.productMains
                    join ds in context.discountMasters on pm.DiscountMasterId equals ds.DiscountId
                    join ct in context.categoryMasters on pm.CategoryId equals ct.CategoryId
                    where pm.IsDeleted == false && pm.CategoryId == id
                    select new ProductHomeDisplayModel
                    {
                        ProductId=pm.ProductId,
                        CategoryId=pm.CategoryId,
                        CategoryName=ct.CategoryName,
                        Image1=pm.ProductMainImage1,
                        Image2=pm.ProductMainImage2,
                        Title=pm.ProductTitle,
                        SellingPrice=pm.SellingPrice,
                        Discount=ds.DiscountPercentage
                    }).ToList();
            //foreach(var p in pd)
            //{
            //    p.DiscountBeforeRound = Math.Round((decimal)p.SellingPrice * (1 - ((decimal)p.Discount / 100)),2);
            //}

            return pd;
        }
        public int GetTotalProductsCount()
        {
            return context.productMains.Where(a => a.IsDeleted == false).Count();
        }
        public int GetCategoryProductCount(int catId)
        {
            return context.productMains.Where(a => a.IsDeleted == false && a.CategoryId == catId).Count();
        }
        public int GetSubCategoryProductsCount(int subCatId)
        {
            return context.productMains.Where(a => a.IsDeleted == false && a.SubcategoryId == subCatId).Count();
        }

        public List<ProductHomeDisplayModel> GetProductsBySubCatId(int id)
        {
            List<ProductHomeDisplayModel> pd = new List<ProductHomeDisplayModel>();
            //pd= (from pm in context.productMains
            //        join ds in context.discountMasters on pm.DiscountMasterId equals ds.DiscountId
            //        join ct in context.categoryMasters on pm.CategoryId equals ct.CategoryId
            //        join st in context.subCategoryMasters on pm.SubcategoryId equals st.SubCategoryId
            //        where pm.IsDeleted == false && pm.SubcategoryId == id
            //        select new ProductHomeDisplayModel
            //        {
            //            ProductId = pm.ProductId,
            //           SubCategoryId=pm.SubcategoryId,
            //           CategoryId=pm.CategoryId,
            //           CategoryName=ct.CategoryName,
            //           SubCategoryName=st.SubCategoryName,
            //            Image1 = pm.ProductMainImage1,
            //            Image2 = pm.ProductMainImage2,
            //            Title = pm.ProductTitle,
            //            SellingPrice = pm.SellingPrice,
            //            Discount = ds.DiscountPercentage

            //        }).ToList();
            //foreach (var p in pd)
            //{
            //    p.DiscountBeforeRound = Math.Round((decimal)p.SellingPrice * (1 - ((decimal)p.Discount / 100)), 2);
            //}

            return pd;

        }

        public List<ProductDisplayModel> GetFeaturedProductList(int id=0)
        {
            List<ProductDisplayModel> response = new List<ProductDisplayModel>();           
                response = (from pm in context.productMains
                            join ct in context.categoryMasters on pm.CategoryId equals ct.CategoryId
                            join ds in context.discountMasters on pm.DiscountMasterId equals ds.DiscountId

                            join dt in context.measurementMasters on pm.MeasurementMasterId equals dt.MeasurementId
                            where pm.IsDeleted == false&& pm.IsFeatured==true
                            select new ProductDisplayModel
                            {
                                ProductId = pm.ProductId,
                                ProductCode = pm.ProductCode,
                                ProductTitle = pm.ProductTitle,
                                ProductMainImage1 = pm.ProductMainImage1,
                                ProductMainImage2 = pm.ProductMainImage2,
                                ProductDescription = pm.ProductDescription,
                                ProductSize = pm.ProductSize,
                                ProductColor = pm.ProductColor,
                                QtyAvailable = pm.QtyAvailable,
                                SellingPrice = pm.SellingPrice,
                                DiscountedCost= Math.Round(((decimal)pm.SellingPrice * (1 - ((decimal)ds.DiscountPercentage / 100))), 2),
                                CategoryId = pm.CategoryId,
                                Category_Name = ct.CategoryName,
                                SubcategoryId = pm.SubcategoryId,
                                DiscountPercentage = (decimal)ds.DiscountPercentage,
                                Measurement_Name = dt.MeasurementName,
                                BrandName = pm.BrandName,
                                PackHeight = pm.PackHeight,
                                PackLength = pm.PackLength,
                                PackWidth = pm.PackWidth,
                                ActualWeight = pm.ActualWeight,
                                VolumetricWeight = pm.VolumetricWeight,
                                IsFeatured = pm.IsFeatured,
                                MinimumOrderQty=pm.MinimumOrderQty
                            }).ToList();
            if (id >0)
            {
                foreach (ProductDisplayModel d in response)
                {
                    Cart c = context.carts.Where(a => a.IsDeleted == false && a.CartUserId == id && a.CurrentStatus == "Open").FirstOrDefault();
                    if (c != null)
                    {
                        CartDetails crt = context.cartDetails.Where(a => a.IsDeleted == false && a.ProductId == d.ProductId && a.CartId == c.CartId).FirstOrDefault();
                        if (crt != null)
                        {
                            d.isInCart = true;
                        }
                        else
                        {
                            d.isInCart = false;
                        }
                    }
                    else
                    {
                        d.isInCart = false;
                    }
                    WishList ws = context.wishLists.Where(a => a.IsDeleted == false && a.ProductId == d.ProductId && a.UserId == id).FirstOrDefault();
                    {
                        if(ws!=null)
                        {
                            d.isInWishlist = true;
                        }
                        else
                        {
                            d.isInWishlist = false;
                        }
                    }
                }
            }
            return response;
        }
    }
}
