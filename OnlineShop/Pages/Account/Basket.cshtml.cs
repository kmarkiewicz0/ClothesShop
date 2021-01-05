﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Core;
using OnlineShop.Data;

namespace OnlineShop.Pages.Account
{
    [Authorize]
    public class BasketModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IMemberData memberData;
        private readonly IProductData productData;

        public IdentityUser LoggedUser { get; set; }
        public Member UserData { get; set; }
        public List<Product> BasketItems { get; set; }
        public List<int> ItemsId { get; set; }
        public int ItemIdToDelete { get; set; }
        [TempData]
        public string Message { get; set; }

        public BasketModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager
                        , IMemberData memberData, IProductData productData)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.memberData = memberData;
            this.productData = productData;
            BasketItems = new List<Product>();
            ItemsId = new List<int>();
        }
        public async Task<IActionResult> OnGet()
        {
            LoggedUser = await userManager.GetUserAsync(User);
            UserData = memberData.GetMemberById(LoggedUser.Id);
            UserData.Basket = memberData.GetAllBasketItems(UserData.Id).ToList();


            foreach(var item in UserData.Basket)
            {
                BasketItems.Add(productData.GetById(item.ProductId));
                ItemsId.Add(item.Id);
            }

            return Page();
        }
        public async Task<IActionResult> OnPostDelete(int itemId)
        {
            LoggedUser = await userManager.GetUserAsync(User);
            UserData = memberData.GetMemberById(LoggedUser.Id);
            var item = memberData.GetBasketItem(itemId);

            if (item != null)
            {
                if (UserData.Id == item.MemberId)
                {
                    memberData.DeleteFromBasket(itemId);
                    memberData.Commit();
                    TempData["Message"] = "Item deleted!";
                    return RedirectToPage("./Basket");
                }
                else
                {
                    TempData["Message"] = "Nice try! You can not delete this item!";
                    return RedirectToPage("./Basket");
                }
            }
            else
            {
                TempData["Message"] = "Item is not existing";
                return RedirectToPage("./Basket");
            }


        }
    }
}