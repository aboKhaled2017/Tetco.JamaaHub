﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Domain.Entities.AsasLandZone;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.AgentDataModels.Configurations
{
    public sealed class LZSchemaTypeConfiguration : IEntityTypeConfiguration<LZSchemaType>
    {
        public void Configure(EntityTypeBuilder<LZSchemaType> entity)
        {
            entity.ToTable("SchemaTypes");

            entity.HasKey(x => x.Id);
        }
    }
}
