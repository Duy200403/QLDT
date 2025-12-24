using AppApi.DTO.Models.RoleDto;
using AppApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppApi.DTO.Models.Account
{
  public class AccountResponse
  {
    public AccountResponse()
    {
    }
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Pseudonym { get; set; }
    public string Email { get; set; }
    public List<RoleResponse> Roles { get; set; }
    public bool IsActive { get; set; }
  }
}