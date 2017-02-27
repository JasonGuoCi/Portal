using Envision.SPS.Utility.Enums;
using Envision.SPS.Utility.Exceptions;
using Envision.SPS.Utility.Utilities;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envision.SPS.Utility.Handlers
{
    public class PortalHandler : PortalBase
    {
        /// <summary>
        /// 获取天气信息
        /// </summary>
        /// <创建标识>孙晓龙</创建标识>
        /// <创建时间>2016-03-22</创建时间>
        /// <returns></returns>
        public static string GetWeather()
        {
            try
            {
                List<WeatherModel> weatherList = new List<WeatherModel>();
                var cache = CacheHelper.Get(IBKeys.WeatherYahoo);
                if (cache != null)
                {
                    DateTime dt = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now, TimeZoneInfo.Local);
                    weatherList = (List<WeatherModel>)cache;
                    foreach (var wt in weatherList)
                    {
                        if (wt.Location.ToUpper() == "SANTA CLARA")
                        {
                            //硅谷(聖塔克拉拉)
                            wt.CurrentDate = IBUtils.GetGuiGuDate(dt);
                            wt.CurrentTime = IBUtils.GetGuiGuTime(dt);
                        }
                        else if (wt.Location.ToUpper() == "SILKEBORG")
                        {
                            //锡尔克堡（丹麦）
                            wt.CurrentDate = IBUtils.GetXiErKeBaoDate(dt);
                            wt.CurrentTime = IBUtils.GetXiErKeBaoTime(dt);
                        }
                        else if (wt.Location.ToUpper() == "HOUSTON")
                        {
                            //休斯顿
                            wt.CurrentDate = IBUtils.GetXiuSiDunDate(dt);
                            wt.CurrentTime = IBUtils.GetXiuSiDunTime(dt);
                        }
                    }
                }
                else
                {
                    weatherList = Weather.ReadParseXml();
                    CacheHelper.Insert(IBKeys.WeatherYahoo, weatherList, 30);
                }
                return Util.WriteJsonpToResponse(ResponseStatus.Success, weatherList);
            }
            catch (OptionException)
            {
                return Util.WriteJsonpToResponse(ResponseStatus.Noneffect);
            }
            catch (Exception exception)
            {
                return Util.WriteJsonpToResponse(ResponseStatus.Exception, exception.Message);
            }
        }

        /// <summary>
        /// 获取通知公告信息
        /// </summary>
        /// <创建标识>孙晓龙</创建标识>
        /// <创建时间>2016-03-22</创建时间>
        /// <returns></returns>
        public static string GetAnnouncement()
        {
            try
            {
                new PortalBase();
                string weburl = SPContext.Current.Web.Url;
                List<object> AnnouncementList = new List<object>();
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite spSite = new SPSite(weburl))
                    {
                        spSite.AllowUnsafeUpdates = true;
                        using (SPWeb web = spSite.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            SPListItemCollection announcementListIitem = GetAnnouncementListItems(web, EnvisionPagesConfig.EnvsionAnnouncement);
                            foreach (SPListItem listitem in announcementListIitem)
                            {
                                AnnouncementList.Add(new
                                {
                                    id = listitem.ID,
                                    title = listitem.Title,
                                    publishedDate = listitem["PublishedDate"] == null ? "" : IBUtils.ObjectToDateTime(listitem["PublishedDate"]).ToString("MM-dd")
                                });
                            }
                            web.AllowUnsafeUpdates = false;
                        }
                        spSite.AllowUnsafeUpdates = false;
                    }
                });
                return Util.WriteJsonpToResponse(ResponseStatus.Success, AnnouncementList);
            }
            catch (OptionException)
            {
                return Util.WriteJsonpToResponse(ResponseStatus.Noneffect);
            }
            catch (Exception exception)
            {
                return Util.WriteJsonpToResponse(ResponseStatus.Exception, exception.Message);
            }
        }

        public static string GetAnnouncementDetailed(int listItemId)
        {
            try
            {
                new PortalBase();
                string weburl = SPContext.Current.Web.Url;
                object AnnouncementDetailed = null;
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite spSite = new SPSite(weburl))
                    {
                        spSite.AllowUnsafeUpdates = true;
                        using (SPWeb web = spSite.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            SPListItem listItem = web.Lists[EnvisionPagesConfig.EnvsionAnnouncement].GetItemById(listItemId);
                            AnnouncementDetailed = new
                            {
                                id = listItem.ID,
                                title = listItem.Title,
                                publishedDate = listItem["PublishedDate"] == null ? "" : IBUtils.ObjectToDateTime(listItem["PublishedDate"]).ToString("yyyy-MM-dd"),
                                contentBox = IBUtils.ObjectToStr(listItem["ContentBox"])
                            };
                            web.AllowUnsafeUpdates = false;
                        }
                        spSite.AllowUnsafeUpdates = false;
                    }
                });
                return Util.WriteJsonpToResponse(ResponseStatus.Success, AnnouncementDetailed);
            }
            catch (OptionException)
            {
                return Util.WriteJsonpToResponse(ResponseStatus.Noneffect);
            }
            catch (Exception exception)
            {
                return Util.WriteJsonpToResponse(ResponseStatus.Exception, exception.Message);
            }
        }

        private static SPListItemCollection GetAnnouncementListItems(SPWeb web, string listTitle)
        {
            string viewFields = @"<FieldRef Name='Title' />
                                    <FieldRef Name='PublishedDate' />";
            string orderBy = @"<FieldRef Name='IsTop' Ascending='False'/>
                                     <FieldRef Name='PublishedDate' Ascending='False'/>";
            return GetListItems(web, listTitle, null, viewFields, "", orderBy, 10);
        }
    }
}
