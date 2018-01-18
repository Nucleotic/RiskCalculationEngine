using System;
using System.Collections.Generic;
using System.Linq;
using Nucleotic.Common.Extensions;
using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.DataContracts.CalculationEngine.Model.Services;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;

namespace Nucleotic.Module.CalculationEngine.Assemble.Helpers
{
    public static class ModelMappingHelpers
    {
        /// <summary>
        /// Maps the result from context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="result">The result.</param>
        public static void MapResultFromContext(LoadingsContext context, CalculationResponse result)
        {
            result.BaseRate = context.BaseRate;
            result.CalculatedRate = context.CalculatedRate;
            result.CalculatedAnnualPremium = decimal.Round(context.CalculatedAnnualPremium, 2);
            result.CalculatedMonthlyPremium = decimal.Round(context.CalculatedMonthlyPremium, 2);
            result.CalculationDate = context.CalculationDate;
            result.IsDeclinedReferred = context.IsDeclinedReferred;
            result.DeclinedReferredReason = context.DeclinedReferredReason;
            result.LoadedFactor = context.LoadedFactor;
            result.Loadings = context.Loadings.OrderBy(l => l.LoadingName);
            result.Messages = from error in context.Errors select new ExceptionMessage{ Message = error.Message };
            result.DiscretionaryFloorAmountAnnual = decimal.Round(context.DiscretionaryFloorAmountAnnual, 2);
            result.DiscretionaryFloorAmountMonthly = decimal.Round(context.DiscretionaryFloorAmountMonthly, 2);
        }

        /// <summary>
        /// Maps the result from context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="result">The result.</param>
        public static void MapResultFromContext(ExcessWaiverContext context, ExcessWaiverCalculationResponse result)
        {
            result.MonthlyBasicWaiverPremium = decimal.Round(context.BasicMonthlyWaiverPremium, 2);
            result.AnnualisedBasicWaiverPremium = decimal.Round(context.BasicAnnualisedWaiverPremium, 2);
            result.MonthlyTotalLossWaiverPremium = decimal.Round(context.TotalLossMonthlyWaiverPremium, 2);
            result.AnnualisedTotalLossWaiverPremium = decimal.Round(context.TotalLossAnnualisedWaiverPremium, 2);
            result.CalculationDate = context.CalculationDate;
            result.IsDeclinedReferred = context.IsDeclinedReferred;
            result.DeclinedReferredReason = context.DeclinedReferredReason;
            result.Loadings = context.Loadings.OrderBy(l => l.LoadingName);
            result.ExcessAmount = decimal.Round(context.ExcessAmount);
            result.Messages = from error in context.Errors select new ExceptionMessage { Message = error.Message };
        }

        /// <summary>
        /// Maps the motor ratings context.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="request">The request.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public static LoadingsContext MapMotorLoadingsContext(int version, VehicleCalculationRequest request, string connectionString = null)
        {
            RiskCoverType coverType;
            try { coverType = (RiskCoverType)Enum.Parse(typeof(RiskCoverType), request.RiskItemCoverType.Replace("&", "And").Replace(" ", "").Replace(",", "")); }
            catch { coverType = RiskCoverType.Unknown; }
            var claimItems = ParseClaims(request.Claims);
            var enumerable = request.VehicleDetails.SecurityFeatures != null ? request.VehicleDetails.SecurityFeatures as IList<string> ?? request.VehicleDetails.SecurityFeatures.ToList(): new List<string>();
            var features = enumerable.Select(feature => (SecurityFeatures)Enum.Parse(typeof(SecurityFeatures), feature.Replace("Factoryimmobiliser", "FactoryImmobiliser"))).ToList();
            var vType = (VehicleType)Enum.Parse(typeof(VehicleType), request.VehicleDetails.VehicleType.Replace("4x4", "FourByFourVehicle").Replace(" ", ""));
            var usage = !string.IsNullOrEmpty(request.VehicleDetails.VehicleUse) ? (ScaleOfUse)Enum.Parse(typeof(ScaleOfUse), request.VehicleDetails.VehicleUse.Replace(" ", "")) : ScaleOfUse.PrivateAndBusiness;
            var productType = !string.IsNullOrEmpty(request.ProductType) ? (ProductType) Enum.Parse(typeof(ProductType), request.ProductType) : ProductType.Domestic;
            var context = new LoadingsContext(version)
            {
                EngineType = EngineType.MotorLoadings,
                BaseCalculationsDone = false,
                PolicyRiskItemRatingType = RiskItemRatingType.MotorVehicle,
                BasicSumInsured = request.BasicSumInsured,
                PolicyNumber = request.PolicyNumber,
                RiskItemId = request.RiskItemId,
                RiskItemCoverType = coverType,
                PostalCode = request.PostalCode,
                HasSpecializedShortfallCover = request.HasCreditShortfallCover,
                Broker = new BrokerDetails { BrokerId = request.BrokerId },
                Claims = claimItems,
                ActionedByUserName = Environment.UserName, //TODO: Update to actual caller of application
                CallingContextId = Guid.NewGuid(),
                Age = request.VehicleDetails.DriverAge,
                AssetDetails = new VehicleDetails
                {
                    RiskItemCoverType = coverType,
                    IsHighPerformance = request.VehicleDetails.IsHighPerformance,
                    Make = request.VehicleDetails.Make,
                    Model = request.VehicleDetails.Model,
                    SecurityFeatures = features,
                    VehicleUse = usage,
                    VehicleType = vType,
                    Year = request.VehicleDetails.Year,
                    DriverAge = request.VehicleDetails.DriverAge,
                    LoadedFromPreviousCalculation = false
                },
                AdditionalExcess = request.AdditionalExcess,
                PolicyProductType = productType
            };
            context.ClaimsCount = context.Claims.Count();
            MapConnectionString(context, connectionString);
            return context;
        }

