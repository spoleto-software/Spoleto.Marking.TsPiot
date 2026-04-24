using Spoleto.Common.Helpers;
using Spoleto.Marking.TsPiot.Exceptions;
using Spoleto.Marking.TsPiot.Models;
using Spoleto.Marking.TsPiot.Options;

namespace Spoleto.Marking.TsPiot.Extensions
{
    internal static class ModelExtensions
    {
        public static ClientInfo ToRestClientInfo(this TsPiotClientAppOptions appOptions)
        {
            return new()
            {
                Id = appOptions.Id,
                Name = appOptions.Name,
                Token = appOptions.Token,
                Version = appOptions.Version
            };
        }

        public static Grpc.ClientInfo ToGrpcClientInfo(this TsPiotClientAppOptions appOptions)
            => new()
            {
                Id = appOptions.Id,
                Name = appOptions.Name,
                Token = appOptions.Token,
                Version = appOptions.Version
            };

        public static CodesCheckResult ToDto(this Grpc.CodesCheckResult codesCheckResult)
            => new()
            {
                CodesResponse = codesCheckResult.CodesResponse.ToDto(),
                Error = codesCheckResult.Error.ToDto()
            };

        public static CodesResponse ToDto(this Grpc.CodesResponse codesResponse)
            => new()
            {
                CodeResponses = codesResponse.CodesResponse_.Select(x => x.ToDto()).ToList()
            };

        public static CodeResponse ToDto(this Grpc.CodeResponse codeResponse)
            => new()
            {
                Code = codeResponse.Code,
                Description = codeResponse.Desctiption,
                IsCheckedOffline = codeResponse.IsCheckedOffline,
                ReqId = codeResponse.ReqId,
                ReqTimestamp = codeResponse.ReqTimestamp,
                Codes = codeResponse.Codes.Select(x => x.ToDto()).ToList()
            };

        public static CodeInfo ToDto(this Grpc.Codes codes)
            => new()
            {
                Cis = codes.Cis,
                EliminationState = codes.EliminationState,
                ErrorCode = codes.ErrorCode,
                ErrorMessage = codes.Message,
                ExpireDate = codes.ExpireDate,
                FactorySerialNumber = codes.FactorySerialNumber,
                Found = codes.Found,
                GrayZone = codes.GrayZone,
                GroupIds = codes.GroupIds.ToList(),
                Gtin = codes.Gtin,
                InnerUnitCount = codes.InnerUnitCount,
                //Inst= codes.inner
                IsBlocked = codes.IsBlocked,
                IsOwner = codes.IsOwner,
                IsTracking = codes.IsTracking,
                Mrp = codes.Mrp,
                Ogvs = codes.Ogvs.ToList(),
                PackageQuantity = codes.PackageQuantity,
                PackageType = codes.PackageType,
                Parent = codes.Parent,
                PrintView = codes.PrintView,
                ProducerInn = codes.ProducerInn,
                ProductionBatchNumber = codes.ProductionBatchNubmer,
                ProductionDate = codes.ProductionDate,
                ProductionSerialNumber = codes.ProductionSerialNumber,
                ProductWeight = Double.TryParse(codes.ProductWeight, out var d) ? d : null,
                PrVetDocument = codes.PrVetDocument,
                Realizable = codes.Realizable,
                Smp = codes.Smp,
                Sold = codes.Sold,
                SoldUnitCount = codes.SoldUnitCount,
                Utilised = codes.Utilised,
                Valid = codes.Valid,
                VariableExpirations = codes.VariableExpirations,
                Verified = codes.Verified,
                //Version = codes.vers
            };

        public static Error ToDto(this Grpc.Error error)
            => new()
            {
                ErrorCode = (int)error.ErrorCode,
                ErrorDescription = error.ErrorDescription
            };


        public static TsPiotInfo ToDto(this Grpc.TsPiotInfo info)
            => new()
            {
                CodesCheckTimeout = info.CodesCheckTimeout,
                FnSerial = info.FnSerial,
                KktInn = info.KktInn,
                KktSerial = info.KktSerial,
                TsPiotId = info.TsPiotId
            };

        public static SimpleVerificationResult AsSimpleResult(this CodesCheckResult codesCheckResult)
        {
            var result = codesCheckResult.CodesResponse?.CodeResponses?.FirstOrDefault();
            if (result == null)
            {
                throw new TsPiotException("Ошибка проверки. Результат не был получен");
            }

            var simpleResult = new SimpleVerificationResult();
            simpleResult.IsCheckedOffline = result.IsCheckedOffline;
            simpleResult.IsEmergencyMode = codesCheckResult.IsEmergencyMode;
            simpleResult.ReqTimestamp = (long)result.ReqTimestamp;
            simpleResult.ReqId = result.ReqId;
            simpleResult.VerificationResultItems = new List<SimpleVerificationResultItem>();
            foreach (var item in result.Codes)
            {
                var simpleItem = item.AsSimpleResult(result);
                simpleResult.VerificationResultItems.Add(simpleItem);
            }
            return simpleResult;
        }

        private static SimpleVerificationResultItem AsSimpleResult(this CodeInfo codeInfo, CodeResponse codeResponse) => new()
        {
            MarkingCode = codeInfo.Cis,
            Success = codeResponse.IsCheckedOffline
            ? !codeInfo.IsBlocked && codeResponse.Code == 0
            : codeInfo.Found && codeInfo.Utilised && codeInfo.Verified && !codeInfo.Sold && !codeInfo.IsBlocked && codeInfo.Realizable,
            Message = codeInfo.ErrorMessage,
            Inst = codeInfo.Inst,
            Version = codeInfo.Version,
            AdditionalInfo = JsonHelper.ToJson(codeInfo),
        };
    }
}
