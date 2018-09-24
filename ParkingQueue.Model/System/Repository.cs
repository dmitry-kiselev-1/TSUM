using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;


namespace ParkingQueue.Model
{
    /// <summary>
    /// Репозиторий (работает на основе паттернов Repository и Unit of work)
    /// </summary>
    public partial class Repository : DbContext
    {
        public Repository()
            : base("name=ParkingQueueDataModel")
        {
            //this.Database.CommandTimeout = 60000;

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Model.Log.Put(
                    message: ((Exception)args.ExceptionObject).Message,
                    exception: (Exception)args.ExceptionObject);
            };
        }

        public const byte EntityDatetimePrecision = 3;
        public const byte LogDatetimePrecision = 3;

        public virtual DbSet<Operator> Operator { get; set; }
        public virtual DbSet<OutputReason> OutputReason { get; set; }
        public virtual DbSet<Queue> Queue { get; set; }
        public virtual DbSet<QueueHistory> QueueHistory { get; set; }
        public virtual DbSet<Parameter> Parameter { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Ключи
            modelBuilder.Entity<Queue>().HasKey(t => new { t.ParkingCard });
            modelBuilder.Entity<QueueHistory>().HasKey(t => new { t.Input, t.ParkingCard });

            // Индексы
            /*
            modelBuilder.Entity<Queue>()
                .Property(e => e.Output)
                .HasColumnAnnotation("Index",
                new IndexAnnotation(
                    new IndexAttribute(name: "UK_Queue_Position") { IsUnique = true }
                    ));
            */

            modelBuilder.Entity<QueueHistory>()
                .Property(e => e.Output)
                .HasColumnAnnotation("Index",
                new IndexAnnotation(
                    new IndexAttribute(name: "IX_QueueHistory_Output")));

            modelBuilder.Entity<QueueHistory>()
                .Property(e => e.ParkingCard)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(
                new IndexAttribute(name: "IX_Queue_ParkingCard")));

            // Ссылки
            modelBuilder.Entity<OutputReason>()
                .HasMany(e => e.QueueHistory)
                .WithRequired(e => (OutputReason)e.OutputReason)
                .WillCascadeOnDelete(false);

            // Типы данных

            modelBuilder.Entity<Queue>()
                .Property(e => e.Input)
                .HasColumnType("datetime2")
                .HasPrecision(EntityDatetimePrecision);

            modelBuilder.Entity<Queue>()
                .Property(e => e.Output)
                .HasColumnType("datetime2")
                .HasPrecision(EntityDatetimePrecision);

            modelBuilder.Entity<QueueHistory>()
                .Property(e => e.Input)
                .HasColumnType("datetime2")
                .HasPrecision(EntityDatetimePrecision);

            modelBuilder.Entity<QueueHistory>()
                .Property(e => e.Output)
                .HasColumnType("datetime2")
                .HasPrecision(EntityDatetimePrecision);

            modelBuilder.Entity<Log>()
                .Property(e => e.Date)
                .HasColumnType("datetime2")
                .HasPrecision(LogDatetimePrecision);

            modelBuilder.Entity<Queue>()
                .Property(e => e.RotationStart)
                .HasColumnType("datetime2")
                .HasPrecision(0);

            modelBuilder.Entity<QueueHistory>()
                .Property(e => e.RotationStart)
                .HasColumnType("datetime2")
                .HasPrecision(0);
        }
    }
}
