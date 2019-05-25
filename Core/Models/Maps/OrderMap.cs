using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Data.Mapping;

namespace Core.Models.Maps
{
    public class OrderMap : EmoEntityTypeConfiguration<Order>
    {
        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable(nameof(Order));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Buyer).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            
                

            base.Configure(builder);
        }
    }
}
