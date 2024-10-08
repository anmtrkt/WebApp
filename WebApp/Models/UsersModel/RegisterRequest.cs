﻿using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.UsersModel
{
    public class RegisterRequest
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required]
        [Display(Name = "Дата рождения")]
        public DateOnly BirthDate { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = null!;

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; } = null!;

        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; } = null!;

        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Отчество")]
        public string? MiddleName { get; set; }

        [Display(Name = "Пол")]
        public int Sex { get; set; }
    }
}
