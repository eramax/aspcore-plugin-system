using Microsoft.EntityFrameworkCore;
using SharedKernel.Data;

namespace PaypalPaymentPlugin.Models
{
    public class PaypalTranscationContext : PluginContext
    {
        public DbSet<PaypalTransaction> PaypalTransactions { get; set; }
        public PaypalTranscationContext(DbContextOptions<PluginContext> options) : base(options)
        {
        }
    }
}
