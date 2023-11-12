using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SmartLocker.Software.Backend.Entities
{
    public partial class SmartLockerContext : DbContext
    {
        public SmartLockerContext()
        {
        }

        public SmartLockerContext(DbContextOptions<SmartLockerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccountPaymentInfo> AccountPaymentInfo { get; set; }
        public virtual DbSet<Accounts> Accounts { get; set; }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<CardType> CardType { get; set; }
        public virtual DbSet<EventTypes> EventTypes { get; set; }
        public virtual DbSet<FormRequestLockers> FormRequestLockers { get; set; }
        public virtual DbSet<Locations> Locations { get; set; }
        public virtual DbSet<LockerAmount> LockerAmount { get; set; }
        public virtual DbSet<LockerDiagrams> LockerDiagrams { get; set; }
        public virtual DbSet<LockerRooms> LockerRooms { get; set; }
        public virtual DbSet<LockerSizes> LockerSizes { get; set; }
        public virtual DbSet<Lockers> Lockers { get; set; }
        public virtual DbSet<Notifications> Notifications { get; set; }
        public virtual DbSet<PartnerDetail> PartnerDetail { get; set; }
        public virtual DbSet<RateTypes> RateTypes { get; set; }
        public virtual DbSet<RepairForm> RepairForm { get; set; }
        public virtual DbSet<RepairLockerRoom> RepairLockerRoom { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<ServiceRate> ServiceRate { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }
        public virtual DbSet<Transfers> Transfers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=157.245.203.196;Initial Catalog=SmartLocker;User ID=sa;Password=ab8mbu3t053tEn8;Max Pool Size=50000;Pooling=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountPaymentInfo>(entity =>
            {
                entity.HasKey(e => e.InfoId)
                    .HasName("PK__ACCOUNT___072F0527CB7A8F9E");

                entity.ToTable("ACCOUNT_PAYMENT_INFO");

                entity.Property(e => e.CardName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CardNumber)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ExpDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountPaymentInfo)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ACCOUNT_PAYMENT_INFO_ACCOUNT");

                entity.HasOne(d => d.CardType)
                    .WithMany(p => p.AccountPaymentInfo)
                    .HasForeignKey(d => d.CardTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ACCOUNT_PAYMENT_INFO_CARD_TYPE");
            });

            modelBuilder.Entity<Accounts>(entity =>
            {
                entity.HasKey(e => e.AccountId)
                    .HasName("PK__ACCOUNTS__B19E45E9B0B94FF9");

                entity.ToTable("ACCOUNTS");

                entity.Property(e => e.AccountCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ApproveDate).HasColumnType("datetime");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.IdCard)
                    .IsRequired()
                    .HasMaxLength(13);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Status)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ACCOUNTS_ROLE");
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("ADDRESS");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.District)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.Province)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SubDistrict)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("BOOKING");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.PassCode)
                    .IsRequired()
                    .HasMaxLength(6);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.LkRoom)
                    .WithMany(p => p.Booking)
                    .HasForeignKey(d => d.LkRoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BOOKING_LOCKER_ROOMS");
            });

            modelBuilder.Entity<CardType>(entity =>
            {
                entity.ToTable("CARD_TYPE");

                entity.Property(e => e.CardTypeImgUrl).IsRequired();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TypeCardName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<EventTypes>(entity =>
            {
                entity.HasKey(e => e.EventId)
                    .HasName("PK__EVENT_TY__7944C810337392D4");

                entity.ToTable("EVENT_TYPES");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.EventName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<FormRequestLockers>(entity =>
            {
                entity.HasKey(e => e.FormId)
                    .HasName("PK__FORM_REQ__332ADA95DEC605C2");

                entity.ToTable("FORM_REQUEST_LOCKERS");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.FormCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OptionalRequest).IsUnicode(false);

                entity.Property(e => e.Remark).IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.FormRequestLockers)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FORM_REQUEST_LOCKERS_ACCOUNTS");

                entity.HasOne(d => d.Locker)
                    .WithMany(p => p.FormRequestLockers)
                    .HasForeignKey(d => d.LockerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FORM_REQUEST_LOCKERS_LOCKERS");
            });

            modelBuilder.Entity<Locations>(entity =>
            {
                entity.HasKey(e => e.LocateId)
                    .HasName("PK__LOCATION__D1A22D0958006BF8");

                entity.ToTable("LOCATIONS");

                entity.Property(e => e.ApproveDate).HasColumnType("datetime");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Latitude)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.LocateName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Longtitude)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Remark).IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LOCATIONS_ACCOUNTS");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LOCATIONS_ADDRESS");
            });

            modelBuilder.Entity<LockerAmount>(entity =>
            {
                entity.HasKey(e => e.LkAmountId)
                    .HasName("PK__LOCKER_A__BCF365E00A0ABD19");

                entity.ToTable("LOCKER_AMOUNT");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Form)
                    .WithMany(p => p.LockerAmount)
                    .HasForeignKey(d => d.FormId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LOCKER_AMOUNT_FORM_REQUEST_LOCKERS");

                entity.HasOne(d => d.LkSize)
                    .WithMany(p => p.LockerAmount)
                    .HasForeignKey(d => d.LkSizeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LOCKER_AMOUNT_LOCKER_SIZES");
            });

            modelBuilder.Entity<LockerDiagrams>(entity =>
            {
                entity.HasKey(e => e.LkDiagramId)
                    .HasName("PK__LOCKER_D__91DBF6E86836A054");

                entity.ToTable("LOCKER_DIAGRAMS");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.LkRoom)
                    .WithMany(p => p.LockerDiagrams)
                    .HasForeignKey(d => d.LkRoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LOCKER_DIAGRAMS_LOCKER_ROOMS");
            });

            modelBuilder.Entity<LockerRooms>(entity =>
            {
                entity.HasKey(e => e.LkRoomId)
                    .HasName("PK__LOCKER_R__C3A04E5CD1C8ACE7");

                entity.ToTable("LOCKER_ROOMS");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.LkRoomCode)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.LkSize)
                    .WithMany(p => p.LockerRooms)
                    .HasForeignKey(d => d.LkSizeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LOCKER_ROOMS_LOCKER_SIZES");

                entity.HasOne(d => d.Locker)
                    .WithMany(p => p.LockerRooms)
                    .HasForeignKey(d => d.LockerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LOCKER_ROOMS_LOCKERS");
            });

            modelBuilder.Entity<LockerSizes>(entity =>
            {
                entity.HasKey(e => e.LkSizeId)
                    .HasName("PK__LOCKER_S__6DFDCE9EE3C4CEA9");

                entity.ToTable("LOCKER_SIZES");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.LkSizeName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Lockers>(entity =>
            {
                entity.HasKey(e => e.LockerId)
                    .HasName("PK__LOCKERS__9A451EFC2D615A15");

                entity.ToTable("LOCKERS");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.LockerCode)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Locate)
                    .WithMany(p => p.Lockers)
                    .HasForeignKey(d => d.LocateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LOCKERS_LOCATIONS");
            });

            modelBuilder.Entity<Notifications>(entity =>
            {
                entity.HasKey(e => e.NotiId)
                    .HasName("PK__NOTIFICA__EDC08E924E03A7ED");

                entity.ToTable("NOTIFICATIONS");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ReadStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NOTIFICATIONS_ACCOUNTS");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NOTIFICATIONS_EVENT_TYPES");
            });

            modelBuilder.Entity<PartnerDetail>(entity =>
            {
                entity.ToTable("PARTNER_DETAIL");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.PartnerAddress)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.PartnerNumber)
                    .IsRequired()
                    .HasMaxLength(13);

                entity.Property(e => e.PartnerType)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.TelNo)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.PartnerDetail)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PARTNER_DETAIL_ACCOUNTS");
            });

            modelBuilder.Entity<RateTypes>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK__RATE_TYP__FE91E1E6432CE037");

                entity.ToTable("RATE_TYPES");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<RepairForm>(entity =>
            {
                entity.ToTable("REPAIR_FORM");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.RepairForm)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_REPAIR_FORM_ACCOUNTS");
            });

            modelBuilder.Entity<RepairLockerRoom>(entity =>
            {
                entity.HasKey(e => e.RepairLkRoomId);

                entity.ToTable("REPAIR_LOCKER_ROOM");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.RepairForm)
                    .WithMany(p => p.RepairLockerRoom)
                    .HasForeignKey(d => d.RepairFormId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_REPAIR_LOCKER_ROOM_REPAIR_FORM");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__ROLES__D80AB4BBF49C683E");

                entity.ToTable("ROLES");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ServiceRate>(entity =>
            {
                entity.HasKey(e => e.Srid)
                    .HasName("PK__SERVICE___1D8D1061709B3D58");

                entity.ToTable("SERVICE_RATE");

                entity.Property(e => e.Srid).HasColumnName("SRId");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SurplusPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.LkSize)
                    .WithMany(p => p.ServiceRate)
                    .HasForeignKey(d => d.LkSizeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SERVICE_RATE_LOCKER_SIZES");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.ServiceRate)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SERVICE_RATE_RATE_TYPES");
            });

            modelBuilder.Entity<Transactions>(entity =>
            {
                entity.HasKey(e => e.TransferId)
                    .HasName("PK_TRANSACTIONS_1");

                entity.ToTable("TRANSACTIONS");

                entity.Property(e => e.TransferId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Amont).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TRANSACTIONS_ACCOUNTS");

                entity.HasOne(d => d.RateType)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.RateTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TRANSACTIONS_RATE_TYPES");
            });

            modelBuilder.Entity<Transfers>(entity =>
            {
                entity.HasKey(e => e.TransferId)
                    .HasName("PK__TRANSFER__1CAAE2AC523C9A6B");

                entity.ToTable("TRANSFERS");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.ResBooking)
                    .WithMany(p => p.TransfersResBooking)
                    .HasForeignKey(d => d.ResBookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TRANSFERS_RESBOOKING");

                entity.HasOne(d => d.TransBooking)
                    .WithMany(p => p.TransfersTransBooking)
                    .HasForeignKey(d => d.TransBookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TRANSFERS_TRANSBOOKING");
            });

            modelBuilder.HasSequence("CountBy1");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
