using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Data.Mapping;

namespace PaypalPaymentPlugin.Models.Maps
{
    public class PaypalTranscationMap : EmoEntityTypeConfiguration<PaypalTransaction>
    {
        public override void Configure(EntityTypeBuilder<PaypalTransaction> builder)
        {
            builder.ToTable(nameof(PaypalTransaction));
            builder.HasKey(trans => trans.Id);
            builder.Property(trans => trans.OrderId).IsRequired();
            builder.Property(trans => trans.PaidAmount).IsRequired();
            base.Configure(builder);
        }
    }
}
