using Abp.Application.Services.Dto;
using Abp.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Pbt.Individual;
using Pbt.Individual.Application.DeliveryRequests.Dto;
using Pbt.Individual.ApplicationUtils;
using Pbt.Individual.Authorization;
using Pbt.Individual.Core;
using Pbt.Individual.DeliveryRequests.Dto;
using PBT.CacheService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions;

namespace pbt.DeliveryRequests
{
    [Authorize]
    [AbpAuthorize]
    public class DeliveryRequestAppService : IndividualAppServiceBase, IDeliveryRequestAppService

    {
        private readonly TelegramBotClient _botClient;
        private readonly string _telegramChannelId; // Thay bằng ID kênh Telegram của bạn
        private readonly IConfiguration _configuration;

        public DeliveryRequestAppService(AppCacheService cacheService, IConfiguration configuration) : base(cacheService)
        {
            _configuration = configuration;
            var teleToken = _configuration.GetValue<string>("NotificationSettings:TelegramBot:BotToken");
            _telegramChannelId = _configuration.GetValue<string>("NotificationSettings:TelegramBot:NotifyChannelId");
            _botClient = new TelegramBotClient(teleToken);
        }

        [HttpGet]
        public async Task<DeliveryRequestDto> GetByIdAsync(int id)
        {
            var prs = new[]
            {
                new SqlParameter("@Id", id)
            };

            //SP_DeliveryRequests_GetDetailById 

            var data = await ConnectDb.GetItemAsync<DeliveryRequestDto>(
                "SP_DeliveryRequests_GetDetailById",
                System.Data.CommandType.StoredProcedure,
                prs
            );

            return data;
        }
    

