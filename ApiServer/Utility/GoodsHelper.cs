using ApiScheme.Scheme;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ApiServer.Utility
{
    public static class GoodsHelper
    {
        public static List<PurchasableGood> GetGoods(string userId)
        {
            //const string SellerId = "04806629248295947480";
            //const string SellerSecret = "xzFzun3WgEG6nAc1x0rtOQ";
            string SellerId = ConfigurationManager.AppSettings["SellerId"];
            string SellerSecret = ConfigurationManager.AppSettings["SellerSecret"];

            var desc = string.Format("毎月ゲームの運営と品質向上を支援します。(ユーザーID:{0})", userId);
            var descOneTime = string.Format("ゲームの運営と品質向上を支援します。(ユーザーID:{0})", userId);

            var goods = new List<PurchasableGood>();
            goods.Add(PurchasableGood.From(SellerId, SellerSecret, userId, "werewolfgame.apwei.com:Monthly_SupporterSmall", 1.0, "人狼ゲームオンライン2を毎月支援する", desc, 500, 500, "JPY", "500円/月"));
            goods.Add(PurchasableGood.From(SellerId, SellerSecret, userId, "werewolfgame.apwei.com:Monthly_SupporterMedium", 1.0, "人狼ゲームオンライン2を毎月支援する", desc, 2000, 2000, "JPY", "2,000円/月"));
            goods.Add(PurchasableGood.From(SellerId, SellerSecret, userId, "werewolfgame.apwei.com:Monthly_SupporterLarge", 1.0, "人狼ゲームオンライン2を毎月支援する", desc, 10000, 10000, "JPY", "10,000円/月"));

            goods.Add(PurchasableGood.From(SellerId, SellerSecret, userId, "werewolfgame.apwei.com:OneTime_SupporterSmall", 1.0, "人狼ゲームオンライン2を1回支援する", descOneTime, 500, null, "JPY", "500円"));
            goods.Add(PurchasableGood.From(SellerId, SellerSecret, userId, "werewolfgame.apwei.com:OneTime_SupporterMedium", 1.0, "人狼ゲームオンライン2を1回支援する", descOneTime, 2000, null, "JPY", "2,000円"));
            goods.Add(PurchasableGood.From(SellerId, SellerSecret, userId, "werewolfgame.apwei.com:OneTime_SupporterLarge", 1.0, "人狼ゲームオンライン2を1回支援する", descOneTime, 10000, null, "JPY", "10,000円"));
            return goods;
        }
    }
}