﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Domain.Entities.AsasLandZone;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.AgentDataModels.Configurations
{
    public sealed class LZAgentBatchConfiguration : IEntityTypeConfiguration<LZAgentBatch>
    {
        public void Configure(EntityTypeBuilder<LZAgentBatch> entity)
        {
            entity.ToTable("Batches");

            entity.HasKey(x => x.Id);
        }
    }
}