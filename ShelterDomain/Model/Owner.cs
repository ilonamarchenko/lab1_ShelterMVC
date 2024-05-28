using System;
using System.Collections.Generic;

namespace ShelterDomain.Model;

public partial class Owner
{
    public int OwnerId { get; set; }

    public string FullName { get; set; } = null!;

    public string Contact { get; set; } = null!;

    public string? Comment { get; set; }

    public virtual ICollection<Adoption> Adoptions { get; set; } = new List<Adoption>();
}
