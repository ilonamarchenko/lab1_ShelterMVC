using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ShelterInfrastructure.Validation;

namespace ShelterDomain.Model;

public partial class Shelter
{
    public static int AnimalId { get; set; }

    [Display(Name = "ID Притулку")]
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    public int ShelterId { get; set; }

    [Display(Name = "Назва")]
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    public string ShelterName { get; set; } = null!;

    [Display(Name = "Адреса")]
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    public string Address { get; set; } = null!;

    [Display(Name = "Номер телефону")]
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [RegularExpression(@"^\+380\d{9}$", ErrorMessage = "Контакти повинні бути в форматі +380XXXXXXXXX.")]
    [UniquePhone(ErrorMessage = "Цей номер телефону вже використовується.")]
    public string Contact { get; set; } = null!;

    public ICollection<Animal> Animals { get; set; } = new List<Animal>();

}