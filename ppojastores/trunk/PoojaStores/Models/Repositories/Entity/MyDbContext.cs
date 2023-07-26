using Microsoft.EntityFrameworkCore;
using PoojaStores.Models.ModelClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Users>().ToTable("Users");
            modelBuilder.Entity<ApplicationErrorLog>().ToTable("ApplicationErrorLog");
            modelBuilder.Entity<CategoryMaster>().ToTable("CategoryMaster");
            modelBuilder.Entity<SubCategoryMaster>().ToTable("SubCategoryMaster");
            modelBuilder.Entity<DetailCategoryMaster>().ToTable("DetailCategoryMaster");
            modelBuilder.Entity<UserTypeMaster>().ToTable("UserTypeMaster");
            modelBuilder.Entity<CityMaster>().ToTable("CityMaster");
            modelBuilder.Entity<CountryMaster>().ToTable("CountryMaster");
            modelBuilder.Entity<StateMaster>().ToTable("StateMaster");
            modelBuilder.Entity<PojaItemMaster>().ToTable("PojaItemMaster");
            modelBuilder.Entity<PojaServiceMaster>().ToTable("PojaServiceMaster");
            modelBuilder.Entity<SpecialMaster>().ToTable("SpecialMaster");
            modelBuilder.Entity<DiscountMaster>().ToTable("DiscountMaster");
            modelBuilder.Entity<DeliveryMaster>().ToTable("DeliveryMaster");
            modelBuilder.Entity<ProductMain>().ToTable("ProductMain");
            modelBuilder.Entity<ProductImages>().ToTable("ProductImages");
            modelBuilder.Entity<HomePageImages>().ToTable("HomePageImages");
            modelBuilder.Entity<WishList>().ToTable("WishList");
            modelBuilder.Entity<Cart>().ToTable("Cart");
            modelBuilder.Entity<ProductListModel>().HasNoKey();
            modelBuilder.Entity<ProductGetModel>().HasNoKey();
            modelBuilder.Entity<ProductsCountFromSQL>().HasNoKey();
            modelBuilder.Entity<Address>().ToTable("Address");
            modelBuilder.Entity<AddressTypes>().ToTable("AddressTypes");
            modelBuilder.Entity<PoFollowUp>().ToTable("PoFollowUp");
            modelBuilder.Entity<DispatchDetails>().ToTable("DispatchDetails");
            modelBuilder.Entity<OTPTransactions>().ToTable("OTPTransactions");
            modelBuilder.Entity<ContactUs>().ToTable("ContactUs");
            //modelBuilder.Entity<OtpTransactions>().ToTable("OtpTransactions");
            modelBuilder.Entity<GSTMaster>().ToTable("GSTMaster");
            //modelBuilder.Entity<PayRollMaster>().ToTable("PayRollMaster");
            modelBuilder.Entity<EmailTemplates>().ToTable("EmailTemplates");
            modelBuilder.Entity<MeasurementMaster>().ToTable("MeasurementMaster");
            modelBuilder.Entity<RazorPaymentResult>().ToTable("RazorPaymentResult");
            //modelBuilder.Entity<MonthlyPayRoll>().ToTable("MonthlyPayRoll");
            //modelBuilder.Entity<ModulesMaster>().ToTable("ModulesMaster");
            //modelBuilder.Entity<PrevilegeMaster>().ToTable("PrevilegeMaster");
            //modelBuilder.Entity<HolidayMaster>().ToTable("HolidayMaster");
            //modelBuilder.Entity<ShiftMaster>().ToTable("ShiftMaster");
            //modelBuilder.Entity<InventoryDisplayModel>().HasNoKey();
            //modelBuilder.Entity<RecordsCountFromSql>().HasNoKey();
            //modelBuilder.Entity<InventoryDocuments>().ToTable("InventoryDocuments");
            //modelBuilder.Entity<StockMovementRegister>().ToTable("StockMovementRegister");
            //modelBuilder.Entity<StockMovementDisplayModel>().HasNoKey();
            //modelBuilder.Entity<CompanyMaster>().ToTable("CompanyMaster");
            //modelBuilder.Entity<StockMovementMaster>().ToTable("StockMovementMaster");
            //modelBuilder.Entity<StockMovmentInvoice>().ToTable("StockMovmentInvoice");
            //modelBuilder.Entity<LoginTracking>().ToTable("LoginTracking");
            //modelBuilder.Entity<CustomerMaster>().ToTable("CustomerMaster");
            //modelBuilder.Entity<AddressMaster>().ToTable("AddressMaster");
            //modelBuilder.Entity<CRFQMaster>().ToTable("CRFQMaster");
            //modelBuilder.Entity<QuoteMaster>().ToTable("QuoteMaster");
            //modelBuilder.Entity<SalesOrderMaster>().ToTable("SalesOrderMaster");
            //modelBuilder.Entity<CRFQDetails>().ToTable("CRFQDetails");
            //modelBuilder.Entity<QuoteDetails>().ToTable("QuoteDetails");
            //modelBuilder.Entity<SalesOrderDetails>().ToTable("SalesOrderDetails");
            //modelBuilder.Entity<FinanceGroups>().ToTable("FinanceGroups");
            //modelBuilder.Entity<FinanceHeads>().ToTable("FinanceHeads");
            //modelBuilder.Entity<CartMasterEntity>().ToTable("CartMaster");
            modelBuilder.Entity<CartDetails>().ToTable("CartDetails");
            modelBuilder.Entity<POMaster>().ToTable("POMaster");
            modelBuilder.Entity<PODetails>().ToTable("PODetails");
        }

        public DbSet<Users> users { get; set; }
        public DbSet<AddressTypes> addressTypes { get; set; }
        public DbSet<Address> addresses { get; set; }
        public DbSet<RazorPaymentResult> razorPaymentResults { get; set; }
        public DbSet<ContactUs> contactUs { get; set; }
        //public DbSet<InventoryDocuments> inventoryDocuments { get; set; }
        public DbSet<EmailTemplates> emailTemplates { get; set; }
        public DbSet<GSTMaster> gSTMasters { get; set; }
        //public DbSet<OtpTransactions> OtpTransactions { get; set; }
        //public DbSet<InventoryMaster> inventoryMasters { get; set; }
        //public DbSet<InventoryImages> InventoryImages { get; set; }
        public DbSet<CityMaster> cityMasters { get; set; }
        public DbSet<CountryMaster> countryMasters { get; set; }
        public DbSet<StateMaster> stateMasters { set; get; }
        public DbSet<CartDetails> cartDetails { set; get; }
        public DbSet<POMaster> pOMasters { set; get; }
        public DbSet<PODetails> pODetails { set; get; }
        public DbSet<DispatchDetails> dispatchDetails { get; set; }
        public DbSet<OTPTransactions> oTPTransactions { get; set; }
        //public DbSet<InventoryStatusMaster> inventoryStatusMasters { get; set; }

        //public DbSet<UserMaster> userMasters { set; get; }
        public DbSet<ApplicationErrorLog> applicationErrorLogs { get; set; }
        public DbSet<CategoryMaster> categoryMasters { get; set; }
        public DbSet<SubCategoryMaster> subCategoryMasters { get; set; }
        public DbSet<DetailCategoryMaster> detailCategoryMasters { get; set; }
        public DbSet<UserTypeMaster> userTypeMasters { get; set; }
        public DbSet<DiscountMaster> discountMasters { get; set; }
        public DbSet<DeliveryMaster> deliveryMasters { get; set; }
        public DbSet<ProductMain> productMains { get; set; }
        public DbSet<ProductImages> productImages { get; set; }
        public DbSet<MeasurementMaster> measurementMasters { get; set; }
        public DbSet<HomePageImages> homePageImages { get; set; }
        //public DbSet<MonthlyPayRoll> monthlyPayRolls { get; set; }
        //public DbSet<ModulesMaster> modulesMasters { get; set; }
        //public DbSet<PrevilegeMaster> previlegeMasters { get; set; }
        //public DbSet<HolidayMaster> holidayMasters { get; set; }
        //public DbSet<ShiftMaster> shiftMasters { get; set; }
        //public DbSet<CompanyMaster> companyMasters { get; set; }
        //public DbSet<LoginTracking> loginTracking { get; set; }
        //public DbSet<CustomerMaster> customerMasters { get; set; }
        //public DbSet<AddressMaster> addressMasters { get; set; }
        //public DbSet<CRFQMaster> cRFQMasters { get; set; }
        //public DbSet<QuoteMaster> quoteMasters { get; set; }
        //public DbSet<SalesOrderMaster> salesOrderMasters { get; set; }
        //public DbSet<CRFQDetails> cRFQDetails { get; set; }
        //public DbSet<QuoteDetails> quoteDetails { get; set; }
        //public DbSet<SalesOrderDetails> salesOrderDetails { get; set; }
        public DbSet<WishList> wishLists { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<PojaItemMaster> pojaItemMasters { get; set; }
        public DbSet<PojaServiceMaster> pojaServiceMasters { get; set; }
        public DbSet<SpecialMaster> specialMasters { get; set; }
        public DbSet<PoFollowUp> poFollowUps { get; set; }
    }
}