        /// <summary>
        /// Maps the building loadings context.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="request">The request.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public static LoadingsContext MapBuildingLoadingsContext(int version, BuildingCalculationRequest request, string connectionString = null)
        {
            var context = SetupBasicContext(version, request, EngineType.BuildingLoadings, RiskItemRatingType.Buildings, connectionString);
            RiskCoverType coverType;
            try { coverType = (RiskCoverType)Enum.Parse(typeof(RiskCoverType), request.RiskItemCoverType.Replace("&", "And").Replace(" ", "").Replace(",", "")); }
            catch { coverType = RiskCoverType.Unknown; }
            var features = request.BuildingDetails.SecurityFeatures != null && request.BuildingDetails.SecurityFeatures.Any()
                ? request.BuildingDetails.SecurityFeatures.Select(f => (SecurityFeatures)Enum.Parse(typeof(SecurityFeatures), f)).ToList()
                : null;
            context.PostalCode = request.PostalCode;
            context.HasSpecializedShortfallCover = request.HasSubsidenceCover;
            context.AssetDetails = new BuildingDetails
            {
                RiskItemCoverType = coverType,
                SecurityFeatures = features,
                ProductType = request.BuildingDetails.ProductType,
                CoverAdditions = request.HasSubsidenceCover ? new List<string> {"SubsidenceLandslipCover"} : null,
                WallType = !string.IsNullOrEmpty(request.BuildingDetails.WallType) ? request.BuildingDetails.WallType : null,
                BuildingType = !string.IsNullOrEmpty(request.BuildingDetails.BuildingType) ? request.BuildingDetails.BuildingType : "None",
                BuildingConstructionType = request.BuildingDetails.BuildingConstructionType,
                RoofType = !string.IsNullOrEmpty(request.BuildingDetails.RoofType) ? request.BuildingDetails.RoofType : null,
                OccupancyType = request.BuildingDetails.OccupancyType,
                LoadedFromPreviousCalculation = false
            };
            context.AdditionalExcess = request.AdditionalExcess ?? default(decimal);
            return context;
        }

