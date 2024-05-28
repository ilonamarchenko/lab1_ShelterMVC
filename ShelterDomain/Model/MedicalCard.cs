using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShelterDomain.Model;
using ShelterDomain.Validation;


namespace ShelterDomain.Model;

public partial class MedicalCard
{
    [Display(Name = "ID Медичної картки")]
    public int MedicalId { get; set; }
    
    public int AnimalId { get; set; }

    [Display(Name = "Дата створення")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [DateNotInFuture(ErrorMessage = "Дата створення не може бути у майбутньому.")]
    public DateTime DateOfCreation { get; set; }


    [Display(Name = "Опис")]
    public string? Description { get; set; }
    

    [Display(Name = "Тваринка")]
    public virtual Animal Animal { get; set; } = null!;
    
}
