using System;
using System.Collections.Generic;

namespace ShelterDomain.Model;

public partial class Adoption
{
    public int AdoptionId { get; set; }

    public int AnimalId { get; set; }

    public int OwnerId { get; set; }

    public DateOnly AdoptionDate { get; set; }

    public virtual Animal Animal { get; set; } = null!;

    public virtual Owner Owner { get; set; } = null!;
}