        /// <summary>
        /// Maps the contents loadings context.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="request">The request.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public static LoadingsContext MapContentsLoadingsContext(int version, ContentsCalculationRequest request, string connectionString = null)
        {
            var context = SetupBasicContext(version, request, EngineType.ContentLoadings, RiskItemRatingType.HouseholdContents, connectionString);
            RiskCoverType coverType;
            try { coverType = (RiskCoverType)Enum.Parse(typeof(RiskCoverType), request.RiskItemCoverType.Replace("&", "And").Replace(" ", "").Replace(",", "")); }
            catch { coverType = RiskCoverType.Unknown; }
            var features = request.BuildingDetails.SecurityFeatures != null && request.BuildingDetails.SecurityFeatures.Any()
                ? request.BuildingDetails.SecurityFeatures.Select(f => (SecurityFeatures)Enum.Parse(typeof(SecurityFeatures), f)).ToList()
                : null;
            context.PostalCode = request.PostalCode;
            context.AssetDetails = new BuildingDetails
            {
                RiskItemCoverType = coverType,
                SecurityFeatures = features,
                ProductType = request.BuildingDetails.ProductType,
                WallType = !string.IsNullOrEmpty(request.BuildingDetails.WallType) ? request.BuildingDetails.WallType : null,
                BuildingType = !string.IsNullOrEmpty(request.BuildingDetails.BuildingType) ? request.BuildingDetails.BuildingType : "None",
                BuildingConstructionType = request.BuildingDetails.BuildingConstructionType,
                RoofType = !string.IsNullOrEmpty(request.BuildingDetails.RoofType) ? request.BuildingDetails.RoofType : null,
                OccupancyType = request.BuildingDetails.OccupancyType,
                LoadedFromPreviousCalculation = false
            };
            return context;
        }

        /// <summary>
        /// Mas all risk loadings context.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="request">The request.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public static LoadingsContext MapAllRiskLoadingsContext(int version, AllRiskCalculationRequest request, string connectionString = null)
        {
            var riskItemType = !string.IsNullOrEmpty(request.RiskItemType)
                ? (AllRiskType) Enum.Parse(typeof(AllRiskType), request.RiskItemType.Replace("and", "And").Replace("&", "And").Replace("in a", "").Replace(" ", ""))
                : AllRiskType.Unspecified;
            var context = SetupBasicContext(version, request, EngineType.AllRiskLoadings, RiskItemRatingType.AllRisk, connectionString);
            context.AssetDetails = new AllRiskDetails { RiskItemType = riskItemType };
            return context;
        }

        /// <summary>
        /// Maps the flat rate context.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="request">The request.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public static LoadingsContext MapFlatRateLoadingsContext(int version, FlatRateCalculationRequest request, string connectionString = null)
        {
            var coverType = (RiskCoverType)Enum.Parse(typeof(RiskCoverType), request.RiskItemCoverType.Replace("&", "And").Replace(" ", "").Replace(",", ""));
            var calcType = (FlatRateCalculationTypes)Enum.Parse(typeof(FlatRateCalculationTypes), request.FlatRateCalculationType);
            var productType = !string.IsNullOrEmpty(request.ProductType) ? (ProductType)Enum.Parse(typeof(ProductType), request.ProductType) : ProductType.Domestic;
            var context = new LoadingsContext(version)
            {
                EngineType = EngineType.FlatRateLoadings,
                BaseCalculationsDone = false,
                PolicyRiskItemRatingType = RiskItemRatingType.FlatRate,
                BasicSumInsured = request.BasicSumInsured,
                PolicyNumber = request.PolicyNumber,
                RiskItemId = request.RiskItemId,
                RiskItemCoverType = coverType,
                ActionedByUserName = Environment.UserName, //TODO: Update to actual caller of application
                CallingContextId = Guid.NewGuid(),
                FlatRateCalculationType = calcType,
                ProductCount = request.ProductCount,
                PolicyProductType = productType,
                ProductName = !string.IsNullOrEmpty(request.ProductName) ? request.ProductName.Trim().Replace(' ', '\n') : request.ProductName
            };
            MapConnectionString(context, connectionString);
            return context;
        }

        /// <summary>
        /// Setups the basic context.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="request">The request.</param>
        /// <param name="engineType">Type of the engine.</param>
        /// <param name="ratingType">Type of the rating.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        private static LoadingsContext SetupBasicContext(int version, BaseRequest request, EngineType engineType, RiskItemRatingType ratingType, string connectionString = null)
        {
            RiskCoverType coverType;
            try { coverType = (RiskCoverType)Enum.Parse(typeof(RiskCoverType), request.RiskItemCoverType.Replace("&", "And").Replace(" ", "").Replace(",", "")); }
            catch { coverType = RiskCoverType.Unknown; }
            var productType = !string.IsNullOrEmpty(request.ProductType) ? (ProductType)Enum.Parse(typeof(ProductType), request.ProductType) : ProductType.Domestic;
            var context = new LoadingsContext(version)
            {
                EngineType = engineType,
                BaseCalculationsDone = false,
                PolicyRiskItemRatingType = ratingType,
                BasicSumInsured = request.BasicSumInsured,
                PolicyNumber = request.PolicyNumber,
                RiskItemId = request.RiskItemId,
                RiskItemCoverType = coverType,
                Broker = new BrokerDetails { BrokerId = request.BrokerId },
                Claims = ParseClaims(request.Claims),
                ActionedByUserName = Environment.UserName, //TODO: Update to actual caller of application
                CallingContextId = Guid.NewGuid(),
                PolicyProductType = productType
            };
            context.ClaimsCount = context.Claims.Count();
            MapConnectionString(context, connectionString);
            return context;
        }

