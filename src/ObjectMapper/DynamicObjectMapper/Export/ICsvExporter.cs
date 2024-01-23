﻿using DynamicObjectMapper.Models;
using System.Collections.Generic;

namespace DynamicObjectMapper.Export
{
    public interface ICsvExporter
    {
        byte[] Export(IDictionary<string, MappingDescription> propertyMappings, IList<dynamic> data, bool includeHeaders = false);

        byte[] Export(string xml, IList<dynamic> data, bool includeHeaders = false);
    }
}