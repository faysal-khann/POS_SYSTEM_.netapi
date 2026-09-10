using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using POS_SYSTEM.Domain.Entities;

namespace POS_SYSTEM.Infrastructure.Persistence;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivityLog> ActivityLogs { get; set; }

    public virtual DbSet<ActivityLog1> ActivityLogs1 { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<ExpenseCategory> ExpenseCategories { get; set; }

    public virtual DbSet<LoyaltyTransaction> LoyaltyTransactions { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductStock> ProductStocks { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<PurchaseItem> PurchaseItems { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SaleItem> SaleItems { get; set; }

    public virtual DbSet<SaleReturn> SaleReturns { get; set; }

    public virtual DbSet<SaleReturnItem> SaleReturnItems { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<StockMovement> StockMovements { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserPermission> UserPermissions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-QR71IO6\\SQLEXPRESS;Database=POS_SYSTEM;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Activity__5E5499A884BD259D");

            entity.ToTable("ActivityLog");

            entity.HasIndex(e => e.CreatedAt, "IX_ActivityLog_CreatedAt").IsDescending();

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.ActionType).HasMaxLength(20);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(45)
                .HasColumnName("IPAddress");
            entity.Property(e => e.Module).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.ActivityLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_ActivityLog_Users");
        });

        modelBuilder.Entity<ActivityLog1>(entity =>
        {
            entity.HasKey(e => e.LogId);

            entity.ToTable("ActivityLogs");

            entity.HasIndex(e => e.CreatedAt, "IX_ActivityLogs_CreatedAt");

            entity.HasIndex(e => e.Module, "IX_ActivityLogs_Module");

            entity.HasIndex(e => e.UserId, "IX_ActivityLogs_UserID");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.Action).HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_ActivityLogs_CreatedAt");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(50)
                .HasColumnName("IPAddress");
            entity.Property(e => e.Module).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.ActivityLog1s)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActivityLogs_User");
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.BranchId).HasName("PK__Branches__A1682FA5648540EC");

            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.BranchCode).HasMaxLength(20);
            entity.Property(e => e.BranchName).HasMaxLength(150);
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ManagerName).HasMaxLength(150);
            entity.Property(e => e.Phone).HasMaxLength(50);

            entity.HasOne(d => d.Company).WithMany(p => p.Branches)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Branches_Companies");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK__Brands__DAD4F3BE0837CB71");

            entity.Property(e => e.BrandId).HasColumnName("BrandID");
            entity.Property(e => e.BrandName).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Active");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2BA85EF0CC");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Active");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK__Companie__2D971C4C3AA76247");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CompanyName).HasMaxLength(200);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Currency).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LogoPath).HasMaxLength(300);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.TaxNo).HasMaxLength(50);
            entity.Property(e => e.Website).HasMaxLength(200);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D8619C6386");

            entity.HasIndex(e => e.CustomerCode, "UQ__Customer__06678521C761A9AB").IsUnique();

            entity.Property(e => e.AddressLine1)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.AddressLine2)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("Bangladesh");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreditLimit)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(12, 2)");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CustomerGroup)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CustomerName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DueAmount)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NationalIdTaxId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Notes).IsUnicode(false);
            entity.Property(e => e.OpeningBalance)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.StateDivision)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Active");
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasIndex(e => e.BranchId, "IX_Expenses_BranchID");

            entity.HasIndex(e => e.ExpenseCategoryId, "IX_Expenses_ExpenseCategoryID");

            entity.HasIndex(e => e.ExpenseDate, "IX_Expenses_ExpenseDate");

            entity.Property(e => e.ExpenseId).HasColumnName("ExpenseID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.ExpenseCategoryId).HasColumnName("ExpenseCategoryID");
            entity.Property(e => e.ExpenseDate).HasDefaultValueSql("(CONVERT([date],sysutcdatetime()))", "DF_Expenses_ExpenseDate");
            entity.Property(e => e.PaymentMethod).HasMaxLength(30);
            entity.Property(e => e.ReferenceNo).HasMaxLength(50);

            entity.HasOne(d => d.Branch).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expenses_Branch");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expenses_CreatedBy");

            entity.HasOne(d => d.ExpenseCategory).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.ExpenseCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expenses_Category");
        });

        modelBuilder.Entity<ExpenseCategory>(entity =>
        {
            entity.HasIndex(e => e.CategoryName, "UQ_ExpenseCategories_Name").IsUnique();

            entity.Property(e => e.ExpenseCategoryId).HasColumnName("ExpenseCategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<LoyaltyTransaction>(entity =>
        {
            entity.HasKey(e => e.LoyaltyId);

            entity.HasIndex(e => e.CreatedAt, "IX_LoyaltyTransactions_CreatedAt");

            entity.HasIndex(e => e.CustomerId, "IX_LoyaltyTransactions_CustomerID");

            entity.HasIndex(e => e.SaleId, "IX_LoyaltyTransactions_SaleID");

            entity.Property(e => e.LoyaltyId).HasColumnName("LoyaltyID");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_LoyaltyTransactions_CreatedAt");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.SaleId).HasColumnName("SaleID");
            entity.Property(e => e.TransactionType).HasMaxLength(20);

            entity.HasOne(d => d.Customer).WithMany(p => p.LoyaltyTransactions)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LoyaltyTransactions_Customer");

            entity.HasOne(d => d.Sale).WithMany(p => p.LoyaltyTransactions)
                .HasForeignKey(d => d.SaleId)
                .HasConstraintName("FK_LoyaltyTransactions_Sale");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__Permissi__EFA6FB0F8D01D9F4");

            entity.HasIndex(e => e.PermissionKey, "UQ_Permissions_Key").IsUnique();

            entity.Property(e => e.PermissionId).HasColumnName("PermissionID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Module).HasMaxLength(50);
            entity.Property(e => e.ParentPermissionId).HasColumnName("ParentPermissionID");
            entity.Property(e => e.PermissionKey).HasMaxLength(100);
            entity.Property(e => e.PermissionName).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Active");

            entity.HasOne(d => d.ParentPermission).WithMany(p => p.InverseParentPermission)
                .HasForeignKey(d => d.ParentPermissionId)
                .HasConstraintName("FK_Permissions_Parent");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6ED2043B81F");

            entity.HasIndex(e => e.ProductCode, "UQ__Products__2F4E024F88D3DFE9").IsUnique();

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Barcode).HasMaxLength(100);
            entity.Property(e => e.BrandId).HasColumnName("BrandID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.ProductCode).HasMaxLength(50);
            entity.Property(e => e.ProductName).HasMaxLength(150);
            entity.Property(e => e.PurchasePrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SalePrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Active");
            entity.Property(e => e.TaxPercent).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.UnitId).HasColumnName("UnitID");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK_Products_Brand");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Category");

            entity.HasOne(d => d.Unit).WithMany(p => p.Products)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Unit");
        });

        modelBuilder.Entity<ProductStock>(entity =>
        {
            entity.HasKey(e => e.ProductStockId).HasName("PK__ProductS__8EA5111AE7A9F84B");

            entity.ToTable("ProductStock");

            entity.HasIndex(e => new { e.ProductId, e.BranchId }, "UQ_ProductStock_Product_Branch").IsUnique();

            entity.Property(e => e.ProductStockId).HasColumnName("ProductStockID");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.LastUpdatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Branch).WithMany(p => p.ProductStocks)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductStock_Branches");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductStocks)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductStock_Products");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.PurchaseId).HasName("PK__Purchase__6B0A6BDEE9768A90");

            entity.HasIndex(e => e.PurchaseNo, "UQ__Purchase__6B0A93146E612CE5").IsUnique();

            entity.Property(e => e.PurchaseId).HasColumnName("PurchaseID");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DiscountAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GrandTotal)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(20)
                .HasDefaultValue("Paid");
            entity.Property(e => e.PaymentTerm).HasMaxLength(50);
            entity.Property(e => e.PurchaseNo).HasMaxLength(30);
            entity.Property(e => e.ReferenceNo).HasMaxLength(100);
            entity.Property(e => e.Remarks).HasMaxLength(500);
            entity.Property(e => e.ShippingCharge)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Completed");
            entity.Property(e => e.SubTotal)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.TaxAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TaxPercent)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Branch).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchases_Branches");

            entity.HasOne(d => d.Company).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchases_Companies");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchases_Suppliers");
        });

        modelBuilder.Entity<PurchaseItem>(entity =>
        {
            entity.HasKey(e => e.PurchaseItemId).HasName("PK__Purchase__B48BB6A7BA3D884D");

            entity.Property(e => e.PurchaseItemId).HasColumnName("PurchaseItemID");
            entity.Property(e => e.BatchNo).HasMaxLength(50);
            entity.Property(e => e.DiscountPercent)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.LineTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.PurchaseId).HasColumnName("PurchaseID");
            entity.Property(e => e.Qty).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SizeId).HasColumnName("SizeID");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.PurchaseItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseItems_Products");

            entity.HasOne(d => d.Purchase).WithMany(p => p.PurchaseItems)
                .HasForeignKey(d => d.PurchaseId)
                .HasConstraintName("FK_PurchaseItems_Purchases");

            entity.HasOne(d => d.Size).WithMany(p => p.PurchaseItems)
                .HasForeignKey(d => d.SizeId)
                .HasConstraintName("FK_PurchaseItems_Sizes");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A6A26409B");

            entity.HasIndex(e => e.RoleName, "UQ_Roles_RoleName").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.RoleName).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Active");
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => e.RolePermissionId).HasName("PK__RolePerm__120F469AD6975E1F");

            entity.HasIndex(e => new { e.RoleId, e.PermissionId }, "UQ_RolePermissions_Role_Permission").IsUnique();

            entity.Property(e => e.RolePermissionId).HasColumnName("RolePermissionID");
            entity.Property(e => e.PermissionId).HasColumnName("PermissionID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("FK_RolePermissions_Permissions");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_RolePermissions_Roles");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasIndex(e => e.BranchId, "IX_Sales_BranchID");

            entity.HasIndex(e => e.CashierId, "IX_Sales_CashierID");

            entity.HasIndex(e => e.CustomerId, "IX_Sales_CustomerID");

            entity.HasIndex(e => e.SaleDate, "IX_Sales_SaleDate");

            entity.HasIndex(e => e.InvoiceNo, "UQ_Sales_InvoiceNo").IsUnique();

            entity.Property(e => e.SaleId).HasColumnName("SaleID");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.CashierId).HasColumnName("CashierID");
            entity.Property(e => e.ChangeAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.InvoiceNo).HasMaxLength(30);
            entity.Property(e => e.PaidAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaymentMethod).HasMaxLength(30);
            entity.Property(e => e.SaleDate)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Sales_SaleDate");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Completed", "DF_Sales_Status");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Branch).WithMany(p => p.Sales)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Branch");

            entity.HasOne(d => d.Cashier).WithMany(p => p.Sales)
                .HasForeignKey(d => d.CashierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Cashier");

            entity.HasOne(d => d.Customer).WithMany(p => p.Sales)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Sales_Customer");
        });

        modelBuilder.Entity<SaleItem>(entity =>
        {
            entity.HasIndex(e => e.ProductId, "IX_SaleItems_ProductID");

            entity.HasIndex(e => e.SaleId, "IX_SaleItems_SaleID");

            entity.Property(e => e.SaleItemId).HasColumnName("SaleItemID");
            entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LineTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Qty)
                .HasDefaultValue(1m, "DF_SaleItems_Qty")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SaleId).HasColumnName("SaleID");
            entity.Property(e => e.TaxRate).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.SaleItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleItems_Product");

            entity.HasOne(d => d.Sale).WithMany(p => p.SaleItems)
                .HasForeignKey(d => d.SaleId)
                .HasConstraintName("FK_SaleItems_Sale");
        });

        modelBuilder.Entity<SaleReturn>(entity =>
        {
            entity.HasKey(e => e.ReturnId);

            entity.HasIndex(e => e.SaleId, "IX_SaleReturns_SaleID");

            entity.HasIndex(e => e.ReturnNo, "UQ_SaleReturns_ReturnNo").IsUnique();

            entity.Property(e => e.ReturnId).HasColumnName("ReturnID");
            entity.Property(e => e.RefundMethod).HasMaxLength(30);
            entity.Property(e => e.ReturnDate)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_SaleReturns_ReturnDate");
            entity.Property(e => e.ReturnNo).HasMaxLength(30);
            entity.Property(e => e.SaleId).HasColumnName("SaleID");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SaleReturns)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleReturns_CreatedBy");

            entity.HasOne(d => d.Sale).WithMany(p => p.SaleReturns)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleReturns_Sale");
        });

        modelBuilder.Entity<SaleReturnItem>(entity =>
        {
            entity.HasKey(e => e.ReturnItemId);

            entity.HasIndex(e => e.ProductId, "IX_SaleReturnItems_ProductID");

            entity.HasIndex(e => e.ReturnId, "IX_SaleReturnItems_ReturnID");

            entity.Property(e => e.ReturnItemId).HasColumnName("ReturnItemID");
            entity.Property(e => e.LineTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Qty)
                .HasDefaultValue(1m, "DF_SaleReturnItems_Qty")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ReturnId).HasColumnName("ReturnID");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.SaleReturnItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleReturnItems_Product");

            entity.HasOne(d => d.Return).WithMany(p => p.SaleReturnItems)
                .HasForeignKey(d => d.ReturnId)
                .HasConstraintName("FK_SaleReturnItems_Return");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.SizeId).HasName("PK__Sizes__83BD095A03C83DF9");

            entity.HasIndex(e => e.SizeName, "UQ_Sizes_SizeName").IsUnique();

            entity.Property(e => e.SizeId).HasColumnName("SizeID");
            entity.Property(e => e.SizeCode).HasMaxLength(20);
            entity.Property(e => e.SizeName).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Active");
        });

        modelBuilder.Entity<StockMovement>(entity =>
        {
            entity.HasKey(e => e.MovementId).HasName("PK__StockMov__D1822466C458C5FA");

            entity.HasIndex(e => new { e.ProductId, e.BranchId, e.CreatedAt }, "IX_StockMovements_Product_Branch_Date");

            entity.Property(e => e.MovementId).HasColumnName("MovementID");
            entity.Property(e => e.BalanceQty).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.MovementType).HasMaxLength(20);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.ProductStockId).HasColumnName("ProductStockID");
            entity.Property(e => e.QtyIn).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.QtyOut).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ReferenceId).HasColumnName("ReferenceID");
            entity.Property(e => e.ReferenceType).HasMaxLength(30);

            entity.HasOne(d => d.Branch).WithMany(p => p.StockMovements)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StockMovements_Branches");

            entity.HasOne(d => d.Product).WithMany(p => p.StockMovements)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StockMovements_Products");

            entity.HasOne(d => d.ProductStock).WithMany(p => p.StockMovements)
                .HasForeignKey(d => d.ProductStockId)
                .HasConstraintName("FK_StockMovements_ProductStock");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE666B4C682B1A2");

            entity.HasIndex(e => e.SupplierCode, "UQ__Supplier__44BE981BDED10A55").IsUnique();

            entity.Property(e => e.AddressLine1)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.AddressLine2)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ContactPerson)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.ContactPersonPhone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("Bangladesh");
            entity.Property(e => e.CreditLimit)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)");
            entity.Property(e => e.DueAmount)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Notes).IsUnicode(false);
            entity.Property(e => e.OpeningBalance)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.StateDivision)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Active");
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SupplierName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TaxVatNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Website)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.UnitId).HasName("PK__Units__44F5EC954236A0FB");

            entity.Property(e => e.UnitId).HasColumnName("UnitID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Active");
            entity.Property(e => e.UnitName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACB88223E7");

            entity.HasIndex(e => e.Email, "UQ_Users_Email").IsUnique();

            entity.HasIndex(e => e.Username, "UQ_Users_Username").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Designation).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .HasColumnName("EmployeeID");
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.PrimaryBranchId).HasColumnName("PrimaryBranchID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Active");
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.PrimaryBranch).WithMany(p => p.Users)
                .HasForeignKey(d => d.PrimaryBranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Branches");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        modelBuilder.Entity<UserPermission>(entity =>
        {
            entity.HasKey(e => e.UserPermissionId).HasName("PK__UserPerm__A90F88D2469A4510");

            entity.HasIndex(e => new { e.UserId, e.PermissionId }, "UQ_UserPermissions_User_Permission").IsUnique();

            entity.Property(e => e.UserPermissionId).HasColumnName("UserPermissionID");
            entity.Property(e => e.PermissionId).HasColumnName("PermissionID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Permission).WithMany(p => p.UserPermissions)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("FK_UserPermissions_Permissions");

            entity.HasOne(d => d.User).WithMany(p => p.UserPermissions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserPermissions_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
