using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using vproker.Models;

namespace vproker.Migrations
{
    [DbContext(typeof(VprokerDbContext))]
    partial class VprokerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348");

            modelBuilder.Entity("vproker.Models.Client", b =>
                {
                    b.Property<string>("ID");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<int>("DiscountPercent");

                    b.Property<byte[]>("DocumentData");

                    b.Property<string>("DocumentGivenBy")
                        .IsRequired();

                    b.Property<DateTime>("DocumentGivenWhen");

                    b.Property<string>("DocumentNumber")
                        .IsRequired();

                    b.Property<string>("DocumentSerial")
                        .IsRequired();

                    b.Property<string>("DocumentUnitCode");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("KnowSourceID")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("LivingAddress");

                    b.Property<string>("MiddleName");

                    b.Property<string>("Note");

                    b.Property<string>("Phone1");

                    b.Property<string>("Phone2");

                    b.Property<string>("Phone3");

                    b.Property<string>("RegistrationAddress");

                    b.Property<string>("WorkingAddress");

                    b.HasKey("ID");
                });

            modelBuilder.Entity("vproker.Models.KnowledgeSource", b =>
                {
                    b.Property<string>("ID");

                    b.Property<string>("Name");

                    b.HasKey("ID");
                });

            modelBuilder.Entity("vproker.Models.Order", b =>
                {
                    b.Property<string>("ID");

                    b.Property<string>("ClientName")
                        .IsRequired();

                    b.Property<string>("ClientPhoneNumber")
                        .IsRequired();

                    b.Property<string>("Description");

                    b.Property<DateTime?>("EndDate");

                    b.Property<decimal>("PaidPledge");

                    b.Property<decimal?>("Payment");

                    b.Property<decimal>("Price");

                    b.Property<DateTime>("StartDate");

                    b.Property<string>("ToolID")
                        .IsRequired();

                    b.HasKey("ID");
                });

            modelBuilder.Entity("vproker.Models.Tool", b =>
                {
                    b.Property<string>("ID");

                    b.Property<decimal>("DayPrice");

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<decimal>("Pledge");

                    b.Property<decimal>("Price");

                    b.Property<decimal>("WorkShiftPrice");

                    b.HasKey("ID");
                });

            modelBuilder.Entity("vproker.Models.Client", b =>
                {
                    b.HasOne("vproker.Models.KnowledgeSource")
                        .WithMany()
                        .HasForeignKey("KnowSourceID");
                });

            modelBuilder.Entity("vproker.Models.Order", b =>
                {
                    b.HasOne("vproker.Models.Tool")
                        .WithMany()
                        .HasForeignKey("ToolID");
                });
        }
    }
}
