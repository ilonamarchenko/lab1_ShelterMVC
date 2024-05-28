using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShelterDomain.Validation;

namespace ShelterDomain.Model;

public partial class Animal
{
    public int AnimalId { get; set; }

    [Display(Name = "ID Притулку")]
    public int ShelterId { get; set; }

    [Display(Name = "Назва")]
    public string Name { get; set; } = null!;

    [Display(Name = "Дата народження")]
    //[RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])-(0[1-9]|1[0-2])-\d{4}$", ErrorMessage = "Введіть дату у форматі dd-mm-yyyy")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [DateNotInFuture(ErrorMessage = "Дата створення не може бути у майбутньому.")]
    public DateTime DateOfBirth { get; set; }

    [Display(Name = "Біологічна стать")]
    public bool Gender { get; set; }

    [Display(Name = "Спеціальні потреби")]
    public string? SpecialNeeds { get; set; }

    public virtual ICollection<Adoption> Adoptions { get; set; } = new List<Adoption>();

    public virtual ICollection<MedicalCard> MedicalCards { get; set; } = new List<MedicalCard>();

    [Display(Name = "Притулок")]
    public virtual Shelter Shelter { get; set; } = null!;
}