        /// <summary>
        /// Maps the excess waiver context.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="policyNumber">The policy number.</param>
        /// <param name="baseCover">The base cover.</param>
        /// <param name="riskItemId">The risk item identifier.</param>
        /// <param name="riskitemFactor">The risk item factor.</param>
        /// <param name="ratingType">Type of the rating.</param>
        /// <param name="riskType">Type of the risk.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public static ExcessWaiverContext MapExcessWaiverContext(int version, string policyNumber, decimal baseCover, int riskItemId, int? riskitemFactor, string ratingType, string riskType, string connectionString = null)
        {
            var type = (RiskItemRatingType)Enum.Parse(typeof(RiskItemRatingType), ratingType);
            var context = new ExcessWaiverContext(version)
            {
                EngineType = EngineType.ExcessWaivers,
                BaseCalculationsDone = false,
                PolicyRiskItemRatingType = type,
                BasicSumInsured = baseCover,
                PolicyNumber = policyNumber,
                RiskItemId = riskItemId,
                PolicyAge = type == RiskItemRatingType.MotorVehicle && riskitemFactor.HasValue ? riskitemFactor.Value : 0,
                RiskAddressCount = type != RiskItemRatingType.MotorVehicle && riskitemFactor.HasValue ? riskitemFactor.Value : 0,
                AllRiskType = !string.IsNullOrEmpty(riskType) ? (AllRiskType)Enum.Parse(typeof(AllRiskType), riskType) : AllRiskType.Unspecified
            };
            MapConnectionString(context, connectionString);
            return context;
        }

        /// <summary>
        /// Maps the connection string.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="connectionString">The connection string.</param>
        private static void MapConnectionString(BaseContext context, string connectionString = null)
        {
            if (string.IsNullOrEmpty(connectionString)) return;
            const string baseConnectionString = @"metadata=res://*/Model.RatingsDataModel.csdl|res://*/Model.RatingsDataModel.ssdl|res://*/Model.RatingsDataModel.msl;provider=System.Data.SqlClient;provider connection string='{0};MultipleActiveResultSets=True;App=EntityFramework'";
            context.EntityConnectionString = string.Format(baseConnectionString, connectionString);
        }

        /// <summary>
        /// Parses the claims.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <returns></returns>
        private static IEnumerable<ClaimItem> ParseClaims(IEnumerable<ClaimItemRequest> claims)
        {
            var claimItems = new List<ClaimItem>();
            if (claims == null) return claimItems;
            var claimItemRequests = claims as ClaimItemRequest[] ?? claims.ToArray();
            if (claimItemRequests.Any())
            {
                claimItems.AddRange(
                    from c in claimItemRequests
                    select new ClaimItem
                    {
                        ClaimNumber = c.ClaimNumber,
                        ClaimType = ParseClaimType(c),
                        DateOfLoss = c.DateOfLoss,
                        ClaimAmount = c.ClaimAmount,
                        RiskItemDescription = c.RiskItemDescription
                    });
            }
            return claimItems;
        }

        /// <summary>
        /// Parses the type of the claim.
        /// </summary>
        /// <param name="claimItem">The claimItem.</param>
        /// <returns></returns>
        private static ClaimType ParseClaimType(ClaimItemRequest claimItem)
        {
            ClaimType claimType;
            try
            {
                claimType = (ClaimType)Enum.Parse(typeof(ClaimType), claimItem.ClaimType.Replace("3rd", "Third").ReplaceAll(new[] { ',', '\'', ' ' }, '\n'));
            }
            catch (Exception)
            {
                claimType = ClaimType.Unknown;
            }
            return claimType;
        }
    }
}
