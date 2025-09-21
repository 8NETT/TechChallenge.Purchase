using TechChallenge.Purchases.Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TechChallenge.Purchases.Infrastructure.Configurations
{
    internal class CompraConfigurations : IEntityTypeConfiguration<Compra>
    {
        public void Configure(EntityTypeBuilder<Compra> builder)
        {
            builder.ToTable("Compra");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnType("INT").UseIdentityColumn();
            builder.Property(c => c.CompradorId).HasColumnType("INT").IsRequired();
            builder.Property(c => c.JogoId).HasColumnType("INT").IsRequired();
            builder.Property(c => c.CreatedAt).HasColumnType("DATETIME").IsRequired();
            builder.Property(c => c.Valor).HasColumnType("DECIMAL(4,2)").IsRequired();
            builder.Property(c => c.Desconto).HasColumnType("INT").IsRequired();
            builder.Property(c => c.Total).HasColumnType("DECIMAL(4,2)").IsRequired();
            builder.Property(c => c.PaymentMethodType).HasColumnType("INT").IsRequired();
        }
    }
}