using Microsoft.EntityFrameworkCore;
using System;

namespace GeoSensePlus.Data.DatabaseModels;

public class NamedEntity<T>
{
    public T Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}