        [HttpPost]
        public async Task<DeliveryRequestDto> CreateAsync()
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error("Error creating delivery request", ex);
                throw new Exception("Error creating delivery request", ex);
            }
        }



        [HttpGet]
        public async Task<PagedResultDto<DeliveryRequestDto>> GetPagedAsync(
            PagedDeliveryRequestsResultRequestDto input)
        {
            var currentUser = await GetCurrentUserAsync();

            if(currentUser is null || !currentUser.CustomerId.HasValue)
            {
                throw new AbpAuthorizationException("You are not authorized to access delivery requests.");
            }   

            var customerIdsString = currentUser.CustomerId.Value.ToString();

            var totalCountPr = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            var prs = new[]
            {
                new SqlParameter("@PermissionCase", 4),
                new SqlParameter("@CustomerIds", customerIdsString),
                new SqlParameter("@CustomerId",   currentUser.CustomerId.Value),
                new SqlParameter("@WarehouseId",  -1),
                new SqlParameter("@FromCreationTime", input.StartDate ?? (object)DBNull.Value),
                new SqlParameter("@ToCreationTime", input.EndDate ?? (object)DBNull.Value),
                new SqlParameter("@Keyword",  input.Code ),
                new SqlParameter("@Status", input.Status ),
                new SqlParameter("@SkipCount", input.SkipCount),
                new SqlParameter("@MaxResultCount", input.MaxResultCount),
                totalCountPr
            };

            var data = await ConnectDb.GetListAsync<DeliveryRequestDto>(
                "SP_DeliveryRequests_GetPaged",
                System.Data.CommandType.StoredProcedure,
                prs
            );
            var totalCount = (int)totalCountPr.Value;

            return new PagedResultDto<DeliveryRequestDto>()
            {
                Items = data,
                TotalCount = totalCount,
            };
        }

        [HttpPost]
        public async Task<JsonResult> AddItemAsync(DeliveryRequestItemDto item)
        {

            var statusCodeOutputParam = new SqlParameter
            {
                ParameterName = "@StatusCode",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            var messageOutputParam = new SqlParameter
            {
                ParameterName = "@Message",
                SqlDbType = System.Data.SqlDbType.NVarChar,
                Size = -1,
                Direction = System.Data.ParameterDirection.Output
            };
            await ConnectDb.ExecuteNonQueryAsync("SP_DeliveryRequests_AddItem", System.Data.CommandType.StoredProcedure,
                new[]{
                    new SqlParameter(){ ParameterName = "@DeliveryRequestId", Value = item.DeliveryRequestId , SqlDbType = SqlDbType.Int },
                    new SqlParameter(){ ParameterName = "@ItemId", Value = item.ItemId , SqlDbType = SqlDbType.Int },
                    new SqlParameter(){ ParameterName = "@ItemType", Value = item.ItemType , SqlDbType = SqlDbType.Int },
                    new SqlParameter(){ ParameterName = "@CreatorUserId", Value = AbpSession.UserId ?? (object)DBNull.Value , SqlDbType = SqlDbType.BigInt },
                    statusCodeOutputParam,
                    messageOutputParam
                }
            );

            var statusCode = (int)statusCodeOutputParam.Value;
            var message = messageOutputParam.Value.ToString();
            if (statusCode <= 0)
            {
                return new JsonResult(new { success = false, message = message });
            }
            return new JsonResult(new { success = true, message = "Thêm vật phẩm vào yêu cầu giao hàng thành công." });
        }

        [HttpDelete]
        public async Task<JsonResult> DeleteItemAsync(int deliveryRequestItemId)
        {
            await ConnectDb.ExecuteNonQueryAsync("SP_DeliveryRequests_RemoveItem", System.Data.CommandType.StoredProcedure,
                new[]{
                    new SqlParameter(){ ParameterName = "@DeliveryRequestItemId", Value = deliveryRequestItemId , SqlDbType = SqlDbType.Int }
                }
            );

            return new JsonResult(new { success = true, message = "Đã xóa khỏi yêu cầu giao" });
        }

        [HttpPost]
        public async Task<JsonResult> SubmitAsync(SubmitDeliveryRequestDto input)
        {
            var statusCodeOutputParam = new SqlParameter
            {
                ParameterName = "@StatusCode",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            var messageOutputParam = new SqlParameter
            {
                ParameterName = "@Message",
                SqlDbType = System.Data.SqlDbType.NVarChar,
                Size = -1,
                Direction = System.Data.ParameterDirection.Output
            };

            await ConnectDb.ExecuteNonQueryAsync("SP_DeliveryRequests_SubmitRequest", System.Data.CommandType.StoredProcedure,
                new[]{
                    new SqlParameter(){ ParameterName = "@Id", Value = input.Id , SqlDbType = SqlDbType.Int },
                    new SqlParameter(){ ParameterName = "@ShippingMethod", Value = input.ShippingMethod , SqlDbType = SqlDbType.Int },
                    new SqlParameter(){ ParameterName = "@Address", Value = input.Address , SqlDbType = SqlDbType.NVarChar },
                    new SqlParameter(){ ParameterName = "@Note", Value = input.Note ?? (object)DBNull.Value , SqlDbType = SqlDbType.NVarChar },
                    new SqlParameter(){ ParameterName = "@SubmitUserId", Value = AbpSession.UserId ?? (object)DBNull.Value , SqlDbType = SqlDbType.BigInt },
                    new SqlParameter(){ ParameterName = "@Status", Value = DeliveryRequestStatus.Submited , SqlDbType = SqlDbType.Int },
                    new SqlParameter(){ ParameterName = "@BagStatus", Value = (int) PackageShippingStatusEnum.DeliveryRequest , SqlDbType = SqlDbType.Int },
                    new SqlParameter(){ ParameterName = "@PackageStatus", Value = (int) PackageShippingStatusEnum.DeliveryRequest , SqlDbType = SqlDbType.Int },
                    statusCodeOutputParam,
                    messageOutputParam
                }
            );

            var statusCode = (int)statusCodeOutputParam.Value;
            var message = messageOutputParam.Value.ToString();
            if (statusCode <= 0)
            {
                return new JsonResult(new { success = false, message = message });
            }

            // Send notify to Telegram channel

            await SendDeliveryRequestNotificationAsync(input.Id);
            return new JsonResult(new { success = true, message = "Gửi yêu cầu giao hàng thành công." });
        }


        [HttpGet]
        public async Task<List<DeliveryRequestItemDto>> GetItemsByRequestIdAsync(int deliveryRequestId)
        {
            var pr = new[]
                {
                    new SqlParameter(){ ParameterName = "@RequestId", Value = deliveryRequestId, SqlDbType = SqlDbType.Int }
                };

            return await ConnectDb.GetListAsync<DeliveryRequestItemDto>(
                "SP_DeliveryRequestItems_GetByRequestId",
                System.Data.CommandType.StoredProcedure,
               pr
            );
        }


        private async Task SendDeliveryRequestNotificationAsync(int id)
        {
            var items = await GetItemsByRequestIdAsync(id);
            var title = $"Yêu cầu giao hàng mới với mã yêu cầu {id} vừa được tạo. ";

            var checkListMessage = new List<string>();
            foreach (DeliveryRequestItemDto item in items)
            {
                checkListMessage.Add($"- {((DeliveryNoteItemType)item.ItemType).GetDescription()}: {(item.ItemType == (int)DeliveryNoteItemType.Package ? item.PackageCode : item.BagNumber)}");
            }

            var msg = "\nChi tiết:\n" + string.Join("\n", checkListMessage);


            if (!string.IsNullOrEmpty(_telegramChannelId))
            {
                try
                {
                    await _botClient.SendHtml(
                        chatId: _telegramChannelId, // Hoặc ID số nếu là kênh riêng tư
                        html: $"<b style='font-size:20px;color:red;'>{title}</b>\n {msg}"
                    );
                    await Task.CompletedTask;
                }
                catch (Exception ex)
                {
                    Logger.Warn("Failed to send delivery request notification to Telegram.", ex);
                }
            }
        }


    }
}