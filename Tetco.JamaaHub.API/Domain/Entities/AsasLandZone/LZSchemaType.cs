﻿using Domain.BuildingBlocks;

namespace Domain.Entities.AsasLandZone;

public sealed class LZSchemaType:BaseEntity<int>
{
    public string SchemaNameAr { get; set; }
    public string SchemaNameEn { get; set; }
}