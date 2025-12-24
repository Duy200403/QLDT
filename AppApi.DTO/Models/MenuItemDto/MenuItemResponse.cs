using System;
using AppApi.DTO.Models.Account;
using AppApi.Entities.Models;

namespace AppApi.DTO.Models.MenuItemDto
{
  public class MenuItemResponse
  {
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Icon { get; set; }
    public string Path { get; set; }
    public string ParentId { get; set; }
    public int OrderNumber { get; set; }

    public MenuItemResponse Parent { get; set; }
    public List<MenuItemResponse> Children { get; set; }
  }
}