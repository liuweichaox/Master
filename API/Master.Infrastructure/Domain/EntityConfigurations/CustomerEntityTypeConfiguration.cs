using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Master.Domain.Customers;

namespace Master.Infrastructure.Domain.EntityConfigurations
{
    internal sealed class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");
            
            builder.HasKey(b => b.Id);

            builder.Property("WelcomeEmailWasSent").HasColumnName("WelcomeEmailWasSent").HasComment("是否发送过欢迎邮件");
            builder.Property("Email").HasColumnName("Email").HasComment("邮箱");
            builder.Property("Name").HasColumnName("Name").HasComment("姓名");
        }
    }
}